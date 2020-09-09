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

    public class NNBazooka : GunBehaviour
    {


        public static void Add()
        {
            Gun gun = ETGMod.Databases.Items.NewGun("Bazooka", "bazooka");
            Game.Items.Rename("outdated_gun_mods:bazooka", "nn:bazooka");
            gun.gameObject.AddComponent<NNBazooka>();
            gun.SetShortDescription("Boom Boom Boom Boom");
            gun.SetLongDescription("It takes a lunatic to be a legend."+"\n\nThis powerful explosive weapon has one major drawback; it is capable of damaging it's bearer. You'd think more bombs would do that, but the Gungeon forgives.");

            gun.SetupSprite(null, "bazooka_idle_001", 8);

            gun.SetAnimationFPS(gun.shootAnimation, 12);

            gun.AddProjectileModuleFrom(PickupObjectDatabase.GetById(39) as Gun, true, false);

            //GUN STATS
            gun.DefaultModule.ammoCost = 1;
            gun.DefaultModule.shootStyle = ProjectileModule.ShootStyle.SemiAutomatic;
            gun.DefaultModule.sequenceStyle = ProjectileModule.ProjectileSequenceStyle.Random;
            gun.reloadTime = 4f;
            gun.DefaultModule.cooldownTime = 2f;
            gun.muzzleFlashEffects.type = VFXPoolType.None;
            gun.DefaultModule.numberOfShotsInClip = 3;
            gun.barrelOffset.transform.localPosition = new Vector3(2.5f, 0.68f, 0f);
            gun.SetBaseMaxAmmo(100);

            //BULLET STATS
            Projectile projectile = UnityEngine.Object.Instantiate<Projectile>(gun.DefaultModule.projectiles[0]);
            projectile.gameObject.SetActive(false);
            FakePrefab.MarkAsFakePrefab(projectile.gameObject);
            UnityEngine.Object.DontDestroyOnLoad(projectile);
            gun.DefaultModule.projectiles[0] = projectile;
            projectile.baseData.damage *= 3f;
            projectile.baseData.speed *= 1.2f;
            projectile.ignoreDamageCaps = true;
            projectile.pierceMinorBreakables = true;
            FuckingExplodeYouCunt explodePleaseImBeggingYou = projectile.gameObject.AddComponent<FuckingExplodeYouCunt>();
            projectile.SetProjectileSpriteRight("bazooka_projectile", 26, 7, false, tk2dBaseSprite.Anchor.MiddleCenter, 26, 7);

            projectile.transform.parent = gun.barrelOffset;

            // Here we just set the quality of the gun and the "EncounterGuid", which is used by Gungeon to identify the gun.
            gun.quality = PickupObject.ItemQuality.B;
            gun.encounterTrackable.EncounterGuid = "this is the Bazooka";
            ETGMod.Databases.Items.Add(gun, null, "ANY");

        }
        
        public override void OnPostFired(PlayerController player, Gun gun)
        {
            //This determines what sound you want to play when you fire a gun.
            //Sounds names are based on the Gungeon sound dump, which can be found at EnterTheGungeon/Etg_Data/StreamingAssets/Audio/GeneratedSoundBanks/Windows/sfx.txt
            gun.PreventNormalFireAudio = true;
            AkSoundEngine.PostEvent("Play_WPN_plasmarifle_shot_01", gameObject);
        }


        //All that's left now is sprite stuff. 
        //Your sprites should be organized, like how you see in the mod folder. 
        //Every gun requires that you have a .json to match the sprites or else the gun won't spawn at all
        //.Json determines the hand sprites for your character. You can make a gun two handed by having both "SecondaryHand" and "PrimaryHand" in the .json file, which can be edited through Notepad or Visual Studios
        //By default this gun is a one-handed weapon
        //If you need a basic two handed .json. Just use the jpxfrd2.json.
        //And finally, don't forget to add your Gun to your ETGModule class!

        public NNBazooka()
        {

        }
    }
    public class FuckingExplodeYouCunt : MonoBehaviour
    {
        public FuckingExplodeYouCunt()
        {

        }
        private Projectile m_projectile;

        private void Awake()
        {
            this.m_projectile = base.GetComponent<Projectile>();
            m_projectile.OnDestruction += this.OnDestroy;
        }
        private void OnDestroy(Projectile projectile)
        {
            //ETGModConsole.Log("On Destroy was called");
            Exploder.DoDefaultExplosion(projectile.specRigidbody.UnitTopCenter, new Vector2(), null, false, CoreDamageTypes.None, true);
        }
        private void Update()
        {

        }
    }
}