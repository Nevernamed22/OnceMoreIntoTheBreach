using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;
using Alexandria.ItemAPI;
using SaveAPI;
using Alexandria.Misc;
using Alexandria.Assetbundle;

namespace NevernamedsItems
{
    public class HauntedAmulet : PassiveItem
    {
        public static void Init()
        {
            PickupObject item = ItemSetup.NewItem<HauntedAmulet>(
                "Haunted Amulet",
               "Spirit Shaker",
               "Summons phantoms from the bodies of the slain." + "\n\nOriginally worn by the necrogunsmith Nuign to pacify the souls used in his accursed experiments.",
               "hauntedamulet_icon");
            item.quality = PickupObject.ItemQuality.C;
            ID = item.PickupObjectId;


            blinky = (PickupObjectDatabase.GetById(86) as Gun).DefaultModule.projectiles[0].InstantiateAndFakeprefab();
            blinky.AnimateProjectileBundle("GhostProjectileBlinky",
                   Initialisation.ProjectileCollection,
                   Initialisation.projectileAnimationCollection,
                   "GhostProjectileBlinky",
                   MiscTools.DupeList(new IntVector2(14, 27), 5), //Pixel Sizes
                   MiscTools.DupeList(false, 5), //Lightened
                   MiscTools.DupeList(tk2dBaseSprite.Anchor.MiddleCenter, 5), //Anchors
                   MiscTools.DupeList(true, 5), //Anchors Change Colliders
                   MiscTools.DupeList(false, 5), //Fixes Scales
                   MiscTools.DupeList<Vector3?>(null, 5), //Manual Offsets
                   MiscTools.DupeList<IntVector2?>(new IntVector2(8, 8), 5), //Override colliders
                   MiscTools.DupeList<IntVector2?>(null, 5), //Override collider offsets
                   MiscTools.DupeList<Projectile>(null, 5)); // Override to copy from    
            blinky.baseData.speed *= 0.2f;
            blinky.baseData.force = 0;
            blinky.baseData.damage = 15;
            PierceProjModifier blinkypierce = blinky.gameObject.GetOrAddComponent<PierceProjModifier>();
            blinkypierce.penetration += 3;
            blinkypierce.penetratesBreakables = true;
            blinky.PenetratesInternalWalls = true;
            HomingModifier blinkyhoming = blinky.gameObject.GetOrAddComponent<HomingModifier>();
            blinkyhoming.AngularVelocity = 360;
            blinkyhoming.HomingRadius = 100;
            BounceProjModifier blinkybounce = blinky.gameObject.GetOrAddComponent<BounceProjModifier>();
            blinkybounce.numberOfBounces = 1;
            blinky.baseData.UsesCustomAccelerationCurve = true;
            blinky.baseData.AccelerationCurve = (PickupObjectDatabase.GetById(760) as Gun).DefaultModule.projectiles[0].baseData.AccelerationCurve;
            blinky.baseData.CustomAccelerationCurveDuration = (PickupObjectDatabase.GetById(760) as Gun).DefaultModule.projectiles[0].baseData.CustomAccelerationCurveDuration;
            blinky.baseData.IgnoreAccelCurveTime = (PickupObjectDatabase.GetById(760) as Gun).DefaultModule.projectiles[0].baseData.IgnoreAccelCurveTime;
            blinky.hitEffects.overrideMidairDeathVFX = RainbowGuonStone.RedGuonTransitionVFX;
            blinky.hitEffects.alwaysUseMidair = true;

            pinky = (PickupObjectDatabase.GetById(86) as Gun).DefaultModule.projectiles[0].InstantiateAndFakeprefab();
            pinky.AnimateProjectileBundle("GhostProjectilePinky",
                   Initialisation.ProjectileCollection,
                   Initialisation.projectileAnimationCollection,
                   "GhostProjectilePinky",
                   MiscTools.DupeList(new IntVector2(14, 27), 5), //Pixel Sizes
                   MiscTools.DupeList(false, 5), //Lightened
                   MiscTools.DupeList(tk2dBaseSprite.Anchor.MiddleCenter, 5), //Anchors
                   MiscTools.DupeList(true, 5), //Anchors Change Colliders
                   MiscTools.DupeList(false, 5), //Fixes Scales
                   MiscTools.DupeList<Vector3?>(null, 5), //Manual Offsets
                   MiscTools.DupeList<IntVector2?>(new IntVector2(8, 8), 5), //Override colliders
                   MiscTools.DupeList<IntVector2?>(null, 5), //Override collider offsets
                   MiscTools.DupeList<Projectile>(null, 5)); // Override to copy from               
            pinky.baseData.speed *= 0.2f;
            pinky.baseData.force = 0;
            pinky.baseData.damage = 15;
            PierceProjModifier pinkypierce = pinky.gameObject.GetOrAddComponent<PierceProjModifier>();
            pinkypierce.penetration += 3;
            pinkypierce.penetratesBreakables = true;
            pinky.PenetratesInternalWalls = true;
            HomingModifier pinkyhoming = pinky.gameObject.GetOrAddComponent<HomingModifier>();
            pinkyhoming.AngularVelocity = 360;
            pinkyhoming.HomingRadius = 100;
            BounceProjModifier pinkybounce = pinky.gameObject.GetOrAddComponent<BounceProjModifier>();
            pinkybounce.numberOfBounces = 1;
            pinky.baseData.UsesCustomAccelerationCurve = true;
            pinky.baseData.AccelerationCurve = (PickupObjectDatabase.GetById(760) as Gun).DefaultModule.projectiles[0].baseData.AccelerationCurve;
            pinky.baseData.CustomAccelerationCurveDuration = (PickupObjectDatabase.GetById(760) as Gun).DefaultModule.projectiles[0].baseData.CustomAccelerationCurveDuration;
            pinky.baseData.IgnoreAccelCurveTime = (PickupObjectDatabase.GetById(760) as Gun).DefaultModule.projectiles[0].baseData.IgnoreAccelCurveTime;
            pinky.hitEffects.overrideMidairDeathVFX = RainbowGuonStone.RedGuonTransitionVFX;
            pinky.hitEffects.alwaysUseMidair = true;

            inky = (PickupObjectDatabase.GetById(86) as Gun).DefaultModule.projectiles[0].InstantiateAndFakeprefab();
            inky.AnimateProjectileBundle("GhostProjectileInky",
                   Initialisation.ProjectileCollection,
                   Initialisation.projectileAnimationCollection,
                   "GhostProjectileInky",
                   MiscTools.DupeList(new IntVector2(14, 27), 5), //Pixel Sizes
                   MiscTools.DupeList(false, 5), //Lightened
                   MiscTools.DupeList(tk2dBaseSprite.Anchor.MiddleCenter, 5), //Anchors
                   MiscTools.DupeList(true, 5), //Anchors Change Colliders
                   MiscTools.DupeList(false, 5), //Fixes Scales
                   MiscTools.DupeList<Vector3?>(null, 5), //Manual Offsets
                   MiscTools.DupeList<IntVector2?>(new IntVector2(8, 8), 5), //Override colliders
                   MiscTools.DupeList<IntVector2?>(null, 5), //Override collider offsets
                   MiscTools.DupeList<Projectile>(null, 5)); // Override to copy from                  
            inky.baseData.speed *= 0.2f;
            inky.baseData.force = 0;
            inky.baseData.damage = 15;
            PierceProjModifier inkypierce = inky.gameObject.GetOrAddComponent<PierceProjModifier>();
            inkypierce.penetration += 3;
            inkypierce.penetratesBreakables = true;
            inky.PenetratesInternalWalls = true;
            HomingModifier inkyhoming = inky.gameObject.GetOrAddComponent<HomingModifier>();
            inkyhoming.AngularVelocity = 360;
            inkyhoming.HomingRadius = 100;
            BounceProjModifier inkybounce = inky.gameObject.GetOrAddComponent<BounceProjModifier>();
            inkybounce.numberOfBounces = 1;
            inky.baseData.UsesCustomAccelerationCurve = true;
            inky.baseData.AccelerationCurve = (PickupObjectDatabase.GetById(760) as Gun).DefaultModule.projectiles[0].baseData.AccelerationCurve;
            inky.baseData.CustomAccelerationCurveDuration = (PickupObjectDatabase.GetById(760) as Gun).DefaultModule.projectiles[0].baseData.CustomAccelerationCurveDuration;
            inky.baseData.IgnoreAccelCurveTime = (PickupObjectDatabase.GetById(760) as Gun).DefaultModule.projectiles[0].baseData.IgnoreAccelCurveTime;
            inky.hitEffects.overrideMidairDeathVFX = RainbowGuonStone.CyanGuonTransitionVFX;
            inky.hitEffects.alwaysUseMidair = true;

            clyde = (PickupObjectDatabase.GetById(86) as Gun).DefaultModule.projectiles[0].InstantiateAndFakeprefab();
            clyde.AnimateProjectileBundle("GhostProjectileClyde",
                   Initialisation.ProjectileCollection,
                   Initialisation.projectileAnimationCollection,
                   "GhostProjectileClyde",
                   MiscTools.DupeList(new IntVector2(14, 27), 5), //Pixel Sizes
                   MiscTools.DupeList(false, 5), //Lightened
                   MiscTools.DupeList(tk2dBaseSprite.Anchor.MiddleCenter, 5), //Anchors
                   MiscTools.DupeList(true, 5), //Anchors Change Colliders
                   MiscTools.DupeList(false, 5), //Fixes Scales
                   MiscTools.DupeList<Vector3?>(null, 5), //Manual Offsets
                   MiscTools.DupeList<IntVector2?>(new IntVector2(8, 8), 5), //Override colliders
                   MiscTools.DupeList<IntVector2?>(null, 5), //Override collider offsets
                   MiscTools.DupeList<Projectile>(null, 5)); // Override to copy from  
            clyde.baseData.speed *= 0.2f;
            clyde.baseData.force = 0;
            clyde.baseData.damage = 15;
            PierceProjModifier clydepierce = clyde.gameObject.GetOrAddComponent<PierceProjModifier>();
            clydepierce.penetration += 3;
            clydepierce.penetratesBreakables = true;
            clyde.PenetratesInternalWalls = true;
            HomingModifier clydehoming = clyde.gameObject.GetOrAddComponent<HomingModifier>();
            clydehoming.AngularVelocity = 360;
            clydehoming.HomingRadius = 100;
            BounceProjModifier clydebounce = clyde.gameObject.GetOrAddComponent<BounceProjModifier>();
            clydebounce.numberOfBounces = 1;
            clyde.baseData.UsesCustomAccelerationCurve = true;
            clyde.baseData.AccelerationCurve = (PickupObjectDatabase.GetById(760) as Gun).DefaultModule.projectiles[0].baseData.AccelerationCurve;
            clyde.baseData.CustomAccelerationCurveDuration = (PickupObjectDatabase.GetById(760) as Gun).DefaultModule.projectiles[0].baseData.CustomAccelerationCurveDuration;
            clyde.baseData.IgnoreAccelCurveTime = (PickupObjectDatabase.GetById(760) as Gun).DefaultModule.projectiles[0].baseData.IgnoreAccelCurveTime;
            clyde.hitEffects.overrideMidairDeathVFX = RainbowGuonStone.OrangeGuonTransitionVFX;
            clyde.hitEffects.alwaysUseMidair = true;
        }
        public static int ID;
        public void OnKilledEnemy(PlayerController player, HealthHaver enemy)
        {
            float activationchance = 0.5f;
            Projectile toSpawn = StandardisedProjectiles.ghost;
            if (player.PlayerHasActiveSynergy("Pac is Back!"))
            {
                List<Projectile> ghosts = new List<Projectile>()
                {
                    pinky, blinky, inky, clyde
                };
                toSpawn = BraveUtility.RandomElement(ghosts);
                activationchance = 1;
            }
            if (UnityEngine.Random.value <= activationchance)
            {
                int num = 1;
                if (player.PlayerHasActiveSynergy("Spectrecular")) { num++; }
                for (int i =0; i < num; i++)
                    {
                GameObject gemy = toSpawn.InstantiateAndFireInDirection(
                      enemy.specRigidbody.UnitCenter,
                      UnityEngine.Random.Range(0, 360));
                Projectile proj = gemy.GetComponent<Projectile>();
                proj.Owner = player;
                    proj.baseData.range *= 10;
                proj.Shooter = player.specRigidbody;
                proj.specRigidbody.RegisterGhostCollisionException(enemy.specRigidbody);
                player.DoPostProcessProjectile(proj);
                    
                    if (player.PlayerHasActiveSynergy("Spectrecular"))
                    {
                        ChainLightningModifier orAddComponent = proj.gameObject.GetOrAddComponent<ChainLightningModifier>();
                        orAddComponent.LinkVFXPrefab = (PickupObjectDatabase.GetById(298) as ComplexProjectileModifier).ChainLightningVFX;
                        orAddComponent.damageTypes = (PickupObjectDatabase.GetById(298) as ComplexProjectileModifier).ChainLightningDamageTypes;
                        orAddComponent.maximumLinkDistance = (PickupObjectDatabase.GetById(298) as ComplexProjectileModifier).ChainLightningMaxLinkDistance;
                        orAddComponent.damagePerHit = (PickupObjectDatabase.GetById(298) as ComplexProjectileModifier).ChainLightningDamagePerHit;
                        orAddComponent.damageCooldown = (PickupObjectDatabase.GetById(298) as ComplexProjectileModifier).ChainLightningDamageCooldown;
                        orAddComponent.UsesDispersalParticles = false;

                        proj.baseData.damage *= 0.5f;
                        proj.RuntimeUpdateScale(0.75f);
                    }
                }
            }
        }
        public override void Pickup(PlayerController player)
        {
            base.Pickup(player);
            player.OnKilledEnemyContext += OnKilledEnemy;
        }
        public override void DisableEffect(PlayerController player)
        {
            if (player != null)
            {
                player.OnKilledEnemyContext -= OnKilledEnemy;
            }
            base.DisableEffect(player);
        }
        public static Projectile blinky;
        public static Projectile pinky;
        public static Projectile inky;
        public static Projectile clyde;
    }
}