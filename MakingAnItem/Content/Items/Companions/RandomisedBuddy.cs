using System;
using System.Collections;
using System.Collections.Generic;
using Dungeonator;
using Gungeon;
using Alexandria.ItemAPI;
using Alexandria.Misc;
using UnityEngine;
using SaveAPI;

namespace NevernamedsItems
{
    public class RandomisedBuddyController : CompanionController
    {
        public static tk2dSpriteCollectionData RandomisedBuddyCollection;
        public static tk2dSpriteAnimation RandomCompanionAnimationCollection;
        public static GameObject prefab;
        public static readonly string guid = "randomisedBuddy_abiyykwgdidsi6cx373248873286";
        public static List<string> validForms = new List<string>()
        {
        /*Numbers*/ "8bomb",
        /*A*/   "amogus", "amogusded", "adventurer", "artifactmonger", "apachethunder",
        /*B*/    "baba", "blinky", "boo", "bunnybullet", "birdblue", "birdgreen", "birdpurple", "birdyellow", "blackstache", "blasthelmet", "blocknerghost", "brotheralbern", "bubbleblack", "bulletfrost", "bulletshadow", "bulletshockrounds",
        /*Buddy Syntax*/    "buddyalexandria", "buddyandromeda", "buddyantipus", "buddyarrowkin", "buddyaseprite", "buddybattery", "buddybee", "buddybello", "buddyblobulon", "buddyblueshotgun", "buddybookllet", "buddybubs", "buddybulletkin", "buddybulletplayer", "buddycompanioncube", "buddydisarmingpersonality", "buddyfairy", "buddyfallenbulletkin", "buddyflynt", "buddykyle", "buddymistake", "buddyoldred", "buddypetrock", "buddypig", "buddypoisonvial", "buddypooka", "buddyprismatism", "buddyredshotgun", "buddyrobot", "buddyrusty", "buddyshade", "buddyskeleton", "buddyskilotar", "buddyskullet", "buddysnipershell", "buddytentacle", "buddytim", "buddytnt", "buddytrorc", "buddywilson",
        /*C*/   "clickanddrag", "clyde", "crimsonchamber", "cuphead", "cuteghost", "carrot", "chocoboblack", "chocoboyellow", "clippy", "clone", "coolbullet","cooltist", "cosmonaut", "cubeflesh", "cubelead", "cubemountain", "cultist", "cultistgreenapple", "cultistorange", "cultistredapple", "cultiststrawberry", "claygod", "crappysword",
        /*D*/   "defaultman", "darumablue", "darumabrown", "darumared", "decoy", "decoyice", "decoypoison", "demonhead", "detex", "detplus", "ddebot1", "ddebot2", "drifloon",
        /*E*/   "elquilliam", "eyeball", "elevatoractionbadguy", "evilcryptbug", "evilcryptdemon", "evilcryptevillord", "evilcryptghost", "evilcryptsnake", "evilcryptspaceman", "evilcrypttreeman",
        /*F*/   "fygar", "froggy", "fuselier", "flemoid", "fredchexter", "freetvs", 
        /*G*/   "ghostsword", "gregtheeggbuddy", "grimora", "glocktorock", "gunslingking", "guongold", "guongreen", "guonwhite",
        /*H*/   "helious", "hitboxdraw", "hologramkin", "hazmat", "hazmatgasmask", "hungryhippo",
        /*I*/   "inky", "iris", "iceogre", "indianajones",
        /*J*/   "jellyblorb", "jetpackcat",
        /*K*/   "kingslime", "kernal",
        /*L*/   "leshy", "labtech", "lala", "lamey", "lameyalt", "lasthuman", "lichbullets", "lolo", "looplich", "lowpriest", "lonelywizard",
        /*M*/   "magnificus", "mainframe", "minesweepersmiley", "miru", "mrdonut", "mrmatt", "mrspacman", "magnet", "manny", "marinegray", "marinegreen", "marinered", "marineyellow", "marketdoor", "maskred", "mauser", "metroidbaby", "monk", "moogleff4", "mooglemodern", "moon", "mushroom1up", "mushroomlife", "mushroomred", "mushroomstar", "mushroomvarg", "meowitzerchievo", "mrmagnum",
        /*N*/   "nanomachine", "ninja", "notabotclassic", "notabot2000",
        /*O*/   "oldnevernamed","octorok", "oldblood", "oldknight", "owlskeleton",
        /*P*/   "p03", "pacman", "perfection", "pinky", "poglizard", "personalityfear", "personalityrage", "phanto", "phantogold", "piku", "plasmacube", "peglin",
        /*R*/   "randal", "revolvenant", "rippy", "rubberjester",
        /*S*/   "sansyskeleton", "scaredghost", "semibot", "settingsbutton", "slenderman", "stickman", "sue", "scientistchilds","scientistdukes", "sepulchergeist", "serenade", "skullred", "snitchbrick", "spaceboy", "spacehole", "spiketrap", "spindrone", "steambot", "stonehead", "sun", "skifreeyeti", "spoilsportrobot",
        /*T*/   "tcboutblue", "tcboutred", "technoblade", "textfilebuddy", "thedreamer", "thepointyone", "thomasthebullet", "turtlemelon", "terminator", "tootscraze", "toughguy", "theguyfromelevatoraction",
        /*U*/   "ufo",
        /*V*/   "voidcore", "vvvvvv",
        /*W*/   "wallmongerthing", "whatface", "witch", "wolfenclaw", "williethewizard", "wx78", "witchbot",
        /*Y*/   "yumyum"
        };
        public string currentForm = "amogus";
        public bool setUp = false;
        public static List<string> fireSoundEffects = new List<string>()
        {
            "Play_WPN_Gun_Shot_01","Play_WPN_Gun_Shot_02","Play_WPN_Gun_Shot_03","Play_WPN_Gun_Shot_04","Play_WPN_Gun_Shot_05", "Play_WPN_gunhand_shot_01", "Play_WPN_h4mmer_shot_01", "Play_WPN_iceogre_shot_01", "Play_WPN_knav3_shot_01", "Play_WPN_kthulu_blast_01", "Play_WPN_lamp_shot_01",
            "Play_WPN_looper_shot_01", "Play_WPN_m1911_shot_01", "Play_WPN_magnum_shot_01", "Play_WPN_mailbox_shot_01", "Play_WPN_makarov_shot_01", "Play_WPN_megablaster_shot_01", "Play_WPN_minigun_shot_01", "Play_WPN_peashooter_shot_01", "Play_WPN_rustysidearm_shot_01"
        };

