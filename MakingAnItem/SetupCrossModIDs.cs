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
            foreach (ETGModule mod in ETGMod.AllMods)
            {
                if (mod != null)
                {
                    if (mod.Metadata != null && mod.Metadata.Name != null)
                    {
                        if (mod.Metadata.Name.Contains("Planetside Of Gunymede"))
                        {
                            ModInstallFlags.PlanetsideOfGunymededInstalled = true;
                        }
                        if (mod.Metadata.Name.Contains("FrostAndGunfireItems"))
                        {
                            ModInstallFlags.FrostAndGunfireInstalled = true;
                        }
                        if (mod.Metadata.Name.Contains("Prismatismas"))
                        {
                            ModInstallFlags.PrismatismInstalled = true;
                            if (PickupObjectDatabase.GetByEncounterName("Jeremy the Blobulon")) PrismatismItemIDs.JeremyTheBlobulonID = PickupObjectDatabase.GetByEncounterName("Jeremy the Blobulon").PickupObjectId;
                        }
                        if (mod.Metadata.Name.Contains("SpecialAPI's Stuff"))
                        {
                            ModInstallFlags.SpecialAPIsStuffInstalled = true;
                            if (PickupObjectDatabase.GetByEncounterName("Round King")) SpecialAPIsStuffIDs.RoundKingID = PickupObjectDatabase.GetByEncounterName("Round King").PickupObjectId;
                            if (PickupObjectDatabase.GetByEncounterName("Crown of the Jammed")) SpecialAPIsStuffIDs.CrownOfTheJammedID = PickupObjectDatabase.GetByEncounterName("Crown of the Jammed").PickupObjectId;
                        }
                        if (mod.Metadata.Name.Contains("ExpandTheGungeon"))
                        {
                            ModInstallFlags.ExpandTheGungeonInstalled = true;
                            if (PickupObjectDatabase.GetByEncounterName("Baby Sitter")) ExpandTheGungeonIDs.BabySitterID = PickupObjectDatabase.GetByEncounterName("Baby Sitter").PickupObjectId;
                            if (PickupObjectDatabase.GetByEncounterName("Baby Good Hammer")) ExpandTheGungeonIDs.BabyGoodHammerID = PickupObjectDatabase.GetByEncounterName("Baby Good Hammer").PickupObjectId;
                        }
                        if (mod.Metadata.Name.Contains("Hunter's ror2 items"))
                        {
                            ModInstallFlags.RORItemsInstalled = true;
                            if (PickupObjectDatabase.GetByEncounterName("Will the Wisp")) RORItemIDs.WillTheWispID = PickupObjectDatabase.GetByEncounterName("Will the Wisp").PickupObjectId;
                        }
                        if (mod.Metadata.Name.Contains("CelsItems"))
                        {
                            ModInstallFlags.CelsItemsInstalled = true;
                            if (PickupObjectDatabase.GetByEncounterName("Pet Rock")) CelsItemIDs.PetRockID = ETGMod.Databases.Items["Pet Rock"].PickupObjectId;
                        }
                        if (mod.Metadata.Name.Contains("KTS Item Pack"))
                        {
                            ModInstallFlags.KylesItemsInstalled = true;
                            if (PickupObjectDatabase.GetByEncounterName("Capture Sphere")) KylesItemIDs.CaptureSphereID = PickupObjectDatabase.GetByEncounterName("Capture Sphere").PickupObjectId;
                        }
                        if (mod.Metadata.Name.Contains("Some Bunnys Item Pack"))
                        {
                            ModInstallFlags.SomeBunnysItemsInstalled = true;
                       if (PickupObjectDatabase.GetByEncounterName("Soul In A Jar"))     SomeBunnysItemIDs.SoulInAJarID = PickupObjectDatabase.GetByEncounterName("Soul In A Jar").PickupObjectId;
                            if (PickupObjectDatabase.GetByEncounterName("Gun Soul Phylactery")) SomeBunnysItemIDs.GunSoulPhylacteryID = PickupObjectDatabase.GetByEncounterName("Gun Soul Phylactery").PickupObjectId;
                            if (PickupObjectDatabase.GetByEncounterName("Gunthemimic")) SomeBunnysItemIDs.GunthemimicID = PickupObjectDatabase.GetByEncounterName("Gunthemimic").PickupObjectId;
                            if (PickupObjectDatabase.GetByEncounterName("Casemimic")) SomeBunnysItemIDs.CasemimicID = PickupObjectDatabase.GetByEncounterName("Casemimic").PickupObjectId;
                            if (PickupObjectDatabase.GetByEncounterName("Blasphemimic")) SomeBunnysItemIDs.BlasphemimicID = PickupObjectDatabase.GetByEncounterName("Blasphemimic").PickupObjectId;
                        }
                        if (mod.Metadata.Name.Contains("[Retrash's] Custom Items Collection"))
                        {
                            ModInstallFlags.RetrashItemsInstalled = true;
                        }
                    }
                }
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
