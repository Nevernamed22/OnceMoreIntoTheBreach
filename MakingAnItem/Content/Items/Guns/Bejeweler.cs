using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using Gungeon;
using MonoMod;
using UnityEngine;
using Alexandria.Misc;
using Alexandria.ItemAPI;
using Dungeonator;

namespace NevernamedsItems
{

    public class Bejeweler : GunBehaviour
    {
        public static void Add()
        {
            Gun gun = ETGMod.Databases.Items.NewGun("Bejeweler", "bejeweler");
            Game.Items.Rename("outdated_gun_mods:bejeweler", "nn:bejeweler");
            gun.gameObject.AddComponent<Bejeweler>();
            gun.SetShortDescription("Gem Jam");
            gun.SetLongDescription("Stacks different coloured jewels on enemies. When three gems of the same colour exist in a room, they will be... erased."+
                "\n\nVolatile crystals like this are a rare but lucrative opportunity for wandering Minelets deep within the Gungeon.");

            gun.SetGunSprites("bejeweler");

            gun.gunSwitchGroup = (PickupObjectDatabase.GetById(199) as Gun).gunSwitchGroup;

            gun.SetAnimationFPS(gun.shootAnimation, 14);
            gun.SetAnimationFPS(gun.reloadAnimation, 11);
            gun.muzzleFlashEffects = (PickupObjectDatabase.GetById(97) as Gun).muzzleFlashEffects;

            gun.AddProjectileModuleFrom(PickupObjectDatabase.GetById(86) as Gun, true, false);

            //GUN STATS
            gun.DefaultModule.ammoCost = 1;
            gun.DefaultModule.shootStyle = ProjectileModule.ShootStyle.SemiAutomatic;
            gun.DefaultModule.sequenceStyle = ProjectileModule.ProjectileSequenceStyle.Random;
            gun.reloadTime = 1f;
            gun.DefaultModule.cooldownTime = 0.2f;
            gun.DefaultModule.numberOfShotsInClip = 6;
            gun.barrelOffset.transform.localPosition = new Vector3(28f / 16f, 16f / 16f, 0f);
            gun.SetBaseMaxAmmo(200);
            gun.ammo = 200;
            gun.gunClass = GunClass.PISTOL;

            foreach (string colour in new List<string>() { "blue", "green", "orange", "pink", "red", "white", "yellow" })
            {
                VFXPool pool = VFXToolbox.CreateVFXPool($"Bejeweler {colour} Impact",
               new List<string>()
               {
                    $"NevernamedsItems/Resources/MiscVFX/GunVFX/Bejeweler/bejeweler_projectilehitvfx_{colour}_001",
                    $"NevernamedsItems/Resources/MiscVFX/GunVFX/Bejeweler/bejeweler_projectilehitvfx_{colour}_002",
                    $"NevernamedsItems/Resources/MiscVFX/GunVFX/Bejeweler/bejeweler_projectilehitvfx_{colour}_003",
                    $"NevernamedsItems/Resources/MiscVFX/GunVFX/Bejeweler/bejeweler_projectilehitvfx_{colour}_004",
                    $"NevernamedsItems/Resources/MiscVFX/GunVFX/Bejeweler/bejeweler_projectilehitvfx_{colour}_005",
               },
               13, //FPS
               new IntVector2(42, 36), //Dimensions
               tk2dBaseSprite.Anchor.MiddleLeft, //Anchor
               false, //Uses a Z height off the ground
               0, //The Z height, if used
               false,
              VFXAlignment.VelocityAligned
                 );
                switch (colour)
                {
                    case "blue": hitEffectBlue = pool; break;
                    case "green": hitEffectGreen = pool; break;
                    case "orange": hitEffectOrange = pool; break;
                    case "pink": hitEffectPink = pool; break;
                    case "red": hitEffectRed = pool; break;
                    case "white": hitEffectWhite = pool; break;
                    case "yellow": hitEffectYellow = pool; break;
                }
            }

            //BULLET STATS
            Projectile bluegem = (PickupObjectDatabase.GetById(86) as Gun).DefaultModule.projectiles[0].InstantiateAndFakeprefab();
            bluegem.SetProjectileSprite("bejeweler_projectile_blue", 11, 11, false, tk2dBaseSprite.Anchor.MiddleCenter, 11, 11);
            bluegem.hitEffects.deathAny = hitEffectBlue;
            bluegem.hitEffects.HasProjectileDeathVFX = true;
            bluegem.gameObject.AddComponent<BejewelerStuckJewels>().colour = GemColour.BLUE;

            Projectile greengem = (PickupObjectDatabase.GetById(86) as Gun).DefaultModule.projectiles[0].InstantiateAndFakeprefab();
            greengem.SetProjectileSprite("bejeweler_projectile_green", 10, 9, false, tk2dBaseSprite.Anchor.MiddleCenter, 10, 9);
            greengem.hitEffects.deathAny = hitEffectGreen;
            greengem.hitEffects.HasProjectileDeathVFX = true;
            greengem.gameObject.AddComponent<BejewelerStuckJewels>().colour = GemColour.GREEN;

            Projectile orangegem = (PickupObjectDatabase.GetById(86) as Gun).DefaultModule.projectiles[0].InstantiateAndFakeprefab();
            orangegem.SetProjectileSprite("bejeweler_projectile_orange", 8, 8, false, tk2dBaseSprite.Anchor.MiddleCenter, 8, 8);
            orangegem.hitEffects.deathAny = hitEffectOrange;
            orangegem.hitEffects.HasProjectileDeathVFX = true;
            orangegem.gameObject.AddComponent<BejewelerStuckJewels>().colour = GemColour.ORANGE;

            Projectile pinkgem = (PickupObjectDatabase.GetById(86) as Gun).DefaultModule.projectiles[0].InstantiateAndFakeprefab();
            pinkgem.SetProjectileSprite("bejeweler_projectile_pink", 9, 10, false, tk2dBaseSprite.Anchor.MiddleCenter, 9, 10);
            pinkgem.hitEffects.deathAny = hitEffectPink;
            pinkgem.hitEffects.HasProjectileDeathVFX = true;
            pinkgem.gameObject.AddComponent<BejewelerStuckJewels>().colour = GemColour.PINK;

            Projectile redgem = (PickupObjectDatabase.GetById(86) as Gun).DefaultModule.projectiles[0].InstantiateAndFakeprefab();
            redgem.SetProjectileSprite("bejeweler_projectile_red", 9, 9, false, tk2dBaseSprite.Anchor.MiddleCenter, 9, 9);
            redgem.hitEffects.deathAny = hitEffectRed;
            redgem.hitEffects.HasProjectileDeathVFX = true;
            redgem.gameObject.AddComponent<BejewelerStuckJewels>().colour = GemColour.RED;

            Projectile whitegem = (PickupObjectDatabase.GetById(86) as Gun).DefaultModule.projectiles[0].InstantiateAndFakeprefab();
            whitegem.SetProjectileSprite("bejeweler_projectile_white", 10, 10, false, tk2dBaseSprite.Anchor.MiddleCenter, 10, 10);
            whitegem.hitEffects.deathAny = hitEffectWhite;
            whitegem.hitEffects.HasProjectileDeathVFX = true;
            whitegem.gameObject.AddComponent<BejewelerStuckJewels>().colour = GemColour.WHITE;

            Projectile yellowgem = (PickupObjectDatabase.GetById(86) as Gun).DefaultModule.projectiles[0].InstantiateAndFakeprefab();
            yellowgem.SetProjectileSprite("bejeweler_projectile_yellow", 10, 10, false, tk2dBaseSprite.Anchor.MiddleCenter, 10, 10);
            yellowgem.hitEffects.deathAny = hitEffectYellow;
            yellowgem.hitEffects.HasProjectileDeathVFX = true;
            yellowgem.gameObject.AddComponent<BejewelerStuckJewels>().colour = GemColour.YELLOW;

            gun.DefaultModule.projectiles[0] = bluegem;
            gun.DefaultModule.projectiles.AddRange(new List<Projectile>() { greengem, orangegem, pinkgem, redgem, whitegem, yellowgem });



            gun.DefaultModule.ammoType = GameUIAmmoType.AmmoType.CUSTOM;
            gun.DefaultModule.customAmmoType = CustomClipAmmoTypeToolbox.AddCustomAmmoType("Bejeweler Ammo", "NevernamedsItems/Resources/CustomGunAmmoTypes/bejeweler_clipfull", "NevernamedsItems/Resources/CustomGunAmmoTypes/bejeweler_clipempty");


            gun.quality = PickupObject.ItemQuality.A;
            ETGMod.Databases.Items.Add(gun, null, "ANY");
            ID = gun.PickupObjectId;

            stickyBlue = VFXToolbox.CreateVFX("BlueGemSticky", new List<string>() { "NevernamedsItems/Resources/MiscVFX/GunVFX/Bejeweler/bejeweler_projectile_blue" }, 0, new IntVector2(11, 11), tk2dBaseSprite.Anchor.MiddleCenter, true, 1, -1, null, tk2dSpriteAnimationClip.WrapMode.Loop, true);
            stickyGreen = VFXToolbox.CreateVFX("GreenGemSticky", new List<string>() { "NevernamedsItems/Resources/MiscVFX/GunVFX/Bejeweler/bejeweler_projectile_green" }, 0, new IntVector2(10, 9), tk2dBaseSprite.Anchor.MiddleCenter, true, 1, -1, null, tk2dSpriteAnimationClip.WrapMode.Loop, true);
            stickyOrange = VFXToolbox.CreateVFX("OrangeGemSticky", new List<string>() { "NevernamedsItems/Resources/MiscVFX/GunVFX/Bejeweler/bejeweler_projectile_orange" }, 0, new IntVector2(8, 8), tk2dBaseSprite.Anchor.MiddleCenter, true, 1, -1, null, tk2dSpriteAnimationClip.WrapMode.Loop, true);
            stickyPink = VFXToolbox.CreateVFX("PinkGemSticky", new List<string>() { "NevernamedsItems/Resources/MiscVFX/GunVFX/Bejeweler/bejeweler_projectile_pink" }, 0, new IntVector2(9, 10), tk2dBaseSprite.Anchor.MiddleCenter, true, 1, -1, null, tk2dSpriteAnimationClip.WrapMode.Loop, true);
            stickyRed = VFXToolbox.CreateVFX("RedGemSticky", new List<string>() { "NevernamedsItems/Resources/MiscVFX/GunVFX/Bejeweler/bejeweler_projectile_red" }, 0, new IntVector2(9, 9), tk2dBaseSprite.Anchor.MiddleCenter, true, 1, -1, null, tk2dSpriteAnimationClip.WrapMode.Loop, true);
            stickyWhite = VFXToolbox.CreateVFX("WhiteGemSticky", new List<string>() { "NevernamedsItems/Resources/MiscVFX/GunVFX/Bejeweler/bejeweler_projectile_white" }, 0, new IntVector2(10, 10), tk2dBaseSprite.Anchor.MiddleCenter, true, 1, -1, null, tk2dSpriteAnimationClip.WrapMode.Loop, true);
            stickyYellow = VFXToolbox.CreateVFX("YellowGemSticky", new List<string>() { "NevernamedsItems/Resources/MiscVFX/GunVFX/Bejeweler/bejeweler_projectile_yellow" }, 0, new IntVector2(10, 10), tk2dBaseSprite.Anchor.MiddleCenter, true, 1, -1, null, tk2dSpriteAnimationClip.WrapMode.Loop, true);

            foreach (GameObject obj in new List<GameObject>() { stickyBlue, stickyGreen, stickyOrange, stickyPink, stickyRed, stickyWhite, stickyYellow })
            {
                BuffVFXAnimator buffanimator = obj.AddComponent<BuffVFXAnimator>();
                buffanimator.animationStyle = BuffVFXAnimator.BuffAnimationStyle.TETRIS;
                buffanimator.AdditionalPierceDepth = 0;
            }

            cubeVFX = VFXToolbox.CreateVFX("BejewelerCube",
                new List<string>()
                {
                    "NevernamedsItems/Resources/MiscVFX/GunVFX/Bejeweler/bejeweler_projectile_cube_001",
                    "NevernamedsItems/Resources/MiscVFX/GunVFX/Bejeweler/bejeweler_projectile_cube_002",
                    "NevernamedsItems/Resources/MiscVFX/GunVFX/Bejeweler/bejeweler_projectile_cube_003",
                    "NevernamedsItems/Resources/MiscVFX/GunVFX/Bejeweler/bejeweler_projectile_cube_004",
                    "NevernamedsItems/Resources/MiscVFX/GunVFX/Bejeweler/bejeweler_projectile_cube_005",
                    "NevernamedsItems/Resources/MiscVFX/GunVFX/Bejeweler/bejeweler_projectile_cube_006",
                    "NevernamedsItems/Resources/MiscVFX/GunVFX/Bejeweler/bejeweler_projectile_cube_007",
                    "NevernamedsItems/Resources/MiscVFX/GunVFX/Bejeweler/bejeweler_projectile_cube_008",
                    "NevernamedsItems/Resources/MiscVFX/GunVFX/Bejeweler/bejeweler_projectile_cube_009",
                    "NevernamedsItems/Resources/MiscVFX/GunVFX/Bejeweler/bejeweler_projectile_cube_010",
                    "NevernamedsItems/Resources/MiscVFX/GunVFX/Bejeweler/bejeweler_projectile_cube_011",
                    "NevernamedsItems/Resources/MiscVFX/GunVFX/Bejeweler/bejeweler_projectile_cube_012",
                    "NevernamedsItems/Resources/MiscVFX/GunVFX/Bejeweler/bejeweler_projectile_cube_013",
                    "NevernamedsItems/Resources/MiscVFX/GunVFX/Bejeweler/bejeweler_projectile_cube_014",
                    "NevernamedsItems/Resources/MiscVFX/GunVFX/Bejeweler/bejeweler_projectile_cube_015",
                    "NevernamedsItems/Resources/MiscVFX/GunVFX/Bejeweler/bejeweler_projectile_cube_016",
                    "NevernamedsItems/Resources/MiscVFX/GunVFX/Bejeweler/bejeweler_projectile_cube_017",
                    "NevernamedsItems/Resources/MiscVFX/GunVFX/Bejeweler/bejeweler_projectile_cube_018",
                    "NevernamedsItems/Resources/MiscVFX/GunVFX/Bejeweler/bejeweler_projectile_cube_019",
                    "NevernamedsItems/Resources/MiscVFX/GunVFX/Bejeweler/bejeweler_projectile_cube_020",
                    "NevernamedsItems/Resources/MiscVFX/GunVFX/Bejeweler/bejeweler_projectile_cube_021",
                    "NevernamedsItems/Resources/MiscVFX/GunVFX/Bejeweler/bejeweler_projectile_cube_022",
                    "NevernamedsItems/Resources/MiscVFX/GunVFX/Bejeweler/bejeweler_projectile_cube_023",
                },
                12, new IntVector2(13, 26),
                tk2dBaseSprite.Anchor.LowerCenter,
                true, 1, -1, null,
                tk2dSpriteAnimationClip.WrapMode.LoopSection, true, 6);

            TenPoints = VFXToolbox.CreateVFX("Ten Points VFX",
                new List<string>() {
                    "NevernamedsItems/Resources/MiscVFX/tenpoints_001",
                    "NevernamedsItems/Resources/MiscVFX/tenpoints_002",
                    "NevernamedsItems/Resources/MiscVFX/tenpoints_003",
                    "NevernamedsItems/Resources/MiscVFX/tenpoints_004",
                    "NevernamedsItems/Resources/MiscVFX/tenpoints_005",
                    "NevernamedsItems/Resources/MiscVFX/tenpoints_006",
                    "NevernamedsItems/Resources/MiscVFX/tenpoints_007",
                    "NevernamedsItems/Resources/MiscVFX/tenpoints_008",
                    "NevernamedsItems/Resources/MiscVFX/tenpoints_009",
                    "NevernamedsItems/Resources/MiscVFX/tenpoints_010",
                    "NevernamedsItems/Resources/MiscVFX/tenpoints_011",
                    "NevernamedsItems/Resources/MiscVFX/tenpoints_012",
                    "NevernamedsItems/Resources/MiscVFX/tenpoints_013",
                },
                12, new IntVector2(35, 20),
                tk2dBaseSprite.Anchor.LowerCenter,
                true, 1, -1, null,
                tk2dSpriteAnimationClip.WrapMode.Once, false, 0);

            railgun = (PickupObjectDatabase.GetById(370) as Gun).DefaultModule.chargeProjectiles[1].Projectile.InstantiateAndFakeprefab();
            railgun.PenetratesInternalWalls = true;
            railgun.baseData.damage = 70f;
        }
        public static Projectile railgun;

