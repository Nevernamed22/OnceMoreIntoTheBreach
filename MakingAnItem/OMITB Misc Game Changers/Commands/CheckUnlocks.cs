using SaveAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NevernamedsItems
{
    class CheckUnlocks
    {
        public static void Init()
        {
            ETGModConsole.Commands.GetGroup("nn").AddUnit("checkunlocks", delegate (string[] args)
            {
                ETGModConsole.Log("<color=#00d6e6>    ---------------   Item Unlocks in Once More Into The Breach    --------------    </color>");
                ETGModConsole.Log("Unlocks requiring the player to do X amount of things can be done across multiple runs unless otherwise specified.");

                //Armoured Armour
                if (SaveAPIManager.GetFlag(CustomDungeonFlags.PLAYERHELDMORETHANFIVEARMOUR))
                {
                    ETGModConsole.Log("Armoured Armour <color=#04eb00>[Unlocked]</color>!");
                }
                else { ETGModConsole.Log("Armoured Armour <color=#ff0000ff>[Locked]</color> - Hold 5 pieces of armour or more at once (11 as Robot)."); }

                //Keybullet Effigy
                if (SaveAPIManager.GetFlag(CustomDungeonFlags.KILLEDJAMMEDKEYBULLETKIN))
                {
                    ETGModConsole.Log("Keybullet Effigy <color=#04eb00>[Unlocked]</color>!");
                }
                else { ETGModConsole.Log("Keybullet Effigy <color=#ff0000ff>[Locked]</color> - Kill a Jammed Keybullet Kin."); }

                //Chance Effigy
                if (SaveAPIManager.GetFlag(CustomDungeonFlags.KILLEDJAMMEDCHANCEKIN))
                {
                    ETGModConsole.Log("Chance Effigy <color=#04eb00>[Unlocked]</color>!");
                }
                else { ETGModConsole.Log("Chance Effigy <color=#ff0000ff>[Locked]</color> - Kill a Jammed Chance Kin."); }

                //Bloody Box
                if (SaveAPIManager.GetFlag(CustomDungeonFlags.KILLEDJAMMEDMIMIC))
                {
                    ETGModConsole.Log("Bloody Box <color=#04eb00>[Unlocked]</color>!");
                }
                else { ETGModConsole.Log("Bloody Box <color=#ff0000ff>[Locked]</color> - Kill a Jammed Mimic."); }

                //Risky Ring
                if (SaveAPIManager.GetFlag(CustomDungeonFlags.HASBEENDAMAGEDBYRISKRIFLE))
                {
                    ETGModConsole.Log("Risky Ring <color=#04eb00>[Unlocked]</color>!");
                }
                else { ETGModConsole.Log("Risky Ring <color=#ff0000ff>[Locked]</color> - Take damage to the Risk Rifle."); }

                //Cheese Heart
                if (SaveAPIManager.GetFlag(CustomDungeonFlags.FAILEDRATMAZE))
                {
                    ETGModConsole.Log("Cheese Heart <color=#04eb00>[Unlocked]</color>!");
                }
                else { ETGModConsole.Log("Cheese Heart <color=#ff0000ff>[Locked]</color> - Take too many wrong turns in the Rat's maze."); }

                //Dagger of the Aimgels
                if (SaveAPIManager.GetFlag(CustomDungeonFlags.USEDFALLENANGELSHRINE))
                {
                    ETGModConsole.Log("Dagger of the Aimgels <color=#04eb00>[Unlocked]</color>!");
                }
                else { ETGModConsole.Log("Dagger of the Aimgels <color=#ff0000ff>[Locked]</color> - Use a fallen angel shrine."); }

                //Diamond Bracelet
                if (SaveAPIManager.GetFlag(CustomDungeonFlags.KILLEDENEMYWITHTHROWNGUN))
                {
                    ETGModConsole.Log("Diamond Bracelet <color=#04eb00>[Unlocked]</color>!");
                }
                else { ETGModConsole.Log("Diamond Bracelet <color=#ff0000ff>[Locked]</color> - Kill an enemy with a thrown gun."); }

                //True Blank
                if (SaveAPIManager.GetFlag(CustomDungeonFlags.USED_FALSE_BLANK_TEN_TIMES))
                {
                    ETGModConsole.Log("True Blank <color=#04eb00>[Unlocked]</color>!");
                }
                else { ETGModConsole.Log("True Blank <color=#ff0000ff>[Locked]</color> - Use the False Blank ten times in one run."); }

                //Titan Bullets
                if (SaveAPIManager.GetPlayerStatValue(CustomTrackedStats.TITAN_KIN_KILLED) >= 5)
                {
                    ETGModConsole.Log("Titan Bullets <color=#04eb00>[Unlocked]</color>!");
                }
                else { ETGModConsole.Log("Titan Bullets <color=#ff0000ff>[Locked]</color> - Kill 5 Titan Bullet Kin; [" + SaveAPIManager.GetPlayerStatValue(CustomTrackedStats.TITAN_KIN_KILLED) + "/5]"); }

                //Mutagen
                if (SaveAPIManager.GetFlag(CustomDungeonFlags.HAS_BEATEN_BOSS_BY_SKIN_OF_TEETH))
                {
                    ETGModConsole.Log("Mutagen <color=#04eb00>[Unlocked]</color>!");
                }
                else { ETGModConsole.Log("Mutagen <color=#ff0000ff>[Locked]</color> - Kill a boss while one hit from death."); }

                //Doggun
                if (SaveAPIManager.GetFlag(CustomDungeonFlags.KILLED_DRAGUN_WITH_DOG))
                {
                    ETGModConsole.Log("Doggun <color=#04eb00>[Unlocked]</color>!");
                }
                else { ETGModConsole.Log("Doggun <color=#ff0000ff>[Locked]</color> - Kill the Dragun with the Dog by your side."); }

                //The Beholster
                if (SaveAPIManager.GetPlayerStatValue(CustomTrackedStats.BEHOLSTER_KILLS) >= 15)
                {
                    ETGModConsole.Log("The Beholster <color=#04eb00>[Unlocked]</color>!");
                }
                else { ETGModConsole.Log("The Beholster <color=#ff0000ff>[Locked]</color> - Kill 15 Beholsters; [" + SaveAPIManager.GetPlayerStatValue(CustomTrackedStats.BEHOLSTER_KILLS) + "/15]"); }

                //Flayed Revovler
                if (SaveAPIManager.GetPlayerStatValue(CustomTrackedStats.MINEFLAYER_KILLS) >= 10)
                {
                    ETGModConsole.Log("Flayed Revolver <color=#04eb00>[Unlocked]</color>!");
                }
                else { ETGModConsole.Log("Flayed Revolver <color=#ff0000ff>[Locked]</color> - Kill 10 Mine Flayers; [" + SaveAPIManager.GetPlayerStatValue(CustomTrackedStats.MINEFLAYER_KILLS) + "/10]"); }

                //Justice
                if (SaveAPIManager.GetFlag(CustomDungeonFlags.ANGERED_BELLO))
                {
                    ETGModConsole.Log("Justice <color=#04eb00>[Unlocked]</color>!");
                }
                else { ETGModConsole.Log("Justice <color=#ff0000ff>[Locked]</color> - Make the Shopkeep very, very mad."); }

                //Orgun
                if (SaveAPIManager.GetPlayerMaximum(CustomTrackedMaximums.MAX_HEART_CONTAINERS_EVER) >= 8)
                {
                    ETGModConsole.Log("Orgun <color=#04eb00>[Unlocked]</color>!");
                }
                else { ETGModConsole.Log("Orgun <color=#ff0000ff>[Locked]</color> - Have more than 8 heart containers at once."); }

                //Junkllets
                if (SaveAPIManager.GetFlag(CustomDungeonFlags.ROBOT_HELD_FIVE_JUNK))
                {
                    ETGModConsole.Log("Junkllets <color=#04eb00>[Unlocked]</color>!");
                }
                else { ETGModConsole.Log("Junkllets <color=#ff0000ff>[Locked]</color> - Hold more than five junk at once as the Robot."); }

                //Kalibers Eye
                if (SaveAPIManager.GetPlayerStatValue(CustomTrackedStats.CHARMED_ENEMIES_KILLED) >= 100)
                {
                    ETGModConsole.Log("Kalibers Eye <color=#04eb00>[Unlocked]</color>!");
                }
                else { ETGModConsole.Log("Kalibers Eye <color=#ff0000ff>[Locked]</color> - Kill 100 Charmed Enemies; [" + SaveAPIManager.GetPlayerStatValue(CustomTrackedStats.CHARMED_ENEMIES_KILLED) + "/100]"); }

                //Shroomed Gun
                if (SaveAPIManager.GetFlag(CustomDungeonFlags.HURT_BY_SHROOMER))
                {
                    ETGModConsole.Log("Shroomed Gun <color=#04eb00>[Unlocked]</color>!");
                }
                else { ETGModConsole.Log("Shroomed Gun <color=#ff0000ff>[Locked]</color> - Take damage to a Shroomer."); }

                //Missinguno
                if (SaveAPIManager.GetFlag(CustomDungeonFlags.UNLOCKED_MISSINGUNO))
                {
                    ETGModConsole.Log("Missinguno <color=#04eb00>[Unlocked]</color>!");
                }
                else { ETGModConsole.Log("Missinguno <color=#ff0000ff>[Locked]</color> - Open a Glitched Chest OR kill the Lich as the Paradox."); }

                //Holey Water
                if (SaveAPIManager.GetFlag(CustomDungeonFlags.FLOOR_CLEARED_WITH_CURSE))
                {
                    ETGModConsole.Log("Holey Water <color=#04eb00>[Unlocked]</color>!");
                }
                else { ETGModConsole.Log("Holey Water <color=#ff0000ff>[Locked]</color> - Clear a floor with an active... special curse."); }

                //Cursed Tumbler
                if (SaveAPIManager.GetPlayerStatValue(CustomTrackedStats.JAMMED_CHESTS_OPENED) >= 1)
                {
                    ETGModConsole.Log("Cursed Tumbler <color=#04eb00>[Unlocked]</color>!");
                }
                else { ETGModConsole.Log("Cursed Tumbler <color=#ff0000ff>[Locked]</color> - Open a Jammed Chest in All-Jammed mode."); }

                //Rusty Casing
                if (SaveAPIManager.GetPlayerStatValue(CustomTrackedStats.RUSTY_ITEMS_PURCHASED) >= 3)
                {
                    ETGModConsole.Log("Rusty Casing <color=#04eb00>[Unlocked]</color>!");
                }
                else { ETGModConsole.Log($"Rusty Casing <color=#ff0000ff>[Locked]</color> - Purchase 3 items from Rusty; [{SaveAPIManager.GetPlayerStatValue(CustomTrackedStats.RUSTY_ITEMS_PURCHASED)}/3]"); }

                //Rusty Shotgun
                if (SaveAPIManager.GetPlayerStatValue(CustomTrackedStats.RUSTY_ITEMS_STOLEN) >= 1)
                {
                    ETGModConsole.Log("Rusty Shotgun <color=#04eb00>[Unlocked]</color>!");
                }
                else { ETGModConsole.Log("Rusty Shotgun <color=#ff0000ff>[Locked]</color> - Steal an item from Rusty."); }

                ETGModConsole.Log("<color=#00d6e6>Shade Unlocks:</color>"); //------------------------------------------------------------

                //Shade Kill Dragun
                if (SaveAPIManager.GetFlag(CustomDungeonFlags.DRAGUN_BEATEN_SHADE))
                {
                    ETGModConsole.Log("Shade Shot <color=#04eb00>[Unlocked]</color>!");
                }
                else { ETGModConsole.Log("Shade Shot <color=#ff0000ff>[Locked]</color> - Kill the Dragun as the Shade."); }

                //Lead Soul
                if (SaveAPIManager.GetFlag(CustomDungeonFlags.LICH_BEATEN_SHADE))
                {
                    ETGModConsole.Log("Lead Soul <color=#04eb00>[Unlocked]</color>!");
                }
                else { ETGModConsole.Log("Lead Soul <color=#ff0000ff>[Locked]</color> - Kill the Lich as the Shade."); }

                //Shades Eye
                if (SaveAPIManager.GetFlag(CustomDungeonFlags.CHEATED_DEATH_SHADE))
                {
                    ETGModConsole.Log("Shade's Eye <color=#04eb00>[Unlocked]</color>!");
                }
                else { ETGModConsole.Log("Shade's Eye <color=#ff0000ff>[Locked]</color> - Take damage as the Shade... and live!"); }

                //----------------------------------------------------------BOSSRUSH
                ETGModConsole.Log("<color=#00d6e6>Bossrush Unlocks:</color>");

                //Keygen
                if (SaveAPIManager.GetFlag(CustomDungeonFlags.BOSSRUSH_PILOT))
                {
                    ETGModConsole.Log("Keygen <color=#04eb00>[Unlocked]</color>!");
                }
                else { ETGModConsole.Log("Keygen  <color=#ff0000ff>[Locked]</color> - Beat Bossrush as the Pilot."); }

                //Lvl. 2 Molotov
                if (SaveAPIManager.GetFlag(CustomDungeonFlags.BOSSRUSH_CONVICT))
                {
                    ETGModConsole.Log("Lvl. 2 Molotov <color=#04eb00>[Unlocked]</color>!");
                }
                else { ETGModConsole.Log("Lvl. 2 Molotov  <color=#ff0000ff>[Locked]</color> - Beat Bossrush as the Convict."); }

                //Paraglocks
                if (SaveAPIManager.GetFlag(CustomDungeonFlags.BOSSRUSH_PARADOX))
                {
                    ETGModConsole.Log("Paraglocks <color=#04eb00>[Unlocked]</color>!");
                }
                else { ETGModConsole.Log("Paraglocks  <color=#ff0000ff>[Locked]</color> - Beat Bossrush as the Paradox."); }

                //Jaws of Defeat
                if (SaveAPIManager.GetFlag(CustomDungeonFlags.BOSSRUSH_SHADE))
                {
                    ETGModConsole.Log("Jaws of Defeat <color=#04eb00>[Unlocked]</color>!");
                }
                else { ETGModConsole.Log("Jaws of Defeat  <color=#ff0000ff>[Locked]</color> - Beat Bossrush as the Shade."); }

                //----------------------------------------------------------RAINBOW MODE
                ETGModConsole.Log("<color=#00d6e6>Rainbow Mode Unlocks:</color>");

                //Rainbow Guon Stone
                if (SaveAPIManager.GetFlag(CustomDungeonFlags.RAINBOW_KILLED_LICH))
                {
                    ETGModConsole.Log("Rainbow Guon Stone <color=#04eb00>[Unlocked]</color>!");
                }
                else { ETGModConsole.Log("Rainbow Guon Stone <color=#ff0000ff>[Locked]</color> - Kill the Lich in Rainbow Mode."); }

                ETGModConsole.Log("<color=#00d6e6>Turbo Mode Unlocks:</color>"); //------------------------------------------------------------

                //Chaos Ruby
                if (SaveAPIManager.GetFlag(CustomDungeonFlags.BEATEN_ANY_BOSS_TURBO_MODE))
                {
                    ETGModConsole.Log("Chaos Ruby <color=#04eb00>[Unlocked]</color>!");
                }
                else { ETGModConsole.Log("Chaos Ruby <color=#ff0000ff>[Locked]</color> - Beat any boss in Turbo Mode."); }

                //Ringer
                if (SaveAPIManager.GetFlag(CustomDungeonFlags.BEATEN_MINES_BOSS_TURBO_MODE))
                {
                    ETGModConsole.Log("Ringer <color=#04eb00>[Unlocked]</color>!");
                }
                else { ETGModConsole.Log("Ringer <color=#ff0000ff>[Locked]</color> - Beat the Black Powder Mine on Turbo Mode."); }

                //Supersonic Shots
                if (SaveAPIManager.GetFlag(CustomDungeonFlags.BEATEN_HOLLOW_BOSS_TURBO_MODE))
                {
                    ETGModConsole.Log("Supersonic Shots <color=#04eb00>[Unlocked]</color>!");
                }
                else { ETGModConsole.Log("Supersonic Shots <color=#ff0000ff>[Locked]</color> - Beat the Hollow on Turbo Mode."); }



                ETGModConsole.Log("<color=#00d6e6>All-Jammed Mode Unlocks:</color>"); //------------------------------------------------------------
                if (SaveAPIManager.GetFlag(CustomDungeonFlags.ALLJAMMED_BEATEN_KEEP))
                {
                    ETGModConsole.Log("Hallowed Bullets <color=#04eb00>[Unlocked]</color>!");
                }
                else { ETGModConsole.Log("Hallowed Bullets <color=#ff0000ff>[Locked]</color> - Beat the Keep on All-Jammed Mode"); }
                if (SaveAPIManager.GetFlag(CustomDungeonFlags.ALLJAMMED_BEATEN_OUB))
                {
                    ETGModConsole.Log("Chemical Burn <color=#04eb00>[Unlocked]</color>!");
                }
                else { ETGModConsole.Log("Chemical Burn <color=#ff0000ff>[Locked]</color> - Beat the Oubliette on All-Jammed Mode"); }
                if (SaveAPIManager.GetFlag(CustomDungeonFlags.ALLJAMMED_BEATEN_PROPER))
                {
                    ETGModConsole.Log("Silver Guon Stone <color=#04eb00>[Unlocked]</color>!");
                }
                else { ETGModConsole.Log("Silver Guon Stone <color=#ff0000ff>[Locked]</color> - Beat the Gungeon Proper on All-Jammed Mode"); }
                if (SaveAPIManager.GetFlag(CustomDungeonFlags.ALLJAMMED_BEATEN_ABBEY))
                {
                    ETGModConsole.Log("Gunidae solvit Haatelis <color=#04eb00>[Unlocked]</color>!");
                }
                else { ETGModConsole.Log("Gunidae solvit Haatelis <color=#ff0000ff>[Locked]</color> - Beat the Abbey on All-Jammed Mode"); }
                if (SaveAPIManager.GetFlag(CustomDungeonFlags.ALLJAMMED_BEATEN_MINES))
                {
                    ETGModConsole.Log("Bloodthirsty Bullets <color=#04eb00>[Unlocked]</color>!");
                }
                else { ETGModConsole.Log("Bloodthirsty Bullets <color=#ff0000ff>[Locked]</color> - Beat the Mines on All-Jammed Mode"); }
                if (SaveAPIManager.GetFlag(CustomDungeonFlags.ALLJAMMED_BEATEN_RAT))
                {
                    ETGModConsole.Log("Ammo Trap <color=#04eb00>[Unlocked]</color>!");
                }
                else { ETGModConsole.Log("Ammo Trap <color=#ff0000ff>[Locked]</color> - Beat the Rat's Lair on All-Jammed Mode"); }
                if (SaveAPIManager.GetFlag(CustomDungeonFlags.ALLJAMMED_BEATEN_HOLLOW))
                {
                    ETGModConsole.Log("Mirror Bullets <color=#04eb00>[Unlocked]</color>!");
                }
                else { ETGModConsole.Log("Mirror Bullets <color=#ff0000ff>[Locked]</color> - Beat the Hollow on All-Jammed Mode"); }
                if (SaveAPIManager.GetFlag(CustomDungeonFlags.ALLJAMMED_BEATEN_OFFICE))
                {
                    ETGModConsole.Log("Scroll of Exact Knowledge <color=#04eb00>[Unlocked]</color>!");
                }
                else { ETGModConsole.Log("Scroll of Exact Knowledge <color=#ff0000ff>[Locked]</color> - Beat the R&G Dept. on All-Jammed Mode"); }
                if (SaveAPIManager.GetFlag(CustomDungeonFlags.ALLJAMMED_BEATEN_FORGE))
                {
                    ETGModConsole.Log("Cloak of Darkness <color=#04eb00>[Unlocked]</color>!");
                }
                else { ETGModConsole.Log("Cloak of Darkness <color=#ff0000ff>[Locked]</color> - Beat the Forge on All-Jammed Mode"); }
                if (SaveAPIManager.GetFlag(CustomDungeonFlags.ALLJAMMED_BEATEN_HELL))
                {
                    ETGModConsole.Log("Gun of a Thousand Sins <color=#04eb00>[Unlocked]</color>!");
                }
                else { ETGModConsole.Log("Gun of a Thousand Sins <color=#ff0000ff>[Locked]</color> - Beat Bullet Hell on All-Jammed Mode"); }

                ETGModConsole.Log("<color=#00d6e6>Challenge Run Unlocks:</color>"); //------------------------------------------------------------
                ETGModConsole.Log("View all challenges and how to activate them by entering 'nnchallenges' into the console.");
                //Supersonic Shots
                if (SaveAPIManager.GetFlag(CustomDungeonFlags.CHALLENGE_WHATARMY_BEATEN))
                {
                    ETGModConsole.Log("Tablet of Order <color=#04eb00>[Unlocked]</color>!");
                }
                else { ETGModConsole.Log("Tablet of Order <color=#ff0000ff>[Locked]</color> - Kill the Dragun on the 'What Army?' Challenge."); }
                if (SaveAPIManager.GetFlag(CustomDungeonFlags.CHALLENGE_INVISIBLEO_BEATEN))
                {
                    ETGModConsole.Log("Ring of Invisibility <color=#04eb00>[Unlocked]</color>!");
                }
                else { ETGModConsole.Log("Ring of Invisibility <color=#ff0000ff>[Locked]</color> - Kill the Dragun on the 'Invisible-O' Challenge."); }
                if (SaveAPIManager.GetFlag(CustomDungeonFlags.CHALLENGE_TOILANDTROUBLE_BEATEN))
                {
                    ETGModConsole.Log("Witches Brew <color=#04eb00>[Unlocked]</color>!");
                }
                else { ETGModConsole.Log("Witches Brew <color=#ff0000ff>[Locked]</color> - Kill the Dragun on the 'Toil And Trouble' Challenge."); }
                if (SaveAPIManager.GetFlag(CustomDungeonFlags.CHALLENGE_KEEPITCOOL_BEATEN))
                {
                    ETGModConsole.Log("Blizzkrieg <color=#04eb00>[Unlocked]</color>!");
                }
                else { ETGModConsole.Log("Blizzkrieg <color=#ff0000ff>[Locked]</color> - Beat the Hollow on the 'Keep It Cool' Challenge."); }

                ETGModConsole.Log("<color=#00d6e6>Hunting Quest Unlocks:</color>"); //------------------------------------------------------------
                ETGModConsole.Log("See Frifle and Mauser for details.");

                if (SaveAPIManager.GetFlag(CustomDungeonFlags.MISFIREBEAST_QUEST_REWARDED))
                {
                    ETGModConsole.Log("Beastclaw <color=#04eb00>[Unlocked]</color>!");
                }
                else { ETGModConsole.Log("Beastclaw <color=#ff0000ff>[Locked]</color>."); }
                if (SaveAPIManager.GetFlag(CustomDungeonFlags.NITRA_QUEST_REWARDED))
                {
                    ETGModConsole.Log("Dynamite Launcher <color=#04eb00>[Unlocked]</color>!");
                }
                else { ETGModConsole.Log("Dynamite Launcher <color=#ff0000ff>[Locked]</color>."); }
                if (SaveAPIManager.GetFlag(CustomDungeonFlags.GUNCULTIST_QUEST_REWARDED))
                {
                    ETGModConsole.Log("Kalibers Prayer <color=#04eb00>[Unlocked]</color>!");
                }
                else { ETGModConsole.Log("Kalibers Prayer <color=#ff0000ff>[Locked]</color>."); }
                if (SaveAPIManager.GetFlag(CustomDungeonFlags.PHASERSPIDER_QUEST_REWARDED))
                {
                    ETGModConsole.Log("Phaser Spiderling <color=#04eb00>[Unlocked]</color>!");
                }
                else { ETGModConsole.Log("Phaser Spiderling <color=#ff0000ff>[Locked]</color>."); }
                if (SaveAPIManager.GetFlag(CustomDungeonFlags.JAMMEDBULLETKIN_QUEST_REWARDED))
                {
                    ETGModConsole.Log("Maroon Guon Stone <color=#04eb00>[Unlocked]</color>!");
                }
                else { ETGModConsole.Log("Maroon Guon Stone <color=#ff0000ff>[Locked]</color>."); }
                if (SaveAPIManager.GetFlag(CustomDungeonFlags.JAMMEDSHOTGUNKIN_QUEST_REWARDED))
                {
                    ETGModConsole.Log("Blight Shell <color=#04eb00>[Unlocked]</color>!");
                }
                else { ETGModConsole.Log("Blight Shell <color=#ff0000ff>[Locked]</color>."); }
                if (SaveAPIManager.GetFlag(CustomDungeonFlags.JAMMEDLEADMAIDEN_QUEST_REWARDED))
                {
                    ETGModConsole.Log("Maiden-Shaped Box <color=#04eb00>[Unlocked]</color>!");
                }
                else { ETGModConsole.Log("Maiden-Shaped Box <color=#ff0000ff>[Locked]</color>."); }
                if (SaveAPIManager.GetFlag(CustomDungeonFlags.JAMMEDBULLETSHARK_QUEST_REWARDED))
                {
                    ETGModConsole.Log("Gunshark <color=#04eb00>[Unlocked]</color>!");
                }
                else { ETGModConsole.Log("Gunshark <color=#ff0000ff>[Locked]</color>."); }
                if (SaveAPIManager.GetFlag(CustomDungeonFlags.JAMMEDGUNNUT_QUEST_REWARDED))
                {
                    ETGModConsole.Log("Bullet Blade <color=#04eb00>[Unlocked]</color>!");
                }
                else { ETGModConsole.Log("Bullet Blade <color=#ff0000ff>[Locked]</color>."); }
            });
        }
    }
}
