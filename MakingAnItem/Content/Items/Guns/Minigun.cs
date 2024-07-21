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

    public class NNMinigun : GunBehaviour
    {
        public static void Add()
        {
            Gun gun = ETGMod.Databases.Items.NewGun("Mini Gun", "nnminigun2");
            Game.Items.Rename("outdated_gun_mods:mini_gun", "nn:mini_gun");
            gun.gameObject.AddComponent<NNMinigun>();
            gun.SetShortDescription("Misleading Name");
            gun.SetLongDescription("A tiny toy gun, probably pulled from the grip of a tiny toy soldier.");

            gun.SetGunSprites("nnminigun2", 8, false, 2);

            gun.SetAnimationFPS(gun.shootAnimation, 16);
            gun.SetAnimationFPS(gun.reloadAnimation, 16);
            gun.GetComponent<tk2dSpriteAnimator>().GetClipByName(gun.reloadAnimation).wrapMode = tk2dSpriteAnimationClip.WrapMode.Loop;
            gun.muzzleFlashEffects = (PickupObjectDatabase.GetById(79) as Gun).muzzleFlashEffects;

            gun.gunSwitchGroup = (PickupObjectDatabase.GetById(43) as Gun).gunSwitchGroup;

            gun.AddProjectileModuleFrom(PickupObjectDatabase.GetById(86) as Gun, true, false);
            gun.DefaultModule.ammoCost = 1;
            gun.DefaultModule.shootStyle = ProjectileModule.ShootStyle.SemiAutomatic;
            gun.DefaultModule.sequenceStyle = ProjectileModule.ProjectileSequenceStyle.Random;
            gun.reloadTime = 1.1f;
            gun.gunClass = GunClass.PISTOL;
            gun.DefaultModule.cooldownTime = 0.1f;
            gun.DefaultModule.numberOfShotsInClip = 4;
            gun.SetBaseMaxAmmo(60);
            gun.quality = PickupObject.ItemQuality.D;
            ETGMod.Databases.Items.Add(gun, null, "ANY");

            gun.SetBarrel(9, 5);

            Projectile projectile = UnityEngine.Object.Instantiate<Projectile>(gun.DefaultModule.projectiles[0]);
            projectile.gameObject.SetActive(false);
            FakePrefab.MarkAsFakePrefab(projectile.gameObject);
            UnityEngine.Object.DontDestroyOnLoad(projectile);
            gun.DefaultModule.projectiles[0] = projectile;
            projectile.AdditionalScaleMultiplier *= 0.5f;
            projectile.baseData.damage = 6f;

            gun.AddShellCasing(0, 0, 4, 1, "shell_tiny");

            gun.AddClipSprites("minigun");

            MiniGunID = gun.PickupObjectId;
        }
        public static int MiniGunID;
        public override void PostProcessProjectile(Projectile projectile)
        {
            if (projectile && projectile.ProjectilePlayerOwner())
            {
                if (projectile.ProjectilePlayerOwner().PlayerHasActiveSynergy("Micro Aggressions"))
                {
                    if (UnityEngine.Random.value <= 0.1f)
                    {
                        projectile.AdjustPlayerProjectileTint(ExtendedColours.vibrantOrange, 2);
                        projectile.statusEffectsToApply.Add(StatusEffectHelper.GenerateSizeEffect(5, new Vector2(0.4f, 0.4f)));
                    }
                }
            }
            base.PostProcessProjectile(projectile);
        }
    }
}

