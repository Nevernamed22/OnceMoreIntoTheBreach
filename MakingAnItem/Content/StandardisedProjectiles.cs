using Alexandria.Assetbundle;
using Alexandria.ItemAPI;
using Alexandria.Misc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace NevernamedsItems
{
    public static class StandardisedProjectiles
    {
        public static void Init()
        {
            Projectile projectile = (PickupObjectDatabase.GetById(56) as Gun).DefaultModule.projectiles[0].InstantiateAndFakeprefab();
            projectile.AnimateProjectileBundle("SmokeProjectile",
                   Initialisation.ProjectileCollection,
                   Initialisation.projectileAnimationCollection,
                   "SmokeProjectile",
                   MiscTools.DupeList(new IntVector2(48, 48), 12), //Pixel Sizes
                   MiscTools.DupeList(false, 12), //Lightened
                   MiscTools.DupeList(tk2dBaseSprite.Anchor.MiddleCenter, 12), //Anchors
                   MiscTools.DupeList(true, 12), //Anchors Change Colliders
                   MiscTools.DupeList(false, 12), //Fixes Scales
                   MiscTools.DupeList<Vector3?>(null,12), //Manual Offsets
                   MiscTools.DupeList<IntVector2?>(new IntVector2(26, 26), 12), //Override colliders
                   MiscTools.DupeList<IntVector2?>(null, 12), //Override collider offsets
                   MiscTools.DupeList<Projectile>(null, 12)); // Override to copy from               
            projectile.sprite.renderer.material.shader = ShaderCache.Acquire("Brave/LitBlendUber");
            projectile.sprite.renderer.material.SetFloat("_VertexColor", 1f);
            projectile.sprite.color = projectile.sprite.color.WithAlpha(0.65f);
            projectile.sprite.usesOverrideMaterial = true;
            projectile.baseData.speed *= 0.65f;
            projectile.baseData.force = 0;
            projectile.baseData.damage = 1;
            PierceProjModifier pierce = projectile.gameObject.GetOrAddComponent<PierceProjModifier>();
            pierce.penetration += 100;
            pierce.penetratesBreakables = true;
            BounceProjModifier bounce = projectile.gameObject.GetOrAddComponent<BounceProjModifier>();
            bounce.numberOfBounces = 5;
            SlowDownOverTimeModifier slowDown = projectile.gameObject.GetOrAddComponent<SlowDownOverTimeModifier>();
            slowDown.extendTimeByRangeStat = true;
            slowDown.killAfterCompleteStop = true;
            slowDown.targetSpeed = 0;
            slowDown.timeToSlowOver = 1f;
            slowDown.timeTillKillAfterCompleteStop = 20f;
            slowDown.doRandomTimeMultiplier = true;
            projectile.gameObject.GetOrAddComponent<DieWhenOwnerNotInRoom>();

            VFXPool SmokePoof = VFXToolbox.CreateVFXPool("SmokePoof",
                new List<string>()
                {
                    "NevernamedsItems/Resources/MiscVFX/GunVFX/smokeimpact_vfx_001",
                    "NevernamedsItems/Resources/MiscVFX/GunVFX/smokeimpact_vfx_002",
                    "NevernamedsItems/Resources/MiscVFX/GunVFX/smokeimpact_vfx_003",
                    "NevernamedsItems/Resources/MiscVFX/GunVFX/smokeimpact_vfx_004",
                    "NevernamedsItems/Resources/MiscVFX/GunVFX/smokeimpact_vfx_005",
                    "NevernamedsItems/Resources/MiscVFX/GunVFX/smokeimpact_vfx_006",
                },
                10, //FPS
                new IntVector2(48, 48), //Dimensions
                tk2dBaseSprite.Anchor.MiddleLeft, //Anchor
                false, //Uses a Z height off the ground
                0 //The Z height, if used
                  );
            tk2dBaseSprite smokePoofSprite = SmokePoof.effects[0].effects[0].effect.GetComponent<tk2dBaseSprite>();
            smokePoofSprite.renderer.material.shader = ShaderCache.Acquire("Brave/LitBlendUber");
            smokePoofSprite.renderer.material.SetFloat("_VertexColor", 1f);
            smokePoofSprite.color = projectile.sprite.color.WithAlpha(0.65f);
            smokePoofSprite.usesOverrideMaterial = true;

            projectile.hitEffects.tileMapVertical = SmokePoof;
            projectile.hitEffects.tileMapHorizontal = SmokePoof;
            projectile.hitEffects.deathAny = SmokePoof;
            projectile.hitEffects.overrideMidairDeathVFX = SmokePoof.effects[0].effects[0].effect;
            VFXPool smallSmokePoof = VFXToolbox.CreateVFXPool("SmokePoof Small",
                new List<string>()
                {
                    "NevernamedsItems/Resources/MiscVFX/GunVFX/smokeenemyimpact_vfx_001",
                    "NevernamedsItems/Resources/MiscVFX/GunVFX/smokeenemyimpact_vfx_002",
                    "NevernamedsItems/Resources/MiscVFX/GunVFX/smokeenemyimpact_vfx_003",
                    "NevernamedsItems/Resources/MiscVFX/GunVFX/smokeenemyimpact_vfx_004",
                    "NevernamedsItems/Resources/MiscVFX/GunVFX/smokeenemyimpact_vfx_005",
                    "NevernamedsItems/Resources/MiscVFX/GunVFX/smokeenemyimpact_vfx_006",
                },
                10, //FPS
                new IntVector2(24, 24), //Dimensions
                tk2dBaseSprite.Anchor.MiddleLeft, //Anchor
                false, //Uses a Z height off the ground
                0 //The Z height, if used
                  );
            tk2dBaseSprite smallSmokePoofSprite = smallSmokePoof.effects[0].effects[0].effect.GetComponent<tk2dBaseSprite>();
            smallSmokePoofSprite.renderer.material.shader = ShaderCache.Acquire("Brave/LitBlendUber");
            smallSmokePoofSprite.renderer.material.SetFloat("_VertexColor", 1f);
            smallSmokePoofSprite.color = projectile.sprite.color.WithAlpha(0.65f);
            smallSmokePoofSprite.usesOverrideMaterial = true;
            projectile.hitEffects.enemy = smallSmokePoof;
            smoke = projectile;

            Projectile flamethrowerBase = (PickupObjectDatabase.GetById(56) as Gun).DefaultModule.projectiles[0].InstantiateAndFakeprefab();
            flamethrowerBase.AnimateProjectileBundle("FlamethrowerProjectile",
                   Initialisation.ProjectileCollection,
                   Initialisation.projectileAnimationCollection,
                   "FlamethrowerProjectile",
                   MiscTools.DupeList(new IntVector2(32, 31), 13), //Pixel Sizes
                   MiscTools.DupeList(false, 13), //Lightened
                   MiscTools.DupeList(tk2dBaseSprite.Anchor.MiddleCenter, 13), //Anchors
                   MiscTools.DupeList(true, 13), //Anchors Change Colliders
                   MiscTools.DupeList(false, 13), //Fixes Scales
                   MiscTools.DupeList<Vector3?>(null, 13), //Manual Offsets
                   MiscTools.DupeList<IntVector2?>(new IntVector2(18, 18), 13), //Override colliders
                   MiscTools.DupeList<IntVector2?>(null, 13), //Override collider offsets
                   MiscTools.DupeList<Projectile>(null, 13)); // Override to copy from    
            flamethrowerBase.baseData.speed *= 0.65f;
            flamethrowerBase.baseData.force = 0;
            flamethrowerBase.baseData.damage = 2;
            flamethrowerBase.baseData.range = 16;
            flamethrowerBase.AppliesFire = true;
            flamethrowerBase.FireApplyChance = 1;
            flamethrowerBase.fireEffect = StaticStatusEffects.hotLeadEffect;
            PierceProjModifier pierce2 = flamethrowerBase.gameObject.GetOrAddComponent<PierceProjModifier>();
            pierce2.penetration += 100;
            pierce2.penetratesBreakables = true;
            BulletLifeTimer timer = flamethrowerBase.gameObject.GetOrAddComponent<BulletLifeTimer>();
            timer.secondsTillDeath = 1;
            ParticleShitter particles = flamethrowerBase.gameObject.GetOrAddComponent<ParticleShitter>();
            particles.particleType = GlobalSparksDoer.SparksType.STRAIGHT_UP_FIRE;

            VFXPool fireImp = VFXToolbox.CreateBlankVFXPool();
            fireImp.effects[0].effects[0].effect = (PickupObjectDatabase.GetById(336) as Gun).DefaultModule.projectiles[0].hitEffects.overrideMidairDeathVFX;
            flamethrowerBase.hitEffects.tileMapVertical = smoke.hitEffects.tileMapVertical;
            flamethrowerBase.hitEffects.tileMapHorizontal = smoke.hitEffects.tileMapHorizontal;
            flamethrowerBase.hitEffects.deathAny = smoke.hitEffects.deathAny;
            flamethrowerBase.hitEffects.overrideMidairDeathVFX = smoke.hitEffects.overrideMidairDeathVFX;
            flamethrowerBase.hitEffects.enemy = fireImp;

            flamethrowerBase.enemyImpactEventName = "flame";
            flamethrowerBase.objectImpactEventName = "flame";

            flamethrower = flamethrowerBase;


            ghost = (PickupObjectDatabase.GetById(86) as Gun).DefaultModule.projectiles[0].InstantiateAndFakeprefab();
            ghost.AnimateProjectileBundle("GhostProjectile",
                   Initialisation.ProjectileCollection,
                   Initialisation.projectileAnimationCollection,
                   "GhostProjectile",
                   MiscTools.DupeList(new IntVector2(14, 27), 5), //Pixel Sizes
                   MiscTools.DupeList(false, 5), //Lightened
                   MiscTools.DupeList(tk2dBaseSprite.Anchor.MiddleCenter, 5), //Anchors
                   MiscTools.DupeList(true, 5), //Anchors Change Colliders
                   MiscTools.DupeList(false, 5), //Fixes Scales
                   MiscTools.DupeList<Vector3?>(null, 5), //Manual Offsets
                   MiscTools.DupeList<IntVector2?>(new IntVector2(8, 8), 5), //Override colliders
                   MiscTools.DupeList<IntVector2?>(null, 5), //Override collider offsets
                   MiscTools.DupeList<Projectile>(null, 5)); // Override to copy from              
            ghost.sprite.renderer.material.shader = ShaderCache.Acquire("Brave/LitBlendUber");
            ghost.sprite.renderer.material.SetFloat("_VertexColor", 1f);
            ghost.sprite.color = ghost.sprite.color.WithAlpha(0.8f);
            ghost.sprite.usesOverrideMaterial = true;
            ghost.baseData.speed *= 0.2f;
            ghost.baseData.force = 0;
            ghost.baseData.damage = 15;
            PierceProjModifier ghostpierce = ghost.gameObject.GetOrAddComponent<PierceProjModifier>();
            ghostpierce.penetration += 3;
            ghostpierce.penetratesBreakables = true;
            ghost.PenetratesInternalWalls = true;
            HomingModifier ghosthoming = ghost.gameObject.GetOrAddComponent<HomingModifier>();
            ghosthoming.AngularVelocity = 360;
            ghosthoming.HomingRadius = 100;
            BounceProjModifier ghostbounce = ghost.gameObject.GetOrAddComponent<BounceProjModifier>();
            ghostbounce.numberOfBounces = 1;
            ghost.baseData.UsesCustomAccelerationCurve = true;
            ghost.baseData.AccelerationCurve = (PickupObjectDatabase.GetById(760) as Gun).DefaultModule.projectiles[0].baseData.AccelerationCurve;
            ghost.baseData.CustomAccelerationCurveDuration = (PickupObjectDatabase.GetById(760) as Gun).DefaultModule.projectiles[0].baseData.CustomAccelerationCurveDuration;
            ghost.baseData.IgnoreAccelCurveTime = (PickupObjectDatabase.GetById(760) as Gun).DefaultModule.projectiles[0].baseData.IgnoreAccelCurveTime;

            ghost.hitEffects.overrideMidairDeathVFX = SharedVFX.ColouredPoofWhite;
            ghost.hitEffects.alwaysUseMidair = true;


            Projectile initsnake = (PickupObjectDatabase.GetById(56) as Gun).DefaultModule.projectiles[0].InstantiateAndFakeprefab();
            initsnake.gameObject.SetActive(false);
            FakePrefab.MarkAsFakePrefab(initsnake.gameObject);
            UnityEngine.Object.DontDestroyOnLoad(initsnake);
            initsnake.baseData.damage = 13f;
            initsnake.baseData.force *= 0.2f;
            initsnake.baseData.speed *= 0.5f;
            initsnake.PoisonApplyChance = 0.2f;
            initsnake.AppliesPoison = true;
            initsnake.healthEffect = StaticStatusEffects.irradiatedLeadEffect;
            HomingModifier snakeHominh = initsnake.gameObject.GetOrAddComponent<HomingModifier>();
            snakeHominh.AngularVelocity = 360;
            snakeHominh.HomingRadius = 100;
            BounceProjModifier snakeBounce = initsnake.gameObject.GetOrAddComponent<BounceProjModifier>();
            snakeBounce.numberOfBounces = 2;
            PierceProjModifier snakePierce = initsnake.gameObject.GetOrAddComponent<PierceProjModifier>();
            snakePierce.penetration = 2;
            initsnake.pierceMinorBreakables = true;
            initsnake.gameObject.AddComponent<ConvertToHelixOnSpawn>();
            initsnake.SetProjectileSprite("snake_proj", 12, 9, false, tk2dBaseSprite.Anchor.MiddleCenter, 10, 7);

            VFXPool bite = VFXToolbox.CreateVFXPool($"Bite Impact",
               new List<string>()
               {
                    $"NevernamedsItems/Resources/MiscVFX/biteimpact_001",
                    $"NevernamedsItems/Resources/MiscVFX/biteimpact_002",
                    $"NevernamedsItems/Resources/MiscVFX/biteimpact_003",
                    $"NevernamedsItems/Resources/MiscVFX/biteimpact_004",
                    $"NevernamedsItems/Resources/MiscVFX/biteimpact_005",
               },
               13, //FPS
               new IntVector2(27, 17), //Dimensions
               tk2dBaseSprite.Anchor.MiddleCenter, //Anchor
               true, //Uses a Z height off the ground
               0.2f, //The Z height, if used
               false,
              VFXAlignment.Fixed
                 );
            initsnake.hitEffects.enemy = bite;

            EasyTrailBullet snakeTrail = initsnake.gameObject.AddComponent<EasyTrailBullet>();
            snakeTrail.TrailPos = initsnake.transform.position;
            snakeTrail.StartWidth = 0.25f;
            snakeTrail.EndWidth = 0f;
            snakeTrail.LifeTime = 0.4f;
            Color snakeGreen = new Color(71f /255f, 210f / 255f, 49f / 255f);
            snakeTrail.BaseColor = snakeGreen;
            snakeTrail.StartColor = snakeGreen;
            snakeTrail.EndColor = snakeGreen;

            VFXPool snakedeath = VFXToolbox.CreateVFXPool("Snake Death",
              new List<string>()
              {
                    $"NevernamedsItems/Resources/MiscVFX/GunVFX/snakeproj_death_001",
                    $"NevernamedsItems/Resources/MiscVFX/GunVFX/snakeproj_death_002",
                    $"NevernamedsItems/Resources/MiscVFX/GunVFX/snakeproj_death_003",
                    $"NevernamedsItems/Resources/MiscVFX/GunVFX/snakeproj_death_004",
                    $"NevernamedsItems/Resources/MiscVFX/GunVFX/snakeproj_death_005",
                    $"NevernamedsItems/Resources/MiscVFX/GunVFX/snakeproj_death_006",
                    $"NevernamedsItems/Resources/MiscVFX/GunVFX/snakeproj_death_007",
                    $"NevernamedsItems/Resources/MiscVFX/GunVFX/snakeproj_death_008",
                    $"NevernamedsItems/Resources/MiscVFX/GunVFX/snakeproj_death_009",
                    $"NevernamedsItems/Resources/MiscVFX/GunVFX/snakeproj_death_010",
              },
              13, //FPS
              new IntVector2(29, 59), //Dimensions
              tk2dBaseSprite.Anchor.LowerCenter, //Anchor
              true, //Uses a Z height off the ground
              -1.73f, //The Z height, if used
              true,
             VFXAlignment.Fixed
                );

            snakedeath.effects[0].effects[0].effect.gameObject.AddComponent<IsCorpseWithSynergy>();
            // = LayerMask.NameToLayer("BG_Critical");
            initsnake.hitEffects.deathAny = snakedeath;
            initsnake.hitEffects.HasProjectileDeathVFX = true;

            snake = initsnake;

        }
        public static Projectile snake;
        public static Projectile smoke;
        public static Projectile ghost;
        public static Projectile flamethrower;
        private class IsCorpseWithSynergy : MonoBehaviour
        {
            public void Start()
            {
                if (GameManager.Instance.AnyPlayerHasActiveSynergy("Rock Python")) { StaticReferenceManager.AllCorpses.Add(base.gameObject); }
            }
        }
    }
    
}
