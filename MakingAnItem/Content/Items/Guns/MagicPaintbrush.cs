using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using Gungeon;
using MonoMod;
using UnityEngine;
using Alexandria.ItemAPI;
using MonoMod.RuntimeDetour;
using System.Reflection;
using SaveAPI;
using Alexandria.Misc;

namespace NevernamedsItems
{
    public class MagicPaintbrush : GunBehaviour
    {
        public static void Add()
        {
            Gun gun = ETGMod.Databases.Items.NewGun("Magic Paintbrush", "magicpaintbrush");
            Game.Items.Rename("outdated_gun_mods:magic_paintbrush", "nn:magic_paintbrush");
            gun.gameObject.AddComponent<MagicPaintbrush>();
            gun.SetShortDescription("Scrybe of Magicks");
            gun.SetLongDescription("Fires multicoloured magical gems which explode into bursts of paint on passing through Gundead.\n\nNothing beautiful can last.");

            gun.SetGunSprites("magicpaintbrush", 8, false, 2);

            gun.SetAnimationFPS(gun.shootAnimation, 12);
            gun.SetAnimationFPS(gun.reloadAnimation, 14);

            gun.gunSwitchGroup = (PickupObjectDatabase.GetById(145) as Gun).gunSwitchGroup;

            gun.AddProjectileModuleFrom(PickupObjectDatabase.GetById(86) as Gun, true, false);

            gun.gunHandedness = GunHandedness.OneHanded;
            gun.muzzleFlashEffects = (PickupObjectDatabase.GetById(145) as Gun).muzzleFlashEffects;

            //GUN STATS
            gun.DefaultModule.ammoCost = 1;
            gun.DefaultModule.shootStyle = ProjectileModule.ShootStyle.Automatic;
            gun.DefaultModule.sequenceStyle = ProjectileModule.ProjectileSequenceStyle.Random;
            gun.reloadTime = 1f;
            gun.DefaultModule.cooldownTime = 0.5f;
            gun.DefaultModule.numberOfShotsInClip = 5;
            gun.SetBarrel(36, 17);
            gun.SetBaseMaxAmmo(100);
            gun.ammo = 200;
            gun.gunClass = GunClass.SILLY;

            //BULLET STATS

            Color RubyColour =  new Color(246 / 255f, 129 / 255f, 37 / 255f);
            Color SapphireColour = new Color(63 / 255f, 162 / 255f, 115 / 255f);
            Color EmeraldColour = new Color(156 / 255f, 191 / 255f, 85 / 255f);

            Projectile RubyMox = ProjectileSetupUtility.MakeProjectile(86, 12f);
            RubyMox.SetProjectileSprite("magicpaintbrush_proj_ruby", 7, 7, false, tk2dBaseSprite.Anchor.MiddleCenter, 7, 7);
            RubyMox.gameObject.AddComponent<ProjectileSpriteRotation>().RotPerFrame = 2f;
            EasyTrailBullet rubyTrail = RubyMox.gameObject.AddComponent<EasyTrailBullet>();
            rubyTrail.TrailPos = RubyMox.transform.position;
            rubyTrail.StartWidth = 0.1875f;
            rubyTrail.EndWidth = 0f;
            rubyTrail.LifeTime = 0.1f;
            rubyTrail.BaseColor = RubyColour;
            rubyTrail.EndColor = RubyColour;
            rubyTrail.StartColor = RubyColour;
            RubyMox.hitEffects = (PickupObjectDatabase.GetById(Bejeweler.ID) as Gun).DefaultModule.projectiles[2].hitEffects;

            Projectile OrangePaint = ProjectileSetupUtility.MakeProjectile(86, 2.5f, -1, 3);
            OrangePaint.SetProjectileSprite("magicpaintbrush_subproj_orange", 5, 5, false, tk2dBaseSprite.Anchor.MiddleCenter, 5, 5);
            OrangePaint.gameObject.AddComponent<FungoRandomBullets>().HunterSporeChance = -1;
            EasyTrailBullet orangePaintTrail = OrangePaint.gameObject.AddComponent<EasyTrailBullet>();
            orangePaintTrail.TrailPos = RubyMox.transform.position;
            orangePaintTrail.StartWidth = 0.1875f;
            orangePaintTrail.EndWidth = 0f;
            orangePaintTrail.LifeTime = 0.3f;
            orangePaintTrail.BaseColor = RubyColour;
            orangePaintTrail.EndColor = RubyColour;
            orangePaintTrail.StartColor = RubyColour;
            OrangePaint.hitEffects.overrideMidairDeathVFX = SharedVFX.ColouredPoofOrange;
            OrangePaint.hitEffects.alwaysUseMidair = true;

            RubyMox.gameObject.AddComponent<PierceProjModifier>().penetration = 3;
            RubyMox.gameObject.AddComponent<PaintbrushProjectile>().paint = OrangePaint;

            Projectile SapphireMox = ProjectileSetupUtility.MakeProjectile(86, 12f);
            SapphireMox.SetProjectileSprite("magicpaintbrush_proj_sapphire", 7, 7, false, tk2dBaseSprite.Anchor.MiddleCenter, 7, 7);
            SapphireMox.gameObject.AddComponent<ProjectileSpriteRotation>().RotPerFrame = 2f;
            EasyTrailBullet sapphireTrail = SapphireMox.gameObject.AddComponent<EasyTrailBullet>();
            sapphireTrail.TrailPos = SapphireMox.transform.position;
            sapphireTrail.StartWidth = 0.1875f;
            sapphireTrail.EndWidth = 0f;
            sapphireTrail.LifeTime = 0.1f;
            sapphireTrail.BaseColor = SapphireColour;
            sapphireTrail.EndColor = SapphireColour;
            sapphireTrail.StartColor = SapphireColour;
            SapphireMox.hitEffects = (PickupObjectDatabase.GetById(Bejeweler.ID) as Gun).DefaultModule.projectiles[0].hitEffects;
            SapphireMox.hitEffects.alwaysUseMidair = true;

            Projectile BluePaint = ProjectileSetupUtility.MakeProjectile(86, 2.5f, -1, 3);
            BluePaint.SetProjectileSprite("magicpaintbrush_subproj_blue", 5, 5, false, tk2dBaseSprite.Anchor.MiddleCenter, 5, 5);
            BluePaint.gameObject.AddComponent<FungoRandomBullets>().HunterSporeChance = -1;
            EasyTrailBullet bluePaintTrail = BluePaint.gameObject.AddComponent<EasyTrailBullet>();
            bluePaintTrail.TrailPos = RubyMox.transform.position;
            bluePaintTrail.StartWidth = 0.1875f;
            bluePaintTrail.EndWidth = 0f;
            bluePaintTrail.LifeTime = 0.3f;
            bluePaintTrail.BaseColor = SapphireColour;
            bluePaintTrail.EndColor = SapphireColour;
            bluePaintTrail.StartColor = SapphireColour;
            BluePaint.hitEffects.overrideMidairDeathVFX = SharedVFX.ColouredPoofBlue;
            BluePaint.hitEffects.alwaysUseMidair = true;

            SapphireMox.gameObject.AddComponent<PierceProjModifier>().penetration = 3;
            SapphireMox.gameObject.AddComponent<PaintbrushProjectile>().paint = BluePaint;

            Projectile EmeraldMox = ProjectileSetupUtility.MakeProjectile(86, 12f);
            EmeraldMox.SetProjectileSprite("magicpaintbrush_proj_emerald", 7, 6, false, tk2dBaseSprite.Anchor.MiddleCenter, 7, 6);
            EmeraldMox.gameObject.AddComponent<ProjectileSpriteRotation>().RotPerFrame = 2f;
            EasyTrailBullet trail4 = EmeraldMox.gameObject.AddComponent<EasyTrailBullet>();
            trail4.TrailPos = EmeraldMox.transform.position;
            trail4.StartWidth = 0.1875f;
            trail4.EndWidth = 0f;
            trail4.LifeTime = 0.1f;
            trail4.BaseColor = EmeraldColour;
            trail4.EndColor = EmeraldColour;
            trail4.StartColor = EmeraldColour;
            EmeraldMox.hitEffects = (PickupObjectDatabase.GetById(Bejeweler.ID) as Gun).DefaultModule.projectiles[1].hitEffects;

            Projectile GreenPaint = ProjectileSetupUtility.MakeProjectile(86, 2.5f, -1, 3);
            GreenPaint.SetProjectileSprite("magicpaintbrush_subproj_green", 5, 5, false, tk2dBaseSprite.Anchor.MiddleCenter, 5, 5);
            GreenPaint.gameObject.AddComponent<FungoRandomBullets>().HunterSporeChance = -1;
            EasyTrailBullet greenPaintTrail = GreenPaint.gameObject.AddComponent<EasyTrailBullet>();
            greenPaintTrail.TrailPos = RubyMox.transform.position;
            greenPaintTrail.StartWidth = 0.1875f;
            greenPaintTrail.EndWidth = 0f;
            greenPaintTrail.LifeTime = 0.3f;
            greenPaintTrail.BaseColor = EmeraldColour;
            greenPaintTrail.EndColor = EmeraldColour;
            greenPaintTrail.StartColor = EmeraldColour;
            GreenPaint.hitEffects.overrideMidairDeathVFX = SharedVFX.ColouredPoofGreen;
            GreenPaint.hitEffects.alwaysUseMidair = true;

            EmeraldMox.gameObject.AddComponent<PierceProjModifier>().penetration = 3;
            EmeraldMox.gameObject.AddComponent<PaintbrushProjectile>().paint = GreenPaint;

            gun.DefaultModule.projectiles[0] = RubyMox;
            gun.DefaultModule.projectiles.Add(SapphireMox);
            gun.DefaultModule.projectiles.Add(EmeraldMox);

            gun.AddClipSprites("magicpaintbrush");

            gun.quality = PickupObject.ItemQuality.A;
            ETGMod.Databases.Items.Add(gun, false, "ANY");

            ID = gun.PickupObjectId;
        }
        public static int ID;

