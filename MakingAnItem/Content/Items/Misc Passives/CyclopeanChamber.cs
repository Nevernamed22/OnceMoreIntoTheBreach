using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Alexandria.Misc;
using Alexandria.ItemAPI;
using UnityEngine;

namespace NevernamedsItems
{
    public class CyclopeanChamber : PassiveItem
    {
        public static void Init()
        {
            PickupObject item = ItemSetup.NewItem<CyclopeanChamber>(
            "Cyclopean Cylinder",
            "Make It Count",
            "Reduces clips to one shot, but increases damage for every bullet removed." + "\n\nOnce thought to be the cylinder of a powerful one-chambered firerarm, further research suggests this artefact may be the chamber of an ancient Elephant Gun.",
            "cyclopeancylinder_icon");
            item.AddPassiveStatModifier( PlayerStats.StatType.AdditionalClipCapacityMultiplier, 0.000000001f, StatModifier.ModifyMethod.MULTIPLICATIVE);
            item.AddPassiveStatModifier( PlayerStats.StatType.Curse, 1, StatModifier.ModifyMethod.ADDITIVE);
            item.quality = PickupObject.ItemQuality.B;
            ID = item.PickupObjectId;
        }
        public override void Pickup(PlayerController player)
        {
            player.PostProcessProjectile += PostProcess;
            base.Pickup(player);
        }
        public override DebrisObject Drop(PlayerController player)
        {
            player.PostProcessProjectile -= PostProcess;
            return base.Drop(player);
        }
        private void PostProcess(Projectile bullet, float flot)
        {
            if (bullet.ProjectilePlayerOwner())
            {
                Gun curGun = bullet.ProjectilePlayerOwner().CurrentGun;
                if (curGun && curGun.DefaultModule.shootStyle != ProjectileModule.ShootStyle.Beam)
                {
                    float realClipShots = curGun.ClipCapacity;
                    float prefabClipShots = curGun.DefaultModule.numberOfShotsInClip;
                    if (prefabClipShots - realClipShots > 0)
                    {
                        float multiplier = (prefabClipShots - realClipShots) * 0.25f;
                        if (curGun.reloadTime < 1) multiplier *= curGun.reloadTime;
                        bullet.baseData.damage *= 1 + multiplier;
                    }
                }
            }
        }
        public override void OnDestroy()
        {
            if (Owner) Owner.PostProcessProjectile -= PostProcess;
            base.OnDestroy();
        }
        public static int ID;
    }
}
