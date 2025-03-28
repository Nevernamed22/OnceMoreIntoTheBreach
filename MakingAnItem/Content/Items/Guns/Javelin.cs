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
using SaveAPI;
using Dungeonator;

namespace NevernamedsItems
{
    public class Javelin : GunBehaviour
    {
        public static void Add()
        {
            Gun gun = ETGMod.Databases.Items.NewGun("Javelin", "javelin");
            Game.Items.Rename("outdated_gun_mods:javelin", "nn:javelin");
            gun.gameObject.AddComponent<Javelin>();
            gun.SetShortDescription("Tap To Stab");
            gun.SetLongDescription("A sharp stick.\n\nThings don't really have to be that complicated.");

            gun.SetGunSprites("javelin", 8, false, 2);

            ThrowAnim = gun.UpdateAnimation("throw", Initialisation.gunCollection2, true);
            gun.SetAnimationFPS(ThrowAnim, 12);

            gun.GetComponent<tk2dSpriteAnimator>().GetClipByName(gun.chargeAnimation).wrapMode = tk2dSpriteAnimationClip.WrapMode.LoopSection;
            gun.GetComponent<tk2dSpriteAnimator>().GetClipByName(gun.chargeAnimation).loopStart = 7;

            gun.AddProjectileModuleFrom(PickupObjectDatabase.GetById(56) as Gun, true, false);

            gun.gunSwitchGroup = (PickupObjectDatabase.GetById(8) as Gun).gunSwitchGroup;

            gun.AddPassiveStatModifier(PlayerStats.StatType.Curse, 1f, StatModifier.ModifyMethod.ADDITIVE);

            //GUN STATS
            gun.DefaultModule.ammoCost = 1;
            gun.DefaultModule.shootStyle = ProjectileModule.ShootStyle.Charged;
            gun.DefaultModule.sequenceStyle = ProjectileModule.ProjectileSequenceStyle.Random;
            gun.reloadTime = 1f;
            gun.DefaultModule.cooldownTime = 0.5f;
            gun.DefaultModule.numberOfShotsInClip = 1;
            gun.SetBarrel(82, 31);
            gun.SetBaseMaxAmmo(12);
            gun.gunClass = GunClass.CHARGE;



            //BULLET STATS
            Projectile stab = ProjectileSetupUtility.MakeProjectile(56, 15f);
            stab.gameObject.AddComponent<DrainClipBehav>().shotsToDrain = -1;
            stab.sprite.renderer.enabled = false;
            ProjectileSlashingBehaviour slash = stab.gameObject.AddComponent<ProjectileSlashingBehaviour>();
            slash.DestroyBaseAfterFirstSlash = true;
            slash.slashParameters = ScriptableObject.CreateInstance<SlashData>();
            slash.slashParameters.soundEvent = null;
            slash.slashParameters.projInteractMode = SlashDoer.ProjInteractMode.IGNORE;
            slash.SlashDamageUsesBaseProjectileDamage = true;
            slash.slashParameters.doVFX = true;
            slash.slashParameters.doHitVFX = true;
            slash.slashParameters.VFX = VFXToolbox.CreateVFXPoolBundle("JavelinSlash", false, 0, VFXAlignment.Fixed);
            slash.slashParameters.slashRange = 3f;
            slash.slashParameters.slashDegrees = 5f;
            slash.slashParameters.playerKnockbackForce = 0f;
            gun.DefaultModule.chargeProjectiles.Add(new ProjectileModule.ChargeProjectile()
            {
                ChargeTime = 0,
                Projectile = stab,
                UsedProperties = ProjectileModule.ChargeProjectileProperties.ammo,
                AmmoCost = 0
            });

            Projectile thrownJavelin = ProjectileSetupUtility.MakeProjectile(56, 25f);
            thrownJavelin.gameObject.name = "Thrown_Javelin";
            thrownJavelin.SetProjectileSprite("woodenjavelin_projectile", 38, 4, false, tk2dBaseSprite.Anchor.MiddleCenter, 36, 2);

            thrownJavelin.baseData.force *= 2f;
            thrownJavelin.baseData.speed *= 2f;

            GameObject JavelinSticky = VFXToolbox.CreateVFXBundle("JavelinSticky", new IntVector2(31, 2), tk2dBaseSprite.Anchor.MiddleLeft, true, 0.4f);
            BuffVFXAnimator buffanimator = JavelinSticky.gameObject.AddComponent<BuffVFXAnimator>();
            buffanimator.animationStyle = BuffVFXAnimator.BuffAnimationStyle.PIERCE;
            buffanimator.AdditionalPierceDepth = 0;

            SimpleStickInEnemyHandler sticker = thrownJavelin.gameObject.AddComponent<SimpleStickInEnemyHandler>();
            sticker.stickyToSpawn = JavelinSticky;

            thrownJavelin.hitEffects.deathTileMapHorizontal = VFXToolbox.CreateVFXPoolBundle("JavelinImpact", false, 0, persist: true);
            thrownJavelin.hitEffects.deathTileMapVertical = thrownJavelin.hitEffects.deathTileMapHorizontal;
            thrownJavelin.hitEffects.enemy = VFXToolbox.CreateBlankVFXPool(SharedVFX.BloodImpactVFX);
            thrownJavelin.hitEffects.HasProjectileDeathVFX = true;

            ProjWeaknessModifier weakness = thrownJavelin.gameObject.AddComponent<ProjWeaknessModifier>();
            weakness.chanceToApply = 1f;

            gun.DefaultModule.chargeProjectiles.Add(new ProjectileModule.ChargeProjectile()
            {
                ChargeTime = 0.75f,
                Projectile = thrownJavelin,
                UsedProperties = ProjectileModule.ChargeProjectileProperties.shootAnim,
                OverrideShootAnimation = ThrowAnim
            });

            gun.muzzleFlashEffects.type = VFXPoolType.None;
            gun.gunHandedness = GunHandedness.AutoDetect;

            gun.AddClipSprites("javelin");
            gun.CanAttackThroughObjects = true;
            Material mat = new Material(EnemyDatabase.GetOrLoadByName("GunNut").sprite.renderer.material);
            mat.mainTexture = gun.sprite.renderer.material.mainTexture;
            mat.SetColor("_EmissiveColor", new Color(1f, 248f / 255f, 204f / 255f)); //RGB value of the color you want glowing
            mat.SetFloat("_EmissiveColorPower", 200); // no idea tbh
            mat.SetFloat("_EmissivePower", 200); //brightness
            gun.sprite.renderer.material = mat;

            gun.quality = PickupObject.ItemQuality.D;
            ETGMod.Databases.Items.Add(gun, null, "ANY");

            ID = gun.PickupObjectId;
        }
        public override void PostProcessProjectile(Projectile projectile)
        {
            if (projectile && projectile.gameObject.name.Contains("Thrown_Javelin") && projectile.ProjectilePlayerOwner())
            {
                if (projectile.ProjectilePlayerOwner().PlayerHasActiveSynergy("Brown And Sticky"))
                {
                    ProjectileShootbackMod shootback = projectile.gameObject.AddComponent<ProjectileShootbackMod>();
                    shootback.prefabToFire = ((Gun)PickupObjectDatabase.GetById(14)).DefaultModule.projectiles[0];
                    shootback.shootBackOnTimer = true;
                    shootback.timebetweenShootbacks = 0.05f;
                }
                if (projectile.ProjectilePlayerOwner().PlayerHasActiveSynergy("Stick 2 It"))
                {
                    projectile.gameObject.AddComponent<GainAmmoOnHitEnemyModifier>().requireKill = true;
                }
            }
        }
        public override void AutoreloadOnEmptyClip(GameActor owner, Gun gun, ref bool autoreload)
        {
            autoreload = false;
        }
        public static int ID;
        public static string ThrowAnim;
    }
}

