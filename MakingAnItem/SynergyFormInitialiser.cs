using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ItemAPI;
using UnityEngine;

namespace NevernamedsItems
{
    class SynergyFormInitialiser
    {
        public static void AddSynergyForms()
        {
            //------------------------------------------------------SYNERGY FORMES
            //GUNSHARK - MEGASHARK SYNERGY FORM
            AdvancedTransformGunSynergyProcessor MegaSharkSynergyForme = (PickupObjectDatabase.GetById(Gunshark.GunsharkID) as Gun).gameObject.AddComponent<AdvancedTransformGunSynergyProcessor>();
            MegaSharkSynergyForme.NonSynergyGunId = Gunshark.GunsharkID;
            MegaSharkSynergyForme.SynergyGunId = GunsharkMegasharkSynergyForme.GunsharkMegasharkSynergyFormeID;
            MegaSharkSynergyForme.SynergyToCheck = "Megashark";
            //DISC GUN - SUPER DISC FORM
            AdvancedTransformGunSynergyProcessor SuperDiscSynergyForme = (PickupObjectDatabase.GetById(DiscGun.DiscGunID) as Gun).gameObject.AddComponent<AdvancedTransformGunSynergyProcessor>();
            SuperDiscSynergyForme.NonSynergyGunId = DiscGun.DiscGunID;
            SuperDiscSynergyForme.SynergyGunId = DiscGunSuperDiscForme.DiscGunSuperDiscSynergyFormeID;
            SuperDiscSynergyForme.SynergyToCheck = "Super Disc";
            //ORGUN - HEADACHE FORM
            AdvancedTransformGunSynergyProcessor HeadacheSynergyForme = (PickupObjectDatabase.GetById(Orgun.OrgunID) as Gun).gameObject.AddComponent<AdvancedTransformGunSynergyProcessor>();
            HeadacheSynergyForme.NonSynergyGunId = Orgun.OrgunID;
            HeadacheSynergyForme.SynergyGunId = OrgunHeadacheSynergyForme.OrgunHeadacheSynergyFormeID;
            HeadacheSynergyForme.SynergyToCheck = "Headache";
            //MINI GUN - MINI SHOTGUN FORM
            AdvancedTransformGunSynergyProcessor MiniShotgunSynergyForme = (PickupObjectDatabase.GetById(NNMinigun.MiniGunID) as Gun).gameObject.AddComponent<AdvancedTransformGunSynergyProcessor>();
            MiniShotgunSynergyForme.NonSynergyGunId = NNMinigun.MiniGunID;
            MiniShotgunSynergyForme.SynergyGunId = MinigunMiniShotgunSynergyForme.MiniShotgunID;
            MiniShotgunSynergyForme.SynergyToCheck = "Mini Shotgun";
            //DOGGUN - DISCORD AND RHYME (WOLFGUN) FORME
            AdvancedTransformGunSynergyProcessor WolfgunSynergyForme = (PickupObjectDatabase.GetById(Corgun.DoggunID) as Gun).gameObject.AddComponent<AdvancedTransformGunSynergyProcessor>();
            WolfgunSynergyForme.NonSynergyGunId = Corgun.DoggunID;
            WolfgunSynergyForme.SynergyGunId = Wolfgun.WolfgunID;
            WolfgunSynergyForme.SynergyToCheck = "Discord and Rhyme";
            //-------------------------------------------------------------DUAL WIELDING
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
        }
    }
}
