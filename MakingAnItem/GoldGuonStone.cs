using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using ItemAPI;
using System.Collections;
using System.Reflection;
using MonoMod.RuntimeDetour;

namespace NevernamedsItems
{
    // Token: 0x02000034 RID: 52
    internal class GoldGuonStone : IounStoneOrbitalItem
    {
        // Token: 0x0600017B RID: 379 RVA: 0x0000E9D4 File Offset: 0x0000CBD4
        public static void Init()
        {
            string name = "Gold Guon Stone";
            string resourcePath = "NevernamedsItems/Resources/goldguonstone_icon";
            GameObject gameObject = new GameObject();
            GoldGuonStone item = gameObject.AddComponent<GoldGuonStone>();
            ItemBuilder.AddSpriteToObject(name, resourcePath, gameObject);
            string shortDesc = "Greedy Rock";
            string longDesc = "This opulent stone will occasionally suck the casings right off of enemy bullets that make contact with it."+"\n\nDespite being illogical, bullets in the Gungeon are often fired casing and all for extra damage. That's 65% more bullet per bullet!";
            
            ItemBuilder.SetupItem(item, shortDesc, longDesc, "nn");

            item.quality = PickupObject.ItemQuality.A;
            GoldGuonStone.BuildPrefab();
            item.OrbitalPrefab = GoldGuonStone.orbitalPrefab;
            item.Identifier = IounStoneOrbitalItem.IounStoneIdentifier.GENERIC;
        }

        // Token: 0x0600017C RID: 380 RVA: 0x0000EA44 File Offset: 0x0000CC44
        public static int cashSpawnedThisRoom;
        public static void BuildPrefab()
        {
            string value = "I don't know what this string is for";
            string.IsNullOrEmpty(value);
            bool flag = GoldGuonStone.orbitalPrefab != null;
            if (!flag)
            {
                GameObject gameObject = SpriteBuilder.SpriteFromResource("NevernamedsItems/Resources/goldguonstone_ingame");
                gameObject.name = "Gold Guon Orbital";
                SpeculativeRigidbody speculativeRigidbody = gameObject.GetComponent<tk2dSprite>().SetUpSpeculativeRigidbody(IntVector2.Zero, new IntVector2(7, 13));
                speculativeRigidbody.CollideWithTileMap = false;
                speculativeRigidbody.CollideWithOthers = true;
                speculativeRigidbody.PrimaryPixelCollider.CollisionLayer = CollisionLayer.EnemyBulletBlocker;
                GoldGuonStone.orbitalPrefab = gameObject.AddComponent<PlayerOrbital>();
                GoldGuonStone.orbitalPrefab.motionStyle = PlayerOrbital.OrbitalMotionStyle.ORBIT_PLAYER_ALWAYS;
                GoldGuonStone.orbitalPrefab.shouldRotate = false;
                GoldGuonStone.orbitalPrefab.orbitRadius = 2.5f;
                GoldGuonStone.orbitalPrefab.SetOrbitalTier(0);
                GoldGuonStone.orbitalPrefab.orbitDegreesPerSecond = 90f;
                UnityEngine.Object.DontDestroyOnLoad(gameObject);
                FakePrefab.MarkAsFakePrefab(gameObject);
                gameObject.SetActive(false);
            }
        }

        // Token: 0x0600017D RID: 381 RVA: 0x0000EB24 File Offset: 0x0000CD24
        public override void Pickup(PlayerController player)
        {
            GoldGuonStone.guonHook = new Hook(typeof(PlayerOrbital).GetMethod("Initialize"), typeof(GoldGuonStone).GetMethod("GuonInit"));
            player.gameObject.AddComponent<GoldGuonStone.MirrorGuonBehavior>();
            GameManager.Instance.OnNewLevelFullyLoaded += this.FixGuon;
            player.OnEnteredCombat += this.resetCash;
            base.Pickup(player);
        }

