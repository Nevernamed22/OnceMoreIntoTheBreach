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
using Alexandria.Misc;
using Alexandria.Assetbundle;

namespace NevernamedsItems
{
    public class BigShot : AdvancedGunBehavior
    {
        public static void Add()
        {
            Gun gun = ETGMod.Databases.Items.NewGun("Big Shot", "bigshot");
            Game.Items.Rename("outdated_gun_mods:big_shot", "nn:big_shot");
         var behav =   gun.gameObject.AddComponent<BigShot>();
            behav.preventNormalFireAudio = true;
            behav.preventNormalReloadAudio = true;
            behav.overrideNormalReloadAudio = "Play_NowsYourChanceToBeABigShot";
            gun.SetShortDescription("Now's Your Chance!");
            gun.SetLongDescription("The sign4ture weap0n of a [ONCE IN A LIFETIME OPPORTUNITY] that came to the Gungeon [ON AN ALL EXPENSES PAID HOLIDAY] seeking $$fr33$$! KROMER." + "\n\nYou're fill3d with [Hyperlink Blocked.].");

            gun.SetGunSprites("bigshot");

            gun.SetAnimationFPS(gun.shootAnimation, 12);
            gun.gunSwitchGroup = (PickupObjectDatabase.GetById(519) as Gun).gunSwitchGroup;
            gun.AddProjectileModuleFrom(PickupObjectDatabase.GetById(86) as Gun, true, false);
            gun.GetComponent<tk2dSpriteAnimator>().GetClipByName(gun.reloadAnimation).wrapMode = tk2dSpriteAnimationClip.WrapMode.Loop;

            //GUN STATS
            gun.DefaultModule.ammoCost = 1;
            gun.DefaultModule.shootStyle = ProjectileModule.ShootStyle.Automatic;
            gun.DefaultModule.sequenceStyle = ProjectileModule.ProjectileSequenceStyle.Random;
            gun.reloadTime = 1.6f;
            gun.DefaultModule.cooldownTime = 0.45f;
            gun.DefaultModule.numberOfShotsInClip = 3;
            gun.barrelOffset.transform.localPosition = new Vector3(1.75f, 0.62f, 0f);
            gun.SetBaseMaxAmmo(100);
            gun.ammo = 100;
            gun.gunClass = GunClass.SILLY;

            //BULLET STATS
            Projectile projectile = UnityEngine.Object.Instantiate<Projectile>(gun.DefaultModule.projectiles[0]);
            projectile.gameObject.SetActive(false);
            FakePrefab.MarkAsFakePrefab(projectile.gameObject);
            UnityEngine.Object.DontDestroyOnLoad(projectile);
            gun.DefaultModule.projectiles[0] = projectile;
            projectile.baseData.damage = 29.99f;
            projectile.baseData.speed *= 0.7f;
            projectile.baseData.range *= 2f;
            //ANIMATE BULLET

            projectile.AnimateProjectileBundle("BigShotOrangeProjectile",
                   Initialisation.ProjectileCollection,
                   Initialisation.projectileAnimationCollection,
                   "BigShotOrangeProjectile",
                   MiscTools.DupeList(new IntVector2(17, 16), 3), //Pixel Sizes
                   MiscTools.DupeList(true, 3), //Lightened
                   MiscTools.DupeList(tk2dBaseSprite.Anchor.MiddleCenter, 3), //Anchors
                   MiscTools.DupeList(true, 3), //Anchors Change Colliders
                   MiscTools.DupeList(false, 3), //Fixes Scales
                   MiscTools.DupeList<Vector3?>(null, 3), //Manual Offsets
                   MiscTools.DupeList<IntVector2?>(null, 3), //Override colliders
                   MiscTools.DupeList<IntVector2?>(null, 3), //Override collider offsets
                   MiscTools.DupeList<Projectile>(null, 3)); // Override to copy from        

            projectile.gameObject.AddComponent<BigShotProjectileComp>();
            projectile.DestroyMode = Projectile.ProjectileDestroyMode.BecomeDebris;

            Projectile projectile2 = UnityEngine.Object.Instantiate<Projectile>((PickupObjectDatabase.GetById(86) as Gun).DefaultModule.projectiles[0]);
            projectile2.gameObject.SetActive(false);
            FakePrefab.MarkAsFakePrefab(projectile2.gameObject);
            UnityEngine.Object.DontDestroyOnLoad(projectile2);
            projectile2.baseData.damage = 29.99f;
            projectile2.baseData.speed *= 0.7f;
            projectile2.baseData.range *= 2f;

            //ANIMATE BULLET
            projectile2.AnimateProjectileBundle("BigShotPinkProjectile",
                   Initialisation.ProjectileCollection,
                   Initialisation.projectileAnimationCollection,
                   "BigShotPinkProjectile",
                   MiscTools.DupeList(new IntVector2(17, 15), 3), //Pixel Sizes
                   MiscTools.DupeList(true, 3), //Lightened
                   MiscTools.DupeList(tk2dBaseSprite.Anchor.MiddleCenter, 3), //Anchors
                   MiscTools.DupeList(true, 3), //Anchors Change Colliders
                   MiscTools.DupeList(false, 3), //Fixes Scales
                   MiscTools.DupeList<Vector3?>(null, 3), //Manual Offsets
                   MiscTools.DupeList<IntVector2?>(null, 3), //Override colliders
                   MiscTools.DupeList<IntVector2?>(null, 3), //Override collider offsets
                   MiscTools.DupeList<Projectile>(null, 3)); // Override to copy from    

            projectile2.DestroyMode = Projectile.ProjectileDestroyMode.BecomeDebris;
            projectile2.gameObject.AddComponent<BigShotProjectileComp>();
            gun.DefaultModule.projectiles.Add(projectile2);


            gun.quality = PickupObject.ItemQuality.B;
            ETGMod.Databases.Items.Add(gun, false, "ANY");

            BigShotID = gun.PickupObjectId;


            gun.DefaultModule.ammoType = GameUIAmmoType.AmmoType.CUSTOM;
            gun.DefaultModule.customAmmoType = CustomClipAmmoTypeToolbox.AddCustomAmmoType("Round King", "NevernamedsItems/Resources/CustomGunAmmoTypes/bigshot_clipfull", "NevernamedsItems/Resources/CustomGunAmmoTypes/bigshot_clipempty");



            //SPAMTON HEAD
            Projectile spamtonHead = UnityEngine.Object.Instantiate<Projectile>((PickupObjectDatabase.GetById(56) as Gun).DefaultModule.projectiles[0]);
            spamtonHead.gameObject.SetActive(false);
            FakePrefab.MarkAsFakePrefab(spamtonHead.gameObject);
            UnityEngine.Object.DontDestroyOnLoad(spamtonHead);
            spamtonHead.baseData.damage = 7f;
            spamtonHead.baseData.speed *= 1.2f;
            spamtonHead.hitEffects.alwaysUseMidair = true;
            spamtonHead.hitEffects.overrideMidairDeathVFX = SharedVFX.WhiteCircleVFX;

            spamtonHead.AnimateProjectileBundle("BigShotSpamtonHeadProjectile",
                   Initialisation.ProjectileCollection,
                   Initialisation.projectileAnimationCollection,
                   "BigShotSpamtonHeadProjectile",
                   new List<IntVector2> {
                        new IntVector2(10, 13), //1
                        new IntVector2(10, 11), //2
                   }, //Pixel Sizes
                   MiscTools.DupeList(true, 2), //Lightened
                   MiscTools.DupeList(tk2dBaseSprite.Anchor.MiddleCenter, 2), //Anchors
                   MiscTools.DupeList(true, 2), //Anchors Change Colliders
                   MiscTools.DupeList(false, 2), //Fixes Scales
                   MiscTools.DupeList<Vector3?>(null, 2), //Manual Offsets
                   MiscTools.DupeList<IntVector2?>(new IntVector2(5, 5), 2), //Override colliders
                   MiscTools.DupeList<IntVector2?>(null, 2), //Override collider offsets
                   MiscTools.DupeList<Projectile>(null, 2)); // Override to copy from 

            //PIPIS
            Projectile pipisProj = UnityEngine.Object.Instantiate<Projectile>((PickupObjectDatabase.GetById(86) as Gun).DefaultModule.projectiles[0]);
            pipisProj.gameObject.SetActive(false);
            FakePrefab.MarkAsFakePrefab(pipisProj.gameObject);
            UnityEngine.Object.DontDestroyOnLoad(pipisProj);
            pipisProj.baseData.damage = 29.99f;
            pipisProj.baseData.speed *= 0.7f;
            pipisProj.baseData.range *= 0.7f;

            pipisProj.AnimateProjectileBundle("BigShotPipisProjectile",
                   Initialisation.ProjectileCollection,
                   Initialisation.projectileAnimationCollection,
                   "BigShotPipisProjectile",
                   MiscTools.DupeList(new IntVector2(12, 12), 4), //Pixel Sizes
                   MiscTools.DupeList(true, 4), //Lightened
                   MiscTools.DupeList(tk2dBaseSprite.Anchor.MiddleCenter, 4), //Anchors
                   MiscTools.DupeList(true, 4), //Anchors Change Colliders
                   MiscTools.DupeList(false, 4), //Fixes Scales
                   MiscTools.DupeList<Vector3?>(null, 4), //Manual Offsets
                   MiscTools.DupeList<IntVector2?>(new IntVector2(9, 9), 4), //Override colliders
                   MiscTools.DupeList<IntVector2?>(null, 4), //Override collider offsets
                   MiscTools.DupeList<Projectile>(null, 4)); // Override to copy from    

            pipisProj.DestroyMode = Projectile.ProjectileDestroyMode.BecomeDebris;
            pipisProj.gameObject.AddComponent<BigShotProjectileComp>();
            SpawnProjModifier projMod = pipisProj.gameObject.AddComponent<SpawnProjModifier>();
            projMod.spawnOnObjectCollisions = true;
            projMod.spawnProjecitlesOnDieInAir = true;
            projMod.spawnProjectilesOnCollision = true;
            projMod.spawnProjectilesInFlight = false;
            projMod.collisionSpawnStyle = SpawnProjModifier.CollisionSpawnStyle.FLAK_BURST;
            projMod.alignToSurfaceNormal = true;
            projMod.numberToSpawnOnCollison = 5;
            projMod.projectileToSpawnOnCollision = spamtonHead;
            pipis = pipisProj;
        }
        public static Projectile pipis;
        public override Projectile OnPreFireProjectileModifier(Gun gun, Projectile projectile, ProjectileModule mod)
        {
            if (gun && gun.GunPlayerOwner() && gun.GunPlayerOwner().PlayerHasActiveSynergy("pipis"))
            {
                return pipis;
            }
            return base.OnPreFireProjectileModifier(gun, projectile, mod);
        }
        public override void OnPostFired(PlayerController player, Gun gun)
        {
            if (gun.ClipShotsRemaining <= 0)
            {
                AkSoundEngine.PostEvent("Play_BeABigShot", gun.gameObject);

            }
            else
            {
                AkSoundEngine.PostEvent("Play_BeABig", gun.gameObject);

            }
            base.OnPostFired(player, gun);
        }

