using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Gungeon;
using Dungeonator;
using SaveAPI;
using System.Collections;

namespace NevernamedsItems
{
    public class Commands
    {
        public static bool allJammedState;
        public static bool itemsHaveBeenRarityBoosted;
        public static void Init()
        {
            ETGModConsole.Commands.AddGroup("nn", delegate (string[] args)
            {
                ETGModConsole.Log("<size=100><color=#ff0000ff>Please specify a command. Type 'nn help' for a list of commands.</color></size>", false);
            });
            ETGModConsole.Commands.GetGroup("nn").AddUnit("help", delegate (string[] args)
            {
                ETGModConsole.Log("<size=100><color=#ff0000ff>List of Commands</color></size>", false);
                ETGModConsole.Log("<size=100><color=#ff0000ff>-------------------</color></size>", false);
                ETGModConsole.Log("<color=#ff0000ff>checkunlocks</color> - Lists what OMITB unlocks you have yet to achieve, with explanations for each one.", false);
                ETGModConsole.Log("<color=#ff0000ff>togglealljammed</color> - Turns on and off All-Jammed mode.", false);
                ETGModConsole.Log("<color=#ff0000ff>togglelooping</color> - Turns on and off the experimental looping mode.", false);
                ETGModConsole.Log("<color=#ff0000ff>togglerarityboost</color> - Greatly increases the loot weight of modded items, making them show up much more.", false);
                ETGModConsole.Log("<color=#ff0000ff>roomdata</color> - Displays data about the current room (currently only it's name)", false);
                ETGModConsole.Log("<color=#ff0000ff>listInstalledMods</color> - Shows what other mods Once More Into The Breach thinks are active. Debug command.", false);
            });
            ETGModConsole.Commands.GetGroup("nn").AddUnit("togglealljammed", delegate (string[] args)
            {
                if (AllJammedState.AllJammedActive == true)
                {
                    SaveAPIManager.SetFlag(CustomDungeonFlags.ALLJAMMEDMODE_ENABLED_CONSOLE, false);
                    SaveAPIManager.SetFlag(CustomDungeonFlags.ALLJAMMEDMODE_ENABLED_GENUINE, false);
                    ETGModConsole.Log("All-Jammed Mode has been disabed.");
                }
                else
                {
                    SaveAPIManager.SetFlag(CustomDungeonFlags.ALLJAMMEDMODE_ENABLED_CONSOLE, true);
                    SaveAPIManager.SetFlag(CustomDungeonFlags.ALLJAMMEDMODE_ENABLED_GENUINE, false);
                    ETGModConsole.Log("All-Jammed Mode has been enabled.");
                }
            });
            ETGModConsole.Commands.GetGroup("nn").AddUnit("listInstalledMods", delegate (string[] args)
            {
                ETGModConsole.Log("<color=#ff0000ff>Prismatism:</color> " + ModInstallFlags.PrismatismInstalled);
                ETGModConsole.Log("<color=#ff0000ff>Expand The Gungeon:</color> " + ModInstallFlags.ExpandTheGungeonInstalled);
                ETGModConsole.Log("<color=#ff0000ff>Cel's Items:</color> " + ModInstallFlags.CelsItemsInstalled);
                ETGModConsole.Log("<color=#ff0000ff>Fallen Items:</color> " + ModInstallFlags.FallenItemsInstalled);
                ETGModConsole.Log("<color=#ff0000ff>Retrash's Items:</color> " + ModInstallFlags.RetrashItemsInstalled);
                ETGModConsole.Log("<color=#ff0000ff>Some Bunny's Content Pack:</color> " + ModInstallFlags.SomeBunnysItemsInstalled);
                ETGModConsole.Log("<color=#ff0000ff>SpecialAPI's Stuff:</color> " + ModInstallFlags.SpecialAPIsStuffInstalled);
                ETGModConsole.Log("<color=#ff0000ff>Hunter's ROR Items:</color> " + ModInstallFlags.RORItemsInstalled);
                ETGModConsole.Log("<color=#ff0000ff>Kyle's Items:</color> " + ModInstallFlags.KylesItemsInstalled);
                ETGModConsole.Log("<color=#ff0000ff>Frost and Gunfire:</color> " + ModInstallFlags.FrostAndGunfireInstalled);
                ETGModConsole.Log("<color=#ff0000ff>Planetside of Gunymede:</color> " + ModInstallFlags.PlanetsideOfGunymededInstalled);
            });
            ETGModConsole.Commands.GetGroup("nn").AddUnit("roomdata", delegate (string[] args)
            {
                PlayerController player = GameManager.Instance.PrimaryPlayer;
                var roomName = player.CurrentRoom.GetRoomName();
                ETGModConsole.Log("<color=#ff0000ff>Room Name: </color>" + roomName);
            });
            ETGModConsole.Commands.GetGroup("nn").AddUnit("togglerarityboost", delegate (string[] args)
            {
                if (itemsHaveBeenRarityBoosted)
                {
                    ETGModConsole.Log("The loot weight of modded items and guns has been reset to normal.", false);
                    foreach (WeightedGameObject obj in GameManager.Instance.RewardManager.GunsLootTable.defaultItemDrops.elements)
                    {
                        if (obj.pickupId > 823 || obj.pickupId < 0)
                        {
                            obj.weight /= 100;
                        }
                    }
                    foreach (WeightedGameObject obj in GameManager.Instance.RewardManager.ItemsLootTable.defaultItemDrops.elements)
                    {
                        if (obj.pickupId > 823 || obj.pickupId < 0)
                        {
                            obj.weight /= 100;
                        }
                    }
                    itemsHaveBeenRarityBoosted = false;
                }
                else
                {
                    ETGModConsole.Log("The loot weight of modded items and guns has been GREATLY increased.", false);
                    foreach (WeightedGameObject obj in GameManager.Instance.RewardManager.GunsLootTable.defaultItemDrops.elements)
                    {
                        if (obj.pickupId > 823 || obj.pickupId < 0)
                        {
                            float initialWeight = obj.weight;
                            obj.weight *= 100;

                            string displayName = "ERROR DISPLAY NAME NULL";
                         if (PickupObjectDatabase.GetById(obj.pickupId) != null)   displayName = (PickupObjectDatabase.GetById(obj.pickupId) as Gun).DisplayName;
                            ETGModConsole.Log(displayName + " (" + obj.pickupId + "): " + initialWeight + " ---> " + obj.weight);

                        }
                    }
                    foreach (WeightedGameObject obj in GameManager.Instance.RewardManager.ItemsLootTable.defaultItemDrops.elements)
                    {
                        if (obj.pickupId > 823 || obj.pickupId < 0)
                        {
                            obj.weight *= 100;
                        }
                    }
                    itemsHaveBeenRarityBoosted = true;
                }
            });
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

            ETGModConsole.Commands.GetGroup("nn").AddUnit("givebyid", delegate (string[] args)
            {
                if (args == null) ETGModConsole.Log("No args");
                if (args.Length > 1) ETGModConsole.Log("Too many args");
                string id = args[0];
                int id2 = -1;
                try
                {
                    id2 = int.Parse(id);
                }
                catch
                {
                    ETGModConsole.Log("That's not a numerical id");
                }
                if (id2 >= 0)
                {
                    if (PickupObjectDatabase.GetById(id2) != null)
                    {
                        LootEngine.SpawnItem(PickupObjectDatabase.GetById(id2).gameObject, GameManager.Instance.PrimaryPlayer.CenterPosition, Vector2.zero, 0);
                    }
                }

            });

            #region DeconstructGun
            ETGModConsole.Commands.GetGroup("nn").AddUnit("deconstructgun", delegate (string[] args)
        {
            if (GameManager.Instance == null)
            {
                ETGModConsole.Log("Somehow the fucking game manager was null lol rip get fucked mate.");
                return;
            }
            if (GameManager.Instance.PrimaryPlayer == null)
            {
                ETGModConsole.Log("<size=100><color=#ff0000ff>ERROR: There is no player 1 to check for a gun.</color></size>", false);
                return;
            }
            if (GameManager.Instance.PrimaryPlayer.CurrentGun == null)
            {
                ETGModConsole.Log("<size=100><color=#ff0000ff>ERROR: Player 1 is not holding a gun to deconstruct.</color></size>", false);
                return;
            }
            Gun target = GameManager.Instance.PrimaryPlayer.CurrentGun;

            ETGModConsole.Log("<color=#09b022>-------------------------------------</color>");
            ETGModConsole.Log("<color=#09b022>Base Gun Stats:</color>");
            ETGModConsole.Log("<color=#ff0000ff>Display Name: </color>" + target.DisplayName); ;
            ETGModConsole.Log("<color=#ff0000ff>Object Name: </color>" + target.name);
            ETGModConsole.Log("<color=#ff0000ff>Internal Name:</color>" + target.gunName);
            ETGModConsole.Log("<color=#ff0000ff>ID: </color>" + target.PickupObjectId);
            ETGModConsole.Log("<color=#ff0000ff>Class: </color>" + target.gunClass);
            ETGModConsole.Log("<color=#09b022>Numerical Base Stats:</color>");
            ETGModConsole.Log("<color=#ff0000ff>Infinite Ammo: </color>" + target.InfiniteAmmo);
            ETGModConsole.Log("<color=#ff0000ff>Ammo Max: </color>" + target.GetBaseMaxAmmo());
            ETGModConsole.Log("<color=#ff0000ff>Reload Time: </color>" + target.reloadTime);
            ETGModConsole.Log("<color=#ff0000ff>Components:</color>");
            foreach (Component component in target.GetComponents<Component>())
            {
                ETGModConsole.Log(component.GetType().ToString());
                if (component is DualWieldSynergyProcessor)
                {
                    ETGModConsole.Log("<color=#ff0000ff>    Req Synergy: </color>" + (component as DualWieldSynergyProcessor).SynergyToCheck);
                    ETGModConsole.Log("<color=#ff0000ff>    Other Gun: </color>" + PickupObjectDatabase.GetById((component as DualWieldSynergyProcessor).PartnerGunID).DisplayName + " (" + (component as DualWieldSynergyProcessor).PartnerGunID + ")");
                }
                else if (component is FireOnReloadSynergyProcessor)
                {
                    ETGModConsole.Log("<color=#ff0000ff>    Req Synergy: </color>" + (component as FireOnReloadSynergyProcessor).SynergyToCheck);
                    ETGModConsole.Log("<color=#ff0000ff>    Use Gun Proj: </color>" + (component as FireOnReloadSynergyProcessor).DirectedBurstSettings.ProjectileInterface.UseCurrentGunProjectile);
                    if ((component as FireOnReloadSynergyProcessor).DirectedBurstSettings.ProjectileInterface.SpecifiedProjectile != null)
                    {
                        ETGModConsole.Log("<color=#ff0000ff>    Proj Name: </color>" + (component as FireOnReloadSynergyProcessor).DirectedBurstSettings.ProjectileInterface.SpecifiedProjectile.name);
                        ETGModConsole.Log("<color=#ff0000ff>    Proj Damage: </color>" + (component as FireOnReloadSynergyProcessor).DirectedBurstSettings.ProjectileInterface.SpecifiedProjectile.baseData.damage);
                        ETGModConsole.Log("<color=#ff0000ff>    Proj Speed: </color>" + (component as FireOnReloadSynergyProcessor).DirectedBurstSettings.ProjectileInterface.SpecifiedProjectile.baseData.speed);
                        ETGModConsole.Log("<color=#ff0000ff>    Proj Range: </color>" + (component as FireOnReloadSynergyProcessor).DirectedBurstSettings.ProjectileInterface.SpecifiedProjectile.baseData.range);
                        ETGModConsole.Log("<color=#ff0000ff>    Proj Knockback: </color>" + (component as FireOnReloadSynergyProcessor).DirectedBurstSettings.ProjectileInterface.SpecifiedProjectile.baseData.force);
                        ETGModConsole.Log("<color=#ff0000ff>    Proj BossDMGMult: </color>" + (component as FireOnReloadSynergyProcessor).DirectedBurstSettings.ProjectileInterface.SpecifiedProjectile.BossDamageMultiplier);
                        ETGModConsole.Log("<color=#ff0000ff>    Proj DMGTypes: </color>" + (component as FireOnReloadSynergyProcessor).DirectedBurstSettings.ProjectileInterface.SpecifiedProjectile.damageTypes);
                    }
                }
                if (component is TransformGunSynergyProcessor)
                {
                    ETGModConsole.Log("<color=#ff0000ff>    Req Synergy: </color>" + (component as TransformGunSynergyProcessor).SynergyToCheck);
                    ETGModConsole.Log("<color=#ff0000ff>    Transform Target: </color>" + PickupObjectDatabase.GetById((component as TransformGunSynergyProcessor).SynergyGunId).DisplayName + " (" + (component as TransformGunSynergyProcessor).SynergyGunId + ")");
                }
            }
            ETGModConsole.Log("<color=#09b022>Volleys and Modules:</color>");
            int iteratedVolleyModules = 0;
            if (target.RawSourceVolley != null)
            {
                if (target.RawSourceVolley.projectiles != null)
                {
                    foreach (ProjectileModule module in target.RawSourceVolley.projectiles)
                    {
                        ETGModConsole.Log("<color=#09b022>Volley Module: </color>" + iteratedVolleyModules);
                        ETGModConsole.Log("<color=#ff0000ff>    Module Stats:</color>");
                        ETGModConsole.Log("<color=#ff0000ff>    ShootStyle: </color>" + module.shootStyle);
                        ETGModConsole.Log("<color=#ff0000ff>    Sequence: </color>" + module.sequenceStyle);
                        ETGModConsole.Log("<color=#ff0000ff>    Fire Delay: </color>" + module.cooldownTime);
                        ETGModConsole.Log("<color=#ff0000ff>    Ammo Cost: </color>" + module.ammoCost);
                        ETGModConsole.Log("<color=#ff0000ff>    UI Ammo Type: </color>" + module.ammoType);
                        ETGModConsole.Log("<color=#ff0000ff>    AngleFromAim: </color>" + module.angleFromAim);
                        ETGModConsole.Log("<color=#ff0000ff>    AngleVariance: </color>" + module.angleVariance);
                        ETGModConsole.Log("<color=#ff0000ff>    ClipShots: </color>" + module.numberOfShotsInClip);

                        ETGModConsole.Log("<color=#09b022>    Projectiles:</color>");
                        if (module.projectiles != null && module.projectiles.Count > 0)
                        {
                            int iteratedBullets = 0;
                            foreach (Projectile bullet in module.projectiles)
                            {
                                ETGModConsole.Log("<color=#09b022>        Bullet: </color>" + iteratedBullets);
                                if (!string.IsNullOrEmpty(bullet.name)) ETGModConsole.Log("<color=#ff0000ff>        Name: </color>" + bullet.name);
                                else ETGModConsole.Log("<color=#ff0000ff>        Bullet has no name.</color>");
                                iteratedBullets++;
                                if (bullet != null)
                                {
                                    ETGModConsole.Log("<color=#09b022>        BaseData:</color>");
                                    if (bullet.baseData != null)
                                    {
                                        ETGModConsole.Log("<color=#ff0000ff>        Damage: </color>" + bullet.baseData.damage);
                                        ETGModConsole.Log("<color=#ff0000ff>        Speed: </color>" + bullet.baseData.speed);
                                        ETGModConsole.Log("<color=#ff0000ff>        Range: </color>" + bullet.baseData.range);
                                        ETGModConsole.Log("<color=#ff0000ff>        Knockback: </color>" + bullet.baseData.force);
                                    }
                                    else ETGModConsole.Log("<color=#ff0000ff>           BaseData is somehow null?</color>");
                                    ETGModConsole.Log("<color=#ff0000ff>        Other Stats:</color>");
                                    ETGModConsole.Log("<color=#ff0000ff>        BossDMGMult: </color>" + bullet.BossDamageMultiplier);
                                    ETGModConsole.Log("<color=#ff0000ff>        Damage Types: </color>" + bullet.damageTypes);

                                }
                                else ETGModConsole.Log("<color=#ff0000ff>       Bullet is somehow null.</color>");
                            }
                        }
                        else { ETGModConsole.Log("<color=#ff0000ff>    This module has no normal projectiles.</color>"); }

                        ETGModConsole.Log("<color=#09b022>    Charge Projectiles:</color>");
                        if (module.chargeProjectiles != null && module.chargeProjectiles.Count > 0)
                        {
                            int iteratedBullets = 0;
                            foreach (ProjectileModule.ChargeProjectile chargie in module.chargeProjectiles)
                            {
                                if (chargie.Projectile != null)
                                {
                                    Projectile bullet = chargie.Projectile;
                                    ETGModConsole.Log("<color=#09b022>        Bullet: </color>" + iteratedBullets);
                                    if (!string.IsNullOrEmpty(bullet.name)) ETGModConsole.Log("<color=#ff0000ff>        Name: </color>" + bullet.name);
                                    else ETGModConsole.Log("<color=#ff0000ff>        Bullet has no name.</color>");
                                    iteratedBullets++;
                                    ETGModConsole.Log("<color=#ff0000ff>        Charge Time: </color>" + chargie.ChargeTime);
                                    ETGModConsole.Log("<color=#ff0000ff>        BaseData:</color>");
                                    if (bullet.baseData != null)
                                    {
                                        ETGModConsole.Log("<color=#ff0000ff>        Damage: </color>" + bullet.baseData.damage);
                                        ETGModConsole.Log("<color=#ff0000ff>        Speed: </color>" + bullet.baseData.speed);
                                        ETGModConsole.Log("<color=#ff0000ff>        Range: </color>" + bullet.baseData.range);
                                        ETGModConsole.Log("<color=#ff0000ff>        Knockback: </color>" + bullet.baseData.force);
                                    }
                                    else ETGModConsole.Log("<color=#ff0000ff>           BaseData is somehow null?</color>");
                                    ETGModConsole.Log("<color=#ff0000ff>        Other Stats:</color>");
                                    ETGModConsole.Log("<color=#ff0000ff>        BossDMGMult: </color>" + bullet.BossDamageMultiplier);
                                    ETGModConsole.Log("<color=#ff0000ff>        Damage Types: </color>" + bullet.damageTypes);


                                }
                                else ETGModConsole.Log("<color=#ff0000ff>       Bullet is somehow null.</color>");
                            }
                        }
                        else { ETGModConsole.Log("<color=#ff0000ff>    This module has no charge projectiles.</color>"); }

                        iteratedVolleyModules++;
                    }
                }
                else { ETGModConsole.Log("<color=#ff0000ff>Gun has a volley, but no ProjectileModules within it.</color>"); }
            }
            else { ETGModConsole.Log("<color=#ff0000ff>Gun has no volley.</color>"); }

            if (target.DefaultModule != null)
            {
                ProjectileModule module = target.DefaultModule;
                ETGModConsole.Log("<color=#09b022>Default Module: </color>");
                ETGModConsole.Log("<color=#ff0000ff>    Module Stats:</color>");
                ETGModConsole.Log("<color=#ff0000ff>    ShootStyle: </color>" + module.shootStyle);
                ETGModConsole.Log("<color=#ff0000ff>    Sequence: </color>" + module.sequenceStyle);
                ETGModConsole.Log("<color=#ff0000ff>    Fire Delay: </color>" + module.cooldownTime);
                ETGModConsole.Log("<color=#ff0000ff>    Ammo Cost: </color>" + module.ammoCost);
                ETGModConsole.Log("<color=#ff0000ff>    UI Ammo Type: </color>" + module.ammoType);
                ETGModConsole.Log("<color=#ff0000ff>    AngleFromAim: </color>" + module.angleFromAim);
                ETGModConsole.Log("<color=#ff0000ff>    AngleVariance: </color>" + module.angleVariance);
                ETGModConsole.Log("<color=#ff0000ff>    ClipShots: </color>" + module.numberOfShotsInClip);
                if (target.DefaultModule.projectiles != null && target.DefaultModule.projectiles.Count > 0)
                {
                    ETGModConsole.Log("<color=#09b022>    Projectiles:</color>");
                    int iteratedBullets = 0;
                    foreach (Projectile bullet in target.DefaultModule.projectiles)
                    {
                        ETGModConsole.Log("<color=#09b022>        Bullet: </color>" + iteratedBullets);
                        if (!string.IsNullOrEmpty(bullet.name)) ETGModConsole.Log("<color=#ff0000ff>        Name: </color>" + bullet.name);
                        else ETGModConsole.Log("<color=#ff0000ff>        Bullet has no name.</color>");
                        iteratedBullets++;
                        if (bullet != null)
                        {
                            ETGModConsole.Log("<color=#09b022>        BaseData:</color>");
                            if (bullet.baseData != null)
                            {
                                ETGModConsole.Log("<color=#ff0000ff>        Damage: </color>" + bullet.baseData.damage);
                                ETGModConsole.Log("<color=#ff0000ff>        Speed: </color>" + bullet.baseData.speed);
                                ETGModConsole.Log("<color=#ff0000ff>        Range: </color>" + bullet.baseData.range);
                                ETGModConsole.Log("<color=#ff0000ff>        Knockback: </color>" + bullet.baseData.force);
                            }
                            else ETGModConsole.Log("<color=#ff0000ff>           BaseData is somehow null?</color>");
                            ETGModConsole.Log("<color=#ff0000ff>        Other Stats:</color>");
                            ETGModConsole.Log("<color=#ff0000ff>        BossDMGMult: </color>" + bullet.BossDamageMultiplier);
                            ETGModConsole.Log("<color=#ff0000ff>        Damage Types: </color>" + bullet.damageTypes);

                        }
                        else ETGModConsole.Log("<color=#ff0000ff>       Bullet is somehow null.</color>");
                    }
                }
                else { ETGModConsole.Log("<color=#ff0000ff>    Default module has no normal projectiles.</color>"); }

                ETGModConsole.Log("<color=#09b022>    Charge Projectiles:</color>");
                if (target.DefaultModule.chargeProjectiles != null && target.DefaultModule.chargeProjectiles.Count > 0)
                {
                    int iteratedBullets = 0;
                    foreach (ProjectileModule.ChargeProjectile chargie in target.DefaultModule.chargeProjectiles)
                    {
                        if (chargie.Projectile != null)
                        {
                            Projectile bullet = chargie.Projectile;
                            ETGModConsole.Log("<color=#09b022>        Bullet: </color>" + iteratedBullets);
                            if (!string.IsNullOrEmpty(bullet.name)) ETGModConsole.Log("<color=#ff0000ff>        Name: </color>" + bullet.name);
                            else ETGModConsole.Log("<color=#ff0000ff>        Bullet has no name.</color>");
                            iteratedBullets++;
                            ETGModConsole.Log("<color=#ff0000ff>        Charge Time: </color>" + chargie.ChargeTime);
                            ETGModConsole.Log("<color=#ff0000ff>        BaseData:</color>");
                            if (bullet.baseData != null)
                            {
                                ETGModConsole.Log("<color=#ff0000ff>        Damage: </color>" + bullet.baseData.damage);
                                ETGModConsole.Log("<color=#ff0000ff>        Speed: </color>" + bullet.baseData.speed);
                                ETGModConsole.Log("<color=#ff0000ff>        Range: </color>" + bullet.baseData.range);
                                ETGModConsole.Log("<color=#ff0000ff>        Knockback: </color>" + bullet.baseData.force);
                            }
                            else ETGModConsole.Log("<color=#ff0000ff>           BaseData is somehow null?</color>");
                            ETGModConsole.Log("<color=#ff0000ff>        Other Stats:</color>");
                            ETGModConsole.Log("<color=#ff0000ff>        BossDMGMult: </color>" + bullet.BossDamageMultiplier);
                            ETGModConsole.Log("<color=#ff0000ff>        Damage Types: </color>" + bullet.damageTypes);


                        }
                        else ETGModConsole.Log("<color=#ff0000ff>       Bullet is somehow null.</color>");
                    }
                }
                else { ETGModConsole.Log("<color=#ff0000ff>    Default module has no charge projectiles.</color>"); }
            }
            else { ETGModConsole.Log("<color=#ff0000ff>Gun has no Default Module.</color>"); }
        });
            #endregion

            #region LoadRoomCommands
            /* ETGModConsole.Commands.GetGroup("nn").AddUnit("removegun", delegate (string[] args)
             {
                 PlayerController player = GameManager.Instance.PrimaryPlayer;
                 player.inventory.RemoveGunFromInventory(player.CurrentGun);
             });
                 ETGModConsole.Commands.GetGroup("nn").AddUnit("goto", delegate (string[] args)
             {
                 if (args == null) ETGModConsole.Log("No args");
                 if (args.Length > 2) ETGModConsole.Log("Too many args");

                 string targetdungeon = args[0];
                 string targetname = args[1];

                 bool hasFoundtarget = false;
                 PrototypeDungeonRoom targetPrototype = null;
                 if (DungeonDatabase.GetOrLoadByName(targetdungeon) != null)
                 {
                     Dungeon levelInsance = DungeonDatabase.GetOrLoadByName(targetdungeon);
                     foreach (WeightedRoom wRoom in levelInsance.PatternSettings.flows[0].fallbackRoomTable.includedRooms.elements)
                     {
                         if (wRoom.room != null && !string.IsNullOrEmpty(wRoom.room.name))
                         {
                             if (wRoom.room.name == targetname)
                             {
                                 hasFoundtarget = true;
                                 targetPrototype = wRoom.room;
                             }
                         }
                     }
                     levelInsance = null;
                     if (hasFoundtarget)
                     {
                         GameManager.Instance.StartCoroutine(GenerateRoomAndGoTo(GameManager.Instance.PrimaryPlayer, targetPrototype));
                     }
                     else
                     {
                         ETGModConsole.Log("Could not find target room in floor table");
                     }
                 }
                 else ETGModConsole.Log("Not a valid dungeon");
             });
         }
         public static bool m_IsTeleporting;
         public static IEnumerator GenerateRoomAndGoTo(PlayerController user, PrototypeDungeonRoom target)
         {
             RoomHandler currentRoom = user.CurrentRoom;
             user.ForceStopDodgeRoll();
             user.healthHaver.IsVulnerable = false;
             yield return new WaitForSeconds(0.1f);
             Dungeon dungeon = GameManager.Instance.Dungeon;

             RoomHandler GlitchRoom = otherapacheshit.AddCustomRuntimeRoom(target);
             if (GlitchRoom.area.PrototypeRoomCategory == PrototypeDungeonRoom.RoomCategory.SECRET && GlitchRoom.IsSecretRoom)
             {
                 GlitchRoom.secretRoomManager.OpenDoor();
             }
             TeleportToRoom(user, GlitchRoom);
             yield return null;
             while (m_IsTeleporting) { yield return null; }
             yield break;
         }

         public static void TeleportToRoom(PlayerController targetPlayer, RoomHandler targetRoom, bool isSecondaryPlayer = false)
         {
             m_IsTeleporting = true;
             IntVector2? randomAvailableCell = targetRoom.GetRandomAvailableCell(new IntVector2?(new IntVector2(2, 2)), new CellTypes?(CellTypes.FLOOR), false, null);
             if (targetRoom.area.PrototypeRoomCategory == PrototypeDungeonRoom.RoomCategory.EXIT)
             {
                 randomAvailableCell = (new IntVector2(5, 2) + targetRoom.area.basePosition);
             }
             if (!randomAvailableCell.HasValue)
             {
                 m_IsTeleporting = false;
                 return;
             }
             if (GameManager.Instance.CurrentGameType == GameManager.GameType.COOP_2_PLAYER && !isSecondaryPlayer)
             {
                 PlayerController otherPlayer = GameManager.Instance.GetOtherPlayer(targetPlayer);
                 if (otherPlayer) { TeleportToRoom(otherPlayer, targetRoom, true); }
             }
             // targetPlayer.m_isStartingTeleport = false;
             targetPlayer.DoVibration(Vibration.Time.Normal, Vibration.Strength.Medium);
             GameManager.Instance.StartCoroutine(HandleTeleportToRoom(targetPlayer, randomAvailableCell.Value.ToCenterVector2()));
             targetPlayer.specRigidbody.Velocity = Vector2.zero;
             targetPlayer.knockbackDoer.TriggerTemporaryKnockbackInvulnerability(1f);
             targetRoom.EnsureUpstreamLocksUnlocked();
             // GameManager.Instance.StartCoroutine(DelayedRoomReset(targetPlayer, targetPlayer.CurrentRoom));
         }
         private static IEnumerator HandleTeleportToRoom(PlayerController targetPlayer, Vector2 targetPoint)
         {
             targetPlayer.healthHaver.IsVulnerable = false;
             CameraController cameraController = GameManager.Instance.MainCameraController;
             Vector2 offsetVector = (cameraController.transform.position - targetPlayer.transform.position);
             offsetVector -= cameraController.GetAimContribution();
             Minimap.Instance.ToggleMinimap(false, false);
             cameraController.SetManualControl(true, false);
             cameraController.OverridePosition = cameraController.transform.position;
             // targetPlayer.CurrentInputState = PlayerInputState.NoInput;
             yield return new WaitForSeconds(0.1f);
             // yield return new WaitForSeconds(0.4f);
             yield return new WaitForSeconds(1);
             targetPlayer.ToggleRenderer(false, "arbitrary teleporter");
             targetPlayer.ToggleGunRenderers(false, "arbitrary teleporter");
             targetPlayer.ToggleHandRenderers(false, "arbitrary teleporter");
             yield return new WaitForSeconds(1);
             Pixelator.Instance.FadeToBlack(0.15f, false, 0f);
             yield return new WaitForSeconds(0.15f);
             // targetPlayer.specRigidbody.Position = new Position(targetPoint);
             targetPlayer.transform.position = targetPoint;
             GameUIRoot.Instance.HideCoreUI(string.Empty);            
             targetPlayer.specRigidbody.Reinitialize();
             targetPlayer.SetIsStealthed(true, "meh");
             GameObject stealthVFX = OMITBReflectionHelpers.ReflectGetField<GameObject>(typeof(GameActor), "m_stealthVfx", targetPlayer.gameActor);
             UnityEngine.Object.Destroy(stealthVFX);
             targetPlayer.specRigidbody.RecheckTriggers = true;
             if (GameManager.Instance.CurrentGameType == GameManager.GameType.COOP_2_PLAYER)
             {
                 cameraController.OverridePosition = cameraController.GetIdealCameraPosition();
             }
             else
             {
                 cameraController.OverridePosition = (targetPoint + offsetVector).ToVector3ZUp(0f);
             }
             targetPlayer.WarpFollowersToPlayer();
             targetPlayer.WarpCompanionsToPlayer(false);
             Pixelator.Instance.MarkOcclusionDirty();
             Pixelator.Instance.FadeToBlack(0.15f, true, 0f);
             yield return null;
             cameraController.SetManualControl(false, true);
             // yield return new WaitForSeconds(0.75f);
             yield return new WaitForSeconds(0.15f);
             targetPlayer.DoVibration(Vibration.Time.Normal, Vibration.Strength.Medium);
             // yield return new WaitForSeconds(0.25f);
             yield return new WaitForSeconds(1.7f);
             //  targetPlayer.ToggleRenderer(true, "arbitrary teleporter");
             //  targetPlayer.ToggleGunRenderers(true, "arbitrary teleporter");
             // targetPlayer.ToggleHandRenderers(true, "arbitrary teleporter");
             PhysicsEngine.Instance.RegisterOverlappingGhostCollisionExceptions(targetPlayer.specRigidbody, null, false);

             //7  targetPlayer.CurrentInputState = PlayerInputState.AllInput;
             targetPlayer.healthHaver.IsVulnerable = true;
             m_IsTeleporting = false;
             yield break;
         }*/
            #endregion

        }
    }
}

