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

namespace NevernamedsItems
{

    public class KillDevil : GunBehaviour
    {
        public static void Add()
        {
            Gun gun = ETGMod.Databases.Items.NewGun("Kill-Devil", "killdevil");
            Game.Items.Rename("outdated_gun_mods:killdevil", "nn:killdevil");
            gun.gameObject.AddComponent<KillDevil>();
            gun.SetShortDescription("Hellbrand");
            gun.SetLongDescription("Press reload on a full clip to alternate fire modes. Every part of this gun is designed from the ground up to obliterate the demons found deep within the Gungeon.\n\nFor anyone familiar with demon hunters, it should come as no surprise that it takes its name from Brandy.");

            gun.SetGunSprites("killdevil", 8, false, 2);

            gun.gunSwitchGroup = (PickupObjectDatabase.GetById(81) as Gun).gunSwitchGroup;

            for (int i = 0; i < 5; i++)
            {
                gun.AddProjectileModuleFrom(PickupObjectDatabase.GetById(86) as Gun, true, false);
            }
            gun.muzzleFlashEffects = (PickupObjectDatabase.GetById(45) as Gun).muzzleFlashEffects;

            //GUN STATS
            int j = 1;
            foreach (ProjectileModule mod in gun.Volley.projectiles)
            {
                mod.ammoCost = mod != gun.DefaultModule ? 0 : 1;
                mod.shootStyle = ProjectileModule.ShootStyle.SemiAutomatic;
                mod.sequenceStyle = ProjectileModule.ProjectileSequenceStyle.Random;
                mod.cooldownTime = 0.66f;
                mod.angleVariance = 33f;
                mod.numberOfShotsInClip = 3;
                Projectile projectile = ProjectileSetupUtility.MakeProjectile(86, 5f, 5 + j, 30);

                projectile.AnimateProjectileBundle("KillDevilProjectile",
                   Initialisation.ProjectileCollection,
                   Initialisation.projectileAnimationCollection,
                   "KillDevilProjectile",
                   MiscTools.DupeList(new IntVector2(9, 9), 4), //Pixel Sizes
                   MiscTools.DupeList(true, 4), //Lightened
                   MiscTools.DupeList(tk2dBaseSprite.Anchor.MiddleCenter, 4), //Anchors
                   MiscTools.DupeList(true, 4), //Anchors Change Colliders
                   MiscTools.DupeList(false, 4), //Fixes Scales
                   MiscTools.DupeList<Vector3?>(null, 4), //Manual Offsets
                   MiscTools.DupeList<IntVector2?>(new IntVector2(7, 7), 4), //Override colliders
                   MiscTools.DupeList<IntVector2?>(null, 4), //Override collider offsets
                   MiscTools.DupeList<Projectile>(null, 4)); // Override to copy from    

                CustomVFXTrail trail = projectile.gameObject.AddComponent<CustomVFXTrail>();
                trail.timeBetweenSpawns = 0.15f;
                trail.anchor = CustomVFXTrail.Anchor.Center;
                trail.VFX = VFXToolbox.CreateBlankVFXPool(VFXToolbox.CreateVFXBundle("TinyBluePoof", false, 0), true);
                trail.heightOffset = -1f;

                projectile.baseData.UsesCustomAccelerationCurve = true;
                projectile.baseData.AccelerationCurve = AnimationCurve.Linear(0, 0f, 1f, 1f);

                projectile.gameObject.name = "KillDevilProj";

                ExplosiveModifier expl = projectile.gameObject.GetOrAddComponent<ExplosiveModifier>();
                expl.doExplosion = true;
                expl.explosionData = DataCloners.CopyExplosionData(StaticExplosionDatas.explosiveRoundsExplosion);
                expl.explosionData.damageRadius = 2;
                expl.explosionData.damage = 15;
                expl.explosionData.effect = SharedVFX.KillDevilExplosion;
                expl.explosionData.pushRadius = 0.2f;
                projectile.hitEffects.overrideMidairDeathVFX = SharedVFX.BlueLaserCircleVFX;
                projectile.hitEffects.alwaysUseMidair = true;

                projectile.BlackPhantomDamageMultiplier = 3;

                mod.projectiles[0] = projectile;
                j++;
            }

            gun.AddShellCasing(1, 1, 0, 0, "shell_killdevil");
            gun.AddClipSprites("killdevil");

            gun.AddPassiveStatModifier(PlayerStats.StatType.Curse, 1f, StatModifier.ModifyMethod.ADDITIVE);

            gun.Volley.UsesShotgunStyleVelocityRandomizer = true;

            gun.reloadTime = 1.5f;
            gun.SetBarrel(55, 25);
            gun.SetBaseMaxAmmo(50);

            gun.gunClass = GunClass.EXPLOSIVE;
            gun.quality = PickupObject.ItemQuality.A;
            ETGMod.Databases.Items.Add(gun, null, "ANY");

            gun.AddToSubShop(ItemBuilder.ShopType.Cursula);
            ID = gun.PickupObjectId;

            SlashData = ScriptableObject.CreateInstance<SlashData>();
            SlashData.damage = 20;
            SlashData.damagesBreakables = true;
            SlashData.doHitVFX = true;
            SlashData.doVFX = false;
            SlashData.enemyKnockbackForce = 10;
            SlashData.jammedDamageMult = 3;
            SlashData.playerKnockbackForce = 0;
            SlashData.slashDegrees = 25;
            SlashData.slashRange = 1.6f;
            SlashData.soundEvent = "Play_WPN_blasphemy_shot_01";

            MineData = DataCloners.CopyExplosionData(StaticExplosionDatas.explosiveRoundsExplosion);
            MineData.damageRadius = 4;
            MineData.damage = 15;
            MineData.effect = SharedVFX.KillDevilExplosion;
        }
        public static int ID;
        public static SlashData SlashData;
        public static ExplosionData MineData;
        public bool MinefieldMode = false;
        public override void PostProcessProjectile(Projectile projectile)
        {
            if (MinefieldMode && projectile.gameObject.name.Contains("KillDevilProj"))
            {
                SlowDownOverTimeModifier slow = projectile.gameObject.AddComponent<SlowDownOverTimeModifier>();
                slow.timeToSlowOver = 0.75f;
                slow.targetSpeed = 0.01f;
                projectile.baseData.range = 1000;
                projectile.gameObject.AddComponent<DieWhenOwnerNotInRoom>();
                projectile.GetComponent<ExplosiveModifier>().explosionData = MineData;
                }
            base.PostProcessProjectile(projectile);
        }
        public override void OnReloadPressed(PlayerController player, Gun gun, bool manual)
        {
            if (player & gun & manual && gun.ClipShotsRemaining == gun.ClipCapacity)
            {
                MinefieldMode = !MinefieldMode;
                VFXToolbox.DoRisingStringFade(MinefieldMode ? "MINEFIELD MODE" : "BLAST MODE", player.specRigidbody.UnitTopCenter + new Vector2(0,1), new Color32(149, 197, 246, 255));
                AkSoundEngine.PostEvent("Play_OBJ_power_up_01", player.gameObject);
            }
            base.OnReloadPressed(player, gun, manual);
        }
        public override void OnReloadedPlayer(PlayerController owner, Gun gun)
        {
            SlashDoer.DoSwordSlash(gun.barrelOffset.position, gun.CurrentAngle, owner, SlashData);
            base.OnReloadedPlayer(owner, gun);
        }
        private IEnumerator Slash(PlayerController player, Gun gun)
        {
            yield return new WaitForSeconds(0.25f);
            SlashDoer.DoSwordSlash(gun.CasingLaunchPoint, gun.CurrentAngle, player, SlashData);
            yield break;
        }
    }
}