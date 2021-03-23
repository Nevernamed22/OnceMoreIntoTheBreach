using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Gungeon;
using Dungeonator;
using SaveAPI;

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
                if (AllJammedState.allJammedActive == true)
                {
                    AllJammedState.allJammedActive = false;
                    ETGModConsole.Log("All-Jammed Mode has been disabed.");
                }
                else
                {
                    AllJammedState.allJammedActive = true;
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
                            obj.weight *= 100;
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
                ETGModConsole.Log("<color=#ff0000ff>Room Name: </color>");
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
                else { ETGModConsole.Log("The Beholster <color=#ff0000ff>[Locked]</color> - Kill 15 Beholsters; ["+ SaveAPIManager.GetPlayerStatValue(CustomTrackedStats.BEHOLSTER_KILLS)+"/15]"); }

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

                ETGModConsole.Log("<color=#00d6e6>Rainbow Mode Unlocks:</color>"); //------------------------------------------------------------

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
            });

        }
    }
}
 
