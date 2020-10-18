using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Gungeon;
using Dungeonator;

namespace NevernamedsItems
{
    public class Commands : ETGModule
    {
        public static bool allJammedState;
        public static bool instakillersDoubleDamage = false;
        public override void Exit()
        {
        }
        public override void Start()
        {
        }
        public override void Init()
        {
            ETGModConsole.Commands.AddGroup("nn", delegate (string[] args)
            {
                ETGModConsole.Log("<size=100><color=#ff0000ff>Please specify a command. Type 'nn help' for a list of commands.</color></size>", false);
            });
            ETGModConsole.Commands.GetGroup("nn").AddUnit("help", delegate (string[] args)
            {
                ETGModConsole.Log("<size=100><color=#ff0000ff>List of Commands</color></size>", false);
                ETGModConsole.Log("<size=100><color=#ff0000ff>-------------------</color></size>", false);
                ETGModConsole.Log("<color=#ff0000ff>togglealljammed</color> - Turns on and off All-Jammed mode.", false);
                ETGModConsole.Log("<color=#ff0000ff>toggleinstakillitems</color> - Toggles whether or not insta-killing items insta-kill enemies, or just deal double damage.", false);
                ETGModConsole.Log("<color=#ff0000ff>togglelooping</color> - Turns on and off the experimental looping mode.", false);
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
                ETGModConsole.Log("<color=#ff0000ff>Prismatism:</color> "+ModInstallFlags.PrismatismInstalled);
                ETGModConsole.Log("<color=#ff0000ff>Expand The Gungeon:</color> "+ModInstallFlags.ExpandTheGungeonInstalled);
                ETGModConsole.Log("<color=#ff0000ff>Cel's Items:</color> " + ModInstallFlags.CelsItemsInstalled);
                ETGModConsole.Log("<color=#ff0000ff>Fallen Items:</color> " + ModInstallFlags.FallenItemsInstalled);
                ETGModConsole.Log("<color=#ff0000ff>Retrash's Items:</color> " + ModInstallFlags.RetrashItemsInstalled);
                ETGModConsole.Log("<color=#ff0000ff>Some Bunny's Content Pack:</color> " + ModInstallFlags.SomeBunnysItemsInstalled);
                ETGModConsole.Log("<color=#ff0000ff>SpecialAPI's Stuff:</color> " + ModInstallFlags.SpecialAPIsStuffInstalled);
                ETGModConsole.Log("<color=#ff0000ff>Hunter's ROR Items:</color> " + ModInstallFlags.RORItemsInstalled);
                ETGModConsole.Log("<color=#ff0000ff>Kyle's Items:</color> " + ModInstallFlags.KylesItemsInstalled);
            });
            ETGModConsole.Commands.GetGroup("nn").AddUnit("toggleinstakillitems", delegate (string[] args)
            {
                if (instakillersDoubleDamage == true)
                {
                    instakillersDoubleDamage = false;
                    ETGModConsole.Log("Insta-kill items will now correctly instantly kill things.");
                }
                else
                {
                    instakillersDoubleDamage = true;
                    ETGModConsole.Log("Insta-kill items will now deal double damage to enemies instead.");
                }
            });
            ETGModConsole.Commands.GetGroup("nn").AddUnit("roomdata", delegate (string[] args)
            {
                PlayerController player = GameManager.Instance.PrimaryPlayer;

                //var roomType = player.CurrentRoom.GetType();
                //roomType.ToString();
                var roomName = player.CurrentRoom.GetRoomName();
                //ETGModConsole.Log("<color=#ff0000ff>-------------------------------</color>");
                ETGModConsole.Log("<color=#ff0000ff>Room Name: </color>" + roomName);
                //ETGModConsole.Log("<color=#ff0000ff>Room Type: </color>" + roomType);
            });
            //ETGModConsole.Commands.GetGroup("nn").AddUnit("spawnchest", new Action<string[]>(this.SpawnChestNN));
            /*ETGModConsole.Commands.GetGroup("nn").AddUnit("togglelooping", delegate (string[] args)
            {
                if (LoopingMode.loopingActive == true)
                {
                    LoopingMode.loopingActive = false;
                    ETGModConsole.Log("Looping Mode Deactivated");
                }
                else
                {
                    LoopingMode.loopingActive = true;
                    ETGModConsole.Log("Looping Mode Activated");
                }
            });*/
        }
        public static List<Chest> RandomNormalChestTiers = new List<Chest>()
        {
            GameManager.Instance.RewardManager.D_Chest,
            GameManager.Instance.RewardManager.C_Chest,
            GameManager.Instance.RewardManager.B_Chest,
            GameManager.Instance.RewardManager.A_Chest,
            GameManager.Instance.RewardManager.S_Chest,
            GameManager.Instance.RewardManager.Synergy_Chest,
            GameManager.Instance.RewardManager.Rainbow_Chest,
        };
        private void SpawnChestNN(string[] args)
        {
            if (!ETGModConsole.ArgCount(args, 1, 2))
            {
                RoomHandler currentRoom = GameManager.Instance.PrimaryPlayer.CurrentRoom;
                string text = args[0].ToLower();
                Chest chestTypeToSpawn = null;
                bool shouldAlsoBeRainbow = false;
                bool shouldBeGlitched = false;
                bool playerEnteredValidChestTier = false;
                if (text != null)
                {
                    switch (text)
                    {
                        case "brown":
                            chestTypeToSpawn = GameManager.Instance.RewardManager.D_Chest;
                            playerEnteredValidChestTier = true;
                            break;
                        case "blue":
                            chestTypeToSpawn = GameManager.Instance.RewardManager.C_Chest;
                            playerEnteredValidChestTier = true;
                            break;
                        case "green":
                            chestTypeToSpawn = GameManager.Instance.RewardManager.B_Chest;
                            playerEnteredValidChestTier = true;
                            break;
                        case "red":
                            chestTypeToSpawn = GameManager.Instance.RewardManager.A_Chest;
                            playerEnteredValidChestTier = true;
                            break;
                        case "black":
                            chestTypeToSpawn = GameManager.Instance.RewardManager.S_Chest;
                            playerEnteredValidChestTier = true;
                            break;
                        case "synergy":
                            chestTypeToSpawn = GameManager.Instance.RewardManager.Synergy_Chest;
                            playerEnteredValidChestTier = true;
                            break;
                        case "rainbow":
                            chestTypeToSpawn = GameManager.Instance.RewardManager.Rainbow_Chest;
                            playerEnteredValidChestTier = true;
                            break;
                        case "rainbow_synergy":
                            chestTypeToSpawn = GameManager.Instance.RewardManager.Synergy_Chest;
                            shouldAlsoBeRainbow = true;
                            playerEnteredValidChestTier = true;
                            break;
                        case "gliched":
                            chestTypeToSpawn = BraveUtility.RandomElement(RandomNormalChestTiers);
                            shouldBeGlitched = true;
                            playerEnteredValidChestTier = true;
                            break;
                        case "random":
                            chestTypeToSpawn = BraveUtility.RandomElement(RandomNormalChestTiers);
                            playerEnteredValidChestTier = true;
                            break;
                    }
                    if (playerEnteredValidChestTier && chestTypeToSpawn != null)
                    {
                        WeightedGameObject weightedGameObject = new WeightedGameObject();
                        weightedGameObject.rawGameObject = chestTypeToSpawn.gameObject;
                        WeightedGameObjectCollection weightedGameObjectCollection = new WeightedGameObjectCollection();
                        weightedGameObjectCollection.Add(weightedGameObject);
                        float overrideMimicChance = chestTypeToSpawn.overrideMimicChance;
                        chestTypeToSpawn.overrideMimicChance = 0f;
                        if (args.Length > 1)
                        {
                            int Num = 1;
                            bool Flag = int.TryParse(args[1], out Num);
                            if (!Flag)
                            {
                                ETGModConsole.Log("Second argument must be an integer (number)", false);
                                return;
                            }
                            for (int i = 0; i < Num; i++)
                            {
                                Chest chest2 = currentRoom.SpawnRoomRewardChest(weightedGameObjectCollection, currentRoom.GetBestRewardLocation(new IntVector2(2, 1), RoomHandler.RewardLocationStyle.PlayerCenter, true));
                                if (shouldBeGlitched)
                                {
                                    chest2.BecomeGlitchChest();
                                }
                                if (shouldAlsoBeRainbow)
                                {
                                    chest2.IsRainbowChest = true;
                                }
                            }
                            chestTypeToSpawn.overrideMimicChance = overrideMimicChance;
                            return;
                        }
                    }
                    else
                    {
                        ETGModConsole.Log("Chest type " + args[0] + " doesn't exist! Valid types: brown, blue, green, red, black, rainbow, glitched, synergy, or rainbow_synergy!", false);
                    }
                }
            }
        }
    }
}