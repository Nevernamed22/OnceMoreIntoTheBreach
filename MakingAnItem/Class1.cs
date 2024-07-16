using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Dungeonator;
using Alexandria.EnemyAPI;
using GungeonAPI;
using Alexandria.ItemAPI;
using SaveAPI;
using UnityEngine;
using MonoMod.RuntimeDetour;
using System.Reflection;
using MonoMod.Utils;
using Brave.BulletScript;
using Random = System.Random;
using FullSerializer;
using Gungeon;
using Alexandria.CharacterAPI;
using BepInEx;
using Alexandria;
using Alexandria.Misc;
using Alexandria.Assetbundle;
using HarmonyLib;

namespace NevernamedsItems
{
    [BepInPlugin(GUID, "Once More Into The Breach", "1.30.0")]
    [BepInDependency(ETGModMainBehaviour.GUID)]
    [BepInDependency("etgmodding.etg.mtgapi")]
    [BepInDependency("kyle.etg.gapi")]
    [BepInDependency("alexandria.etgmod.alexandria")]
    [BepInDependency("pretzel.etg.gunfig")]
    public class Initialisation : BaseUnityPlugin
    {
        public const string GUID = "nevernamed.etg.omitb";
        public static Initialisation instance;
        //public static AdvancedStringDB Strings;
        public static string FilePathFolder;
        public static bool DEBUG_ITEM_DISABLE = false;

        //Assets
        public static AssetBundle assetBundle;
        public static tk2dSpriteCollectionData itemCollection;
        public static tk2dSpriteCollectionData gunCollection;
        public static tk2dSpriteCollectionData companionCollection;
        public static tk2dSpriteCollectionData VFXCollection;
        public static tk2dSpriteCollectionData NPCCollection;
        public static tk2dSpriteCollectionData ProjectileCollection;
        public static tk2dSpriteCollectionData MysteriousStrangerCollection;
        public static tk2dSpriteCollectionData TrapCollection;
        public static tk2dSpriteCollectionData EnvironmentCollection;
        public static tk2dSpriteCollectionData GunDressingCollection;

