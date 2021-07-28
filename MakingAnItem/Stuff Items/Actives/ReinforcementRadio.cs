using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Dungeonator;
using ItemAPI;
using UnityEngine;

namespace NevernamedsItems
{
    class ReinforcementRadio : PlayerItem
    {
        //Call this method from the Start() method of your ETGModule extension class
        public static void Init()
        {
            //The name of the item
            string itemName = "Reinforcement Radio";

            //Refers to an embedded png in the project. Make sure to embed your resources! Google it.
            string resourceName = "NevernamedsItems/Resources/reinforcementradio_icon";

            //Create new GameObject
            GameObject obj = new GameObject(itemName);

            //Add a ActiveItem component to the object
            var item = obj.AddComponent<ReinforcementRadio>();

            //Adds a tk2dSprite component to the object and adds your texture to the item sprite collection
            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);

            //Ammonomicon entry variables
            string shortDesc = "I Need Backup!";
            string longDesc = "Taps into secret Gundead radio frequencies to confuse their reinforcement divisions, and get them to send aid to the wrong side."+"\n\nGundead are not the brightest of creatures.";

            //Adds the item to the gungeon item list, the ammonomicon, the loot table, etc.
            //"kts" here is the item pool. In the console you'd type kts:sweating_bullets
            ItemBuilder.SetupItem(item, shortDesc, longDesc, "nn");

            //Set the cooldown type and duration of the cooldown
            ItemBuilder.SetCooldownType(item, ItemBuilder.CooldownType.Damage, 500);

            //Adds a passive modifier, like curse, coolness, damage, etc. to the item. Works for passives and actives.

            //Set some other fields
            item.consumable = false;
            item.quality = ItemQuality.B;

