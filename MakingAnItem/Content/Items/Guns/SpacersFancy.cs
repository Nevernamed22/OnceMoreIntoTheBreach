
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using Gungeon;
using MonoMod;
using UnityEngine;
using Alexandria.ItemAPI;
using Alexandria.Misc;

namespace NevernamedsItems
{

    public class SpacersFancy : AdvancedGunBehavior
    {
        public static void Add()
        {
            Gun gun = ETGMod.Databases.Items.NewGun("Spacers Fancy", "spacersfancy");
            Game.Items.Rename("outdated_gun_mods:spacers_fancy", "nn:spacers_fancy");
            gun.gameObject.AddComponent<SpacersFancy>();
            gun.SetShortDescription("Not the Best Choice");
            gun.SetLongDescription("A cheaply made sidearm from a far flung solar system."+"\n\nBecomes stronger each time it's slinger basks in the light of glorious commerce.");
            gun.SetupSprite(null, "spacersfancy_idle_001", 8);
            gun.SetAnimationFPS(gun.shootAnimation, 15);
            gun.SetAnimationFPS(gun.reloadAnimation, 0);

            gun.AddProjectileModuleFrom(PickupObjectDatabase.GetById(56) as Gun, true, false);

            //GUN STATS
            gun.DefaultModule.ammoCost = 1;
            gun.DefaultModule.shootStyle = ProjectileModule.ShootStyle.SemiAutomatic;
            gun.DefaultModule.sequenceStyle = ProjectileModule.ProjectileSequenceStyle.Random;
            gun.reloadTime = 0.9f;
            gun.DefaultModule.cooldownTime = 0.14f;
            gun.DefaultModule.numberOfShotsInClip = 9;
            gun.barrelOffset.transform.localPosition = new Vector3(25f / 16f, 13f / 16f, 0f);
            gun.SetBaseMaxAmmo(330);
            gun.gunClass = GunClass.PISTOL;

            //BULLET STATS
            Projectile projectile = gun.DefaultModule.projectiles[0].InstantiateAndFakeprefab();
            gun.DefaultModule.projectiles[0] = projectile;
            projectile.baseData.speed *= 1.2f;
            projectile.SetProjectileSpriteRight("spacersfancy_proj", 9, 4, true, tk2dBaseSprite.Anchor.MiddleCenter, 9, 4);

            
            gun.quality = PickupObject.ItemQuality.C;
            ETGMod.Databases.Items.Add(gun, false, "ANY");

            ID = gun.PickupObjectId;
        }
        public static int ID;
        public int timesPurchased;
        public override void PostProcessProjectile(Projectile projectile)
        {
            projectile.baseData.damage *= ((timesPurchased * 0.1f) + 1);
            if (projectile.ProjectilePlayerOwner() && projectile.ProjectilePlayerOwner().PlayerHasActiveSynergy("Mag-2-Zap"))
            {
                projectile.AppliesStun = true;
                projectile.StunApplyChance += (timesPurchased * 0.1f);
                projectile.AppliedStunDuration = 1;
            }
            base.PostProcessProjectile(projectile);
        }

        private void Purchase(PlayerController player, ShopItemController item)
        {
            timesPurchased++;
        }
        protected override void OnPickedUpByPlayer(PlayerController player)
        {
            player.OnItemPurchased += Purchase;
            base.OnPickedUpByPlayer(player);
        }
        protected override void OnPostDroppedByPlayer(PlayerController player)
        {
            player.OnItemPurchased -= Purchase;
            base.OnPostDroppedByPlayer(player);
        }
        public override void OnDestroy()
        {
            if (gun && gun.GunPlayerOwner()) { gun.GunPlayerOwner().OnItemPurchased += Purchase; }
            base.OnDestroy();
        }

    }  
}