        public static tk2dSpriteAnimation projectileAnimationCollection;
        public static tk2dSpriteAnimation gunAnimationCollection;
        public static tk2dSpriteAnimation vfxAnimationCollection;
        public static tk2dSpriteAnimation npcAnimationCollection;
        public static tk2dSpriteAnimation companionAnimationCollection;
        public static tk2dSpriteAnimation mysteriousStrangerAnimationCollection;
        public static tk2dSpriteAnimation trapAnimationCollection;
        public static tk2dSpriteAnimation environmentAnimationCollection;
        public void Awake()
        {
        }
        public void Start()
        {
            ETGModMainBehaviour.WaitForGameManagerStart(GMStart);
        }
        public void GMStart(GameManager manager)
        {
            try
            {
                ETGModConsole.Log("Once More Into The Breach started initialising...");

                var harmony = new Harmony(GUID);
                harmony.PatchAll(Assembly.GetExecutingAssembly());

                instance = this;

                //Bepin bullshit
                ETGMod.Assets.SetupSpritesFromFolder(System.IO.Path.Combine(this.FolderPath(), "sprites"));
                FilePathFolder = this.FolderPath();

                //Assets
                assetBundle = AssetBundleLoader.LoadAssetBundleFromLiterallyAnywhere("omitbbundle", true);
                itemCollection = AssetBundleLoader.FastLoadSpriteCollection(assetBundle, "ItemCollection", "ItemCollectionMaterial.mat");
                gunCollection = AssetBundleLoader.FastLoadSpriteCollection(assetBundle, "GunCollection", "GunCollectionMaterial.mat");
                ProjectileCollection = AssetBundleLoader.FastLoadSpriteCollection(assetBundle, "ProjectileCollection", "ProjectileCollectionMaterial.mat");
                VFXCollection = AssetBundleLoader.FastLoadSpriteCollection(assetBundle, "VFXCollection", "VFXCollectionMaterial.mat");
                NPCCollection = AssetBundleLoader.FastLoadSpriteCollection(assetBundle, "NPCCollection", "NPCCollectionMaterial.mat");
                companionCollection = AssetBundleLoader.FastLoadSpriteCollection(assetBundle, "CompanionCollection", "CompanionCollectionMaterial.mat");
                MysteriousStrangerCollection = AssetBundleLoader.FastLoadSpriteCollection(assetBundle, "MysteriousStrangerCollection", "MysteriousStrangerCollectionMaterial.mat");
                TrapCollection = AssetBundleLoader.FastLoadSpriteCollection(assetBundle, "TrapCollection", "TrapCollectionMaterial.mat");
                EnvironmentCollection = AssetBundleLoader.FastLoadSpriteCollection(assetBundle, "EnvironmentCollection", "EnvironmentCollectionMaterial.mat");
                GunDressingCollection = AssetBundleLoader.FastLoadSpriteCollection(assetBundle, "GunDressing", "GunDressingMaterial.mat");

                projectileAnimationCollection = assetBundle.LoadAsset<GameObject>("ProjectileAnimationCollection").GetComponent<tk2dSpriteAnimation>();
                gunAnimationCollection = assetBundle.LoadAsset<GameObject>("GunAnimationCollection").GetComponent<tk2dSpriteAnimation>();
                vfxAnimationCollection = assetBundle.LoadAsset<GameObject>("VFXAnimationCollection").GetComponent<tk2dSpriteAnimation>();
                npcAnimationCollection = assetBundle.LoadAsset<GameObject>("NPCAnimationCollection").GetComponent<tk2dSpriteAnimation>();
                companionAnimationCollection = assetBundle.LoadAsset<GameObject>("CompanionAnimationCollection").GetComponent<tk2dSpriteAnimation>();
                mysteriousStrangerAnimationCollection = assetBundle.LoadAsset<GameObject>("MysteriousStrangerAnimationCollection").GetComponent<tk2dSpriteAnimation>();
                trapAnimationCollection = assetBundle.LoadAsset<GameObject>("TrapAnimationCollection").GetComponent<tk2dSpriteAnimation>();
                environmentAnimationCollection = assetBundle.LoadAsset<GameObject>("EnvironmentAnimationCollection").GetComponent<tk2dSpriteAnimation>();

                JsonEmbedder.EmbedJsonDataFromAssembly(Assembly.GetExecutingAssembly(), gunCollection, "NevernamedsItems/Resources/GunJsons");

                //Tools and Toolboxes
                StaticReferences.Init();
                ExoticPlaceables.Init();
                Tools.Init();

                Gunfigs.Init();

                EnemyTools.Init();
                SaveAPIManager.Setup("nn");
                AudioResourceLoader.InitAudio();
                CurseManager.Init();
                ETGModMainBehaviour.Instance.gameObject.AddComponent<GlobalUpdate>();
                ETGModMainBehaviour.Instance.gameObject.AddComponent<CustomDarknessHandler>();
                GameOfLifeHandler.Init();

                //Challenges
                Challenges.Init();

                //Hooks n Shit
                PlayerToolsSetup.Init();
                CompanionisedEnemyUtility.InitHooks();

                FloorAndGenerationToolbox.Init();
                ComplexProjModBeamCompatibility.Init();

                //VFX Setup
                VFXToolbox.InitVFX();
                EasyVFXDatabase.Init(); //Needs to occur before goop definition
                ShadeFlightHookFix.Init();

                //Status Effect Setup
                StaticStatusEffects.InitCustomEffects();
                PlagueStatusEffectSetup.Init();
                Confusion.Init();
                ExsanguinationSetup.Init();

                //Goop Setup
                EasyGoopDefinitions.DefineDefaultGoops();
                HoleGoop.Init();
                JarateGoop.Init();

                //Commands and Other Console Utilities
                Commands.Init();

                //Gamemodes
                AllJammedState.Init();
                JammedChests.Init();

                //Exotic Object Shit

                //VFX
                LockdownStatusEffect.Initialise();

                //Tweaks and Changes
                EnemyHealthModifiers.Init();

                //Unlock Handlers and Hooks

                MiscUnlockHooks.InitHooks();


                if (!DEBUG_ITEM_DISABLE)
                {
                    //Testing / Debug Items
                    ActiveTestingItem.Init();
                    PassiveTestingItem.Init();
                    BulletComponentLister.Init();
                    ObjectComponentLister.Init();

                    StandardisedProjectiles.Init();

                    //-----------------------------------------------------ITEMS GET INITIALISED
                    #region ItemInitialisation
                    //Character Starters
                    ShadeHand.Init();
                    ShadeHeart.Init();
                    //Egg Salad and Prima Bean can go here, because they were the first
                    EggSalad.Init();
                    PrimaBean.Init();
                    //Bullet modifiers
                    BashingBullets.Init();
                    TitanBullets.Init();
                    MistakeBullets.Init();
                    FiftyCalRounds.Init();
                    UnengravedBullets.Init();
                    EngravedBullets.Init();
                    HardReloadBullets.Init();
                    NitroBullets.Init();
                    SupersonicShots.Init();
                    GlassRounds.Init();
                    Junkllets.Init();
                    BloodthirstyBullets.Init();
                    CleansingRounds.Init();
                    HallowedBullets.Init();
                    PromethianBullets.Init();
                    EpimethianBullets.Init();
                    RandoRounds.Init();
                    HematicRounds.Init();
                    FullArmourJacket.Init();
                    MirrorBullets.Init();
                    CrowdedClip.Init();
                    BashfulShot.Init();
                    OneShot.Init();
                    BulletBullets.Init();
                    AntimatterBullets.Init();
                    SpectreBullets.Init();
                    Tabullets.Init();
                    TierBullets.Init(); //Unfinished
                    BombardierShells.Init();
                    GildedLead.Init();
                    DemoterBullets.Init();
                    Voodoollets.Init();
                    TracerRound.Init();
                    EndlessBullets.Init();
                    HellfireRounds.Init();
                    Birdshot.Init();
                    Unpredictabullets.Init();
                    WarpBullets.Init();
                    BulletsWithGuns.Init();
                    LaserBullets.Init();
                    WoodenBullets.Init();
                    ComicallyGiganticBullets.Init(); //Excluded
                    KnightlyBullets.Init();
                    EmptyRounds.Init();
                    LongswordShot.Init();
                    DrillBullets.Init();
                    FoamDarts.Init();
                    BatterBullets.Init();
                    ElectrumRounds.Init();
                    BreachingRounds.Init();
                    MagnetItem.Init();
                    EargesplittenLoudenboomerRounds.Init();
                    RoundsOfTheReaver.Init();
                    TheShell.Init();
                    //Status Effect Bullet Mods
                    SnailBullets.Init();
                    LockdownBullets.Init();
                    PestiferousLead.Init();
                    Shrinkshot.Init();
                    //Volley Modifying Bullet Mods
                    FlamingShells.Init();
                    ShroomedBullets.Init();
                    Splattershot.Init();
                    BackwardsBullets.Init();
                    CrossBullets.Init();
                    ShadeShot.Init();
                    //Insta-Kill Bullet Modifiers
                    MinersBullets.Init();
                    AntimagicRounds.Init();
                    AlkaliBullets.Init();
                    ShutdownShells.Init();
                    ERRORShells.Init();
                    OsteoporosisBullets.Init();
                    //NonBullet Stat Changers
                    MicroAIContact.Init();
                    LuckyCoin.Init();
                    IronSights.Init();
                    Lewis.Init();
                    MysticOil.Init();
                    VenusianBars.Init();
                    NumberOneBossMug.Init();
                    LibramOfTheChambers.Init();
                    OrganDonorCard.Init();
                    GlassGod.Init();
                    ChaosRuby.Init();
                    BlobulonRage.Init();
                    OverpricedHeadband.Init();
                    GunslingerEmblem.Init();
                    MobiusClip.Init();
                    ClipOnAmmoPouch.Init();
                    JawsOfDefeat.Init();
                    IridiumSnakeMilk.Init();
                    Starfruit.Init();
                    //Armour
                    ArmourBandage.Init();
                    GoldenArmour.Init();
                    ExoskeletalArmour.Init();
                    PowerArmour.Init();
                    ArmouredArmour.Init();
                    //Consumable Givers
                    LooseChange.Init();
                    SpaceMetal.Init();
                    //Blank Themed Items
                    TrueBlank.Init();
                    FalseBlank.Init();
                    SpareBlank.Init();
                    OpulentBlank.Init();
                    GrimBlanks.Init();
                    NNBlankPersonality.Init();
                    BlankDie.Init();
                    Blombk.Init();
                    Blanket.Init();
                    Blankh.Init();
                    //Key Themed Items
                    BlankKey.Init();
                    SharpKey.Init();
                    SpareKey.Init();
                    KeyChain.Init();
                    KeyBullwark.Init();
                    KeyBulletEffigy.Init();
                    FrostKey.Init();
                    ShadowKey.Init();
                    Keygen.Init();
                    CursedTumbler.Init();
                    //Ammo Box Themed Items
                    TheShellactery.Init();
                    BloodyAmmo.Init();
                    MengerAmmoBox.Init();
                    AmmoTrap.Init();
                    //Boxes and Stuff
                    BloodyBox.Init();
                    MaidenShapedBox.Init();
                    SetOfAllSets.Init();
                    Toolbox.Init();
                    PocketChest.Init();
                    DeliveryBox.Init();
                    Wonderchest.Init();
                    //Heart themed items
                    HeartPadlock.Init();
                    Mutagen.Init();
                    ForsakenHeart.Init();
                    HeartOfGold.Init();
                    GooeyHeart.Init();
                    ExaltedHeart.Init();
                    CheeseHeart.Init();
                    TinHeart.Init();
                    //Chambers
                    BarrelChamber.Init();
                    GlassChamber.Init();
                    FlameChamber.Init();
                    Recyclinder.Init();
                    Nitroglycylinder.Init();
                    SpringloadedChamber.Init();
                    WitheringChamber.Init();
                    HeavyChamber.Init();
                    CyclopeanChamber.Init();
                    ElectricCylinder.Init();
                    //Table Techs
                    TableTechTable.Init();
                    TableTechSpeed.Init();
                    TableTechInvulnerability.Init();
                    TableTechAmmo.Init();
                    TableTechGuon.Init();
                    TableTechNology.Init();
                    TableTechSpectre.Init();
                    TableTechAstronomy.Init();
                    TableTechVitality.Init();
                    UnsTableTech.Init();
                    RectangularMirror.Init();
                    //Guon Stones
                    WoodGuonStone.Init();
                    YellowGuonStone.Init();
                    GreyGuonStone.Init();
                    BlackGuonStone.Init();
                    GoldGuonStone.Init();
                    BrownGuonStone.Init();
                    CyanGuonStone.Init();
                    IndigoGuonStone.Init();
                    SilverGuonStone.Init();
                    MaroonGuonStone.Init();
                    UltraVioletGuonStone.Init();
                    InfraredGuonStone.Init();
                    LimeGuonStone.Init();
                    RainbowGuonStone.Init();
                    KaleidoscopicGuonStone.Init();
                    GuonBoulder.Init();
                    BloodglassGuonStone.Init();
                    //Ammolets
                    GlassAmmolet.Init();
                    WickerAmmolet.Init();
                    FuriousAmmolet.Init();
                    SilverAmmolet.Init();
                    IvoryAmmolet.Init();
                    KinAmmolet.Init();
                    Autollet.Init();
                    Keymmolet.Init();
                    Ammolock.Init();
                    HepatizonAmmolet.Init();
                    BronzeAmmolet.Init();
                    PearlAmmolet.Init();
                    NeutroniumAmmolet.Init();
                    Shatterblank.Init();
                    // Boots
                    CycloneCylinder.Init();
                    BootLeg.Init();
                    BlankBoots.Init();
                    BulletBoots.Init();
                    //Bracelets and Jewelry
                    FriendshipBracelet.Init();
                    ShellNecklace.Init();
                    DiamondBracelet.Init();
                    PearlBracelet.Init();
                    AmethystBracelet.Init();
                    PanicPendant.Init();
                    GunknightAmulet.Init();
                    AmuletOfShelltan.Init();
                    CrosshairNecklace.Init();
                    HauntedAmulet.Init();
                    //Rings
                    RingOfOddlySpecificBenefits.Init();
                    FowlRing.Init();
                    RingOfAmmoRedemption.Init();
                    RiskyRing.Init();
                    WidowsRing.Init();
                    ShadowRing.Init();
                    RingOfFortune.Init();
                    RingOfInvisibility.Init();
                    //Holsters
                    BlackHolster.Init();
                    TheBeholster.Init();
                    HiveHolster.Init();
                    ShoulderHolster.Init();
                    ArtilleryBelt.Init();
                    BeltFeeder.Init();
                    BulletShuffle.Init();
                    //Companions
                    MolotovBuddy.Init();
                    BabyGoodChanceKin.Init();
                    Potty.Init();
                    Peanut.Init();
                    DarkPrince.Init();
                    Diode.Init();
                    DroneCompanion.Init();
                    GregTheEgg.Init();
                    FunGuy.Init();
                    Gungineer.Init();
                    BabyGoodDet.Init();
                    AngrySpirit.Init();
                    Gusty.Init();
                    ScrollOfExactKnowledge.Init();
                    LilMunchy.Init();
                    Cubud.Init();
                    Hapulon.Init();
                    PrismaticSnail.Init();
                    //Potions / Jars 
                    SpeedPotion.Init();
                    LovePotion.Init();
                    HoneyPot.Init();
                    ChemicalBurn.Init();
                    WitchsBrew.Init();
                    Nigredo.Init();
                    Albedo.Init();
                    Citrinitas.Init();
                    Rubedo.Init();
                    HoleyWater.Init();
                    Jarate.Init();
                    //Remotes
                    ReinforcementRadio.Init();
                    //Medicine
                    BloodThinner.Init();
                    BoosterShot.Init();
                    ShotInTheArm.Init();
                    //Knives and Blades
                    WoodenKnife.Init();
                    DaggerOfTheAimgel.Init();
                    CombatKnife.Init();
                    Bayonet.Init();
                    LaserKnife.Init();
                    //Books
                    BookOfMimicAnatomy.Init();
                    KalibersPrayer.Init();
                    GunidaeSolvitHaatelis.Init();
                    //Maps
                    MapFragment.Init();
                    TatteredMap.Init();
                    //Clothing
                    CloakOfDarkness.Init();
                    TimeFuddlersRobe.Init();
                    //Eyes
                    BlueSteel.Init();
                    CartographersEye.Init();
                    BloodshotEye.Init();
                    ShadesEye.Init();
                    BeholsterEye.Init();
                    KalibersEye.Init();
                    //Hands
                    Lefthandedness.Init();
                    NecromancersRightHand.Init();
                    //Bombs
                    InfantryGrenade.Init();
                    DiceGrenade.Init();
                    //Peppers
                    PickledPepper.Init();
                    LaserPepper.Init();
                    PepperPoppers.Init();
                    //Mushrooms
                    PercussionCap.Init();
                    BlastingCap.Init();
                    //True Misc
                    Lvl2Molotov.Init();
                    GoldenAppleCore.Init();
                    AppleCore.Init();
                    AppleActive.Init();
                    LibationtoIcosahedrax.Init(); //Unfinished
                    BagOfHolding.Init();
                    ItemCoupon.Init();
                    IdentityCrisis.Init();
                    Pyromania.Init();
                    LiquidMetalBody.Init();
                    GunGrease.Init();
                    BomberJacket.Init();
                    DragunsScale.Init();
                    GTCWTVRP.Init();
                    BlightShell.Init();
                    BulletKinPlushie.Init();
                    Kevin.Init();
                    PurpleProse.Init();
                    RustyCasing.Init();
                    HikingPack.Init();
                    GunpowderPheromones.Init();
                    GunsmokePerfume.Init();
                    Pestilence.Init();
                    ElevatorButton.Init();
                    Bullut.Init();
                    GSwitch.Init();
                    FaultyHoverboots.Init(); //Unfinished
                    Accelerant.Init();
                    HornedHelmet.Init();
                    HelmOfChaos.Init();
                    RocketMan.Init();
                    Roulette.Init(); //Unfinished
                    FinishedBullet.Init();
                    ChanceKinEffigy.Init();
                    MagickeCauldron.Init();
                    Bombinomicon.Init();
                    ClaySculpture.Init();
                    GracefulGoop.Init();
                    MrFahrenheit.Init();
                    MagicQuiver.Init();
                    FocalLenses.Init();
                    MagicMissile.Init();
                    AmberDie.Init();
                    ObsidianPistol.Init();
                    Showdown.Init();
                    UnderbarrelShotgun.Init();
                    LootEngineItem.Init();
                    Ammolite.Init();
                    PortableHole.Init();
                    CardinalsMitre.Init();
                    GunjurersBelt.Init();
                    GoomperorsCrown.Init();
                    ChemGrenade.Init();
                    EightButton.Init();
                    TitaniumClip.Init();
                    PaperBadge.Init();
                    SculptorsChisel.Init();
                    Permafrost.Init();
                    GlassShard.Init();
                    EqualityItem.Init();
                    BitBucket.Init();
                    Eraser.Init();
                    GunpowderGreen.Init();
                    TackShooter.Init();
                    ChanceCard.Init();
                    Moonrock.Init();
                    Telekinesis.Init();
                    DeathMask.Init();
                    TabletOfOrder.Init();
                    Bambarrage.Init();
                    AmmoGland.Init();
                    BeggarsBelief.Init();
                    LeadSoul.Init();
                    LeadOfLife.Init();
                    AWholeBulletKin.Init();
                    #endregion

                    //-----------------------------------------------------GUNS GET INITIALISED
                    #region GunInitialisation
                    //UNFINISHED / TEST GUNS
                    WailingMagnum.Add();
                    Defender.Add();
                    TestGun.Add();
                    Gunycomb.Add();
                    GlobbitSMALL.Add();
                    GlobbitMED.Add();
                    GlobbitMEGA.Add();


                    //GUNS

                    //CHARACTERSTARTERS
                    ElderMagnum.Add();

                    //REVOLVERS
                    FlayedRevolver.Add();
                    G20.Add();
                    MamaGun.Add();
                    LovePistol.Add();
                    DiscGun.Add();
                    Repeatovolver.Add();
                    Pista.Add();
                    NNGundertale.Add();
                    DiamondGun.Add();
                    NNMinigun.Add();
                    ShroomedGun.Add();
                    GoldenRevolver.Add();
                    Nocturne.Add();
                    BackWarder.Add();
                    Redhawk.Add();
                    ToolGun.Add();
                    //GENERAL HANDGUNS
                    StickGun.Add();
                    Glock42.Add();
                    StarterPistol.Add();
                    ScrapStrap.Add();
                    PopGun.Add();
                    UnusCentum.Add();
                    StunGun.Add();
                    CopperSidearm.Add();
                    Rekeyter.Add();
                    HotGlueGun.Add();
                    UpNUp.Add();
                    RedRobin.Add();
                    VariableGun.Add();
                    CrescendoBlaster.Add();
                    Glasster.Add();
                    HandGun.Add();
                    Viper.Add();
                    DiamondCutter.Add();
                    MarchGun.Add();
                    RebarGun.Add();
                    MinuteGun.Add();
                    Ulfberht.Add();
                    SpacersFancy.Add();
                    FractalGun.Add();
                    SalvatorDormus.Add();
                    ServiceWeapon.Add();
                    HeadOfTheOrder.Add();
                    GunOfAThousandSins.Add();
                    DoubleGun.Add();
                    //SHOTGUNS
                    JusticeGun.Add();
                    Orgun.Add();
                    Octagun.Add();
                    ClownShotgun.Add();
                    Ranger.Add();
                    RustyShotgun.Add();
                    TheBride.Add();
                    TheGroom.Add();
                    IrregularShotgun.Add();
                    GrenadeShotgun.Add();
                    Jackhammer.Add();
                    Tomahawk.Add();
                    SaltGun.Add();
                    SoapGun.Add();
                    //CANNONS
                    Felissile.Add();
                    HandCannon.Add();
                    Lantaka.Add();
                    GreekFire.Add();
                    EmberCannon.Add();
                    ElysiumCannon.Add();
                    DisplacerCannon.Add();
                    //SCI-FI GUNS
                    Rewarp.Add();
                    Blasmaster.Add();
                    St4ke.Add();
                    Robogun.Add();
                    CortexBlaster.Add();
                    RedBlaster.Add();
                    BeamBlade.Add();
                    Neutrino.Add();
                    Rico.Add();
                    TheThinLine.Add();
                    RocketPistol.Add();
                    Repetitron.Add();
                    Dimensionaliser.Add();
                    Purpler.Add();
                    VacuumGun.Add();
                    Oxygun.Add();
                    LtBluesPhaser.Add();
                    TriBeam.Add();
                    WaveformLens.Add();
                    KineticBlaster.Add();
                    LaserWelder.Add();
                    QBeam.Add();
                    HighVelocityRifle.Add();
                    Demolitionist.Add();
                    PumpChargeShotgun.Add();
                    TheOutbreak.Add();
                    Multiplicator.Add();
                    PunishmentRay.Add();
                    YBeam.Add();
                    WallRay.Add();
                    BolaGun.Add();
                    AlphaBeam.Add();
                    Glazerbeam.Add();
                    StasisRifle.Add();
                    Gravitron.Add();
                    Ferrobolt.Add();
                    ParticleBeam.Add();
                    TauCannon.Add();
                    GravityGun.Add();
                    GalaxyCrusher.Add();
                    //ARC Weapons
                    ARCPistol.Add();
                    ARCShotgun.Add();
                    ARCRifle.Add();
                    ARCTactical.Add();
                    ARCCannon.Add();
                    //BOWS AND CROSSBOWS
                    IceBow.Add();
                    TitanSlayer.Add();
                    Boltcaster.Add();
                    VulcanRepeater.Add();
                    Clicker.Add();
                    //ANTIQUES
                    WheelLock.Add();
                    Welrod.Add();
                    Welgun.Add();
                    TheLodger.Add();
                    Gonne.Add();
                    Hwacha.Add();
                    FireLance.Add();
                    HandMortar.Add();
                    GrandfatherGlock.Add();
                    GatlingGun.Add();
                    Blowgun.Add();
                    Smoker.Add();
                    Gaxe.Add();
                    WoodenHorse.Add();
                    AgarGun.Add();
                    MusketRifle.Add();
                    Arquebus.Add();
                    TheBlackSpot.Add();
                    //KNIVES AND BLADES
                    Carnwennan.Add();
                    MantidAugment.Add();
                    Claymore.Add();
                    Scythe.Add();
                    //REALISTIC GUNS
                    HeatRay.Add();
                    BlueGun.Add();
                    BarcodeScanner.Add();
                    AntimaterielRifle.Add();
                    Primos1.Add();
                    DartRifle.Add();
                    AM0.Add();
                    RiskRifle.Add();
                    AverageJoe.Add();
                    RiotGun.Add();
                    Kalashnirang.Add();
                    Schwarzlose.Add();
                    MaidenRifle.Add();
                    Blizzkrieg.Add();
                    Copygat.Add();
                    Skorpion.Add();
                    HeavyAssaultRifle.Add();
                    DynamiteLauncher.Add();
                    BouncerUzi.Add();
                    Borz.Add();
                    Borchardt.Add();
                    MarbledUzi.Add();
                    BurstRifle.Add();
                    DublDuck.Add();
                    Type56.Add();
                    G11.Add();
                    C7A2.Add();
                    Rheinmetole.Add();
                    OlReliable.Add();
                    //FLAMETHROWERS
                    FlamethrowerMk1.Add();
                    FlamethrowerMk2.Add();
                    //MISSILE LAUNCHERS
                    BouncerRPG.Add();
                    BottleRocket.Add();
                    NNBazooka.Add();
                    BoomBeam.Add();
                    Pillarocket.Add();
                    DoomBoom.Add();
                    //ANIMAL / ORGANIC GUNS
                    SporeLauncher.Add();
                    PoisonDartFrog.Add();
                    Corgun.Add();
                    FungoCannon.Add();
                    PhaserSpiderling.Add();
                    Guneonate.Add();
                    KillithidTendril.Add();
                    Gunger.Add();
                    SickWorm.Add();
                    MiniMonger.Add();
                    CarrionFormeTwo.Add();
                    CarrionFormeThree.Add();
                    Carrion.Add();
                    UterinePolyp.Add();
                    Wrinkler.Add();
                    BrainBlast.Add();
                    //SNAKE GUNS
                    SnakePistol.Add();
                    //BLADES
                    ButchersKnife.Add();
                    RapidRiposte.Add();
                    //FUN GUNS
                    Spitballer.Add();
                    ConfettiCannon.Add();
                    Gumgun.Add();
                    Glooper.Add();
                    ChewingGun.Add();
                    Makatov.Add();
                    Accelerator.Add();
                    PaintballGun.Add();
                    Converter.Add();
                    Spiral.Add();
                    Gunshark.Add();
                    FingerGuns.Add();
                    OBrienFist.Add();
                    GolfRifle.Add();
                    Pandephonium.Add();
                    Sweeper.Add();
                    DeskFan.Add();
                    Pencil.Add();
                    SquareBracket.Add();
                    SquarePeg.Add();
                    Ringer.Add();
                    Snaker.Add();
                    GayK47.Add();
                    LaundromaterielRifle.Add();
                    DecretionCarbine.Add();
                    Amalgun.Add();
                    RC360.Add();
                    RazorRifle.Add();
                    UziSpineMM.Add();
                    PineNeedler.Add();
                    AlternatingFire.Add();
                    Autogun.Add();
                    Rebondir.Add();
                    BigShot.Add();
                    W3irdstar.Add();
                    Seismograph.Add();
                    CashBlaster.Add();
                    PocoLoco.Add();
                    BioTranstater2100.Add();
                    //MAGICAL GUNS
                    Bejeweler.Add();
                    TotemOfGundying.Add();
                    Icicle.Add();
                    GunjurersStaff.Add();
                    InitiateWand.Add();
                    LightningRod.Add();
                    OrbOfTheGun.Add();
                    SpearOfJustice.Add();
                    Protean.Add();
                    BulletBlade.Add();
                    Bookllet.Add();
                    Lorebook.Add();
                    Beastclaw.Add();
                    Bullatterer.Add();
                    Entropew.Add();
                    Missinguno.Add();
                    Paraglocks.Add();
                    TheGreyStaff.Add();
                    //CONSUMABLE FIRING GUNS
                    Creditor.Add();
                    Blankannon.Add();
                    Viscerifle.Add();
                    //ENDPAGE GUNS
                    MastersGun.Add();
                    Wrench.Add();
                    Pumhart.Add();


                    //SYNERGY FORME GUNS
                    GunsharkMegasharkSynergyForme.Add();
                    DiscGunSuperDiscForme.Add();
                    OrgunHeadacheSynergyForme.Add();
                    Wolfgun.Add();
                    MinigunMiniShotgunSynergyForme.Add();
                    PenPencilSynergy.Add();
                    ReShelletonKeyter.Add();
                    AM0SpreadForme.Add();
                    BulletBladeGhostForme.Add();
                    GlueGunGlueGunnerSynergy.Add();
                    KingBullatterer.Add();
                    WrenchNullRefException.Add();
                    GatlingGunGatterUp.Add();
                    GravityGunNegativeMatterForm.Add();
                    GonneElder.Add();
                    UterinePolypWombular.Add();
                    DiamondGaxe.Add();
                    RedRebondir.Add();
                    DiamondCutterRangerClass.Add();
                    StickGunQuickDraw.Add();
                    StormRod.Add();
                    UnrustyShotgun.Add();
                    DARCPistol.Add();
                    DARCRifle.Add();
                    DARCShotgun.Add();
                    DARCTactical.Add();
                    DARCCannon.Add();
                    Bloodwash.Add();
                    SalvatorDormusM1893.Add();
                    ServiceWeaponShatter.Add();
                    ServiceWeaponSpin.Add();
                    ServiceWeaponPierce.Add();
                    ServiceWeaponCharge.Add();
                    BigBorz.Add();
                    Spitfire.Add();
                    #endregion


                    //-----------------------------------------------------SHRINES GET INITIALISED
                    #region ShrineInitialisation
                    ShrineSetup.Init();
                    #endregion

                    //-----------------------------------------------------PLACEABLES GET INITIALISED
                    GenericPlaceables.Init();
                    StatueTraps.Init();
                    GuillotineTrap.Init();
                    LowWalls.Init();
                    GoldButton.Init();
                    Breakables.Init();

                    //-----------------------------------------------------NPCS GET INITIALISED
                    #region NPCInitialisation
                    Rusty.Init();
                    Ironside.Init();
                    Boomhildr.Init();
                    Doug.Init();
                    BowlerShop.Init();
                    Dispenser.Init();
                    SlotMachine.Init();
                    Chancellot.Init();
                    MysteriousStranger.Init();
                    Beggar.Init();
                    Jammomaster.Init();
                    GenericCultist.Init();
                    #endregion

                    ChromaGun.Add();

                    //GOOD MIMIC (NEEDS TO BE INITIALISED LATER)
                    GoodMimic.Add();

                    //Characters
                    var data = Loader.BuildCharacter("NevernamedsItems/Characters/Shade",
                       "nevernamed.etg.omitb",
                        new Vector3(12.3f, 21.3f),
                        false,
                         new Vector3(13.1f, 19.1f),
                         false,
                         false,
                         true,
                         true, //Sprites used by paradox
                         false, //Glows
                         null, //Glow Mat
                         null, //Alt Skin Glow Mat
                         0, //Hegemony Cost
                         false, //HasPast
                         ""); //Past ID String
                    /*
                               var acolyteData = Loader.BuildCharacter("NevernamedsItems/Characters/Acolyte",
                               CustomPlayableCharacters.Acolyte,
                               new Vector3(12.3f, 25.3f),
                               false,
                                new Vector3(13.1f, 19.1f),
                                false,
                                false,
                                true,
                                true, //Sprites used by paradox
                                false, //Glows
                                null, //Glow Mat
                                null, //Alt Skin Glow Mat
                                0, //Hegemony Cost
                                false, //HasPast
                                ""); //Past ID String*/

                    //Other Features
                    AdditionalMasteries.Init();
                    CadenceAndOxShopPoolAdditions.Init();
                    CustomHuntingQuest.Init();

                    //NPCS
                    //Carto.Add();

                    //Synergy Setup, Synergy Formes, Dual Wielding, and any changes to Basegame Guns
                    InitialiseSynergies.DoInitialisation();
                    SynergyFormInitialiser.AddSynergyForms();
                    ExistantGunModifiers.Init();
                    Tags.Init();

                    //Setup lead of life companions
                    LeadOfLifeInitCompanions.BuildPrefabs();
                }

                //Post All Items Rooms
                BeggarsBelief.InitRooms();
                ChanceCard.InitRooms();

                KillUnlockHandler.Init();

                ETGModConsole.Commands.GetGroup("nn").AddUnit("listassets", delegate (string[] args)
                {
                    foreach (string name in BundlePrereqs)
                    {
                        string[] names = ResourceManager.LoadAssetBundle(name).GetAllAssetNames();
                        foreach (string n in names)
                        {
                            ETGModConsole.Log(n);
                        }
                    }
                }, null);


                ETGModConsole.Commands.GetGroup("nn").AddUnit("setObj", delegate (string[] args)
                {
                    if (tempdict == null)
                    {
                        tempdict = new Dictionary<string, GameObject>()
                        {
                            {"rattrap", (EnemyDatabase.GetOrLoadByGuid("6868795625bd46f3ae3e4377adce288b").GetComponent<ResourcefulRatController>().MouseTraps[1]) },
                            {"oubdrop", ExoticPlaceables.OubTrapdoor },
                            {"telesign", ExoticPlaceables.TeleporterSign },
                            {"shoplayout", ExoticPlaceables.ShopLayout },

                            {"shopcrates", ExoticPlaceables.Crates },
                            {"shopcrate", ExoticPlaceables.Crate },
                            {"shopsack", ExoticPlaceables.Sack },
                            {"shopshellbarrel", ExoticPlaceables.ShellBarrel },
                            {"shopshelf", ExoticPlaceables.Shelf },
                            {"shopmask", ExoticPlaceables.Mask },
                            {"shopwallsword", ExoticPlaceables.Wallsword },
                            {"shopstandingshelf", ExoticPlaceables.StandingShelf },
                            {"shopakbarrel", ExoticPlaceables.AKBarrel },
                            {"shopstool", ExoticPlaceables.Stool },

                            {"upsign", ExoticPlaceables.SignUp },
                            {"rightsign", ExoticPlaceables.SignRight },
                            {"leftsign", ExoticPlaceables.SignLeft },

                            {"secretroom", ExoticPlaceables.secretroomlayout },

                         };
                    }

                    string floorToCheck = "UNDEFINED";
                    if (args != null && args.Length > 0 && args[0] != null) { if (!string.IsNullOrEmpty(args[0])) { floorToCheck = args[0]; } }
                    floorToCheck = floorToCheck.Replace("=", " ");
                    bool set = false;
                    if (tempdict.ContainsKey(floorToCheck))
                    {
                        toSpawn = tempdict[floorToCheck];
                        set = true;
                    }
                    else
                    {
                        if (LoadHelper.LoadAssetFromAnywhere<GameObject>(floorToCheck))
                        {
                            toSpawn = LoadHelper.LoadAssetFromAnywhere<GameObject>(floorToCheck);
                            set = true;
                        }
                        else if (LoadHelper.LoadAssetFromAnywhere<DungeonPlaceable>(floorToCheck))
                        {
                            toSpawn = LoadHelper.LoadAssetFromAnywhere<DungeonPlaceable>(floorToCheck).variantTiers[0].GetOrLoadPlaceableObject;
                            set = true;
                        }
                    }
                    if (!set) { ETGModConsole.Log("FAILED"); }

                }, null);


                ETGMod.StartGlobalCoroutine(this.delayedstarthandler());
                ETGModConsole.Log("'If you're reading this, I must have done something right' - NN");
            }
            catch (Exception e)
            {
                ETGModConsole.Log(e.Message);
                ETGModConsole.Log(e.StackTrace);
            }
        }

