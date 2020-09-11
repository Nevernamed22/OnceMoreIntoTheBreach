using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using Gungeon;
using MonoMod;
using UnityEngine;
using ItemAPI;

namespace NevernamedsItems
{

    public class RiskRifle : GunBehaviour
    {

        public static void Add()
        {
            Gun gun = ETGMod.Databases.Items.NewGun("Risk Rifle", "riskrifle");
            Game.Items.Rename("outdated_gun_mods:risk_rifle", "nn:risk_rifle");
            gun.gameObject.AddComponent<RiskRifle>();
            gun.SetShortDescription("Danger Zone");
            gun.SetLongDescription("A strong rifle with one notable drawback; it harms it's owner upon emptying a clip. Be very careful to reload before then."+"\n\n");

            gun.SetupSprite(null, "riskrifle_idle_001", 8);

            gun.SetAnimationFPS(gun.shootAnimation, 17);

            gun.AddProjectileModuleFrom(PickupObjectDatabase.GetById(56) as Gun, true, false);

            //GUN STATS
            gun.DefaultModule.ammoCost = 1;
            gun.DefaultModule.shootStyle = ProjectileModule.ShootStyle.Automatic;
            gun.DefaultModule.sequenceStyle = ProjectileModule.ProjectileSequenceStyle.Random;
            gun.reloadTime = 2f;
            gun.DefaultModule.cooldownTime = 0.04f;
            gun.DefaultModule.numberOfShotsInClip = 40;
            gun.barrelOffset.transform.localPosition = new Vector3(2.25f, 0.5f, 0f);
            gun.SetBaseMaxAmmo(400);
            gun.ammo = 400;

            //BULLET STATS
            Projectile projectile = UnityEngine.Object.Instantiate<Projectile>(gun.DefaultModule.projectiles[0]);
            projectile.gameObject.SetActive(false);
            FakePrefab.MarkAsFakePrefab(projectile.gameObject);
            UnityEngine.Object.DontDestroyOnLoad(projectile);
            gun.DefaultModule.projectiles[0] = projectile;
            projectile.baseData.damage *= 2f;
            projectile.SetProjectileSpriteRight("riskrifle_projectile", 6, 4, true, tk2dBaseSprite.Anchor.MiddleCenter, 6, 4);

            projectile.transform.parent = gun.barrelOffset;

            // Here we just set the quality of the gun and the "EncounterGuid", which is used by Gungeon to identify the gun.
            gun.quality = PickupObject.ItemQuality.C;
            gun.encounterTrackable.EncounterGuid = "this is the Risk Rifle";
            ETGMod.Databases.Items.Add(gun, null, "ANY");

        }
        public override void PostProcessProjectile(Projectile projectile)
        {
            PlayerController playerController = projectile.Owner as PlayerController;
            base.PostProcessProjectile(projectile);
        }
        public override void OnPostFired(PlayerController player, Gun gun)
        {
            if (gun.ClipShotsRemaining == 0)
            {
                if (!player.PlayerHasActiveSynergy("Zero Risk"))
                {
                player.healthHaver.ApplyDamage(0.5f, Vector2.zero, "Risky Business", CoreDamageTypes.None, DamageCategory.Normal, true, null, false);

                }

            }
        }


        //All that's left now is sprite stuff. 
        //Your sprites should be organized, like how you see in the mod folder. 
        //Every gun requires that you have a .json to match the sprites or else the gun won't spawn at all
        //.Json determines the hand sprites for your character. You can make a gun two handed by having both "SecondaryHand" and "PrimaryHand" in the .json file, which can be edited through Notepad or Visual Studios
        //By default this gun is a one-handed weapon
        //If you need a basic two handed .json. Just use the jpxfrd2.json.
        //And finally, don't forget to add your Gun to your ETGModule class!

        public RiskRifle()
        {

        }
    }
}