        private class PaintbrushProjectile : BraveBehaviour
        {
            public Projectile paint;
            private void Start()
            {
                if (base.projectile) { base.projectile.OnHitEnemy += OnHitenemy; }
            }
            private void OnHitenemy(Projectile proj, SpeculativeRigidbody body, bool fatal)
            {
                if (body && body.aiActor)
                {
                    base.StartCoroutine(HandleDamageCooldown(body.aiActor));
                    for(int i = 0; i < 12; i++)
                    {
                      Projectile paintinst=  paint.InstantiateAndFireInDirection(proj.SafeCenter, BraveUtility.RandomAngle(), 0, null).GetComponent<Projectile>();
                        paintinst.Owner = proj.Owner;
                        paintinst.Shooter = proj.Shooter;
                        if (paintinst.ProjectilePlayerOwner())
                        {
                            paintinst.ScaleByPlayerStats(proj.ProjectilePlayerOwner());
                        }
                        paintinst.specRigidbody.RegisterTemporaryCollisionException(body);
                        body.RegisterTemporaryCollisionException(paintinst.specRigidbody);
                    }
                }
            }

            private HashSet<AIActor> m_damagedEnemies = new HashSet<AIActor>();
            private IEnumerator HandleDamageCooldown(AIActor damagedTarget)
            {
                this.m_damagedEnemies.Add(damagedTarget);
                yield return new WaitForSeconds(0.5f);
                this.m_damagedEnemies.Remove(damagedTarget);
                yield break;
            }
        }
    }
    
}
