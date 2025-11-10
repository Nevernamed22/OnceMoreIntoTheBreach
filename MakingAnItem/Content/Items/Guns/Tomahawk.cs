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
using Alexandria.Assetbundle;
using Dungeonator;

namespace NevernamedsItems
{

    public class Tomahawk : GunBehaviour
    {
        public static void Add()
        {
            Gun gun = ETGMod.Databases.Items.NewGun("Tomahawk", "tomahawk");
            Game.Items.Rename("outdated_gun_mods:tomahawk", "nn:tomahawk");
            var behav = gun.gameObject.AddComponent<Tomahawk>();
            gun.SetShortDescription("Demahigun");
            gun.SetLongDescription("This mastercraft shotgun is impeccably weighted, and can be thrown in a devstating bladed arc to cut through waves of foes.");

            gun.SetGunSprites("tomahawk", 8);

            gun.SetAnimationFPS(gun.shootAnimation, 12);
            gun.SetAnimationFPS(gun.reloadAnimation, 12);

            gun.GetComponent<tk2dSpriteAnimator>().GetClipByName(gun.reloadAnimation).wrapMode = tk2dSpriteAnimationClip.WrapMode.LoopSection;
            gun.GetComponent<tk2dSpriteAnimator>().GetClipByName(gun.reloadAnimation).loopStart = 5;

            gun.gunSwitchGroup = (PickupObjectDatabase.GetById(98) as Gun).gunSwitchGroup;
            gun.SetAnimationFPS(gun.shootAnimation, 13);
            gun.SetAnimationFPS(gun.idleAnimation, 5);
            gun.muzzleFlashEffects = (PickupObjectDatabase.GetById(23) as Gun).muzzleFlashEffects;

            for (int i = 0; i < 2; i++)
            {
                gun.AddProjectileModuleFrom(PickupObjectDatabase.GetById(86) as Gun, true, false);
            }

            Projectile projectile = UnityEngine.Object.Instantiate<Projectile>(gun.DefaultModule.projectiles[0]);
            projectile.gameObject.SetActive(false);
            FakePrefab.MarkAsFakePrefab(projectile.gameObject);
            UnityEngine.Object.DontDestroyOnLoad(projectile);
            projectile.AdditionalScaleMultiplier = 0.8f;
            projectile.baseData.range = 15;
            projectile.baseData.damage = 7;
            projectile.hitEffects.overrideMidairDeathVFX = (PickupObjectDatabase.GetById(178) as Gun).GetComponent<FireOnReloadSynergyProcessor>().DirectedBurstSettings.ProjectileInterface.SpecifiedProjectile.hitEffects.tileMapHorizontal.effects[0].effects[0].effect;
            projectile.hitEffects.alwaysUseMidair = true;

            //GUN STATS
            foreach (ProjectileModule mod in gun.Volley.projectiles)
            {
                mod.ammoCost = 1;
                mod.shootStyle = ProjectileModule.ShootStyle.SemiAutomatic;
                mod.sequenceStyle = ProjectileModule.ProjectileSequenceStyle.Random;
                mod.cooldownTime = 0.2f;
                mod.angleVariance = 10f;
                mod.numberOfShotsInClip = 7;
                mod.projectiles[0] = projectile;
                if (mod != gun.DefaultModule) { mod.ammoCost = 0; }
            }
            gun.DefaultModule.ammoType = GameUIAmmoType.AmmoType.BLUE_SHOTGUN;

            gun.reloadTime = 1f;
            gun.SetBarrel(35, 29);
            gun.SetBaseMaxAmmo(200);
            gun.gunClass = GunClass.SHOTGUN;
            gun.quality = PickupObject.ItemQuality.B;
            ETGMod.Databases.Items.Add(gun, null, "ANY");
            ID = gun.PickupObjectId;

            thrown = DataCloners.CopyFields<BoomerangProjectile>(gun.DefaultModule.projectiles[0].InstantiateAndFakeprefab());
            thrown.MaximumTraversalDistance = 8f;
            thrown.StunDuration = 0;
            thrown.trackingSpeed = 720;
            thrown.baseData.damage = 25;
            thrown.baseData.speed = 30f;
            thrown.baseData.range = 1000000000000;
            thrown.gameObject.AddComponent<PierceProjModifier>().penetration = 3;
            thrown.gameObject.AddComponent<BounceProjModifier>().numberOfBounces = 50;
            GameObject smack = (PickupObjectDatabase.GetById(178) as Gun).GetComponent<FireOnReloadSynergyProcessor>().DirectedBurstSettings.ProjectileInterface.SpecifiedProjectile.hitEffects.tileMapHorizontal.effects[0].effects[0].effect;
           VFXPool poolsmack = VFXToolbox.CreateBlankVFXPool(smack);
            thrown.hitEffects.tileMapHorizontal = poolsmack;
            thrown.hitEffects.tileMapVertical = poolsmack;
            thrown.hitEffects.deathTileMapHorizontal = poolsmack;
            thrown.hitEffects.deathTileMapVertical = poolsmack;
            thrown.hitEffects.HasProjectileDeathVFX = true;
            thrown.hitEffects.enemy = (PickupObjectDatabase.GetById(178) as Gun).GetComponent<FireOnReloadSynergyProcessor>().DirectedBurstSettings.ProjectileInterface.SpecifiedProjectile.hitEffects.enemy;
            thrown.hitEffects.deathEnemy = (PickupObjectDatabase.GetById(178) as Gun).GetComponent<FireOnReloadSynergyProcessor>().DirectedBurstSettings.ProjectileInterface.SpecifiedProjectile.hitEffects.deathEnemy;
            thrown.AnimateProjectileBundle("TomahawkThrown",
                   Initialisation.ProjectileCollection,
                   Initialisation.projectileAnimationCollection,
                   "TomahawkThrown",
                   MiscTools.DupeList(new IntVector2(32, 30), 4), //Pixel Sizes
                   MiscTools.DupeList(false, 8), //Lightened
                   MiscTools.DupeList(tk2dBaseSprite.Anchor.MiddleCenter, 4), //Anchors
                   MiscTools.DupeList(true, 4), //Anchors Change Colliders
                   MiscTools.DupeList(false, 4), //Fixes Scales
                   MiscTools.DupeList<Vector3?>(null, 4), //Manual Offsets
                   MiscTools.DupeList<IntVector2?>(new IntVector2(30,30), 4), //Override colliders
                   MiscTools.DupeList<IntVector2?>(null, 4), //Override collider offsets
                   MiscTools.DupeList<Projectile>(null, 4)); // Override to copy from   
        }
        public static int ID;
        public static BoomerangProjectile thrown;

        public override void OnReloadPressed(PlayerController player, Gun gun, bool manual)
        {
            base.OnReloadPressed(player, gun, manual);
            if (gun.ClipShotsRemaining < gun.ClipCapacity && timeSincethrow <= 0)
            {
                timeSincethrow = 1f;

                GameObject gameObject = thrown.InstantiateAndFireInDirection(gun.barrelOffset.position, gun.CurrentAngle, 0);
                Projectile component = gameObject.GetComponent<Projectile>();
                if (component != null)
                {
                    component.Owner = player;
                    component.Shooter = player.specRigidbody;
                    component.ScaleByPlayerStats(player);
                    player.DoPostProcessProjectile(component);                  
                }
            }
        }

        public override void Update()
        {
            base.Update();
            if (timeSincethrow > 0) { timeSincethrow -= BraveTime.DeltaTime; }
        }
        public float timeSincethrow;
    }
}