        public static GameObject stickyBlue;
        public static GameObject stickyGreen;
        public static GameObject stickyOrange;
        public static GameObject stickyPink;
        public static GameObject stickyRed;
        public static GameObject stickyWhite;
        public static GameObject stickyYellow;

        public static VFXPool hitEffectBlue;
        public static VFXPool hitEffectGreen;
        public static VFXPool hitEffectOrange;
        public static VFXPool hitEffectPink;
        public static VFXPool hitEffectRed;
        public static VFXPool hitEffectWhite;
        public static VFXPool hitEffectYellow;

        public static GameObject cubeVFX;
        public static GameObject TenPoints;
        public enum GemColour
        {
            BLUE,
            GREEN,
            ORANGE,
            PINK,
            RED,
            WHITE,
            YELLOW
        }
        public static int ID;
    }
    public class BejewelerStuckJewels : AppliedEffectBase
    {
        public Bejeweler.GemColour colour;
        public PlayerController owner;
        public void Start()
        {
            if (base.GetComponent<Projectile>() && base.GetComponent<Projectile>().ProjectilePlayerOwner())
            {
                owner = base.GetComponent<Projectile>().ProjectilePlayerOwner();
            }
        }
        public override void Initialize(AppliedEffectBase source) { }
        public override void AddSelfToTarget(GameObject target)
        {
            if (target.GetComponent<HealthHaver>() == null || owner == null) { return; }
            Jeweled comp = target.GetOrAddComponent<Jeweled>();
            comp.AddGem(target.GetComponent<HealthHaver>().gameActor, colour);

            if (ThreeGemmedEnemiesinRadius(owner, target.GetComponent<SpeculativeRigidbody>(), colour))
            {
                GameManager.Instance.StartCoroutine(HandleCUBE(owner, colour));
            }
        }
        public static IEnumerator HandleCUBE(PlayerController user, Bejeweler.GemColour colour)
        {
            List<AIActor> ActorsToGun = new List<AIActor>();
            List<GameObject> GemsToCrack = new List<GameObject>();
            List<AIActor> activeEnemies = user.CurrentRoom.GetActiveEnemies(RoomHandler.ActiveEnemyType.All);
            if (activeEnemies != null)
            {
                for (int i = 0; i < activeEnemies.Count; i++)
                {
                    AIActor aiactor = activeEnemies[i];
                    if (aiactor && aiactor.GetComponent<Jeweled>())
                    {
                        foreach (Tuple<Bejeweler.GemColour, GameObject> tup in aiactor.GetComponent<Jeweled>().instantiatedGemVFX)
                        {
                            if (GemsToCrack.Count >= 3) { break; }
                            if (tup.First == colour && !GemsToCrack.Contains(tup.Second))
                            {
                                GemsToCrack.Add(tup.Second);
                                if (!ActorsToGun.Contains(aiactor)) { ActorsToGun.Add(aiactor); }
                            }
                        }
                    }
                }
            }
            foreach (AIActor actor in ActorsToGun)
            {
                Jeweled comp = actor.GetComponent<Jeweled>();
                if (comp)
                {
                    foreach (GameObject gem in GemsToCrack)
                    {
                        if (comp.instantiatedGemVFX.Exists(x => x.Second == gem)) comp.ShatterGem(gem);
                    }
                }
            }
            Vector2 positionForCube = GetCenteredPosition(user, ActorsToGun);
            GameObject cube = SpawnManager.SpawnVFX(Bejeweler.cubeVFX, positionForCube, Quaternion.identity);
            yield return new WaitForSeconds(UnityEngine.Random.Range(1f, 2f));

            foreach(AIActor actor in ActorsToGun)
            {
                if (actor != null && actor.healthHaver && actor.healthHaver.IsAlive)
                {
                    GameObject inst = Bejeweler.railgun.InstantiateAndFireTowardsPosition(positionForCube, actor.specRigidbody.UnitCenter, 0, 0);
                    Projectile instproj = inst.GetComponent<Projectile>();
                    if (instproj)
                    {
                        instproj.Owner = user;
                        instproj.Shooter = user.specRigidbody;
                        instproj.ScaleByPlayerStats(user);
                        user.DoPostProcessProjectile(instproj);
                    }
                }
            }
            //shoot

            UnityEngine.Object.Destroy(cube);
            SpawnManager.SpawnVFX(EasyVFXDatabase.ShittyElectricExplosion, positionForCube, Quaternion.identity);


            yield break;
        }
        public static Vector2 GetCenteredPosition(PlayerController relevantPlayer, List<AIActor> actors)
        {
            if (actors.Count == 1)
            {
                return Vector3.Lerp(actors[0].specRigidbody.UnitCenter, relevantPlayer.specRigidbody.UnitCenter, 0.25f);
            }
            else if (actors.Count == 2)
            {
                return Vector3.Lerp(actors[0].specRigidbody.UnitCenter, actors[1].specRigidbody.UnitCenter, 0.5f);
            }
            else if (actors.Count >= 3)
            {
                Vector3 center = new Vector3(0, 0, 0);
                float count = 0;
                foreach (AIActor actor in actors)
                {
                    center += new Vector3(actor.specRigidbody.UnitCenter.x, actor.specRigidbody.UnitCenter.y, 0);
                    count++;
                }
                return center / count;
            }
            return relevantPlayer.CenterPosition;
        }
        public bool ThreeGemmedEnemiesinRadius(PlayerController user, SpeculativeRigidbody center, Bejeweler.GemColour colourToCheck)
        {
            int numGemsOfColourFound = 0;
            List<AIActor> activeEnemies = user.CurrentRoom.GetActiveEnemies(RoomHandler.ActiveEnemyType.All);
            if (activeEnemies != null)
            {
                for (int i = 0; i < activeEnemies.Count; i++)
                {
                    AIActor aiactor = activeEnemies[i];
                    if (aiactor && aiactor.GetComponent<Jeweled>())
                    {
                        numGemsOfColourFound += aiactor.GetComponent<Jeweled>().instantiatedGemVFX.FindAll(x => x.First == colourToCheck).Count();
                    }
                }
            }
            return numGemsOfColourFound >= 3;
        }
    }
    public class Jeweled : MonoBehaviour
    {
        public HealthHaver hh;
        public Gun m_attachedGun;
        public PlayerController m_player;
        private Vector2 m_cachedSourceVector = Vector2.zero;
        private GameObject instantiatedVFX;
        public bool doPoisonPool;

