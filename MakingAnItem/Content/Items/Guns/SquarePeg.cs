using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Reflection;
using Gungeon;
using MonoMod;
using UnityEngine;
using Alexandria.ItemAPI;
using SaveAPI;

namespace NevernamedsItems
{

    public class SquarePeg : GunBehaviour
    {
        public static void Add()
        {
            Gun gun = ETGMod.Databases.Items.NewGun("Square Peg", "squarepeg");
            Game.Items.Rename("outdated_gun_mods:square_peg", "nn:square_peg");
            gun.gameObject.AddComponent<SquarePeg>();
            gun.SetShortDescription("In A Round Chamber");
            gun.SetLongDescription("[] A perfect example of the art of cubism.");

            gun.SetGunSprites("squarepeg");

            gun.SetAnimationFPS(gun.shootAnimation, 15);

            gun.AddProjectileModuleFrom(PickupObjectDatabase.GetById(86) as Gun, true, false);
            gun.gunSwitchGroup = (PickupObjectDatabase.GetById(475) as Gun).gunSwitchGroup;

            //GUN STATS
            gun.DefaultModule.ammoCost = 1;
            gun.DefaultModule.shootStyle = ProjectileModule.ShootStyle.SemiAutomatic;
            gun.DefaultModule.sequenceStyle = ProjectileModule.ProjectileSequenceStyle.Random;
            gun.reloadTime = 1f;
            gun.DefaultModule.cooldownTime = 0.12f;
            gun.DefaultModule.numberOfShotsInClip = 6;
            gun.barrelOffset.transform.localPosition = new Vector3((26f / 16f), (14f / 16f), 0f);
            gun.SetBaseMaxAmmo(150);
            gun.gunClass = GunClass.PISTOL;
            gun.muzzleFlashEffects = VFXToolbox.CreateVFXPoolBundle("SquarePegMuzzle", false, 0, VFXAlignment.Fixed);

            //BULLET STATS
            Projectile projectile = UnityEngine.Object.Instantiate<Projectile>(gun.DefaultModule.projectiles[0]);
            projectile.gameObject.SetActive(false);
            FakePrefab.MarkAsFakePrefab(projectile.gameObject);
            UnityEngine.Object.DontDestroyOnLoad(projectile);
            gun.DefaultModule.projectiles[0] = projectile;
            projectile.transform.parent = gun.barrelOffset;
            projectile.baseData.damage = 8f;
            projectile.hitEffects.overrideMidairDeathVFX = SharedVFX.SquarePegImpact;
            projectile.hitEffects.alwaysUseMidair = true;

            projectile.SetProjectileSpriteRight("squarepeg_proj", 8, 8, false, tk2dBaseSprite.Anchor.MiddleCenter, 8, 8);

            gun.DefaultModule.ammoType = GameUIAmmoType.AmmoType.CUSTOM;
            gun.DefaultModule.customAmmoType = CustomClipAmmoTypeToolbox.AddCustomAmmoType("SquarePegSquares", "NevernamedsItems/Resources/CustomGunAmmoTypes/squarepeg_clipfull", "NevernamedsItems/Resources/CustomGunAmmoTypes/squarepeg_clipempty");

            gun.quality = PickupObject.ItemQuality.C;
            ETGMod.Databases.Items.Add(gun, null, "ANY");
            ID = gun.PickupObjectId;
        }
        public static int ID;
    }
}