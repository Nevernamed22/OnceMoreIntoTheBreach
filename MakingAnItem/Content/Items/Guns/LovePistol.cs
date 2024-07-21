
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

    public class LovePistol : GunBehaviour
    {
        public static void Add()
        {
            Gun gun = ETGMod.Databases.Items.NewGun("Love Pistol", "lovepistol");
            Game.Items.Rename("outdated_gun_mods:love_pistol", "nn:love_pistol");
            gun.gameObject.AddComponent<LovePistol>();
            gun.SetShortDescription(";)");
            gun.SetLongDescription("A low powered pistol, formerly kept in the back pocket of Hespera, the Pride of Venus, for times of need.");

            Alexandria.Assetbundle.GunInt.SetupSprite(gun, Initialisation.gunCollection, "lovepistol_idle_001", 8, "lovepistol_ammonomicon_001");

            gun.SetAnimationFPS(gun.shootAnimation, 15);

            gun.AddProjectileModuleFrom(PickupObjectDatabase.GetById(86) as Gun, true, false);
            gun.gunSwitchGroup = (PickupObjectDatabase.GetById(199) as Gun).gunSwitchGroup;

            //GUN STATS
            gun.DefaultModule.ammoCost = 1;
            gun.DefaultModule.shootStyle = ProjectileModule.ShootStyle.SemiAutomatic;
            gun.DefaultModule.sequenceStyle = ProjectileModule.ProjectileSequenceStyle.Random;
            gun.reloadTime = 0.8f;
            gun.DefaultModule.cooldownTime = 0.2f;
            gun.DefaultModule.numberOfShotsInClip = 9;
            gun.SetBarrel(22, 12);
            gun.SetBaseMaxAmmo(400);
            gun.gunClass = GunClass.CHARM;

            //BULLET STATS
            Projectile projectile = ProjectileSetupUtility.MakeProjectile(86, 4f);
            gun.DefaultModule.projectiles[0] = projectile;
            projectile.baseData.speed *= 0.9f;
            projectile.AppliesCharm = true;
            projectile.CharmApplyChance = 1f;
            projectile.charmEffect = StaticStatusEffects.charmingRoundsEffect;
            projectile.SetProjectileSprite("lovepistol_projectile", 7, 6, true, tk2dBaseSprite.Anchor.MiddleCenter, 7, 6);

            gun.AddShellCasing(1, 0, 0, 0, "shell_pink");
            gun.AddClipSprites("lovepistol");

            gun.muzzleFlashEffects = VFXToolbox.CreateVFXPoolBundle("LovePistolMuzzle", new IntVector2(11,15), tk2dBaseSprite.Anchor.MiddleLeft, false, 0, VFXAlignment.Fixed, 10, new Color32(250, 0, 0, 255));
          
            projectile.hitEffects.overrideMidairDeathVFX = VFXToolbox.CreateVFXBundle("LovePistolImpact", new IntVector2(15, 17), tk2dBaseSprite.Anchor.MiddleCenter, true, 5f);
            projectile.hitEffects.alwaysUseMidair = true;
          
            gun.quality = PickupObject.ItemQuality.B;
            ETGMod.Databases.Items.Add(gun, null, "ANY");

            gun.AddToSubShop(ItemBuilder.ShopType.Cursula);
            LovePistolID = gun.PickupObjectId;
        }
        public static int LovePistolID;
        public override void PostProcessProjectile(Projectile projectile)
        {
            if (projectile && projectile.ProjectilePlayerOwner())
            {
                if (projectile.ProjectilePlayerOwner().PlayerHasActiveSynergy("Toxic Love")) { projectile.baseData.damage *= 2; }
                if (projectile.ProjectilePlayerOwner().PlayerHasActiveSynergy("Everlasting Love"))
                {
                    projectile.charmEffect = GameManager.Instance.Dungeon.sharedSettingsPrefab.DefaultPermanentCharmEffect;
                }
            }
            base.PostProcessProjectile(projectile);
        }
    }
}