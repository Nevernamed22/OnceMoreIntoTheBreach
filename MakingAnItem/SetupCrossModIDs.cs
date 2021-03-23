using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NevernamedsItems
{
    class SetupCrossModIDs
    {
        public static void DoSetup()
        {
            if (PickupObjectDatabase.GetByEncounterName("Baby Good Cannon Kin") != null)
            {
                ModInstallFlags.FrostAndGunfireInstalled = true;
            }
            if (PickupObjectDatabase.GetByEncounterName("Daisukes Polymorphine") != null)
            {
                ModInstallFlags.PlanetsideOfGunymededInstalled = true;
            }
            if (PickupObjectDatabase.GetByEncounterName("Jeremy the Blobulon") != null)
            {
                ModInstallFlags.PrismatismInstalled = true;
                PrismatismItemIDs.JeremyTheBlobulonID = PickupObjectDatabase.GetByEncounterName("Jeremy the Blobulon").PickupObjectId;
            }
            if (PickupObjectDatabase.GetByEncounterName("Cyberkinetic Superbattery") != null)
            {
                ModInstallFlags.SpecialAPIsStuffInstalled = true;
                SpecialAPIsStuffIDs.RoundKingID = PickupObjectDatabase.GetByEncounterName("Round King").PickupObjectId;
                SpecialAPIsStuffIDs.CrownOfTheJammedID = PickupObjectDatabase.GetByEncounterName("Crown of the Jammed").PickupObjectId;
            }
            if (PickupObjectDatabase.GetByEncounterName("Baby Good Hammer") != null)
            {
                ModInstallFlags.ExpandTheGungeonInstalled = true;
                ExpandTheGungeonIDs.BabySitterID = PickupObjectDatabase.GetByEncounterName("Baby Sitter").PickupObjectId;
                ExpandTheGungeonIDs.BabyGoodHammerID = PickupObjectDatabase.GetByEncounterName("Baby Good Hammer").PickupObjectId;
            }
            if (PickupObjectDatabase.GetByEncounterName("Aftermarket Missile Launcher") != null)
            {
                ModInstallFlags.RORItemsInstalled = true;
                RORItemIDs.WillTheWispID = PickupObjectDatabase.GetByEncounterName("Will the Wisp").PickupObjectId;
            }
            if (PickupObjectDatabase.GetByEncounterName("Acme Supply Crate") != null)
            {
                ModInstallFlags.CelsItemsInstalled = true;
                CelsItemIDs.PetRockID = ETGMod.Databases.Items["Pet Rock"].PickupObjectId;
            }
            if (PickupObjectDatabase.GetByEncounterName("Ring of Guon Swiftness") != null)
            {
                ModInstallFlags.KylesItemsInstalled = true;
                KylesItemIDs.CaptureSphereID = PickupObjectDatabase.GetByEncounterName("Capture Sphere").PickupObjectId;
            }
            if (PickupObjectDatabase.GetByEncounterName("Reloading Waffle") != null)
            {
                ModInstallFlags.FallenItemsInstalled = true;
                FallenItemIDs.JankanID = PickupObjectDatabase.GetByEncounterName("Jankan").PickupObjectId;
                FallenItemIDs.SpeadCrabID = PickupObjectDatabase.GetByEncounterName("Spead Crab").PickupObjectId;
                FallenItemIDs.DavidID = PickupObjectDatabase.GetByEncounterName("David").PickupObjectId;
                FallenItemIDs.GunJesterID = PickupObjectDatabase.GetByEncounterName("Gun Jester").PickupObjectId;
                FallenItemIDs.CircularKingID = PickupObjectDatabase.GetByEncounterName("Circular King").PickupObjectId;
            }
            if (PickupObjectDatabase.GetByEncounterName("Spirit of Stagnation") != null)
            {
                ModInstallFlags.SomeBunnysItemsInstalled = true;
                SomeBunnysItemIDs.SoulInAJarID = PickupObjectDatabase.GetByEncounterName("Soul In A Jar").PickupObjectId;
                SomeBunnysItemIDs.GunSoulPhylacteryID = PickupObjectDatabase.GetByEncounterName("Gun Soul Phylactery").PickupObjectId;
                SomeBunnysItemIDs.GunthemimicID = PickupObjectDatabase.GetByEncounterName("Gunthemimic").PickupObjectId;
                SomeBunnysItemIDs.CasemimicID = PickupObjectDatabase.GetByEncounterName("Casemimic").PickupObjectId;
                SomeBunnysItemIDs.BlasphemimicID = PickupObjectDatabase.GetByEncounterName("Blasphemimic").PickupObjectId;
            }
            if (PickupObjectDatabase.GetByEncounterName("Blunderbeast Blight") != null)
            {
                ModInstallFlags.RetrashItemsInstalled = true;
            }
        }
    }
    public static class PrismatismItemIDs
    {
        public static int JeremyTheBlobulonID = -1;
    }
    public static class SomeBunnysItemIDs
    {
        public static int SoulInAJarID = -1;
        public static int GunSoulPhylacteryID = -1;
        public static int GunthemimicID = -1;
        public static int CasemimicID = -1;
        public static int BlasphemimicID = -1;
    }
    public static class ExpandTheGungeonIDs
    {
        public static int BabySitterID = -1;
        public static int BabyGoodHammerID = -1;
    }
    public static class RORItemIDs
    {
        public static int WillTheWispID = -1;
    }
    public static class CelsItemIDs
    {
        public static int PetRockID = -1;
    }
    public static class KylesItemIDs
    {
        public static int CaptureSphereID = -1;
    }
    public static class SpecialAPIsStuffIDs
    {
        public static int RoundKingID = -1;
        public static int CrownOfTheJammedID = -1;
    }
    public static class FallenItemIDs
    {
        public static int JankanID = -1;
        public static int SpeadCrabID = -1;
        public static int DavidID = -1;
        public static int GunJesterID = -1;
        public static int CircularKingID = -1;
    }
    public static class ModInstallFlags
    {
        //Simple Content Packs
        public static bool PrismatismInstalled = false;
        public static bool CelsItemsInstalled = false;
        public static bool RetrashItemsInstalled = false;
        public static bool SomeBunnysItemsInstalled = false;
        public static bool FallenItemsInstalled = false;
        public static bool SpecialAPIsStuffInstalled = false;
        //public static bool TitansModInstalled = false;
        public static bool RORItemsInstalled = false;
        public static bool KylesItemsInstalled = false;

        //Major gameplay overhauls
        public static bool FrostAndGunfireInstalled = false;
        public static bool PlanetsideOfGunymededInstalled = false;
        public static bool ExpandTheGungeonInstalled = false;


    }
}
