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
using Alexandria.Assetbundle;
using Dungeonator;
using SaveAPI;

namespace NevernamedsItems
{
    public class DomeLord : GunBehaviour
    {
        public static string IdleSOUTH;
        public static string IdleSE;
        public static string IdleEAST;
        public static string IdleNE;
        public static string IdleNORTH;
        public static string IdleNW;
        public static string IdleWEST;
        public static string IdleSW;

        public static string FireSOUTH;
        public static string FireSE;
        public static string FireEAST;
        public static string FireNE;
        public static string FireNORTH;
        public static string FireNW;
        public static string FireWEST;
        public static string FireSW;
        public static void Add()
        {
            Gun gun = ETGMod.Databases.Items.NewGun("Dome Lord", "domelord");
            Game.Items.Rename("outdated_gun_mods:dome_lord", "nn:dome_lord");
            var behav = gun.gameObject.AddComponent<DomeLord>();
            gun.SetShortDescription("King of the Hill");
            gun.SetLongDescription("A new lord to rule over your listless dome!\n\nDiminutive spawn of the elusive parriarch of portals, earl of entryways, the Lord of Doors.");

            //ETGModConsole.Log("1");
            gun.SetGunSprites("domelord", 8, false, 2);
            //ETGModConsole.Log("2");

            IdleSOUTH = gun.UpdateAnimation("idleSOUTH", Initialisation.gunCollection2);
            IdleSE = gun.UpdateAnimation("idleSE", Initialisation.gunCollection2);
            IdleEAST = gun.UpdateAnimation("idleEAST", Initialisation.gunCollection2);
            IdleNE = gun.UpdateAnimation("idleNE", Initialisation.gunCollection2);
            IdleNORTH = gun.UpdateAnimation("idleNORTH", Initialisation.gunCollection2);
            IdleNW = gun.UpdateAnimation("idleNW", Initialisation.gunCollection2);
            IdleWEST = gun.UpdateAnimation("idleWEST", Initialisation.gunCollection2);
            IdleSW = gun.UpdateAnimation("idleSW", Initialisation.gunCollection2);
            //ETGModConsole.Log("3");

            gun.SetAnimationFPS(IdleSOUTH, 8);
            gun.SetAnimationFPS(IdleSE, 8);
            gun.SetAnimationFPS(IdleEAST, 8);
            gun.SetAnimationFPS(IdleNE, 8);
            gun.SetAnimationFPS(IdleNORTH, 8);
            gun.SetAnimationFPS(IdleNW, 8);
            gun.SetAnimationFPS(IdleWEST, 8);
            gun.SetAnimationFPS(IdleSW, 8);
            //ETGModConsole.Log("4");

            
            FireSOUTH = gun.UpdateAnimation("fireSOUTH", Initialisation.gunCollection2);
            FireSE = gun.UpdateAnimation("fireSE", Initialisation.gunCollection2);
            FireEAST = gun.UpdateAnimation("fireEAST", Initialisation.gunCollection2);
            FireNE = gun.UpdateAnimation("fireNE", Initialisation.gunCollection2);
            FireNORTH = gun.UpdateAnimation("fireNORTH", Initialisation.gunCollection2);
            FireNW = gun.UpdateAnimation("fireNW", Initialisation.gunCollection2);
            FireWEST = gun.UpdateAnimation("fireWEST", Initialisation.gunCollection2);
            FireSW = gun.UpdateAnimation("fireSW", Initialisation.gunCollection2);
            //ETGModConsole.Log("5");

            gun.SetAnimationFPS(FireSOUTH, 8);
            gun.SetAnimationFPS(FireSE, 8);
            gun.SetAnimationFPS(FireEAST, 8);
            gun.SetAnimationFPS(FireNE, 8);
            gun.SetAnimationFPS(FireNORTH, 8);
            gun.SetAnimationFPS(FireNW, 8);
            gun.SetAnimationFPS(FireWEST, 8);
            gun.SetAnimationFPS(FireSW, 8);
            //ETGModConsole.Log("6");
            
            
              tk2dSpriteAnimator animator = gun.GetComponent<tk2dSpriteAnimator>();
              animator.GetClipByName(FireSOUTH).wrapMode = tk2dSpriteAnimationClip.WrapMode.Loop;
              animator.GetClipByName(FireSE).wrapMode = tk2dSpriteAnimationClip.WrapMode.Loop;
              animator.GetClipByName(FireEAST).wrapMode = tk2dSpriteAnimationClip.WrapMode.Loop;
              animator.GetClipByName(FireNE).wrapMode = tk2dSpriteAnimationClip.WrapMode.Loop;
              animator.GetClipByName(FireNORTH).wrapMode = tk2dSpriteAnimationClip.WrapMode.Loop;
              animator.GetClipByName(FireNW).wrapMode = tk2dSpriteAnimationClip.WrapMode.Loop;
              animator.GetClipByName(FireWEST).wrapMode = tk2dSpriteAnimationClip.WrapMode.Loop;
              animator.GetClipByName(FireSW).wrapMode = tk2dSpriteAnimationClip.WrapMode.Loop;
              animator.GetClipByName(FireSW).wrapMode = tk2dSpriteAnimationClip.WrapMode.Loop;
              
            //ETGModConsole.Log("7");

            gun.usesContinuousFireAnimation = true;
            gun.usesContinuousMuzzleFlash = true;

            gun.usesDirectionalAnimator = true;
            gun.preventRotation = true;
            gun.carryPixelOffset = new IntVector2(8, 15);
            gun.leftFacingPixelOffset = new IntVector2(-5, 0);


            gun.IgnoresAngleQuantization = true;

            gun.muzzleFlashEffects = VFXToolbox.CreateVFXPoolBundle("DomeLordMuzzle", false, 0f, VFXAlignment.Fixed, 50, Color.red);
            gun.muzzleFlashEffects.type = VFXPoolType.All;

            AIAnimator aIAnimator = gun.gameObject.GetOrAddComponent<AIAnimator>();
            aIAnimator.IdleAnimation = new DirectionalAnimation
            {
                Type = DirectionalAnimation.DirectionType.EightWayOrdinal,
                Prefix = "domelord_idle",
                AnimNames = new string[]
                {
                        IdleNORTH,
                        IdleNE,
                        IdleEAST,
                        IdleSE,
                        IdleSOUTH,
                        IdleSW,
                        IdleWEST,
                        IdleNW,
                },
                Flipped = new DirectionalAnimation.FlipType[]
                {
                    DirectionalAnimation.FlipType.None,
                    DirectionalAnimation.FlipType.None,
                    DirectionalAnimation.FlipType.None,
                    DirectionalAnimation.FlipType.None,
                    DirectionalAnimation.FlipType.None,
                    DirectionalAnimation.FlipType.None,
                    DirectionalAnimation.FlipType.None,
                    DirectionalAnimation.FlipType.None,
                }
            };
              AIAnimator.NamedDirectionalAnimation fireAnim = new AIAnimator.NamedDirectionalAnimation
              {
                  name = "domelord_fire",
                  anim = new DirectionalAnimation
                  {
                      Prefix =  string.Empty,
                      Type = DirectionalAnimation.DirectionType.EightWayOrdinal,
                      Flipped = new DirectionalAnimation.FlipType[8],
                      AnimNames = new string[]
                  {
                          FireNORTH,
                          FireNE,
                          FireEAST,
                          FireSE,
                          FireSOUTH,
                          FireSW,
                          FireWEST,
                          FireNW,
                  },
                  }
              };
              if (aIAnimator.OtherAnimations == null) aIAnimator.OtherAnimations = new List<AIAnimator.NamedDirectionalAnimation>();
              aIAnimator.OtherAnimations.Add(fireAnim);
           
            gun.isAudioLoop = true;
            gun.gunClass = GunClass.BEAM;
            gun.AddProjectileModuleFrom(PickupObjectDatabase.GetById(86) as Gun, true, false);



            gun.DefaultModule.angleVariance = 0;
            gun.DefaultModule.ammoCost = 11;
            gun.DefaultModule.shootStyle = ProjectileModule.ShootStyle.Beam;
            gun.DefaultModule.sequenceStyle = ProjectileModule.ProjectileSequenceStyle.Random;
            gun.DefaultModule.cooldownTime = 0.001f;
            gun.DefaultModule.numberOfShotsInClip = -1;

   

            Projectile projectile = ProjectileUtility.SetupProjectile(86);
            BasicBeamController beamComp = projectile.GenerateAnchoredBeamPrefabBundle(
                        "domelordbeam_mid_001",
                        Initialisation.ProjectileCollection,
                        Initialisation.projectileAnimationCollection,
                        "DomeLordBeamMid",
                        new Vector2(4, 2),
                        new Vector2(0, -1)
                        );
            EmmisiveBeams emission = projectile.gameObject.GetOrAddComponent<EmmisiveBeams>();
            emission.EmissivePower = 50;
            emission.EmissiveColorPower = 50;
            emission.EmissiveColor = new Color(1f, 161f / 255f, 0f);

            beamComp.HeightOffset = 10;


            projectile.gameObject.name = "Dome Lord Beam";
            projectile.baseData.damage = 25f;
            projectile.baseData.force = 10f;
            projectile.baseData.range = 9;
            projectile.baseData.speed = float.MaxValue;
            beamComp.boneType = BasicBeamController.BeamBoneType.Straight;
            beamComp.endAudioEvent = "Stop_WPN_radiationlaser_loop_01";
            beamComp.startAudioEvent = "Play_WPN_radiationlaser_shot_01";
            beamComp.penetration = 10;
            beamComp.PenetratesCover = true;
            
            GoopModifier goop = projectile.gameObject.AddComponent<GoopModifier>();
            goop.SpawnAtBeamEnd = true;
            goop.SpawnGoopOnCollision = true;
            goop.BeamEndRadius = 0.5f;
            goop.CollisionSpawnRadius = 0.25f;
            goop.goopDefinition = GoopUtility.FireDef;

            OscillatingProjectileModifier osc = projectile.gameObject.AddComponent<OscillatingProjectileModifier>();
            osc.multiplyRange = true;

            projectile.gameObject.AddComponent<BeamAttachVFXToEnd>().VFX = VFXToolbox.CreateVFXBundle("DomeLordEndVFX", true, 0.18f, 5, 10, new Color(1f,0f, 0f), overrideCollection: Initialisation.ProjectileCollection);

            gun.DefaultModule.projectiles[0] = projectile;

            gun.gunSwitchGroup = "nn:EMPTY";

            //GUN STATS
            gun.gunScreenShake = (PickupObjectDatabase.GetById(60) as Gun).gunScreenShake;
            gun.reloadTime = 1.25f;
            gun.muzzleFlashEffects.type = VFXPoolType.None;
            gun.SetBarrel(6, 11);
            gun.SetBaseMaxAmmo(900);
            gun.ammo = 900;

            gun.DefaultModule.ammoType = GameUIAmmoType.AmmoType.CUSTOM;
            gun.DefaultModule.customAmmoType = "Y-Beam Laser";


            gun.AddCurrentGunDamageTypeModifier(CoreDamageTypes.Fire, 0);

            gun.gunHandedness = GunHandedness.NoHanded;

            gun.quality = PickupObject.ItemQuality.B;
            ETGMod.Databases.Items.Add(gun, false, "ANY");

            gun.SetupUnlockOnCustomStat(CustomTrackedStats.DOOR_LORD_KILLS, 0, DungeonPrerequisite.PrerequisiteOperation.GREATER_THAN);


        }
    }
}