        public static List<VFXPool> validMuzzleFlashes = new List<VFXPool>()
        {
            (PickupObjectDatabase.GetById(1) as Gun).muzzleFlashEffects,
            (PickupObjectDatabase.GetById(2) as Gun).muzzleFlashEffects,
            (PickupObjectDatabase.GetById(5) as Gun).muzzleFlashEffects,
            (PickupObjectDatabase.GetById(7) as Gun).muzzleFlashEffects,
            (PickupObjectDatabase.GetById(9) as Gun).muzzleFlashEffects,
            (PickupObjectDatabase.GetById(13) as Gun).muzzleFlashEffects,
            (PickupObjectDatabase.GetById(17) as Gun).muzzleFlashEffects,
            (PickupObjectDatabase.GetById(18) as Gun).muzzleFlashEffects,
            (PickupObjectDatabase.GetById(23) as Gun).muzzleFlashEffects,
            (PickupObjectDatabase.GetById(24) as Gun).muzzleFlashEffects,
            (PickupObjectDatabase.GetById(25) as Gun).muzzleFlashEffects,
            (PickupObjectDatabase.GetById(26) as Gun).muzzleFlashEffects,
            (PickupObjectDatabase.GetById(28) as Gun).muzzleFlashEffects,
            (PickupObjectDatabase.GetById(29) as Gun).muzzleFlashEffects,
            (PickupObjectDatabase.GetById(30) as Gun).muzzleFlashEffects,
            (PickupObjectDatabase.GetById(31) as Gun).muzzleFlashEffects,
            (PickupObjectDatabase.GetById(32) as Gun).muzzleFlashEffects,
            (PickupObjectDatabase.GetById(33) as Gun).muzzleFlashEffects,
            (PickupObjectDatabase.GetById(38) as Gun).muzzleFlashEffects,
            (PickupObjectDatabase.GetById(41) as Gun).muzzleFlashEffects,
            (PickupObjectDatabase.GetById(45) as Gun).muzzleFlashEffects,
            (PickupObjectDatabase.GetById(53) as Gun).muzzleFlashEffects,
            (PickupObjectDatabase.GetById(55) as Gun).muzzleFlashEffects,
            (PickupObjectDatabase.GetById(56) as Gun).muzzleFlashEffects,
            (PickupObjectDatabase.GetById(57) as Gun).muzzleFlashEffects,
            (PickupObjectDatabase.GetById(58) as Gun).muzzleFlashEffects,
            (PickupObjectDatabase.GetById(59) as Gun).muzzleFlashEffects,
            (PickupObjectDatabase.GetById(61) as Gun).muzzleFlashEffects,
            (PickupObjectDatabase.GetById(82) as Gun).muzzleFlashEffects,
            (PickupObjectDatabase.GetById(83) as Gun).muzzleFlashEffects,
            (PickupObjectDatabase.GetById(89) as Gun).muzzleFlashEffects,
            (PickupObjectDatabase.GetById(97) as Gun).muzzleFlashEffects,
        };
        public override void Update()
        {
            base.sprite.FlipX = base.aiActor.FacingDirection > 90 || base.aiActor.FacingDirection < -90;
            if (base.aiAnimator.IdleAnimation.AnimNames[0] != currentForm)
            {
                base.aiAnimator.IdleAnimation.AnimNames[0] = currentForm;
                base.aiAnimator.IdleAnimation.AnimNames[1] = currentForm;
            }
            if (base.aiActor.TargetRigidbody != null && Vector2.Distance(base.aiActor.TargetRigidbody.UnitCenter, base.specRigidbody.UnitCenter) <= 9)
            {
                if (timer <= 0)
                {
                    Fire();
                    shotsFired++;
                    if (shotsFired >= shotsInClip) { timer = reloadTime; shotsFired = 0; }
                    else { timer = cooldownTime; }
                }
                else
                {
                    timer -= BraveTime.DeltaTime;
                }
            }
            base.Update();
        }
        public void Fire()
        {
            Vector2 originPoint = BraveMathCollege.ClosestPointOnRectangle(base.aiActor.TargetRigidbody.UnitCenter, base.specRigidbody.UnitBottomLeft, base.specRigidbody.UnitDimensions);
            Vector2 direction = originPoint.CalculateVectorBetween(base.aiActor.TargetRigidbody.aiActor != null ? base.aiActor.TargetRigidbody.aiActor.CenterPosition : base.aiActor.TargetRigidbody.UnitCenter);
            float angle = direction.ToAngle();
            if (MuzzleFlash != null) { MuzzleFlash.SpawnAtPosition(originPoint, angle, base.transform, null, null); }
            if (!string.IsNullOrEmpty(soundEvent)) { AkSoundEngine.PostEvent(soundEvent, base.gameObject); }
            if (chosenProjectile != null)
            {
              Projectile spawned =  chosenProjectile.InstantiateAndFireInDirection(originPoint, direction.ToAngle(), angleVariance, null).GetComponent<Projectile>();
                if (spawned)
                {
                    if (base.m_owner)
                    {
                        spawned.Owner = base.m_owner;
                        spawned.Shooter = base.m_owner.specRigidbody;
                        spawned.ScaleByPlayerStats(base.m_owner);
                        spawned.ApplyCompanionModifierToBullet(base.m_owner);
                        base.m_owner.DoPostProcessProjectile(spawned);
                    }                
                }
            }
        }
        public float timer = 0;
        public float reloadTime = 1;
        public float cooldownTime = 1;
        public float angleVariance = 5;
        public int shotsFired = 0;
        public int shotsInClip = 0;
        public VFXPool MuzzleFlash = null;
        public Projectile chosenProjectile = null;
        public string soundEvent = null;
        private void Start()
        {
            LootEngine.DoDefaultPurplePoof(base.gameActor.CenterPosition);
            if (!setUp)
            {
                ETGModConsole.Log("NOTSETUP");
            }
        }
        public static void Init()
        {
            RandomisedBuddyCollection = AssetBundleLoader.FastLoadSpriteCollection(Initialisation.assetBundle, "RandomCompanionCollection", "RandomCompanionCollectionMaterial.mat");
            RandomCompanionAnimationCollection = Initialisation.assetBundle.LoadAsset<GameObject>("RandomCompanionAnimationCollection").GetComponent<tk2dSpriteAnimation>();

            if (prefab == null || !CompanionBuilder.companionDictionary.ContainsKey(guid))
            {
                prefab = EntityTools.BuildEntity("RandomisedBuddy", guid, "amogus_idle_001", RandomisedBuddyCollection, new IntVector2(8, 8), new IntVector2(-4, 0));
                RandomisedBuddyController companionController = prefab.AddComponent<RandomisedBuddyController>();
                companionController.companionID = CompanionController.CompanionIdentifier.NONE;

                tk2dSpriteAnimator dumbanimator = prefab.GetComponent<tk2dSpriteAnimator>();
                dumbanimator.Library = RandomCompanionAnimationCollection;

                AIActor aiactor = prefab.GetComponent<AIActor>();
                aiactor.CanDropCurrency = false;
                aiactor.CanDropItems = false;
                aiactor.BaseMovementSpeed = 6;
                aiactor.IgnoreForRoomClear = false;
                aiactor.TryDodgeBullets = false;
                aiactor.ActorName = "Randomised Buddy";
                aiactor.DoDustUps = true;
                aiactor.SetIsFlying(true, "hovering", false, true);
                //aiactor.ActorShadowOffset = new Vector3(0f, 0.4f);

                AIAnimator animator = prefab.GetComponent<AIAnimator>();
                animator.IdleAnimation = new DirectionalAnimation()
                {
                    Type = DirectionalAnimation.DirectionType.TwoWayHorizontal,
                    Flipped = new DirectionalAnimation.FlipType[] { DirectionalAnimation.FlipType.None, DirectionalAnimation.FlipType.Flip },
                    AnimNames = new List<string>() { "amogus", "amogus" }.ToArray(),
                    Prefix = string.Empty,
                };
                
                BehaviorSpeculator component = prefab.GetComponent<BehaviorSpeculator>();
                component.MovementBehaviors.Add(new CompanionFollowPlayerBehavior
                {
                    CatchUpRadius = 6,
                    CatchUpMaxSpeed = 10,
                    CatchUpAccelTime = 1,
                    CatchUpSpeed = 7,
                });
                component.MovementBehaviors.Add(new SeekTargetBehavior
                {
                    CustomRange = 7,
                    PathInterval = 0.25f
                });
                component.TargetBehaviors = new List<TargetBehaviorBase>()
                {
                    new TargetPlayerBehavior()
                    {
                        LineOfSight = false,
                        ObjectPermanence = false,
                        PauseOnTargetSwitch = false,
                        PauseTime = 0f,
                        Radius = 200f, //you can change this to whatever radius you want your companion to see enemies in
                        SearchInterval = 0.25f
                    }
                };
            }
        }
    }
}

