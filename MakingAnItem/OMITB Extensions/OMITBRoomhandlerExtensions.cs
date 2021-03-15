using Dungeonator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NevernamedsItems
{
    static class OMITBRoomhandlerExtensions
    {
        public static List<AIActor> GetXEnemiesInRoom(this RoomHandler room, int numOfEnemiesToReturn, bool reqForRoomClear = true, bool canReturnBosses = true)
        {
            if (numOfEnemiesToReturn <= 0) return null;
            RoomHandler.ActiveEnemyType type = RoomHandler.ActiveEnemyType.All;
            if (reqForRoomClear) type = RoomHandler.ActiveEnemyType.RoomClear;
            List<AIActor> activeEnemies = room.GetActiveEnemies(type);
            if (activeEnemies != null)
            {
                if (!canReturnBosses)
                {
                    for (int i = 0; i < activeEnemies.Count; i++)
                    {
                        if (activeEnemies[i].IsBoss()) activeEnemies.RemoveAt(i);
                    }
                }
                if (activeEnemies.Count > numOfEnemiesToReturn)
                {
                    List<AIActor> pickedEnemies = new List<AIActor>();
                    for (int i = 0; i < numOfEnemiesToReturn; i++)
                    {
                        AIActor actor = BraveUtility.RandomElement(activeEnemies);
                        pickedEnemies.Add(actor);
                        activeEnemies.Remove(actor);
                    }
                    return pickedEnemies;
                }
                else
                {
                    return activeEnemies;
                }
            }
            else return null;
        }
    }
}
