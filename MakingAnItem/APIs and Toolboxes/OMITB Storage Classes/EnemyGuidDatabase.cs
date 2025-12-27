using Dungeonator;
using HutongGames.PlayMaker.Actions;
using NevernamedsItems;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GUIDs
{
    //Bullet Kin & Variants
    public static readonly string Bullet_Kin = "01972dee89fc4404a5c408d50007dad5";
    public static readonly string Bullet_Kin_AK47 = "db35531e66ce41cbb81d507a34366dfe";
    public static readonly string Bandana_Bullet_Kin = "88b6b6a93d4b4234a67844ef4728382c";
    public static readonly string Veteran_Bullet_Kin = "70216cae6c1346309d86d4a0b4603045";
    public static readonly string Tanker = "df7fb62405dc4697b7721862c7b6b3cd";
    public static readonly string Minelet = "3cadf10c489b461f9fb8814abc1a09c1";
    public static readonly string Cardinal = "8bb5578fba374e8aae8e10b754e61d62";
    public static readonly string Shroomer = "e5cffcfabfae489da61062ea20539887";
    public static readonly string Ashen_Bullet_Kin = "1a78cfb776f54641b832e92c44021cf2";
    public static readonly string Mutant_Bullet_Kin = "d4a9836f8ab14f3fadd0f597438b1f1f";
    public static readonly string Fallen_Bullet_Kin = "5f3abc2d561b4b9c9e72b879c6f10c7e";
    public static readonly string Confirmed = "844657ad68894a4facb1b8e1aef1abf9";
    public static readonly string Red_Caped_Bullet_Kin = "fa6a7ac20a0e4083a4c2ee0d86f8bbf7";
    public static readonly string Office_Bullet_Kin = "906d71ccc1934c02a6f4ff2e9c07c9ec";
    public static readonly string Office_Bullette_Kin = "9eba44a0ea6c4ea386ff02286dd0e6bd";
    public static readonly string Brollet = "05cb719e0178478685dc610f8b3e8bfc";
    public static readonly string Western_Bullet_Kin = "5861e5a077244905a8c25c2b7b4d6ebb";
    public static readonly string Pirate_Bullet_Kin = "6f818f482a5c47fd8f38cce101f6566c";
    public static readonly string Knight_Bullet_Kin = "39e6f47a16ab4c86bec4b12984aece4c";
    public static readonly string Tutorial_Bullet_Kin = "b08ec82bef6940328c7ecd9ffc6bd16c";

    //Loot Enemies
    public static readonly string Keybullet_Kin = "699cd24270af4cd183d671090d8323a1";
    public static readonly string Chance_Kin = "a446c626b56d4166915a4e29869737fd";

    //Dead/Undead Bullet Kin
    public static readonly string Hollowpoint = "4db03291a12144d69fe940d5a01de376";
    public static readonly string Spectre = "56f5a0f2c1fc4bc78875aea617ee31ac";
    public static readonly string Skullet = "336190e29e8a4f75ab7486595b700d4a";
    public static readonly string Skullmet = "95ec774b5a75467a9ab05fa230c0c143";
    public static readonly string Gummy = "5288e86d20184fa69c91ceb642d31474";
    public static readonly string Spent = "249db525a9464e5282d02162c88e0357";
    public static readonly string Large_Spent = "e21ac9492110493baef6df02a2682a0d";

    //Shotgun Kin & Variants
    public static readonly string Red_Shotgun_Kin = "128db2f0781141bcb505d8f00f9e4d47";
    public static readonly string Blue_Shotgun_Kin = "b54d89f9e802455cbb2b8a96a31e8259";
    public static readonly string Veteran_Shotgun_Kin = "2752019b770f473193b08b4005dc781f";
    public static readonly string Mutant_Shotgun_Kin = "7f665bd7151347e298e4d366f8818284";
    public static readonly string Executioner = "b1770e0f1c744d9d887cc16122882b4f";
    public static readonly string Ashen_Shotgun_Kin = "1bd8e49f93614e76b140077ff2e33f2b";
    public static readonly string Shotgrub = "044a9f39712f456597b9762893fbc19c";
    public static readonly string Creech = "37340393f97f41b2822bc02d14654172";
    public static readonly string Western_Shotgun_Kin = "ddf12a4881eb43cfba04f36dd6377abb";
    public static readonly string Pirate_Shotgun_Kin = "86dfc13486ee4f559189de53cfb84107";

    //Blobulonians
    public static readonly string Blobulon = "0239c0680f9f467dbe5c4aab7dd1eca6";
    public static readonly string Blobuloid = "042edb1dfb614dc385d5ad1b010f2ee3";
    public static readonly string Blobulin = "42be66373a3d4d89b91a35c9ff8adfec";
    public static readonly string Poisbulon = "e61cab252cfb435db9172adc96ded75f";
    public static readonly string Poisbuloid = "fe3fe59d867347839824d5d9ae87f244";
    public static readonly string Poisbulin = "b8103805af174924b578c98e95313074";
    public static readonly string Blizzbulon = "022d7c822bc146b58fe3b0287568aaa2";
    public static readonly string Leadbulon = "ccf6d241dad64d989cbcaca2a8477f01";
    public static readonly string Bloodbulon = "062b9b64371e46e195de17b6f10e47c8";
    public static readonly string Poopulon = "116d09c26e624bca8cca09fc69c714b3";
    public static readonly string Cubulon = "864ea5a6a9324efc95a0dd2407f42810";
    public static readonly string Cubulead = "0b547ac6b6fc4d68876a241a88f5ca6a";
    public static readonly string Chancebulon = "1bc2a07ef87741be90c37096910843ab";

    //Skulls & Skeletons
    public static readonly string Skusket = "af84951206324e349e1f13f9b7b60c1a";
    public static readonly string Black_Skusket = "1cec0cdf383e42b19920787798353e46";
    public static readonly string Skusket_Head = "c2f902b7cbe745efb3db4399927eab34";
    public static readonly string Shelleton = "21dd14e5ca2a4a388adab5b11b69a1e1";
    public static readonly string Revolvenant = "d5a7b95774cd41f080e517bea07bf495";
    public static readonly string Gunreaper = "88f037c3f93b4362a040a87b30770407";
    public static readonly string Lord_Of_The_Jammed_Dummy = "0d3f7c641557426fbac8596b61c9fb45";

    //Bullats
    public static readonly string Bullat = "2feb50a6a40f4f50982e89fd276f6f15";
    public static readonly string Shotgat = "2d4f8b5404614e7d8b235006acde427a";
    public static readonly string Grenat = "b4666cb6ef4f4b038ba8924fd8adf38f";
    public static readonly string Spirat = "7ec3e8146f634c559a7d58b19191cd43";
    public static readonly string King_Bullat = "1a4872dafdb34fd29fe8ac90bd2cea67";
    public static readonly string Bullat_Gargoyle = "981d358ffc69419bac918ca1bdf0c7f7";

    //Gunjurers
    public static readonly string Apprentice_Gunjurer = "206405acad4d4c33aac6717d184dc8d4";
    public static readonly string Gunjurer = "c4fba8def15e47b297865b18e36cbef8";
    public static readonly string High_Gunjurer = "9b2cf2949a894599917d4d391a0b7394";
    public static readonly string Lore_Gunjurer = "56fb939a434140308b8f257f0f447829";

    //Bookllets
    public static readonly string Bookllet = "c0ff3744760c4a2eb0bb52ac162056e6";
    public static readonly string Blue_Bookllet = "6f22935656c54ccfb89fca30ad663a64";
    public static readonly string Green_Bookllet = "a400523e535f41ac80a43ff6b06dc0bf";
    public static readonly string Angry_Necronomicon = "216fd3dfb9da439d9bd7ba53e1c76462";
    public static readonly string Angry_Tablet = "78e0951b097b46d89356f004dda27c42";

    //Sliding Cubes
    public static readonly string Mountain_Cube = "f155fd2759764f4a9217db29dd21b7eb";
    public static readonly string Lead_Cube = "33b212b856b74ff09252bf4f2e8b8c57";
    public static readonly string Flesh_Cube = "3f2026dc3712490289c4658a2ba4a24b";
    public static readonly string Rat_Cube = "ba928393c8ed47819c2c5f593100a5bc";

    //Mimics
    public static readonly string Mimic_Brown = "2ebf8ef6728648089babb507dec4edb7";
    public static readonly string Mimic_Blue = "d8d651e3484f471ba8a2daa4bf535ce6";
    public static readonly string Mimic_Green = "abfb454340294a0992f4173d6e5898a8";
    public static readonly string Mimic_Red = "d8fd592b184b4ac9a3be217bc70912a2";
    public static readonly string Mimic_Black = "6450d20137994881aff0ddd13e3d40c8";
    public static readonly string Mimic_Rat = "ac9d345575444c9a8d11b799e8719be0";
    public static readonly string Pedestal_Mimic = "796a7ed4ad804984859088fc91672c7f";
    public static readonly string Wall_Mimic = "479556d05c7c44f3b6abb3b2067fc778";

    //Gun Nuts
    public static readonly string Gun_Nut = "ec8ea75b557d4e7b8ceeaacdf6f8238c";
    public static readonly string Chain_Gunner = "463d16121f884984abe759de38418e48";
    public static readonly string Spectral_Gun_Nut = "383175a55879441d90933b5c4e60cf6f";

    //Dets (All Directions)
    public static readonly string Det = "ac986dabc5a24adab11d48a4bccf4cb1";
    public static readonly string Det_Diagonal = "d9632631a18849539333a92332895ebd";
    public static readonly string Det_Vertical = "b67ffe82c66742d1985e5888fd8e6a03";
    public static readonly string Det_Horizontal = "1898f6fe1ee0408e886aaf05c23cc216";
    public static readonly string X_Det = "48d74b9c65f44b888a94f9e093554977";
    public static readonly string X_Det_Diagonal = "c5a0fd2774b64287bf11127ca59dd8b4";
    public static readonly string X_Det_Vertical = "abd816b0bcbf4035b95837ca931169df";
    public static readonly string X_Det_Horizontal = "07d06d2b23cc48fe9f95454c839cb361";


    //Buff/Support Enemies
    public static readonly string Gunsinger = "cf2b7021eac44e3f95af07db9a7c442c";
    public static readonly string Aged_Gunsinger = "c50a862d19fc4d30baeba54795e8cb93";
    public static readonly string Ammomancer = "b1540990a4f1480bbcb3bea70d67f60d";
    public static readonly string Jammomancer = "8b4a938cdbc64e64822e841e482ba3d2";
    public static readonly string Jamerlengo = "ba657723b2904aa79f9e51bce7d23872";

    //Humans
    public static readonly string Gun_Cultist = "57255ed50ee24794b7aac1ac3cfb8a95";
    public static readonly string Robots_Past_Human_1 = "1398aaccb26d42f3b998c367b7800b85";
    public static readonly string Robots_Past_Human_2 = "9044d8e4431f490196ba697927a4e3d4";
    public static readonly string Robots_Past_Human_3 = "40bf8b0d97794a26b8c440425ec8cac1";
    public static readonly string Robots_Past_Human_4 = "3590db6c4eac474a93299a908cb77ab2";
    public static readonly string Dr_Wolf = "ce2d2a0dced0444fb751b262ec6af08a";

    //Misc Robots
    public static readonly string Gat = "9b4fb8a2a60a457f90dcf285d34143ac";
    public static readonly string Grey_Cylinder = "d4f4405e0ff34ab483966fd177f2ece3";
    public static readonly string Red_Cylinder = "534f1159e7cf4f6aa00aeea92459065e";
    public static readonly string Bullet_Mech = "2b6854c0849b4b8fb98eb15519d7db1c";

    //Misc Explosive Kin
    public static readonly string Pinhead = "4d37ce3d666b4ddda8039929225b7ede";
    public static readonly string Nitra = "c0260c286c8d4538a697c5bf24976ccf";
    public static readonly string Bombshee = "19b420dec96d4e9ea4aebc3398c0ba7a";
    public static readonly string M80_Guy = "5f15093e6f684f4fb09d3e7e697216b4";

    //Muzzle Wisps
    public static readonly string Muzzle_Wisp = "ffdc8680bdaa487f8f31995539f74265";
    public static readonly string Muzzle_Flare = "d8a445ea4d944cc1b55a40f22821ae69";

    //Misc Small Enemies
    public static readonly string Rubber_Kin = "6b7ef9e5d05b4f96b04f05ef4a0d1b18";
    public static readonly string Tazie = "98fdf153a4dd4d51bf0bafe43f3c77ff";
    public static readonly string Sniper_Shell = "31a3ea0c54a745e182e22ea54844a82d";
    public static readonly string Professional = "c5b11bfc065d417b9c4d03a5e385fe2c";
    public static readonly string Wizbang = "43426a2e39584871b287ac31df04b544";
    public static readonly string Coaler = "9d50684ce2c044e880878e86dbada919";
    public static readonly string Fungun = "f905765488874846b7ff257ff81d6d0c";
    public static readonly string Bullet_Shark = "72d2f44431da43b8a3bae7d8a114a46d";
    public static readonly string Arrowkin = "05891b158cd542b1a5f3df30fb67a7ff";
    public static readonly string Gun_Fairy = "c182a5cb704d460d9d099a47af49c913";
    public static readonly string Musketball_Guy = "226fd90be3a64958a5b13cb0a4f43e97";
    public static readonly string Cactus = "3b0bd258b4c9432db3339665cc61c356";
    public static readonly string Candle_Guy = "37de0df92697431baa47894a075ba7e9";

    //Aggressive Animals
    public static readonly string Gigi = "ed37fa13e0fa4fcf8239643957c51293";
    public static readonly string Misfire_Beast = "45192ff6d6cb43ed8f1a874ab6bef316";
    public static readonly string Phaser_Spider = "98ca70157c364750a60f5e0084f9d3e2";
    public static readonly string Gunzookie = "6e972cd3b11e4b429b888b488e308551";
    public static readonly string Gunzockie = "8a9e9bedac014a829a48735da6daf3da";
    public static readonly string Chameleon = "80ab6cd15bfc46668a8844b2975c6c26";
    public static readonly string Parrot = "4b21a913e8c54056bc05cafecf9da880";
    public static readonly string Snake_Enemy = "e861e59012954ab2b9b6977da85cb83c";

    //Misc Mutants / Weirdos
    public static readonly string Kalibullet = "ff4f54ce606e4604bf8d467c1279be3e";
    public static readonly string K_Bullet = "f020570a42164e2699dcf57cac8a495c";
    public static readonly string Bullet_Fish_Blue = "06f5623a351c4f28bc8c6cda56004b80";
    public static readonly string Bullet_Fish_Green = "143be8c9bbb84e3fb3ab98bcd4cf5e5b";

    //Misc Strong Enemies
    public static readonly string Tarnisher = "475c20c1fd474dfbad54954e7cba29c1";
    public static readonly string Agonizer = "3f6d6b0c4a7c4690807435c7b37c35a5";
    public static readonly string Lead_Maiden = "cd4a4b7f612a4ba9a720b9f97c52f38c";
    public static readonly string Gripmaster = "22fc2c2c45fb47cf9fb5f7b043a70122";
    public static readonly string Shambling_Round = "98ea2fe181ab4323ab6e9981955a9bca";
    public static readonly string Great_Bullet_Shark = "b70cbd875fea498aa7fd14b970248920";
    public static readonly string Killithid = "3e98ccecf7334ff2800188c417e67c15";
    public static readonly string Dead_Blow_Dummy = "a38e9dca103c4e4fa4bf478cf9a2f2de";
    public static readonly string Spogre = "eed5addcc15148179f300cc0d9ee7f94";
    public static readonly string Fridge_Maiden = "9215d1a221904c7386b481a171e52859";
    public static readonly string Titan_Bullet_Kin = "c4cf0620f71c4678bb8d77929fd4feff";
    public static readonly string Titan_Bullet_Kin_Boss = "1f290ea06a4c416cabc52d6b3cf47266";
    public static readonly string Titaness_Bullet_Kin_Boss = "df4e9fedb8764b5a876517431ca67b86";

    //Misc Boss Minions
    public static readonly string Tanker_Summoned = "47bdfec22e8e4568a619130a267eab5b";
    public static readonly string Beadie = "7b0b1b6d9ce7405b86b75ce648025dd6";
    public static readonly string Tombstoner = "cf27dd464a504a428d87a8b2560ad40a";
    public static readonly string Ammoconda_Ball = "f38686671d524feda75261e469f30e0b";
    public static readonly string Mine_Flayer_Bell = "78a8ee40dff2477e9c2134f6990ef297";
    public static readonly string Mine_Flayer_Claymore = "566ecca5f3b04945ac6ce1f26dedbf4f";
    public static readonly string High_Priest_Candle = "eeb33c3a5a8e4eaaaaf39a743e8767bc";
    public static readonly string Chancellor = "b5e699a0abb94666bda567ab23bd91c4";
    public static readonly string Chancellor_Revenge = "d4dd2b2bbda64cc9bcec534b4e920518";
    public static readonly string Old_Chancellor = "02a14dec58ab45fb8aacde7aacd25b01";
    public static readonly string Bomb_Bot = "4538456236f64ea79f483784370bc62f";
    public static readonly string Mouser = "be0683affb0e41bbb699cb7125fdded6";
    public static readonly string Dragun_Knife = "78eca975263d4482a4bfa4c07b32e252";
    public static readonly string Advanced_Dragun_Knife = "2e6223e42e574775b56c6349921f42cb";
    public static readonly string Imp = "a9cc6a4e9b3d46ea871e70a03c9f77d4";
    public static readonly string Hegemony_Soldier = "556e9f2a10f9411cb9dbfd61e0e0f1e1";
    public static readonly string Terminator = "12a054b8a6e549dcac58a82b89e319e5";

    //Minibosses
    public static readonly string Ser_Manuel = "fc809bd43a4d41738a62d7565456622c";
    public static readonly string Blockner = "bb73eeeb9e584fbeaf35877ec176de28";
    public static readonly string Blockners_Ghost = "edc61b105ddd4ce18302b82efdc47178";
    public static readonly string Fuselier = "39de9bd6a863451a97906d949c103538";
    public static readonly string Shadow_Magician = "db97e486ef02425280129e1e27c33118";

    //Bosses
    public static readonly string Smiley = "ea40fcc863d34b0088f490f4e57f8913";
    public static readonly string Shades = "c00390483f394a849c36143eb878998f";
    public static readonly string Gatling_Gull = "ec6b674e0acd4553b47ee94493d66422";
    public static readonly string Bullet_King = "ffca09398635467da3b1f4a54bcfda80";
    public static readonly string Blobulord = "1b5810fafbec445d89921a4efb4e42b7";
    public static readonly string Beholster = "4b992de5b4274168a8878ef9bf7ea36b";
    public static readonly string Gorgun = "c367f00240a64d5d9f3c26484dc35833";
    public static readonly string Ammoconda = "da797878d215453abba824ff902e21b4";
    public static readonly string Old_King = "5729c8b5ffa7415bb3d01205663a33ef";
    public static readonly string Treadnaught = "fa76c8cfdf1c4a88b55173666b4bc7fb";
    public static readonly string Mine_Flayer = "8b0dd96e2fe74ec7bebc1bc689c0008a";
    public static readonly string Cannonbalrog = "5e0af7f7d9de4755a68d2fd3bbc15df4";
    public static readonly string Door_Lord = "9189f46c47564ed588b9108965f975c9";
    public static readonly string Resourceful_Rat = "6868795625bd46f3ae3e4377adce288b";
    public static readonly string Resourceful_Rat_Mech = "4d164ba3f62648809a4a82c90fc22cae";
    public static readonly string High_Priest = "6c43fddfd401456c916089fdd1c99b1c";
    public static readonly string Kill_Pillars = "3f11bbbc439c4086a180eb0fb9990cb4";
    public static readonly string Wallmonger = "f3b04a067a65492f8b279130323b41f0";
    public static readonly string Helicopter_Agunim = "41ee1c8538e8474a82a74c4aff99c712";
    public static readonly string Dragun = "465da2bb086a4a88a803f79fe3a27677";
    public static readonly string Advanced_Dragun = "05b8afe0b6cc4fffa9dc6036fa24c8ec";
    public static readonly string Lich = "cd88c3ce60c442e9aa5b3904d31652bc";
    public static readonly string Lich_Phase_2 = "68a238ed6a82467ea85474c595c49c6e";
    public static readonly string Lich_Phase_3 = "7c5d5f09911e49b78ae644d2b50ff3bf";

    //Past Bosses
    public static readonly string Dr_Wolfs_Monster = "8d441ad4e9924d91b6070d5b3438d066";
    public static readonly string Black_Stache = "8b913eea3d174184be1af362d441910d";
    public static readonly string HM_Absolution = "b98b10fca77d469e80fb45f3c5badec5";
    public static readonly string Interdimensional_Horror = "dc3cd41623d447aeba77c77c99598426";
    public static readonly string Last_Human = "880bbe4ce1014740ba6b4e2ea521e49d";
    public static readonly string Agunim = "2ccaa1b7ae10457396a1796decda9cf6";
    public static readonly string Cannon = "39dca963ae2b4688b016089d926308ab";

    //Unused Bosses
    public static readonly string Bunker = "8817ab9de885424d9ba83455ead5ffef";
    public static readonly string Vampire_Mantis = "575a37abca8d414d89c4e251dd606050";
    public static readonly string Bullet_Bishop = "5d045744405d4438b371eb5ed3e2cdb2";
    public static readonly string Boss_Template = "7ee0a3fbb3dc417db4c3073ba23e1be8";
    public static readonly string Shopkeeper_Boss = "70058857b0a641a888ac4389bd10f455";

    //Weird Unused/Test Entities
    public static readonly string Bullet_Kin_Fakery = "b5503988e3684e8fa78274dd0dda0bf5";
    public static readonly string Veteran_Bullet_Kin_Fakery = "06a82532447247f9ada1940d079a31a7";
    public static readonly string Unused_Muzzle_Wisp = "98e52539e1964749a8bbce0fe6a85d6b";
    public static readonly string Test_Dummy = "5fa8c86a65234b538cd022f726af2aea";

    //Passive Critters
    public static readonly string Chicken = "76bc43539fc24648bff4568c75c686d1";
    public static readonly string Poopulon_Corn = "0ff278534abb4fbaaa65d3f638003648";
    public static readonly string Snake_Harmless = "1386da0f42fb4bcabc5be8feb16a7c38";
    public static readonly string Blobulord_Death_Crawler = "d1c9781fdac54d9e8498ed89210a0238";
    public static readonly string Chick = "95ea1a31fc9e4415a5f271b9aedf9b15";
    public static readonly string Bunny = "42432592685e47c9941e339879379d3a";
    public static readonly string Squirrel = "4254a93fc3c84c0dbe0a8f0dddf48a5a";
    public static readonly string Rat = "6ad1cafc268f4214a101dca7af61bc91";
    public static readonly string Candle_Rat = "14ea47ff46b54bb4a98f91ffcffb656d";
    public static readonly string Gudetama_Blob = "8b43a5c59b854eb780f9ab669ec26b7a";

    //Companions
    public static readonly string Friendly_Bullet_Kin = "c1757107b9a44f0c823a707adeb4ae7e";
    public static readonly string Portable_Turret = "998807b57e454f00a63d67883fcf90d6";
    public static readonly string Super_Space_Turtle = "3a077fa5872d462196bb9a3cb1af02a3";
    public static readonly string Dog = "c07ef60ae32b404f99e294a6f9acba75";
    public static readonly string Wolf = "ededff1deaf3430eaf8321d0c6b2bd80";
    public static readonly string R2G2 = "1ccdace06ebd42dc984d46cb1f0db6cf";
    public static readonly string Ticket_Gatling_Gull = "538669d3b2cd4edca2e3699812bcf2b6";
    public static readonly string Cop = "705e9081261446039e1ed9ff16905d04";
    public static readonly string Caterpillar = "d375913a61d1465f8e4ffcf4894e4427";
    public static readonly string Chicken_Flute_Chicken = "7bd9c670f35b4b8d84280f52a5cc47f6";
    public static readonly string Raccoon = "e9fa6544000942a79ad05b6e4afb62db";
    public static readonly string Baby_Good_Mimic = "e456b66ed3664a4cb590eab3a8ff3814";
    public static readonly string Pig = "fe51c83b41ce4a46b42f54ab5f31e6d0";
    public static readonly string Blank_Companion = "5695e8ffa77c4d099b4d9bd9536ff35e";
    public static readonly string Dog_Pup = "ebf2314289ff4a4ead7ea7ef363a0a2e";
    public static readonly string Wolf_Pup = "ab4a779d6e8f429baafa4bf9e5dca3a9";
    public static readonly string Super_Space_Turtle_DX = "9216803e9c894002a4b931d7ea9c6bdf";
    public static readonly string Turtle = "cc9c41aa8c194e17b44ac45f993dd212";
    public static readonly string Payday_Companion_1 = "45f5291a60724067bd3ccde50f65ac22";
    public static readonly string Payday_Companion_2 = "41ab10778daf4d3692e2bc4b370ab037";
    public static readonly string Payday_Companion_3 = "2976522ec560460c889d11bb54fbe758";
    public static readonly string Turkey = "6f9c28403d3248c188c391f5e40774c5";
    public static readonly string Ser_Junkan = "c6c8e59d0f5d41969c74e802c9d67d07";
    public static readonly string Phoenix = "11a14dbd807e432985a89f69b5f9b31e";
    public static readonly string Pig_Cannon = "86237c6482754cd29819c239403a2de7";
    public static readonly string Elder_Blank_Companion = "ad35abc5a3bf451581c3530417d89f2c";
    public static readonly string Cop_Android = "640238ba85dd4e94b3d6f68888e6ecb8";
    public static readonly string Baby_Good_Shelleton = "3f40178e10dc4094a1565cd4fdc4af56";

    //Tutorial Turrets
    public static readonly string Tutorial_Turret = "e667fdd01f1e43349c03a18e5b79e579";
    public static readonly string Tutorial_Turret_Fast = "41ba74c517534f02a62f2e2028395c58";

    public static readonly List<string> CommonEnemies = new List<string>()
    {
        Bullet_Kin, Bullet_Kin_AK47, Bandana_Bullet_Kin, Veteran_Bullet_Kin, Tanker, Minelet, Cardinal, Shroomer, Ashen_Bullet_Kin, Mutant_Bullet_Kin, Fallen_Bullet_Kin,
        Chance_Kin, Keybullet_Kin, Confirmed, Red_Shotgun_Kin, Blue_Shotgun_Kin, Veteran_Shotgun_Kin, Mutant_Shotgun_Kin, Executioner, Ashen_Shotgun_Kin, Shotgrub, Sniper_Shell, Professional,
        Gummy, Skullet, Skullmet, Creech, Hollowpoint, Bombshee, Rubber_Kin, Tazie, King_Bullat, Pinhead, Nitra, Arrowkin, Blobulon, Blobuloid, Blobulin, Poisbulon, Poisbuloid, Poisbulin, Blizzbulon,
        Leadbulon, Poopulon, Bloodbulon, Skusket, Bookllet, Blue_Bookllet, Green_Bookllet, Gigi, Muzzle_Flare, Muzzle_Wisp, Cubulon, Chancebulon, Cubulead, Apprentice_Gunjurer, Gunjurer, High_Gunjurer,
        Lore_Gunjurer, Gunsinger, Aged_Gunsinger, Ammomancer, Jammomancer, Jamerlengo, Wizbang, Gun_Fairy, Bullat, Shotgat, Grenat, Spirat, Coaler, Gat, Det, X_Det, Det_Diagonal, Det_Horizontal,
        Det_Vertical, X_Det_Horizontal, X_Det_Vertical, X_Det_Diagonal, Gunzookie, Gunzockie, Bullet_Shark, Great_Bullet_Shark, Tombstoner, Gun_Cultist, Beadie, Gun_Nut, Spectral_Gun_Nut, Chain_Gunner,
        Fungun, Spogre, Mountain_Cube, Flesh_Cube, Lead_Cube, Lead_Maiden, Misfire_Beast, Phaser_Spider, Killithid, Tarnisher, Shambling_Round, Shelleton, Agonizer, Revolvenant, Gunreaper, Spent
    };
    public static readonly List<string> RNGDeptEnemies = new List<string>()
    {
        Candle_Guy, Office_Bullette_Kin, Office_Bullet_Kin, Brollet, Western_Bullet_Kin, Pirate_Bullet_Kin, Knight_Bullet_Kin, Western_Shotgun_Kin, Pirate_Shotgun_Kin, Bullat_Gargoyle, Angry_Necronomicon,
        Angry_Tablet, Grey_Cylinder, Red_Cylinder, Bullet_Mech, M80_Guy, Cactus, Musketball_Guy, Parrot, Snake_Enemy, Kalibullet, K_Bullet, Bullet_Fish_Blue, Bullet_Fish_Green, Fridge_Maiden,
        Titan_Bullet_Kin, Titan_Bullet_Kin_Boss, Titaness_Bullet_Kin_Boss,
    };
    public static readonly List<string> WeirdEnemies = new List<string>()
    {
        Tanker_Summoned,  Mine_Flayer_Bell, Mine_Flayer_Claymore, Bomb_Bot, Ammoconda_Ball, Chancellor, Chancellor_Revenge, Old_Chancellor, Dragun_Knife, Advanced_Dragun_Knife, Large_Spent
    };
    public static readonly List<string> PastEnemies = new List<string>()
    {
        Terminator, Hegemony_Soldier, Imp
    };
    public static readonly List<string> HarmlessEntities = new List<string>()
    {
        Chicken, Rat, Candle_Rat, Chick, Squirrel, Bunny, Gudetama_Blob, Poopulon_Corn, Blobulord_Death_Crawler, Snake_Harmless
    };
    public static readonly List<string> UnusedEnemies = new List<string>()
    {
        Skusket_Head, Black_Skusket, Spectre, Robots_Past_Human_1, Robots_Past_Human_2, Robots_Past_Human_3, Robots_Past_Human_4,
    };
    public static readonly List<string> AllDetGUIDs = new List<string>()
    {
        Det, Det_Diagonal, Det_Horizontal, Det_Vertical, X_Det, X_Det_Diagonal, X_Det_Horizontal, X_Det_Vertical
    };

    public static List<string> GenerateChaosPalette(bool includeWeirdEnemies = true, bool includeRNGDept = true, bool includePastEnemies = true, bool includeHarmlessEntities = true, bool includeUnusedEnemies = true, bool includeModdedEnemies = true)
    {
        bool ExpandInstalled = BepInEx.Bootstrap.Chainloader.PluginInfos.ContainsKey("ApacheThunder.etg.ExpandTheGungeon");
        bool FaGFInstalled = BepInEx.Bootstrap.Chainloader.PluginInfos.ContainsKey("kp.etg.frostandgunfire");
        bool PlanetsideInstalled = BepInEx.Bootstrap.Chainloader.PluginInfos.ContainsKey("somebunny.etg.planetsideofgunymede");
        bool ModularInstalled = BepInEx.Bootstrap.Chainloader.PluginInfos.ContainsKey("somebunny.etg.modularcharacter");

        List<string> finalList = new List<string>();
        finalList.AddRange(CommonEnemies);
        finalList.AddRange(OMITB.CommonEnemies);
        if (includeModdedEnemies)
        {
            if (ExpandInstalled) finalList.AddRange(Expand.CommonEnemies);
            if (FaGFInstalled) finalList.AddRange(FrostAndGunfire.CommonEnemies);
            if (PlanetsideInstalled) finalList.AddRange(Planetside.CommonEnemies);
        }

        if (includeWeirdEnemies)
        {
            finalList.AddRange(WeirdEnemies);
            if (includeModdedEnemies)
            {
                if (ExpandInstalled) finalList.AddRange(Expand.WeirdEnemies);
                if (PlanetsideInstalled) finalList.AddRange(Planetside.WeirdEnemies);
            }
        }
        if (includeRNGDept)
        {
            finalList.AddRange(RNGDeptEnemies);
        }
        if (includePastEnemies)
        {
            finalList.AddRange(PastEnemies);
            if (includeModdedEnemies)
            {
                if (ModularInstalled) finalList.AddRange(Modular.PastEnemies);
            }
        }
        if (includeHarmlessEntities)
        {
            finalList.AddRange(HarmlessEntities);
        }
        if (includeUnusedEnemies)
        {
            finalList.AddRange(UnusedEnemies);
            if (includeModdedEnemies)
            {
                if (FaGFInstalled) finalList.AddRange(FrostAndGunfire.UnusedEnemies);
                if (PlanetsideInstalled) finalList.AddRange(Planetside.UnusedEnemies);
            }
        }
        return finalList;
    }

    public static List<string> CurrentFloorEnemyPalette = new List<string>();
    public static void RegenerateCurrentFloorEnemyPalette(bool canReturnMimics = false, bool canReturnBosses = false, bool doLogging = false)
    {
        CurrentFloorEnemyPalette.Clear();
        if (GameManager.Instance.Dungeon)
        {
            if (StaticReferenceManager.AllEnemies != null)
            {
                foreach (AIActor enemy in StaticReferenceManager.AllEnemies)
                {
                    if (EnemyIsValid(enemy.EnemyGuid, canReturnMimics, canReturnBosses) && !CurrentFloorEnemyPalette.Contains(enemy.EnemyGuid)) { CurrentFloorEnemyPalette.Add(enemy.EnemyGuid); }
                }
            }
            if (GameManager.Instance.Dungeon.data != null && GameManager.Instance.Dungeon.data.rooms != null)
            {
                foreach (RoomHandler room in GameManager.Instance.Dungeon.data.rooms)
                {
                    if (room.remainingReinforcementLayers != null)
                    {
                        foreach (PrototypeRoomObjectLayer layer in room.remainingReinforcementLayers)
                        {
                            if (layer != null && layer.placedObjects != null)
                            {
                                foreach (PrototypePlacedObjectData objData in layer.placedObjects)
                                {
                                    if (objData != null)
                                    {
                                        if (objData.unspecifiedContents != null)
                                        {
                                            if (objData.unspecifiedContents.GetComponent<AIActor>() != null && !string.IsNullOrEmpty(objData.unspecifiedContents.GetComponent<AIActor>().EnemyGuid))
                                            {
                                                string enGUID = objData.unspecifiedContents.GetComponent<AIActor>().EnemyGuid;
                                                if (EnemyIsValid(enGUID, canReturnMimics,canReturnBosses) && !CurrentFloorEnemyPalette.Contains(enGUID))
                                                {
                                                    CurrentFloorEnemyPalette.Add(enGUID);
                                                }
                                            }
                                        }
                                        else if (objData.placeableContents != null)
                                        {
                                            foreach (DungeonPlaceableVariant variantTier in objData.placeableContents.variantTiers)
                                            {
                                                if (variantTier.enemyPlaceableGuid != null && EnemyIsValid(variantTier.enemyPlaceableGuid, canReturnMimics, canReturnBosses) && !CurrentFloorEnemyPalette.Contains(variantTier.enemyPlaceableGuid))
                                                {
                                                    CurrentFloorEnemyPalette.Add(variantTier.enemyPlaceableGuid);
                                                }
  
                                            }
                                        }
                                    }                                 
                                }
                            }
                        }
                    }
                }
            }
        }
    }
    private static bool EnemyIsValid(string enemyGUID, bool canReturnMimic, bool canReturnBoss)
    {
        if (enemyGUID != null)
        {
            AIActor enemy = EnemyDatabase.GetOrLoadByGuid(enemyGUID);
            if (enemy)
            {
                AIActor realEnemy = enemy;
                if (enemy is AIActorDummy)
                {
                    if ((enemy as AIActorDummy).realPrefab.GetComponent<AIActor>() != null)
                    {
                        realEnemy = (enemy as AIActorDummy).realPrefab.GetComponent<AIActor>();
                    }
                }
                if ((!canReturnMimic && !realEnemy.IsMimicEnemy) || canReturnMimic)
                {
                    if ((!canReturnBoss && realEnemy.healthHaver && !realEnemy.healthHaver.IsBoss) || canReturnBoss)
                    {
                        return true;
                    }
                }
            }
        }
        return false;
    }

    public static class OMITB
    {
        //Debug Enemies
        public static readonly string De_Bug = "omitb_debug_lkjhgfdgglkjhgfdggggggmnbvcxaaassd";

        //Actual Enemies
        public static readonly string Deacon = "omitb_deacon_567890983hruiwfh8auiqy387fhwoe719h4rutijouqiy";
        public static readonly string Bouncer_Bullet_Kin = "omitb_bouncerbulletkin_iopupkkppoughihuhomomoarfdar";
        public static readonly string Molotovnik = "omitb_molotovkin_ppppppkdhfyuuietyeuigyeuioweiywuywu";
        public static readonly string Muskin = "omitb_muskin_yee338q7881ooo1837467iuuueee";
        public static readonly string Chasser = "omitb_chasser";

        public static readonly List<string> CommonEnemies = new List<string>()
        {
            Deacon, Bouncer_Bullet_Kin, Molotovnik, Muskin, Chasser
        };
    }
    public static class Expand
    {
        //Hotshot Enemies
        public static readonly string Hotshot_Bullet_Kin = "8a0b7a287410464bb17b9e656958bd19";
        public static readonly string Hotshot_Shotgun_Kin = "758a0a0215e6448ab52adf73bc44ae5e";
        public static readonly string Hotshot_Cultist = "61a8112544ce4389ab14f2287616a71b";

        //Bootlegs
        public static readonly string Bootleg_Bullat = "7ef020b9-11fb-4a24-a818-60581e6d3105";
        public static readonly string Bootleg_Bullet_Kin = "a52cfba8-f141-4a98-9022-48816201f834";
        public static readonly string Bootleg_Bandana_Bullet_Kin = "7093253e-a118-4813-8feb-703a1ad31665";
        public static readonly string Bootleg_Red_Shotgun_Kin = "01e4e238-89bb-4b30-b93a-ae17dc19e748";
        public static readonly string Bootleg_Blue_Shotgun_Kin = "f7c0b0ab-3d80-4855-9fd6-38861af1147a";

        //Other Enemies
        public static readonly string Grenade_Rat = "1a1dc5ed-92a6-4bd1-bbee-098991e7d2d4";
        public static readonly string Cronenberg = "0a2433d6e0784eefb28762c5c127d0b3";
        public static readonly string Aggressive_Cronenberg = "6d2d7a845e464d3ca453fe1fff5fed84";
        public static readonly string Western_Cube = "c1e60b8c0691499183c69393e02c9de9";
        public static readonly string Corrupted_Enemy = "182c39c4d904493283f75ab29775d9c6";
        public static readonly string Clown_Kin = "5736cc6185294b839666c65ac8e082c1";
        public static readonly string Clown_Kin_NoFX = "ccd416569b6d4ca0bb057a837a517d73";
        public static readonly string Clown_Kin_Angry = "3eee833068614536a5f56cbe7dc6cfe9";
        public static readonly string Poisbulord_Death_Crawler = "aa060bcd358048b8ac99127571e500ae";

        //Bosses
        public static readonly string Boss_Bullet_Kin = "170bad1ea59344278c996c4ccc3bee51";
        public static readonly string Doppelgunner = "5f0fa34b5a2e44cdab4a06f89bb5c442";
        public static readonly string Parasitic_Abomination = "acd8d483f24e4c43b964fa4e54068cf1";
        public static readonly string Com4nd0_Boss = "0a406e36-80eb-43b8-8ad0-c56232f9496e";
        public static readonly string Poisbulord = "b19ec5d13d754e5f8293910e10bf7ff1";

        //Companions
        public static readonly string Baby_Good_Hammer = "05145e1a-1a10-4797-b37e-a15bb26d7641";
        public static readonly string Cultist_Companion = "1d1e1070617842f09e6f45df3cb223f6";
        public static readonly string Sonic = "38e61aef773a481697c4956d85279087";
        public static readonly string Clown_Kin_Companion = "dd1505fb84744002ad42ee8316b86ea0";

        //Other
        public static readonly string Backrooms_Entity = "0108a031c74940739c56a22068c915b6";
        public static readonly string Explosive_Barrel_Dummy = "27638478e52e4785925b578b52bf79d3";

        public static readonly List<string> CommonEnemies = new List<string>()
        {
            Hotshot_Bullet_Kin, Hotshot_Shotgun_Kin, Hotshot_Cultist, Grenade_Rat
        };
        public static readonly List<string> WeirdEnemies = new List<string>()
        {
            Bootleg_Bullat, Bootleg_Bullet_Kin, Bootleg_Bandana_Bullet_Kin, Bootleg_Blue_Shotgun_Kin, Bootleg_Red_Shotgun_Kin, Cronenberg, Aggressive_Cronenberg, Clown_Kin, Poisbulord_Death_Crawler,
            Explosive_Barrel_Dummy, Backrooms_Entity, Clown_Kin_Angry, Clown_Kin_NoFX
        };
    }
    public static class Planetside
    {
        //Enemies
        public static readonly string Coallet = "coallet_psog";
        public static readonly string Detscavator = "detscavator";
        public static readonly string Trapper_Cube_Vertical = "proper_cube";
        public static readonly string Trapper_Cube_Horizontal = "proper_cubeleftRight";
        public static readonly string Shamber = "shamber_psog";
        public static readonly string Arch_Gunjurer = "arch_gunjurer";
        public static readonly string Barretina = "barretina";
        public static readonly string Cursebulon = "cursebulon";
        public static readonly string Fodder = "fodder_enemy";
        public static readonly string Glockulus = "glockulus";
        public static readonly string Skullvenant = "skullvenant";
        public static readonly string Wailer = "wailer";
        public static readonly string Snipeidolon = "psog:snipeidolon";
        public static readonly string Revenant = "PSOG_Revenant";

        //Forgotten Enemies
        public static readonly string Bloat = "bloat_isaac_reference";
        public static readonly string Collective = "collective";
        public static readonly string Creationist = "creationist";
        public static readonly string Inquisitor = "inquisitor";
        public static readonly string Observant = "observant";
        public static readonly string Oppressor = "oppressor_psog";
        public static readonly string Stagnant = "stagnant";
        public static readonly string Blockade = "the_tower_psog";
        public static readonly string Unwilling = "unwilling";
        public static readonly string Vessel = "vessel";

        //Bullet Banker Summons
        public static readonly string BB_An3s = "an3s_bullet";
        public static readonly string BB_Apache = "apache_bullet";
        public static readonly string BB_Bleak = "bleak_bullet";
        public static readonly string BB_NotABot = "bot_bullet";
        public static readonly string BB_SomeBunny = "bunny_bullet";
        public static readonly string BB_UnstableStrafe = "unstablestrafe_bullet";
        public static readonly string BB_Cortify = "cortify_bullet";
        public static readonly string BB_Dallan = "dallan_bullet";
        public static readonly string BB_Glaurung = "glaurung_bullet";
        public static readonly string BB_GoldenRevolver = "gr_bullet";
        public static readonly string BB_BlackHunter = "hunter_bullet";
        public static readonly string BB_June = "june_bullet";
        public static readonly string BB_RoundQueen = "king_bullet";
        public static readonly string BB_Kyle = "kyle_bullet";
        public static readonly string BB_Lynceus = "lynceus_bullet";
        public static readonly string BB_Neighborino = "neighborino_bullet";
        public static readonly string BB_Nevernamed = "nevernamed_bullet";
        public static readonly string BB_NotSoAI = "notsoai_bullet";
        public static readonly string BB_Panda = "panda_bullet";
        public static readonly string BB_Qaday = "qaday_bullet";
        public static readonly string BB_Retrash = "retrash_bullet";
        public static readonly string BB_Shotzer = "shotzer_bullet";
        public static readonly string BB_Skilotar = "skilotar_bullet";
        public static readonly string BB_Spapi = "spapi_bullet";
        public static readonly string BB_Spcreat = "spcreat_bullet";
        public static readonly string BB_TurboGTXS = "turbo_bullet";
        public static readonly string BB_SirWow = "wow_bullet";

        //Weird Enemies
        public static readonly string Jammed_Guardian = "jammed_guardian";
        public static readonly string Something_Wicked = "something_wicked";

        //Unused Enemies
        public static readonly string Deturret_Enemy = "deturret_enemy";
        public static readonly string Deturret_Enemy_Left = "deturretleft_enemy";

        //Minibosses
        public static readonly string Shellrax = "Shellrax";
        public static readonly string Annihi_Chamber = "annihichamber";
        public static readonly string Nemesis = "nemesis";
        public static readonly string HM_Prime = "psog:hm_prime";
        public static readonly string Bullet_Banker = "Bullet_Banker";

        //Bosses
        public static readonly string Fungannon = "Fungannon";
        public static readonly string Ophanaim = "Ophanaim";
        public static readonly string Prisoner = "Prisoner_Cloaked";
        public static readonly string Earthmover = "1000THR Earthmover";

        public static readonly List<string> CommonEnemies = new List<string>()
        {
            Coallet, Detscavator, Trapper_Cube_Horizontal, Trapper_Cube_Vertical, Shamber, Arch_Gunjurer, Barretina, Cursebulon, Glockulus, Fodder, Skullvenant, Wailer, Snipeidolon, Revenant
        };
        public static readonly List<string> WeirdEnemies = new List<string>()
        {
            Bloat, Collective, Creationist, Inquisitor, Observant, Oppressor, Stagnant, Blockade, Unwilling, Vessel, BB_An3s, BB_Apache, BB_Bleak, BB_NotABot, BB_SomeBunny, BB_UnstableStrafe, BB_Cortify,
            BB_Dallan, BB_Glaurung, BB_GoldenRevolver, BB_BlackHunter, BB_June, BB_RoundQueen, BB_Kyle, BB_Lynceus, BB_Neighborino, BB_Nevernamed, BB_NotSoAI, BB_Panda, BB_Qaday, BB_Retrash, BB_Shotzer,
            BB_Skilotar, BB_Spapi, BB_Spcreat, BB_TurboGTXS, BB_SirWow
        };
        public static readonly List<string> UnusedEnemies = new List<string>()
        {
           Deturret_Enemy, Deturret_Enemy_Left
        };
    }
    public static class FrostAndGunfire
    {
        //Blobulonians
        public static readonly string Bubbulon = "bubbulon";
        public static readonly string Globbulon = "globbulon";

        //Misc
        public static readonly string Cannon_Kin = "cannon";
        public static readonly string Suppressor = "silencer";
        public static readonly string Skell = "shellet";
        public static readonly string Salamander = "salamander";
        public static readonly string Mushboom = "mini mushboom";
        public static readonly string Ophaim = "ophaim";
        public static readonly string Firefly = "firefly";
        public static readonly string Spitfire = "spitfire";
        public static readonly string Gazer = "gazer";
        public static readonly string Observer = "observer";
        public static readonly string Gunzooka = "spitter";
        public static readonly string Suppores = "suppores";

        //Minibosses
        public static readonly string Room_Mimic = "Room Mimic";

        //Debug & Test Enemies
        public static readonly string Humphrey = "humphrey";
        public static readonly string Mushboom_Unfinished = "mushboom";

        public static readonly List<string> CommonEnemies = new List<string>()
        {
            Bubbulon, Globbulon, Cannon_Kin, Suppressor, Skell, Salamander, Mushboom, Ophaim, Firefly, Spitfire, Gazer, Observer, Gunzooka, Suppores
        };
        public static readonly List<string> UnusedEnemies = new List<string>()
        {
            Humphrey, Mushboom_Unfinished
        };
    }
    public static class Modular
    {
        //Enemies
        public static readonly string Big_Drone = "BigDrone_MDLR";
        public static readonly string Burster_Vertical = "Burster_MDLR_Vertical";
        public static readonly string Burster_Horizontal = "Burster_MDLR_Horizontal";
        public static readonly string Laser_Diode = "LaserDiode_MDLR";
        public static readonly string Node = "Node_MDLR";
        public static readonly string Nopticon_Four = "Nopticon_MDLR(4)";
        public static readonly string Nopticon_Eight = "Nopticon_MDLR(8)";
        public static readonly string Nopticon_Twelve = "Nopticon_MDLR(12)";
        public static readonly string Nopticon_Sixteen = "Nopticon_MDLR(16)";
        public static readonly string Mini_Mech_Robot = "RobotMiniMecha_MDLR";
        public static readonly string Sentry = "SentryTurret_MDLR";
        public static readonly string Slapper = "Slapper_MDLR";

        //Bosses
        public static readonly string Steel_Panopticon = "Steel_Panopticon_MDLR";
        public static readonly string Modular_Prime = "ModularPrime_MDLR";

        public static readonly List<string> PastEnemies = new List<string>()
        {
            Big_Drone, Burster_Horizontal, Burster_Vertical, Laser_Diode, Node, Nopticon_Four, Nopticon_Eight, Nopticon_Sixteen, Nopticon_Twelve, Mini_Mech_Robot, Sentry, Slapper
        };
    }

}

