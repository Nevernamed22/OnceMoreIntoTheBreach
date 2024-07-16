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
    public class Protean : AdvancedGunBehavior
    {
        public static void Add()
        {
            Gun gun = ETGMod.Databases.Items.NewGun("Protean", "protean");
            Game.Items.Rename("outdated_gun_mods:protean", "nn:protean");
            gun.gameObject.AddComponent<Protean>();
            gun.SetShortDescription("Inexplicable");
            gun.SetLongDescription("These loosely bound motes of gun floated into existence through a tear in the curtain eons before the Great Bullet struck."+"\n\nIt's fractal bullets carve hauntingly beautiful patterns in the air, at least for those who can understand them.");

            gun.SetGunSprites("protean");

            gun.SetAnimationFPS(gun.shootAnimation, 12);
            gun.SetAnimationFPS(gun.idleAnimation, 12);
            gun.gunSwitchGroup = (PickupObjectDatabase.GetById(479) as Gun).gunSwitchGroup;

            gun.AddProjectileModuleFrom(PickupObjectDatabase.GetById(86) as Gun, true, false);

            //GUN STATS
            gun.DefaultModule.ammoCost = 1;
            gun.DefaultModule.shootStyle = ProjectileModule.ShootStyle.SemiAutomatic;
            gun.DefaultModule.sequenceStyle = ProjectileModule.ProjectileSequenceStyle.Random;
            gun.reloadTime = 0.8f;
            gun.DefaultModule.cooldownTime = 0.4f;
            gun.muzzleFlashEffects = (PickupObjectDatabase.GetById(97) as Gun).muzzleFlashEffects;
            gun.DefaultModule.numberOfShotsInClip = 6;
            gun.barrelOffset.transform.localPosition = new Vector3(1.56f, 0.62f, 0f);
            gun.SetBaseMaxAmmo(200);
            gun.gunClass = GunClass.PISTOL;
            //BULLET STATS
            Projectile projectile = UnityEngine.Object.Instantiate<Projectile>(gun.DefaultModule.projectiles[0]);
            projectile.gameObject.SetActive(false);
            FakePrefab.MarkAsFakePrefab(projectile.gameObject);
            UnityEngine.Object.DontDestroyOnLoad(projectile);
            gun.DefaultModule.projectiles[0] = projectile;
            projectile.baseData.damage *= 3f;
            projectile.baseData.speed *= 0.7f;
            projectile.baseData.range *= 10f;
            BounceProjModifier Bouncing = projectile.gameObject.GetOrAddComponent<BounceProjModifier>();
            Bouncing.numberOfBounces = 5;
            RepeatedRandomReAim erraticmovement = projectile.gameObject.AddComponent<RepeatedRandomReAim>();

            projectile.pierceMinorBreakables = true;

            projectile.AnimateProjectileBundle("ProteanProjectile",
                   Initialisation.ProjectileCollection,
                   Initialisation.projectileAnimationCollection,
                   "ProteanProjectile",
                   MiscTools.DupeList(new IntVector2(9, 9), 8), //Pixel Sizes
                   MiscTools.DupeList(true, 8), //Lightened
                   MiscTools.DupeList(tk2dBaseSprite.Anchor.MiddleCenter, 8), //Anchors
                   MiscTools.DupeList(true, 8), //Anchors Change Colliders
                   MiscTools.DupeList(false, 8), //Fixes Scales
                   MiscTools.DupeList<Vector3?>(null, 8), //Manual Offsets
                   MiscTools.DupeList<IntVector2?>(null, 8), //Override colliders
                   MiscTools.DupeList<IntVector2?>(null, 8), //Override collider offsets
                   MiscTools.DupeList<Projectile>(null, 8)); // Override to copy from    

            gun.quality = PickupObject.ItemQuality.C;
            ETGMod.Databases.Items.Add(gun, null, "ANY");

            ProteanID = gun.PickupObjectId;
        }
        public static int ProteanID;
        public Protean()
        {

        }
    }
    public class RepeatedRandomReAim : MonoBehaviour
    {
        public RepeatedRandomReAim()
        {
        }
        private void Start()
        {
            this.m_projectile = base.GetComponent<Projectile>();
            StartCoroutine(DoReAim());
        }
        private IEnumerator DoReAim()
        {
            while (m_projectile != null)
            {
                yield return new WaitForSeconds(0.2f);
                m_projectile.SendInRandomDirection();
            }
        }

        private Projectile m_projectile;
    }
}