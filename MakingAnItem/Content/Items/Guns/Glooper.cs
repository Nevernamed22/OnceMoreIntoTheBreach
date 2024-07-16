using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using Gungeon;
using MonoMod;
using UnityEngine;
using Alexandria.ItemAPI;
using System.Reflection;
using Alexandria.Assetbundle;

namespace NevernamedsItems
{
    public class Glooper : GunBehaviour
    {
        public static void Add()
        {
            Gun gun = ETGMod.Databases.Items.NewGun("Glooper", "glooper");
            Game.Items.Rename("outdated_gun_mods:glooper", "nn:glooper");
            gun.gameObject.AddComponent<Glooper>();
            gun.SetShortDescription("Slippery");
            gun.SetLongDescription("Made of strange soapy goo, this weapon slips out of your hands when reloaded.");

            gun.SetGunSprites("glooper");

            gun.gunSwitchGroup = (PickupObjectDatabase.GetById(599) as Gun).gunSwitchGroup;

            gun.SetAnimationFPS(gun.shootAnimation, 12);

            gun.AddProjectileModuleFrom(PickupObjectDatabase.GetById(86) as Gun, true, false);

            //GUN STATS
            gun.DefaultModule.ammoCost = 1;
            gun.DefaultModule.shootStyle = ProjectileModule.ShootStyle.SemiAutomatic;
            gun.DefaultModule.sequenceStyle = ProjectileModule.ProjectileSequenceStyle.Random;
            gun.reloadTime = 0f;
            gun.DefaultModule.angleVariance = 12;
            gun.DefaultModule.cooldownTime = 0.2f;
            gun.DefaultModule.numberOfShotsInClip = 10;
            gun.muzzleFlashEffects.type = VFXPoolType.None;
            gun.barrelOffset.transform.localPosition = new Vector3(1.06f, 0.93f, 0f);
            gun.SetBaseMaxAmmo(200);
            gun.ammo = 200;
            gun.gunClass = GunClass.SHITTY;
            //BULLET STATS
            Projectile projectile = UnityEngine.Object.Instantiate<Projectile>(gun.DefaultModule.projectiles[0]);
            projectile.gameObject.SetActive(false);
            FakePrefab.MarkAsFakePrefab(projectile.gameObject);
            UnityEngine.Object.DontDestroyOnLoad(projectile);
            projectile.baseData.damage = 10f;
            projectile.baseData.speed *= 1f;
            projectile.baseData.range *= 1f;
            projectile.baseData.force *= 2.5f;

            projectile.AnimateProjectileBundle("GlooperProjectile",
                   Initialisation.ProjectileCollection,
                   Initialisation.projectileAnimationCollection,
                   "GlooperProjectile",
                   new List<IntVector2> {
                        new IntVector2(7, 7), //1
                        new IntVector2(5, 9), //2            
                        new IntVector2(7, 7), //3
                        new IntVector2(9, 5), //4
                   }, //Pixel Sizes
                   MiscTools.DupeList(false, 4), //Lightened
                   MiscTools.DupeList(tk2dBaseSprite.Anchor.MiddleCenter, 4), //Anchors
                   MiscTools.DupeList(true, 4), //Anchors Change Colliders
                   MiscTools.DupeList(false, 4), //Fixes Scales
                   MiscTools.DupeList<Vector3?>(null, 4), //Manual Offsets
                   MiscTools.DupeList<IntVector2?>(null, 4), //Override colliders
                   MiscTools.DupeList<IntVector2?>(null, 4), //Override collider offsets
                   MiscTools.DupeList<Projectile>(null, 4)); // Override to copy from    

            gun.DefaultModule.projectiles[0] = projectile;

            gun.DefaultModule.ammoType = GameUIAmmoType.AmmoType.CUSTOM;
            gun.DefaultModule.customAmmoType = CustomClipAmmoTypeToolbox.AddCustomAmmoType("Glooper Ammo", "NevernamedsItems/Resources/CustomGunAmmoTypes/glooper_clipfull", "NevernamedsItems/Resources/CustomGunAmmoTypes/glooper_clipempty");


            gun.quality = PickupObject.ItemQuality.D;
            ETGMod.Databases.Items.Add(gun, null, "ANY");
            GlooperID = gun.PickupObjectId;
            gun.AddToSubShop(ItemBuilder.ShopType.Goopton);
        }
        public static int GlooperID;
        public override void OnReloadPressed(PlayerController player, Gun gun, bool bSOMETHING)
        {
            base.OnReloadPressed(player, gun, bSOMETHING);
            if (player.CurrentGun && player.CurrentGun.PickupObjectId == GlooperID)
            {
                ForceThrow(player);
            }
        }
        private void ForceThrow(PlayerController user)
        {
            user.CurrentGun.PrepGunForThrow();
            typeof(Gun).GetField("m_prepThrowTime", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(user.CurrentGun, 999);
            user.CurrentGun.CeaseAttack();
        }
        public Glooper()
        {

        }
    }
}
