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
using Alexandria.Misc;

namespace NevernamedsItems
{

    public class ServiceWeapon : AdvancedGunBehavior
    {
        public static void Add()
        {
            Gun gun = ETGMod.Databases.Items.NewGun("Service Weapon", "serviceweapon");
            Game.Items.Rename("outdated_gun_mods:service_weapon", "nn:service_weapon");
            gun.gameObject.AddComponent<ServiceWeapon>();
            gun.SetShortDescription("Last Line of Defence");
            gun.SetLongDescription("This weapon is officially categorised as a F.O.P, or 'Firearm of Power', a highly anomalous, highly powerful weapon."+"\n\nDespite not belonging within the Gungeon, something about the shifting nature of the Gungeon's depths seems to soothe it.");

            Alexandria.Assetbundle.GunInt.SetupSprite(gun, Initialisation.gunCollection, "serviceweapon_idle_001", 8, "serviceweapon_ammonomicon_001");

            gun.SetAnimationFPS(gun.shootAnimation, 12);
            gun.SetAnimationFPS(gun.reloadAnimation, 12);

            gun.gunSwitchGroup = (PickupObjectDatabase.GetById(47) as Gun).gunSwitchGroup;

            gun.muzzleFlashEffects = VFXToolbox.CreateVFXPool("ServiceWeapon Muzzleflash",
                new List<string>()
                {
                    "NevernamedsItems/Resources/MiscVFX/GunVFX/ServiceWeapon/serviceweapon_muzzle_001",
                    "NevernamedsItems/Resources/MiscVFX/GunVFX/ServiceWeapon/serviceweapon_muzzle_002",
                    "NevernamedsItems/Resources/MiscVFX/GunVFX/ServiceWeapon/serviceweapon_muzzle_003",
                    "NevernamedsItems/Resources/MiscVFX/GunVFX/ServiceWeapon/serviceweapon_muzzle_004",
                },
                12, //FPS
                new IntVector2(27, 10), //Dimensions
                tk2dBaseSprite.Anchor.MiddleLeft, //Anchor
                false, //Uses a Z height off the ground
                0, //The Z height, if used
                false,
               VFXAlignment.Fixed
                  );
            gun.muzzleFlashEffects.effects[0].effects[0].attached = false;

            gun.gunScreenShake = (PickupObjectDatabase.GetById(47) as Gun).gunScreenShake;

            gun.AddProjectileModuleFrom(PickupObjectDatabase.GetById(56) as Gun, true, false);

            gun.GetComponent<tk2dSpriteAnimator>().GetClipByName(gun.reloadAnimation).wrapMode = tk2dSpriteAnimationClip.WrapMode.LoopSection;
            gun.GetComponent<tk2dSpriteAnimator>().GetClipByName(gun.reloadAnimation).loopStart = 2;

            //GUN STATS
            gun.DefaultModule.ammoCost = 1;
            gun.DefaultModule.shootStyle = ProjectileModule.ShootStyle.SemiAutomatic;
            gun.DefaultModule.sequenceStyle = ProjectileModule.ProjectileSequenceStyle.Random;
            gun.reloadTime = 1f;
            gun.DefaultModule.cooldownTime = 0.13f;
            gun.DefaultModule.numberOfShotsInClip = -1;
            gun.barrelOffset.transform.localPosition = new Vector3(33f / 16f, 17f / 16f, 0f);
            gun.SetBaseMaxAmmo(14);
            gun.gunClass = GunClass.PISTOL;

            //BULLET STATS
            Projectile projectile = gun.DefaultModule.projectiles[0].InstantiateAndFakeprefab();
            gun.DefaultModule.projectiles[0] = projectile;
            projectile.baseData.damage = 20;
            projectile.baseData.speed *= 2;
            projectile.baseData.force *= 2;
            projectile.SetProjectileSprite("serviceweapon_proj", 11, 6, true, tk2dBaseSprite.Anchor.MiddleCenter, 10, 5);
            projectile.hitEffects = (PickupObjectDatabase.GetById(15) as Gun).DefaultModule.projectiles[0].hitEffects;

            gun.DefaultModule.ammoType = GameUIAmmoType.AmmoType.CUSTOM;
            gun.DefaultModule.customAmmoType = CustomClipAmmoTypeToolbox.AddCustomAmmoType("Service Weapon Bullets", "NevernamedsItems/Resources/CustomGunAmmoTypes/serviceweapon_clipfull", "NevernamedsItems/Resources/CustomGunAmmoTypes/serviceweapon_clipempty");

            CustomClipAmmoTypeToolbox.AddCustomAmmoType("Service Weapon Bullets Critical", "NevernamedsItems/Resources/CustomGunAmmoTypes/serviceweapon_critical_clipfull", "NevernamedsItems/Resources/CustomGunAmmoTypes/serviceweapon_critical_clipempty");

            gun.TrimGunSprites();

            gun.quality = PickupObject.ItemQuality.S;
            ETGMod.Databases.Items.Add(gun, null, "ANY");

            ID = gun.PickupObjectId;

            board = VFXToolbox.CreateVFX("TheBoard", new List<string>()
            {
                 "NevernamedsItems/Resources/MiscVFX/GunVFX/ServiceWeapon/theboard_vfx1",
                 "NevernamedsItems/Resources/MiscVFX/GunVFX/ServiceWeapon/theboard_vfx2",
                 "NevernamedsItems/Resources/MiscVFX/GunVFX/ServiceWeapon/theboard_vfx3",
                 "NevernamedsItems/Resources/MiscVFX/GunVFX/ServiceWeapon/theboard_vfx4",
            },
            1, new IntVector2(30, 26), tk2dBaseSprite.Anchor.LowerCenter, false, 0, -1, null, tk2dSpriteAnimationClip.WrapMode.Loop, true);
        }
        public static int ID;
     /*   public override void Start()
        {
            gun.OnAmmoChanged += AmmoChanged;
            base.Start();
        }
        private void AmmoChanged(PlayerController player, Gun gun)
        {
            if (gun.ClipShotsRemaining == 0) { Criticalise(true); }
        }*/
        protected override void Update()
        {
            if (gun && gun.CurrentOwner)
            {
                if (critical) { this.gun.RuntimeModuleData[this.gun.DefaultModule].onCooldown = true; }
                if (timeSinceLastFired <= 1.5f)
                {
                    //If the timer is under 1 second, we just increment it while we wait for the grace period until ammo can regenerate.
                 if (!gun.IsCharging)   timeSinceLastFired += BraveTime.DeltaTime;
                }
                else
                {
                    //If the timer is over one, the gun is capable of regenerating ammo
                    if (gun.CurrentAmmo < gun.AdjustedMaxAmmo)
                    {
                        float req = 0.15f;
                        if (gun.DefaultModule.shootStyle == ProjectileModule.ShootStyle.Charged) req = 0.25f;
                        if (timeSinceLastAmmoIncremented <= req) { timeSinceLastAmmoIncremented += BraveTime.DeltaTime; }
                        else
                        {
                            gun.GainAmmo(1);
                            gun.MoveBulletsIntoClip(1);
                            timeSinceLastAmmoIncremented = 0;
                        }
                    }
                }
                if (critical && gun.ClipShotsRemaining == gun.ClipCapacity) { Criticalise(false); }
            }
            base.Update();
        }
        public void Criticalise(bool state)
        {
            if (critical == state) return;
            if (gun && gun.GunPlayerOwner())
            {
                if (state)
                {
                    gun.DefaultModule.customAmmoType = "Service Weapon Bullets Critical";
                    this.gun.RuntimeModuleData[this.gun.DefaultModule].onCooldown = true;
                }
                else
                {
                    gun.DefaultModule.customAmmoType = "Service Weapon Bullets";
                    this.gun.RuntimeModuleData[this.gun.DefaultModule].onCooldown = false;
                }
                ForceUpdateClip(GameUIRoot.Instance.ammoControllers[(!gun.GunPlayerOwner().IsPrimaryPlayer) ? 0 : 1], gun.GunPlayerOwner().inventory);
                critical = state;
            }
        }
        private void ForceUpdateClip(GameUIAmmoController controller, GunInventory inv)
        {
            if (!controller.m_initialized) { controller.Initialize(); }

            Gun currentGun = inv.CurrentGun;
            Gun currentSecondaryGun = inv.CurrentSecondaryGun;

            int num = 0;
            for (int i = 0; i < currentGun.Volley.projectiles.Count; i++)
            {
                ProjectileModule projectileModule = currentGun.Volley.projectiles[i];
                if (projectileModule == currentGun.DefaultModule || (projectileModule.IsDuctTapeModule && projectileModule.ammoCost > 0)) { num++; }
            }
            if (currentSecondaryGun)
            {
                for (int j = 0; j < currentSecondaryGun.Volley.projectiles.Count; j++)
                {
                    ProjectileModule projectileModule2 = currentSecondaryGun.Volley.projectiles[j];
                    if (projectileModule2 == currentSecondaryGun.DefaultModule || (projectileModule2.IsDuctTapeModule && projectileModule2.ammoCost > 0)) { num++; }
                }
            }

            bool didChangeGun = currentGun != controller.m_cachedGun || currentGun.DidTransformGunThisFrame;
            currentGun.DidTransformGunThisFrame = false;


            controller.CleanupLists(num);
            int num4 = 0;
            int num5 = currentGun.Volley.projectiles.Count;
            if (currentSecondaryGun)
            {
                num5 += currentSecondaryGun.Volley.projectiles.Count;
            }
            for (int k = 0; k < num5; k++)
            {
                Gun gun = (k < currentGun.Volley.projectiles.Count) ? currentGun : currentSecondaryGun;
                int index = (!(gun == currentGun)) ? (k - currentGun.Volley.projectiles.Count) : k;
                ProjectileModule projectileModule3 = gun.Volley.projectiles[index];
                bool flag = projectileModule3 == gun.DefaultModule || (projectileModule3.IsDuctTapeModule && projectileModule3.ammoCost > 0);
                if (flag)
                {
                    controller.EnsureInitialization(num4);
                    dfTiledSprite value = controller.fgSpritesForModules[num4];
                    dfTiledSprite value2 = controller.bgSpritesForModules[num4];
                    List<dfTiledSprite> addlModuleFGSprites = controller.addlFgSpritesForModules[num4];
                    List<dfTiledSprite> addlModuleBGSprites = controller.addlBgSpritesForModules[num4];
                    dfSprite moduleTopCap = controller.topCapsForModules[num4];
                    dfSprite moduleBottomCap = controller.bottomCapsForModules[num4];
                    GameUIAmmoType.AmmoType value3 = controller.cachedAmmoTypesForModules[num4];
                    string value4 = controller.cachedCustomAmmoTypesForModules[num4];
                    int value5 = controller.m_cachedModuleShotsRemaining[num4];
                    controller.UpdateAmmoUIForModule(ref value, ref value2, addlModuleFGSprites, addlModuleBGSprites, moduleTopCap, moduleBottomCap, projectileModule3, gun, ref value3, ref value4, ref value5, didChangeGun, num - (num4 + 1));
                    controller.fgSpritesForModules[num4] = value;
                    controller.bgSpritesForModules[num4] = value2;
                    controller.cachedAmmoTypesForModules[num4] = value3;
                    controller.cachedCustomAmmoTypesForModules[num4] = value4;
                    controller.m_cachedModuleShotsRemaining[num4] = value5;
                    num4++;
                }
            }
            if (!controller.bottomCapsForModules[0].IsVisible)
            {
                for (int n = 0; n < controller.bgSpritesForModules.Count; n++)
                {
                    controller.fgSpritesForModules[n].IsVisible = true;
                    controller.bgSpritesForModules[n].IsVisible = true;
                }
                for (int num6 = 0; num6 < controller.topCapsForModules.Count; num6++)
                {
                    controller.topCapsForModules[num6].IsVisible = true;
                    controller.bottomCapsForModules[num6].IsVisible = true;
                }
            }
            controller.GunClipCountLabel.IsVisible = false;
            controller.m_cachedGun = currentGun;
            controller.m_cachedNumberModules = num;
            controller.m_cachedTotalAmmo = currentGun.CurrentAmmo;
            controller.m_cachedMaxAmmo = currentGun.AdjustedMaxAmmo;
            controller.m_cachedUndertaleness = currentGun.IsUndertaleGun;
            controller.UpdateAdditionalSprites();
        }
        public override void OnSwitchedToThisGun()
        {
            if (critical) { ForceUpdateClip(GameUIRoot.Instance.ammoControllers[(!gun.GunPlayerOwner().IsPrimaryPlayer) ? 0 : 1], gun.GunPlayerOwner().inventory); }
            base.OnSwitchedToThisGun();
        }


