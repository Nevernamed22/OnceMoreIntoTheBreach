using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using UnityEngine;
using Alexandria.ItemAPI;
using MonoMod.RuntimeDetour;
using System.Reflection;
using Alexandria.Misc;

namespace NevernamedsItems
{
    public class AntimagicRounds : PassiveItem
    {
        public static void Init()
        {
            PickupObject item = ItemSetup.NewItem<AntimagicRounds>(
            "Antimagic Rounds",
            "Casting Time: 1 Action",
            "Instantly slays magical enemies.\n\nThe arcane runes and nullifying antimagic field of these bullets allows them to break through the protective wards of Gunjurers with ease.",
            "antimagicrounds_improved");
            item.quality = PickupObject.ItemQuality.B;
            item.SetTag("bullet_modifier");
            item.AddToSubShop(ItemBuilder.ShopType.Cursula);
            Doug.AddToLootPool(item.PickupObjectId);

            ID = item.PickupObjectId;
        }
        public static int ID;

        public override void Pickup(PlayerController player)
        {
            base.Pickup(player);
            player.PostProcessProjectile += this.PostProcessProjectile;
            player.PostProcessBeam += this.PostProcessBeam;
        }
        private void PostProcessProjectile(Projectile sourceProjectile, float effectChanceScalar)
        {
            SpriteSparkler particles = sourceProjectile.gameObject.GetOrAddComponent<SpriteSparkler>();
            particles.doVFX = true;
            particles.VFX = SharedVFX.PinkSparkle;
            particles.particlesPerSecond = 20f;

            ProjectileInstakillBehaviour instakill = sourceProjectile.gameObject.GetOrAddComponent<ProjectileInstakillBehaviour>();
            instakill.tagsToKill.AddRange(new List<string> { "gunjurer", "gunsinger", "bookllet" });
            instakill.enemyGUIDsToKill.AddRange(new List<string> { GUIDs.Wizbang, GUIDs.Gun_Fairy });
            instakill.vfx = (PickupObjectDatabase.GetById(57) as Gun).DefaultModule.projectiles[0].hitEffects.tileMapVertical.effects[0].effects[0].effect;
            instakill.extraKnockback += 30f;
            instakill.onInstaKill += OnInstaKill;
            instakill.soundEvents.Add("Play_WPN_spellactionrevolver_shot_01");
        }
        public static void OnInstaKill(Projectile bullet, AIActor actor)
        {
            for (int i = 0; i < 20; i++)
            {
                GameObject sparkleinst = UnityEngine.Object.Instantiate(SharedVFX.PinkSparkle, actor.sprite.WorldCenter, Quaternion.identity);
                SimpleMover orAddComponent = sparkleinst.GetOrAddComponent<SimpleMover>();
                orAddComponent.velocity = BraveUtility.RandomAngle().DegreeToVector2() * UnityEngine.Random.Range(5, 10) * 0.4f;
                orAddComponent.acceleration = orAddComponent.velocity / 1.3f * -1f;
            }
        }
        private void PostProcessBeam(BeamController sourceBeam)
        {
            if (sourceBeam.projectile) this.PostProcessProjectile(sourceBeam.projectile, 1);
        }
        public override void DisableEffect(PlayerController player)
        {
            if (player)
            {
                player.PostProcessProjectile -= this.PostProcessProjectile;
                player.PostProcessBeam -= this.PostProcessBeam;
            }
            base.DisableEffect(player);
        }
    }
}