        public static SharedInjectionData GungeonProperInjections;
        public static Dictionary<string, GameObject> tempdict;
        public static GameObject toSpawn;
        public static string[] BundlePrereqs = new string[]
                {
                    "dungeon_scene_001",
                    "encounters_base_001",
                    "enemies_base_001",
                    "flows_base_001",
                    "foyer_001",
                    "foyer_002",
                    "foyer_003",
                    "shared_base_001",
                    "dungeons/base_bullethell",
                    "dungeons/base_castle",
                    "dungeons/base_catacombs",
                    "dungeons/base_cathedral",
                    "dungeons/base_forge",
                    "dungeons/base_foyer",
                    "dungeons/base_gungeon",
                    "dungeons/base_mines",
                    "dungeons/base_nakatomi",
                    "dungeons/base_resourcefulrat",
                    "dungeons/base_sewer",
                    "dungeons/base_tutorial",
                    "dungeons/finalscenario_bullet",
                    "dungeons/finalscenario_convict",
                    "dungeons/finalscenario_coop",
                    "dungeons/finalscenario_guide",
                    "dungeons/finalscenario_pilot",
                    "dungeons/finalscenario_robot",
                    "dungeons/finalscenario_soldier"
                };
        public IEnumerator delayedstarthandler()
        {
            yield return null;
            this.DelayedInitialisation();
            yield break;
        }
        public void DelayedInitialisation()
        {
            try
            {
                LibramOfTheChambers.LateInit();

                CrossmodNPCLootPoolSetup.CheckItems();

                OMITBChars.Shade = ETGModCompatibility.ExtendEnum<PlayableCharacters>(Initialisation.GUID, "Shade");

                ETGModConsole.Log("(Also finished DelayedInitialisation)");
            }
            catch (Exception e)
            {
                ETGModConsole.Log(e.Message);
                ETGModConsole.Log(e.StackTrace);
            }
        }
    }
}