        public List<Tuple<Bejeweler.GemColour, GameObject>> instantiatedGemVFX = new List<Tuple<Bejeweler.GemColour, GameObject>>();
        public Dictionary<Bejeweler.GemColour, GameObject> colourToVFX = new Dictionary<Bejeweler.GemColour, GameObject>()
        {
            { Bejeweler.GemColour.BLUE, Bejeweler.stickyBlue },
            { Bejeweler.GemColour.GREEN, Bejeweler.stickyGreen },
            { Bejeweler.GemColour.ORANGE, Bejeweler.stickyOrange },
            { Bejeweler.GemColour.PINK, Bejeweler.stickyPink },
            { Bejeweler.GemColour.RED, Bejeweler.stickyRed },
            { Bejeweler.GemColour.WHITE, Bejeweler.stickyWhite },
            { Bejeweler.GemColour.YELLOW, Bejeweler.stickyYellow }
        };
        public Dictionary<Bejeweler.GemColour, GameObject> colourToShatter = new Dictionary<Bejeweler.GemColour, GameObject>()
        {
            { Bejeweler.GemColour.BLUE, Bejeweler.hitEffectBlue.effects[0].effects[0].effect },
            { Bejeweler.GemColour.GREEN, Bejeweler.hitEffectGreen.effects[0].effects[0].effect },
            { Bejeweler.GemColour.ORANGE, Bejeweler.hitEffectOrange.effects[0].effects[0].effect },
            { Bejeweler.GemColour.PINK, Bejeweler.hitEffectPink.effects[0].effects[0].effect },
            { Bejeweler.GemColour.RED, Bejeweler.hitEffectRed.effects[0].effects[0].effect },
            { Bejeweler.GemColour.WHITE, Bejeweler.hitEffectWhite.effects[0].effects[0].effect },
            { Bejeweler.GemColour.YELLOW, Bejeweler.hitEffectYellow.effects[0].effects[0].effect }
        };