        // Token: 0x0600017E RID: 382 RVA: 0x0000EB90 File Offset: 0x0000CD90
        private void FixGuon()
        {
            bool flag = base.Owner && base.Owner.GetComponent<GoldGuonStone.MirrorGuonBehavior>() != null;
            bool flag2 = flag;
            bool flag3 = flag2;
            if (flag3)
            {
                base.Owner.GetComponent<GoldGuonStone.MirrorGuonBehavior>().Destroy();
            }
            PlayerController owner = base.Owner;
            owner.gameObject.AddComponent<GoldGuonStone.MirrorGuonBehavior>();
        }

        // Token: 0x0600017F RID: 383 RVA: 0x0000EBF0 File Offset: 0x0000CDF0
        public override DebrisObject Drop(PlayerController player)
        {
            player.GetComponent<GoldGuonStone.MirrorGuonBehavior>().Destroy();
            GoldGuonStone.guonHook.Dispose();
            GameManager.Instance.OnNewLevelFullyLoaded -= this.FixGuon;
            player.OnEnteredCombat -= this.resetCash;
            return base.Drop(player);
        }

        // Token: 0x06000180 RID: 384 RVA: 0x0000EC38 File Offset: 0x0000CE38
        protected override void OnDestroy()
        {
            GoldGuonStone.guonHook.Dispose();
            bool flag = base.Owner && base.Owner.GetComponent<GoldGuonStone.MirrorGuonBehavior>() != null;
            bool flag2 = flag;
            bool flag3 = flag2;
            if (flag3)
            {
                base.Owner.GetComponent<GoldGuonStone.MirrorGuonBehavior>().Destroy();
            }
            GameManager.Instance.OnNewLevelFullyLoaded -= this.FixGuon;
            Owner.OnEnteredCombat -= this.resetCash;
            base.OnDestroy();
        }

        private void resetCash()
        {
            cashSpawnedThisRoom = 0;
        }

        // Token: 0x06000181 RID: 385 RVA: 0x0000ECAB File Offset: 0x0000CEAB
        public static void GuonInit(Action<PlayerOrbital, PlayerController> orig, PlayerOrbital self, PlayerController player)
        {
            orig(self, player);
        }
        // Token: 0x04000083 RID: 131
        public static Hook guonHook;

        // Token: 0x04000084 RID: 132
        public static PlayerOrbital orbitalPrefab;

        // Token: 0x02000067 RID: 103
        private class MirrorGuonBehavior : BraveBehaviour
        {
            // Token: 0x06000275 RID: 629 RVA: 0x00014FE0 File Offset: 0x000131E0
            private void Start()
            {
                this.owner = base.GetComponent<PlayerController>();
                foreach (IPlayerOrbital playerOrbital in this.owner.orbitals)
                {
                    PlayerOrbital playerOrbital2 = (PlayerOrbital)playerOrbital;
                    SpeculativeRigidbody specRigidbody = playerOrbital2.specRigidbody;
                    specRigidbody.OnPreRigidbodyCollision = (SpeculativeRigidbody.OnPreRigidbodyCollisionDelegate)Delegate.Combine(specRigidbody.OnPreRigidbodyCollision, new SpeculativeRigidbody.OnPreRigidbodyCollisionDelegate(this.OnPreCollision));
                }
            }
            // Token: 0x06000276 RID: 630 RVA: 0x00015074 File Offset: 0x00013274
            private PlayerController owner;
            private void OnPreCollision(SpeculativeRigidbody myRigidbody, PixelCollider myCollider, SpeculativeRigidbody other, PixelCollider otherCollider)
            {
                Projectile component = other.GetComponent<Projectile>();
                bool flag = component != null && !(component.Owner is PlayerController);
                if (flag)
                {
                   if (UnityEngine.Random.value < 0.1f && cashSpawnedThisRoom < 20)
                    {
                        LootEngine.SpawnItem(PickupObjectDatabase.GetById(68).gameObject, other.specRigidbody.UnitCenter, Vector2.zero, 1f, false, true, false);
                        cashSpawnedThisRoom += 1;
                        //ETGModConsole.Log("Cash Spawned this room: " + cashSpawnedThisRoom);
                    }
                }
            }
            public void Destroy()
            {
                UnityEngine.Object.Destroy(this);
            }

            // Token: 0x04000158 RID: 344
        }
    }
}