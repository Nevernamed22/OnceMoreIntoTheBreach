#!/usr/bin/python
#Script for generating WWise sound banks for Enter the Gungeon modding
#  Run with -h flag for usage information
#  Note: volume is measured in dB gain, not percentage

ALLOW_AUTORUN = True # change to False if you don't wish to allow this script to autogenerate sound files when ran without arguments

#References:
#  - BNK File Format: https://wiki.xentax.com/index.php/Wwise_SoundBank_(*.bnk)
#    - Backup Link:   https://web.archive.org/web/20230817173834/http://wiki.xentax.com/index.php/Wwise_SoundBank_(*.bnk)
#  - WEM File Format: https://github.com/WolvenKit/wwise-audio-tools/blob/master/ksy/wem.ksy
#  - In dumped SFX.bnk.xml, Actor Mixer 189241348 references all music and probably controls volume (<fld ty="sid" na="ulID" va="189241348"/>)
#     - 3649037401 = STOP_MUS_ALL
#     - 1075162602 = Play_MUS_Boss_Theme_Beholster
#  - Extracting OGG encoded files (like the Boss theme):
#     - Download and build ww2ogg https://github.com/hcs64/ww2ogg
#     - Extract the WEM file using this script, then run ./ww2ogg beholster.wem -o beholster.bin.ogg --mod-packets --pcb packed_codebooks_aoTuV_603.bin

'''
    354   : { Play_MUS_Boss_Theme_Beholster data
      extra_bytes          : 48
      wem_header           : b'RIFF'
      wem_length           : 1512170
      wem_wave             : b'WAVE'
      fmt_header           : b'fmt '
      fmt_size             : 66
      [auto-1]             : 0
      compression_code     : -1
      channels             : 2
      sample_rate          : 44100
      avg_byte_rate        : 10458
      block_align          : 0
      sample_width         : 0
      valid_bits           : 12546
      [auto-2]             : [array of 42 bytes]
      akd_header           : None
      junk_header          : None
      data_header          : b'data'
      data_chunk_size      : 1512084
      wav_data             : [array of 1512084 bytes]
    }

    ww2ogg output:
      Input: beholster.wem
        RIFF WAVE 2 channels 44100 Hz 83664 bps
        6350400 samples
        - 2 byte packet headers, no granule
        - stripped setup header
        - external codebooks (packed_codebooks_aoTuV_603.bin)
        - modified Vorbis packets
        Output: beholster.bin.ogg
        Done!
'''

#Todo:
#  - figure out pausing music on pause screen
#  - figure out looping music
#  - figure out why 8-bit WAVs make Gungeon explode (e.g., minish cap sounds)
#  -
#  - add checking for duplicate IDs (in case strings hash to same thing)
#  - (maybe) add support for different HIRC actions
#  - (maybe) UI channels
#  - (maybe) figure out why we have to pretend mono tracks are stereo
#  - (maybe) finish up support for reversing .bnk to .wem / .wav files

SCRIPT_DESCRIPTION = "create a WWise soundbank (.bnk) compatibile with Enter the Gungeon"

# Import necessary modules
import sys, os, struct, io, wave, csv, argparse, time
# import numpy as np
# from soundfile import SoundFile

# Install pyaudio and and uncomment below line to use playWAVData()
#   (tested with Python 3.9, may not work with Python 3.10 and up)
# import pyaudio

#Pre-argparse hack for obeying color information
if "--nocolor" not in sys.argv:
  class col: #ANSI colors
    BLN = '\033[0m'    # Blank
    INV = '\033[1;7m'  # Inverted
    CRT = '\033[1;41m' # Critical
    BLK = '\033[1;30m' # Black
    RED = '\033[1;31m' # Red
    GRN = '\033[1;32m' # Green
    YLW = '\033[1;33m' # Yellow
    BLU = '\033[1;34m' # Blue
    MGN = '\033[1;35m' # Magenta
    CYN = '\033[1;36m' # Cyan
    WHT = '\033[1;37m' # White
else: #no colors
  class col:
    BLN = ''
    UND = ''
    INV = ''
    CRT = ''
    BLK = ''
    RED = ''
    GRN = ''
    YLW = ''
    BLU = ''
    MGN = ''
    CYN = ''
    WHT = ''

# Create argument parser and parse the args
parser = argparse.ArgumentParser()
parser.description = f"{os.path.basename(sys.argv[0])}: {SCRIPT_DESCRIPTION}"
if ALLOW_AUTORUN:
  parser.add_argument("-i", "--input_path",
    help=f"folder containing all of the wav files to be parsed")
  parser.add_argument("-o", "--output_bank_name",
    help=f"name of output bank; puts in {col.YLW}input_path{col.BLN} unless absolute path is given")
else:
  parser.add_argument("input_path",
    help=f"folder containing all of the wav files to be parsed")
  parser.add_argument("output_bank_name",
    help=f"name of output bank; puts in {col.YLW}input_path{col.BLN} unless absolute path is given")
parser.add_argument("-v", "--verbose",   action="store_true",
  help=f"print verbose information")
parser.add_argument("-q", "--quiet",   action="store_true",
  help=f"hide warnings, only show errors")
parser.add_argument("-s", "--spreadsheet",
  help=f"load sound effect information from {col.YLW}spreadsheet{col.BLN}; creates an example spreadsheet if none exists")
parser.add_argument("-r", "--recursive", action="store_true",
  help=f"recursively scan subfolders of {col.YLW}input_path{col.BLN} for .wav files")
parser.add_argument("-w", "--create_wems", action="store_true",
  help=f"create .wem files from .wav files in {col.YLW}input_path{col.BLN}")
parser.add_argument("-O", "--overwrite", action="store_true",
  help=f"overwrite existing .bnk files without confirmation")
parser.add_argument("--nocolor",   action="store_true",
  help=f"({col.BLU}debug{col.BLN}) disable colored output (if terminal doesn't support ANSI codes)")
parser.add_argument("--showparse",   action="store_true",
  help=f"({col.BLU}debug{col.BLN}) show parse information while parsing BNK / WEM data")
parser.add_argument("--dumpparse",   action="store_true",
  help=f"({col.BLU}debug{col.BLN}) dump parse structure after parsing BNK data")
parser.add_argument("--readbank",   action="store_true",
  help=f"({col.BLU}debug{col.BLN}) dump a sound bank to the console (useful for reverse engineering)")
parser.add_argument("--skipchecks",   action="store_true",
  help=f"({col.BLU}debug{col.BLN}) skip sanity checks for parsing bnk files; {col.RED}debug only, can cause crashes{col.BLN}")
args = parser.parse_args()

# Hashing constants for using FNV-1 to transform strings to ids
FNV_32_PRIME = 0x01000193 # decimal: 16777619
FNV_32_INIT  = 0x811c9dc5 # decimal: 2166136261
FNV_32_MOD   = 2**32

# Shared magic ids (i think you can safely always use these exact values for gungeon)
GUNGEON_BUS_ID        = 3803692087 # magic, needs to be shared among sounds
GUNGEON_RTPC_ID_SFX   = 3273357900 # magic, needs to be shared among rtpcs (sound channel)
GUNGEON_RTPC_ID_MUSIC = 2714767868 # magic, needs to be shared among rtpcs (music channel)

# Unknown RTPC_IDs
# 3139137016 # unknown, global volume, very loud
# 766806760  # unknown, global volume, very loud
# 1395692419 # unknown, global volume, very loud

# Global magic ids (applicable for any WWise sound bank)
HIRC_TYPE_SFX    = 2 # HIRC event type for a Sound Effect
HIRC_TYPE_ACTION = 3 # HIRC event type for an Action
HIRC_TYPE_EVENT  = 4 # HIRC event type for an Event

# Misc. debug stuff
DUMP_WAV_FILES = False

#Verbose printing
def vprint(*listargs, **kwargs):
  if args.verbose:
    print(*listargs, **kwargs)

def warn(*listargs, **kwargs):
  if not args.quiet:
    print(*listargs, **kwargs)

