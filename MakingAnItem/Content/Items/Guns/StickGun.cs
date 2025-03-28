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

    public class StickGun : AdvancedGunBehavior
    {
        public static void Add()
        {
            Gun gun = ETGMod.Databases.Items.NewGun("Stick Gun", "stickgun");
            Game.Items.Rename("outdated_gun_mods:stick_gun", "nn:stick_gun");
            var behav = gun.gameObject.AddComponent<StickGun>();
            behav.preventNormalReloadAudio = true;
            behav.preventNormalFireAudio = true;
            behav.overrideNormalFireAudio = "Play_PencilScratch";
            gun.SetShortDescription("Scribble");
            gun.SetLongDescription("Carried by a brave stickman as he ventured through the pages of a bored child's homework." + "\n\nHe may be long erased, but his legacy lives on.");

            Alexandria.Assetbundle.GunInt.SetupSprite(gun, Initialisation.gunCollection, "stickgun_idle_001", 8, "stickgun_ammonomicon_001");

            gun.AddProjectileModuleFrom(PickupObjectDatabase.GetById(56) as Gun, true, false);

            //GUN STATS
            gun.DefaultModule.ammoCost = 1;
            gun.DefaultModule.shootStyle = ProjectileModule.ShootStyle.SemiAutomatic;
            gun.DefaultModule.sequenceStyle = ProjectileModule.ProjectileSequenceStyle.Random;
            gun.reloadTime = 0.7f;
            gun.DefaultModule.cooldownTime = 0.1f;
            gun.DefaultModule.numberOfShotsInClip = 3;
            gun.barrelOffset.transform.localPosition = new Vector3(0.93f, 0.68f, 0f);
            gun.SetBaseMaxAmmo(222);
            gun.ammo = 222;
            gun.gunClass = GunClass.SHITTY;
            gun.gunHandedness = GunHandedness.OneHanded;

            //VFX
            gun.muzzleFlashEffects = VFXToolbox.CreateVFXPoolBundle("StickGunMuzzle", false, 0, VFXAlignment.Fixed);
             
            //BULLET STATS
            Projectile projectile = UnityEngine.Object.Instantiate<Projectile>(gun.DefaultModule.projectiles[0]);
            projectile.gameObject.SetActive(false);
            FakePrefab.MarkAsFakePrefab(projectile.gameObject);
            UnityEngine.Object.DontDestroyOnLoad(projectile);
            projectile.baseData.damage = 6f;
            projectile.baseData.speed *= 1.5f;

            PierceProjModifier pierce = projectile.gameObject.AddComponent<PierceProjModifier>();
            pierce.penetration = 1;

            projectile.SetProjectileSprite("stickgun_projectile", 12, 2, false, tk2dBaseSprite.Anchor.MiddleCenter, 12, 2);
            gun.DefaultModule.projectiles[0] = projectile;

            //Projectile VFX
            projectile.hitEffects.overrideMidairDeathVFX = VFXToolbox.CreateVFXBundle("StickGunMidair", false, 0f); ;
            projectile.hitEffects.tileMapHorizontal = VFXToolbox.CreateVFXPoolBundle("StickGunTilemapHoriz", false, 0);
            projectile.hitEffects.tileMapVertical = VFXToolbox.CreateVFXPoolBundle("StickGunTilemapVert", false, 0);
            projectile.hitEffects.enemy = VFXToolbox.CreateVFXPoolBundle("StickGunEnemyImpact", false, 0, VFXAlignment.Fixed);

            gun.DefaultModule.ammoType = GameUIAmmoType.AmmoType.CUSTOM;
            gun.DefaultModule.customAmmoType = CustomClipAmmoTypeToolbox.AddCustomAmmoType("StickGun Bullets", "NevernamedsItems/Resources/CustomGunAmmoTypes/stickgun_clipfull", "NevernamedsItems/Resources/CustomGunAmmoTypes/stickgun_clipempty");

            gun.quality = PickupObject.ItemQuality.D;
            ETGMod.Databases.Items.Add(gun, false, "ANY");
            ID = gun.PickupObjectId;

        }
        public static int ID;       
    }
}
