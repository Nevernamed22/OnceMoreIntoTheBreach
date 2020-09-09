using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EnemyAPI;
using GungeonAPI;
using ItemAPI;

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
                EasyGoopDefinitions.DefineDefaultGoops();
                
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
                //ArmouredArmour.Init(); //Unfinished
                //Consumable Givers
                LooseChange.Init();
                SpaceMetal.Init();
                //Blank Themed Items
                TrueBlank.Init();
                FalseBlank.Init();
                SpareBlank.Init();
                NNBlankPersonality.Init();
                //Blombk.Init();
                //Key Themed Items
                BlankKey.Init();
                SharpKey.Init();
                SpareKey.Init();
                KeyChain.Init();
                KeyBullwark.Init();
                KeyBulletEffigy.Init();
                FrostKey.Init();
                //Boxes and Stuff
                BloodyBox.Init();
                MaidenShapedBox.Init();
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
                // Boots
                CycloneCylinder.Init();
                BootLeg.Init();
                BlankBoots.Init();
                //Bracelets
                DiamondBracelet.Init();
                PearlBracelet.Init();
                //Rings
                RingOfOddlySpecificBenefits.Init();
                //Holsters
                BlackHolster.Init();
                TheBeholster.Init();
                HiveHolster.Init();
                //Companions
                BabyGoodChanceKin.Init();
                //Potions
                SpeedPotion.Init();
                LovePotion.Init();
                //True Misc
                DaggerOfTheAimgel.Init();
                BookOfMimicAnatomy.Init();
                CaseyMimic.Init(); //Unfinished
                LibationtoIcosahedrax.Init();
                BagOfHolding.Init();
                ItemCoupon.Init();
                TimeFuddlersRobe.Init();
                TheShellactery.Init();
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
                GoomperorsCrown.Init();
                Permafrost.Init();
                CombatKnife.Init(); //Unfinished
                NNGundertale.Init(); //Unfinished
                AWholeBulletKin.Init();

                //UNFINISHED / TEST GUNS
                WailingMagnum.Add();
                G20.Add();
                Defender.Add();
                TestGun.Add();
                Felissile.Add();

                //GUNS
                FlayedRevolver.Add();
                RocketPistol.Add();
                TheLodger.Add();
                NNMinigun.Add();
                ElderMagnum.Add();
                PaintballGun.Add();
                OrbOfTheGun.Add();
                AntimaterielRifle.Add();
                NNBazooka.Add();
                Spiral.Add();
                MamaGun.Add();
                Gunshark.Add();
                LovePistol.Add();
                DiscGun.Add();
                JusticeGun.Add();
                FingerGuns.Add();
                Gonne.Add();
                Orgun.Add();
                Octagun.Add();
                Hwacha.Add();
                FireLance.Add();
                HandCannon.Add();
                GolfRifle.Add();
                Purpler.Add();
                HandMortar.Add();
                SpearOfJustice.Add();
                Repeatovolver.Add();
                StunGun.Add();
                Blowgun.Add();
                DiamondGun.Add();
                Lantaka.Add();
                PoisonDartFrog.Add();
                DartRifle.Add();
                GreekFire.Add();
                Kalashnirang.Add();
                SporeLauncher.Add();
                Blasmaster.Add();
                Corgun.Add();
                Wolfgun.Add();
                UpNUp.Add();
                Demolitionist.Add();
                VacuumGun.Add();
                UnusCentum.Add();
                Oxygun.Add();
                Pista.Add();
                ClownShotgun.Add();
                Viscerifle.Add();
                Blankannon.Add();
                MastersGun.Add();

                //SYNERGY FORME GUNS
                GunsharkMegasharkSynergyForme.Add();
                DiscGunSuperDiscForme.Add();
                OrgunHeadacheSynergyForme.Add();
                MinigunMiniShotgunSynergyForme.Add();

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

                ETGModConsole.Log("'If you're reading this, I must have done something right' - NN");
            }
            catch (Exception e)
            {
                ETGModConsole.Log(e.Message);
                ETGModConsole.Log(e.StackTrace);
            }

        }
    }

}


