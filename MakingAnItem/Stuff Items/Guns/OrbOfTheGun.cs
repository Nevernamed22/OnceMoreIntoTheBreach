using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using Gungeon;
using MonoMod;
using UnityEngine;
using ItemAPI;

namespace NevernamedsItems
{

    public class OrbOfTheGun : GunBehaviour
    {


        public static void Add()
        {
            Gun gun = ETGMod.Databases.Items.NewGun("Orb of the Gun", "orbofthegun");
            Game.Items.Rename("outdated_gun_mods:orb_of_the_gun", "nn:orb_of_the_gun");
            gun.gameObject.AddComponent<OrbOfTheGun>();
            gun.SetShortDescription("Hypershot");
            gun.SetLongDescription("This gun is from a strange non-euclidian plane of reality, where it used to be able to only point in one direction."+"\n\nReloading when the clip is more than 50% empty sends bullets close to you to another dimension.");

            gun.SetupSprite(null, "orbofthegun_idle_001", 8);

            gun.SetAnimationFPS(gun.shootAnimation, 15);

            gun.AddProjectileModuleFrom(PickupObjectDatabase.GetById(56) as Gun, true, false);
            
            //GUN STATS
            gun.DefaultModule.ammoCost = 1;
            gun.DefaultModule.shootStyle = ProjectileModule.ShootStyle.SemiAutomatic;
            gun.DefaultModule.sequenceStyle = ProjectileModule.ProjectileSequenceStyle.Random;
            gun.reloadTime = 1.2f;
            gun.DefaultModule.cooldownTime = 0.7f;
            gun.muzzleFlashEffects.type = VFXPoolType.None;
            gun.DefaultModule.numberOfShotsInClip = 15;
            gun.gunHandedness = GunHandedness.HiddenOneHanded;
            gun.barrelOffset.transform.localPosition = new Vector3(1.25f, 0.62f, 0f);
            gun.SetBaseMaxAmmo(150);
            gun.gunClass = GunClass.RIFLE;
            //BULLET STATS
            Projectile projectile = UnityEngine.Object.Instantiate<Projectile>(gun.DefaultModule.projectiles[0]);
            projectile.gameObject.SetActive(false);
            FakePrefab.MarkAsFakePrefab(projectile.gameObject);
            UnityEngine.Object.DontDestroyOnLoad(projectile);
            gun.DefaultModule.projectiles[0] = projectile;
            projectile.baseData.damage *= 4f;
            projectile.baseData.speed *= 2f;
            projectile.pierceMinorBreakables = true;
            //projectile.shouldRotate = true;
            projectile.SetProjectileSpriteRight("orbofthegun_projectile", 20, 12, true, tk2dBaseSprite.Anchor.MiddleCenter, 20, 12);

            projectile.transform.parent = gun.barrelOffset;

            gun.quality = PickupObject.ItemQuality.A;
            gun.encounterTrackable.EncounterGuid = "this is the Orb of the Gun";
            ETGMod.Databases.Items.Add(gun, null, "ANY");

        }
        public override void OnReloadPressed(PlayerController player, Gun gun, bool bSOMETHING)
        {
            if (HasReloaded && gun.ClipShotsRemaining < gun.ClipCapacity / 2)
            {
                HasReloaded = false;
                PlayerController owner = this.gun.CurrentOwner as PlayerController;
                GameObject silencerVFX = (GameObject)ResourceCache.Acquire("Global VFX/BlankVFX_Ghost");
                AkSoundEngine.PostEvent("Play_OBJ_silenceblank_small_01", base.gameObject);
                GameObject gameObject = new GameObject("silencer");
                SilencerInstance silencerInstance = gameObject.AddComponent<SilencerInstance>();
                float additionalTimeAtMaxRadius = 0.25f;
                silencerInstance.TriggerSilencer(owner.sprite.WorldCenter, 25f, 5f, silencerVFX, 0f, 3f, 3f, 3f, 250f, 5f, additionalTimeAtMaxRadius, owner, false, false);
            }

            base.OnReloadPressed(player, gun, bSOMETHING);
        }
        private bool HasReloaded;
        protected void Update()
        {
            if (gun.CurrentOwner)
            {
                if (!gun.IsReloading && !HasReloaded)
                {
                    this.HasReloaded = true;
                }
            }
        }
        public override void OnPostFired(PlayerController player, Gun gun)
        {           
            gun.PreventNormalFireAudio = true;
            AkSoundEngine.PostEvent("Play_BOSS_agunim_orb_01", gameObject);
        }
        public OrbOfTheGun()
        {

        }
    }
}

