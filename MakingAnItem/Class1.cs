using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EnemyAPI;
using GungeonAPI;
using ItemAPI;
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


                //Weird Technical Shit
                GungeonAP.Init();
                FakePrefabHooks.Init();
                ItemBuilder.Init();
                ShrineFactory.Init();
                Tools.Init();
                EnemyTools.Init();
                Hooks.Init();
                EasyVFXDatabase.InitComplexVFX(); //Needs to occur before goop definition
                EasyGoopDefinitions.DefineDefaultGoops();
                DoGoopEffectHook.Init();

                //Null some shit
                for (int i = 0; i < RoomFactory.assetBundles.Length; i++)
                {
                    RoomFactory.assetBundles[i] = null;
                }

                StaticReferences.AssetBundles.Clear();

                ActiveTestingItem.Init();
                PassiveTestingItem.Init();

                //VFX
                LockdownStatusEffect.Initialise();

                //Component Listers
                BulletComponentLister.Init();
                ObjectComponentLister.Init();

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
                SnailBullets.Init();
                BackwardsBullets.Init(); //Unfinished
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
                BalancedBullets.Init();
                WoodenBullets.Init();
                ComicallyGiganticBullets.Init();
                KnightlyBullets.Init();
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
                //Key Themed Items
                BlankKey.Init();
                SharpKey.Init();
                SpareKey.Init();
                KeyChain.Init();
                KeyBullwark.Init();
                KeyBulletEffigy.Init();
                FrostKey.Init();
                //Ammo Box Themed Items
                TheShellactery.Init();
                BloodyAmmo.Init();
                MengerAmmoBox.Init();
                //Boxes and Stuff
                BloodyBox.Init();
                MaidenShapedBox.Init();
                Toolbox.Init();
                //Heart themed items
                HeartPadlock.Init();
                Mutagen.Init();
                ForsakenHeart.Init();
                HeartOfGold.Init();
                GooeyHeart.Init();
                ExaltedHeart.Init();
                CheeseHeart.Init();
                //Chambers
                GlassChamber.Init();
                FlameChamber.Init();
                Recyclinder.Init();
                Nitroglycylinder.Init();
                SpringloadedChamber.Init();
                //Table Techs
                TableTechTable.Init();
                TableTechSpeed.Init();
                TableTechInvulnerability.Init();
                TableTechAmmo.Init();
                TableTechGuon.Init();
                //Guon Stones
                WoodGuonStone.Init();
                YellowGuonStone.Init();
                GreyGuonStone.Init();
                GoldGuonStone.Init();
                BrownGuonStone.Init();
                GuonBoulder.Init();
                BloodglassGuonStone.Init();
                //Ammolets
                GlassAmmolet.Init();
                WickerAmmolet.Init();
                FuriousAmmolet.Init();
                SilverAmmolet.Init();
                Keymmolet.Init();
                // Boots
                CycloneCylinder.Init();
                BootLeg.Init();
                BlankBoots.Init();
                //Bracelets
                DiamondBracelet.Init();
                PearlBracelet.Init();
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
                //Potions / Jars 
                SpeedPotion.Init();
                LovePotion.Init();
                HoneyPot.Init();
                //True Misc
                DaggerOfTheAimgel.Init();
                BookOfMimicAnatomy.Init();
                CaseyMimic.Init(); //Unfinished
                LibationtoIcosahedrax.Init();
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
                ReinforcementRadio.Init();
                GTCWTVRP.Init();
                BlightShell.Init();
                BulletKinPlushie.Init();
                Kevin.Init();
                PurpleProse.Init();
                RustyCasing.Init();
                HikingPack.Init();
                GunpowderPheromones.Init();
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
                Ammolite.Init();
                PortableHole.Init();
                CardinalsMitre.Init();
                GunjurersBelt.Init();
                GoomperorsCrown.Init();
                ChemGrenade.Init();
                Permafrost.Init();
                CombatKnife.Init(); //Unfinished
                AWholeBulletKin.Init();

                //UNFINISHED / TEST GUNS
                WailingMagnum.Add();
                Defender.Add();
                TestGun.Add();
                Blankannon.Add();
                Felissile.Add();
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
                NNGundertale.Add(); //Unfinished
                DiamondGun.Add();
                //GENERAL HANDGUNS
                UnusCentum.Add();
                StunGun.Add();
                Rekeyter.Add();
                HotGlueGun.Add();
                UpNUp.Add();
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
                DisplacerCannon.Add();
                //SCI-FI GUNS
                Blasmaster.Add();
                RocketPistol.Add();
                Purpler.Add();
                VacuumGun.Add();
                Oxygun.Add();
                Demolitionist.Add();
                Multiplicator.Add();
                //ANTIQUES
                TheLodger.Add();
                Gonne.Add();
                Hwacha.Add();
                FireLance.Add();
                HandMortar.Add();
                GrandfatherGlock.Add();
                Blowgun.Add();
                //REALISTIC GUNS
                NNMinigun.Add();
                AntimaterielRifle.Add();
                DartRifle.Add();
                AM0.Add();
                RiskRifle.Add();
                NNBazooka.Add();
                Kalashnirang.Add();
                HeavyAssaultRifle.Add();
                //ANIMAL / ORGANIC GUNS
                SporeLauncher.Add();
                PoisonDartFrog.Add();
                Corgun.Add();
                FungoCannon.Add();
                PhaserSpiderling.Add();
                //FUN GUNS
                PaintballGun.Add();
                Spiral.Add();
                Gunshark.Add();
                FingerGuns.Add();
                GolfRifle.Add();
                DeskFan.Add();
                Pencil.Add();
                //MAGICAL GUNS
                Icicle.Add();
                OrbOfTheGun.Add();
                SpearOfJustice.Add();
                Protean.Add();
                BulletBlade.Add();
                Bookllet.Add();
                Lorebook.Add();
                //ENDPAGE GUNS
                Viscerifle.Add();
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

                //GOOD MIMIC (NEEDS TO BE INITIALISED LATER)
                GoodMimic.Add();

                //Other Features
                MasteryReplacementOub.InitDungeonHook();

                //NPCS
                TheJammomaster.Add();
                ShrineFactory.PlaceBreachShrines();


                //Testing Items
                THECAGE.Init();

                //DoSynergies
                InitialiseSynergies.DoInitialisation();


                SynergyFormInitialiser.AddSynergyForms();
                ExistantGunModifiers.Init();

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