            //SYNERGIES
            List<string> mandatorySynergyItems = new List<string>() { "nn:reinforcement_radio", "magnum" };
            CustomSynergies.Add("Bullet+", mandatorySynergyItems);
            List<string> mandatorySynergyItems2 = new List<string>() { "nn:reinforcement_radio", "regular_shotgun" };
            CustomSynergies.Add("Red Shotgun+", mandatorySynergyItems2);
            List<string> mandatorySynergyItems3 = new List<string>() { "nn:reinforcement_radio", "winchester" };
            CustomSynergies.Add("Blue Shotgun+", mandatorySynergyItems3);
            List<string> mandatorySynergyItems4 = new List<string>() { "nn:reinforcement_radio", "machine_pistol" };
            CustomSynergies.Add("Bandana+", mandatorySynergyItems4);
            List<string> mandatorySynergyItems5 = new List<string>() { "nn:reinforcement_radio", "ak47" };
            CustomSynergies.Add("Tank+", mandatorySynergyItems5);
            List<string> mandatorySynergyItems6 = new List<string>() { "nn:reinforcement_radio", "pitchfork" };
            CustomSynergies.Add("Devil+", mandatorySynergyItems6);
            List<string> mandatorySynergyItems7 = new List<string>() { "nn:reinforcement_radio", "huntsman" };
            CustomSynergies.Add("Execution+", mandatorySynergyItems7);
            List<string> mandatorySynergyItems8 = new List<string>() { "nn:reinforcement_radio", "sniper_rifle" };
            CustomSynergies.Add("Sniper+", mandatorySynergyItems8);
            List<string> mandatorySynergyItems9 = new List<string>() { "nn:reinforcement_radio", "thompson" };
            CustomSynergies.Add("Spirit+", mandatorySynergyItems9);
            List<string> mandatorySynergyItems10 = new List<string>() { "nn:reinforcement_radio", "bow" };
            CustomSynergies.Add("Arrow+", mandatorySynergyItems10);
            List<string> mandatorySynergyItems11 = new List<string>() { "nn:reinforcement_radio", "the_scrambler" };
            CustomSynergies.Add("Egg+", mandatorySynergyItems11);
            List<string> mandatorySynergyItems12 = new List<string>() { "nn:reinforcement_radio", "bubble_blaster" };
            CustomSynergies.Add("Bubble+", mandatorySynergyItems12);
            List<string> mandatorySynergyItems13 = new List<string>() { "nn:reinforcement_radio", "trank_gun" };
            CustomSynergies.Add("Eye+", mandatorySynergyItems13);
            //SYNERGIES WITH MULTIPLE ITEMS INVOLVED
            List<string> mandatorySynergyItems14 = new List<string>() { "nn:reinforcement_radio" };
            List<string> optionalSynergyItems14 = new List<string>() { "bomb", "ice_bomb", "lil_bomber", "cluster_mine" };
            CustomSynergies.Add("Bomb+", mandatorySynergyItems14, optionalSynergyItems14);
            List<string> mandatorySynergyItemsOilSlick = new List<string>() { "nn:reinforcement_radio" };
            List<string> optionalSynergyItemsOilSlick = new List<string>() { "bundle_of_wands", "hexagun", "magic_bullets" };
            CustomSynergies.Add("Magic+", mandatorySynergyItemsOilSlick, optionalSynergyItemsOilSlick);
        }
        protected override void DoEffect(PlayerController user)
        {
            AkSoundEngine.PostEvent("Play_OBJ_supplydrop_activate_01", base.gameObject);
            int r = UnityEngine.Random.Range(1, 6);
            if (r == 1)
            {
                SpawnFourBulletKin();
            }
            else if (r == 2)
            {
                SpawnTwoShotgunKin();
            }
            else if (r == 3)
            {
                SpawnTwoBulletsAndABlueShotgunKin();
            }
            else if (r == 4)
            {
                SpawnBandanaKin();
            }
            else if (r == 5)
            {
                SpawnBombEnemies();
            }

            //Synergy Adds
            if (user.HasPickupID(38)) //Magnum --> Bullet Kin
            {
                string EnemyGuid = "01972dee89fc4404a5c408d50007dad5";
                SpawnBonus(EnemyGuid);
            }
            if (user.HasPickupID(51)) //Regular Shotgun --> Red Shotty
            {
                string EnemyGuid = "128db2f0781141bcb505d8f00f9e4d47";
                SpawnBonus(EnemyGuid);
            }
            if (user.HasPickupID(1)) //Winchester --> Blue Shotty
            {
                string EnemyGuid = "b54d89f9e802455cbb2b8a96a31e8259";
                SpawnBonus(EnemyGuid);
            }
            if (user.HasPickupID(43)) //Machine Pistol --> Bandnana Kin
            {
                string EnemyGuid = "88b6b6a93d4b4234a67844ef4728382c";
                SpawnBonus(EnemyGuid);
            }
            if (user.HasPickupID(15)) //ak47 --> Tanker
            {
                string EnemyGuid = "df7fb62405dc4697b7721862c7b6b3cd";
                SpawnBonus(EnemyGuid);
            }
            if (user.HasPickupID(336)) //pitchfork --> Fallen Bullet Kin
            {
                string EnemyGuid = "5f3abc2d561b4b9c9e72b879c6f10c7e";
                SpawnBonus(EnemyGuid);
            }
            if (user.HasPickupID(346)) //Huntsman --> Executioner
            {
                string EnemyGuid = "b1770e0f1c744d9d887cc16122882b4f";
                SpawnBonus(EnemyGuid);
            }
            if (user.HasPickupID(49)) //Sniper Rifle --> Sniper Shell
            {
                string EnemyGuid = "31a3ea0c54a745e182e22ea54844a82d";
                SpawnBonus(EnemyGuid);
            }
            if (user.HasPickupID(2)) //Thompson --> Hollowpoint
            {
                string EnemyGuid = "4db03291a12144d69fe940d5a01de376";
                SpawnBonus(EnemyGuid);
            }
            if (user.HasPickupID(8)) //Bow --> Arrow Kin
            {
                string EnemyGuid = "05891b158cd542b1a5f3df30fb67a7ff";
                SpawnBonus(EnemyGuid);
            }
            if (user.HasPickupID(108) || user.HasPickupID(109) || user.HasPickupID(332) || user.HasPickupID(308)) //Bomb, Ice Bomb, Lil Bomber, or Cluster Mine --> Pinhead.
            {
                string EnemyGuid = "4d37ce3d666b4ddda8039929225b7ede";
                SpawnBonus(EnemyGuid);
            }
            if (user.HasPickupID(445)) //Scrambler --> Gigi
            {
                string EnemyGuid = "ed37fa13e0fa4fcf8239643957c51293";
                SpawnBonus(EnemyGuid);
            }
            if (user.HasPickupID(61) || user.HasPickupID(385) || user.HasPickupID(533)) //Bundle of Wands, Hexagun, Magic Bullets --> Apprentice Gunjurer
            {
                string EnemyGuid = "206405acad4d4c33aac6717d184dc8d4";
                SpawnBonus(EnemyGuid);
            }
            if (user.HasPickupID(599)) //Bubble Blaster --> Gunzookie
            {
                string EnemyGuid = "6e972cd3b11e4b429b888b488e308551";
                SpawnBonus(EnemyGuid);
            }
            if (user.HasPickupID(42)) //Trank Gun --> Beadie
            {
                string EnemyGuid = "7b0b1b6d9ce7405b86b75ce648025dd6";
                SpawnBonus(EnemyGuid);
            }
        }

