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

    public class G20 : AdvancedGunBehavior
    {
        public static void Add()
        {

            Gun gun = ETGMod.Databases.Items.NewGun("G20", "g20");
            Game.Items.Rename("outdated_gun_mods:g20", "nn:g20");
            gun.gameObject.AddComponent<G20>();
            gun.SetShortDescription("Roll and Die");
            gun.SetLongDescription("Randomises stats upon entering combat." + "\n\nThe preferred weapon of a young disciple of Icosahedrax, stolen by his michevious nephew.");
            gun.SetupSprite(null, "g20_idle_001", 8);
            gun.SetAnimationFPS(gun.shootAnimation, 14);

            gun.AddProjectileModuleFrom(PickupObjectDatabase.GetById(86) as Gun, true, false);
            gun.gunSwitchGroup = (PickupObjectDatabase.GetById(38) as Gun).gunSwitchGroup;
            gun.muzzleFlashEffects = (PickupObjectDatabase.GetById(83) as Gun).muzzleFlashEffects;

            //GUN STATS
            gun.DefaultModule.ammoCost = 1;
            gun.DefaultModule.shootStyle = ProjectileModule.ShootStyle.SemiAutomatic;
            gun.DefaultModule.sequenceStyle = ProjectileModule.ProjectileSequenceStyle.Random;
            gun.reloadTime = 1f;
            gun.DefaultModule.cooldownTime = 0.5f;
            gun.DefaultModule.numberOfShotsInClip = 10;
            gun.barrelOffset.transform.localPosition = new Vector3(33f / 16f, 18f / 16f, 0f);
            gun.SetBaseMaxAmmo(350);
            gun.gunClass = GunClass.PISTOL;
            //BULLET STATS
            Projectile projectile = gun.DefaultModule.projectiles[0].InstantiateAndFakeprefab();
            gun.DefaultModule.projectiles[0] = projectile;
            projectile.baseData.damage = 10f;
            projectile.hitEffects.overrideMidairDeathVFX = (PickupObjectDatabase.GetById(519) as Gun).DefaultModule.projectiles[0].hitEffects.tileMapVertical.effects[0].effects[0].effect;
            projectile.hitEffects.alwaysUseMidair = true;
            projectile.AnimateProjectile(new List<string> {
                "g20_newproj_001",
                "g20_newproj_002",
                "g20_newproj_003",
                "g20_newproj_003",
            }, 10, tk2dSpriteAnimationClip.WrapMode.Loop,
      AnimateBullet.ConstructListOfSameValues(new IntVector2(11, 10), 4),
      AnimateBullet.ConstructListOfSameValues(true, 4),
      AnimateBullet.ConstructListOfSameValues(tk2dBaseSprite.Anchor.MiddleCenter, 4),
      AnimateBullet.ConstructListOfSameValues(true, 4),
      AnimateBullet.ConstructListOfSameValues(false, 4),
      AnimateBullet.ConstructListOfSameValues<Vector3?>(null, 4),
      AnimateBullet.ConstructListOfSameValues<IntVector2?>(null, 4),
      AnimateBullet.ConstructListOfSameValues<IntVector2?>(null, 4),
      AnimateBullet.ConstructListOfSameValues<Projectile>(null, 4), 0);

            gun.TrimGunSprites();

            gun.quality = PickupObject.ItemQuality.D;
            ETGMod.Databases.Items.Add(gun, null, "ANY");

            G20ID = gun.PickupObjectId;
        }
        public static int G20ID;
        public override void OnReloadPressedSafe(PlayerController player, Gun gun, bool manualReload)
        {
            if (player.PlayerHasActiveSynergy("Rerollin Rollin Rollin"))
            {
                if ((gun.ClipCapacity == gun.ClipShotsRemaining) || (gun.CurrentAmmo == gun.ClipShotsRemaining))
                {
                    if (gun.CurrentAmmo >= 10)
                    {
                        gun.CurrentAmmo -= 10;
                        EnteredCombat();
                    }
                }
            }
            base.OnReloadPressedSafe(player, gun, manualReload);
        }
        protected override void OnPickedUpByPlayer(PlayerController player)
        {
            player.OnEnteredCombat += this.EnteredCombat;
            base.OnPickedUpByPlayer(player);
        }
        protected override void OnPostDroppedByPlayer(PlayerController player)
        {
            player.OnEnteredCombat -= this.EnteredCombat;
            base.OnPostDroppedByPlayer(player);
        }
        protected override void Update()
        {
            if (ClipSize != -1 && gun.DefaultModule.numberOfShotsInClip != ClipSize)
            {
                gun.DefaultModule.numberOfShotsInClip = ClipSize;
            }
            if (CooldownTime != -1 && gun.DefaultModule.cooldownTime != CooldownTime)
            {
                gun.DefaultModule.cooldownTime = CooldownTime;
            }
            base.Update();
        }
        private void EnteredCombat()
        {
            gun.reloadTime = UnityEngine.Random.Range(10f, 191f) / 100f;

            CooldownTime = (gun.GunPlayerOwner().PlayerHasActiveSynergy("Critical Success")) ? UnityEngine.Random.Range(0.05f, 0.5f) : UnityEngine.Random.Range(0.1f, 0.8f);
            ClipSize = UnityEngine.Random.Range(1, 31);

            gun.DefaultModule.numberOfShotsInClip = ClipSize;
            gun.DefaultModule.cooldownTime = CooldownTime;

            damageMod = (gun.GunPlayerOwner() && gun.GunPlayerOwner().PlayerHasActiveSynergy("Critical Success")) ? UnityEngine.Random.Range(0.5f, 2.5f) : UnityEngine.Random.Range(0.1f, 2f);
            rangeMod = UnityEngine.Random.Range(0.1f, 2f);
            speedMod = UnityEngine.Random.Range(0.1f, 2f);
            knockbackMod = UnityEngine.Random.Range(0.1f, 2f);
            scaleMod = UnityEngine.Random.Range(0.5f, 2f);

            if (gun.IsCurrentGun()) AkSoundEngine.PostEvent("Play_OBJ_Chest_Synergy_Slots_01", gameObject);
        }
        private int ClipSize = -1;
        private float CooldownTime = -1;
        public override void PostProcessProjectile(Projectile projectile)
        {
            projectile.baseData.damage *= damageMod;
            projectile.baseData.force *= knockbackMod;
            projectile.baseData.speed *= speedMod;
            projectile.baseData.range *= rangeMod;
            projectile.UpdateSpeed();
            projectile.RuntimeUpdateScale(scaleMod);
            base.PostProcessProjectile(projectile);
        }
        private float damageMod = 1;
        private float rangeMod = 1;
        private float speedMod = 1;
        private float knockbackMod = 1;
        private float scaleMod = 1;

        public G20()
        {

        }
    }
}

