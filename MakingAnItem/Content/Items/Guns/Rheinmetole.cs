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
using SaveAPI;

namespace NevernamedsItems
{

    public class Rheinmetole : GunBehaviour
    {
        public static void Add()
        {
            Gun gun = ETGMod.Databases.Items.NewGun("Rheinmetole", "rheinmetole");
            Game.Items.Rename("outdated_gun_mods:rheinmetole", "nn:rheinmetole");
            gun.gameObject.AddComponent<Rheinmetole>();
            gun.SetShortDescription("Auf wiedersehen");
            gun.SetLongDescription("This monster of a weapon is made out of exlusively 100% melted down and reforged gunmetal, so that it might absorb the fighting spirit of all the guns that came before it."+"\n\nIt is, quite frankly, excessive.");

            gun.SetGunSprites("rheinmetole");

            gun.SetAnimationFPS(gun.shootAnimation, 20);
            gun.SetAnimationFPS(gun.reloadAnimation, 0);
            gun.usesContinuousFireAnimation = true;
            gun.GetComponent<tk2dSpriteAnimator>().GetClipByName(gun.shootAnimation).wrapMode = tk2dSpriteAnimationClip.WrapMode.LoopSection;
            gun.GetComponent<tk2dSpriteAnimator>().GetClipByName(gun.shootAnimation).loopStart = 0;

            gun.AddProjectileModuleFrom(PickupObjectDatabase.GetById(86) as Gun, true, false);
            gun.gunSwitchGroup = (PickupObjectDatabase.GetById(153) as Gun).gunSwitchGroup;

            //GUN STATS
            gun.muzzleFlashEffects = (PickupObjectDatabase.GetById(157) as Gun).muzzleFlashEffects;
            gun.reloadTime = 2f;
            gun.barrelOffset.transform.localPosition = new Vector3(44f / 16f, 14f / 16f, 0f);
            gun.SetBaseMaxAmmo(1000);
            gun.gunClass = GunClass.FULLAUTO;

            gun.Volley.projectiles[0].ammoCost = 1;
            gun.Volley.projectiles[0].shootStyle = ProjectileModule.ShootStyle.Automatic;
            gun.Volley.projectiles[0].sequenceStyle = ProjectileModule.ProjectileSequenceStyle.Random;
            gun.Volley.projectiles[0].cooldownTime = 0.07f;
            gun.Volley.projectiles[0].numberOfShotsInClip = 100;
            gun.Volley.projectiles[0].positionOffset = new Vector3(0.48f, 0, 0);

            //BULLET STATS
            Projectile projectile = (PickupObjectDatabase.GetById(329) as Gun).DefaultModule.chargeProjectiles[1].Projectile;
            gun.Volley.projectiles[0].projectiles[0] = projectile;

            gun.quality = PickupObject.ItemQuality.S;
            ETGMod.Databases.Items.Add(gun, null, "ANY");

            gun.SetupUnlockOnCustomFlag(CustomDungeonFlags.PURCHASED_RHEINMETOLE, true);
            gun.AddItemToTrorcMetaShop(100);
            gun.AddToSubShop(ItemBuilder.ShopType.Trorc);
            ID = gun.PickupObjectId;
        }
        public static int ID;
        public override void PostProcessProjectile(Projectile projectile)
        {
            this.gun.DefaultModule.positionOffset.x *= -1;
            base.PostProcessProjectile(projectile);
        }
    }
}