        private void SpawnBonus(string EnemyGuid)
        {
            var Enemy = EnemyDatabase.GetOrLoadByGuid(EnemyGuid);
            IntVector2? bestRewardLocation = LastOwner.CurrentRoom.GetRandomVisibleClearSpot(2, 2);
            AIActor TargetActor = AIActor.Spawn(Enemy.aiActor, bestRewardLocation.Value, GameManager.Instance.Dungeon.data.GetAbsoluteRoomFromPosition(bestRewardLocation.Value), true, AIActor.AwakenAnimationType.Default, true);
            TargetActor.ApplyEffect(GameManager.Instance.Dungeon.sharedSettingsPrefab.DefaultPermanentCharmEffect, 1f, null);
            PhysicsEngine.Instance.RegisterOverlappingGhostCollisionExceptions(TargetActor.specRigidbody, null, false);
            TargetActor.gameObject.AddComponent<KillOnRoomClear>();
            TargetActor.IsHarmlessEnemy = true;
            TargetActor.IgnoreForRoomClear = true;
            TargetActor.HandleReinforcementFallIntoRoom(0f);
        }
        private void SpawnFourBulletKin()
        {
            for (int i = 0; i < 4; i++)
            {
                try
                {
                    var BulletKin = EnemyDatabase.GetOrLoadByGuid("01972dee89fc4404a5c408d50007dad5");
                    //float radius = 5;
                    //var random = UnityEngine.Random.insideUnitCircle * radius;
                    //IntVector2 temp = random.ToIntVector2() + LastOwner.CurrentRoom.GetNearestCellToPosition(LastOwner.specRigidbody.UnitCenter).position; // or something like this to get the player's pos relative to the room
                    //IntVector2? spawnPos = LastOwner.CurrentRoom.GetRandomAvailableCell(temp);
                    IntVector2? spawnPos = LastOwner.CurrentRoom.GetRandomVisibleClearSpot(1, 1);
                    if (spawnPos != null)
                    {
                        AIActor TargetActor = AIActor.Spawn(BulletKin.aiActor, spawnPos.Value, GameManager.Instance.Dungeon.data.GetAbsoluteRoomFromPosition(spawnPos.Value), true, AIActor.AwakenAnimationType.Default, true);
                        TargetActor.ApplyEffect(GameManager.Instance.Dungeon.sharedSettingsPrefab.DefaultPermanentCharmEffect, 1f, null);
                        PhysicsEngine.Instance.RegisterOverlappingGhostCollisionExceptions(TargetActor.specRigidbody, null, false);
                        TargetActor.gameObject.AddComponent<KillOnRoomClear>();
                        TargetActor.IsHarmlessEnemy = true;
                        TargetActor.IgnoreForRoomClear = true;
                        TargetActor.HandleReinforcementFallIntoRoom(0f);
                    }
                }
                catch (Exception e)
                {
                    ETGModConsole.Log(e.Message);
                }
            }
        }
        private void SpawnTwoShotgunKin()
        {
            for (int i = 0; i < 2; i++)
            {
                var RedShotgun = EnemyDatabase.GetOrLoadByGuid("128db2f0781141bcb505d8f00f9e4d47");
                IntVector2? bestRewardLocation = LastOwner.CurrentRoom.GetRandomVisibleClearSpot(2, 2);
                AIActor TargetActor = AIActor.Spawn(RedShotgun.aiActor, bestRewardLocation.Value, GameManager.Instance.Dungeon.data.GetAbsoluteRoomFromPosition(bestRewardLocation.Value), true, AIActor.AwakenAnimationType.Default, true);
                TargetActor.ApplyEffect(GameManager.Instance.Dungeon.sharedSettingsPrefab.DefaultPermanentCharmEffect, 1f, null);
                PhysicsEngine.Instance.RegisterOverlappingGhostCollisionExceptions(TargetActor.specRigidbody, null, false);
                TargetActor.gameObject.AddComponent<KillOnRoomClear>();
                TargetActor.IsHarmlessEnemy = true;
                TargetActor.IgnoreForRoomClear = true;
                TargetActor.HandleReinforcementFallIntoRoom(0f);
            }
        }
        private void SpawnTwoBulletsAndABlueShotgunKin()
        {
            for (int i = 0; i < 2; i++)
            {
                var BulletKin = EnemyDatabase.GetOrLoadByGuid("01972dee89fc4404a5c408d50007dad5");
                //Vector2? locationInRadius = UnityEngine.Random.insideUnitCircle;
                //vector.ToVector2Int;
                IntVector2? bestRewardLocation2 = LastOwner.CurrentRoom.GetRandomVisibleClearSpot(2, 2);
                AIActor TargetActor2 = AIActor.Spawn(BulletKin.aiActor, bestRewardLocation2.Value, GameManager.Instance.Dungeon.data.GetAbsoluteRoomFromPosition(bestRewardLocation2.Value), true, AIActor.AwakenAnimationType.Default, true);
                TargetActor2.ApplyEffect(GameManager.Instance.Dungeon.sharedSettingsPrefab.DefaultPermanentCharmEffect, 1f, null);
                PhysicsEngine.Instance.RegisterOverlappingGhostCollisionExceptions(TargetActor2.specRigidbody, null, false);
                TargetActor2.gameObject.AddComponent<KillOnRoomClear>();
                TargetActor2.IsHarmlessEnemy = true;
                TargetActor2.IgnoreForRoomClear = true;
                TargetActor2.HandleReinforcementFallIntoRoom(0f);
            }
            var BlueShotgun = EnemyDatabase.GetOrLoadByGuid("b54d89f9e802455cbb2b8a96a31e8259");
            IntVector2? bestRewardLocation = LastOwner.CurrentRoom.GetRandomVisibleClearSpot(2, 2);
            AIActor TargetActor = AIActor.Spawn(BlueShotgun.aiActor, bestRewardLocation.Value, GameManager.Instance.Dungeon.data.GetAbsoluteRoomFromPosition(bestRewardLocation.Value), true, AIActor.AwakenAnimationType.Default, true);
            TargetActor.ApplyEffect(GameManager.Instance.Dungeon.sharedSettingsPrefab.DefaultPermanentCharmEffect, 1f, null);
            PhysicsEngine.Instance.RegisterOverlappingGhostCollisionExceptions(TargetActor.specRigidbody, null, false);
            TargetActor.gameObject.AddComponent<KillOnRoomClear>();
            TargetActor.IsHarmlessEnemy = true;
            TargetActor.IgnoreForRoomClear = true;
            TargetActor.HandleReinforcementFallIntoRoom(0f);
        }
        private void SpawnBandanaKin()
        {
            for (int i = 0; i < 2; i++)
            {
                var BandanaKin = EnemyDatabase.GetOrLoadByGuid("88b6b6a93d4b4234a67844ef4728382c");
                //Vector2? locationInRadius = UnityEngine.Random.insideUnitCircle;
                //vector.ToVector2Int;
                IntVector2? bestRewardLocation2 = LastOwner.CurrentRoom.GetRandomVisibleClearSpot(2, 2);
                AIActor TargetActor2 = AIActor.Spawn(BandanaKin.aiActor, bestRewardLocation2.Value, GameManager.Instance.Dungeon.data.GetAbsoluteRoomFromPosition(bestRewardLocation2.Value), true, AIActor.AwakenAnimationType.Default, true);
                TargetActor2.ApplyEffect(GameManager.Instance.Dungeon.sharedSettingsPrefab.DefaultPermanentCharmEffect, 1f, null);
                PhysicsEngine.Instance.RegisterOverlappingGhostCollisionExceptions(TargetActor2.specRigidbody, null, false);
                TargetActor2.gameObject.AddComponent<KillOnRoomClear>();
                TargetActor2.IsHarmlessEnemy = true;
                TargetActor2.IgnoreForRoomClear = true;
                TargetActor2.HandleReinforcementFallIntoRoom(0f);
            }
        }
        private void SpawnBombEnemies()
        {
            for (int i = 0; i < 2; i++)
            {
                var Pinhead = EnemyDatabase.GetOrLoadByGuid("4d37ce3d666b4ddda8039929225b7ede");
                //Vector2? locationInRadius = UnityEngine.Random.insideUnitCircle;
                //vector.ToVector2Int;
                IntVector2? bestRewardLocation2 = LastOwner.CurrentRoom.GetRandomVisibleClearSpot(2, 2);
                AIActor TargetActor2 = AIActor.Spawn(Pinhead.aiActor, bestRewardLocation2.Value, GameManager.Instance.Dungeon.data.GetAbsoluteRoomFromPosition(bestRewardLocation2.Value), true, AIActor.AwakenAnimationType.Default, true);
                TargetActor2.ApplyEffect(GameManager.Instance.Dungeon.sharedSettingsPrefab.DefaultPermanentCharmEffect, 1f, null);
                PhysicsEngine.Instance.RegisterOverlappingGhostCollisionExceptions(TargetActor2.specRigidbody, null, false);
                TargetActor2.gameObject.AddComponent<KillOnRoomClear>();
                TargetActor2.IsHarmlessEnemy = true;
                TargetActor2.IgnoreForRoomClear = true;
                TargetActor2.HandleReinforcementFallIntoRoom(0f);
            }
            var Nitra = EnemyDatabase.GetOrLoadByGuid("c0260c286c8d4538a697c5bf24976ccf");
            IntVector2? bestRewardLocation = LastOwner.CurrentRoom.GetRandomVisibleClearSpot(2, 2);
            AIActor TargetActor = AIActor.Spawn(Nitra.aiActor, bestRewardLocation.Value, GameManager.Instance.Dungeon.data.GetAbsoluteRoomFromPosition(bestRewardLocation.Value), true, AIActor.AwakenAnimationType.Default, true);
            TargetActor.ApplyEffect(GameManager.Instance.Dungeon.sharedSettingsPrefab.DefaultPermanentCharmEffect, 1f, null);
            PhysicsEngine.Instance.RegisterOverlappingGhostCollisionExceptions(TargetActor.specRigidbody, null, false);
            TargetActor.gameObject.AddComponent<KillOnRoomClear>();
            TargetActor.IsHarmlessEnemy = true;
            TargetActor.IgnoreForRoomClear = true;
            TargetActor.HandleReinforcementFallIntoRoom(0f);
        }

    }
}