        public override void PostProcessProjectile(Projectile projectile)
        {
            timeSinceLastFired = 0;
            timeSinceLastAmmoIncremented = 0;
            if (gun.LastShotIndex == gun.ClipCapacity - 1) { Criticalise(true); }
            base.PostProcessProjectile(projectile);
        }
        protected override void OnPickedUpByPlayer(PlayerController player)
        {
            if (!everPickedUpByPlayer)
            {
                player.StartCoroutine(DoBoardDialogue(
                new List<string>()
                {
                    "<This is an unauthorized/unusual/exciting turn of events.>",
                    "<The service weapon is a gun/weapon/you.>",
                    "<Wield the gun/weapon/you well>",
                    "<Welcome to the Bureau, Director/Boss/Head-Honcho>"
                }, 
                player
                ));
            }
            base.OnPickedUpByPlayer(player);
        }
        protected override void OnPostDroppedByPlayer(PlayerController player)
        {
            player.StartCoroutine(DoBoardDialogue(
                new List<string>()
                {
                    "<Please be aware/advised>",
                    "<This action constitutes dereliction of duty/treason/oopsie>",
                    "<Please reconsider/reconcile/change>"
                },
                player
                ));
            base.OnPostDroppedByPlayer(player);
        }
        public IEnumerator DoBoardDialogue(List<string> text, PlayerController player)
        {
            if (talking) { yield break; }
            talking = true;
            VFXToolbox.GlitchScreenForSeconds(0.5f);
            GameObject boardInst = player.PlayEffectOnActor(board, new Vector3(1, 2, 0), true, false, true);

            foreach (string dialogue in text)
            {
                RelativeLabelAttacher label = boardInst.AddComponent<RelativeLabelAttacher>();
                label.colour = Color.red;
                label.offset = new Vector3(0, 3f, 0);
                label.labelValue = dialogue;
                label.centered = true;
                AkSoundEngine.PostEvent("Play_BoardCommunication", player.gameObject);

                yield return new WaitForSeconds(5f);
                UnityEngine.Object.Destroy(label);
            }

            VFXToolbox.GlitchScreenForSeconds(0.5f);
            UnityEngine.Object.Destroy(boardInst);
            talking = false;
            yield break;
        }


        public bool talking;
        public float timeSinceLastFired;
        public float timeSinceLastAmmoIncremented = 0;
        public bool critical;
        public static GameObject board;
    }
}