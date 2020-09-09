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

    public class GoodMimic : GunBehaviour
    {


        public static void Add()
        {
            // Get yourself a new gun "base" first.
            // Let's just call it "Basic Gun", and use "jpxfrd" for all sprites and as "codename" All sprites must begin with the same word as the codename. For example, your firing sprite would be named "jpxfrd_fire_001".
            Gun gun = ETGMod.Databases.Items.NewGun("Good Mimic", "goodmimic");
            // "kp:basic_gun determines how you spawn in your gun through the console. You can change this command to whatever you want, as long as it follows the "name:itemname" template.
            Game.Items.Rename("outdated_gun_mods:good_mimic", "nn:good_mimic");
            gun.gameObject.AddComponent<GoodMimic>();
            //These two lines determines the description of your gun, ".SetShortDescription" being the description that appears when you pick up the gun and ".SetLongDescription" being the description in the Ammonomicon entry. 
            gun.SetShortDescription("All Grown Up");
            gun.SetLongDescription("Unlike most, this mimic thirsts for adventure rather than blood." + "\n\nBest to be polite though, just in case.");
        
            gun.SetupSprite(null, "wailingmagnum_idle_001", 8);
            // ETGMod automatically checks which animations are available.
            // The numbers next to "shootAnimation" determine the animation fps. You can also tweak the animation fps of the reload animation and idle animation using this method.
            gun.SetAnimationFPS(gun.shootAnimation, 10);
            // Every modded gun has base projectile it works with that is borrowed from other guns in the game. 
            // The gun names are the names from the JSON dump! While most are the same, some guns named completely different things. If you need help finding gun names, ask a modder on the Gungeon discord.
            gun.AddProjectileModuleFrom(PickupObjectDatabase.GetById(38) as Gun, true, false);
            // Here we just take the default projectile module and change its settings how we want it to be.
            gun.DefaultModule.ammoCost = 1;
            gun.DefaultModule.shootStyle = ProjectileModule.ShootStyle.SemiAutomatic;
            gun.DefaultModule.sequenceStyle = ProjectileModule.ProjectileSequenceStyle.Random;
            gun.reloadTime = 1.1f;
            gun.DefaultModule.cooldownTime = 0.1f;
            gun.DefaultModule.numberOfShotsInClip = 6;
            gun.SetBaseMaxAmmo(300);
            // Here we just set the quality of the gun and the "EncounterGuid", which is used by Gungeon to identify the gun.
            gun.quality = PickupObject.ItemQuality.EXCLUDED; //C

            // Add the custom component here
            gun.gameObject.AddComponent<ChangeFormProcessor>();

            gun.encounterTrackable.EncounterGuid = "this is the Good Mimic";


            ETGMod.Databases.Items.Add(gun, null, "ANY");

        }      

        public override void OnPostFired(PlayerController player, Gun gun)
        {
            //This determines what sound you want to play when you fire a gun.
            //Sounds names are based on the Gungeon sound dump, which can be found at EnterTheGungeon/Etg_Data/StreamingAssets/Audio/GeneratedSoundBanks/Windows/sfx.txt
            gun.PreventNormalFireAudio = true;
            AkSoundEngine.PostEvent("Play_WPN_smileyrevolver_shot_01", gameObject);
        }
        public GoodMimic()
        {

        }
    }
    public class ChangeFormProcessor : MonoBehaviour, IGunInheritable
    {

        public ChangeFormProcessor()
        {

        }


        private bool m_hasAwoken;
        private Gun m_gun;
        private PlayerController m_playerOwner;

        private void Awake()
        {
            m_hasAwoken = true;
            if (!m_gun) { m_gun = GetComponent<Gun>(); }
            if (m_gun)
            {
                // OnInitializedWithOwner event gets used to handle adding changes to the gun's properties when picked up.
                m_gun.OnInitializedWithOwner = (Action<GameActor>)Delegate.Combine(m_gun.OnInitializedWithOwner, new Action<GameActor>(OnGunInitialized));
                m_gun.OnDropped = (Action)Delegate.Combine(m_gun.OnDropped, new Action(OnGunDroppedOrDestroyed));
                if (m_gun.CurrentOwner != null) { OnGunInitialized(m_gun.CurrentOwner); }
            }
        }

        private void Start() { }

        private void Update() { }

        private void OnGunInitialized(GameActor actor)
        {
            if (m_playerOwner != null) { OnGunDroppedOrDestroyed(); }
            if (actor == null) { return; }

            if (actor is PlayerController)
            {
                m_playerOwner = (actor as PlayerController);
                // This will add your PostProcessProjectile event to the gun.
                m_gun.PostProcessProjectile += PostProcessBullet;
            }
        }

        public void PostProcessBullet(Projectile projectile)
        {
            // Setup PreCollision event
            projectile.specRigidbody.OnPreRigidbodyCollision += OnPreRigidBodyCollision;
        }

        public void OnPreRigidBodyCollision(SpeculativeRigidbody myRigidbody, PixelCollider myPixelCollider, SpeculativeRigidbody otherRigidbody, PixelCollider otherPixelCollider)
        {
            // I added check for boss. Remove if you want to also try and give player gun boss uses. (thoguh not many bosses have a usable gun)
            if (otherRigidbody.GetComponent<AIActor>() && otherRigidbody.GetComponent<AIActor>().aiShooter && otherRigidbody.GetComponent<AIActor>().healthHaver.IsDead && !otherRigidbody.GetComponent<AIActor>().healthHaver.IsBoss)
            {
                AIActor TargetAIActor = otherRigidbody.GetComponent<AIActor>();

                // Setup your code to check for gun and call ChaneForme from here. TargetAIActor.aiShooter.CurrentGun is where to look for if the enemy has a gun ID you can use.
                // aiShooter.CurrentGun returns a Gun object so if it's not null you can directly use CurrentGun's value with ChangeForm.
                // Just change the ChangeForm method to use Gun as input var instead of int targetID if you want to directly pass a gun to it instead of a ID.

                if (TargetAIActor.aiShooter.CurrentGun)
                {
                    ChangeForme(TargetAIActor.aiShooter.CurrentGun);
                }

                return;
            }
        }

        // Changed to accept a Gun directly.
        // private void ChangeForme(int targetID) {
        private void ChangeForme(Gun targetGun)
        {
            // Gun targetGun = PickupObjectDatabase.GetById(targetID) as Gun;
            m_gun.TransformToTargetGun(targetGun);
        }
        public void MidGameSerialize(List<object> data, int dataIndex) { }

        public void MidGameDeserialize(List<object> data, ref int dataIndex)
        {

        }

        private void OnGunDroppedOrDestroyed()
        {
            if (m_playerOwner != null)
            {
                m_playerOwner = null;

            }
        }

        public void InheritData(Gun sourceGun)
        {
            if (sourceGun)
            {
                if (!m_hasAwoken) { m_gun = GetComponent<Gun>(); }
            }
        }

        private void OnDestroy() { OnGunDroppedOrDestroyed(); }
    }
}

