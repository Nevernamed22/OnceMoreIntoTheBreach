﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SaveAPI
{
    public enum CustomDungeonFlags
    {
        //Add your custom flags here
        //You can remove any flags here (except NONE, don't remove it)
        NONE,
        //Non unlock related flags
        ALLJAMMEDMODE_ENABLED_GENUINE,
        ALLJAMMEDMODE_ENABLED_CONSOLE,
        CURSES_DISABLED,
        BOWLERSHOP_METONCE,
        CHANCELOT_METONCE,
        //---------------------------------------------Task Based Unlocks
        PLAYERHELDMORETHANFIVEARMOUR,
        KILLEDJAMMEDKEYBULLETKIN,
        KILLEDJAMMEDCHANCEKIN,
        KILLEDJAMMEDMIMIC,
        HASBEENDAMAGEDBYRISKRIFLE,
        FAILEDRATMAZE,
        USEDFALLENANGELSHRINE,
        KILLEDENEMYWITHTHROWNGUN,
        USED_FALSE_BLANK_TEN_TIMES,
        KILLED_TITAN_KIN,
        HAS_BEATEN_BOSS_BY_SKIN_OF_TEETH,
        ANGERED_BELLO,
        ROBOT_HELD_FIVE_JUNK,
        HURT_BY_SHROOMER,
        UNLOCKED_MISSINGUNO,
        FLOOR_CLEARED_WITH_CURSE,
        //Turbo
        BEATEN_KEEP_TURBO_MODE,
        BEATEN_MINES_BOSS_TURBO_MODE,
        BEATEN_RAT_BOSS_TURBO_MODE,
        BEATEN_HOLLOW_BOSS_TURBO_MODE,
        BEATEN_HELL_BOSS_TURBO_MODE,
        //Rainbow
        RAINBOW_KILLED_LICH,
        //All-Jammed
        ALLJAMMED_BEATEN_KEEP,
        ALLJAMMED_BEATEN_OUB,
        ALLJAMMED_BEATEN_PROPER,
        ALLJAMMED_BEATEN_ABBEY,
        ALLJAMMED_BEATEN_MINES,
        ALLJAMMED_BEATEN_RAT,
        ALLJAMMED_BEATEN_HOLLOW,
        ALLJAMMED_BEATEN_OFFICE,
        ALLJAMMED_BEATEN_FORGE,
        ALLJAMMED_BEATEN_HELL,
        ALLJAMMED_BEATEN_ADVANCEDDRAGUN,
        //Bossrush Unlocks
        BOSSRUSH_SHADE,
        BOSSRUSH_PARADOX,
        BOSSRUSH_CONVICT,
        BOSSRUSH_PILOT,
        BOSSRUSH_GUNSLINGER,
        BOSSRUSH_HUNTER,
        BOSSRUSH_MARINE,
        BOSSRUSH_ROBOT,
        BOSSRUSH_BULLET,
        //Rat Unlocks
        RAT_KILLED_PILOT,
        RAT_KILLED_SHADE,
        RAT_KILLED_ROBOT,
        RAT_KILLED_BULLET,
        //Dragun Unlocks
        DRAGUN_KILLED_HUNTER,
        DRAGUN_KILLED_SHADE,
        //Advanced Dragun Unlocks
        ADVDRAGUN_KILLED_ROBOT,
        ADVDRAGUN_KILLED_BULLET,
        ADVDRAGUN_KILLED_SHADE,
        //Challenges
        CHALLENGE_WHATARMY_BEATEN,
        CHALLENGE_TOILANDTROUBLE_BEATEN,
        CHALLENGE_INVISIBLEO_BEATEN,
        CHALLENGE_KEEPITCOOL_BEATEN,
        //Shade  Unlocks
        LICH_BEATEN_SHADE,
        CHEATED_DEATH_SHADE,
        //---------------------------------------------Beggar
        GIVEN_SPITBALLER,
        GIVEN_SCRAPSTRAP,
        GIVEN_FLAMINGSHELLS,
        GIVEN_SHELLNECKLACE,
        GIVEN_UNDERBARRELSHOTGUN,
        GIVEN_WOODENKNIFE,
        GIVEN_GUNGINEER,
        GIVEN_SHROOMEDBULLETS,
        GIVEN_RINGOFFORTUNE,
        GIVEN_BEGGARSBELIEF,
        //---------------------------------------------Hunting Quests
        //Quest Completion
        MISFIREBEAST_QUEST_COMPLETE,
        NITRA_QUEST_COMPLETE,
        GUNCULTIST_QUEST_COMPLETE,
        PHASERSPIDER_QUEST_COMPLETE,
        JAMMEDBULLETKIN_QUEST_COMPLETE,
        JAMMEDSHOTGUNKIN_QUEST_COMPLETE,
        JAMMEDLEADMAIDEN_QUEST_COMPLETE,
        JAMMEDBULLETSHARK_QUEST_COMPLETE,
        JAMMEDGUNNUT_QUEST_COMPLETE,
        KEVIN_QUEST_COMPLETE,
        //Quest Rewarded
        MISFIREBEAST_QUEST_REWARDED,
        NITRA_QUEST_REWARDED,
        GUNCULTIST_QUEST_REWARDED,
        PHASERSPIDER_QUEST_REWARDED,
        JAMMEDBULLETKIN_QUEST_REWARDED,
        JAMMEDSHOTGUNKIN_QUEST_REWARDED,
        JAMMEDLEADMAIDEN_QUEST_REWARDED,
        JAMMEDBULLETSHARK_QUEST_REWARDED,
        JAMMEDGUNNUT_QUEST_REWARDED,
        KEVIN_QUEST_REWARDED,
        //---------------------------------------------Shop Based Dummy Unlocks
        //DOUG
        PURCHASED_LOCKDOWNBULLETS,
        PURCHASED_THETHINLINE,
        PURCHASED_NITROBULLETS,
        PURCHASED_KINAMMOLET,
        PURCHASED_SHUTDOWNSHELLS,
        PURCHASED_ERRORSHELLS,
        PURCHASED_MEATSHIELD,
        PURCHASED_MAGICMISSILE,
        PURCHASED_MINERSBULLETS,
        PURCHASED_RANDOROUNDS,
        PURCHASED_PESTIFEROUSLEAD,
        PURCHASED_THEOUTBREAK,
        PURCHASED_SHRINKSHOT,
        PURCHASED_BRONZEAMMOLET,
        PURCHASED_HEPATIZONAMMOLET,
        PURCHASED_HEMATICROUNDS,
        PURCHASED_BABYGOODDET,
        PURCHASED_NEUTRONIUMAMMOLET,
        PURCHASED_BATTERBULLETS,
        //TRORC
        PURCHASED_TRACERROUNDS,
        PURCHASED_BAZOOKA,
        PURCHASED_ANTIMATERIELRIFLE,
        PURCHASED_RIOTGUN,
        PURCHASED_UNENGRAVEDBULLETS,
        PURCHASED_SPRINGLOADEDCHAMBER,
        PURCHASED_GRENADESHOTGUN,
        PURCHASED_LOUDENBOOMER,
        PURCHASED_BORZ,
        PURCHASED_BORCHARDT,
        PURCHASED_RHEINMETOLE,
        //GOOPTON
        PURCHASED_GRACEFULGOOP,
        PURCHASED_GOOMPERORSCROWN,
        PURCHASED_VACUUMGUN,
        PURCHASED_ALKALIBULLETS,
        PURCHASED_VISCERIFLE,
        PURCHASED_LIQUIDMETALBODY,
        PURCHASED_LOVEPOTION,
        PURCHASED_SPEEDPOTION,
        PURCHASED_SANCTIFIEDOIL,
        PURCHASED_SNAILBULLETS,
        PURCHASED_SPORELAUNCHER,
        //CADENCE AND OX
        PURCHASED_RISKRIFLE,
        PURCHASED_CREDITOR,
        PURCHASED_OVERPRICEDHEADBAND,
        PURCHASED_POWERARMOUR,
        PURCHASED_RECYCLINDER,
        PURCHASED_BLASMASTER,
        PURCHASED_DARTRIFLE,
        PURCHASED_DEMOLITIONIST,
        PURCHASED_REPEATOVOLVER,
        PURCHASED_SPIRAL,
        PURCHASED_STUNGUN,
        PURCHASED_DRONE,
        PURCHASED_AUTOGUN,
        PURCHASED_REBONDIR,
        PURCHASED_CONVERTER,
    }
}