#Play raw WEM data (from a Ref) using PyAudio
def playWEMData(wp):
  playWAVData(
    wp["wav_data"].val,
    wp["channels"].val,
    wp["sample_rate"].val,
    wp["sample_width"].val)

#Save raw WAV Data
def saveWAVData(fname,data,channels,rate,samplebits):
  print(samplebits)
  width = samplebits#//8
  with wave.open(fname, "wb") as fout:
      fout.setnchannels(channels)
      fout.setsampwidth(4 if width == 0 else width) # number of bytes
      fout.setframerate(rate)
      fout.writeframes(data)
      # fout.writeframesraw(data)

#Play raw WAV data using PyAudio
def playWAVData(data,channels,rate,samplebits):
  if not "pyaudio" in sys.modules:
    print("pyaudio not loaded, refusing to play audio")
    return

  CHUNK        = 256  # Chunk size for streaming audio data
  BUFFSIZE     = 1024 # Buffer size for streaming audio data

  tmpfile = "/tmp/tmptest.wav"
  width = samplebits//8
  with wave.open(tmpfile, "wb") as fout:
      fout.setnchannels(channels)
      fout.setsampwidth(width) # number of bytes
      fout.setframerate(rate)
      fout.writeframesraw(data)

  with wave.open(tmpfile, "rb") as wf:

    # suppress errors
    devnull = os.open(os.devnull, os.O_WRONLY)
    old_stderr = os.dup(2)
    sys.stderr.flush()
    os.dup2(devnull, 2)
    os.close(devnull)

    # open player
    player = pyaudio.PyAudio()

    # unsuppress errors
    os.dup2(old_stderr, 2)
    os.close(old_stderr)

    # open output stream
    stream = player.open(
        format   = player.get_format_from_width(width),
        channels = channels,
        rate     = rate,
        output   = True)
    # playback loop
    while more := wf.readframes(CHUNK):
      stream.write(more)
    stream.close()
    player.terminate()

#Get .bnk id from a string using a 32-bit FNV1 hash
#Modified from https://github.com/znerol/py-fnvhash/blob/master/fnvhash/__init__.py
#See also
# - https://www.audiokinetic.com/library/edge/?source=SDK&id=_ak_f_n_v_hash_8h_source.html
# - https://www.audiokinetic.com/library/edge/?source=SDK&id=namespace_a_k_1_1_sound_engine_a1aae6ebdec25946fb2897ce0e025366d.html#a1aae6ebdec25946fb2897ce0e025366d
def stringToBnkID(string):
    data = string.lower().encode()
    hval = FNV_32_INIT
    for byte in data:
        hval = (hval * FNV_32_PRIME) % FNV_32_MOD
        hval = hval ^ byte
    return hval

#Check header of file and see if it matches wav signature
def isWaveFile(path):
  try:
    with open(path,'rb') as fin:
      header = fin.read()[:12]
    if not (header[:4] == b'RIFF'):
      return False
    if not (header[8:] == b'WAVE'):
      return False
    return True
  except Exception:
    return False #if anything goes wrong, assume its not a valid wave file

#Scan a directory for valid wav files
def findWavsInDirectory(path,recursive=False):
  wavs_to_parse = []
  for f in sorted(os.listdir(path)):
    p = os.path.join(path,f)
    if os.path.isdir(p) and recursive:
      wavs_to_parse.extend(findWavsInDirectory(p,True))
    if not (p.endswith(".wav") and isWaveFile(p)):
      continue
    wavs_to_parse.append(p)
  return wavs_to_parse

# Wrapper class for passing a bunch of data by reference for clean(er) binary data parsing
class Ref(object):
  def __init__(self,val={},mode="read"):
    self._val         = val
    self.mode         = mode
    self.autokeycount = 0
    self.autoindex    = 0

  def __getitem__(self, key):
    if isinstance(self.val,list):
      return self.val[key]
    if isinstance(self.val,dict):
      if key == "":
        self.autokeycount += 1
        key = f"[auto-{self.autokeycount}]"
      # print(f"GET {key}")
      if not key in self.val:
        self.val[key] = Ref(None,self.mode)
      return self.val[key]

  def __setitem__(self, key, value):
    if isinstance(self.val,list):
      self.val[key] = Ref(value,self.mode)
    elif isinstance(self.val,dict):
      if key == "":
        self.autokeycount += 1
        key = f"[auto-{self.autokeycount}]"
      self.val[key] = Ref(value,self.mode)
    return self.val[key]

  def __add__(self,other):
    return self.val + other

  def __sub__(self,other):
    return self.val - other

  def __mul__(self,other):
    return self.val * other

  def __truediv__(self,other):
    return self.val / other

  def __floordiv__(self,other):
    return self.val // other

  def __int__(self):
    return int(self.val)

  def __eq__(self, other):
    return self.val == other

  #check if internal container contains item without setting it
  def checkKey(self, key):
    return self.val.get(key, None) is not None

  def next(self):
    if self.val is None:
      self.val = []
    if isinstance(self.val,list):
      i = self.autoindex
      self.autoindex += 1
      if i >= len(self.val):
        self.append({})
      return self.val[i]

  def append(self,other):
    if self.val is None:
      self.val = []
    if isinstance(self.val,list):
      v = Ref(other,self.mode)
      self.val.append(v)
      return v

  def dump(self,indent=0):
    newindent = indent+2
    if isinstance(self.val,dict):
      print("{")
      for k,v in self.val.items():
        print(f"""{'':>{newindent}s}{k:20s} : """,end='')
        v.dump(newindent)
      print(f"""{'':>{indent}s}"""+"}")
    elif isinstance(self.val,list):
      print("[")
      for i,v in enumerate(self.val):
        print(f"""{'':>{newindent}s}{i:<5d} : """,end='')
        v.dump(newindent)
      print(f"""{'':>{indent}s}"""+"]")
    elif isinstance(self.val,bytes) and len(self.val) > 4:
      print(f"[array of {len(self.val)} bytes]")
    elif self.val is None:
      print(col.CRT+str(self.val)+col.BLN)
    else:
      print(str(self.val))

  @property
  def val(self):
      return self._val

  @val.setter
  def val(self, value):
      self._val = value
      return self._val

  @val.deleter
  def val(self):
      del self._val

  def setMode(self,mode):
    if mode == "read":
      self.updatefunc = self.set
    elif mode == "write":
      self.updatefunc = self.get
    self.mode = mode
    if isinstance(self.val,dict):
      for k in self.val.keys():
        self[k].setMode(mode)

  #reset counters for self and all children
  def resetState(self):
    self.autokeycount = 0
    self.autoindex = 0

    if isinstance(self.val,dict):
      for k,v in self.val.items():
        v.resetState()
    elif isinstance(self.val,list):
      for i,v in enumerate(self.val):
        v.resetState()

