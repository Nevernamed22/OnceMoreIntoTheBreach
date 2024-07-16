using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Dungeonator;
using UnityEngine;
using Alexandria.ItemAPI;

namespace NevernamedsItems
{
    class Bombinomicon : PlayerItem
    {
        public static void Init()
        {
            PlayerItem item = ItemSetup.NewItem<Bombinomicon>(
              "Bombinomicon",
              "GUTS AN' GLORY, LADS!",
              "An ancient tome literally full to bursting with explosive knowledge." + "\n\nPeople who would read this recreationally probably huff gunpowder.",
              "bombinomicon_icon") as PlayerItem;
            ItemBuilder.SetCooldownType(item, ItemBuilder.CooldownType.Damage, 200);
            ItemBuilder.AddPassiveStatModifier(item, PlayerStats.StatType.Curse, 1f, StatModifier.ModifyMethod.ADDITIVE);
            item.consumable = false;
            item.quality = ItemQuality.B;
            item.AddToSubShop(ItemBuilder.ShopType.Cursula);
            ID = item.PickupObjectId;
        }
        public static int ID;

        public override void DoEffect(PlayerController user)
        {
            int bombToSpawn = 108;
            if (user.HasPickupID(109) || user.HasPickupID(364) || user.HasPickupID(170)) bombToSpawn = 109;

            if (user.HasPickupID(118)) RandomBombSpawn(user, bombToSpawn);

            List<AIActor> activeEnemies = user.CurrentRoom.GetActiveEnemies(RoomHandler.ActiveEnemyType.All);
            if (activeEnemies != null)
            {
                for (int i = 0; i < activeEnemies.Count; i++)
                {
                    AIActor enemy = activeEnemies[i];
                    SpawnObjectPlayerItem bigbombPrefab = PickupObjectDatabase.GetById(bombToSpawn).GetComponent<SpawnObjectPlayerItem>();
                    GameObject bigbombObject = bigbombPrefab.objectToSpawn.gameObject;

                    GameObject bigbombObject2 = UnityEngine.Object.Instantiate<GameObject>(bigbombObject, enemy.sprite.WorldBottomCenter, Quaternion.identity);
                    tk2dBaseSprite bombsprite = bigbombObject2.GetComponent<tk2dBaseSprite>();
                    if (bombsprite)
                    {
                        bombsprite.PlaceAtPositionByAnchor(enemy.sprite.WorldBottomCenter, tk2dBaseSprite.Anchor.MiddleCenter);
                    }
                    if (user.HasPickupID(19) || user.HasPickupID(332))
                    {
                        RestoreAmmo(user, activeEnemies.Count);
                        SpawnObjectPlayerItem secondarybigbombPrefab = PickupObjectDatabase.GetById(bombToSpawn).GetComponent<SpawnObjectPlayerItem>();
                        GameObject secondarybigbombObject = secondarybigbombPrefab.objectToSpawn.gameObject;

                        GameObject secondarybigbombObject2 = UnityEngine.Object.Instantiate<GameObject>(secondarybigbombObject, enemy.sprite.WorldBottomCenter, Quaternion.identity);
                        tk2dBaseSprite secondarybombsprite = secondarybigbombObject2.GetComponent<tk2dBaseSprite>();
                        if (secondarybombsprite)
                        {
                            secondarybombsprite.PlaceAtPositionByAnchor(enemy.sprite.WorldBottomCenter, tk2dBaseSprite.Anchor.MiddleCenter);
                        }
                    }
                }
            }
        }
        private void RestoreAmmo(PlayerController user, int amount)
        {
            foreach (Gun gun in user.inventory.AllGuns)
            {
                if (gun.PickupObjectId == 19 || gun.PickupObjectId == 332)
                {
                    gun.GainAmmo(amount);
                }
            }
        }
        private void RandomBombSpawn(PlayerController user, int bombToSpawn)
        {
            for (int i = 0; i < 10; i++)
            {
                Vector3 positionToSpawn3 = user.CurrentRoom.GetRandomVisibleClearSpot(1, 1).ToVector3();

                SpawnObjectPlayerItem bigbombPrefab = PickupObjectDatabase.GetById(bombToSpawn).GetComponent<SpawnObjectPlayerItem>();
                GameObject bigbombObject = bigbombPrefab.objectToSpawn.gameObject;

                GameObject bigbombObject2 = UnityEngine.Object.Instantiate<GameObject>(bigbombObject, positionToSpawn3, Quaternion.identity);
                tk2dBaseSprite bombsprite = bigbombObject2.GetComponent<tk2dBaseSprite>();
                if (bombsprite)
                {
                    bombsprite.PlaceAtPositionByAnchor(positionToSpawn3, tk2dBaseSprite.Anchor.MiddleCenter);
                }
                if (user.HasPickupID(19) || user.HasPickupID(332))
                {
                    RestoreAmmo(user, 10);
                    SpawnObjectPlayerItem secondarybigbombPrefab = PickupObjectDatabase.GetById(bombToSpawn).GetComponent<SpawnObjectPlayerItem>();
                    GameObject secondarybigbombObject = secondarybigbombPrefab.objectToSpawn.gameObject;

                    GameObject secondarybigbombObject2 = UnityEngine.Object.Instantiate<GameObject>(secondarybigbombObject, positionToSpawn3, Quaternion.identity);
                    tk2dBaseSprite secondarybombsprite = secondarybigbombObject2.GetComponent<tk2dBaseSprite>();
                    if (secondarybombsprite)
                    {
                        secondarybombsprite.PlaceAtPositionByAnchor(positionToSpawn3, tk2dBaseSprite.Anchor.MiddleCenter);
                    }
                }
            }
        }
    }
}