using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using Gungeon;
using MonoMod;
using UnityEngine;

namespace NevernamedsItems
{

    public class TheLodger : GunBehaviour
    {


        public static void Add()
        {
            // Get yourself a new gun "base" first.
            // Let's just call it "Basic Gun", and use "jpxfrd" for all sprites and as "codename" All sprites must begin with the same word as the codename. For example, your firing sprite would be named "jpxfrd_fire_001".
            Gun gun = ETGMod.Databases.Items.NewGun("The Lodger", "lodger");
            // "kp:basic_gun determines how you spawn in your gun through the console. You can change this command to whatever you want, as long as it follows the "name:itemname" template.
            Game.Items.Rename("outdated_gun_mods:the_lodger", "nn:the_lodger");
            gun.gameObject.AddComponent<TheLodger>();
            //These two lines determines the description of your gun, ".SetShortDescription" being the description that appears when you pick up the gun and ".SetLongDescription" being the description in the Ammonomicon entry. 
            gun.SetShortDescription("Cherish What You Have");
            gun.SetLongDescription("Many Gungeoneers have a bad habit of turning their noses up at items they deem to be of poor quality, but the Lodger seeks to teach them a lesson in humility.");
            // This is required, unless you want to use the sprites of the base gun.
            // That, by default, is the pea shooter.
            // SetupSprite sets up the default gun sprite for the ammonomicon and the "gun get" popup.
            // WARNING: Add a copy of your default sprite to Ammonomicon Encounter Icon Collection!
            // That means, "sprites/Ammonomicon Encounter Icon Collection/defaultsprite.png" in your mod .zip. You can see an example of this with inside the mod folder.
            gun.SetupSprite(null, "lodger_idle_001", 8);
            // ETGMod automatically checks which animations are available.
            // The numbers next to "shootAnimation" determine the animation fps. You can also tweak the animation fps of the reload animation and idle animation using this method.
            gun.SetAnimationFPS(gun.shootAnimation, 10);
            // Every modded gun has base projectile it works with that is borrowed from other guns in the game. 
            // The gun names are the names from the JSON dump! While most are the same, some guns named completely different things. If you need help finding gun names, ask a modder on the Gungeon discord.
            gun.AddProjectileModuleFrom(PickupObjectDatabase.GetById(464) as Gun, true, false);
            // Here we just take the default projectile module and change its settings how we want it to be.
            gun.DefaultModule.ammoCost = 1;
            gun.DefaultModule.shootStyle = ProjectileModule.ShootStyle.SemiAutomatic;
            gun.DefaultModule.sequenceStyle = ProjectileModule.ProjectileSequenceStyle.Random;
            gun.reloadTime = 1.1f;
            gun.DefaultModule.cooldownTime = 0.3f;
            gun.DefaultModule.numberOfShotsInClip = 10;
            gun.SetBaseMaxAmmo(1924);
            // Here we just set the quality of the gun and the "EncounterGuid", which is used by Gungeon to identify the gun.
            gun.quality = PickupObject.ItemQuality.D;
            gun.encounterTrackable.EncounterGuid = "this is the Lodger";
            ETGMod.Databases.Items.Add(gun, null, "ANY");

        }
        // This determines what the projectile does when it fires.
        public override void PostProcessProjectile(Projectile projectile)
        {
            PlayerController playerController = this.gun.CurrentOwner as PlayerController;
            if (playerController == null)
                this.gun.ammo = this.gun.GetBaseMaxAmmo();
            projectile.baseData.damage *= 0.63f;
            float badStuffModifier = 1f;
            foreach (PassiveItem item in playerController.passiveItems)
            {
                if (badStuff.Contains(item.PickupObjectId))
                {
                    badStuffModifier += 0.1f;
                }
                else if (reallyBadStuff.Contains(item.PickupObjectId))
                {
                    badStuffModifier += 0.2f;
                }
                else if (item.PickupObjectId == 127)
                {
                    badStuffModifier += 0.05f;
                }
            }
            foreach (PlayerItem item in playerController.activeItems)
            {
                if (badStuff.Contains(item.PickupObjectId))
                {
                    badStuffModifier += 0.1f;
                }
                else if (reallyBadStuff.Contains(item.PickupObjectId))
                {
                    badStuffModifier += 0.2f;
                }
                else if (item.PickupObjectId == 409f)
                {
                    badStuffModifier += 1f;
                }
            }
            foreach (Gun gun in playerController.inventory.AllGuns)
            {
                if (badStuff.Contains(gun.PickupObjectId))
                {
                    badStuffModifier += 0.1f;
                }
                else if (reallyBadStuff.Contains(gun.PickupObjectId))
                {
                    badStuffModifier += 0.2f;
                }
            }
            projectile.baseData.damage *= badStuffModifier;
            projectile.AdjustPlayerProjectileTint(ExtendedColours.brown, 1, 0f);
            this.gun.DefaultModule.ammoCost = 1;
            base.PostProcessProjectile(projectile);
        }
        public static List<int> badStuff = new List<int>()
        {
            378, //Derringer
            122, //Blunderbuss
            440, //Ruby Bracelet
            63, //Medkit
            104, //Ration
            108, //Bomb
            109, //Ice Bomb
            234, //iBomb Companion App
            403, //Melted Rock
            462, //Smoke Bomb
            216, //Box
            205, //Poison Vial
            201, //Portable Turret
            448, //Boomerang
            447, //Shield of the Maiden
            521, //Chance Bullets
            488, //Ring of Chest Vampirism
            256, //Heavy Boots
            119, //Metronome
            432, //Jar of Bees
            306, //Escape Rope
            106, //Jetpack
            487, //Book of Chest Anatomy
            197, //Pea Shooter
            83, //Unfinished Gun
            79, //Makarov
            9, //Dueling Pistol
            10, //Mega Douser
            510, //JK-47
            383, //Flash Ray
            334, //Wind Up Gun
            3, //Screecher
            196, //Fossilised Gun
            26, //Nail Gun
            292, //Molotov Launcher
            340, //Lower Case R
            150, //T-Shirt Cannon
        };
        public static List<int> reallyBadStuff = new List<int>()
        {
            209, //Sense of Direction  
            460, //Chaff Grenade
            136, //C4
            66, //Proximity Mine
            308, //Cluster Mine
            132, //Ring of Miserly Protection
            31, //Klobbe
            202, //Sawed-Off
        };
        public override void OnPostFired(PlayerController player, Gun gun)
        {
            //This determines what sound you want to play when you fire a gun.
            //Sounds names are based on the Gungeon sound dump, which can be found at EnterTheGungeon/Etg_Data/StreamingAssets/Audio/GeneratedSoundBanks/Windows/sfx.txt
            gun.PreventNormalFireAudio = false;
            //AkSoundEngine.PostEvent("Play_WPN_smileyrevolver_shot_01", gameObject);
        }


        //All that's left now is sprite stuff. 
        //Your sprites should be organized, like how you see in the mod folder. 
        //Every gun requires that you have a .json to match the sprites or else the gun won't spawn at all
        //.Json determines the hand sprites for your character. You can make a gun two handed by having both "SecondaryHand" and "PrimaryHand" in the .json file, which can be edited through Notepad or Visual Studios
        //By default this gun is a one-handed weapon
        //If you need a basic two handed .json. Just use the jpxfrd2.json.
        //And finally, don't forget to add your Gun to your ETGModule class!

        public TheLodger()
        {

        }
    }
}