# Class for serializing / deserializing data from / to a hieararchical Ref structure
class Decoder(object):
  def __init__(self,data,iomode="read"):
    self.data      = data
    self.len       = 0 if self.data is None else len(self.data)
    self.iostream  = io.BytesIO(self.data)
    self.indent    = 2
    self.iomode    = iomode
    self.printmode = self.pprint
    self.labelpos  = 20
    self.failed    = False
    self.cmap   = {
      "discard" : col.BLU,
      "good"    : col.GRN,
      "bad"     : col.CRT,
      "mixed"   : col.YLW,
      "any"     : col.WHT,
    }

    self.bytes_read = 0

  def dontPrint(self):
    self.printmode = self.noprint

  def noprint(self,x,outcome,tag=None):
    return

  def pprint(self,x,outcome,tag=None):
    if isinstance(x,bytes) and len(x) > 8:
      xp = f"[bytes x {len(x)}]"
    else:
      xp = x
    pad = self.labelpos - len(str(xp))
    print(f"{self.cmap[outcome]}{xp}{col.BLN}{' '*pad}{'' if tag is None else (col.YLW if '?' in tag else col.CYN)+tag+col.BLN}")

  def readAndEval(self,ref,nbytes,fapply,*,val=None,tag=None):
    if self.iomode == "read":
      x       = fapply(self.read(nbytes))
      ref.val = x
    elif self.iomode == "write":
      x = ref.val
      self.write(fapply(x))
    else:
      raise Exception("Unsupported io")
    if val is None or args.skipchecks:
      outcome = "any"
    elif (x == val) or (isinstance(val,list) and x in val):
      outcome = "good"
    else:
      raise Exception(f"expected to read {col.CRT}{val}{col.BLN}, actually read {col.CRT}{x}{col.BLN}")
      outcome = "bad"
      self.failed = True
    self.printmode(x,outcome,tag)
    if outcome == "bad":
      print(f"  Expected: {val}")
    return x

  def asAny(self,ref,n,*,tag=None):
    return self.readAndEval(ref, n, lambda x: x, val=None, tag=tag)

  def asConst(self,ref,*,val=None,tag=None):
    return self.readAndEval(ref, len(val), lambda x: x, val=val, tag=tag)

  def asByte(self,ref,*,val=None,tag=None):
    if self.iomode == "read":
      cb = lambda x: int.from_bytes(struct.unpack('c', x)[0],'little')
    else:
      cb = lambda x: struct.pack('c',x.to_bytes(1,"little",signed=False))
    return self.readAndEval(ref, 1, cb, val=val, tag=tag)

  def asUnsigned(self,ref,*,val=None,tag=None):
    if self.iomode == "read":
      cb = lambda x: struct.unpack('<I', x)[0]
    else:
      cb = lambda x: struct.pack('<I', int(x))
    return self.readAndEval(ref, 4, cb, val=val, tag=tag)

  def asFloat(self,ref,*,val=None,tag=None):
    if self.iomode == "read":
      cb = lambda x: struct.unpack('<f', x)[0]
    else:
      cb = lambda x: struct.pack('<f', float(x))
    return self.readAndEval(ref, 4, cb, val=val, tag=tag)

  def asSigned(self,ref,*,val=None,tag=None):
    if self.iomode == "read":
      cb = lambda x: struct.unpack('<i', x)[0]
    elif self.iomode == "write":
      cb = lambda x: struct.pack('<i', int(x))
    return self.readAndEval(ref, 4, cb, val=val, tag=tag)

  def asShort(self,ref,*,val=None,tag=None):
    if self.iomode == "read":
      cb = lambda x: struct.unpack('<h', x)[0]
    else:
      cb = lambda x: struct.pack('<h', int(x))
    return self.readAndEval(ref, 2, cb, val=val, tag=tag)

  def read(self,n):
    self.bytes_read += n
    return self.iostream.read(n)

  def write(self,data):
    self.bytes_read += self.iostream.write(data)
    return data

  def speculate(self,ref,*,val,tag):
    n = len(val)
    if self.iomode == "read":
      orig = self.iostream.tell()
      x = self.iostream.read(n)
      if x == val:
        self.bytes_read += n
        ref.val = x
        self.printmode(x,"good",tag)
        return True
      else:
        self.iostream.seek(orig)
        return False
    else:
      try:
        x = ref.val
        if x is None:
          del ref.val
          return False
        else:
          self.bytes_read += self.iostream.write(x)
          self.printmode(x,"good",tag)
          return True
      except AttributeError:
        return False

# Generic Parser class
class Parser(object):
  def __init__(self):
    super(Parser, self).__init__()
    self.root     = Ref({})
    self._valid   = False
    self.filename = None

  def valid(self):
    return self._valid

  def loadFrom(self,file):
    self.filename = file
    self.root = Ref()
    with open(file,'rb') as fin:
      data = fin.read()

    decoder = Decoder(data,"read")

    if not args.showparse:
      decoder.printmode = decoder.noprint

    self.parse(decoder,self.root,"read")
    return self

  def saveTo(self,file):
    self.filename = file
    if os.path.exists(file):
      os.remove(file)
    self.root.resetState()

    decoder = Decoder(None,"write")
    if not args.showparse:
      decoder.printmode = decoder.noprint

    data = self.parse(decoder,self.root,"write")
    with open(file,'wb') as fout:
      fout.write(data)

  def parse(self,decoder,root,mode):
    if root.val is None:
      root.val = {}

_uniq = 0
def getUniqueId():
  global _uniq
  _uniq += 1
  return f"{_uniq}"