        public void AddGem(GameActor target, Bejeweler.GemColour colour)
        {
            if (hh == null) hh = base.GetComponent<HealthHaver>();

            instantiatedVFX = SpawnManager.SpawnVFX(colourToVFX[colour], base.transform.position, Quaternion.identity, true);
            tk2dSprite vfxSprite = this.instantiatedVFX.GetComponent<tk2dSprite>();
            tk2dSprite hostsprite = base.GetComponent<tk2dSprite>();
            if (vfxSprite != null && hostsprite != null)
            {
                hostsprite.AttachRenderer(vfxSprite);
                vfxSprite.HeightOffGround = 1f;
                vfxSprite.IsPerpendicular = true;
                vfxSprite.usesOverrideMaterial = true;
            }

            BuffVFXAnimator buffVFXAnimat = this.instantiatedVFX.GetComponent<BuffVFXAnimator>();
            if (buffVFXAnimat != null) { buffVFXAnimat.Initialize(base.GetComponent<GameActor>()); }

            instantiatedGemVFX.Add(new Tuple<Bejeweler.GemColour, GameObject>(colour, instantiatedVFX));
        }
        public void ShatterGem(Bejeweler.GemColour colour)
        {

            if (instantiatedGemVFX.Exists(x => x.First == colour))
            {
                Tuple<Bejeweler.GemColour, GameObject> both = instantiatedGemVFX.Find(x => x.First == colour);
                GameObject gem = both.Second;


                SpawnManager.SpawnVFX(Bejeweler.TenPoints, gem.GetComponent<tk2dSprite>().WorldCenter, Quaternion.identity);
                SpawnManager.SpawnVFX(colourToShatter[colour], gem.GetComponent<tk2dSprite>().WorldCenter, Quaternion.identity);

                UnityEngine.Object.Destroy(gem);
                instantiatedGemVFX.Remove(both);

            }

        }
        public void ShatterGem(GameObject gem)
        {
            if (instantiatedGemVFX.Exists(x => x.Second == gem))
            {
                Tuple<Bejeweler.GemColour, GameObject> both = instantiatedGemVFX.Find(x => x.Second == gem);
                SpawnManager.SpawnVFX(Bejeweler.TenPoints, gem.GetComponent<tk2dSprite>().WorldCenter, Quaternion.identity);
                SpawnManager.SpawnVFX(colourToShatter[both.First], gem.GetComponent<tk2dSprite>().WorldCenter, Quaternion.identity);
                UnityEngine.Object.Destroy(gem);
                instantiatedGemVFX.Remove(both);
            }
        }
    }
}

