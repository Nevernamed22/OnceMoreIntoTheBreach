using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using UnityEngine;
using ItemAPI;
using MonoMod.RuntimeDetour;
using System.Reflection;

namespace NevernamedsItems
{
    public class AntimagicRounds : PassiveItem
    {
        public static void Init()
        {
            string itemName = "Antimagic Rounds";
            string resourceName = "NevernamedsItems/Resources/antimagicrounds_icon";
            GameObject obj = new GameObject(itemName);
            var item = obj.AddComponent<AntimagicRounds>();
            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);
            string shortDesc = "Casting Time: 1 Action";
            string longDesc = "The arcane runes and nullifying antimagic field of these bullets allows them to break through the protective wards of Gunjurers with ease.";
            ItemBuilder.SetupItem(item, shortDesc, longDesc, "nn");
            item.quality = PickupObject.ItemQuality.B;

            item.AddToSubShop(ItemBuilder.ShopType.Cursula);

            AntimagicRoundsID = item.PickupObjectId;

            GunjurerCatchHook = new Hook(
                typeof(WizardSpinShootBehavior).GetMethod("OnTriggerCollision", BindingFlags.Instance | BindingFlags.NonPublic),
                typeof(AntimagicRounds).GetMethod("GunjurerPreCatch", BindingFlags.Static | BindingFlags.Public)
            );
        }
        public static int AntimagicRoundsID;
        public override void Pickup(PlayerController player)
        {
            base.Pickup(player);
            player.PostProcessProjectile += this.PostProcessProjectile;
            player.PostProcessBeam += this.PostProcessBeam;
        }
        private void PostProcessProjectile(Projectile sourceProjectile, float effectChanceScalar)
        {
            InstaKillEnemyTypeBehaviour instakill = sourceProjectile.gameObject.GetOrAddComponent<InstaKillEnemyTypeBehaviour>();
            instakill.EnemyTypeToKill.AddRange(EasyEnemyTypeLists.ModInclusiveMagicEnemies);
        }
        private void PostProcessBeam(BeamController sourceBeam)
        {
            if (sourceBeam.projectile)
            {
                this.PostProcessProjectile(sourceBeam.projectile, 1);
            }
        }
        public override DebrisObject Drop(PlayerController player)
        {
            DebrisObject debrisObject = base.Drop(player);
            player.PostProcessProjectile -= this.PostProcessProjectile;
            player.PostProcessBeam -= this.PostProcessBeam;
            return debrisObject;
        }
        public override void OnDestroy()
        {
            if (Owner)
            {
                Owner.PostProcessProjectile -= this.PostProcessProjectile;
                Owner.PostProcessBeam -= this.PostProcessBeam;
            }
            base.OnDestroy();
        }
        public static Hook GunjurerCatchHook;
        public static void GunjurerPreCatch(Action<WizardSpinShootBehavior, SpeculativeRigidbody, SpeculativeRigidbody, CollisionData> orig, WizardSpinShootBehavior self, SpeculativeRigidbody specRigidbody, SpeculativeRigidbody sourceSpecRigidbody, CollisionData collisionData)
        {
            Projectile proj = collisionData.OtherRigidbody.GetComponent<Projectile>();
            bool flag = proj && proj.ProjectilePlayerOwner() && proj.ProjectilePlayerOwner().HasPickupID(AntimagicRoundsID);
            if (!flag)
            {
                orig(self, specRigidbody, sourceSpecRigidbody, collisionData);
            }
        }

    }

}

