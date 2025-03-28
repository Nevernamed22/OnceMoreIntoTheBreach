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

namespace NevernamedsItems
{
    public class Solstice : AdvancedGunBehavior
    {
        public static void Add()
        {
            Gun gun = ETGMod.Databases.Items.NewGun("Solstice", "solstice");
            Game.Items.Rename("outdated_gun_mods:solstice", "nn:solstice");
            var behav = gun.gameObject.AddComponent<Solstice>();
            behav.preventNormalFireAudio = true;
            gun.SetShortDescription("Flaring Muzzle");
            gun.SetLongDescription("Contains a miniature portal directly to the core of Gunymede's star, channeling immense thermal and gravitational energies.");

            gun.SetGunSprites("solstice", 8, false, 2);

            gun.SetAnimationFPS(gun.shootAnimation, 8);
            gun.GetComponent<tk2dSpriteAnimator>().GetClipByName(gun.shootAnimation).wrapMode = tk2dSpriteAnimationClip.WrapMode.LoopSection;
            gun.GetComponent<tk2dSpriteAnimator>().GetClipByName(gun.shootAnimation).loopStart = 1;

            gun.SetAnimationFPS(gun.reloadAnimation, 12);


            gun.isAudioLoop = true;
            gun.gunClass = GunClass.BEAM;
            for (int i = 0; i < 2; i++)
            {
                gun.AddProjectileModuleFrom(PickupObjectDatabase.GetById(86) as Gun, true, false);
            }

            gun.Volley.projectiles[0].angleVariance = 0;
            gun.Volley.projectiles[0].ammoCost = 11;
            gun.Volley.projectiles[0].shootStyle = ProjectileModule.ShootStyle.Beam;
            gun.Volley.projectiles[0].sequenceStyle = ProjectileModule.ProjectileSequenceStyle.Random;
            gun.Volley.projectiles[0].cooldownTime = 0.001f;
            gun.Volley.projectiles[0].numberOfShotsInClip = 400;
            Projectile projectile = ProjectileUtility.SetupProjectile(86);
            BasicBeamController beamComp = projectile.GenerateAnchoredBeamPrefabBundle(
                        "solsticebeam_mid_001",
                        Initialisation.ProjectileCollection,
                        Initialisation.projectileAnimationCollection,
                        "SolsticeBeam_Mid",
                        new Vector2(16, 4),
                        new Vector2(0, -2),
                        muzzleAnimationName: "SolsticeBeam_Start",
                        muzzleColliderDimensions: new Vector2(16, 4),
                        muzzleColliderOffsets: new Vector2(0, -2),
                        impactVFXAnimationName: "SolsticeBeam_Impact",
                        impactVFXColliderDimensions: new Vector2(4, 4),
                        impactVFXColliderOffsets: new Vector2(-2, -2)
                        );
            EmmisiveBeams emission = projectile.gameObject.GetOrAddComponent<EmmisiveBeams>();
            emission.EmissivePower = 100;
            emission.EmissiveColorPower = 100;
            emission.EmissiveColor = new Color(1f, 161f / 255f, 0f);

            projectile.gameObject.name = "Solstice Beam";
            projectile.baseData.damage = 20f;
            projectile.baseData.force = 20f;
            projectile.baseData.range = 100;
            projectile.baseData.speed = 60f;
            beamComp.boneType = BasicBeamController.BeamBoneType.Projectile;
            beamComp.endAudioEvent = "Stop_WPN_demonhead_loop_01";
            beamComp.startAudioEvent = "Play_WPN_demonhead_shot_01";
            BeamAutoAddMotionModule orbit = projectile.gameObject.AddComponent<BeamAutoAddMotionModule>();
            orbit.Orbit = true;
            orbit.beamOrbitRadius = 2f;


            gun.Volley.projectiles[0].projectiles[0] = projectile;



            gun.Volley.projectiles[1].angleVariance = 0;
            gun.Volley.projectiles[1].ammoCost = 0;
            gun.Volley.projectiles[1].shootStyle = ProjectileModule.ShootStyle.Beam;
            gun.Volley.projectiles[1].sequenceStyle = ProjectileModule.ProjectileSequenceStyle.Random;
            gun.Volley.projectiles[1].cooldownTime = 0.001f;
            gun.Volley.projectiles[1].numberOfShotsInClip = 400;
            Projectile small = ProjectileUtility.SetupProjectile(86);
            BasicBeamController beamsmall = small.GenerateAnchoredBeamPrefabBundle(
                        "solsticebeam2_mid_001",
                        Initialisation.ProjectileCollection,
                        Initialisation.projectileAnimationCollection,
                        $"SolsticeBeam2_Mid",
                        new Vector2(16, 4),
                        new Vector2(0, -2),
                        impactVFXAnimationName: "SolsticeBeam2_Impact",
                        impactVFXColliderDimensions: new Vector2(4, 4),
                        impactVFXColliderOffsets: new Vector2(-2, -2)
                        );
            EmmisiveBeams emission2 = projectile.gameObject.GetOrAddComponent<EmmisiveBeams>();
            emission2.EmissivePower = 40;
            emission2.EmissiveColorPower = 40;
            emission2.EmissiveColor = new Color(1f, 236f / 255f, 94f / 255f);

            small.gameObject.name = "Solstice Beam Small";
            small.baseData.damage = 10f;
            small.baseData.force = 10f;
            small.baseData.range = 100;
            small.baseData.speed = 60f;
            beamsmall.boneType = BasicBeamController.BeamBoneType.Projectile;
            gun.Volley.projectiles[1].projectiles[0] = small;

            //GUN STATS
            gun.gunScreenShake = (PickupObjectDatabase.GetById(60) as Gun).gunScreenShake;
            gun.reloadTime = 1.25f;
            gun.muzzleFlashEffects.type = VFXPoolType.None;
            gun.SetBarrel(59, 29);
            gun.SetBaseMaxAmmo(800);
            gun.ammo = 800;

            gun.DefaultModule.ammoType = GameUIAmmoType.AmmoType.CUSTOM;
            gun.DefaultModule.customAmmoType = "Y-Beam Laser";


            gun.gunHandedness = GunHandedness.TwoHanded;

            gun.quality = PickupObject.ItemQuality.B;
            ETGMod.Databases.Items.Add(gun, false, "ANY");

        }
        public override void OnReload(PlayerController player, Gun gun)
        {
            Exploder.DoRadialKnockback(player.CenterPosition, 30, 5);
            if (player != null && player.CurrentRoom != null)
            {
                List<AIActor> activeEnemies = player.CurrentRoom.GetActiveEnemies(RoomHandler.ActiveEnemyType.All);

                if (activeEnemies != null)
                {
                    for (int i = 0; i < activeEnemies.Count; i++)
                    {
                        AIActor aiactor = activeEnemies[i];
                        if (aiactor.IsNormalEnemy && Vector2.Distance(aiactor.CenterPosition, player.CenterPosition) <= 4f)
                        {
                            aiactor.gameActor.ApplyEffect(StaticStatusEffects.SunlightBurn, 1f, null);
                        }
                    }
                }
                base.OnReload(player, gun);
            }
        }
    }
}