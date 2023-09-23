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
            projectile.AnimateProjectile(new List<string> {
                "smokeproj_001",
                "smokeproj_002",
                "smokeproj_003",
                "smokeproj_004",
                "smokeproj_005",
                "smokeproj_006",
                "smokeproj_007",
                "smokeproj_008",
                "smokeproj_009",
                "smokeproj_010",
                "smokeproj_011",
                "smokeproj_012",
            }, 12, tk2dSpriteAnimationClip.WrapMode.Loop, AnimateBullet.ConstructListOfSameValues(new IntVector2(48, 48), 12),
            AnimateBullet.ConstructListOfSameValues(false, 12),
            AnimateBullet.ConstructListOfSameValues(tk2dBaseSprite.Anchor.MiddleCenter, 12),
            AnimateBullet.ConstructListOfSameValues(true, 12),
            AnimateBullet.ConstructListOfSameValues(false, 12),
            AnimateBullet.ConstructListOfSameValues<Vector3?>(null, 12),
            AnimateBullet.ConstructListOfSameValues<IntVector2?>(new IntVector2(26, 26), 12),
            AnimateBullet.ConstructListOfSameValues<IntVector2?>(null, 12),
            AnimateBullet.ConstructListOfSameValues<Projectile>(null, 12), 0);
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
            flamethrowerBase.AnimateProjectile(new List<string> {
                "flamethrower_proj1",
                "flamethrower_proj2",
                "flamethrower_proj3",
                "flamethrower_proj4",
                "flamethrower_proj5",
                "flamethrower_proj6",
                "flamethrower_proj7",
                "flamethrower_proj8",
                "flamethrower_proj9",
                "flamethrower_proj10",
                "flamethrower_proj11",
                "flamethrower_proj12",
                "flamethrower_proj13",
            }, 13, tk2dSpriteAnimationClip.WrapMode.LoopSection, AnimateBullet.ConstructListOfSameValues(new IntVector2(32, 31), 13),
            AnimateBullet.ConstructListOfSameValues(false, 13),
            AnimateBullet.ConstructListOfSameValues(tk2dBaseSprite.Anchor.MiddleCenter, 13),
            AnimateBullet.ConstructListOfSameValues(true, 13),
            AnimateBullet.ConstructListOfSameValues(false, 13),
            AnimateBullet.ConstructListOfSameValues<Vector3?>(null, 13),
            AnimateBullet.ConstructListOfSameValues<IntVector2?>(new IntVector2(18, 18), 13),
            AnimateBullet.ConstructListOfSameValues<IntVector2?>(null, 13),
            AnimateBullet.ConstructListOfSameValues<Projectile>(null, 13), 9);

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

            flamethrower = flamethrowerBase;


            ghost = (PickupObjectDatabase.GetById(86) as Gun).DefaultModule.projectiles[0].InstantiateAndFakeprefab();
            ghost.AnimateProjectile(new List<string> {
                "ghostproj_001",
                "ghostproj_002",
                "ghostproj_003",
                "ghostproj_004",
                "ghostproj_005"
            }, 10, tk2dSpriteAnimationClip.WrapMode.Loop, AnimateBullet.ConstructListOfSameValues(new IntVector2(14, 27), 5),
            AnimateBullet.ConstructListOfSameValues(false, 5),
            AnimateBullet.ConstructListOfSameValues(tk2dBaseSprite.Anchor.MiddleCenter, 5),
            AnimateBullet.ConstructListOfSameValues(true, 5),
            AnimateBullet.ConstructListOfSameValues(false, 5),
            AnimateBullet.ConstructListOfSameValues<Vector3?>(null, 5),
            AnimateBullet.ConstructListOfSameValues<IntVector2?>(new IntVector2(8, 8), 5),
            AnimateBullet.ConstructListOfSameValues<IntVector2?>(null, 5),
            AnimateBullet.ConstructListOfSameValues<Projectile>(null, 5), 0);
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

            ghost.hitEffects.overrideMidairDeathVFX = RainbowGuonStone.WhiteGuonTransitionVFX;
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
            initsnake.SetProjectileSpriteRight("snake_proj", 12, 9, false, tk2dBaseSprite.Anchor.MiddleCenter, 10, 7);

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
            snakeTrail.TrailPos = projectile.transform.position;
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
            //snakedeath.effects[0].effects[0].effect.gameObject.layer = LayerMask.NameToLayer("BG_Critical");
            initsnake.hitEffects.deathAny = snakedeath;
            initsnake.hitEffects.HasProjectileDeathVFX = true;

            snake = initsnake;

        }
        public static Projectile snake;
        public static Projectile smoke;
        public static Projectile ghost;
        public static Projectile flamethrower;
    }
}