# Parser for Gungeon WEM Data
class WEMParser(Parser):
  def __init__(self):
    super(WEMParser, self).__init__()

  def parse(self,decoder,root,mode):
    super(WEMParser, self).parse(decoder,root,mode)


    bs = decoder
    start_bytes = bs.bytes_read
    bs.asConst(root["wem_header"],  val=b"RIFF",  tag="'RIFF'")
    bs.asSigned(root["wem_length"], tag="bytes remaining")
    bs.asConst(root["wem_wave"],    val=b"WAVE",  tag="'WAVE'")

    bs.asConst(root["fmt_header"], val=b"fmt ",tag="format chunk")
    fmt_size = bs.asShort(root["fmt_size"], val=[24,66], tag="????? always 24 [wav i think?] or 66 [vorbis i think?]")

    bs.asShort(root[""],val=0, tag="????? always 0")
    cc = bs.asShort(root["compression_code"],val=[-2,-1, 2], tag="compression code, always -2 for flat bitrate / no compression, -1 = ogg compression?, 2 = wav compression?")

    bs.asShort(root["channels"],val=[1,2], tag="number of audio channels (1-2)")
    bs.asSigned(root["sample_rate"], tag="samples / second")
    bs.asSigned(root["avg_byte_rate"],tag="avg. bytes / second")

    blockalignbytes = bs.asShort(root["block_align"], tag="block align? (2 or 4 for our purposes, 36 and 72 also seen)")
    if cc == -2: # no compression
      sample_width = int(root["avg_byte_rate"]) / int(root["sample_rate"]) / int(root["channels"]) * 8
      # bs.asShort(root["sample_width"],val=sample_width, tag="bits per sample")
      bs.asShort(root["sample_width"],tag="bits per sample")
    elif cc == -1: # unknown
      bs.asShort(root["sample_width"],tag="bits per sample")
    elif cc == 2: # ogg???
      bs.asShort(root["sample_width"],tag="bits per sample")
    else:
      raise Exception("unrecognized compression format")

    extra_bytes = bs.asShort(root["extra_bytes"],val=fmt_size-18, tag="extra byte count == fmt_size - 18 (always 6 for WAVs and 48 for oggs)")
    bs.asShort(root["extra_unk"], tag="2 unknown extra bytes")
    true_extra_bytes = extra_bytes - 4 # discount header

    if true_extra_bytes == 44: #extra ogg data
      bs.asSigned(root["ogg_subtype"], tag="ogg subtype") # same as valid bits
      #Begin Vorbis header 0x00
      bs.asSigned(root["ogg_sample_count"], tag="ogg sample count")
      #0x04
      bs.asUnsigned(root["ogg_mod_signal"], tag="ogg mod signal")
      #0x08
      bs.asAny(root["ogg_unknown_1"], 8, tag="unknown ogg bytes")
      #0x10
      bs.asUnsigned(root["ogg_setup_packet_offset"], tag="ogg setup packet offset")
      #0x14
      bs.asUnsigned(root["ogg_first_audio_packet_offset"], tag="ogg first audio packet offset")
      #0x18
      bs.asAny(root["ogg_unknown_2"], 12, tag="unknown ogg bytes")
      #0x24
      bs.asUnsigned(root["ogg_uid"], tag="ogg uid")
      #0x28
      bs.asByte(root["ogg_blocksize_0_pow"], tag="ogg blocksize 0 pow")
      #0x29
      bs.asByte(root["ogg_blocksize_1_pow"], tag="ogg blocksize 1 pow")
      #0x2A
    elif true_extra_bytes in [4,2]:
      bs.asSigned(root["valid_bits"],val=[12546,16641], tag="valid bits per sample? always 12546 or 16641")
      # bs.asShort(root["valid_bits"],val=[12546,16641], tag="valid bits per sample? always 12546 or 16641")
    else:
      raise Exception(f"unknown extra bytes {extra_bytes}")

    found_akd  = False
    found_junk = False
    while True:
      # optional akd header
      if (not found_akd) and bs.speculate(root["akd_header"], val=b"akd ",tag="akd chunk"):
        found_akd = True
        akd_size = bs.asSigned(root["akd_size"],tag="@@@akd chunk length")
        bs.asAny(root[""], akd_size, tag=f"{akd_size} unknown bytes?")

      # optional JUNK header
      elif (not found_junk) and bs.speculate(root["junk_header"], val=b"JUNK",tag="junk chunk"):
        found_junk = True
        junk_size = bs.asSigned(root["junk_size"], tag="junk length")
        bs.asAny(root["junk_data"], junk_size, tag=f"{junk_size} junk bytes")

      # mandatory data header
      elif bs.speculate(root["data_header"], val=b"data",tag="data chunk"):
        data_size = bs.asSigned(root["data_chunk_size"], tag="data chunk size") #can't compute size if nested
        wavdata = bs.asAny(root["wav_data"],data_size,tag="WAV data")
        # if data_size == 1512084:# and data_size == 91152:# or data_size == 70884:# or data_size == 1512084:
        #   print("FOUND BEHOLSTER SONG")
        #   saveWAVData(f"/home/pretzel/downloads/gungeon-sounds/{getUniqueId()}.wav", wavdata, 1, int(root["sample_rate"]), int(root["sample_width"]))
        #   saveWAVData(f"/home/pretzel/downloads/gungeon-sounds/{getUniqueId()}.wav", wavdata, int(root["channels"]), int(root["sample_rate"]), int(root["sample_width"]))
        #   playWAVData(wavdata, int(root["channels"]), int(root["sample_rate"]), 8)
        #   A1 = np.frombuffer(wavdata, dtype=np.int8)
        #   A2 = np.pad(A1, (2, 2 + (4 - (len(A1) % 4))))
        #   A = np.frombuffer(A2, dtype=np.int16)
        #   b = A.byteswap()
        #   if cc == -2:
        #   playWAVData(b, int(root["channels"]), int(root["sample_rate"]), 8)
        #   with open(f"/home/pretzel/downloads/gungeon-sounds/{getUniqueId()}", 'wb') as fout:
        #     fout.write(wavdata)
        #   time.sleep(100000)
        break

      else:
        for i in range(64):
          bs.asAny(root[""], 1, tag="???")
          break

    wemdata = bs.iostream.getvalue()
    return wemdata

  def createMinimal(self, isOgg):
    root                     = self.root
    root["wem_header"]       = b"RIFF"
    root["wem_length"]       = None
    root["wem_wave"]         = b"WAVE"
    root["fmt_header"]       = b"fmt "
    root["fmt_size"]         = 66 if isOgg else 24
    root[""]                 = 0
    root["compression_code"] = -1 if isOgg else -2 # -2 == no compression
    root["channels"]         = None
    root["sample_rate"]      = None
    root["avg_byte_rate"]    = None
    root["block_align"]      = 0 if isOgg else None
    root["sample_width"]     = 0 if isOgg else None
    root["extra_bytes"]      = int(root["fmt_size"]) - 18
    root["extra_unk"]        = 0
    if isOgg:
      root["ogg_subtype"]                   = 12546
      root["ogg_sample_count"]              = None
      root["ogg_mod_signal"]                = 186 # unknown if this affects codebooks
      root["ogg_unknown_1"]                 = b'\x14\xfb\x16\x00\x00\x00@\x00' #b'\0'*8 # need to edit?
      root["ogg_setup_packet_offset"]       = 6016 # hardcode for beholster, need to edit
      root["ogg_first_audio_packet_offset"] = int(root["ogg_mod_signal"]) + int(root["ogg_setup_packet_offset"])
      root["ogg_unknown_2"]                 = b'\x05\x01@\x00\xf43\x00\x00\x945\x00\x00' #b'\0'*12 # need to edit?
      root["ogg_uid"]                       = 2840645231 # hardcode for beholster, need to edit?
      root["ogg_blocksize_0_pow"]           = 8
      root["ogg_blocksize_1_pow"]           = 11
    else:
      root["valid_bits"]       = 12546 # stereo
      # root["valid_bits"]       = 16641 # mono
    root["junk_header"]      = b"JUNK"
    root["junk_size"]        = 4
    root["junk_data"]        = b'\0\0\0\0'
    root["data_header"]      = b"data"
    root["data_chunk_size"]  = None
    root["wav_data"]         = None
    return self

  def loadFromOggFile(self, file):
    isOgg = True
    self.createMinimal(isOgg = isOgg)
    root = self.root

    wf        = SoundFile(file, 'r') # open any other audio file
    rate      = wf.samplerate
    total     = wf.frames
    channels  = wf.channels
    samples   = wf.frames

    with open(file, 'rb') as fin:
      wavdata = fin.read()[3675:] #[58:] # skip first 58 bytes
    # wavdata = wf.read(dtype="int32")
    # wavdata = wf.read(dtype="int32")
    # wavdata = wf.read(dtype="float64")
    print(f"ogg data: rate: {rate}, total: {total}, channels {channels}, data: {len(wavdata)} frames")

    root["channels"]        = channels
    root["sample_rate"]     = rate

    root["avg_byte_rate"]   = 10458 # 0 # don't know how to compute
    root["ogg_sample_count"] = 6350400 # samples

    root["data_chunk_size"] = len(wavdata) #* 2#4#8
    root["wav_data"]        = wavdata

    root["wem_length"]      = root["data_chunk_size"] + 56

    return self

  def loadFromWavFile(self,file):
    self.createMinimal(isOgg = False)
    root = self.root

    wf        = wave.open(file, 'rb')
    rate      = wf.getframerate()
    total     = wf.getnframes()
    channels  = wf.getnchannels()
    sampwidth = wf.getsampwidth()

    wavdata   = wf.readframes(total)

    root["channels"]        = channels
    root["sample_width"]    = sampwidth*8 # sample_width is encoded in bits, not bytes
    root["sample_rate"]     = rate
    if rate < 16000 and channels == 1: #mono tracks with low sample rates have been known to be pitch shifted in game, so issue a warning here
      warn(f"WARNING: mono sound {file} is {rate}hz, less than minimum supported 16000hz.")

    # vprint(f"Data: rate={rate}, channels={channels}, frames={total}, width={sampwidth}")

    # stereo: 12546 == 0110001 00000010
    #   mono: 16641 == 1000001 00000001
    root["valid_bits"]      = 12546 if (channels == 2) else 16641 #TODO: figure out what these magic bits do...
    root["block_align"]     = channels * sampwidth

    root["avg_byte_rate"]   = 0 # unnecessary #sampwidth * rate * channels
    # root["avg_byte_rate"]   = sampwidth * rate * channels

    root["data_chunk_size"] = len(wavdata)
    root["wav_data"]        = wavdata

    root["wem_length"]      = root["data_chunk_size"] + 56

    return self

  def saveToWavFile(self,file):
    root = self.root
    wf = wave.open(file, 'wb')

    wf.setframerate(int(root["sample_rate"]))
    wf.setnchannels(int(root["channels"]))
    wf.setsampwidth(int(root["sample_width"]//8))

    wf.writeframes(root["wav_data"].val)

# Parser for Gungeon BNK Data
class BNKParser(Parser):
  default_sound_params = {
    "volume"  : 0.0,
    "loops"   : 1,
    "channel" : "sound", #can also be "music" (and hopefully, in the future, "ui"))
    "limit"   : 0, #limit to number of sounds that can be simulatneously played
  }

  def __init__(self):
    super(BNKParser, self).__init__()
    self.n_embeds        = 0     #number of files currently embedded for wave export purposes
    self.next_wem_offset = 0     #byte offset within data section of next embedded WEM
    self.sound_params    = {}    #sound parameters
    self.embedded_files  = []    #list of filenames for embedded waves
    self.is_music        = False #whether we're currently parsing music

  def parse(self,decoder,root,mode):
    super(BNKParser, self).parse(decoder,root,mode)

    bs = decoder

    bs.asConst(root["bnk_head"] ,val=b'BKHD',tag="bkhd header")
    bs.asSigned(root["seclen"]  ,val=[24,28],tag="section length")
    bs.asSigned(root["version"]  ,tag="bank version")
    bs.asUnsigned(root["bankid"] ,tag="bank id")
    bs.asUnsigned(root[""]       ,tag="?????") # language id
    bs.asSigned(root[""]         ,tag="?????")
    bs.asSigned(root[""]         ,tag="?????")
    bs.asSigned(root[""]         ,tag="padding")
    if root["seclen"] == 28:
      bs.asSigned(root[""]         ,tag="extra padding")

    bs.asConst(root["didx_head"],val=b'DIDX',tag="didx header")
    bs.asSigned(root["didx_seclen"],tag="section length")
    wemfiles            = root["didx_seclen"] // 12
    wemids              = []
    wemlengths          = []
    wemoffs             = []
    # root["wemfileinfo"] = []
    for i in range(wemfiles):
      w = root["wemfileinfo"].next()
      bs.asUnsigned(w["wemid"],tag=f"wem {i} id")
      bs.asSigned(w["wemoff"],tag=f"wem {i} offset")
      bs.asSigned(w["wemlen"],tag=f"wem {i} length")
      wemids.append(w["wemid"])
      wemoffs.append(w["wemoff"])
      wemlengths.append(w["wemlen"])

    bs.asConst(root["data_head"],val=b'DATA',tag="data header")
    bs.asSigned(root["data_seclen"],tag="section length")
    wem_start = bs.bytes_read
    for i in range(wemfiles):
      w     = root["wemfiledata"].next()
      extra = wemoffs[i]+wem_start-bs.bytes_read
      if extra > 0:
        bs.asAny(w["extra_bytes"],extra)

      WEMParser().parse(bs,w,mode) # parse WEM substructure
      if DUMP_WAV_FILES:
        saveWAVData(f"/home/pretzel/downloads/{int(wemids[i])}.wav", w["wav_data"].val, int(w["channels"]), int(w["sample_rate"]), int(w["sample_width"]) // 8)
      # playWEMData(w["wem-data"])

    bs.asConst(root["hirc_head"],val=b'HIRC',tag="hirc header")
    bs.asSigned(root["hirc_seclen"],tag="section length")
    bs.asSigned(root["hirc_numobjects"],tag="number of objects")
    for i in range(int(root["hirc_numobjects"])):
      h         = root["hircobjects"].next()
      bs.asByte(h["type"],tag=f"obj {i} type")

      if h["type"] == 2: #sound effect
        bs.asSigned(h["subseclen"]      ,tag=f"SFX {i} subsection length")
        bs.asUnsigned(h["sfx_id"]       ,tag=f"SFX {i} ID")
        bs.asSigned(h["plugin_id"]      ,val=[65537,262145,131073],tag=f"SFX {i} PluginID (65537 = PCM, 131073 = ADPCM, 262145 = VORBIS)")
        bs.asByte(h["external_state"] ,val=0,tag=f"SFX {i} external state (should be 0 == embedded)")
        if h["external_state"] == 0:
          bs.asUnsigned(h["wem_file_id"]      ,val=wemids,tag=f"SFX {i} WEM file id")
          bs.asSigned(h["wem_file_num_bytes"] ,val=wemlengths,tag=f"SFX {i} in-memory byte length")
        else:
          raise Exception("don't know how to handle this event type")
        bs.asByte(h["sfx_unknown"] ,val=0,tag=f"SFX {i} source bits (usually 0, see uSourceBits in XML)")

        #begin sound structure section
        bs.asByte(h["sfx_override_parent"]  ,val=[0,1],tag=f"SFX {i} override parent")
        num_fx = bs.asByte(h["num_fx"]               ,tag=f"SFX {i} number of effects")
        if num_fx > 0:
          bs.asByte(h["fx_bypass"]               ,tag=f"SFX {i} bits effects bypass?")
          for j in range(num_fx):
            bs.asAny(h[""], 7, tag=f"SFX {i} FX {j} effect data")
            # raise Exception("don't know how to handle this event type")

        bs.asByte(h["override_attachments"] ,tag=f"SFX {i} override attachments")
        bs.asUnsigned(h["bus_id"]               ,tag=f"SFX {i} bus id")
        bs.asSigned(h["parent_id"]            ,tag=f"SFX {i} parent object id")
        bs.asByte(h["misc_flags"]           ,tag=f"SFX {i} misc. flags")
        bs.asByte(h["num_params"]           ,tag=f"SFX {i} num additional parameters")
        for j in range(int(h["num_params"])):
          p = h["param_type_list"].next()
          bs.asByte(p,tag=f"SFX {i} param {j} type")
        for j in range(int(h["num_params"])):
          p = h["param_list"].next()
          t = h["param_type_list"][j]
          if t == 0: #volume
            bs.asFloat(p["volume"],tag=f"SFX {i} param {j} volume (float)")
          # elif t == 2: #pitch # doesn't work yet
          #   bs.asSigned(p["pitch"],tag=f"SFX {i} param {j} pitch (float)")
          elif t == 58: #loop
            bs.asSigned(p["num_loops"],tag=f"SFX {i} param {j} num loops (float, 0 == inf)")
          else:
            bs.asAny(p["param_value"], 4, tag=f"SFX {i} unknown param {j} value")

        num_range_mods = bs.asByte(h["num_range_modifiers"], tag=f"SFX {i} num additional range paramters")
        for j in range(num_range_mods):
          p = h["range_param_type_list"].next()
          bs.asByte(p,tag=f"SFX {i} range param {j} type")
        for j in range(num_range_mods):
          p = h["range_param_list"].next()
          bs.asFloat(p["min_value"],tag=f"SFX {i} range param {j} min value (float)")
          bs.asFloat(p["min_value"],tag=f"SFX {i} range param {j} max value (float)")

        pos_data = bs.asByte(h["positioning_data"] ,tag=f"SFX {i} positioning data (7 is normal)")
        if pos_data & 0b10000: # 3D bit is set
          bs.asByte(h["positioning_data_3d"] ,tag=f"SFX {i} 3D positioning data")
          bs.asSigned(p["3d_attenuation_id"],tag=f"SFX {i} param {j} 3D attenutation id")

        aux = bs.asByte(h["aux_params"]           ,tag=f"SFX {i} aux parameters (if 4rd bit is set, we have aux params)")
        if aux & 0b1000:
          for j in range(4): # always exactly 4
            auxid = bs.asSigned(h[""]   ,tag=f"SFX {i} aux {j} id")

        bs.asByte(h["priority_tiebreak"]    ,tag=f"SFX {i} priority tiebreaker + other bits")
        bs.asByte(h["virt_queue_behavior"]  ,tag=f"SFX {i} virtual queue behavior (1 == use virtual voice)")
        bs.asShort(h["max_sounds"]           ,tag=f"SFX {i} max sound limit (0 == no limit)")
        bs.asByte(h["below_thres_behavior"] ,tag=f"SFX {i} below threshold behavior (0 == continue to play)")
        bs.asByte(h["envelope"]             ,tag=f"SFX {i} envelope bits")
        bs.asByte(h["num_state_props"]      ,val=0,tag=f"SFX {i} number of state props (needs to be 0)")
        bs.asByte(h["num_state_groups"]     ,val=0,tag=f"SFX {i} number of state groups (needs to be 0)")
        bs.asShort(h["num_rtpcs"]            ,tag=f"SFX {i} num RTPCs")
        for j in range(int(h["num_rtpcs"])):
          r                    = h["rtpcs"].next()
          bs.asUnsigned(r["x_axis"]          ,tag=f"SFX {i} RTPC {j} x-axis game parameter id")
          bs.asByte(r["rtpc_type"]       ,val=0,tag=f"SFX {i} RTPC {j} type (0 == gameparameter)")
          bs.asByte(r["rtpc_accum"]      ,val=2,tag=f"SFX {i} RTPC {j} accum (2 == additive)")
          bs.asByte(r["rtpc_param"]      ,val=[0,2],tag=f"SFX {i} RTPC {j} parameter id (0 == volume, 2 == pitch)")
          bs.asSigned(r["rtpc_curve_id"]   ,tag=f"SFX {i} RTPC {j} curve id")
          bs.asByte(r["rtpc_scaling"]    ,val=[0,2],tag=f"SFX {i} RTPC {j} scaling (2 == decibels, 0 == none)")
          bs.asShort(r["num_rtpc_points"] ,tag=f"SFX {i} RTPC {j} num points")
          for k in range(int(r["num_rtpc_points"])):
            p           = r["rtpc_points"].next()
            bs.asFloat(p["x"]      , tag=f"SFX {i} RTPC {j} point {k} x coord")
            bs.asFloat(p["y"]      , tag=f"SFX {i} RTPC {j} point {k} y coord")
            bs.asSigned(p["interp"] , tag=f"SFX {i} RTPC {j} point {k} interpolation type (4 == linear)")

      elif h["type"] == 3: #action
        action_len = bs.asSigned(h["subseclen"]       ,tag=f"Action {i} subsection length")
        bs.asUnsigned(h["action_id"]       ,tag=f"Action {i} ID")
        bs.asByte(h["action_scope"]    ,tag=f"Action {i} action scope (byte 1/2) (3 == game object, 2 = global)")
        bs.asByte(h["action_type"]     ,val=[1,2,3,4,12,14,18],tag=f"Action {i} action type (byte 2/2) (1 == stop, 2 pause, 3 resume, 4 play, 18 setState, 14 SetLPF_O, 12 set bus volume)")
        bs.asUnsigned(h["action_sfx_id"],tag=f"Action {i} game object (SFX) id")
        bs.asByte(h["action_bus_bits"] ,tag=f"Action {i} bus bits")
        action_props = bs.asByte(h["action_props_1"], tag=f"Action {i} props (usually 0)")
        for j in range(action_props):
            bs.asAny(h[""], 1, tag=f"Action {i} prop {j} type")
            bs.asAny(h[""], 4, tag=f"Action {i} prop {j} value")

        range_props = bs.asByte(h["action_props_2"]  ,val=0,tag=f"Action {i} range props (needs to be 0)")
        for j in range(range_props):
            bs.asAny(h[""], 1, tag=f"Action {i} range prop {j} type")
            bs.asAny(h[""], 4, tag=f"Action {i} range prop {j} value")

        if int(h["action_type"]) == 4: #play
          bs.asByte(h["action_play_fade_curve"] ,tag=f"Action {i} play fade curve (4 == linear)")
          bs.asUnsigned(h["bank_id"] ,val=root["bankid"],tag=f"Action {i} bank id")
        elif int(h["action_type"]) == 1: #stop
          bs.asByte(h["action_stop_fade_curve"]     ,tag=f"Action {i} stop fade curve (4 == linear)")
          bs.asByte(h["action_stop_flags"]          ,val=6,tag=f"Action {i} stop bit flags (6 == expected)")
          exceptions = bs.asByte(h["action_stop_num_exceptions"], tag=f"Action {i} stop exception list size (usually 0)")
          for j in range(exceptions):
            bs.asSigned(h[""], tag=f"Action {i} exception {j} id")
            bs.asByte(h[""], tag=f"Action {i} exception {j} is bus")

        elif int(h["action_type"]) in [2,3]: #pause / resume
          resume = int(h["action_type"]) == 3
          flags = 6 if resume else 7
          name = "resume" if resume else "pause"
          bs.asByte(h["action_pause_fade_curve"]     ,tag=f"Action {i} {name} fade curve (4 == linear)")
          bs.asByte(h["action_pause_flags"]          ,val=[6,7],tag=f"Action {i} {name} bit flags (6 or 7 == expected)")
          bs.asByte(h["action_pause_num_exceptions"] ,val=0,tag=f"Action {i} {name} exception list size (needs to be 0)")
        elif int(h["action_type"]) in [18]: #setState
          bs.asSigned(h["action_state_group_id"], tag=f"Action {i} state group id")
          bs.asSigned(h["action_state_target_id"], tag=f"Action {i} state target id")
        elif int(h["action_type"]) in [14, 12]: #SetLPF_O, set bus volume
          bs.asByte(h["action_lpf_bits"] , tag=f"Action {i} LPF bit vector")
          bs.asByte(h["action_lpf_value_meaning"] , tag=f"Action {i} LPF value meaning")
          bs.asFloat(h["action_lpf_random_base"] , tag=f"Action {i} LPF randomizer base")
          bs.asFloat(h["action_lpf_random_min"] , tag=f"Action {i} LPF randomizer min")
          bs.asFloat(h["action_lpf_random_max"] , tag=f"Action {i} LPF randomizer max")
          exceptions = bs.asByte(h["action_stop_num_exceptions"], tag=f"Action {i} LPF exception list size (usually 0)")
          for j in range(exceptions):
            bs.asSigned(h[""], tag=f"Action {i} LPF exception {j} id")
            bs.asByte(h[""], tag=f"Action {i} LPF exception {j} is bus")
        else:
          raise Exception("don't know how to handle this event type")

      elif h["type"] == 4: #event
        bs.asSigned(h["subseclen"] ,tag=f"Event {i} subsection length")
        bs.asUnsigned(h["event_id"] ,tag=f"Event {i} id")
        bs.asByte(h["num_events"] ,tag=f"Event {i} num actions")
        for j in range(int(h["num_events"])):
          e = h["events"].next()
          bs.asUnsigned(e,tag=f"Event {i} action {j} id")

      else:
        sslen = bs.asSigned(h["subseclen"]      ,tag=f"Generic {i} subsection length")
        bs.asAny(h[""], sslen)
        # raise Exception(f"""unhandled HIRC event {int(h["type"])}""")

    self._valid = not bs.failed
    return bs.iostream.getvalue()

  def createMinimal(self,bankid):
    root                    = self.root
    root["bnk_head"]        = b'BKHD'
    root["seclen"]          = 24
    root["version"]         = 128
    root["bankid"]          = bankid
    root[""]                = bankid
    # root[""]                = 393239870 # stringToBnkID("SFX") [marked internally as "dwLanguageID", should maybe also be bankid?]
    root[""]                = 0
    root[""]                = 0
    root[""]                = 0

    root["didx_head"]       = b'DIDX'
    root["didx_seclen"]     = 0
    root["wemfileinfo"]     = []

    root["data_head"]       = b'DATA'
    root["data_seclen"]     = 0
    root["wemfiledata"]     = []

    root["hirc_head"]       = b'HIRC'
    root["hirc_seclen"]     = 4 #length of hirc_numobjects
    root["hirc_numobjects"] = 0
    root["hircobjects"]     = []

    return self

  def createExampleSpreadsheet(self,fname):
    keys = ["name"] + [k for k in self.default_sound_params.keys()]
    rows = [keys]
    for f in self.embedded_files:
      rows.append([f] + [v for k,v in self.default_sound_params.items()])
    with open(fname,'w') as fout:
      writer = csv.writer(fout)
      for row in rows:
        writer.writerow(row)

  def setSoundParams(self,sound_params):
    self.sound_params = sound_params

  def addDefaultVolumeRTPCToSFX(self,hirc_root):
    h = hirc_root
    rtpc_curve_id        = self.n_embeds+900000 # non-magic, needs to be unique

    h["num_rtpcs"]      += 1
    r                    = h["rtpcs"].next()
    r["x_axis"]          = GUNGEON_RTPC_ID_MUSIC if self.is_music else GUNGEON_RTPC_ID_SFX
    r["rtpc_type"]       = 0 # 0 == volume
    r["rtpc_accum"]      = 2 # 2 == additive
    r["rtpc_param"]      = 0
    r["rtpc_curve_id"]   = rtpc_curve_id
    r["rtpc_scaling"]    = 2
    r["num_rtpc_points"] = 2

    r1                   = r["rtpc_points"].next()
    r1["x"]              = 0.0
    r1["y"]              = -1.0
    r1["interp"]         = 4 #linear

    r2                   = r["rtpc_points"].next()
    r2["x"]              = 100.0
    r2["y"]              = 0.0
    r2["interp"]         = 4 #linear

    h["subseclen"]      += 39

  def updateHircMetadataFromRef(self,h):
    self.root["hirc_seclen"]     += 1                   #size of hircobject->type
    self.root["hirc_seclen"]     += 4                   #size of hircobject->subseclen
    self.root["hirc_seclen"]     += int(h["subseclen"]) #total length of fields added above
    self.root["hirc_numobjects"] += 1
    self.root["hircobjects"].append(h.val)

  def addDefaultVolumeParamToSFX(self,h,volume=1.0):
    h["num_params"] += 1
    h["param_type_list"].append(0) #volume type
    vol = h["param_list"].next()
    vol["volume"] = volume #volume value
    h["subseclen"] += 5

  def addDefaultPitchParamToSFX(self,h,pitch=0.0):
    return # doesn't work
    # h["num_params"] += 1
    # h["param_type_list"].append(2) #pitch type
    # p = h["param_list"].next()
    # p["pitch"] = pitch #pitch value
    # h["subseclen"] += 5

  def addDefaultLoopParamToSFX(self,h,num_loops=1):
    h["num_params"] += 1
    h["param_type_list"].append(58) #loop type
    loop = h["param_list"].next()
    loop["num_loops"] = num_loops #number of loops (0 == infinite)
    h["subseclen"] += 5

  def addHircSFX(self,sfx_id,wfi,isOgg,limit):
    h                         = Ref({})
    h["type"]                 = HIRC_TYPE_SFX
    h["subseclen"]            = 43 #minimum length of subsection
      # + (5 * num_params)
      # + (1 if num_effects > 0)
      # + (7 * num_effects)
      # + (total size of rtpcs)
      #     (each rtpc is 15 + 12*num_points bytes)
    h["sfx_id"]               = sfx_id
    h["plugin_id"]            = 262145 if isOgg else 65537
    h["external_state"]       = 0 # 0 == embedded
    h["wem_file_id"]          = int(wfi["wemid"])
    h["wem_file_num_bytes"]   = int(wfi["wemlen"])
    h["sfx_unknown"]          = 0
    h["sfx_override_parent"]  = 0
    h["num_fx"]               = 0
    h["override_attachments"] = 0
    h["bus_id"]               = GUNGEON_BUS_ID
    h["parent_id"]            = 0 # 90804066 for music???
    h["misc_flags"]           = 0
    h["num_params"]           = 0
    h["num_range_modifiers"]  = 0
    h["positioning_data"]     = 7
    h["aux_params"]           = 0
    h["priority_tiebreak"]    = 8 # 0 == follow parent, 8 == ignore parent, destroy oldest, 9 == ignore parent, destroy newest
    h["virt_queue_behavior"]  = 1
    h["max_sounds"]           = limit # 0
    h["below_thres_behavior"] = 0
    h["envelope"]             = 0
    h["num_state_props"]      = 0
    h["num_state_groups"]     = 0
    h["num_rtpcs"]            = 0
    return h

  def addHircPlayAction(self,action_id,sfx_id):
    h                           = Ref({})
    h["type"]                   = HIRC_TYPE_ACTION
    h["subseclen"]              = 18 #always 18 for play events?
    h["action_id"]              = action_id
    h["action_scope"]           = 3 # 3 == game object, 2 == global
    h["action_type"]            = 4 # 4 == play, 1 == stop
    h["action_sfx_id"]          = sfx_id
    h["action_bus_bits"]        = 0
    h["action_props_1"]         = 0
    h["action_props_2"]         = 0
    h["action_play_fade_curve"] = 4 # 4 == linear
    h["bank_id"]                = int(self.root["bankid"])
    return h

  def addHircPauseAction(self,action_id,sfx_id,resume=False):
    h                                = Ref({})
    h["type"]                        = HIRC_TYPE_ACTION
    h["subseclen"]                   = 16 #always 16 for pause events?
    h["action_id"]                   = action_id
    h["action_scope"]                = 4 if self.is_music else 3 # 3 == game object, 2 == global, 4 == also global???
    h["action_type"]                 = 3 if resume else 2 # 2 == pause, 3 = resume
    h["action_sfx_id"]               = sfx_id
    h["action_bus_bits"]             = 0
    h["action_props_1"]              = 0
    h["action_props_2"]              = 0
    h["action_pause_fade_curve"]     = 4 # 4 == linear
    h["action_pause_flags"]          = 7 # magic numbers (7 allows a master pause / resume)
    h["action_pause_num_exceptions"] = 0
    h["bank_id"]                     = int(self.root["bankid"])
    return h

  def addHircStopAction(self,action_id,sfx_id,stop_all=False):
    h                               = Ref({})
    h["type"]                       = HIRC_TYPE_ACTION
    h["subseclen"]                  = 16 #always 16 for stop events?
    h["action_id"]                  = action_id
    h["action_scope"]               = 2 if (stop_all or self.is_music) else 3 # 3 == game object, 2 == global
    h["action_type"]                = 1 # 4 == play, 1 == stop
    h["action_sfx_id"]              = sfx_id
    h["action_bus_bits"]            = 0
    h["action_props_1"]             = 0
    h["action_props_2"]             = 0
    h["action_stop_fade_curve"]     = 4 # 4 == linear
    h["action_stop_flags"]          = 6 # 6 == magic number
    h["action_stop_num_exceptions"] = 0
    h["bank_id"]                    = int(self.root["bankid"])
    return h

  def addHircEvent(self,event_id):
    h               = Ref({})
    h["type"]       = HIRC_TYPE_EVENT
    h["subseclen"]  = 5
    h["event_id"]   = event_id
    h["num_events"] = 0
    return h

  def addHircActionToHircEvent(self,action_id,hirc_event):
    h = hirc_event
    h["num_events"] += 1
    h["subseclen"]  += 4
    h["events"].append(action_id)

  def embedFromWav(self, wavfile, isOgg):
    base_fname   = os.path.splitext(os.path.basename(wavfile))[0]
    self.embedded_files.append(base_fname)
    sound_params = None

    if base_fname in self.sound_params:
      sound_params = self.sound_params[base_fname]
      vprint(f"      >> Found custom sound params {sound_params} for {base_fname}")
    else:
      sound_params = self.default_sound_params
      vprint(f"      >> Using default sound params {sound_params} for {base_fname}")

    self.is_music = sound_params.get("channel","sound")=="music"

    root           = self.root
    self.n_embeds += 1

    # Set up unique generated ids
    play_event_id      = stringToBnkID(base_fname)
    pause_event_id     = stringToBnkID(base_fname+"_pause")
    resume_event_id    = stringToBnkID(base_fname+"_resume")
    stop_event_id      = stringToBnkID(base_fname+"_stop")
    stop_all_event_id  = stringToBnkID(base_fname+"_stop_all")
    wemid              = stringToBnkID(base_fname+"_wem_id")   #non-magic, needs to be unique
    sfx_id             = stringToBnkID(base_fname+"_sfx_id")   #non-magic, needs to be unique
    play_action_id     = stringToBnkID(str(play_event_id))     #non-magic, needs to be unique
    pause_action_id    = stringToBnkID(str(pause_event_id))    #non-magic, needs to be unique
    resume_action_id   = stringToBnkID(str(resume_event_id))   #non-magic, needs to be unique
    stop_action_id     = stringToBnkID(str(stop_event_id))     #non-magic, needs to be unique
    stop_all_action_id = stringToBnkID(str(stop_all_event_id)) #non-magic, needs to be unique
    vprint(f"      >> event id for playing      '{base_fname            }' -> {play_event_id}")
    vprint(f"      >> event id for pausing      '{base_fname+'_pause'   }' -> {pause_event_id}")
    vprint(f"      >> event id for resuming     '{base_fname+'_resume'  }' -> {resume_event_id}")
    vprint(f"      >> event id for stopping     '{base_fname+'_stop'    }' -> {stop_event_id}")
    vprint(f"      >> event id for stopping all '{base_fname+'_stop_all'}' -> {stop_all_event_id}")

    # Load the wavfile as a WEM
    if isOgg:
      wp = WEMParser().loadFromOggFile(wavfile)
    else:
      wp = WEMParser().loadFromWavFile(wavfile)

    # Create the wem info header
    root["didx_seclen"]  += 12
    wfi                   = Ref({})
    wfi["wemid"]          = wemid
    wfi["wemoff"]         = self.next_wem_offset # needs to be updated for each wem
    wfi["wemlen"]         = int(wp.root["wem_length"])+8
    self.next_wem_offset += int(wfi["wemlen"])
    root["wemfileinfo"].append(wfi.val)

    # Create the wem data
    root["data_seclen"] += int(wfi["wemlen"]) #todo: might need padding
    root["wemfiledata"].append(wp.root.val)

    # Create the hirc SFX data
    sfx = self.addHircSFX(sfx_id,wfi,isOgg=isOgg,limit=sound_params.get("limit",0))
    self.addDefaultVolumeParamToSFX(sfx,volume=sound_params.get("volume",1.0))
    # self.addDefaultPitchParamToSFX(sfx,pitch=sound_params.get("pitch",0.0)) # doesn't work
    self.addDefaultLoopParamToSFX(sfx,num_loops=sound_params.get("loops",1))
    self.addDefaultVolumeRTPCToSFX(sfx)
    self.updateHircMetadataFromRef(sfx)

    # Create the play action
    play_action = self.addHircPlayAction(play_action_id,sfx_id)
    self.updateHircMetadataFromRef(play_action)

    # Create the play event
    play_event = self.addHircEvent(play_event_id)
    self.addHircActionToHircEvent(play_action_id,play_event)
    self.updateHircMetadataFromRef(play_event)

    # Create the pause action
    pause_action = self.addHircPauseAction(pause_action_id,sfx_id,resume=False)
    self.updateHircMetadataFromRef(pause_action)

    # Create the pause event
    pause_event = self.addHircEvent(pause_event_id)
    self.addHircActionToHircEvent(pause_action_id,pause_event)
    self.updateHircMetadataFromRef(pause_event)

    # Create the resume action
    resume_action = self.addHircPauseAction(resume_action_id,sfx_id,resume=True)
    self.updateHircMetadataFromRef(resume_action)

    # Create the resume event
    resume_event = self.addHircEvent(resume_event_id)
    self.addHircActionToHircEvent(resume_action_id,resume_event)
    self.updateHircMetadataFromRef(resume_event)

    # Create the stop action
    stop_action = self.addHircStopAction(stop_action_id,sfx_id,stop_all=False)
    self.updateHircMetadataFromRef(stop_action)

    # Create the stop event
    stop_event = self.addHircEvent(stop_event_id)
    self.addHircActionToHircEvent(stop_action_id,stop_event)
    self.updateHircMetadataFromRef(stop_event)

    # Create the stop all action
    stop_all_action = self.addHircStopAction(stop_all_action_id,sfx_id,stop_all=True)
    self.updateHircMetadataFromRef(stop_all_action)

    # Create the stop all event
    stop_all_event = self.addHircEvent(stop_all_event_id)
    self.addHircActionToHircEvent(stop_all_action_id,stop_all_event)
    self.updateHircMetadataFromRef(stop_all_event)

    return self

# Helper function for converting WAV file to WEM file
def convertWavToWem(ifname,ofname=None):
  if ofname is None: # automatically determine WEM name
    ofname = f"{os.path.splitext(ifname)[0]}.wem"
  vprint(f"    >> exporting {ofname}")
  wp = WEMParser().loadFromWavFile(ifname)
  wp.saveTo(ofname)
  # playWEMData(wp.root)

  # Debug sanity check that we can get the original .WAV file back
  # wp2 = WEMParser().loadFrom(ofname)
  # dfname = ifname+".bak"
  # wp2.saveToWavFile(dfname)
  # os.system(f"/bin/md5sum {ifname} {dfname}")
  # os.remove(dfname)

def prompt(message, default='y'):
    choices = 'Y/n' if default.lower() in ('y', 'yes') else 'y/N'
    choice = input(f"{message} ({choices}) ")
    values = ('y', 'yes', '') if choices == 'Y/n' else ('y', 'yes')
    return choice.strip().lower() in values

def loadSoundParamsFromCSV(csvfile):
    with open(csvfile,'r') as fin:
      reader = csv.reader(fin)
      header = [s.strip() for s in next(reader)]
      ddict = {}
      for row in reader:
        if len(row) == 0:
          continue
        minlen = min(len(row), len(header))
        ddict[row[0].strip()] = {header[i] : row[i].strip() for i in range(1, minlen)}
    return ddict

def main():
  sound_params = None
  if args.spreadsheet and os.path.exists(args.spreadsheet):
    vprint(f">> Loading sound parameters from {args.spreadsheet}")
    sound_params = loadSoundParamsFromCSV(args.spreadsheet)

  if args.readbank:
    args.showparse = True
    args.dumpparse = True
    b = BNKParser()
    b.loadFrom(args.input_path)
    b.root.dump()
    return

  # build list of wav files to parse
  vprint(f">> {col.CYN+'recursively '+col.BLN if args.recursive else ''}scanning {col.GRN}{args.input_path}{col.BLN} for wave files")
  wavs_to_parse = findWavsInDirectory(args.input_path, recursive=args.recursive)

  # Generate a bank id from the file name
  base_bnk_name = os.path.splitext(os.path.basename(args.output_bank_name))[0]
  bank_id       = stringToBnkID(base_bnk_name)

  # Create a sound bank in memory and add our wav files
  vprint(f"  >> Creating bank with id {bank_id}")
  bp            = BNKParser().createMinimal(bank_id)

  if sound_params is not None:
    bp.setSoundParams(sound_params)

  # Add our .wav files to the sound bank
  vprint(f"  >> embedding {len(wavs_to_parse)} .wav files into sound bank")
  for w in wavs_to_parse:
    vprint(f"    >> embedding {col.GRN}{w}{col.BLN} into sound bank")
    bp.embedFromWav(w, isOgg = w.endswith(".ogg"))
    if args.create_wems:
      convertWavToWem(w)

  # Dump parsed bank information if requested
  if args.dumpparse:
    bp.root.dump()

  # Determine path to our output .bnk file to and save it
  outfile = args.output_bank_name
  if not outfile.endswith(".bnk"):
    outfile += ".bnk"
  if not os.path.isabs(outfile):
    outfile = os.path.join(args.input_path,outfile)
  if os.path.exists(outfile):
    if not args.overwrite:
      if not prompt(f"Overwrite {outfile}?"):
        print(f"Exiting without overwriting {outfile}")
        sys.exit(0)
    os.remove(outfile)
  vprint(f"  >> writing bank to {col.GRN}{outfile}{col.BLN}")
  bp.saveTo(outfile)
  vprint(">> done :D")
  print(f"Created soundbank {outfile} with {len(wavs_to_parse)} .wav files")

  if args.spreadsheet and not os.path.exists(args.spreadsheet):
    bp.createExampleSpreadsheet(args.spreadsheet)
    print(f"Saved example spreadsheet to {args.spreadsheet}")

  # (DEBUG) compute checksums w.r.t. reference bank
  # os.system(f"/bin/md5sum ./ref.bnk {outfile}")

def mainAutorun():
  args.overwrite = True
  args.input_path = os.path.dirname(os.path.realpath(__file__))
  bankname = "Sounds"
  for file in os.listdir(args.input_path):
      if file.endswith(".csv"):
          bankname = file[:-4]
          break
  args.spreadsheet = os.path.join(args.input_path, f"{bankname}.csv")
  args.output_bank_name = os.path.join(args.input_path, f"{bankname}.bnk")
  main()
  print()
  input("Press return to exit")

if __name__ == "__main__":
  if ALLOW_AUTORUN and (args.input_path is None):
    mainAutorun()
  else:
    main()
  # print(stringToBnkID("Play_MUS_Boss_Theme_Beholster")) #1075162602
