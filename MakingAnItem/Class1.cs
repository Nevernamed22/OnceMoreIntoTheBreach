﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Dungeonator;
using EnemyAPI;
using GungeonAPI;
using ItemAPI;
using NpcApi;
using SaveAPI;
using UnityEngine;
using MonoMod.RuntimeDetour;
using System.Reflection;
using MonoMod.Utils;
using Brave.BulletScript;
using Random = System.Random;
using FullSerializer;
using Gungeon;

namespace NevernamedsItems
{
    public class Initialisation : ETGModule
    {
        public static ETGModuleMetadata metadata = new ETGModuleMetadata();
        public static string ZipFilePath;
        public static string FilePath;
        public static string ModName;
        //public static AdvancedStringDB Strings;
        public override void Exit()
        {
        }

        public override void Init()
        {
        }

        public override void Start()
        {
            try
            {
                ETGModConsole.Log("Once More Into The Breach started initialising...");

                //Rooms
                ZipFilePath = this.Metadata.Archive;
                FilePath = this.Metadata.Directory + "/rooms";

                //Tools and Toolboxes
                StaticReferences.Init();
                DungeonHandler.Init();
                Tools.Init();
                ShrineFakePrefabHooks.Init();

                ShrineFactory.Init();
                OldShrineFactory.Init();

                FakePrefabHooks.Init();

                ItemBuilder.Init();       


                EnemyTools.Init();
                Hooks.Init();
                SaveAPIManager.Setup("nn");
                //AudioResourceLoader.InitAudio();
                CurseManager.Init();
                ETGModMainBehaviour.Instance.gameObject.AddComponent<GlobalUpdate>();
                ETGModMainBehaviour.Instance.gameObject.AddComponent<CustomDarknessHandler>();


                //Challenges
                Challenges.Init();

                //Hooks n Shit
                PlayerToolsSetup.Init();
                EnemyHooks.InitEnemyHooks();
                MiscUnlockHooks.InitHooks();
                FloorAndGenerationToolbox.Init();
                PedestalHooks.Init();
                ExplosionHooks.Init();
                UIHooks.Init();
                ComplexProjModBeamCompatibility.Init();
                ReloadBreachShrineHooks.Init();
                metadata = this.Metadata;
                //VFX Setup
                VFXToolbox.InitVFX();
                EasyVFXDatabase.Init(); //Needs to occur before goop definition

                //Status Effect Setup
                StaticStatusEffects.InitCustomEffects();
                PlagueStatusEffectSetup.Init();

                //Goop Setup
                EasyGoopDefinitions.DefineDefaultGoops();
                DoGoopEffectHook.Init();

                //Null Asset Bundles to prevent infini-load
                /* for (int i = 0; i < RoomFactory.assetBundles.Length; i++)
                 {
                     RoomFactory.assetBundles[i] = null;
                 }*/
                //StaticReferences.AssetBundles.Clear();

                //Commands and Other Console Utilities
                Commands.Init();

                //Hats
                HatUtility.NecessarySetup();
                HatDefinitions.Init();

                //Gamemodes
                AllJammedState.Init();
                JammedChests.Inithooks();

                //VFX
                LockdownStatusEffect.Initialise();

                //Testing / Debug Items
                ActiveTestingItem.Init();
                PassiveTestingItem.Init();
                BulletComponentLister.Init();
                ObjectComponentLister.Init();
                THECAGE.Init();

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
                IngressBullets.Init(); //Unfinished
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
                BalancedBullets.Init(); //Unfinished
                WoodenBullets.Init();
                ComicallyGiganticBullets.Init(); //Excluded
                KnightlyBullets.Init();
                EmptyRounds.Init();
                LongswordShot.Init();
                DrillBullets.Init();
                FoamDarts.Init();
                EargesplittenLoudenboomerRounds.Init();
                TheShell.Init();
                //Status Effect Bullet Mods
                SnailBullets.Init();
                LockdownBullets.Init();
                PestiferousLead.Init();
                Shrinkshot.Init();
                //Volley Modifying Bullet Mods
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
                Blombk.Init();
                Blankh.Init();
                //Key Themed Items
                BlankKey.Init();
                SharpKey.Init();
                SpareKey.Init();
                KeyChain.Init();
                KeyBullwark.Init();
                KeyBulletEffigy.Init();
                FrostKey.Init();
                CursedTumbler.Init();
                //Ammo Box Themed Items
                TheShellactery.Init();
                BloodyAmmo.Init();
                MengerAmmoBox.Init();
                AmmoTrap.Init();
                //Boxes and Stuff
                BloodyBox.Init();
                MaidenShapedBox.Init();
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
                GlassChamber.Init();
                FlameChamber.Init();
                Recyclinder.Init();
                Nitroglycylinder.Init();
                SpringloadedChamber.Init();
                WitheringChamber.Init();
                HeavyChamber.Init();
                //Table Techs
                TableTechTable.Init();
                TableTechSpeed.Init();
                TableTechInvulnerability.Init();
                TableTechAmmo.Init();
                TableTechGuon.Init();
                TableTechNology.Init();
                UnsTableTech.Init();
                //Guon Stones
                WoodGuonStone.Init();
                YellowGuonStone.Init();
                GreyGuonStone.Init();
                GoldGuonStone.Init();
                BrownGuonStone.Init();
                CyanGuonStone.Init();
                IndigoGuonStone.Init();
                SilverGuonStone.Init();
                MaroonGuonStone.Init();
                UltraVioletGuonStone.Init();
                InfraredGuonStone.Init();
                RainbowGuonStone.Init();
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
                NeutroniumAmmolet.Init();
                Shatterblank.Init();
                // Boots
                CycloneCylinder.Init();
                BootLeg.Init();
                BlankBoots.Init();
                BulletBoots.Init();
                //Bracelets and Jewelry
                DiamondBracelet.Init();
                PearlBracelet.Init();
                GunknightAmulet.Init();
                AmuletOfShelltan.Init();
                //Rings
                RingOfOddlySpecificBenefits.Init();
                FowlRing.Init();
                RingOfAmmoRedemption.Init();
                RiskyRing.Init();
                WidowsRing.Init();
                RingOfInvisibility.Init();
                //Holsters
                BlackHolster.Init();
                TheBeholster.Init();
                HiveHolster.Init();
                ShoulderHolster.Init();
                ArtilleryBelt.Init();
                //Companions
                MolotovBuddy.Init();
                BabyGoodChanceKin.Init();
                Potty.Init();
                Peanut.Init();
                Diode.Init();
                DroneCompanion.Init();
                GregTheEgg.Init();
                BabyGoodDet.Init();
                Gusty.Init();
                ScrollOfExactKnowledge.Init();
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
                //Remotes
                ReinforcementRadio.Init();
                //Medicine
                BloodThinner.Init();
                BoosterShot.Init();
                //Knives and Blades
                DaggerOfTheAimgel.Init();
                CombatKnife.Init();
                Bayonet.Init();
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
                CartographersEye.Init();
                BloodshotEye.Init();
                ShadesEye.Init();
                KalibersEye.Init();
                //Hands
                Lefthandedness.Init();
                NecromancersRightHand.Init();
                //Bombs
                InfantryGrenade.Init();
                //True Misc
                Lvl2Molotov.Init();
                GoldenAppleCore.Init();
                AppleCore.Init();
                AppleActive.Init();
                LibationtoIcosahedrax.Init(); //Unfinished
                BagOfHolding.Init();
                ItemCoupon.Init();
                IdentityCrisis.Init();
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
                AcidAura.Init();
                HornedHelmet.Init();
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
                ObsidianPistol.Init();
                Showdown.Init();
                LootEngineItem.Init();
                Ammolite.Init();
                PortableHole.Init();
                CardinalsMitre.Init();
                GunjurersBelt.Init();
                GoomperorsCrown.Init();
                ChemGrenade.Init();
                TitaniumClip.Init();
                PaperBadge.Init();
                Permafrost.Init();
                GlassShard.Init();
                EqualityItem.Init();
                BitBucket.Init();
                Eraser.Init();
                Moonrock.Init();
                Telekinesis.Init();
                TabletOfOrder.Init();
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
                ToolGun.Add();
                //GENERAL HANDGUNS
                Glock42.Add();
                StarterPistol.Add();
                UnusCentum.Add();
                StunGun.Add();
                Rekeyter.Add();
                HotGlueGun.Add();
                UpNUp.Add();
                VariableGun.Add();
                CrescendoBlaster.Add();
                Glasster.Add();
                HandGun.Add();
                Viper.Add();
                MinuteGun.Add();
                HeadOfTheOrder.Add();
                GunOfAThousandSins.Add();
                DoubleGun.Add();
                //SHOTGUNS
                JusticeGun.Add();
                Orgun.Add();
                Octagun.Add();
                ClownShotgun.Add();
                Ranger.Add();
                TheBride.Add();
                TheGroom.Add();
                GrenadeShotgun.Add();
                SaltGun.Add();
                //CANNONS
                Felissile.Add();
                HandCannon.Add();
                Lantaka.Add();
                GreekFire.Add();
                EmberCannon.Add();
                DisplacerCannon.Add();
                //SCI-FI GUNS
                Blasmaster.Add();
                St4ke.Add();
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
                TriBeam.Add();
                KineticBlaster.Add();
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
                TauCannon.Add();
                GravityGun.Add();
                //BOWS AND CROSSBOWS
                IceBow.Add();
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
                Gaxe.Add();
                WoodenHorse.Add();
                //REALISTIC GUNS
                HeatRay.Add();
                BarcodeScanner.Add();
                AntimaterielRifle.Add();
                Primos1.Add();
                DartRifle.Add();
                AM0.Add();
                RiskRifle.Add();
                RiotGun.Add();
                Kalashnirang.Add();
                MaidenRifle.Add();
                Blizzkrieg.Add();
                Copygat.Add();
                HeavyAssaultRifle.Add();
                DynamiteLauncher.Add();
                //MISSILE LAUNCHERS
                BottleRocket.Add();
                NNBazooka.Add();
                BoomBeam.Add();
                Pillarocket.Add();
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
                //BLADES
                ButchersKnife.Add();
                MantidAugment.Add();
                RapidRiposte.Add();
                //FUN GUNS
                Gumgun.Add();
                Glooper.Add();
                Accelerator.Add();
                PaintballGun.Add();
                Spiral.Add();
                Gunshark.Add();
                FingerGuns.Add();
                GolfRifle.Add();
                Pandephonium.Add();
                DeskFan.Add();
                Pencil.Add();
                Ringer.Add();
                Snaker.Add();
                GayK47.Add();
                RC360.Add();
                BigShot.Add();
                BioTranstater2100.Add();
                //MAGICAL GUNS
                Icicle.Add();
                GunjurersStaff.Add();
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
                #endregion

                //-----------------------------------------------------SHRINES GET INITIALISED
                #region ShrineInitialisation
                InvestmentShrine.Add();
                RelodinShrine.Add();
                DagunShrine.Add();
                ArtemissileShrine.Add();
                ExecutionerShrine.Add();
                TurtleShrine.Add();
                #endregion

                //-----------------------------------------------------NPCS GET INITIALISED
                #region NPCInitialisation
                Rusty.Init();
                #endregion

                ChromaGun.Add();

                //GOOD MIMIC (NEEDS TO BE INITIALISED LATER)
                GoodMimic.Add();

                //Other Features
                MasteryReplacementOub.InitDungeonHook();
                CadenceAndOxShopPoolAdditions.Init();
                CustomHuntingQuest.Init();

                //NPCS
                TheJammomaster.Add();
                //Carto.Add();
                ShrineFactory.PlaceBreachShrines();

                //Synergy Setup, Synergy Formes, Dual Wielding, and any changes to Basegame Guns
                InitialiseSynergies.DoInitialisation();
                SynergyFormInitialiser.AddSynergyForms();
                ExistantGunModifiers.Init();

                //Late Hooks
                AmmoPickupHooks.Init();
                HealthPickupHooks.Init();

                ETGModConsole.Commands.AddUnit("nndebugflow", (args) => { DungeonHandler.debugFlow = !DungeonHandler.debugFlow; string status = DungeonHandler.debugFlow ? "enabled" : "disabled"; string color = DungeonHandler.debugFlow ? "00FF00" : "FF0000"; ETGModConsole.Log($"OMITB flow {status}", false); });

                //PoopySchloopy
                /* Dungeon keepDungeon = DungeonDatabase.GetOrLoadByName("base_jungle");
                 if (keepDungeon == null) ETGModConsole.Log("Jungle null!");
                 if (keepDungeon && keepDungeon.PatternSettings != null)
                 {
                     if (keepDungeon.PatternSettings.flows != null && keepDungeon.PatternSettings.flows.Count > 0)
                     {
                         if (keepDungeon.PatternSettings.flows[0].fallbackRoomTable)
                         {
                             if (keepDungeon.PatternSettings.flows[0].fallbackRoomTable.includedRooms != null)
                             {
                                 if (keepDungeon.PatternSettings.flows[0].fallbackRoomTable.includedRooms.elements != null)
                                 {
                                     foreach (WeightedRoom wRoom in keepDungeon.PatternSettings.flows[0].fallbackRoomTable.includedRooms.elements)
                                     {

                                         if (wRoom.room != null && !string.IsNullOrEmpty(wRoom.room.name))
                                         {
                                             ETGModConsole.Log(wRoom.room.name);
                                         }
                                     }
                                 }
                                 else ETGModConsole.Log("No elements");
                             }
                             else ETGModConsole.Log("No included rooms");
                         }
                         else ETGModConsole.Log("No fallback room table");
                     }
                     else ETGModConsole.Log("Flow was null or empty");
                 }
                 else ETGModConsole.Log("Pattern settings null");
                 keepDungeon = null;*/

                ETGMod.StartGlobalCoroutine(this.delayedstarthandler());
                ETGModConsole.Log("'If you're reading this, I must have done something right' - NN");
            }
            catch (Exception e)
            {
                ETGModConsole.Log(e.Message);
                ETGModConsole.Log(e.StackTrace);
            }

        }
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
                SetupCrossModIDs.DoSetup();
                ObsidianPistol.OverridePossibleItems = new List<int>()
                {
                  AWholeBulletKin.WholeBulletKinID,
                  Lewis.LewisID,
                  Kevin.KevinID,
                  234, //Ibomb Companion App
                  201, //Portable Turret
                  338, //Gunther
                  599, //Bubble Blaster
                  563, //The Exotic
                  176, //Gungeon Ant
                  Gunshark.GunsharkID,
                  PoisonDartFrog.PoisonDartFrogID,
                  SporeLauncher.SporeLauncherID,
                  PhaserSpiderling.PhaserSpiderlingID,

                  PrismatismItemIDs.JeremyTheBlobulonID,

                  SomeBunnysItemIDs.BlasphemimicID,
                  SomeBunnysItemIDs.CasemimicID,
                  SomeBunnysItemIDs.GunthemimicID,
                  SomeBunnysItemIDs.GunSoulPhylacteryID,
                  SomeBunnysItemIDs.SoulInAJarID,

                  ExpandTheGungeonIDs.BabyGoodHammerID,
                  ExpandTheGungeonIDs.BabySitterID,

                  RORItemIDs.WillTheWispID,

                  CelsItemIDs.PetRockID,

                  KylesItemIDs.CaptureSphereID,

                  SpecialAPIsStuffIDs.CrownOfTheJammedID,
                  SpecialAPIsStuffIDs.RoundKingID,

                  FallenItemIDs.CircularKingID,
                  FallenItemIDs.DavidID,
                  FallenItemIDs.GunJesterID,
                  FallenItemIDs.JankanID,
                  FallenItemIDs.SpeadCrabID,
                };
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


