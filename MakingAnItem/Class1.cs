using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EnemyAPI;
using GungeonAPI;
using ItemAPI;
using SaveAPI;
using UnityEngine;

namespace NevernamedsItems
{
    public class Initialisation : ETGModule
    {
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

                //Tools and Toolboxes
                GungeonAP.Init();
                FakePrefabHooks.Init();
                ItemBuilder.Init();
                ShrineFactory.Init();
                Tools.Init();
                EnemyTools.Init();
                Hooks.Init();
                SaveAPIManager.Setup("nn");

                //Hooks n Shit
                PlayerToolsSetup.Init();
                MiscUnlockHooks.InitHooks();
                FloorAndGenerationToolbox.Init();

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
                for (int i = 0; i < RoomFactory.assetBundles.Length; i++)
                {
                    RoomFactory.assetBundles[i] = null;
                }

                StaticReferences.AssetBundles.Clear();

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
                BombardierShells.Init(); //Unfinished
                GildedLead.Init();
                DemoterBullets.Init();
                Voodoollets.Init();
                TracerRound.Init();
                EndlessBullets.Init();
                HellfireRounds.Init();
                Birdshot.Init();
                LockdownBullets.Init();
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
                //Status Effect Bullet Mods
                SnailBullets.Init();
                //Volley Modifying Bullet Mods
                BackwardsBullets.Init();
                CrossBullets.Init();
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
                //Armour
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
                //Boxes and Stuff
                BloodyBox.Init();
                MaidenShapedBox.Init();
                Toolbox.Init();
                PocketChest.Init();
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
                HeavyChamber.Init();
                //Table Techs
                TableTechTable.Init();
                TableTechSpeed.Init();
                TableTechInvulnerability.Init();
                TableTechAmmo.Init();
                TableTechGuon.Init();
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
                RainbowGuonStone.Init();
                GuonBoulder.Init();
                BloodglassGuonStone.Init();
                //Ammolets
                GlassAmmolet.Init();
                WickerAmmolet.Init();
                FuriousAmmolet.Init();
                SilverAmmolet.Init();
                KinAmmolet.Init();
                Autollet.Init();
                Keymmolet.Init();
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
                RingOfAmmoRedemption.Init();
                RiskyRing.Init();
                //Holsters
                BlackHolster.Init();
                TheBeholster.Init();
                HiveHolster.Init();
                //Companions
                BabyGoodChanceKin.Init();
                Potty.Init();
                Peanut.Init();
                Diode.Init();
                DroneCompanion.Init();
                GregTheEgg.Init();
                //Potions / Jars 
                SpeedPotion.Init();
                LovePotion.Init();
                HoneyPot.Init();
                //Remotes
                DoctorsRemote.Init(); //Unfinished
                ReinforcementRadio.Init();
                //Medicine
                BloodThinner.Init();
                //Knives and Blades
                DaggerOfTheAimgel.Init();
                CombatKnife.Init();
                Bayonet.Init();
                //Books
                BookOfMimicAnatomy.Init();
                KalibersPrayer.Init();
                //True Misc
                GoldenAppleCore.Init();
                AppleCore.Init();
                AppleActive.Init();
                CaseyMimic.Init(); //Unfinished
                LibationtoIcosahedrax.Init(); //Unfinished
                BagOfHolding.Init();
                ItemCoupon.Init();
                TimeFuddlersRobe.Init();
                IdentityCrisis.Init();
                LiquidMetalBody.Init();
                KalibersEye.Init();
                GunGrease.Init();
                BomberJacket.Init();
                DragunsScale.Init();
                Wonderchest.Init();
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
                Inevitus.Init(); //Unfinished
                FinishedBullet.Init();
                ChanceKinEffigy.Init();
                MagickeCauldron.Init();
                Bombinomicon.Init();
                ClaySculpture.Init();
                GracefulGoop.Init();
                MrFahrenheit.Init();
                MagicQuiver.Init();
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
                Lefthandedness.Init();
                Eraser.Init();
                LeadOfLife.Init();
                AWholeBulletKin.Init();
                #endregion

                //-----------------------------------------------------GUNS GET INITIALISED
                #region GunInitialisation
                //UNFINISHED / TEST GUNS
                WailingMagnum.Add();
                Defender.Add();
                TestGun.Add();
                Felissile.Add();
                Gunycomb.Add();
                GlobbitSMALL.Add();
                GlobbitMED.Add();
                GlobbitMEGA.Add();
                HeatRay.Add();

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
                //GENERAL HANDGUNS
                Glock42.Add();
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
                HeadOfTheOrder.Add();
                //SHOTGUNS
                JusticeGun.Add();
                Orgun.Add();
                Octagun.Add();
                ClownShotgun.Add();
                Ranger.Add();
                //CANNONS
                HandCannon.Add();
                Lantaka.Add();
                GreekFire.Add();
                EmberCannon.Add();
                DisplacerCannon.Add();
                //SCI-FI GUNS
                Blasmaster.Add();
                Neutrino.Add();
                TheThinLine.Add();
                RocketPistol.Add();
                Repetitron.Add();
                Purpler.Add();
                VacuumGun.Add();
                Oxygun.Add();
                KineticBlaster.Add();
                HighVelocityRifle.Add();
                Demolitionist.Add();
                PumpChargeShotgun.Add();
                Multiplicator.Add();
                PunishmentRay.Add();
                StasisRifle.Add();
                Gravitron.Add();
                GravityGun.Add();
                //BOWS AND CROSSBOWS
                IceBow.Add();
                //ANTIQUES
                WheelLock.Add();
                TheLodger.Add();
                Gonne.Add();
                Hwacha.Add();
                FireLance.Add();
                HandMortar.Add();
                GrandfatherGlock.Add();
                GatlingGun.Add();
                Blowgun.Add();
                //REALISTIC GUNS
                AntimaterielRifle.Add();
                Primos1.Add();
                DartRifle.Add();
                AM0.Add();
                RiskRifle.Add();
                RiotGun.Add();
                Kalashnirang.Add();
                MaidenRifle.Add();
                HeavyAssaultRifle.Add();
                DynamiteLauncher.Add();
                //MISSILE LAUNCHERS
                NNBazooka.Add();
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
                MiniMonger.Add();
                //BLADES
                ButchersKnife.Add();
                MantidAugment.Add();
                RapidRiposte.Add();
                //FUN GUNS
                Gumgun.Add();
                PaintballGun.Add();
                Spiral.Add();
                Gunshark.Add();
                FingerGuns.Add();
                GolfRifle.Add();
                DeskFan.Add();
                Pencil.Add();
                Ringer.Add();
                Snaker.Add();
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
                //CONSUMABLE FIRING GUNS
                Creditor.Add();
                Blankannon.Add();
                Viscerifle.Add();
                //ENDPAGE GUNS
                MastersGun.Add();
                Wrench.Add();


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
                #endregion

                //GOOD MIMIC (NEEDS TO BE INITIALISED LATER)
                GoodMimic.Add();

                //Other Features
                MasteryReplacementOub.InitDungeonHook();
                CadenceAndOxShopPoolAdditions.Init();

                //NPCS
                TheJammomaster.Add();
                ShrineFactory.PlaceBreachShrines();

                //Synergy Setup, Synergy Formes, Dual Wielding, and any changes to Basegame Guns
                InitialiseSynergies.DoInitialisation();
                SynergyFormInitialiser.AddSynergyForms();
                ExistantGunModifiers.Init();

                //Late Hooks
                AmmoPickupHooks.Init();

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