        public static int BigShotID;
        public override void PostProcessProjectile(Projectile projectile)
        {
            if (projectile && projectile.ProjectilePlayerOwner())
            {
                if (projectile.ProjectilePlayerOwner().PlayerHasActiveSynergy("Hyperlink Blocked"))
                {
                    if (UnityEngine.Random.value <= 0.35f)
                    {
                        HungryProjectileModifier hungry = projectile.gameObject.AddComponent<HungryProjectileModifier>();
                        hungry.HungryRadius = 1.5f;
                        projectile.AdjustPlayerProjectileTint(ExtendedColours.purple, 1);
                    }
                }
                if (projectile.ProjectilePlayerOwner().PlayerHasActiveSynergy("BIGGEST SHOT"))
                {
                    projectile.RuntimeUpdateScale(2);
                    projectile.baseData.damage *= 1.25f;
                }
            }
            base.PostProcessProjectile(projectile);
        }
        public bool hasRequestedHeal;
        protected override void Update()
        {
            if (!hasRequestedHeal && gun && gun.GunPlayerOwner())
            {
                if (Input.GetKey(KeyCode.F1))
                {
                    AkSoundEngine.PostEvent("Play_OBJ_heart_heal_01", gun.GunPlayerOwner().gameObject);
                    gun.GunPlayerOwner().PlayEffectOnActor((PickupObjectDatabase.GetById(73).GetComponent<HealthPickup>().healVFX), Vector3.zero, true, false, false);
                    if (gun.GunPlayerOwner().ForceZeroHealthState)
                    {
                        gun.GunPlayerOwner().healthHaver.Armor += 1;
                    }
                    else
                    {
                        gun.GunPlayerOwner().healthHaver.ApplyHealing(1);
                    }
                    hasRequestedHeal = true;
                }
            }
            base.Update();
        }
        public BigShot()
        {

        }
    }
    public class BigShotProjectileComp : MonoBehaviour
    {
        public BigShotProjectileComp()
        {

        }
        private List<string> dialogue = new List<string>()
        {
            "NOW'S YOUR CHANCE TO BE A BIG SHOT",
            "LOW, LOW, PRICE!",
            "BIGGER AND BETTER THAN EVER",
            "All Alone On A Late Night?",
            "BIG SHOT!!!!!",
            "Hyperlink Blocked.",
            "FREE KROMER",
            "WELL HAVE I GOT A DEAL FOR YOU!!",
            "$VALUES$",
            "$$DEALS$",
            "$'CHEAP'",
            "$$49.998",
            "Press F1 for Help"
        };
        private void OnHitEnemy(Projectile bullet, SpeculativeRigidbody enemy, bool fatal)
        {
            if (bullet && enemy)
            {
                if (UnityEngine.Random.value <= 0.05f)
                {
                    VFXToolbox.DoStringSquirt(BraveUtility.RandomElement(dialogue), enemy.Position.GetPixelVector2(), ExtendedColours.honeyYellow);
                    if (bullet.ProjectilePlayerOwner() && bullet.ProjectilePlayerOwner().PlayerHasActiveSynergy("De4l 0f 4 Lif3tim3"))
                    {
                        LootEngine.SpawnCurrency(enemy.Position.GetPixelVector2(), UnityEngine.Random.Range(2, 5));
                    }
                }

            }
        }
        private void Start()
        {
            self = base.GetComponent<Projectile>();
            if (self)
            {
                self.OnHitEnemy += OnHitEnemy;
                int random = UnityEngine.Random.Range(1, 7);
                switch (random)
                {
                    case 1:
                        PierceProjModifier piercing = self.gameObject.GetOrAddComponent<PierceProjModifier>();
                        piercing.penetration += 2;
                        break;
                    case 2:
                        BounceProjModifier bouncing = self.gameObject.GetOrAddComponent<BounceProjModifier>();
                        bouncing.numberOfBounces += 2;
                        break;
                    case 3:
                        RemoteBulletsProjectileBehaviour remote = self.gameObject.GetOrAddComponent<RemoteBulletsProjectileBehaviour>();
                        break;
                    case 4:
                        AngryBulletsProjectileBehaviour angry = self.gameObject.GetOrAddComponent<AngryBulletsProjectileBehaviour>();
                        break;
                    case 5:
                        OrbitalBulletsBehaviour orbital = self.gameObject.gameObject.GetOrAddComponent<OrbitalBulletsBehaviour>();
                        break;
                    case 6:
                        HomingModifier homing = self.gameObject.gameObject.AddComponent<HomingModifier>(); homing.HomingRadius = 100f;
                        break;
                }
            }
        }
        private Projectile self;
    }
}
