using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Alexandria.ItemAPI;
using Alexandria.VisualAPI;
using Gungeon;
using UnityEngine;

namespace NevernamedsItems
{
    public class BlueShell : PassiveItem
    {
        public static void Init()
        {
            PickupObject item = ItemSetup.NewItem<BlueShell>(
            "Blue Shell",
            "Catch-Up Mechanic",
            "These shells are feared by Gundead and Gungeoneer alike for their relentless hunting nature. To be sought by a blue shell, is to be found by a blue shell...",
            "blueshell_icon");
            item.quality = PickupObject.ItemQuality.B;
            item.SetTag("bullet_modifier");

            ID = item.PickupObjectId;
            Doug.AddToLootPool(item.PickupObjectId);
        }
        public static int ID;
        public override void Pickup(PlayerController player)
        {
            player.PostProcessProjectile += PostProcess; 
            base.Pickup(player);
        }
        public override void DisableEffect(PlayerController player)
        {
            if (player) { player.PostProcessProjectile -= PostProcess; }
            base.DisableEffect(player);
        }
        private void PostProcess(Projectile p, float i)
        {
            if (p && UnityEngine.Random.value <= (0.15f * i))
            {
                p.AdjustPlayerProjectileTint(Color.blue, 2);
                if (p.GetComponent<ImprovedAfterImage>() == null && p.sprite != null)
                {
                    ImprovedAfterImage afterImage = p.gameObject.AddComponent<ImprovedAfterImage>();
                    afterImage.spawnShadows = true;
                    afterImage.shadowLifetime = (UnityEngine.Random.Range(0.1f, 0.2f));
                    afterImage.shadowTimeDelay = 0.003f;
                    afterImage.dashColor = Color.blue;
                    afterImage.name = "BlueShellsTrail";
                }
                HomingModifier hom = p.gameObject.GetOrAddComponent<HomingModifier>();
                hom.AngularVelocity += 1200f;
                hom.HomingRadius += 600f;
                p.baseData.range = 1000f;
                p.baseData.damage *= 1.2f;
            }
        }
    }
}
