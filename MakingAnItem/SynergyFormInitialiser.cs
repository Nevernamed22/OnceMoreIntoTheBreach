using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Alexandria.ItemAPI;
using UnityEngine;

namespace NevernamedsItems
{
    class SynergyFormInitialiser
    {
        public static void AddSynergyForms()
        {
            //------------------------------------------------------SYNERGY FORMES
            #region PreBigUpdate
            AddSynergyForm(Gunshark.GunsharkID, GunsharkMegasharkSynergyForme.GunsharkMegasharkSynergyFormeID, "Megashark");
            AddSynergyForm(DiscGun.DiscGunID, DiscGunSuperDiscForme.DiscGunSuperDiscSynergyFormeID, "Super Disc");
            AddSynergyForm(Orgun.OrgunID, OrgunHeadacheSynergyForme.OrgunHeadacheSynergyFormeID, "Headache");
            AddSynergyForm(NNMinigun.MiniGunID, MinigunMiniShotgunSynergyForme.MiniShotgunID, "Mini Shotgun");
            AddSynergyForm(Corgun.DoggunID, Wolfgun.WolfgunID, "Discord and Rhyme");
            #endregion

            #region BigUpdate (1.14)
            AddSynergyForm(Pencil.pencilID, PenPencilSynergy.penID, "Mightier Than The Gun");
            AddSynergyForm(Rekeyter.RekeyterID, ReShelletonKeyter.ReShelletonKeyterID, "ReShelletonKeyter");
            AddSynergyForm(AM0.ID, AM0SpreadForme.AM0SpreadFormeID, "Spreadshot");
            AddSynergyForm(BulletBlade.BulletBladeID, BulletBladeGhostForme.GhostBladeID, "GHOST SWORD!!!");
            AddSynergyForm(HotGlueGun.HotGlueGunID, GlueGunGlueGunnerSynergy.GlueGunnerID, "Glue Gunner");
            AddSynergyForm(Bullatterer.BullattererID, KingBullatterer.KingBullattererID, "King Bullatterer");
            AddSynergyForm(Wrench.WrenchID, WrenchNullRefException.NullWrenchID, "NullReferenceException");
            AddSynergyForm(GravityGun.GravityGunID, GravityGunNegativeMatterForm.GravityGunNegativeMatterID, "Negative Matter");
            AddSynergyForm(GatlingGun.GatlingGunID, GatlingGunGatterUp.GatGunID, "Gatter Up");
            AddSynergyForm(Gonne.GonneID, GonneElder.ElderGonneId, "Discworld");
            #endregion

            #region ShadowsAndSorcery
            AddSynergyForm(UterinePolyp.UterinePolypID, UterinePolypWombular.WombularPolypID, "Wombular");
            AddSynergyForm(Gaxe.ID, DiamondGaxe.ID, "Diamond Gaxe");
            AddSynergyForm(Rebondir.ID, RedRebondir.ID, "Rebondissement");
            AddSynergyForm(DiamondCutter.ID, DiamondCutterRangerClass.ID, "Ranger Class");
            AddSynergyForm(StickGun.ID, StickGunQuickDraw.ID, "Quick, Draw!");
            AddSynergyForm(LightningRod.ID, StormRod.ID, "Storm Rod");
            AddSynergyForm(RustyShotgun.ID, UnrustyShotgun.ID, "Proper Care & Maintenance");
            #endregion

            AddSynergyForm(ARCPistol.ID, DARCPistol.ID, "DARC Pistol");
            AddSynergyForm(ARCRifle.ID, DARCRifle.ID, "DARC Rifle");
            AddSynergyForm(ARCShotgun.ID, DARCShotgun.ID, "DARC Shotgun");
            AddSynergyForm(ARCTactical.ID, DARCTactical.ID, "DARC Tactical");
            AddSynergyForm(ARCCannon.ID, DARCCannon.ID, "DARC Cannon");
            AddSynergyForm(LaundromaterielRifle.ID, Bloodwash.ID, "Bloodwash");
            AddSynergyForm(SalvatorDormus.ID, SalvatorDormusM1893.ID, "M1893");
            AddSynergyForm(Borz.ID, BigBorz.ID, "Big Borz");
            AddSynergyForm(Spitballer.ID, Spitfire.ID, "Spitfire");
            AddSynergyForm(Repeatovolver.RepeatovolverID, RepeatovolverInfinite.ID, "Ad Infinitum");

            AddSwappableSynergyForm(ServiceWeapon.ID, ServiceWeaponShatter.ID, "<shatter/break/unmake>");
            AddSwappableSynergyForm(ServiceWeapon.ID, ServiceWeaponSpin.ID, "<spin/rotate/shred>");
            AddSwappableSynergyForm(ServiceWeapon.ID, ServiceWeaponPierce.ID, "<pierce/penetrate/eviscerate>");
            AddSwappableSynergyForm(ServiceWeapon.ID, ServiceWeaponCharge.ID, "<charge/decimate/kaboom>");

            //-------------------------------------------------------------DUAL WIELDING
            #region Dual Wielding
            //STUN GUN & TRANQ GUN
            AdvancedDualWieldSynergyProcessor StunTranqDualSTUN = (PickupObjectDatabase.GetById(StunGun.StunGunID) as Gun).gameObject.AddComponent<AdvancedDualWieldSynergyProcessor>();
            StunTranqDualSTUN.PartnerGunID = 42;
            StunTranqDualSTUN.SynergyNameToCheck = "Non Lethal Solutions";
            AdvancedDualWieldSynergyProcessor StunTranqDualTRANK = (PickupObjectDatabase.GetById(42) as Gun).gameObject.AddComponent<AdvancedDualWieldSynergyProcessor>();
            StunTranqDualTRANK.PartnerGunID = StunGun.StunGunID;
            StunTranqDualTRANK.SynergyNameToCheck = "Non Lethal Solutions";
            //BLOWGUN & POISON DART FROG
            AdvancedDualWieldSynergyProcessor BlowFrogDualBLOW = (PickupObjectDatabase.GetById(Blowgun.BlowgunID) as Gun).gameObject.AddComponent<AdvancedDualWieldSynergyProcessor>();
            BlowFrogDualBLOW.PartnerGunID = PoisonDartFrog.PoisonDartFrogID;
            BlowFrogDualBLOW.SynergyNameToCheck = "Dartistry";
            AdvancedDualWieldSynergyProcessor BlowFrogDualFROG = (PickupObjectDatabase.GetById(PoisonDartFrog.PoisonDartFrogID) as Gun).gameObject.AddComponent<AdvancedDualWieldSynergyProcessor>();
            BlowFrogDualFROG.PartnerGunID = Blowgun.BlowgunID;
            BlowFrogDualFROG.SynergyNameToCheck = "Dartistry";
            //BOOKLLET & LOREBOOK
            AdvancedDualWieldSynergyProcessor BooklletLorebookDualLORE = (PickupObjectDatabase.GetById(Lorebook.LorebookID) as Gun).gameObject.AddComponent<AdvancedDualWieldSynergyProcessor>();
            BooklletLorebookDualLORE.PartnerGunID = Bookllet.BooklletID;
            BooklletLorebookDualLORE.SynergyNameToCheck = "Librarian";
            AdvancedDualWieldSynergyProcessor BooklletLorebookDualBOOK = (PickupObjectDatabase.GetById(Bookllet.BooklletID) as Gun).gameObject.AddComponent<AdvancedDualWieldSynergyProcessor>();
            BooklletLorebookDualBOOK.PartnerGunID = Lorebook.LorebookID;
            BooklletLorebookDualBOOK.SynergyNameToCheck = "Librarian";
            //WELROD & WELGUN
            AdvancedDualWieldSynergyProcessor WelWelDualROD = (PickupObjectDatabase.GetById(Welrod.WelrodID) as Gun).gameObject.AddComponent<AdvancedDualWieldSynergyProcessor>();
            WelWelDualROD.PartnerGunID = Welgun.WelgunID;
            WelWelDualROD.SynergyNameToCheck = "Wel Wel Wel";
            AdvancedDualWieldSynergyProcessor WelWelDualGUN = (PickupObjectDatabase.GetById(Welgun.WelgunID) as Gun).gameObject.AddComponent<AdvancedDualWieldSynergyProcessor>();
            WelWelDualGUN.PartnerGunID = Welrod.WelrodID;
            WelWelDualGUN.SynergyNameToCheck = "Wel Wel Wel";
            //SHOTGUN WEDDING
            AdvancedDualWieldSynergyProcessor WeddingBride = (PickupObjectDatabase.GetById(TheBride.TheBrideID) as Gun).gameObject.AddComponent<AdvancedDualWieldSynergyProcessor>();
            WeddingBride.PartnerGunID = TheGroom.TheGroomID;
            WeddingBride.SynergyNameToCheck = "Shotgun Wedding";
            AdvancedDualWieldSynergyProcessor WeddingGroom = (PickupObjectDatabase.GetById(TheGroom.TheGroomID) as Gun).gameObject.AddComponent<AdvancedDualWieldSynergyProcessor>();
            WeddingGroom.PartnerGunID = TheBride.TheBrideID;
            WeddingGroom.SynergyNameToCheck = "Shotgun Wedding";
            //SUPER BOUNCE BROS
            AdvancedDualWieldSynergyProcessor SBBRico = (PickupObjectDatabase.GetById(Rico.RicoID) as Gun).gameObject.AddComponent<AdvancedDualWieldSynergyProcessor>();
            SBBRico.PartnerGunID = Rebondir.ID;
            SBBRico.SynergyNameToCheck = "Super Bounce Bros";
            AdvancedDualWieldSynergyProcessor SBBRebondir = (PickupObjectDatabase.GetById(Rebondir.ID) as Gun).gameObject.AddComponent<AdvancedDualWieldSynergyProcessor>();
            SBBRebondir.PartnerGunID = Rico.RicoID;
            SBBRebondir.SynergyNameToCheck = "Super Bounce Bros";

            AddDualWield(Gaxe.ID, TotemOfGundying.ID, "Offhand Immortal");
            #endregion
        }
        public static void AddSynergyForm(int baseGun, int newGun, string synergy)
        {
            (PickupObjectDatabase.GetById(baseGun) as Gun).AddTransformSynergy(newGun, true, synergy, true);
        }
        public static void AddSwappableSynergyForm(int baseGun, int newGun, string synergy)
        {
            (PickupObjectDatabase.GetById(baseGun) as Gun).AddTransformSynergy(newGun, true, synergy, false);
        }
        public static void AddDualWield(int gun1, int gun2, string synergy)
        {
            AdvancedDualWieldSynergyProcessor gun1DUAL = (PickupObjectDatabase.GetById(gun1) as Gun).gameObject.AddComponent<AdvancedDualWieldSynergyProcessor>();
            gun1DUAL.PartnerGunID = gun2;
            gun1DUAL.SynergyNameToCheck = synergy;
            AdvancedDualWieldSynergyProcessor gun2DUAL = (PickupObjectDatabase.GetById(gun2) as Gun).gameObject.AddComponent<AdvancedDualWieldSynergyProcessor>();
            gun2DUAL.PartnerGunID = gun1;
            gun2DUAL.SynergyNameToCheck = synergy;
        }
    }
}
