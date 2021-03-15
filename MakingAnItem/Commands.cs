using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Gungeon;
using Dungeonator;

namespace NevernamedsItems
{
    public class Commands
    {
        public static bool allJammedState;
        public static bool instakillersDoubleDamage = false;
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
                ETGModConsole.Log("<color=#ff0000ff>Prismatism:</color> " + ModInstallFlags.PrismatismInstalled);
                ETGModConsole.Log("<color=#ff0000ff>Expand The Gungeon:</color> " + ModInstallFlags.ExpandTheGungeonInstalled);
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
        }           
    }
}
 
