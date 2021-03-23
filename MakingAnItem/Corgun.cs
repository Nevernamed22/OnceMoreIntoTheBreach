using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using Gungeon;
using MonoMod;
using UnityEngine;
using ItemAPI;
using SaveAPI;

namespace NevernamedsItems
{
    public class Corgun : AdvancedGunBehavior
    {
        public static void Add()
        {
            Gun gun = ETGMod.Databases.Items.NewGun("Doggun", "corgun");
            Game.Items.Rename("outdated_gun_mods:doggun", "nn:doggun");
            gun.gameObject.AddComponent<Corgun>();
            gun.SetShortDescription("Bork Bork");
            gun.SetLongDescription("Lovingly carved in the image of Junior II, this gun has some fighting spirit, despite it's cuddly appearance."+"\n\n'Bork Bork' - Junior II");

            gun.SetupSprite(null, "corgun_idle_001", 8);

            gun.SetAnimationFPS(gun.shootAnimation, 12);

            gun.AddProjectileModuleFrom(PickupObjectDatabase.GetById(56) as Gun, true, false);

            //GUN STATS
            gun.DefaultModule.ammoCost = 1;
            gun.DefaultModule.shootStyle = ProjectileModule.ShootStyle.SemiAutomatic;
            gun.DefaultModule.sequenceStyle = ProjectileModule.ProjectileSequenceStyle.Random;
            gun.reloadTime = 0.8f;
            gun.DefaultModule.cooldownTime = 0.4f;
            gun.muzzleFlashEffects.type = VFXPoolType.None;
            gun.DefaultModule.numberOfShotsInClip = 5;
            gun.barrelOffset.transform.localPosition = new Vector3(0.5f, 0.56f, 0f);
            gun.SetBaseMaxAmmo(400);

            //BULLET STATS
            Projectile projectile = UnityEngine.Object.Instantiate<Projectile>(gun.DefaultModule.projectiles[0]);
            projectile.gameObject.SetActive(false);
            FakePrefab.MarkAsFakePrefab(projectile.gameObject);
            UnityEngine.Object.DontDestroyOnLoad(projectile);
            gun.DefaultModule.projectiles[0] = projectile;
            projectile.baseData.damage *= 1.6f;
            projectile.baseData.speed *= 1f;
            projectile.baseData.range *= 10f;

            projectile.pierceMinorBreakables = true;
            //projectile.shouldRotate = true;
            PierceProjModifier orAddComponent = projectile.gameObject.GetOrAddComponent<PierceProjModifier>();
            orAddComponent.penetratesBreakables = true;
            orAddComponent.penetration = 5;
            BounceProjModifier Bouncing = projectile.gameObject.GetOrAddComponent<BounceProjModifier>();
            Bouncing.numberOfBounces = 5;

            HomingModifier homing = projectile.gameObject.AddComponent<HomingModifier>();
            homing.AngularVelocity = 70f;
            homing.HomingRadius = 100f;

            projectile.SetProjectileSpriteRight("doggun_projectile", 16, 11, false, tk2dBaseSprite.Anchor.MiddleCenter, 15, 10);

            projectile.transform.parent = gun.barrelOffset;

            
            gun.quality = PickupObject.ItemQuality.A;
            gun.encounterTrackable.EncounterGuid = "this is the Doggun";
            ETGMod.Databases.Items.Add(gun, null, "ANY");

            DoggunID = gun.PickupObjectId;

            gun.SetupUnlockOnCustomFlag(CustomDungeonFlags.KILLED_DRAGUN_WITH_DOG, true);
        }
        public static int DoggunID;
        public override void OnPostFired(PlayerController player, Gun gun)
        {
            gun.PreventNormalFireAudio = true;
            AkSoundEngine.PostEvent("Play_PET_dog_bark_02", gameObject);
        }
        public override void OnReload(PlayerController player, Gun gun)
        {
            base.OnReload(player, gun);
            AkSoundEngine.PostEvent("Stop_WPN_All", base.gameObject);
            AkSoundEngine.PostEvent("Play_PET_dog_bark_02", base.gameObject);
        }
        public Corgun()
        {

        }      
    }
}
