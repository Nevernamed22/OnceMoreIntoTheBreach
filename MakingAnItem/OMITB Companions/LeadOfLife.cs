using Dungeonator;
using ItemAPI;
using MonoMod.RuntimeDetour;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace NevernamedsItems
{
    public class LeadOfLife : PassiveItem
    {
        #region Vanilla Item Stat Setup
        public static Dictionary<int, string> CompanionItemDictionary;
        public static Dictionary<int, string> SecondaryCompanionDictionary;
        public static LeadOfLifeCompanionStats HotLeadCompanion;
        public static LeadOfLifeCompanionStats IrradiatedLeadCompanion;
        public static LeadOfLifeCompanionStats BatteryBulletsCompanion;
        public static LeadOfLifeCompanionStats PlusOneBulletsCompanion;
        public static LeadOfLifeCompanionStats AngryBulletsCompanion;
        public static LeadOfLifeCompanionStats CursedBulletsCompanion;
        public static LeadOfLifeCompanionStats EasyReloadBulletsCompanion;
        public static LeadOfLifeCompanionStats GhostBulletsCompanion;
        public static LeadOfLifeCompanionStats FlakBulletsCompanion;
        public static LeadOfLifeCompanionStats HeavyBulletsCompanion;
        public static LeadOfLifeCompanionStats KatanaBulletsCompanion;
        public static LeadOfLifeCompanionStats RemoteBulletsCompanion;
        public static LeadOfLifeCompanionStats BouncyBulletsCompanion;
        public static LeadOfLifeCompanionStats SilverBulletsCompanion;
        public static LeadOfLifeCompanionStats ZombieBulletsCompanion;
        public static LeadOfLifeCompanionStats Bloody9mmCompanion;
        public static LeadOfLifeCompanionStats BumbulletsCompanion;
        public static LeadOfLifeCompanionStats ChanceBulletsCompanion;
        public static LeadOfLifeCompanionStats CharmingRoundsCompanion;
        public static LeadOfLifeCompanionStats DevolverRoundsCompanion;
        public static LeadOfLifeCompanionStats GildedBulletsCompanion;
        public static LeadOfLifeCompanionStats HelixBulletsCompanion;
        public static LeadOfLifeCompanionStats HomingBulletsCompanion;
        public static LeadOfLifeCompanionStats MagicBulletsCompanion;
        public static LeadOfLifeCompanionStats RocketPoweredBulletsCompanion;
        public static LeadOfLifeCompanionStats ScattershotCompanion;
        public static LeadOfLifeCompanionStats ShadowBulletsCompanion;
        public static LeadOfLifeCompanionStats StoutBulletsCompanion;
        public static LeadOfLifeCompanionStats AlphaBulletsCompanion;
        public static LeadOfLifeCompanionStats OmegaBulletsCompanion;
        public static LeadOfLifeCompanionStats ChaosBulletsCompanion;
        public static LeadOfLifeCompanionStats ExplosiveRoundsCompanion;
        public static LeadOfLifeCompanionStats FatBulletsCompanion;
        public static LeadOfLifeCompanionStats FrostBulletsCompanion;
        public static LeadOfLifeCompanionStats HungryBulletsCompanion;
        public static LeadOfLifeCompanionStats OrbitalBulletsCompanion;
        public static LeadOfLifeCompanionStats ShockRoundsCompanion;
        public static LeadOfLifeCompanionStats SnowballetsCompanion;
        public static LeadOfLifeCompanionStats VorpalBulletsCompanion;
        public static LeadOfLifeCompanionStats BlankBulletsCompanion;
        public static LeadOfLifeCompanionStats PlatinumBulletsCompanion;
        public static LeadOfLifeCompanionStats LichsEyeBulletsCompanionA;
        public static LeadOfLifeCompanionStats LichsEyeBulletsCompanionB;
        public static LeadOfLifeCompanionStats BulletTimeCompanion;
        public static LeadOfLifeCompanionStats DarumaCompanion;
        public static LeadOfLifeCompanionStats RiddleOfLeadCompanion;
        public static LeadOfLifeCompanionStats ShotgunCoffeeCompanion;
        public static LeadOfLifeCompanionStats ShotgaColaCompanion;
        public static LeadOfLifeCompanionStats ElderBlankCompanion;
        public static LeadOfLifeCompanionStats BulletGunCompanion;
        public static LeadOfLifeCompanionStats ShellGunCompanion;
        public static LeadOfLifeCompanionStats CaseyCompanion;
        public static LeadOfLifeCompanionStats BTCKTPCompanion;
        #endregion

        #region Modded Item Stat Setup
        public static LeadOfLifeCompanionStats OneShotCompanion;
        public static LeadOfLifeCompanionStats FiftyCalRoundsCompanion;
        public static LeadOfLifeCompanionStats AlkaliBulletsCompanion;
        public static LeadOfLifeCompanionStats AntimagicRoundsCompanion;
        public static LeadOfLifeCompanionStats AntimatterBulletsCompanion;
        public static LeadOfLifeCompanionStats BashfulShotCompanion;
        public static LeadOfLifeCompanionStats BashingBulletsCompanion;

        public static LeadOfLifeCompanionStats BirdshotCompanion;
        public static LeadOfLifeCompanionStats BlightShellCompanion;
        public static LeadOfLifeCompanionStats BloodthirstyBulletsCompanion;



        #endregion



        public static void Init()
        {
            try
            {
                HotLeadCompanion = new LeadOfLifeCompanionStats();
                IrradiatedLeadCompanion = new LeadOfLifeCompanionStats();
                BatteryBulletsCompanion = new LeadOfLifeCompanionStats();
                PlusOneBulletsCompanion = new LeadOfLifeCompanionStats();
                AngryBulletsCompanion = new LeadOfLifeCompanionStats();
                CursedBulletsCompanion = new LeadOfLifeCompanionStats();
                EasyReloadBulletsCompanion = new LeadOfLifeCompanionStats();
                GhostBulletsCompanion = new LeadOfLifeCompanionStats();
                FlakBulletsCompanion = new LeadOfLifeCompanionStats();
                HeavyBulletsCompanion = new LeadOfLifeCompanionStats();
                KatanaBulletsCompanion = new LeadOfLifeCompanionStats();
                RemoteBulletsCompanion = new LeadOfLifeCompanionStats();
                BouncyBulletsCompanion = new LeadOfLifeCompanionStats();
                SilverBulletsCompanion = new LeadOfLifeCompanionStats();
                ZombieBulletsCompanion = new LeadOfLifeCompanionStats();
                Bloody9mmCompanion = new LeadOfLifeCompanionStats();
                BumbulletsCompanion = new LeadOfLifeCompanionStats();
                ChanceBulletsCompanion = new LeadOfLifeCompanionStats();
                CharmingRoundsCompanion = new LeadOfLifeCompanionStats();
                DevolverRoundsCompanion = new LeadOfLifeCompanionStats();
                GildedBulletsCompanion = new LeadOfLifeCompanionStats();
                HelixBulletsCompanion = new LeadOfLifeCompanionStats();
                HomingBulletsCompanion = new LeadOfLifeCompanionStats();
                MagicBulletsCompanion = new LeadOfLifeCompanionStats();
                RocketPoweredBulletsCompanion = new LeadOfLifeCompanionStats();
                ScattershotCompanion = new LeadOfLifeCompanionStats();
                ShadowBulletsCompanion = new LeadOfLifeCompanionStats();
                StoutBulletsCompanion = new LeadOfLifeCompanionStats();
                AlphaBulletsCompanion = new LeadOfLifeCompanionStats();
                OmegaBulletsCompanion = new LeadOfLifeCompanionStats();
                ChaosBulletsCompanion = new LeadOfLifeCompanionStats();
                ExplosiveRoundsCompanion = new LeadOfLifeCompanionStats();
                FatBulletsCompanion = new LeadOfLifeCompanionStats();
                FrostBulletsCompanion = new LeadOfLifeCompanionStats();
                HungryBulletsCompanion = new LeadOfLifeCompanionStats();
                OrbitalBulletsCompanion = new LeadOfLifeCompanionStats();
                ShockRoundsCompanion = new LeadOfLifeCompanionStats();
                SnowballetsCompanion = new LeadOfLifeCompanionStats();
                VorpalBulletsCompanion = new LeadOfLifeCompanionStats();
                BlankBulletsCompanion = new LeadOfLifeCompanionStats();
                PlatinumBulletsCompanion = new LeadOfLifeCompanionStats();
                LichsEyeBulletsCompanionA = new LeadOfLifeCompanionStats();
                LichsEyeBulletsCompanionB = new LeadOfLifeCompanionStats();
                BulletTimeCompanion = new LeadOfLifeCompanionStats();
                DarumaCompanion = new LeadOfLifeCompanionStats();
                RiddleOfLeadCompanion = new LeadOfLifeCompanionStats();
                ShotgunCoffeeCompanion = new LeadOfLifeCompanionStats();
                ShotgaColaCompanion = new LeadOfLifeCompanionStats();
                ElderBlankCompanion = new LeadOfLifeCompanionStats();
                BulletGunCompanion = new LeadOfLifeCompanionStats();
                ShellGunCompanion = new LeadOfLifeCompanionStats();
                CaseyCompanion = new LeadOfLifeCompanionStats();
                BTCKTPCompanion = new LeadOfLifeCompanionStats();

                OneShotCompanion = new LeadOfLifeCompanionStats();
                FiftyCalRoundsCompanion = new LeadOfLifeCompanionStats();
                AlkaliBulletsCompanion = new LeadOfLifeCompanionStats();
                AntimagicRoundsCompanion = new LeadOfLifeCompanionStats();
                AntimatterBulletsCompanion = new LeadOfLifeCompanionStats();
                BashfulShotCompanion = new LeadOfLifeCompanionStats();
                BashingBulletsCompanion = new LeadOfLifeCompanionStats();
                BirdshotCompanion = new LeadOfLifeCompanionStats();
                BlightShellCompanion = new LeadOfLifeCompanionStats();
                BloodthirstyBulletsCompanion = new LeadOfLifeCompanionStats();





                HotLeadCompanion.guid = "leadoflife_hotlead";
                IrradiatedLeadCompanion.guid = "leadoflife_irradiatedlead";
                BatteryBulletsCompanion.guid = "leadoflife_batterybullets";
                PlusOneBulletsCompanion.guid = "leadoflife_plusonebullets";
                AngryBulletsCompanion.guid = "leadoflife_angrybullets";
                CursedBulletsCompanion.guid = "leadoflife_cursedbullets";
                EasyReloadBulletsCompanion.guid = "leadoflife_easyreloadbullets";
                GhostBulletsCompanion.guid = "leadoflife_ghostbullets";
                FlakBulletsCompanion.guid = "leadoflife_flakbullets";
                HeavyBulletsCompanion.guid = "leadoflife_heavybullets";
                KatanaBulletsCompanion.guid = "leadoflife_katanabullets";
                RemoteBulletsCompanion.guid = "leadoflife_remotebullets";
                BouncyBulletsCompanion.guid = "leadoflife_bouncybullets";
                SilverBulletsCompanion.guid = "leadoflife_silverbullets";
                ZombieBulletsCompanion.guid = "leadoflife_zombiebullets";
                Bloody9mmCompanion.guid = "leadoflife_bloody9mm";
                BumbulletsCompanion.guid = "leadoflife_bumbullets";
                ChanceBulletsCompanion.guid = "leadoflife_chancebullets";
                CharmingRoundsCompanion.guid = "leadoflife_charmingrounds";
                DevolverRoundsCompanion.guid = "leadoflife_devolverrounds";
                GildedBulletsCompanion.guid = "leadoflife_gildedbullets";
                HelixBulletsCompanion.guid = "leadoflife_helixbullets";
                HomingBulletsCompanion.guid = "leadoflife_homingbullets";
                MagicBulletsCompanion.guid = "leadoflife_magicbullets";
                RocketPoweredBulletsCompanion.guid = "leadoflife_rocketpoweredbullets";
                ScattershotCompanion.guid = "leadoflife_scattershot";
                ShadowBulletsCompanion.guid = "leadoflife_shadowbullets";
                StoutBulletsCompanion.guid = "leadoflife_stoutbullets";
                AlphaBulletsCompanion.guid = "leadoflife_alphabullets";
                OmegaBulletsCompanion.guid = "leadoflife_omegabullets";
                ChaosBulletsCompanion.guid = "leadoflife_chaosbullets";
                ExplosiveRoundsCompanion.guid = "leadoflife_explosiverounds";
                FatBulletsCompanion.guid = "leadoflife_fatbullets";
                FrostBulletsCompanion.guid = "leadoflife_frostbullets";
                HungryBulletsCompanion.guid = "leadoflife_hungrybullets";
                OrbitalBulletsCompanion.guid = "leadoflife_orbitalbullets";
                ShockRoundsCompanion.guid = "leadoflife_shockrounds";
                SnowballetsCompanion.guid = "leadoflife_snowballets";
                VorpalBulletsCompanion.guid = "leadoflife_vorpalbullets";
                BlankBulletsCompanion.guid = "leadoflife_blankbullets";
                PlatinumBulletsCompanion.guid = "leadoflife_platinumbullets";
                LichsEyeBulletsCompanionA.guid = "leadoflife_lichseyebullets_a";
                LichsEyeBulletsCompanionB.guid = "leadoflife_lichseyebullets_b";
                BulletTimeCompanion.guid = "leadoflife_bullettime";
                DarumaCompanion.guid = "leadoflife_daruma";
                RiddleOfLeadCompanion.guid = "leadoflife_riddleoflead";
                ShotgunCoffeeCompanion.guid = "leadoflife_shotguncoffee";
                ShotgaColaCompanion.guid = "leadoflife_shotgacola";
                ElderBlankCompanion.guid = "leadoflife_elderblank";
                BulletGunCompanion.guid = "leadoflife_bulletgun";
                ShellGunCompanion.guid = "leadoflife_shellgun";
                CaseyCompanion.guid = "leadoflife_casey";
                BTCKTPCompanion.guid = "leadoflife_btcktp";

                OneShotCompanion.guid = "leadoflife_oneshot";
                FiftyCalRoundsCompanion.guid = "leadoflife_fiftycalrounds";
                AlkaliBulletsCompanion.guid = "leadoflife_alkalibullets";
                AntimagicRoundsCompanion.guid = "leadoflife_antimagicrounds";
                AntimatterBulletsCompanion.guid = "leadoflife_antimatterbullets";
                BashfulShotCompanion.guid = "leadoflife_bashfulshot";
                BashingBulletsCompanion.guid = "leadoflife_bashingbullets";

                BirdshotCompanion.guid = "leadoflife_birdshot";
                BlightShellCompanion.guid = "leadoflife_blightshell";
                BloodthirstyBulletsCompanion.guid = "leadoflife_bloodthirstybullets";



                CompanionItemDictionary = new Dictionary<int, string>()
                {
                    {295, HotLeadCompanion.guid},
                    {204, IrradiatedLeadCompanion.guid},
                    {410, BatteryBulletsCompanion.guid},
                    {286, PlusOneBulletsCompanion.guid},
                    {323, AngryBulletsCompanion.guid},
                    {571, CursedBulletsCompanion.guid},
                    {375, EasyReloadBulletsCompanion.guid},
                    {172,GhostBulletsCompanion.guid},
                    {531,FlakBulletsCompanion.guid},
                    {111,HeavyBulletsCompanion.guid},
                    {822,KatanaBulletsCompanion.guid},
                    {530,RemoteBulletsCompanion.guid},
                    {288,BouncyBulletsCompanion.guid},
                    {538,SilverBulletsCompanion.guid},
                    {528,ZombieBulletsCompanion.guid},
                    {524,Bloody9mmCompanion.guid},
                    {630,BumbulletsCompanion.guid},
                    {521,ChanceBulletsCompanion.guid},
                    {527,CharmingRoundsCompanion.guid},
                    {638,DevolverRoundsCompanion.guid},
                    {532,GildedBulletsCompanion.guid},
                    {568,HelixBulletsCompanion.guid},
                    {284,HomingBulletsCompanion.guid},
                    {533,MagicBulletsCompanion.guid},
                    {113,RocketPoweredBulletsCompanion.guid},
                    {241,ScattershotCompanion.guid},
                    {352,ShadowBulletsCompanion.guid},
                    {523,StoutBulletsCompanion.guid},
                    {373,AlphaBulletsCompanion.guid},
                    {374,OmegaBulletsCompanion.guid},
                    {569,ChaosBulletsCompanion.guid},
                    {304,ExplosiveRoundsCompanion.guid},
                    {277,FatBulletsCompanion.guid},
                    {278,FrostBulletsCompanion.guid},
                    {655,HungryBulletsCompanion.guid},
                    {661,OrbitalBulletsCompanion.guid},
                    {298,ShockRoundsCompanion.guid},
                    {636,SnowballetsCompanion.guid},
                    {640,VorpalBulletsCompanion.guid},
                    {579,BlankBulletsCompanion.guid},
                    {627,PlatinumBulletsCompanion.guid},
                    {815,LichsEyeBulletsCompanionA.guid},
                    {69,BulletTimeCompanion.guid},
                    {643,DarumaCompanion.guid},
                    {271,RiddleOfLeadCompanion.guid},
                    {427,ShotgunCoffeeCompanion.guid},
                    {426,ShotgaColaCompanion.guid},
                    {499,ElderBlankCompanion.guid},
                    {503,BulletGunCompanion.guid},
                    {512,ShellGunCompanion.guid},
                    {541,CaseyCompanion.guid},
                    {303,BTCKTPCompanion.guid},

                    //MODDED
                    {OneShot.OneShotID, OneShotCompanion.guid},
                    {FiftyCalRounds.FiftyCalRoundsID, FiftyCalRoundsCompanion.guid},
                    {AlkaliBullets.AlkaliBulletsID, AlkaliBulletsCompanion.guid},
                    {AntimagicRounds.AntimagicRoundsID, AntimagicRoundsCompanion.guid},
                    {AntimatterBullets.AntimatterBulletsID, AntimatterBulletsCompanion.guid},
                    {BashfulShot.BashfulShotID, BashfulShotCompanion.guid},
                    {BashingBullets.BashingBulletsID, BashingBulletsCompanion.guid},
                    {Birdshot.BirdshotID, BirdshotCompanion.guid},
                    {BlightShell.BlightShellID, BlightShellCompanion.guid},
                    {BloodthirstyBullets.BloodthirstyBulletsID, BloodthirstyBulletsCompanion.guid},






                };

                SecondaryCompanionDictionary = new Dictionary<int, string>()
                {
                    {815,LichsEyeBulletsCompanionB.guid},
                };

                string name = "Lead of Life";
                string resourcePath = "NevernamedsItems/Resources/Companions/LeadOfLife/leadoflife_icon";
                GameObject gameObject = new GameObject(name);
                var item = gameObject.AddComponent<LeadOfLife>();
                ItemBuilder.AddSpriteToObject(name, resourcePath, gameObject);
                string shortDesc = "Forged Friends";
                string longDesc = "Brings bullet upgrades to life!" + "\n\nA tiny fragment of lead left over from the creation of the very first Bullet Kin." + "\n\nIt still glows with lifegiving power...";
                item.quality = PickupObject.ItemQuality.S;
                item.SetupItem(shortDesc, longDesc, "nn");

                LeadOfLifeID = item.PickupObjectId;

                activeItemDropHook = new Hook(
                    typeof(PlayerController).GetMethod("DropActiveItem"),
                    typeof(LeadOfLife).GetMethod("DropActiveHook")
                );

                // AssignGuids();
                BuildPrefabs();
            }
            catch (Exception e)
            {
                ETGModConsole.Log(e.Message);
                ETGModConsole.Log(e.StackTrace);
            }
        }
        public static int LeadOfLifeID;
        public static Hook activeItemDropHook;
        public static DebrisObject DropActiveHook(Func<PlayerController, PlayerItem, float, bool, DebrisObject> orig, PlayerController self, PlayerItem item, float force = 4f, bool deathdrop = false)
        {
            try
            {
                if (GameManager.Instance.PrimaryPlayer && GameManager.Instance.PrimaryPlayer.HasPickupID(LeadOfLife.LeadOfLifeID))
                {
                    foreach (PassiveItem LeadOfLife in GameManager.Instance.PrimaryPlayer.passiveItems)
                    {
                        if (LeadOfLife.GetComponent<LeadOfLife>())
                        {
                            LeadOfLife.GetComponent<LeadOfLife>().DoLateRecalculation();
                        }
                    }
                }
                if (GameManager.Instance.SecondaryPlayer && GameManager.Instance.SecondaryPlayer.HasPickupID(LeadOfLife.LeadOfLifeID))
                {
                    foreach (PassiveItem LeadOfLife in GameManager.Instance.SecondaryPlayer.passiveItems)
                    {
                        if (LeadOfLife.GetComponent<LeadOfLife>())
                        {
                            LeadOfLife.GetComponent<LeadOfLife>().DoLateRecalculation();
                        }
                    }
                }
                return orig(self, item, force, deathdrop);
            }
            catch (Exception e)
            {
                ETGModConsole.Log(e.Message);
                ETGModConsole.Log(e.StackTrace);
                return null;
            }
        }

        public int AmountOfEasyReloadCompanions;
        public int AmountOfHardReloadCompanions;
        public int PlatBulletsLanded = 0;
        public List<LeadOfLifeCompanion> extantCompanions = new List<LeadOfLifeCompanion>();
        public override void Pickup(PlayerController player)
        {
            if (!m_pickedUpThisRun) PlatBulletsLanded = 0;
            base.Pickup(player);
            RecalculateCompanions();
            player.OnNewFloorLoaded += this.OnNewFloor;
        }
        public override DebrisObject Drop(PlayerController player)
        {
            DestroyAllCompanions();
            player.OnNewFloorLoaded -= this.OnNewFloor;
            return base.Drop(player);
        }
        protected override void OnDestroy()
        {
            if (Owner)
            {
                DestroyAllCompanions();
                Owner.OnNewFloorLoaded -= this.OnNewFloor;
            }
            base.OnDestroy();
        }
        public void DoLateRecalculation()
        {
            StartCoroutine(LateRecalculation());
        }
        private IEnumerator LateRecalculation()
        {
            yield return null;
            RecalculateCompanions();
            yield break;
        }
        public void RecalculateCompanions()
        {
            DestroyAllCompanions();
            foreach (PassiveItem item in Owner.passiveItems)
            {
                if (CompanionItemDictionary.ContainsKey(item.PickupObjectId))
                {
                    if (item.PickupObjectId == 375) AmountOfEasyReloadCompanions += 1;
                    SpawnNewCompanion(CompanionItemDictionary[item.PickupObjectId]);
                }
                if (SecondaryCompanionDictionary.ContainsKey(item.PickupObjectId))
                {
                    if (item.PickupObjectId == 375) AmountOfEasyReloadCompanions += 1;
                    SpawnNewCompanion(SecondaryCompanionDictionary[item.PickupObjectId]);
                }
            }
            foreach (PlayerItem item in Owner.activeItems)
            {
                if (CompanionItemDictionary.ContainsKey(item.PickupObjectId))
                {
                    if (item.PickupObjectId == 375) AmountOfEasyReloadCompanions += 1;
                    SpawnNewCompanion(CompanionItemDictionary[item.PickupObjectId]);
                }
                if (SecondaryCompanionDictionary.ContainsKey(item.PickupObjectId))
                {
                    if (item.PickupObjectId == 375) AmountOfEasyReloadCompanions += 1;
                    SpawnNewCompanion(SecondaryCompanionDictionary[item.PickupObjectId]);
                }
            }
            foreach (Gun item in Owner.inventory.AllGuns)
            {
                if (CompanionItemDictionary.ContainsKey(item.PickupObjectId))
                {
                    if (item.PickupObjectId == 375) AmountOfEasyReloadCompanions += 1;
                    SpawnNewCompanion(CompanionItemDictionary[item.PickupObjectId]);
                }
                if (SecondaryCompanionDictionary.ContainsKey(item.PickupObjectId))
                {
                    if (item.PickupObjectId == 375) AmountOfEasyReloadCompanions += 1;
                    SpawnNewCompanion(SecondaryCompanionDictionary[item.PickupObjectId]);
                }
            }
        }
        private void OnNewFloor(PlayerController player) { RecalculateCompanions(); }
        private void SpawnNewCompanion(string guid)
        {
            AIActor orLoadByGuid = EnemyDatabase.GetOrLoadByGuid(guid);
            Vector3 vector = Owner.transform.position;
            if (GameManager.Instance.CurrentLevelOverrideState == GameManager.LevelOverrideState.FOYER)
            {
                vector += new Vector3(1.125f, -0.3125f, 0f);
            }
            GameObject extantCompanion2 = UnityEngine.Object.Instantiate<GameObject>(orLoadByGuid.gameObject, vector, Quaternion.identity);
            LeadOfLifeCompanion orAddComponent = extantCompanion2.GetOrAddComponent<LeadOfLifeCompanion>();
            extantCompanions.Add(orAddComponent);
            orAddComponent.Initialize(Owner);
            if (orAddComponent.specRigidbody)
            {
                PhysicsEngine.Instance.RegisterOverlappingGhostCollisionExceptions(orAddComponent.specRigidbody, null, false);
            }
        }
        private int currentItems, lastItems;
        protected override void Update()
        {
            if (!Dungeon.IsGenerating)
            {
                if (Owner)
                {
                    currentItems = (Owner.passiveItems.Count + Owner.activeItems.Count + Owner.inventory.AllGuns.Count);
                    if (currentItems != lastItems)
                    {
                        RecalculateCompanions();
                        lastItems = currentItems;
                    }
                }
            }
            base.Update();
        }
        private void DestroyAllCompanions()
        {
            AmountOfEasyReloadCompanions = 0;
            AmountOfHardReloadCompanions = 0;
            if (extantCompanions.Count <= 0) { return; }
            for (int i = extantCompanions.Count - 1; i >= 0; i--)
            {
                if (extantCompanions[i] && extantCompanions[i].gameObject)
                {

                    UnityEngine.Object.Destroy(extantCompanions[i].gameObject);
                }
            }
            extantCompanions.Clear();
        }

        public static void BuildPrefabs()
        {
            try
            {
                #region Bullet Modifiers
                // ETGModConsole.Log("Section 1");
                if (HotLeadCompanion.prefab == null || !CompanionBuilder.companionDictionary.ContainsKey(HotLeadCompanion.guid))
                {
                    HotLeadCompanion.prefab = CompanionBuilder.BuildPrefab("LeadOfLife HotLead", HotLeadCompanion.guid, "NevernamedsItems/Resources/Companions/LeadOfLife/hotleadcompanion_idle_001", new IntVector2(6, 1), new IntVector2(7, 7));
                    var companionController = HotLeadCompanion.prefab.AddComponent<LeadOfLifeCompanion>();
                    companionController.aiActor.MovementSpeed = 5f;
                    companionController.tiedItemID = 295;
                    companionController.gunIDToCopyBulletsFrom = 336;
                    companionController.bulletScaleMultiplier = 0.5f;
                    companionController.fireCooldown = 2.5f;
                    companionController.canIgniteGoop = true;
                    HotLeadCompanion.prefab.AddAnimation("idle", "NevernamedsItems/Resources/Companions/LeadOfLife/hotleadcompanion_idle", 7, CompanionBuilder.AnimationType.Idle, DirectionalAnimation.DirectionType.Single, DirectionalAnimation.FlipType.None);
                    HotLeadCompanion.prefab.AddAnimation("run", "NevernamedsItems/Resources/Companions/LeadOfLife/hotleadcompanion_run", 7, CompanionBuilder.AnimationType.Move, DirectionalAnimation.DirectionType.Single, DirectionalAnimation.FlipType.None);
                    companionController.aiAnimator.GetDirectionalAnimation("idle").Prefix = "idle";
                    companionController.aiAnimator.GetDirectionalAnimation("move").Prefix = "run";
                    BehaviorSpeculator component = HotLeadCompanion.prefab.GetComponent<BehaviorSpeculator>();
                    component.MovementBehaviors.Add(new CustomCompanionBehaviours.LeadOfLifeCompanionApproach());
                    component.MovementBehaviors.Add(new CompanionFollowPlayerBehavior { IdleAnimations = new string[] { "idle" } });

                }
                ///ETGModConsole.Log("Section 2");
                if (IrradiatedLeadCompanion.prefab == null || !CompanionBuilder.companionDictionary.ContainsKey(IrradiatedLeadCompanion.guid))
                {
                    IrradiatedLeadCompanion.prefab = CompanionBuilder.BuildPrefab("LeadOfLife IrradiatedLead", IrradiatedLeadCompanion.guid, "NevernamedsItems/Resources/Companions/LeadOfLife/irradiatedleadcompanion_idle_001", new IntVector2(6, 1), new IntVector2(7, 7));
                    var companionController = IrradiatedLeadCompanion.prefab.AddComponent<LeadOfLifeCompanion>();
                    companionController.aiActor.MovementSpeed = 5f;
                    companionController.tiedItemID = 204;
                    companionController.inflictsPoison = true;
                    companionController.tintsBullets = true;
                    companionController.tintColour = ExtendedColours.poisonGreen;
                    IrradiatedLeadCompanion.prefab.AddAnimation("idle", "NevernamedsItems/Resources/Companions/LeadOfLife/irradiatedleadcompanion_idle", 7, CompanionBuilder.AnimationType.Idle, DirectionalAnimation.DirectionType.Single, DirectionalAnimation.FlipType.None);
                    IrradiatedLeadCompanion.prefab.AddAnimation("run", "NevernamedsItems/Resources/Companions/LeadOfLife/irradiatedleadcompanion_run", 7, CompanionBuilder.AnimationType.Move, DirectionalAnimation.DirectionType.Single, DirectionalAnimation.FlipType.None);
                    companionController.aiAnimator.GetDirectionalAnimation("idle").Prefix = "idle";
                    companionController.aiAnimator.GetDirectionalAnimation("move").Prefix = "run";
                    BehaviorSpeculator component = IrradiatedLeadCompanion.prefab.GetComponent<BehaviorSpeculator>();
                    component.MovementBehaviors.Add(new CustomCompanionBehaviours.LeadOfLifeCompanionApproach());
                    component.MovementBehaviors.Add(new CompanionFollowPlayerBehavior { IdleAnimations = new string[] { "idle" } });
                }
                if (BatteryBulletsCompanion.prefab == null || !CompanionBuilder.companionDictionary.ContainsKey(BatteryBulletsCompanion.guid))
                {
                    BatteryBulletsCompanion.prefab = CompanionBuilder.BuildPrefab("LeadOfLife BatteryBullets", BatteryBulletsCompanion.guid, "NevernamedsItems/Resources/Companions/LeadOfLife/batterybulletscompanion_idle_001", new IntVector2(6, 1), new IntVector2(7, 7));
                    var companionController = BatteryBulletsCompanion.prefab.AddComponent<LeadOfLifeCompanion>();
                    companionController.aiActor.MovementSpeed = 5f;
                    companionController.tiedItemID = 410;
                    companionController.hasElectricBullets = true;
                    companionController.angleVariance = 2f;
                    BatteryBulletsCompanion.prefab.AddAnimation("idle", "NevernamedsItems/Resources/Companions/LeadOfLife/batterybulletscompanion_idle", 7, CompanionBuilder.AnimationType.Idle, DirectionalAnimation.DirectionType.Single, DirectionalAnimation.FlipType.None);
                    BatteryBulletsCompanion.prefab.AddAnimation("run", "NevernamedsItems/Resources/Companions/LeadOfLife/batterybulletscompanion_run", 7, CompanionBuilder.AnimationType.Move, DirectionalAnimation.DirectionType.Single, DirectionalAnimation.FlipType.None);
                    companionController.aiAnimator.GetDirectionalAnimation("idle").Prefix = "idle";
                    companionController.aiAnimator.GetDirectionalAnimation("move").Prefix = "run";
                    BehaviorSpeculator component = BatteryBulletsCompanion.prefab.GetComponent<BehaviorSpeculator>();
                    component.MovementBehaviors.Add(new CustomCompanionBehaviours.LeadOfLifeCompanionApproach());
                    component.MovementBehaviors.Add(new CompanionFollowPlayerBehavior { IdleAnimations = new string[] { "idle" } });
                }
                if (PlusOneBulletsCompanion.prefab == null || !CompanionBuilder.companionDictionary.ContainsKey(PlusOneBulletsCompanion.guid))
                {
                    PlusOneBulletsCompanion.prefab = CompanionBuilder.BuildPrefab("LeadOfLife PlusOneBullets", PlusOneBulletsCompanion.guid, "NevernamedsItems/Resources/Companions/LeadOfLife/plusonebulletscompanion_idle_001", new IntVector2(7, 1), new IntVector2(7, 7));
                    var companionController = PlusOneBulletsCompanion.prefab.AddComponent<LeadOfLifeCompanion>();
                    companionController.aiActor.MovementSpeed = 5f;
                    companionController.damageMultiplier = 1.25f;
                    companionController.tiedItemID = 286;
                    PlusOneBulletsCompanion.prefab.AddAnimation("idle", "NevernamedsItems/Resources/Companions/LeadOfLife/plusonebulletscompanion_idle", 7, CompanionBuilder.AnimationType.Idle, DirectionalAnimation.DirectionType.Single, DirectionalAnimation.FlipType.None);
                    PlusOneBulletsCompanion.prefab.AddAnimation("run", "NevernamedsItems/Resources/Companions/LeadOfLife/plusonebulletscompanion_run", 7, CompanionBuilder.AnimationType.Move, DirectionalAnimation.DirectionType.Single, DirectionalAnimation.FlipType.None);
                    companionController.aiAnimator.GetDirectionalAnimation("idle").Prefix = "idle";
                    companionController.aiAnimator.GetDirectionalAnimation("move").Prefix = "run";
                    BehaviorSpeculator component = PlusOneBulletsCompanion.prefab.GetComponent<BehaviorSpeculator>();
                    component.MovementBehaviors.Add(new CustomCompanionBehaviours.LeadOfLifeCompanionApproach());
                    component.MovementBehaviors.Add(new CompanionFollowPlayerBehavior { IdleAnimations = new string[] { "idle" } });
                }
                if (AngryBulletsCompanion.prefab == null || !CompanionBuilder.companionDictionary.ContainsKey(AngryBulletsCompanion.guid))
                {
                    AngryBulletsCompanion.prefab = CompanionBuilder.BuildPrefab("LeadOfLife AngryBullets", AngryBulletsCompanion.guid, "NevernamedsItems/Resources/Companions/LeadOfLife/angrybulletscompanion_idle_001", new IntVector2(6, 1), new IntVector2(7, 7));
                    var companionController = AngryBulletsCompanion.prefab.AddComponent<LeadOfLifeCompanion>();
                    companionController.aiActor.MovementSpeed = 5f;
                    companionController.tiedItemID = 323;
                    companionController.hasAngryBullets = true;
                    AngryBulletsCompanion.prefab.AddAnimation("idle", "NevernamedsItems/Resources/Companions/LeadOfLife/angrybulletscompanion_idle", 7, CompanionBuilder.AnimationType.Idle, DirectionalAnimation.DirectionType.Single, DirectionalAnimation.FlipType.None);
                    AngryBulletsCompanion.prefab.AddAnimation("run", "NevernamedsItems/Resources/Companions/LeadOfLife/angrybulletscompanion_run", 7, CompanionBuilder.AnimationType.Move, DirectionalAnimation.DirectionType.Single, DirectionalAnimation.FlipType.None);
                    companionController.aiAnimator.GetDirectionalAnimation("idle").Prefix = "idle";
                    companionController.aiAnimator.GetDirectionalAnimation("move").Prefix = "run";
                    BehaviorSpeculator component = AngryBulletsCompanion.prefab.GetComponent<BehaviorSpeculator>();
                    component.MovementBehaviors.Add(new CustomCompanionBehaviours.LeadOfLifeCompanionApproach());
                    component.MovementBehaviors.Add(new CompanionFollowPlayerBehavior { IdleAnimations = new string[] { "idle" } });
                }
                if (CursedBulletsCompanion.prefab == null || !CompanionBuilder.companionDictionary.ContainsKey(CursedBulletsCompanion.guid))
                {
                    CursedBulletsCompanion.prefab = CompanionBuilder.BuildPrefab("LeadOfLife CursedBullets", CursedBulletsCompanion.guid, "NevernamedsItems/Resources/Companions/LeadOfLife/cursedbulletscompanion_idle_001", new IntVector2(6, 1), new IntVector2(7, 7));
                    var companionController = CursedBulletsCompanion.prefab.AddComponent<LeadOfLifeCompanion>();
                    companionController.aiActor.MovementSpeed = 5f;
                    companionController.tiedItemID = 571;
                    companionController.scalesWithCurse = true;
                    companionController.bulletsHaveCurseParticles = true;
                    companionController.tintsBullets = true;
                    companionController.tintColour = ExtendedColours.cursedBulletsPurple;
                    CursedBulletsCompanion.prefab.AddAnimation("idle", "NevernamedsItems/Resources/Companions/LeadOfLife/cursedbulletscompanion_idle", 7, CompanionBuilder.AnimationType.Idle, DirectionalAnimation.DirectionType.Single, DirectionalAnimation.FlipType.None);
                    CursedBulletsCompanion.prefab.AddAnimation("run", "NevernamedsItems/Resources/Companions/LeadOfLife/cursedbulletscompanion_run", 7, CompanionBuilder.AnimationType.Move, DirectionalAnimation.DirectionType.Single, DirectionalAnimation.FlipType.None);
                    companionController.aiAnimator.GetDirectionalAnimation("idle").Prefix = "idle";
                    companionController.aiAnimator.GetDirectionalAnimation("move").Prefix = "run";
                    BehaviorSpeculator component = CursedBulletsCompanion.prefab.GetComponent<BehaviorSpeculator>();
                    component.MovementBehaviors.Add(new CustomCompanionBehaviours.LeadOfLifeCompanionApproach());
                    component.MovementBehaviors.Add(new CompanionFollowPlayerBehavior { IdleAnimations = new string[] { "idle" } });
                }
                if (EasyReloadBulletsCompanion.prefab == null || !CompanionBuilder.companionDictionary.ContainsKey(EasyReloadBulletsCompanion.guid))
                {
                    EasyReloadBulletsCompanion.prefab = CompanionBuilder.BuildPrefab("LeadOfLife EasyReloadBullets", EasyReloadBulletsCompanion.guid, "NevernamedsItems/Resources/Companions/LeadOfLife/easyreloadbulletscompanion_idle_001", new IntVector2(8, 1), new IntVector2(7, 7));
                    var companionController = EasyReloadBulletsCompanion.prefab.AddComponent<LeadOfLifeCompanion>();
                    companionController.aiActor.MovementSpeed = 5f;
                    companionController.tiedItemID = 375;
                    companionController.FiresBullets = false;
                    EasyReloadBulletsCompanion.prefab.AddAnimation("idle", "NevernamedsItems/Resources/Companions/LeadOfLife/easyreloadbulletscompanion_idle", 7, CompanionBuilder.AnimationType.Idle, DirectionalAnimation.DirectionType.Single, DirectionalAnimation.FlipType.None);
                    EasyReloadBulletsCompanion.prefab.AddAnimation("run", "NevernamedsItems/Resources/Companions/LeadOfLife/easyreloadbulletscompanion_run", 7, CompanionBuilder.AnimationType.Move, DirectionalAnimation.DirectionType.Single, DirectionalAnimation.FlipType.None);
                    companionController.aiAnimator.GetDirectionalAnimation("idle").Prefix = "idle";
                    companionController.aiAnimator.GetDirectionalAnimation("move").Prefix = "run";
                    BehaviorSpeculator component = EasyReloadBulletsCompanion.prefab.GetComponent<BehaviorSpeculator>();
                    component.MovementBehaviors.Add(new CompanionFollowPlayerBehavior { IdleAnimations = new string[] { "idle" } });
                }
                if (GhostBulletsCompanion.prefab == null || !CompanionBuilder.companionDictionary.ContainsKey(GhostBulletsCompanion.guid))
                {
                    GhostBulletsCompanion.prefab = CompanionBuilder.BuildPrefab("LeadOfLife GhostBullets", GhostBulletsCompanion.guid, "NevernamedsItems/Resources/Companions/LeadOfLife/ghostbulletscompanion_idle_001", new IntVector2(1, 3), new IntVector2(7, 7));
                    var companionController = GhostBulletsCompanion.prefab.AddComponent<LeadOfLifeCompanion>();
                    companionController.aiActor.MovementSpeed = 5f;
                    companionController.tiedItemID = 172;
                    companionController.hasPiercingBullets = true;
                    companionController.CanCrossPits = true;
                    companionController.aiActor.ActorShadowOffset = new Vector3(0, -0.5f);
                    GhostBulletsCompanion.prefab.AddAnimation("idle", "NevernamedsItems/Resources/Companions/LeadOfLife/ghostbulletscompanion_idle", 7, CompanionBuilder.AnimationType.Idle, DirectionalAnimation.DirectionType.Single, DirectionalAnimation.FlipType.None);
                    companionController.aiAnimator.GetDirectionalAnimation("idle").Prefix = "idle";
                    BehaviorSpeculator component = GhostBulletsCompanion.prefab.GetComponent<BehaviorSpeculator>();
                    component.MovementBehaviors.Add(new CustomCompanionBehaviours.LeadOfLifeCompanionApproach());
                    component.MovementBehaviors.Add(new CompanionFollowPlayerBehavior { IdleAnimations = new string[] { "idle" } });
                }
                if (FlakBulletsCompanion.prefab == null || !CompanionBuilder.companionDictionary.ContainsKey(FlakBulletsCompanion.guid))
                {
                    FlakBulletsCompanion.prefab = CompanionBuilder.BuildPrefab("LeadOfLife FlakBullets", FlakBulletsCompanion.guid, "NevernamedsItems/Resources/Companions/LeadOfLife/flakbulletscompanion_idle_001", new IntVector2(8, 1), new IntVector2(7, 7));
                    var companionController = FlakBulletsCompanion.prefab.AddComponent<LeadOfLifeCompanion>();
                    companionController.aiActor.MovementSpeed = 5f;
                    companionController.tiedItemID = 531;
                    companionController.hasFlakBullets = true;
                    FlakBulletsCompanion.prefab.AddAnimation("idle", "NevernamedsItems/Resources/Companions/LeadOfLife/flakbulletscompanion_idle", 7, CompanionBuilder.AnimationType.Idle, DirectionalAnimation.DirectionType.Single, DirectionalAnimation.FlipType.None);
                    FlakBulletsCompanion.prefab.AddAnimation("run", "NevernamedsItems/Resources/Companions/LeadOfLife/flakbulletscompanion_run", 7, CompanionBuilder.AnimationType.Move, DirectionalAnimation.DirectionType.Single, DirectionalAnimation.FlipType.None);
                    companionController.aiAnimator.GetDirectionalAnimation("idle").Prefix = "idle";
                    companionController.aiAnimator.GetDirectionalAnimation("move").Prefix = "run";
                    BehaviorSpeculator component = FlakBulletsCompanion.prefab.GetComponent<BehaviorSpeculator>();
                    component.MovementBehaviors.Add(new CustomCompanionBehaviours.LeadOfLifeCompanionApproach());
                    component.MovementBehaviors.Add(new CompanionFollowPlayerBehavior { IdleAnimations = new string[] { "idle" } });
                }
                if (HeavyBulletsCompanion.prefab == null || !CompanionBuilder.companionDictionary.ContainsKey(HeavyBulletsCompanion.guid))
                {
                    HeavyBulletsCompanion.prefab = CompanionBuilder.BuildPrefab("LeadOfLife HeavyBullets", HeavyBulletsCompanion.guid, "NevernamedsItems/Resources/Companions/LeadOfLife/heavybulletscompanion_idle_001", new IntVector2(4, 1), new IntVector2(7, 7));
                    var companionController = HeavyBulletsCompanion.prefab.AddComponent<LeadOfLifeCompanion>();
                    companionController.aiActor.MovementSpeed = 3.5f;
                    companionController.tiedItemID = 111;
                    companionController.damageMultiplier = 1.25f;
                    companionController.bulletSpeedMultiplier = 0.5f;
                    companionController.bulletScaleMultiplier = 1.25f;
                    companionController.knockbackMult = 2f;
                    HeavyBulletsCompanion.prefab.AddAnimation("idle", "NevernamedsItems/Resources/Companions/LeadOfLife/heavybulletscompanion_idle", 5, CompanionBuilder.AnimationType.Idle, DirectionalAnimation.DirectionType.Single, DirectionalAnimation.FlipType.None);
                    HeavyBulletsCompanion.prefab.AddAnimation("run", "NevernamedsItems/Resources/Companions/LeadOfLife/heavybulletscompanion_run", 5, CompanionBuilder.AnimationType.Move, DirectionalAnimation.DirectionType.Single, DirectionalAnimation.FlipType.None);
                    companionController.aiAnimator.GetDirectionalAnimation("idle").Prefix = "idle";
                    companionController.aiAnimator.GetDirectionalAnimation("move").Prefix = "run";
                    BehaviorSpeculator component = HeavyBulletsCompanion.prefab.GetComponent<BehaviorSpeculator>();
                    component.MovementBehaviors.Add(new CustomCompanionBehaviours.LeadOfLifeCompanionApproach());
                    component.MovementBehaviors.Add(new CompanionFollowPlayerBehavior { IdleAnimations = new string[] { "idle" } });
                }
                if (RemoteBulletsCompanion.prefab == null || !CompanionBuilder.companionDictionary.ContainsKey(RemoteBulletsCompanion.guid))
                {
                    RemoteBulletsCompanion.prefab = CompanionBuilder.BuildPrefab("LeadOfLife RemoteBullets", RemoteBulletsCompanion.guid, "NevernamedsItems/Resources/Companions/LeadOfLife/remotebulletscompanion_idle_001", new IntVector2(7, 1), new IntVector2(7, 7));
                    var companionController = RemoteBulletsCompanion.prefab.AddComponent<LeadOfLifeCompanion>();
                    companionController.aiActor.MovementSpeed = 5f;
                    companionController.tiedItemID = 530;
                    companionController.hasRemoteBullets = true;
                    companionController.damageMultiplier = 1.1f;
                    RemoteBulletsCompanion.prefab.AddAnimation("idle", "NevernamedsItems/Resources/Companions/LeadOfLife/remotebulletscompanion_idle", 7, CompanionBuilder.AnimationType.Idle, DirectionalAnimation.DirectionType.Single, DirectionalAnimation.FlipType.None);
                    RemoteBulletsCompanion.prefab.AddAnimation("run", "NevernamedsItems/Resources/Companions/LeadOfLife/remotebulletscompanion_run", 7, CompanionBuilder.AnimationType.Move, DirectionalAnimation.DirectionType.Single, DirectionalAnimation.FlipType.None);
                    companionController.aiAnimator.GetDirectionalAnimation("idle").Prefix = "idle";
                    companionController.aiAnimator.GetDirectionalAnimation("move").Prefix = "run";
                    BehaviorSpeculator component = RemoteBulletsCompanion.prefab.GetComponent<BehaviorSpeculator>();
                    component.MovementBehaviors.Add(new CompanionFollowPlayerBehavior { IdleAnimations = new string[] { "idle" } });
                }
                if (KatanaBulletsCompanion.prefab == null || !CompanionBuilder.companionDictionary.ContainsKey(KatanaBulletsCompanion.guid))
                {
                    KatanaBulletsCompanion.prefab = CompanionBuilder.BuildPrefab("LeadOfLife KatanaBullets", KatanaBulletsCompanion.guid, "NevernamedsItems/Resources/Companions/LeadOfLife/katanabulletscompanion_idle_001", new IntVector2(8, 1), new IntVector2(7, 7));
                    var companionController = KatanaBulletsCompanion.prefab.AddComponent<LeadOfLifeCompanion>();
                    companionController.aiActor.MovementSpeed = 5f;
                    companionController.tiedItemID = 822;
                    companionController.fireCooldown = 5f;
                    companionController.FiresBullets = false;
                    companionController.doesKatanaSlash = true;
                    KatanaBulletsCompanion.prefab.AddAnimation("idle", "NevernamedsItems/Resources/Companions/LeadOfLife/katanabulletscompanion_idle", 7, CompanionBuilder.AnimationType.Idle, DirectionalAnimation.DirectionType.Single, DirectionalAnimation.FlipType.None);
                    KatanaBulletsCompanion.prefab.AddAnimation("run", "NevernamedsItems/Resources/Companions/LeadOfLife/katanabulletscompanion_run", 7, CompanionBuilder.AnimationType.Move, DirectionalAnimation.DirectionType.Single, DirectionalAnimation.FlipType.None);
                    companionController.aiAnimator.GetDirectionalAnimation("idle").Prefix = "idle";
                    companionController.aiAnimator.GetDirectionalAnimation("move").Prefix = "run";
                    BehaviorSpeculator component = KatanaBulletsCompanion.prefab.GetComponent<BehaviorSpeculator>();
                    component.MovementBehaviors.Add(new CustomCompanionBehaviours.LeadOfLifeCompanionApproach());
                    component.MovementBehaviors.Add(new CompanionFollowPlayerBehavior { IdleAnimations = new string[] { "idle" } });
                }
                if (BouncyBulletsCompanion.prefab == null || !CompanionBuilder.companionDictionary.ContainsKey(BouncyBulletsCompanion.guid))
                {
                    BouncyBulletsCompanion.prefab = CompanionBuilder.BuildPrefab("LeadOfLife BouncyBullets", BouncyBulletsCompanion.guid, "NevernamedsItems/Resources/Companions/LeadOfLife/bouncybulletscompanion_idle_001", new IntVector2(6, 1), new IntVector2(7, 7));
                    var companionController = BouncyBulletsCompanion.prefab.AddComponent<LeadOfLifeCompanion>();
                    companionController.aiActor.MovementSpeed = 5f;
                    companionController.tiedItemID = 288;
                    companionController.hasBouncyBullets = true;
                    BouncyBulletsCompanion.prefab.AddAnimation("idle", "NevernamedsItems/Resources/Companions/LeadOfLife/bouncybulletscompanion_idle", 7, CompanionBuilder.AnimationType.Idle, DirectionalAnimation.DirectionType.Single, DirectionalAnimation.FlipType.None);
                    BouncyBulletsCompanion.prefab.AddAnimation("run", "NevernamedsItems/Resources/Companions/LeadOfLife/bouncybulletscompanion_run", 7, CompanionBuilder.AnimationType.Move, DirectionalAnimation.DirectionType.Single, DirectionalAnimation.FlipType.None);
                    companionController.aiAnimator.GetDirectionalAnimation("idle").Prefix = "idle";
                    companionController.aiAnimator.GetDirectionalAnimation("move").Prefix = "run";
                    BehaviorSpeculator component = BouncyBulletsCompanion.prefab.GetComponent<BehaviorSpeculator>();
                    component.MovementBehaviors.Add(new CustomCompanionBehaviours.LeadOfLifeCompanionApproach());
                    component.MovementBehaviors.Add(new CompanionFollowPlayerBehavior { IdleAnimations = new string[] { "idle" } });
                }
                if (SilverBulletsCompanion.prefab == null || !CompanionBuilder.companionDictionary.ContainsKey(SilverBulletsCompanion.guid))
                {
                    SilverBulletsCompanion.prefab = CompanionBuilder.BuildPrefab("LeadOfLife SilverBullets", SilverBulletsCompanion.guid, "NevernamedsItems/Resources/Companions/LeadOfLife/silverbulletscompanion_idle_001", new IntVector2(7, 1), new IntVector2(7, 7));
                    var companionController = SilverBulletsCompanion.prefab.AddComponent<LeadOfLifeCompanion>();
                    companionController.aiActor.MovementSpeed = 5f;
                    companionController.tiedItemID = 538;
                    companionController.tintsBullets = true;
                    companionController.tintColour = ExtendedColours.silvedBulletsSilver;
                    companionController.bossDamageMult = 1.25f;
                    companionController.jammedDamageMult = 3.25f;
                    SilverBulletsCompanion.prefab.AddAnimation("idle", "NevernamedsItems/Resources/Companions/LeadOfLife/silverbulletscompanion_idle", 7, CompanionBuilder.AnimationType.Idle, DirectionalAnimation.DirectionType.Single, DirectionalAnimation.FlipType.None);
                    SilverBulletsCompanion.prefab.AddAnimation("run", "NevernamedsItems/Resources/Companions/LeadOfLife/silverbulletscompanion_run", 7, CompanionBuilder.AnimationType.Move, DirectionalAnimation.DirectionType.Single, DirectionalAnimation.FlipType.None);
                    companionController.aiAnimator.GetDirectionalAnimation("idle").Prefix = "idle";
                    companionController.aiAnimator.GetDirectionalAnimation("move").Prefix = "run";
                    BehaviorSpeculator component = SilverBulletsCompanion.prefab.GetComponent<BehaviorSpeculator>();
                    component.MovementBehaviors.Add(new CustomCompanionBehaviours.LeadOfLifeCompanionApproach());
                    component.MovementBehaviors.Add(new CompanionFollowPlayerBehavior { IdleAnimations = new string[] { "idle" } });
                }
                if (ZombieBulletsCompanion.prefab == null || !CompanionBuilder.companionDictionary.ContainsKey(ZombieBulletsCompanion.guid))
                {
                    ZombieBulletsCompanion.prefab = CompanionBuilder.BuildPrefab("LeadOfLife ZombieBullets", ZombieBulletsCompanion.guid, "NevernamedsItems/Resources/Companions/LeadOfLife/zombiebulletscompanion_idle_001", new IntVector2(6, 1), new IntVector2(7, 7));
                    var companionController = ZombieBulletsCompanion.prefab.AddComponent<LeadOfLifeCompanion>();
                    companionController.aiActor.MovementSpeed = 6.5f;
                    companionController.tiedItemID = 528;
                    companionController.FiresBullets = false;
                    ZombieBulletsCompanion.prefab.AddAnimation("idle", "NevernamedsItems/Resources/Companions/LeadOfLife/zombiebulletscompanion_idle", 7, CompanionBuilder.AnimationType.Idle, DirectionalAnimation.DirectionType.Single, DirectionalAnimation.FlipType.None);
                    ZombieBulletsCompanion.prefab.AddAnimation("run", "NevernamedsItems/Resources/Companions/LeadOfLife/zombiebulletscompanion_run", 7, CompanionBuilder.AnimationType.Move, DirectionalAnimation.DirectionType.Single, DirectionalAnimation.FlipType.None);
                    companionController.aiAnimator.GetDirectionalAnimation("idle").Prefix = "idle";
                    companionController.aiAnimator.GetDirectionalAnimation("move").Prefix = "run";
                    BehaviorSpeculator component = ZombieBulletsCompanion.prefab.GetComponent<BehaviorSpeculator>();

                    CustomCompanionBehaviours.LeadOfLifeCompanionApproach ZombieWalk = new CustomCompanionBehaviours.LeadOfLifeCompanionApproach();
                    ZombieWalk.DesiredDistance = 1.2f;
                    ZombieWalk.isZombieBullets = true;
                    component.MovementBehaviors.Add(ZombieWalk);
                    component.MovementBehaviors.Add(new CompanionFollowPlayerBehavior { IdleAnimations = new string[] { "idle" } });
                }
                if (Bloody9mmCompanion.prefab == null || !CompanionBuilder.companionDictionary.ContainsKey(Bloody9mmCompanion.guid))
                {
                    Bloody9mmCompanion.prefab = CompanionBuilder.BuildPrefab("LeadOfLife Bloody9mm", Bloody9mmCompanion.guid, "NevernamedsItems/Resources/Companions/LeadOfLife/bloody9mmcompanion_idle_001", new IntVector2(8, 3), new IntVector2(7, 7));
                    var companionController = Bloody9mmCompanion.prefab.AddComponent<LeadOfLifeCompanion>();
                    companionController.aiActor.MovementSpeed = 5f;
                    companionController.tiedItemID = 524;
                    companionController.fireCooldown = 5f;
                    companionController.overrideBloody9mmBullet = true;
                    Bloody9mmCompanion.prefab.AddAnimation("idle", "NevernamedsItems/Resources/Companions/LeadOfLife/bloody9mmcompanion_idle", 7, CompanionBuilder.AnimationType.Idle, DirectionalAnimation.DirectionType.Single, DirectionalAnimation.FlipType.None);
                    Bloody9mmCompanion.prefab.AddAnimation("run", "NevernamedsItems/Resources/Companions/LeadOfLife/bloody9mmcompanion_run", 7, CompanionBuilder.AnimationType.Move, DirectionalAnimation.DirectionType.Single, DirectionalAnimation.FlipType.None);
                    companionController.aiAnimator.GetDirectionalAnimation("idle").Prefix = "idle";
                    companionController.aiAnimator.GetDirectionalAnimation("move").Prefix = "run";
                    BehaviorSpeculator component = Bloody9mmCompanion.prefab.GetComponent<BehaviorSpeculator>();
                    component.MovementBehaviors.Add(new CustomCompanionBehaviours.LeadOfLifeCompanionApproach());
                    component.MovementBehaviors.Add(new CompanionFollowPlayerBehavior { IdleAnimations = new string[] { "idle" } });
                }
                if (BumbulletsCompanion.prefab == null || !CompanionBuilder.companionDictionary.ContainsKey(BumbulletsCompanion.guid))
                {
                    BumbulletsCompanion.prefab = CompanionBuilder.BuildPrefab("LeadOfLife Bumbullets", BumbulletsCompanion.guid, "NevernamedsItems/Resources/Companions/LeadOfLife/bumbulletscompanion_idle_001", new IntVector2(8, 1), new IntVector2(7, 7));
                    var companionController = BumbulletsCompanion.prefab.AddComponent<LeadOfLifeCompanion>();
                    companionController.aiActor.MovementSpeed = 5f;
                    companionController.tiedItemID = 630;
                    companionController.gunIDToCopyBulletsFrom = 14;
                    BumbulletsCompanion.prefab.AddAnimation("idle", "NevernamedsItems/Resources/Companions/LeadOfLife/bumbulletscompanion_idle", 7, CompanionBuilder.AnimationType.Idle, DirectionalAnimation.DirectionType.Single, DirectionalAnimation.FlipType.None);
                    BumbulletsCompanion.prefab.AddAnimation("run", "NevernamedsItems/Resources/Companions/LeadOfLife/bumbulletscompanion_run", 7, CompanionBuilder.AnimationType.Move, DirectionalAnimation.DirectionType.Single, DirectionalAnimation.FlipType.None);
                    companionController.aiAnimator.GetDirectionalAnimation("idle").Prefix = "idle";
                    companionController.aiAnimator.GetDirectionalAnimation("move").Prefix = "run";
                    BehaviorSpeculator component = BumbulletsCompanion.prefab.GetComponent<BehaviorSpeculator>();
                    component.MovementBehaviors.Add(new CustomCompanionBehaviours.LeadOfLifeCompanionApproach());
                    component.MovementBehaviors.Add(new CompanionFollowPlayerBehavior { IdleAnimations = new string[] { "idle" } });
                }
                if (ChanceBulletsCompanion.prefab == null || !CompanionBuilder.companionDictionary.ContainsKey(ChanceBulletsCompanion.guid))
                {
                    ChanceBulletsCompanion.prefab = CompanionBuilder.BuildPrefab("LeadOfLife ChanceBullets", ChanceBulletsCompanion.guid, "NevernamedsItems/Resources/Companions/LeadOfLife/chancebulletscompanion_idle_001", new IntVector2(10, 1), new IntVector2(7, 7));
                    var companionController = ChanceBulletsCompanion.prefab.AddComponent<LeadOfLifeCompanion>();
                    companionController.aiActor.MovementSpeed = 5f;
                    companionController.tiedItemID = 521;
                    companionController.fireCooldown = 1.5f;
                    companionController.picksRandomPlayerBullet = true;
                    companionController.FiresBullets = false;
                    ChanceBulletsCompanion.prefab.AddAnimation("idle", "NevernamedsItems/Resources/Companions/LeadOfLife/chancebulletscompanion_idle", 7, CompanionBuilder.AnimationType.Idle, DirectionalAnimation.DirectionType.Single, DirectionalAnimation.FlipType.None);
                    ChanceBulletsCompanion.prefab.AddAnimation("run", "NevernamedsItems/Resources/Companions/LeadOfLife/chancebulletscompanion_run", 7, CompanionBuilder.AnimationType.Move, DirectionalAnimation.DirectionType.Single, DirectionalAnimation.FlipType.None);
                    companionController.aiAnimator.GetDirectionalAnimation("idle").Prefix = "idle";
                    companionController.aiAnimator.GetDirectionalAnimation("move").Prefix = "run";
                    BehaviorSpeculator component = ChanceBulletsCompanion.prefab.GetComponent<BehaviorSpeculator>();
                    component.MovementBehaviors.Add(new CustomCompanionBehaviours.LeadOfLifeCompanionApproach());
                    component.MovementBehaviors.Add(new CompanionFollowPlayerBehavior { IdleAnimations = new string[] { "idle" } });
                }
                if (CharmingRoundsCompanion.prefab == null || !CompanionBuilder.companionDictionary.ContainsKey(CharmingRoundsCompanion.guid))
                {
                    CharmingRoundsCompanion.prefab = CompanionBuilder.BuildPrefab("LeadOfLife CharmingRounds", CharmingRoundsCompanion.guid, "NevernamedsItems/Resources/Companions/LeadOfLife/charmingroundscompanion_idle_001", new IntVector2(6, 1), new IntVector2(7, 7));
                    var companionController = CharmingRoundsCompanion.prefab.AddComponent<LeadOfLifeCompanion>();
                    companionController.aiActor.MovementSpeed = 5f;
                    companionController.tiedItemID = 527;
                    companionController.fireCooldown = 1.6f;
                    companionController.inflictsCharm = true;
                    companionController.tintsBullets = true;
                    companionController.tintColour = ExtendedColours.charmPink;
                    CharmingRoundsCompanion.prefab.AddAnimation("idle", "NevernamedsItems/Resources/Companions/LeadOfLife/charmingroundscompanion_idle", 7, CompanionBuilder.AnimationType.Idle, DirectionalAnimation.DirectionType.Single, DirectionalAnimation.FlipType.None);
                    CharmingRoundsCompanion.prefab.AddAnimation("run", "NevernamedsItems/Resources/Companions/LeadOfLife/charmingroundscompanion_run", 7, CompanionBuilder.AnimationType.Move, DirectionalAnimation.DirectionType.Single, DirectionalAnimation.FlipType.None);
                    companionController.aiAnimator.GetDirectionalAnimation("idle").Prefix = "idle";
                    companionController.aiAnimator.GetDirectionalAnimation("move").Prefix = "run";
                    BehaviorSpeculator component = CharmingRoundsCompanion.prefab.GetComponent<BehaviorSpeculator>();
                    component.MovementBehaviors.Add(new CustomCompanionBehaviours.LeadOfLifeCompanionApproach());
                    component.MovementBehaviors.Add(new CompanionFollowPlayerBehavior { IdleAnimations = new string[] { "idle" } });
                }
                if (DevolverRoundsCompanion.prefab == null || !CompanionBuilder.companionDictionary.ContainsKey(DevolverRoundsCompanion.guid))
                {
                    DevolverRoundsCompanion.prefab = CompanionBuilder.BuildPrefab("LeadOfLife DevolverRounds", DevolverRoundsCompanion.guid, "NevernamedsItems/Resources/Companions/LeadOfLife/devolverroundscompanion_idle_001", new IntVector2(7, 1), new IntVector2(7, 7));
                    var companionController = DevolverRoundsCompanion.prefab.AddComponent<LeadOfLifeCompanion>();
                    companionController.aiActor.MovementSpeed = 5f;
                    companionController.tiedItemID = 638;
                    companionController.transmogsEnemies = true;
                    companionController.chanceToTransmog = 0.1f;
                    companionController.transmogSoundEffect = "Play_WPN_devolver_morph_01";
                    companionController.transmogTargetGuid = "05891b158cd542b1a5f3df30fb67a7ff";
                    DevolverRoundsCompanion.prefab.AddAnimation("idle", "NevernamedsItems/Resources/Companions/LeadOfLife/devolverroundscompanion_idle", 7, CompanionBuilder.AnimationType.Idle, DirectionalAnimation.DirectionType.Single, DirectionalAnimation.FlipType.None);
                    DevolverRoundsCompanion.prefab.AddAnimation("run", "NevernamedsItems/Resources/Companions/LeadOfLife/devolverroundscompanion_run", 7, CompanionBuilder.AnimationType.Move, DirectionalAnimation.DirectionType.Single, DirectionalAnimation.FlipType.None);
                    companionController.aiAnimator.GetDirectionalAnimation("idle").Prefix = "idle";
                    companionController.aiAnimator.GetDirectionalAnimation("move").Prefix = "run";
                    BehaviorSpeculator component = DevolverRoundsCompanion.prefab.GetComponent<BehaviorSpeculator>();
                    component.MovementBehaviors.Add(new CustomCompanionBehaviours.LeadOfLifeCompanionApproach());
                    component.MovementBehaviors.Add(new CompanionFollowPlayerBehavior { IdleAnimations = new string[] { "idle" } });
                }
                if (GildedBulletsCompanion.prefab == null || !CompanionBuilder.companionDictionary.ContainsKey(GildedBulletsCompanion.guid))
                {
                    GildedBulletsCompanion.prefab = CompanionBuilder.BuildPrefab("LeadOfLife GildedBullets", GildedBulletsCompanion.guid, "NevernamedsItems/Resources/Companions/LeadOfLife/gildedbulletscompanion_idle_001", new IntVector2(10, 0), new IntVector2(7, 7));
                    var companionController = GildedBulletsCompanion.prefab.AddComponent<LeadOfLifeCompanion>();
                    companionController.aiActor.MovementSpeed = 5f;
                    companionController.tiedItemID = 532;
                    companionController.tintsBullets = true;
                    companionController.tintColour = ExtendedColours.gildedBulletsGold;
                    companionController.scalesWithCurrency = true;
                    companionController.spawnsCurrencyOnRoomClear = true;
                    GildedBulletsCompanion.prefab.AddAnimation("idle", "NevernamedsItems/Resources/Companions/LeadOfLife/gildedbulletscompanion_idle", 7, CompanionBuilder.AnimationType.Idle, DirectionalAnimation.DirectionType.Single, DirectionalAnimation.FlipType.None);
                    GildedBulletsCompanion.prefab.AddAnimation("run", "NevernamedsItems/Resources/Companions/LeadOfLife/gildedbulletscompanion_run", 7, CompanionBuilder.AnimationType.Move, DirectionalAnimation.DirectionType.Single, DirectionalAnimation.FlipType.None);
                    companionController.aiAnimator.GetDirectionalAnimation("idle").Prefix = "idle";
                    companionController.aiAnimator.GetDirectionalAnimation("move").Prefix = "run";
                    BehaviorSpeculator component = GildedBulletsCompanion.prefab.GetComponent<BehaviorSpeculator>();
                    component.MovementBehaviors.Add(new CustomCompanionBehaviours.LeadOfLifeCompanionApproach());
                    component.MovementBehaviors.Add(new CompanionFollowPlayerBehavior { IdleAnimations = new string[] { "idle" } });
                }
                if (HelixBulletsCompanion.prefab == null || !CompanionBuilder.companionDictionary.ContainsKey(HelixBulletsCompanion.guid))
                {
                    HelixBulletsCompanion.prefab = CompanionBuilder.BuildPrefab("LeadOfLife HelixBullets", HelixBulletsCompanion.guid, "NevernamedsItems/Resources/Companions/LeadOfLife/helixbulletscompanion_idle_001", new IntVector2(7, 1), new IntVector2(7, 7));
                    var companionController = HelixBulletsCompanion.prefab.AddComponent<LeadOfLifeCompanion>();
                    companionController.aiActor.MovementSpeed = 5f;
                    companionController.tiedItemID = 568;
                    companionController.FiresBullets = false;
                    companionController.damageMultiplier = 0.75f;
                    companionController.isHelix = true;
                    HelixBulletsCompanion.prefab.AddAnimation("idle", "NevernamedsItems/Resources/Companions/LeadOfLife/helixbulletscompanion_idle", 7, CompanionBuilder.AnimationType.Idle, DirectionalAnimation.DirectionType.Single, DirectionalAnimation.FlipType.None);
                    HelixBulletsCompanion.prefab.AddAnimation("run", "NevernamedsItems/Resources/Companions/LeadOfLife/helixbulletscompanion_run", 7, CompanionBuilder.AnimationType.Move, DirectionalAnimation.DirectionType.Single, DirectionalAnimation.FlipType.None);
                    companionController.aiAnimator.GetDirectionalAnimation("idle").Prefix = "idle";
                    companionController.aiAnimator.GetDirectionalAnimation("move").Prefix = "run";
                    BehaviorSpeculator component = HelixBulletsCompanion.prefab.GetComponent<BehaviorSpeculator>();
                    component.MovementBehaviors.Add(new CustomCompanionBehaviours.LeadOfLifeCompanionApproach());
                    component.MovementBehaviors.Add(new CompanionFollowPlayerBehavior { IdleAnimations = new string[] { "idle" } });
                }
                if (HomingBulletsCompanion.prefab == null || !CompanionBuilder.companionDictionary.ContainsKey(HomingBulletsCompanion.guid))
                {
                    HomingBulletsCompanion.prefab = CompanionBuilder.BuildPrefab("LeadOfLife HomingBullets", HomingBulletsCompanion.guid, "NevernamedsItems/Resources/Companions/LeadOfLife/homingbulletscompanion_idle_001", new IntVector2(5, 0), new IntVector2(7, 7));
                    var companionController = HomingBulletsCompanion.prefab.AddComponent<LeadOfLifeCompanion>();
                    companionController.aiActor.MovementSpeed = 5f;
                    companionController.tiedItemID = 284;
                    companionController.hasHomingBullets = true;
                    HomingBulletsCompanion.prefab.AddAnimation("idle", "NevernamedsItems/Resources/Companions/LeadOfLife/homingbulletscompanion_idle", 7, CompanionBuilder.AnimationType.Idle, DirectionalAnimation.DirectionType.Single, DirectionalAnimation.FlipType.None);
                    HomingBulletsCompanion.prefab.AddAnimation("run", "NevernamedsItems/Resources/Companions/LeadOfLife/homingbulletscompanion_run", 7, CompanionBuilder.AnimationType.Move, DirectionalAnimation.DirectionType.Single, DirectionalAnimation.FlipType.None);
                    companionController.aiAnimator.GetDirectionalAnimation("idle").Prefix = "idle";
                    companionController.aiAnimator.GetDirectionalAnimation("move").Prefix = "run";
                    BehaviorSpeculator component = HomingBulletsCompanion.prefab.GetComponent<BehaviorSpeculator>();
                    component.MovementBehaviors.Add(new CustomCompanionBehaviours.LeadOfLifeCompanionApproach());
                    component.MovementBehaviors.Add(new CompanionFollowPlayerBehavior { IdleAnimations = new string[] { "idle" } });
                }
                if (MagicBulletsCompanion.prefab == null || !CompanionBuilder.companionDictionary.ContainsKey(MagicBulletsCompanion.guid))
                {
                    MagicBulletsCompanion.prefab = CompanionBuilder.BuildPrefab("LeadOfLife MagicBullets", MagicBulletsCompanion.guid, "NevernamedsItems/Resources/Companions/LeadOfLife/magicbulletscompanion_idle_001", new IntVector2(6, 0), new IntVector2(7, 7));
                    var companionController = MagicBulletsCompanion.prefab.AddComponent<LeadOfLifeCompanion>();
                    companionController.aiActor.MovementSpeed = 5f;
                    companionController.tiedItemID = 533;
                    companionController.fireCooldown = 7.5f;
                    companionController.gunIDToCopyBulletsFrom = 61;
                    companionController.transmogsEnemies = true;
                    companionController.transmogTargetGuid = "76bc43539fc24648bff4568c75c686d1";
                    companionController.chanceToTransmog = 1;
                    MagicBulletsCompanion.prefab.AddAnimation("idle", "NevernamedsItems/Resources/Companions/LeadOfLife/magicbulletscompanion_idle", 7, CompanionBuilder.AnimationType.Idle, DirectionalAnimation.DirectionType.Single, DirectionalAnimation.FlipType.None);
                    MagicBulletsCompanion.prefab.AddAnimation("run", "NevernamedsItems/Resources/Companions/LeadOfLife/magicbulletscompanion_run", 7, CompanionBuilder.AnimationType.Move, DirectionalAnimation.DirectionType.Single, DirectionalAnimation.FlipType.None);
                    companionController.aiAnimator.GetDirectionalAnimation("idle").Prefix = "idle";
                    companionController.aiAnimator.GetDirectionalAnimation("move").Prefix = "run";
                    BehaviorSpeculator component = MagicBulletsCompanion.prefab.GetComponent<BehaviorSpeculator>();
                    component.MovementBehaviors.Add(new CustomCompanionBehaviours.LeadOfLifeCompanionApproach());
                    component.MovementBehaviors.Add(new CompanionFollowPlayerBehavior { IdleAnimations = new string[] { "idle" } });
                }
                if (RocketPoweredBulletsCompanion.prefab == null || !CompanionBuilder.companionDictionary.ContainsKey(RocketPoweredBulletsCompanion.guid))
                {
                    RocketPoweredBulletsCompanion.prefab = CompanionBuilder.BuildPrefab("LeadOfLife RocketPoweredBullets", RocketPoweredBulletsCompanion.guid, "NevernamedsItems/Resources/Companions/LeadOfLife/rocketpoweredbulletscompanion_idle_001", new IntVector2(2, 0), new IntVector2(7, 7));
                    var companionController = RocketPoweredBulletsCompanion.prefab.AddComponent<LeadOfLifeCompanion>();
                    companionController.aiActor.MovementSpeed = 8f;
                    companionController.fireCooldown = 1f;
                    companionController.damageMultiplier = 1.1f;
                    companionController.bulletSpeedMultiplier = 1.5f;
                    companionController.CanCrossPits = true;
                    companionController.canIgniteGoop = true;
                    companionController.aiActor.ActorShadowOffset = new Vector3(0, -0.5f);
                    companionController.tiedItemID = 115;
                    RocketPoweredBulletsCompanion.prefab.AddAnimation("idle", "NevernamedsItems/Resources/Companions/LeadOfLife/rocketpoweredbulletscompanion_idle", 7, CompanionBuilder.AnimationType.Idle, DirectionalAnimation.DirectionType.Single, DirectionalAnimation.FlipType.None);
                    companionController.aiAnimator.GetDirectionalAnimation("idle").Prefix = "idle";
                    BehaviorSpeculator component = RocketPoweredBulletsCompanion.prefab.GetComponent<BehaviorSpeculator>();
                    component.MovementBehaviors.Add(new CustomCompanionBehaviours.LeadOfLifeCompanionApproach());
                    component.MovementBehaviors.Add(new CompanionFollowPlayerBehavior { IdleAnimations = new string[] { "idle" } });
                }

                if (ScattershotCompanion.prefab == null || !CompanionBuilder.companionDictionary.ContainsKey(ScattershotCompanion.guid))
                {
                    ScattershotCompanion.prefab = CompanionBuilder.BuildPrefab("LeadOfLife Scattershot", ScattershotCompanion.guid, "NevernamedsItems/Resources/Companions/LeadOfLife/scattershotcompanion_idle_001", new IntVector2(6, 0), new IntVector2(7, 7));
                    var companionController = ScattershotCompanion.prefab.AddComponent<LeadOfLifeCompanion>();
                    companionController.aiActor.MovementSpeed = 5f;
                    companionController.tiedItemID = 241;
                    companionController.angleVariance = 35f;
                    companionController.attacksPerActivation = 3;
                    companionController.damageMultiplier = 0.6f;
                    ScattershotCompanion.prefab.AddAnimation("idle", "NevernamedsItems/Resources/Companions/LeadOfLife/scattershotcompanion_idle", 7, CompanionBuilder.AnimationType.Idle, DirectionalAnimation.DirectionType.Single, DirectionalAnimation.FlipType.None);
                    ScattershotCompanion.prefab.AddAnimation("run", "NevernamedsItems/Resources/Companions/LeadOfLife/scattershotcompanion_run", 7, CompanionBuilder.AnimationType.Move, DirectionalAnimation.DirectionType.Single, DirectionalAnimation.FlipType.None);
                    companionController.aiAnimator.GetDirectionalAnimation("idle").Prefix = "idle";
                    companionController.aiAnimator.GetDirectionalAnimation("move").Prefix = "run";
                    BehaviorSpeculator component = ScattershotCompanion.prefab.GetComponent<BehaviorSpeculator>();
                    component.MovementBehaviors.Add(new CustomCompanionBehaviours.LeadOfLifeCompanionApproach());
                    component.MovementBehaviors.Add(new CompanionFollowPlayerBehavior { IdleAnimations = new string[] { "idle" } });
                }
                if (ShadowBulletsCompanion.prefab == null || !CompanionBuilder.companionDictionary.ContainsKey(ShadowBulletsCompanion.guid))
                {
                    ShadowBulletsCompanion.prefab = CompanionBuilder.BuildPrefab("LeadOfLife ShadowBullets", ShadowBulletsCompanion.guid, "NevernamedsItems/Resources/Companions/LeadOfLife/shadowbulletscompanion_idle_001", new IntVector2(6, 0), new IntVector2(7, 7));
                    var companionController = ShadowBulletsCompanion.prefab.AddComponent<LeadOfLifeCompanion>();
                    companionController.aiActor.MovementSpeed = 5f;
                    companionController.tiedItemID = 352;
                    companionController.hasShadowBullets = true;
                    ShadowBulletsCompanion.prefab.AddAnimation("idle", "NevernamedsItems/Resources/Companions/LeadOfLife/shadowbulletscompanion_idle", 7, CompanionBuilder.AnimationType.Idle, DirectionalAnimation.DirectionType.Single, DirectionalAnimation.FlipType.None);
                    ShadowBulletsCompanion.prefab.AddAnimation("run", "NevernamedsItems/Resources/Companions/LeadOfLife/shadowbulletscompanion_run", 7, CompanionBuilder.AnimationType.Move, DirectionalAnimation.DirectionType.Single, DirectionalAnimation.FlipType.None);
                    companionController.aiAnimator.GetDirectionalAnimation("idle").Prefix = "idle";
                    companionController.aiAnimator.GetDirectionalAnimation("move").Prefix = "run";
                    BehaviorSpeculator component = ShadowBulletsCompanion.prefab.GetComponent<BehaviorSpeculator>();
                    component.MovementBehaviors.Add(new CustomCompanionBehaviours.LeadOfLifeCompanionApproach());
                    component.MovementBehaviors.Add(new CompanionFollowPlayerBehavior { IdleAnimations = new string[] { "idle" } });
                }
                if (StoutBulletsCompanion.prefab == null || !CompanionBuilder.companionDictionary.ContainsKey(StoutBulletsCompanion.guid))
                {
                    StoutBulletsCompanion.prefab = CompanionBuilder.BuildPrefab("LeadOfLife StoutBullets", StoutBulletsCompanion.guid, "NevernamedsItems/Resources/Companions/LeadOfLife/stoutbulletscompanion_idle_001", new IntVector2(6, 0), new IntVector2(6, 7));
                    var companionController = StoutBulletsCompanion.prefab.AddComponent<LeadOfLifeCompanion>();
                    companionController.aiActor.MovementSpeed = 3.5f;
                    companionController.tiedItemID = 523;
                    companionController.damageMultiplier = 3;
                    companionController.bulletScaleMultiplier = 1.5f;
                    companionController.scalesDamageWithPlayerProximity = true;
                    companionController.scaleProxClose = true;
                    StoutBulletsCompanion.prefab.AddAnimation("idle", "NevernamedsItems/Resources/Companions/LeadOfLife/stoutbulletscompanion_idle", 5, CompanionBuilder.AnimationType.Idle, DirectionalAnimation.DirectionType.Single, DirectionalAnimation.FlipType.None);
                    StoutBulletsCompanion.prefab.AddAnimation("run", "NevernamedsItems/Resources/Companions/LeadOfLife/stoutbulletscompanion_run", 5, CompanionBuilder.AnimationType.Move, DirectionalAnimation.DirectionType.Single, DirectionalAnimation.FlipType.None);
                    companionController.aiAnimator.GetDirectionalAnimation("idle").Prefix = "idle";
                    companionController.aiAnimator.GetDirectionalAnimation("move").Prefix = "run";
                    BehaviorSpeculator component = StoutBulletsCompanion.prefab.GetComponent<BehaviorSpeculator>();
                    component.MovementBehaviors.Add(new CustomCompanionBehaviours.LeadOfLifeCompanionApproach());
                    component.MovementBehaviors.Add(new CompanionFollowPlayerBehavior { IdleAnimations = new string[] { "idle" } });
                }
                if (AlphaBulletsCompanion.prefab == null || !CompanionBuilder.companionDictionary.ContainsKey(AlphaBulletsCompanion.guid))
                {
                    AlphaBulletsCompanion.prefab = CompanionBuilder.BuildPrefab("LeadOfLife AlphaBullets", AlphaBulletsCompanion.guid, "NevernamedsItems/Resources/Companions/LeadOfLife/alphabulletscompanion_idle_001", new IntVector2(5, 0), new IntVector2(7, 7));
                    var companionController = AlphaBulletsCompanion.prefab.AddComponent<LeadOfLifeCompanion>();
                    companionController.aiActor.MovementSpeed = 5f;
                    companionController.tiedItemID = 373;
                    companionController.isAlpha = true;
                    AlphaBulletsCompanion.prefab.AddAnimation("idle", "NevernamedsItems/Resources/Companions/LeadOfLife/alphabulletscompanion_idle", 7, CompanionBuilder.AnimationType.Idle, DirectionalAnimation.DirectionType.Single, DirectionalAnimation.FlipType.None);
                    AlphaBulletsCompanion.prefab.AddAnimation("run", "NevernamedsItems/Resources/Companions/LeadOfLife/alphabulletscompanion_run", 7, CompanionBuilder.AnimationType.Move, DirectionalAnimation.DirectionType.Single, DirectionalAnimation.FlipType.None);
                    companionController.aiAnimator.GetDirectionalAnimation("idle").Prefix = "idle";
                    companionController.aiAnimator.GetDirectionalAnimation("move").Prefix = "run";
                    BehaviorSpeculator component = AlphaBulletsCompanion.prefab.GetComponent<BehaviorSpeculator>();
                    component.MovementBehaviors.Add(new CustomCompanionBehaviours.LeadOfLifeCompanionApproach());
                    component.MovementBehaviors.Add(new CompanionFollowPlayerBehavior { IdleAnimations = new string[] { "idle" } });
                }
                if (OmegaBulletsCompanion.prefab == null || !CompanionBuilder.companionDictionary.ContainsKey(OmegaBulletsCompanion.guid))
                {
                    OmegaBulletsCompanion.prefab = CompanionBuilder.BuildPrefab("LeadOfLife OmegaBullets", OmegaBulletsCompanion.guid, "NevernamedsItems/Resources/Companions/LeadOfLife/omegabulletscompanion_idle_001", new IntVector2(5, 0), new IntVector2(7, 7));
                    var companionController = OmegaBulletsCompanion.prefab.AddComponent<LeadOfLifeCompanion>();
                    companionController.aiActor.MovementSpeed = 5f;
                    companionController.tiedItemID = 374;
                    companionController.isOmega = true;
                    OmegaBulletsCompanion.prefab.AddAnimation("idle", "NevernamedsItems/Resources/Companions/LeadOfLife/omegabulletscompanion_idle", 7, CompanionBuilder.AnimationType.Idle, DirectionalAnimation.DirectionType.Single, DirectionalAnimation.FlipType.None);
                    OmegaBulletsCompanion.prefab.AddAnimation("run", "NevernamedsItems/Resources/Companions/LeadOfLife/omegabulletscompanion_run", 7, CompanionBuilder.AnimationType.Move, DirectionalAnimation.DirectionType.Single, DirectionalAnimation.FlipType.None);
                    companionController.aiAnimator.GetDirectionalAnimation("idle").Prefix = "idle";
                    companionController.aiAnimator.GetDirectionalAnimation("move").Prefix = "run";
                    BehaviorSpeculator component = OmegaBulletsCompanion.prefab.GetComponent<BehaviorSpeculator>();
                    component.MovementBehaviors.Add(new CustomCompanionBehaviours.LeadOfLifeCompanionApproach());
                    component.MovementBehaviors.Add(new CompanionFollowPlayerBehavior { IdleAnimations = new string[] { "idle" } });
                }
                if (ChaosBulletsCompanion.prefab == null || !CompanionBuilder.companionDictionary.ContainsKey(ChaosBulletsCompanion.guid))
                {
                    ChaosBulletsCompanion.prefab = CompanionBuilder.BuildPrefab("LeadOfLife ChaosBullets", ChaosBulletsCompanion.guid, "NevernamedsItems/Resources/Companions/LeadOfLife/chaosbulletscompanion_idle_001", new IntVector2(9, 0), new IntVector2(5, 5));
                    var companionController = ChaosBulletsCompanion.prefab.AddComponent<LeadOfLifeCompanion>();
                    companionController.aiActor.MovementSpeed = 7f;
                    companionController.tiedItemID = 569;
                    companionController.hasChaosBullets = true;
                    companionController.CanCrossPits = true;
                    companionController.aiActor.ActorShadowOffset = new Vector3(0, -0.5f);
                    ChaosBulletsCompanion.prefab.AddAnimation("idle", "NevernamedsItems/Resources/Companions/LeadOfLife/chaosbulletscompanion_idle", 9, CompanionBuilder.AnimationType.Idle, DirectionalAnimation.DirectionType.Single, DirectionalAnimation.FlipType.None);
                    companionController.aiAnimator.GetDirectionalAnimation("idle").Prefix = "idle";
                    BehaviorSpeculator component = ChaosBulletsCompanion.prefab.GetComponent<BehaviorSpeculator>();
                    component.MovementBehaviors.Add(new CustomCompanionBehaviours.LeadOfLifeCompanionApproach());
                    component.MovementBehaviors.Add(new CompanionFollowPlayerBehavior { IdleAnimations = new string[] { "idle" } });
                }
                if (ExplosiveRoundsCompanion.prefab == null || !CompanionBuilder.companionDictionary.ContainsKey(ExplosiveRoundsCompanion.guid))
                {
                    ExplosiveRoundsCompanion.prefab = CompanionBuilder.BuildPrefab("LeadOfLife ExplosiveRounds", ExplosiveRoundsCompanion.guid, "NevernamedsItems/Resources/Companions/LeadOfLife/explosiveroundscompanion_idle_001", new IntVector2(4, 0), new IntVector2(7, 7));
                    var companionController = ExplosiveRoundsCompanion.prefab.AddComponent<LeadOfLifeCompanion>();
                    companionController.aiActor.MovementSpeed = 5f;
                    companionController.tiedItemID = 304;
                    companionController.fireCooldown = 5f;
                    companionController.dropsBombs = true;
                    companionController.hasExplosiveBullets = true;
                    ExplosiveRoundsCompanion.prefab.AddAnimation("idle", "NevernamedsItems/Resources/Companions/LeadOfLife/explosiveroundscompanion_idle", 7, CompanionBuilder.AnimationType.Idle, DirectionalAnimation.DirectionType.Single, DirectionalAnimation.FlipType.None);
                    ExplosiveRoundsCompanion.prefab.AddAnimation("run", "NevernamedsItems/Resources/Companions/LeadOfLife/explosiveroundscompanion_run", 7, CompanionBuilder.AnimationType.Move, DirectionalAnimation.DirectionType.Single, DirectionalAnimation.FlipType.None);
                    companionController.aiAnimator.GetDirectionalAnimation("idle").Prefix = "idle";
                    companionController.aiAnimator.GetDirectionalAnimation("move").Prefix = "run";
                    BehaviorSpeculator component = ExplosiveRoundsCompanion.prefab.GetComponent<BehaviorSpeculator>();
                    component.MovementBehaviors.Add(new CustomCompanionBehaviours.LeadOfLifeCompanionApproach());
                    component.MovementBehaviors.Add(new CompanionFollowPlayerBehavior { IdleAnimations = new string[] { "idle" } });
                }
                if (FatBulletsCompanion.prefab == null || !CompanionBuilder.companionDictionary.ContainsKey(FatBulletsCompanion.guid))
                {
                    FatBulletsCompanion.prefab = CompanionBuilder.BuildPrefab("LeadOfLife FatBullets", FatBulletsCompanion.guid, "NevernamedsItems/Resources/Companions/LeadOfLife/fatbulletscompanion_idle_001", new IntVector2(5, 0), new IntVector2(7, 7));
                    var companionController = FatBulletsCompanion.prefab.AddComponent<LeadOfLifeCompanion>();
                    companionController.aiActor.MovementSpeed = 3.5f;
                    companionController.tiedItemID = 277;
                    companionController.damageMultiplier = 1.45f;
                    companionController.knockbackMult = 2;
                    companionController.bulletScaleMultiplier = 2;
                    FatBulletsCompanion.prefab.AddAnimation("idle", "NevernamedsItems/Resources/Companions/LeadOfLife/fatbulletscompanion_idle", 5, CompanionBuilder.AnimationType.Idle, DirectionalAnimation.DirectionType.Single, DirectionalAnimation.FlipType.None);
                    FatBulletsCompanion.prefab.AddAnimation("run", "NevernamedsItems/Resources/Companions/LeadOfLife/fatbulletscompanion_run", 5, CompanionBuilder.AnimationType.Move, DirectionalAnimation.DirectionType.Single, DirectionalAnimation.FlipType.None);
                    companionController.aiAnimator.GetDirectionalAnimation("idle").Prefix = "idle";
                    companionController.aiAnimator.GetDirectionalAnimation("move").Prefix = "run";
                    BehaviorSpeculator component = FatBulletsCompanion.prefab.GetComponent<BehaviorSpeculator>();
                    component.MovementBehaviors.Add(new CustomCompanionBehaviours.LeadOfLifeCompanionApproach());
                    component.MovementBehaviors.Add(new CompanionFollowPlayerBehavior { IdleAnimations = new string[] { "idle" } });
                }
                if (FrostBulletsCompanion.prefab == null || !CompanionBuilder.companionDictionary.ContainsKey(FrostBulletsCompanion.guid))
                {
                    FrostBulletsCompanion.prefab = CompanionBuilder.BuildPrefab("LeadOfLife FrostBullets", FrostBulletsCompanion.guid, "NevernamedsItems/Resources/Companions/LeadOfLife/frostbulletscompanion_idle_001", new IntVector2(5, 0), new IntVector2(7, 7));
                    var companionController = FrostBulletsCompanion.prefab.AddComponent<LeadOfLifeCompanion>();
                    companionController.aiActor.MovementSpeed = 5f;
                    companionController.tiedItemID = 278;
                    companionController.tintColour = ExtendedColours.freezeBlue;
                    companionController.tintsBullets = true;
                    companionController.inflictsFreeze = true;
                    FrostBulletsCompanion.prefab.AddAnimation("idle", "NevernamedsItems/Resources/Companions/LeadOfLife/frostbulletscompanion_idle", 7, CompanionBuilder.AnimationType.Idle, DirectionalAnimation.DirectionType.Single, DirectionalAnimation.FlipType.None);
                    FrostBulletsCompanion.prefab.AddAnimation("run", "NevernamedsItems/Resources/Companions/LeadOfLife/frostbulletscompanion_run", 7, CompanionBuilder.AnimationType.Move, DirectionalAnimation.DirectionType.Single, DirectionalAnimation.FlipType.None);
                    companionController.aiAnimator.GetDirectionalAnimation("idle").Prefix = "idle";
                    companionController.aiAnimator.GetDirectionalAnimation("move").Prefix = "run";
                    BehaviorSpeculator component = FrostBulletsCompanion.prefab.GetComponent<BehaviorSpeculator>();
                    component.MovementBehaviors.Add(new CustomCompanionBehaviours.LeadOfLifeCompanionApproach());
                    component.MovementBehaviors.Add(new CompanionFollowPlayerBehavior { IdleAnimations = new string[] { "idle" } });
                }
                if (HungryBulletsCompanion.prefab == null || !CompanionBuilder.companionDictionary.ContainsKey(HungryBulletsCompanion.guid))
                {
                    HungryBulletsCompanion.prefab = CompanionBuilder.BuildPrefab("LeadOfLife HungryBullets", HungryBulletsCompanion.guid, "NevernamedsItems/Resources/Companions/LeadOfLife/hungrybulletscompanion_idleright_001", new IntVector2(9, 0), new IntVector2(7, 7));
                    var companionController = HungryBulletsCompanion.prefab.AddComponent<LeadOfLifeCompanion>();
                    companionController.aiActor.MovementSpeed = 5f;
                    companionController.tiedItemID = 374;
                    companionController.fireCooldown = 2.5f;
                    companionController.tintsBullets = true;
                    companionController.tintColour = ExtendedColours.purple;
                    companionController.hasHungryBullets = true;
                    HungryBulletsCompanion.prefab.AddAnimation("idle_right", "NevernamedsItems/Resources/Companions/LeadOfLife/hungrybulletscompanion_idleright", 7, CompanionBuilder.AnimationType.Idle, DirectionalAnimation.DirectionType.TwoWayHorizontal, DirectionalAnimation.FlipType.None);
                    HungryBulletsCompanion.prefab.AddAnimation("idle_left", "NevernamedsItems/Resources/Companions/LeadOfLife/hungrybulletscompanion_idleleft", 7, CompanionBuilder.AnimationType.Idle, DirectionalAnimation.DirectionType.TwoWayHorizontal, DirectionalAnimation.FlipType.None);
                    HungryBulletsCompanion.prefab.AddAnimation("move_right", "NevernamedsItems/Resources/Companions/LeadOfLife/hungrybulletscompanion_moveright", 7, CompanionBuilder.AnimationType.Move, DirectionalAnimation.DirectionType.TwoWayHorizontal, DirectionalAnimation.FlipType.None);
                    HungryBulletsCompanion.prefab.AddAnimation("move_left", "NevernamedsItems/Resources/Companions/LeadOfLife/hungrybulletscompanion_moveleft", 7, CompanionBuilder.AnimationType.Move, DirectionalAnimation.DirectionType.TwoWayHorizontal, DirectionalAnimation.FlipType.None);
                    BehaviorSpeculator component = HungryBulletsCompanion.prefab.GetComponent<BehaviorSpeculator>();
                    component.MovementBehaviors.Add(new CustomCompanionBehaviours.LeadOfLifeCompanionApproach());
                    component.MovementBehaviors.Add(new CompanionFollowPlayerBehavior { IdleAnimations = new string[] { "idle" } });
                }
                if (OrbitalBulletsCompanion.prefab == null || !CompanionBuilder.companionDictionary.ContainsKey(OrbitalBulletsCompanion.guid))
                {
                    OrbitalBulletsCompanion.prefab = CompanionBuilder.BuildPrefab("LeadOfLife OrbitalBullets", OrbitalBulletsCompanion.guid, "NevernamedsItems/Resources/Companions/LeadOfLife/orbitalbulletscompanion_idle_001", new IntVector2(6, 0), new IntVector2(6, 6));
                    var companionController = OrbitalBulletsCompanion.prefab.AddComponent<LeadOfLifeCompanion>();
                    companionController.aiActor.MovementSpeed = 5f;
                    companionController.tiedItemID = 661;
                    companionController.angleVariance = 45f;
                    companionController.fireCooldown = 0.5f;
                    companionController.damageMultiplier = 0.6f;
                    companionController.hasOrbitalBullets = true;
                    companionController.CanCrossPits = true;
                    companionController.aiActor.ActorShadowOffset = new Vector3(0, -0.5f);
                    OrbitalBulletsCompanion.prefab.AddAnimation("idle", "NevernamedsItems/Resources/Companions/LeadOfLife/orbitalbulletscompanion_idle", 9, CompanionBuilder.AnimationType.Idle, DirectionalAnimation.DirectionType.Single, DirectionalAnimation.FlipType.None);
                    companionController.aiAnimator.GetDirectionalAnimation("idle").Prefix = "idle";
                    BehaviorSpeculator component = OrbitalBulletsCompanion.prefab.GetComponent<BehaviorSpeculator>();
                    component.MovementBehaviors.Add(new CustomCompanionBehaviours.LeadOfLifeCompanionApproach());
                    component.MovementBehaviors.Add(new CompanionFollowPlayerBehavior { IdleAnimations = new string[] { "idle" } });
                }
                if (ShockRoundsCompanion.prefab == null || !CompanionBuilder.companionDictionary.ContainsKey(ShockRoundsCompanion.guid))
                {
                    ShockRoundsCompanion.prefab = CompanionBuilder.BuildPrefab("LeadOfLife ShockRounds", ShockRoundsCompanion.guid, "NevernamedsItems/Resources/Companions/LeadOfLife/shockroundscompanion_idle_001", new IntVector2(6, 0), new IntVector2(7, 7));
                    var companionController = ShockRoundsCompanion.prefab.AddComponent<LeadOfLifeCompanion>();
                    companionController.aiActor.MovementSpeed = 5f;
                    companionController.tiedItemID = 298;
                    companionController.damageMultiplier *= 0.2f;
                    companionController.bulletSpeedMultiplier = 0.25f;
                    companionController.angleVariance = 60;
                    companionController.fireCooldown = 0.25f;
                    companionController.hasShockRounds = true;
                    ShockRoundsCompanion.prefab.AddAnimation("idle", "NevernamedsItems/Resources/Companions/LeadOfLife/shockroundscompanion_idle", 7, CompanionBuilder.AnimationType.Idle, DirectionalAnimation.DirectionType.Single, DirectionalAnimation.FlipType.None);
                    ShockRoundsCompanion.prefab.AddAnimation("run", "NevernamedsItems/Resources/Companions/LeadOfLife/shockroundscompanion_run", 7, CompanionBuilder.AnimationType.Move, DirectionalAnimation.DirectionType.Single, DirectionalAnimation.FlipType.None);
                    companionController.aiAnimator.GetDirectionalAnimation("idle").Prefix = "idle";
                    companionController.aiAnimator.GetDirectionalAnimation("move").Prefix = "run";
                    BehaviorSpeculator component = ShockRoundsCompanion.prefab.GetComponent<BehaviorSpeculator>();
                    component.MovementBehaviors.Add(new CustomCompanionBehaviours.LeadOfLifeCompanionApproach());
                    component.MovementBehaviors.Add(new CompanionFollowPlayerBehavior { IdleAnimations = new string[] { "idle" } });
                }
                if (SnowballetsCompanion.prefab == null || !CompanionBuilder.companionDictionary.ContainsKey(SnowballetsCompanion.guid))
                {
                    SnowballetsCompanion.prefab = CompanionBuilder.BuildPrefab("LeadOfLife Snowballets", SnowballetsCompanion.guid, "NevernamedsItems/Resources/Companions/LeadOfLife/snowballetscompanion_idle_001", new IntVector2(7, 0), new IntVector2(7, 7));
                    var companionController = SnowballetsCompanion.prefab.AddComponent<LeadOfLifeCompanion>();
                    companionController.aiActor.MovementSpeed = 5f;
                    companionController.tiedItemID = 636;
                    companionController.gunIDToCopyBulletsFrom = 402;
                    companionController.damageMultiplier = 1.4284f;
                    companionController.scalesDamageWithPlayerProximity = true;
                    companionController.scaleProxDistant = true;
                    SnowballetsCompanion.prefab.AddAnimation("idle", "NevernamedsItems/Resources/Companions/LeadOfLife/snowballetscompanion_idle", 7, CompanionBuilder.AnimationType.Idle, DirectionalAnimation.DirectionType.Single, DirectionalAnimation.FlipType.None);
                    SnowballetsCompanion.prefab.AddAnimation("run", "NevernamedsItems/Resources/Companions/LeadOfLife/snowballetscompanion_run", 7, CompanionBuilder.AnimationType.Move, DirectionalAnimation.DirectionType.Single, DirectionalAnimation.FlipType.None);
                    companionController.aiAnimator.GetDirectionalAnimation("idle").Prefix = "idle";
                    companionController.aiAnimator.GetDirectionalAnimation("move").Prefix = "run";
                    BehaviorSpeculator component = SnowballetsCompanion.prefab.GetComponent<BehaviorSpeculator>();
                    component.MovementBehaviors.Add(new CustomCompanionBehaviours.LeadOfLifeCompanionApproach());
                    component.MovementBehaviors.Add(new CompanionFollowPlayerBehavior { IdleAnimations = new string[] { "idle" } });
                }
                if (VorpalBulletsCompanion.prefab == null || !CompanionBuilder.companionDictionary.ContainsKey(VorpalBulletsCompanion.guid))
                {
                    VorpalBulletsCompanion.prefab = CompanionBuilder.BuildPrefab("LeadOfLife VorpalBullets", VorpalBulletsCompanion.guid, "NevernamedsItems/Resources/Companions/LeadOfLife/vorpalbulletscompanion_idle_001", new IntVector2(8, 0), new IntVector2(7, 7));
                    var companionController = VorpalBulletsCompanion.prefab.AddComponent<LeadOfLifeCompanion>();
                    companionController.aiActor.MovementSpeed = 5f;
                    companionController.fireCooldown = 7f;
                    companionController.tiedItemID = 640;
                    companionController.overrideCritbullet = true;
                    VorpalBulletsCompanion.prefab.AddAnimation("idle", "NevernamedsItems/Resources/Companions/LeadOfLife/vorpalbulletscompanion_idle", 7, CompanionBuilder.AnimationType.Idle, DirectionalAnimation.DirectionType.Single, DirectionalAnimation.FlipType.None);
                    VorpalBulletsCompanion.prefab.AddAnimation("run", "NevernamedsItems/Resources/Companions/LeadOfLife/vorpalbulletscompanion_run", 7, CompanionBuilder.AnimationType.Move, DirectionalAnimation.DirectionType.Single, DirectionalAnimation.FlipType.None);
                    companionController.aiAnimator.GetDirectionalAnimation("idle").Prefix = "idle";
                    companionController.aiAnimator.GetDirectionalAnimation("move").Prefix = "run";
                    BehaviorSpeculator component = VorpalBulletsCompanion.prefab.GetComponent<BehaviorSpeculator>();
                    component.MovementBehaviors.Add(new CustomCompanionBehaviours.LeadOfLifeCompanionApproach());
                    component.MovementBehaviors.Add(new CompanionFollowPlayerBehavior { IdleAnimations = new string[] { "idle" } });
                }
                if (BlankBulletsCompanion.prefab == null || !CompanionBuilder.companionDictionary.ContainsKey(BlankBulletsCompanion.guid))
                {
                    BlankBulletsCompanion.prefab = CompanionBuilder.BuildPrefab("LeadOfLife BlankBullets", BlankBulletsCompanion.guid, "NevernamedsItems/Resources/Companions/LeadOfLife/blankbulletscompanion_idle_001", new IntVector2(5, 0), new IntVector2(7, 7));
                    var companionController = BlankBulletsCompanion.prefab.AddComponent<LeadOfLifeCompanion>();
                    companionController.aiActor.MovementSpeed = 5f;
                    companionController.fireCooldown = 5;
                    companionController.FiresBullets = false;
                    companionController.doesBlanks = true;
                    companionController.tiedItemID = 579;
                    BlankBulletsCompanion.prefab.AddAnimation("idle", "NevernamedsItems/Resources/Companions/LeadOfLife/blankbulletscompanion_idle", 7, CompanionBuilder.AnimationType.Idle, DirectionalAnimation.DirectionType.Single, DirectionalAnimation.FlipType.None);
                    BlankBulletsCompanion.prefab.AddAnimation("run", "NevernamedsItems/Resources/Companions/LeadOfLife/blankbulletscompanion_run", 7, CompanionBuilder.AnimationType.Move, DirectionalAnimation.DirectionType.Single, DirectionalAnimation.FlipType.None);
                    companionController.aiAnimator.GetDirectionalAnimation("idle").Prefix = "idle";
                    companionController.aiAnimator.GetDirectionalAnimation("move").Prefix = "run";
                    BehaviorSpeculator component = BlankBulletsCompanion.prefab.GetComponent<BehaviorSpeculator>();
                    component.MovementBehaviors.Add(new CustomCompanionBehaviours.LeadOfLifeCompanionApproach());
                    component.MovementBehaviors.Add(new CompanionFollowPlayerBehavior { IdleAnimations = new string[] { "idle" } });
                }
                if (PlatinumBulletsCompanion.prefab == null || !CompanionBuilder.companionDictionary.ContainsKey(PlatinumBulletsCompanion.guid))
                {
                    PlatinumBulletsCompanion.prefab = CompanionBuilder.BuildPrefab("LeadOfLife PlatinumBullets", PlatinumBulletsCompanion.guid, "NevernamedsItems/Resources/Companions/LeadOfLife/platinumbulletscompanion_idle_001", new IntVector2(6, 0), new IntVector2(7, 7));
                    var companionController = PlatinumBulletsCompanion.prefab.AddComponent<LeadOfLifeCompanion>();
                    companionController.aiActor.MovementSpeed = 5f;
                    companionController.tiedItemID = 627;
                    companionController.hasPlatScaling = true;
                    PlatinumBulletsCompanion.prefab.AddAnimation("idle", "NevernamedsItems/Resources/Companions/LeadOfLife/platinumbulletscompanion_idle", 7, CompanionBuilder.AnimationType.Idle, DirectionalAnimation.DirectionType.Single, DirectionalAnimation.FlipType.None);
                    PlatinumBulletsCompanion.prefab.AddAnimation("run", "NevernamedsItems/Resources/Companions/LeadOfLife/platinumbulletscompanion_run", 7, CompanionBuilder.AnimationType.Move, DirectionalAnimation.DirectionType.Single, DirectionalAnimation.FlipType.None);
                    companionController.aiAnimator.GetDirectionalAnimation("idle").Prefix = "idle";
                    companionController.aiAnimator.GetDirectionalAnimation("move").Prefix = "run";
                    BehaviorSpeculator component = PlatinumBulletsCompanion.prefab.GetComponent<BehaviorSpeculator>();
                    component.MovementBehaviors.Add(new CustomCompanionBehaviours.LeadOfLifeCompanionApproach());
                    component.MovementBehaviors.Add(new CompanionFollowPlayerBehavior { IdleAnimations = new string[] { "idle" } });
                }
                #endregion

                #region Other bullety items
                //SECOND GO AROUND
                if (LichsEyeBulletsCompanionA.prefab == null || !CompanionBuilder.companionDictionary.ContainsKey(LichsEyeBulletsCompanionA.guid))
                {
                    LichsEyeBulletsCompanionA.prefab = CompanionBuilder.BuildPrefab("LeadOfLife LichsEyeBulletsA", LichsEyeBulletsCompanionA.guid, "NevernamedsItems/Resources/Companions/LeadOfLife/lichseyebulletscompaniona_idle_001", new IntVector2(6, 2), new IntVector2(6, 6));
                    var companionController = LichsEyeBulletsCompanionA.prefab.AddComponent<LeadOfLifeCompanion>();
                    companionController.aiActor.MovementSpeed = 5f;
                    companionController.tiedItemID = 815;
                    companionController.incrementallyDifferentShots = true;
                    companionController.incrementForAltShots = 7;
                    companionController.incrementShot = (PickupObjectDatabase.GetById(604) as Gun).GetComponent<FireOnReloadSynergyProcessor>().DirectedBurstSettings.ProjectileInterface.SpecifiedProjectile;
                    LichsEyeBulletsCompanionA.prefab.AddAnimation("idle", "NevernamedsItems/Resources/Companions/LeadOfLife/lichseyebulletscompaniona_idle", 7, CompanionBuilder.AnimationType.Idle, DirectionalAnimation.DirectionType.Single, DirectionalAnimation.FlipType.None);
                    LichsEyeBulletsCompanionA.prefab.AddAnimation("run", "NevernamedsItems/Resources/Companions/LeadOfLife/lichseyebulletscompaniona_run", 7, CompanionBuilder.AnimationType.Move, DirectionalAnimation.DirectionType.Single, DirectionalAnimation.FlipType.None);
                    companionController.aiAnimator.GetDirectionalAnimation("idle").Prefix = "idle";
                    companionController.aiAnimator.GetDirectionalAnimation("move").Prefix = "run";
                    BehaviorSpeculator component = LichsEyeBulletsCompanionA.prefab.GetComponent<BehaviorSpeculator>();
                    component.MovementBehaviors.Add(new CustomCompanionBehaviours.LeadOfLifeCompanionApproach());
                    component.MovementBehaviors.Add(new CompanionFollowPlayerBehavior { IdleAnimations = new string[] { "idle" } });
                }

                if (LichsEyeBulletsCompanionB.prefab == null || !CompanionBuilder.companionDictionary.ContainsKey(LichsEyeBulletsCompanionB.guid))
                {
                    LichsEyeBulletsCompanionB.prefab = CompanionBuilder.BuildPrefab("LeadOfLife LichsEyeBulletsB", LichsEyeBulletsCompanionB.guid, "NevernamedsItems/Resources/Companions/LeadOfLife/lichseyebulletscompanionb_idle_001", new IntVector2(6, 2), new IntVector2(6, 6));
                    var companionController = LichsEyeBulletsCompanionB.prefab.AddComponent<LeadOfLifeCompanion>();
                    companionController.aiActor.MovementSpeed = 5f;
                    companionController.tiedItemID = 815;
                    companionController.incrementallyDifferentShots = true;
                    companionController.incrementForAltShots = 7;
                    companionController.incrementShot = (PickupObjectDatabase.GetById(604) as Gun).GetComponent<FireOnReloadSynergyProcessor>().DirectedBurstSettings.ProjectileInterface.SpecifiedProjectile;
                    LichsEyeBulletsCompanionB.prefab.AddAnimation("idle", "NevernamedsItems/Resources/Companions/LeadOfLife/lichseyebulletscompanionb_idle", 7, CompanionBuilder.AnimationType.Idle, DirectionalAnimation.DirectionType.Single, DirectionalAnimation.FlipType.None);
                    LichsEyeBulletsCompanionB.prefab.AddAnimation("run", "NevernamedsItems/Resources/Companions/LeadOfLife/lichseyebulletscompanionb_run", 7, CompanionBuilder.AnimationType.Move, DirectionalAnimation.DirectionType.Single, DirectionalAnimation.FlipType.None);
                    companionController.aiAnimator.GetDirectionalAnimation("idle").Prefix = "idle";
                    companionController.aiAnimator.GetDirectionalAnimation("move").Prefix = "run";
                    BehaviorSpeculator component = LichsEyeBulletsCompanionB.prefab.GetComponent<BehaviorSpeculator>();
                    component.MovementBehaviors.Add(new CustomCompanionBehaviours.LeadOfLifeCompanionApproach());
                    component.MovementBehaviors.Add(new CompanionFollowPlayerBehavior { IdleAnimations = new string[] { "idle" } });
                }
                if (BulletTimeCompanion.prefab == null || !CompanionBuilder.companionDictionary.ContainsKey(BulletTimeCompanion.guid))
                {
                    BulletTimeCompanion.prefab = CompanionBuilder.BuildPrefab("LeadOfLife BulletTime", BulletTimeCompanion.guid, "NevernamedsItems/Resources/Companions/LeadOfLife/bullettimecompanion_idle_001", new IntVector2(6, 2), new IntVector2(7, 7));
                    var companionController = BulletTimeCompanion.prefab.AddComponent<LeadOfLifeCompanion>();
                    companionController.aiActor.MovementSpeed = 7f;
                    companionController.tiedItemID = 69;
                    companionController.fireCooldown = 25;
                    companionController.slowsTime = true;
                    companionController.FiresBullets = false;
                    BulletTimeCompanion.prefab.AddAnimation("idle", "NevernamedsItems/Resources/Companions/LeadOfLife/bullettimecompanion_idle", 7, CompanionBuilder.AnimationType.Idle, DirectionalAnimation.DirectionType.Single, DirectionalAnimation.FlipType.None);
                    BulletTimeCompanion.prefab.AddAnimation("run", "NevernamedsItems/Resources/Companions/LeadOfLife/bullettimecompanion_run", 7, CompanionBuilder.AnimationType.Move, DirectionalAnimation.DirectionType.Single, DirectionalAnimation.FlipType.None);
                    companionController.aiAnimator.GetDirectionalAnimation("idle").Prefix = "idle";
                    companionController.aiAnimator.GetDirectionalAnimation("move").Prefix = "run";
                    BehaviorSpeculator component = BulletTimeCompanion.prefab.GetComponent<BehaviorSpeculator>();
                    component.MovementBehaviors.Add(new CompanionFollowPlayerBehavior { IdleAnimations = new string[] { "idle" } });
                }
                if (DarumaCompanion.prefab == null || !CompanionBuilder.companionDictionary.ContainsKey(DarumaCompanion.guid))
                {
                    DarumaCompanion.prefab = CompanionBuilder.BuildPrefab("LeadOfLife Daruma", DarumaCompanion.guid, "NevernamedsItems/Resources/Companions/LeadOfLife/darumacompanion_idle_001", new IntVector2(6, 2), new IntVector2(7, 7));
                    var companionController = DarumaCompanion.prefab.AddComponent<LeadOfLifeCompanion>();
                    companionController.aiActor.MovementSpeed = 5f;
                    companionController.tiedItemID = 643;
                    companionController.gunIDToCopyBulletsFrom = 53;
                    companionController.attackOnTimer = false;
                    companionController.fireOnActiveItemUse = true;
                    companionController.doesBlanks = true;
                    companionController.blankType = EasyBlankType.MINI;
                    companionController.activeItemIDToCheckFor = 643;
                    DarumaCompanion.prefab.AddAnimation("idle", "NevernamedsItems/Resources/Companions/LeadOfLife/darumacompanion_idle", 7, CompanionBuilder.AnimationType.Idle, DirectionalAnimation.DirectionType.Single, DirectionalAnimation.FlipType.None);
                    DarumaCompanion.prefab.AddAnimation("run", "NevernamedsItems/Resources/Companions/LeadOfLife/darumacompanion_run", 7, CompanionBuilder.AnimationType.Move, DirectionalAnimation.DirectionType.Single, DirectionalAnimation.FlipType.None);
                    companionController.aiAnimator.GetDirectionalAnimation("idle").Prefix = "idle";
                    companionController.aiAnimator.GetDirectionalAnimation("move").Prefix = "run";
                    BehaviorSpeculator component = DarumaCompanion.prefab.GetComponent<BehaviorSpeculator>();
                    component.MovementBehaviors.Add(new CustomCompanionBehaviours.LeadOfLifeCompanionApproach());
                    component.MovementBehaviors.Add(new CompanionFollowPlayerBehavior { IdleAnimations = new string[] { "idle" } });
                }
                if (RiddleOfLeadCompanion.prefab == null || !CompanionBuilder.companionDictionary.ContainsKey(RiddleOfLeadCompanion.guid))
                {
                    RiddleOfLeadCompanion.prefab = CompanionBuilder.BuildPrefab("LeadOfLife RiddleOfLead", RiddleOfLeadCompanion.guid, "NevernamedsItems/Resources/Companions/LeadOfLife/riddleofleadcompanion_idle_001", new IntVector2(6, 2), new IntVector2(8, 7));
                    var companionController = RiddleOfLeadCompanion.prefab.AddComponent<LeadOfLifeCompanion>();
                    companionController.aiActor.MovementSpeed = 3.5f;
                    companionController.tiedItemID = 271;
                    companionController.usesGunOfAThousandSinsBullets = true;
                    companionController.fireCooldown = 5;
                    RiddleOfLeadCompanion.prefab.AddAnimation("idle", "NevernamedsItems/Resources/Companions/LeadOfLife/riddleofleadcompanion_idle", 7, CompanionBuilder.AnimationType.Idle, DirectionalAnimation.DirectionType.Single, DirectionalAnimation.FlipType.None);
                    RiddleOfLeadCompanion.prefab.AddAnimation("run", "NevernamedsItems/Resources/Companions/LeadOfLife/riddleofleadcompanion_run", 7, CompanionBuilder.AnimationType.Move, DirectionalAnimation.DirectionType.Single, DirectionalAnimation.FlipType.None);
                    companionController.aiAnimator.GetDirectionalAnimation("idle").Prefix = "idle";
                    companionController.aiAnimator.GetDirectionalAnimation("move").Prefix = "run";
                    BehaviorSpeculator component = RiddleOfLeadCompanion.prefab.GetComponent<BehaviorSpeculator>();
                    component.MovementBehaviors.Add(new CustomCompanionBehaviours.LeadOfLifeCompanionApproach());
                    component.MovementBehaviors.Add(new CompanionFollowPlayerBehavior { IdleAnimations = new string[] { "idle" } });
                }
                if (ShotgunCoffeeCompanion.prefab == null || !CompanionBuilder.companionDictionary.ContainsKey(ShotgunCoffeeCompanion.guid))
                {
                    ShotgunCoffeeCompanion.prefab = CompanionBuilder.BuildPrefab("LeadOfLife ShotgunCoffee", ShotgunCoffeeCompanion.guid, "NevernamedsItems/Resources/Companions/LeadOfLife/shotguncoffeecompanion_idle_001", new IntVector2(6, 2), new IntVector2(7, 7));
                    var companionController = ShotgunCoffeeCompanion.prefab.AddComponent<LeadOfLifeCompanion>();
                    companionController.aiActor.MovementSpeed = 10f;
                    companionController.tiedItemID = 427;
                    companionController.doGoop = true;
                    companionController.FiresBullets = false;
                    companionController.fireCooldown = 4.5f;
                    companionController.goopRadiusOrWidth = 3f;
                    companionController.goopDefToSpawn = EasyGoopDefinitions.GenerateBloodGoop(5f, ExtendedColours.brown, 10);
                    ShotgunCoffeeCompanion.prefab.AddAnimation("idle", "NevernamedsItems/Resources/Companions/LeadOfLife/shotguncoffeecompanion_idle", 11, CompanionBuilder.AnimationType.Idle, DirectionalAnimation.DirectionType.Single, DirectionalAnimation.FlipType.None);
                    ShotgunCoffeeCompanion.prefab.AddAnimation("run", "NevernamedsItems/Resources/Companions/LeadOfLife/shotguncoffeecompanion_run", 11, CompanionBuilder.AnimationType.Move, DirectionalAnimation.DirectionType.Single, DirectionalAnimation.FlipType.None);
                    companionController.aiAnimator.GetDirectionalAnimation("idle").Prefix = "idle";
                    companionController.aiAnimator.GetDirectionalAnimation("move").Prefix = "run";
                    BehaviorSpeculator component = ShotgunCoffeeCompanion.prefab.GetComponent<BehaviorSpeculator>();
                    component.MovementBehaviors.Add(new CustomCompanionBehaviours.LeadOfLifeCompanionApproach() { DesiredDistance = 2f }); ;
                    component.MovementBehaviors.Add(new CompanionFollowPlayerBehavior { IdleAnimations = new string[] { "idle" } });
                }
                if (ShotgaColaCompanion.prefab == null || !CompanionBuilder.companionDictionary.ContainsKey(ShotgaColaCompanion.guid))
                {
                    ShotgaColaCompanion.prefab = CompanionBuilder.BuildPrefab("LeadOfLife ShotgaCola", ShotgaColaCompanion.guid, "NevernamedsItems/Resources/Companions/LeadOfLife/shotgacolacompanion_idle_001", new IntVector2(5, 2), new IntVector2(7, 7));
                    var companionController = ShotgaColaCompanion.prefab.AddComponent<LeadOfLifeCompanion>();
                    companionController.aiActor.MovementSpeed = 10f;
                    companionController.tiedItemID = 426;
                    companionController.doGoop = true;
                    companionController.FiresBullets = false;
                    companionController.fireCooldown = 2f;
                    companionController.goopRadiusOrWidth = 1.5f;
                    companionController.doLineGoop = true;
                    companionController.goopDefToSpawn = EasyGoopDefinitions.GenerateBloodGoop(5f, ExtendedColours.brown, 10);
                    ShotgaColaCompanion.prefab.AddAnimation("idle", "NevernamedsItems/Resources/Companions/LeadOfLife/shotgacolacompanion_idle", 11, CompanionBuilder.AnimationType.Idle, DirectionalAnimation.DirectionType.Single, DirectionalAnimation.FlipType.None);
                    ShotgaColaCompanion.prefab.AddAnimation("run", "NevernamedsItems/Resources/Companions/LeadOfLife/shotgacolacompanion_run", 11, CompanionBuilder.AnimationType.Move, DirectionalAnimation.DirectionType.Single, DirectionalAnimation.FlipType.None);
                    companionController.aiAnimator.GetDirectionalAnimation("idle").Prefix = "idle";
                    companionController.aiAnimator.GetDirectionalAnimation("move").Prefix = "run";
                    BehaviorSpeculator component = ShotgaColaCompanion.prefab.GetComponent<BehaviorSpeculator>();
                    component.MovementBehaviors.Add(new CustomCompanionBehaviours.LeadOfLifeCompanionApproach());
                    component.MovementBehaviors.Add(new CompanionFollowPlayerBehavior { IdleAnimations = new string[] { "idle" } });
                }
                if (ElderBlankCompanion.prefab == null || !CompanionBuilder.companionDictionary.ContainsKey(ElderBlankCompanion.guid))
                {
                    ElderBlankCompanion.prefab = CompanionBuilder.BuildPrefab("LeadOfLife ElderBlank", ElderBlankCompanion.guid, "NevernamedsItems/Resources/Companions/LeadOfLife/elderblankcompanion_idle_001", new IntVector2(5, 2), new IntVector2(7, 7));
                    var companionController = ElderBlankCompanion.prefab.AddComponent<LeadOfLifeCompanion>();
                    companionController.aiActor.MovementSpeed = 5f;
                    companionController.tiedItemID = 499;
                    companionController.FiresBullets = false;
                    companionController.attackOnTimer = false;
                    companionController.fireOnActiveItemUse = true;
                    companionController.doesBlanks = true;
                    companionController.roomDMGAmount = 20;
                    companionController.blankType = EasyBlankType.FULL;
                    companionController.activeItemIDToCheckFor = 499;
                    ElderBlankCompanion.prefab.AddAnimation("idle", "NevernamedsItems/Resources/Companions/LeadOfLife/elderblankcompanion_idle", 7, CompanionBuilder.AnimationType.Idle, DirectionalAnimation.DirectionType.Single, DirectionalAnimation.FlipType.None);
                    ElderBlankCompanion.prefab.AddAnimation("run", "NevernamedsItems/Resources/Companions/LeadOfLife/elderblankcompanion_run", 7, CompanionBuilder.AnimationType.Move, DirectionalAnimation.DirectionType.Single, DirectionalAnimation.FlipType.None);
                    companionController.aiAnimator.GetDirectionalAnimation("idle").Prefix = "idle";
                    companionController.aiAnimator.GetDirectionalAnimation("move").Prefix = "run";
                    BehaviorSpeculator component = ElderBlankCompanion.prefab.GetComponent<BehaviorSpeculator>();
                    component.MovementBehaviors.Add(new CustomCompanionBehaviours.LeadOfLifeCompanionApproach());
                    component.MovementBehaviors.Add(new CompanionFollowPlayerBehavior { IdleAnimations = new string[] { "idle" } });
                }
                if (BulletGunCompanion.prefab == null || !CompanionBuilder.companionDictionary.ContainsKey(BulletGunCompanion.guid))
                {
                    BulletGunCompanion.prefab = CompanionBuilder.BuildPrefab("LeadOfLife BulletGun", BulletGunCompanion.guid, "NevernamedsItems/Resources/Companions/LeadOfLife/bulletguncompanion_idle_001", new IntVector2(4, 1), new IntVector2(5, 5));
                    var companionController = BulletGunCompanion.prefab.AddComponent<LeadOfLifeCompanion>();
                    companionController.aiActor.MovementSpeed = 5f;
                    companionController.tiedItemID = 503;
                    companionController.fireCooldown = 1.6f;
                    companionController.gunIDToCopyBulletsFrom = 503;
                    BulletGunCompanion.prefab.AddAnimation("idle", "NevernamedsItems/Resources/Companions/LeadOfLife/bulletguncompanion_idle", 7, CompanionBuilder.AnimationType.Idle, DirectionalAnimation.DirectionType.Single, DirectionalAnimation.FlipType.None);
                    BulletGunCompanion.prefab.AddAnimation("run", "NevernamedsItems/Resources/Companions/LeadOfLife/bulletguncompanion_run", 7, CompanionBuilder.AnimationType.Move, DirectionalAnimation.DirectionType.Single, DirectionalAnimation.FlipType.None);
                    companionController.aiAnimator.GetDirectionalAnimation("idle").Prefix = "idle";
                    companionController.aiAnimator.GetDirectionalAnimation("move").Prefix = "run";
                    BehaviorSpeculator component = BulletGunCompanion.prefab.GetComponent<BehaviorSpeculator>();
                    component.MovementBehaviors.Add(new CustomCompanionBehaviours.LeadOfLifeCompanionApproach());
                    component.MovementBehaviors.Add(new CompanionFollowPlayerBehavior { IdleAnimations = new string[] { "idle" } });
                }
                if (ShellGunCompanion.prefab == null || !CompanionBuilder.companionDictionary.ContainsKey(ShellGunCompanion.guid))
                {
                    ShellGunCompanion.prefab = CompanionBuilder.BuildPrefab("LeadOfLife ShellGun", ShellGunCompanion.guid, "NevernamedsItems/Resources/Companions/LeadOfLife/shellguncompanion_idle_001", new IntVector2(7, 2), new IntVector2(7, 7));
                    var companionController = ShellGunCompanion.prefab.AddComponent<LeadOfLifeCompanion>();
                    companionController.aiActor.MovementSpeed = 5f;
                    companionController.tiedItemID = 512;
                    companionController.gunIDToCopyBulletsFrom = 512;
                    companionController.angleVariance = 16;
                    companionController.attacksPerActivation = 3;
                    companionController.fireCooldown = 2f;
                    ShellGunCompanion.prefab.AddAnimation("idle", "NevernamedsItems/Resources/Companions/LeadOfLife/shellguncompanion_idle", 7, CompanionBuilder.AnimationType.Idle, DirectionalAnimation.DirectionType.Single, DirectionalAnimation.FlipType.None);
                    ShellGunCompanion.prefab.AddAnimation("run", "NevernamedsItems/Resources/Companions/LeadOfLife/shellguncompanion_run", 7, CompanionBuilder.AnimationType.Move, DirectionalAnimation.DirectionType.Single, DirectionalAnimation.FlipType.None);
                    companionController.aiAnimator.GetDirectionalAnimation("idle").Prefix = "idle";
                    companionController.aiAnimator.GetDirectionalAnimation("move").Prefix = "run";
                    BehaviorSpeculator component = ShellGunCompanion.prefab.GetComponent<BehaviorSpeculator>();
                    component.MovementBehaviors.Add(new CustomCompanionBehaviours.LeadOfLifeCompanionApproach());
                    component.MovementBehaviors.Add(new CompanionFollowPlayerBehavior { IdleAnimations = new string[] { "idle" } });
                }
                if (CaseyCompanion.prefab == null || !CompanionBuilder.companionDictionary.ContainsKey(CaseyCompanion.guid))
                {
                    CaseyCompanion.prefab = CompanionBuilder.BuildPrefab("LeadOfLife Casey", CaseyCompanion.guid, "NevernamedsItems/Resources/Companions/LeadOfLife/caseycompanion_idle_001", new IntVector2(8, 2), new IntVector2(4, 4));
                    var companionController = CaseyCompanion.prefab.AddComponent<LeadOfLifeCompanion>();
                    companionController.aiActor.MovementSpeed = 5f;
                    companionController.tiedItemID = 541;
                    companionController.gunIDToCopyBulletsFrom = 541;
                    companionController.fireCooldown = 3.5f;
                    CaseyCompanion.prefab.AddAnimation("idle", "NevernamedsItems/Resources/Companions/LeadOfLife/caseycompanion_idle", 7, CompanionBuilder.AnimationType.Idle, DirectionalAnimation.DirectionType.Single, DirectionalAnimation.FlipType.None);
                    CaseyCompanion.prefab.AddAnimation("run", "NevernamedsItems/Resources/Companions/LeadOfLife/caseycompanion_run", 7, CompanionBuilder.AnimationType.Move, DirectionalAnimation.DirectionType.Single, DirectionalAnimation.FlipType.None);
                    companionController.aiAnimator.GetDirectionalAnimation("idle").Prefix = "idle";
                    companionController.aiAnimator.GetDirectionalAnimation("move").Prefix = "run";
                    BehaviorSpeculator component = CaseyCompanion.prefab.GetComponent<BehaviorSpeculator>();
                    component.MovementBehaviors.Add(new CustomCompanionBehaviours.LeadOfLifeCompanionApproach() { DesiredDistance = 3f });
                    component.MovementBehaviors.Add(new CompanionFollowPlayerBehavior { IdleAnimations = new string[] { "idle" } });
                }
                if (BTCKTPCompanion.prefab == null || !CompanionBuilder.companionDictionary.ContainsKey(BTCKTPCompanion.guid))
                {
                    BTCKTPCompanion.prefab = CompanionBuilder.BuildPrefab("LeadOfLife BTCKTP", BTCKTPCompanion.guid, "NevernamedsItems/Resources/Companions/LeadOfLife/BTCKTPcompanion_idle_001", new IntVector2(6, 2), new IntVector2(8, 8));
                    var companionController = BTCKTPCompanion.prefab.AddComponent<LeadOfLifeCompanion>();
                    companionController.aiActor.MovementSpeed = 7f;
                    companionController.tiedItemID = 303;
                    companionController.CanInterceptBullets = true;
                    companionController.FiresBullets = false;
                    BTCKTPCompanion.prefab.AddAnimation("idle", "NevernamedsItems/Resources/Companions/LeadOfLife/BTCKTPcompanion_idle", 7, CompanionBuilder.AnimationType.Idle, DirectionalAnimation.DirectionType.Single, DirectionalAnimation.FlipType.None);
                    BTCKTPCompanion.prefab.AddAnimation("run", "NevernamedsItems/Resources/Companions/LeadOfLife/BTCKTPcompanion_run", 7, CompanionBuilder.AnimationType.Move, DirectionalAnimation.DirectionType.Single, DirectionalAnimation.FlipType.None);
                    companionController.aiAnimator.GetDirectionalAnimation("idle").Prefix = "idle";
                    companionController.aiAnimator.GetDirectionalAnimation("move").Prefix = "run";
                    companionController.aiActor.CollisionDamage = 0f;
                    //companionController.overrideShader = "Brave/Internal/StarNest_Derivative";
                    companionController.aiActor.specRigidbody.CollideWithOthers = true;
                    companionController.aiActor.specRigidbody.CollideWithTileMap = false;
                    companionController.healthHaver.PreventAllDamage = true;
                    companionController.aiActor.specRigidbody.PixelColliders.Clear();
                    companionController.aiActor.specRigidbody.PixelColliders.Add(new PixelCollider
                    {
                        ColliderGenerationMode = PixelCollider.PixelColliderGeneration.Manual,
                        CollisionLayer = CollisionLayer.EnemyCollider,
                        IsTrigger = false,
                        BagleUseFirstFrameOnly = false,
                        SpecifyBagelFrame = string.Empty,
                        BagelColliderNumber = 0,
                        ManualOffsetX = 6,
                        ManualOffsetY = 2,
                        ManualWidth = 8,
                        ManualHeight = 17,
                        ManualDiameter = 0,
                        ManualLeftX = 0,
                        ManualLeftY = 0,
                        ManualRightX = 0,
                        ManualRightY = 0
                    });
                    companionController.aiAnimator.specRigidbody.PixelColliders.Add(new PixelCollider
                    {
                        ColliderGenerationMode = PixelCollider.PixelColliderGeneration.Manual,
                        CollisionLayer = CollisionLayer.PlayerHitBox,
                        IsTrigger = false,
                        BagleUseFirstFrameOnly = false,
                        SpecifyBagelFrame = string.Empty,
                        BagelColliderNumber = 0,
                        ManualOffsetX = 6,
                        ManualOffsetY = 2,
                        ManualWidth = 8,
                        ManualHeight = 8,
                        ManualDiameter = 0,
                        ManualLeftX = 0,
                        ManualLeftY = 0,
                        ManualRightX = 0,
                        ManualRightY = 0
                    });
                    BehaviorSpeculator component = BTCKTPCompanion.prefab.GetComponent<BehaviorSpeculator>();
                    component.MovementBehaviors.Add(new CompanionFollowPlayerBehavior { IdleAnimations = new string[] { "idle" } });
                }
                #endregion

                #region Custom Bullet Modifiers
                if (OneShotCompanion.prefab == null || !CompanionBuilder.companionDictionary.ContainsKey(OneShotCompanion.guid))
                {
                    OneShotCompanion.prefab = CompanionBuilder.BuildPrefab("LeadOfLife OneShot", OneShotCompanion.guid, "NevernamedsItems/Resources/Companions/LeadOfLifeModded/oneshotcompanion_idle_001", new IntVector2(10, 1), new IntVector2(7, 7));
                    var companionController = OneShotCompanion.prefab.AddComponent<LeadOfLifeCompanion>();
                    companionController.aiActor.MovementSpeed = 8f;
                    companionController.tiedItemID = OneShot.OneShotID;
                    companionController.damageMultiplier = 2;
                    companionController.bulletScaleMultiplier = 2;
                    companionController.bulletSpeedMultiplier = 2;
                    companionController.rangeMultiplier = 2;
                    companionController.knockbackMult = 2;
                    companionController.bossDamageMult = 2;
                    companionController.jammedDamageMult = 2;
                    companionController.fireCooldown = 0.65f;
                    OneShotCompanion.prefab.AddAnimation("idle", "NevernamedsItems/Resources/Companions/LeadOfLifeModded/oneshotcompanion_idle", 7, CompanionBuilder.AnimationType.Idle, DirectionalAnimation.DirectionType.Single, DirectionalAnimation.FlipType.None);
                    OneShotCompanion.prefab.AddAnimation("run", "NevernamedsItems/Resources/Companions/LeadOfLifeModded/oneshotcompanion_run", 7, CompanionBuilder.AnimationType.Move, DirectionalAnimation.DirectionType.Single, DirectionalAnimation.FlipType.None);
                    companionController.aiAnimator.GetDirectionalAnimation("idle").Prefix = "idle";
                    companionController.aiAnimator.GetDirectionalAnimation("move").Prefix = "run";
                    BehaviorSpeculator component = OneShotCompanion.prefab.GetComponent<BehaviorSpeculator>();
                    component.MovementBehaviors.Add(new CustomCompanionBehaviours.LeadOfLifeCompanionApproach());
                    component.MovementBehaviors.Add(new CompanionFollowPlayerBehavior { IdleAnimations = new string[] { "idle" } });
                }
                if (FiftyCalRoundsCompanion.prefab == null || !CompanionBuilder.companionDictionary.ContainsKey(FiftyCalRoundsCompanion.guid))
                {
                    FiftyCalRoundsCompanion.prefab = CompanionBuilder.BuildPrefab("LeadOfLife FiftyCalRounds", FiftyCalRoundsCompanion.guid, "NevernamedsItems/Resources/Companions/LeadOfLifeModded/fiftycalroundscompanion_idle_001", new IntVector2(6, 1), new IntVector2(6, 6));
                    var companionController = FiftyCalRoundsCompanion.prefab.AddComponent<LeadOfLifeCompanion>();
                    companionController.aiActor.MovementSpeed = 5f;
                    companionController.tiedItemID = FiftyCalRounds.FiftyCalRoundsID;
                    companionController.damageMultiplier = 1.16f;
                    companionController.bulletSpeedMultiplier = 1.25f;
                    FiftyCalRoundsCompanion.prefab.AddAnimation("idle", "NevernamedsItems/Resources/Companions/LeadOfLifeModded/fiftycalroundscompanion_idle", 7, CompanionBuilder.AnimationType.Idle, DirectionalAnimation.DirectionType.Single, DirectionalAnimation.FlipType.None);
                    FiftyCalRoundsCompanion.prefab.AddAnimation("run", "NevernamedsItems/Resources/Companions/LeadOfLifeModded/fiftycalroundscompanion_run", 7, CompanionBuilder.AnimationType.Move, DirectionalAnimation.DirectionType.Single, DirectionalAnimation.FlipType.None);
                    companionController.aiAnimator.GetDirectionalAnimation("idle").Prefix = "idle";
                    companionController.aiAnimator.GetDirectionalAnimation("move").Prefix = "run";
                    BehaviorSpeculator component = FiftyCalRoundsCompanion.prefab.GetComponent<BehaviorSpeculator>();
                    component.MovementBehaviors.Add(new CustomCompanionBehaviours.LeadOfLifeCompanionApproach());
                    component.MovementBehaviors.Add(new CompanionFollowPlayerBehavior { IdleAnimations = new string[] { "idle" } });
                }
                if (AlkaliBulletsCompanion.prefab == null || !CompanionBuilder.companionDictionary.ContainsKey(AlkaliBulletsCompanion.guid))
                {
                    AlkaliBulletsCompanion.prefab = CompanionBuilder.BuildPrefab("LeadOfLife AlkaliBullets", AlkaliBulletsCompanion.guid, "NevernamedsItems/Resources/Companions/LeadOfLifeModded/alkalibulletscompanion_idle_001", new IntVector2(6, 1), new IntVector2(7, 7));
                    var companionController = AlkaliBulletsCompanion.prefab.AddComponent<LeadOfLifeCompanion>();
                    companionController.aiActor.MovementSpeed = 5f;
                    companionController.tiedItemID = AlkaliBullets.AlkaliBulletsID;
                    companionController.guidsToInstaKill.AddRange(EasyEnemyTypeLists.ModInclusiveBlobsALL);
                    AlkaliBulletsCompanion.prefab.AddAnimation("idle", "NevernamedsItems/Resources/Companions/LeadOfLifeModded/alkalibulletscompanion_idle", 7, CompanionBuilder.AnimationType.Idle, DirectionalAnimation.DirectionType.Single, DirectionalAnimation.FlipType.None);
                    AlkaliBulletsCompanion.prefab.AddAnimation("run", "NevernamedsItems/Resources/Companions/LeadOfLifeModded/alkalibulletscompanion_run", 7, CompanionBuilder.AnimationType.Move, DirectionalAnimation.DirectionType.Single, DirectionalAnimation.FlipType.None);
                    companionController.aiAnimator.GetDirectionalAnimation("idle").Prefix = "idle";
                    companionController.aiAnimator.GetDirectionalAnimation("move").Prefix = "run";
                    BehaviorSpeculator component = AlkaliBulletsCompanion.prefab.GetComponent<BehaviorSpeculator>();
                    component.MovementBehaviors.Add(new CustomCompanionBehaviours.LeadOfLifeCompanionApproach());
                    component.MovementBehaviors.Add(new CompanionFollowPlayerBehavior { IdleAnimations = new string[] { "idle" } });
                }
                if (AntimagicRoundsCompanion.prefab == null || !CompanionBuilder.companionDictionary.ContainsKey(AntimagicRoundsCompanion.guid))
                {
                    AntimagicRoundsCompanion.prefab = CompanionBuilder.BuildPrefab("LeadOfLife AntimagicRounds", AntimagicRoundsCompanion.guid, "NevernamedsItems/Resources/Companions/LeadOfLifeModded/antimagicroundscompanion_idle_001", new IntVector2(9, 1), new IntVector2(7, 7));
                    var companionController = AntimagicRoundsCompanion.prefab.AddComponent<LeadOfLifeCompanion>();
                    companionController.aiActor.MovementSpeed = 5f;
                    companionController.tintsBullets = true;
                    companionController.tintColour = ExtendedColours.purple;
                    companionController.tiedItemID = AntimagicRounds.AntimagicRoundsID;
                    companionController.guidsToInstaKill.AddRange(EasyEnemyTypeLists.ModInclusiveMagicEnemies);
                    AntimagicRoundsCompanion.prefab.AddAnimation("idle", "NevernamedsItems/Resources/Companions/LeadOfLifeModded/antimagicroundscompanion_idle", 7, CompanionBuilder.AnimationType.Idle, DirectionalAnimation.DirectionType.Single, DirectionalAnimation.FlipType.None);
                    AntimagicRoundsCompanion.prefab.AddAnimation("run", "NevernamedsItems/Resources/Companions/LeadOfLifeModded/antimagicroundscompanion_run", 7, CompanionBuilder.AnimationType.Move, DirectionalAnimation.DirectionType.Single, DirectionalAnimation.FlipType.None);
                    companionController.aiAnimator.GetDirectionalAnimation("idle").Prefix = "idle";
                    companionController.aiAnimator.GetDirectionalAnimation("move").Prefix = "run";
                    BehaviorSpeculator component = AntimagicRoundsCompanion.prefab.GetComponent<BehaviorSpeculator>();
                    component.MovementBehaviors.Add(new CustomCompanionBehaviours.LeadOfLifeCompanionApproach());
                    component.MovementBehaviors.Add(new CompanionFollowPlayerBehavior { IdleAnimations = new string[] { "idle" } });
                }
                if (AntimatterBulletsCompanion.prefab == null || !CompanionBuilder.companionDictionary.ContainsKey(AntimatterBulletsCompanion.guid))
                {
                    AntimatterBulletsCompanion.prefab = CompanionBuilder.BuildPrefab("LeadOfLife AntimatterBullets", AntimatterBulletsCompanion.guid, "NevernamedsItems/Resources/Companions/LeadOfLifeModded/antimatterbulletscompanion_idle_001", new IntVector2(6, 1), new IntVector2(7, 7));
                    var companionController = AntimatterBulletsCompanion.prefab.AddComponent<LeadOfLifeCompanion>();
                    companionController.aiActor.MovementSpeed = 5f;
                    companionController.doesIntersectionEffect = true;
                    companionController.explodeOnIntersection = true;
                    companionController.tiedItemID = AntimatterBullets.AntimatterBulletsID;
                    AntimatterBulletsCompanion.prefab.AddAnimation("idle", "NevernamedsItems/Resources/Companions/LeadOfLifeModded/antimatterbulletscompanion_idle", 7, CompanionBuilder.AnimationType.Idle, DirectionalAnimation.DirectionType.Single, DirectionalAnimation.FlipType.None);
                    AntimatterBulletsCompanion.prefab.AddAnimation("run", "NevernamedsItems/Resources/Companions/LeadOfLifeModded/antimatterbulletscompanion_run", 7, CompanionBuilder.AnimationType.Move, DirectionalAnimation.DirectionType.Single, DirectionalAnimation.FlipType.None);
                    companionController.aiAnimator.GetDirectionalAnimation("idle").Prefix = "idle";
                    companionController.aiAnimator.GetDirectionalAnimation("move").Prefix = "run";
                    BehaviorSpeculator component = AntimatterBulletsCompanion.prefab.GetComponent<BehaviorSpeculator>();
                    component.MovementBehaviors.Add(new CustomCompanionBehaviours.LeadOfLifeCompanionApproach());
                    component.MovementBehaviors.Add(new CompanionFollowPlayerBehavior { IdleAnimations = new string[] { "idle" } });
                }
                if (BashfulShotCompanion.prefab == null || !CompanionBuilder.companionDictionary.ContainsKey(BashfulShotCompanion.guid))
                {
                    BashfulShotCompanion.prefab = CompanionBuilder.BuildPrefab("LeadOfLife BashfulShot", BashfulShotCompanion.guid, "NevernamedsItems/Resources/Companions/LeadOfLifeModded/bashfulshotcompanion_idle_001", new IntVector2(5, 1), new IntVector2(8, 8));
                    var companionController = BashfulShotCompanion.prefab.AddComponent<LeadOfLifeCompanion>();
                    companionController.aiActor.MovementSpeed = 5f;
                    companionController.damageMultiplier = 0.5f;
                    companionController.tiedItemID = BashfulShot.BashfulShotID;
                    companionController.gunIDToCopyBulletsFrom = 9;
                    BashfulShotCompanion.prefab.AddAnimation("idle", "NevernamedsItems/Resources/Companions/LeadOfLifeModded/bashfulshotcompanion_idle", 7, CompanionBuilder.AnimationType.Idle, DirectionalAnimation.DirectionType.Single, DirectionalAnimation.FlipType.None);
                    BashfulShotCompanion.prefab.AddAnimation("run", "NevernamedsItems/Resources/Companions/LeadOfLifeModded/bashfulshotcompanion_run", 9, CompanionBuilder.AnimationType.Move, DirectionalAnimation.DirectionType.Single, DirectionalAnimation.FlipType.None);
                    companionController.aiAnimator.GetDirectionalAnimation("idle").Prefix = "idle";
                    companionController.aiAnimator.GetDirectionalAnimation("move").Prefix = "run";
                    BehaviorSpeculator component = BashfulShotCompanion.prefab.GetComponent<BehaviorSpeculator>();
                    component.MovementBehaviors.Add(new CustomCompanionBehaviours.LeadOfLifeCompanionApproach());
                    component.MovementBehaviors.Add(new CompanionFollowPlayerBehavior { IdleAnimations = new string[] { "idle" } });
                }
                if (BashingBulletsCompanion.prefab == null || !CompanionBuilder.companionDictionary.ContainsKey(BashingBulletsCompanion.guid))
                {
                    BashingBulletsCompanion.prefab = CompanionBuilder.BuildPrefab("LeadOfLife BashingBullets", BashingBulletsCompanion.guid, "NevernamedsItems/Resources/Companions/LeadOfLifeModded/bashingbulletscompanion_idle_001", new IntVector2(7, 1), new IntVector2(7, 7));
                    var companionController = BashingBulletsCompanion.prefab.AddComponent<LeadOfLifeCompanion>();
                    companionController.aiActor.MovementSpeed = 5f;
                    companionController.gunIDToCopyBulletsFrom = 541;
                    companionController.damageMultiplier = 0.125f;
                    companionController.rangeMultiplier = 0.5f;
                    companionController.stunInflictChance = 0.5f;
                    companionController.inflictedStunDuration = 1;
                    companionController.tiedItemID = BashingBullets.BashingBulletsID;
                    BashingBulletsCompanion.prefab.AddAnimation("idle", "NevernamedsItems/Resources/Companions/LeadOfLifeModded/bashingbulletscompanion_idle", 7, CompanionBuilder.AnimationType.Idle, DirectionalAnimation.DirectionType.Single, DirectionalAnimation.FlipType.None);
                    BashingBulletsCompanion.prefab.AddAnimation("run", "NevernamedsItems/Resources/Companions/LeadOfLifeModded/bashingbulletscompanion_run", 7, CompanionBuilder.AnimationType.Move, DirectionalAnimation.DirectionType.Single, DirectionalAnimation.FlipType.None);
                    companionController.aiAnimator.GetDirectionalAnimation("idle").Prefix = "idle";
                    companionController.aiAnimator.GetDirectionalAnimation("move").Prefix = "run";
                    BehaviorSpeculator component = BashingBulletsCompanion.prefab.GetComponent<BehaviorSpeculator>();
                    component.MovementBehaviors.Add(new CustomCompanionBehaviours.LeadOfLifeCompanionApproach() { DesiredDistance = 2 });
                    component.MovementBehaviors.Add(new CompanionFollowPlayerBehavior { IdleAnimations = new string[] { "idle" } });
                }
                if (BirdshotCompanion.prefab == null || !CompanionBuilder.companionDictionary.ContainsKey(BirdshotCompanion.guid))
                {
                    BirdshotCompanion.prefab = CompanionBuilder.BuildPrefab("LeadOfLife Birdshot", BirdshotCompanion.guid, "NevernamedsItems/Resources/Companions/LeadOfLifeModded/birdshotcompanion_idle_001", new IntVector2(14, 1), new IntVector2(7, 7));
                    var companionController = BirdshotCompanion.prefab.AddComponent<LeadOfLifeCompanion>();
                    companionController.aiActor.MovementSpeed = 5f;
                    companionController.tiedItemID = Birdshot.BirdshotID;
                    companionController.dmgUpOnFlyingEnemies = true;
                    BirdshotCompanion.prefab.AddAnimation("idle", "NevernamedsItems/Resources/Companions/LeadOfLifeModded/birdshotcompanion_idle", 7, CompanionBuilder.AnimationType.Idle, DirectionalAnimation.DirectionType.Single, DirectionalAnimation.FlipType.None);
                    BirdshotCompanion.prefab.AddAnimation("run", "NevernamedsItems/Resources/Companions/LeadOfLifeModded/birdshotcompanion_run", 7, CompanionBuilder.AnimationType.Move, DirectionalAnimation.DirectionType.Single, DirectionalAnimation.FlipType.None);
                    companionController.aiAnimator.GetDirectionalAnimation("idle").Prefix = "idle";
                    companionController.aiAnimator.GetDirectionalAnimation("move").Prefix = "run";
                    BehaviorSpeculator component = BirdshotCompanion.prefab.GetComponent<BehaviorSpeculator>();
                    component.MovementBehaviors.Add(new CustomCompanionBehaviours.LeadOfLifeCompanionApproach());
                    component.MovementBehaviors.Add(new CompanionFollowPlayerBehavior { IdleAnimations = new string[] { "idle" } });
                }
                if (BlightShellCompanion.prefab == null || !CompanionBuilder.companionDictionary.ContainsKey(BlightShellCompanion.guid))
                {
                    BlightShellCompanion.prefab = CompanionBuilder.BuildPrefab("LeadOfLife BlightShell", BlightShellCompanion.guid, "NevernamedsItems/Resources/Companions/LeadOfLifeModded/blightshellcompanion_idle_001", new IntVector2(8, 1), new IntVector2(7, 7));
                    var companionController = BlightShellCompanion.prefab.AddComponent<LeadOfLifeCompanion>();
                    companionController.aiActor.MovementSpeed = 5f;
                    companionController.fireCooldown = 3.5f;
                    companionController.attacksPerActivation = 4;
                    companionController.tiedItemID = BlightShell.BlightShellID;
                    companionController.scalesWithCurse = true;
                    companionController.bulletsHaveCurseParticles = true;
                    companionController.tintsBullets = true;
                    companionController.tintColour = ExtendedColours.cursedBulletsPurple;
                    BlightShellCompanion.prefab.AddAnimation("idle", "NevernamedsItems/Resources/Companions/LeadOfLifeModded/blightshellcompanion_idle", 7, CompanionBuilder.AnimationType.Idle, DirectionalAnimation.DirectionType.Single, DirectionalAnimation.FlipType.None);
                    BlightShellCompanion.prefab.AddAnimation("run", "NevernamedsItems/Resources/Companions/LeadOfLifeModded/blightshellcompanion_run", 7, CompanionBuilder.AnimationType.Move, DirectionalAnimation.DirectionType.Single, DirectionalAnimation.FlipType.None);
                    companionController.aiAnimator.GetDirectionalAnimation("idle").Prefix = "idle";
                    companionController.aiAnimator.GetDirectionalAnimation("move").Prefix = "run";
                    BehaviorSpeculator component = BlightShellCompanion.prefab.GetComponent<BehaviorSpeculator>();
                    component.MovementBehaviors.Add(new CustomCompanionBehaviours.LeadOfLifeCompanionApproach());
                    component.MovementBehaviors.Add(new CompanionFollowPlayerBehavior { IdleAnimations = new string[] { "idle" } });
                }
                if (BloodthirstyBulletsCompanion.prefab == null || !CompanionBuilder.companionDictionary.ContainsKey(BloodthirstyBulletsCompanion.guid))
                {
                    BloodthirstyBulletsCompanion.prefab = CompanionBuilder.BuildPrefab("LeadOfLife BloodthirstyBullets", BloodthirstyBulletsCompanion.guid, "NevernamedsItems/Resources/Companions/LeadOfLifeModded/bloodthirstybulletscompanion_idle_001", new IntVector2(5, 1), new IntVector2(7, 7));
                    var companionController = BloodthirstyBulletsCompanion.prefab.AddComponent<LeadOfLifeCompanion>();
                    companionController.aiActor.MovementSpeed = 5f;
                    companionController.tiedItemID = BloodthirstyBullets.BloodthirstyBulletsID;
                    companionController.dmgUpOnFlyingEnemies = true;
                    companionController.isBloodthirstyBullets = true;
                    BloodthirstyBulletsCompanion.prefab.AddAnimation("idle", "NevernamedsItems/Resources/Companions/LeadOfLifeModded/bloodthirstybulletscompanion_idle", 7, CompanionBuilder.AnimationType.Idle, DirectionalAnimation.DirectionType.Single, DirectionalAnimation.FlipType.None);
                    BloodthirstyBulletsCompanion.prefab.AddAnimation("run", "NevernamedsItems/Resources/Companions/LeadOfLifeModded/bloodthirstybulletscompanion_run", 7, CompanionBuilder.AnimationType.Move, DirectionalAnimation.DirectionType.Single, DirectionalAnimation.FlipType.None);
                    companionController.aiAnimator.GetDirectionalAnimation("idle").Prefix = "idle";
                    companionController.aiAnimator.GetDirectionalAnimation("move").Prefix = "run";
                    BehaviorSpeculator component = BloodthirstyBulletsCompanion.prefab.GetComponent<BehaviorSpeculator>();
                    component.MovementBehaviors.Add(new CustomCompanionBehaviours.LeadOfLifeCompanionApproach());
                    component.MovementBehaviors.Add(new CompanionFollowPlayerBehavior { IdleAnimations = new string[] { "idle" } });
                }
                #endregion
            }
            catch (Exception e)
            {
                ETGModConsole.Log(e.Message);
                ETGModConsole.Log(e.StackTrace);
            }
        }
    }

    public class LeadOfLifeCompanionStats
    {
        public string guid;
        public GameObject prefab;
    }

    public class LeadOfLifeCompanion : CompanionController
    {
        public LeadOfLifeCompanion()
        {
            //basics
            overrideShader = null;
            attackOnTimer = true;
            FiresBullets = true;
            doesBlanks = false;
            doesKatanaSlash = false;
            gunIDToCopyBulletsFrom = 86;
            isHelix = false;
            attacksPerActivation = 1;
            incrementallyDifferentShots = false;
            incrementForAltShots = 0;
            incrementShot = null;

            //stats
            fireCooldown = 1.3f;
            angleVariance = 15f;
            bulletScaleMultiplier = 1f;
            bulletSpeedMultiplier = 1;
            rangeMultiplier = 1;
            damageMultiplier = 1;
            jammedDamageMult = 1;
            bossDamageMult = 1;
            knockbackMult = 1;

            //Bullet Colour
            tintsBullets = false;
            bulletsHaveCurseParticles = false;

            //Bullet Status Effects
            inflictsPoison = false;
            inflictsCharm = false;
            inflictsSlow = false;

            //Damage Scales
            scalesWithCurse = false;
            scalesDamageWithPlayerProximity = false;
            scalesWithCurrency = false;
            isAlpha = false;
            isOmega = false;

            //Misc
            hasElectricBullets = false;
            hasAngryBullets = false;
            hasPiercingBullets = false;
            hasFlakBullets = false;
            picksRandomPlayerBullet = false;
            fireOnActiveItemUse = false;
            activeItemIDToCheckFor = -1;
            roomDMGAmount = 0;
            guidsToInstaKill = new List<string>();
        }
        private void Start()
        {
            this.Owner = this.m_owner;
            timer = 1;
            if (Owner)
            {
                foreach (PassiveItem item in Owner.passiveItems)
                {
                    if (item && item.GetComponent<LeadOfLife>())
                    {
                        if (item.GetComponent<LeadOfLife>().extantCompanions.Count > 0)
                        {
                            if (item.GetComponent<LeadOfLife>().extantCompanions.Contains(this))
                            {
                                baseItem = item.GetComponent<LeadOfLife>();
                            }
                        }
                    }
                }
                Owner.OnRoomClearEvent += this.OnRoomClearEffects;
                Owner.OnUsedPlayerItem += this.OnOwnerUsedActiveItem;
                if (!string.IsNullOrEmpty(overrideShader))
                {
                    this.sprite.renderer.material.shader = ShaderCache.Acquire(overrideShader);
                }
            }
        }
        private float timer;
        private int increment;
        public override void Update()
        {
            if (base.specRigidbody && base.aiActor && Owner && base.transform)
            {
                if (canIgniteGoop) DeadlyDeadlyGoopManager.IgniteGoopsCircle(base.specRigidbody.UnitBottomCenter, 1);
                if (Owner.IsInCombat && base.transform.position.GetAbsoluteRoom() == Owner.CurrentRoom)
                {
                    if (timer > 0)
                    {
                        timer -= BraveTime.DeltaTime;
                    }
                    if (timer <= 0)
                    {
                        if (attackOnTimer) Attack();
                        float originalFireCooldown = fireCooldown;
                        if (baseItem && baseItem.AmountOfEasyReloadCompanions > 0)
                        {
                            for (int i = 0; i < baseItem.AmountOfEasyReloadCompanions; i++)
                            {
                                originalFireCooldown *= 0.75f;
                            }
                        }
                        if (baseItem && hasPlatScaling)
                        {
                            for (int i = 0; i < baseItem.PlatBulletsLanded; i++)
                            {
                                originalFireCooldown *= 0.9996f;
                            }
                        }
                        timer = originalFireCooldown;
                    }
                }
            }
            base.Update();
        }
        private void Attack()
        {
            increment++;
            for (int i = 0; i < attacksPerActivation; i++)
            {
                //ETGModConsole.Log("Attack triggered");
                if (FiresBullets)
                {
                    if (incrementallyDifferentShots && increment == incrementForAltShots)
                    {
                        FireBullets(incrementShot);
                        increment = 0;
                    }
                    FireBullets();
                }
                if (picksRandomPlayerBullet) DoChanceBulletsSpawn();
                if (isHelix) { FireBullets(null, true, false); FireBullets(null, true, true); }
                if (doesKatanaSlash) GameManager.Instance.Dungeon.StartCoroutine(this.DoKatanaBulletsChain(base.specRigidbody.UnitCenter, (base.specRigidbody.UnitCenter.GetPositionOfNearestEnemy(false) - base.specRigidbody.UnitCenter).normalized));
                if (dropsBombs) DropBomb();
                if (doesBlanks) DoBlank();
                if (slowsTime) SlowTime();
                if (doGoop) DoGoop();
                if (roomDMGAmount > 0) DoRoomDamage();
            }


        }
        private void DoRoomDamage()
        {
            if (base.aiActor.CurrentRoom() != null)
            {
                List<AIActor> enemies = base.aiActor.CurrentRoom().GetActiveEnemies(RoomHandler.ActiveEnemyType.All);
                if (enemies != null && enemies.Count > 0)
                {
                    foreach (AIActor actor in enemies)
                    {
                        if (actor && actor.healthHaver)
                        {
                            actor.healthHaver.ApplyDamage(roomDMGAmount, Vector2.zero, "Elder Blank");
                        }
                    }
                }
            }
        }
        private void DoGoop()
        {
            if (goopDefToSpawn != null)
            {
                if (doLineGoop)
                {
                    DeadlyDeadlyGoopManager.GetGoopManagerForGoopType(goopDefToSpawn).TimedAddGoopLine(base.specRigidbody.UnitCenter, base.sprite.WorldCenter.GetPositionOfNearestEnemy(false, false), goopRadiusOrWidth, 0.5f);
                }
                else
                {
                    DeadlyDeadlyGoopManager.GetGoopManagerForGoopType(goopDefToSpawn).TimedAddGoopCircle(base.specRigidbody.UnitCenter, goopRadiusOrWidth, 0.5f, false);
                }
            }
        }
        private void FireBullets(Projectile externalOverrideProj = null, bool usesHelix = false, bool helixInverted = false)
        {
            Projectile projectile;
            if ((ETGMod.Databases.Items[gunIDToCopyBulletsFrom] as Gun).DefaultModule.shootStyle == ProjectileModule.ShootStyle.Charged)
            {
                projectile = ((Gun)ETGMod.Databases.Items[gunIDToCopyBulletsFrom]).DefaultModule.chargeProjectiles[0].Projectile;
            }
            else
            {
                projectile = ((Gun)ETGMod.Databases.Items[gunIDToCopyBulletsFrom]).DefaultModule.projectiles[0];
            }
            if (overrideBloody9mmBullet) projectile = PickupObjectDatabase.GetById(524).GetComponent<RandomProjectileReplacementItem>().ReplacementProjectile;
            if (overrideCritbullet) projectile = PickupObjectDatabase.GetById(640).GetComponent<ComplexProjectileModifier>().CriticalProjectile;
            if (usesGunOfAThousandSinsBullets) projectile = (PickupObjectDatabase.GetById(GunOfAThousandSins.GunOfAThousandSinsID) as Gun).DefaultModule.projectiles[0];

            if (externalOverrideProj != null) projectile = externalOverrideProj;

            Vector2 nearestEnemyPosition = base.sprite.WorldCenter.GetPositionOfNearestEnemy(false, true);

            GameObject gameObject = ProjSpawnHelper.SpawnProjectileTowardsPoint(projectile.gameObject, base.sprite.WorldCenter, nearestEnemyPosition, 0, angleVariance);
            Projectile component = gameObject.GetComponent<Projectile>();
            if (component != null)
            {
                component.Owner = Owner;
                component.TreatedAsNonProjectileForChallenge = true;
                component.Shooter = base.specRigidbody;
                component.collidesWithPlayer = false;

                //Stats
                component.baseData.damage *= damageMultiplier;
                component.baseData.speed *= bulletSpeedMultiplier;
                component.baseData.range *= rangeMultiplier;
                component.AdditionalScaleMultiplier *= bulletScaleMultiplier;
                component.baseData.force *= knockbackMult;
                component.BlackPhantomDamageMultiplier *= jammedDamageMult;
                component.BossDamageMultiplier *= bossDamageMult;

                //Status Effect
                if (inflictsPoison || inflictsCharm || inflictsSlow || inflictsFreeze)
                {
                    StatusEffectBulletMod status = component.gameObject.GetOrAddComponent<StatusEffectBulletMod>();
                    if (inflictsPoison)
                    {
                        status.datasToApply.Add(new StatusEffectBulletMod.StatusData() { applyChance = 1f, effectTint = ExtendedColours.poisonGreen, applyTint = true, effect = StaticStatusEffects.irradiatedLeadEffect });
                    }
                    if (inflictsCharm)
                    {
                        status.datasToApply.Add(new StatusEffectBulletMod.StatusData() { applyChance = 1f, effectTint = ExtendedColours.charmPink, applyTint = true, effect = StaticStatusEffects.charmingRoundsEffect });
                    }
                    if (inflictsSlow)
                    {
                        status.datasToApply.Add(new StatusEffectBulletMod.StatusData() { applyChance = 1f, effectTint = ExtendedColours.poisonGreen, applyTint = false, effect = StaticStatusEffects.tripleCrossbowSlowEffect });
                    }
                    if (inflictsFreeze)
                    {
                        status.datasToApply.Add(new StatusEffectBulletMod.StatusData() { applyChance = 1f, effectTint = ExtendedColours.freezeBlue, applyTint = true, effect = StaticStatusEffects.frostBulletsEffect });
                    }
                }
                if (inflictedStunDuration > 0 && stunInflictChance > 0)
                {
                    component.AppliesStun = true;
                    component.StunApplyChance = stunInflictChance;
                    component.AppliedStunDuration = inflictedStunDuration;
                }
                //Colour
                if (tintsBullets) { component.AdjustPlayerProjectileTint(tintColour, 2); }
                if (bulletsHaveCurseParticles) { component.CurseSparks = true; }

                //Other Shit
                if (hasElectricBullets) component.damageTypes |= CoreDamageTypes.Electric;
                if (hasAngryBullets) component.gameObject.AddComponent<AngryBulletsProjectileBehaviour>();
                if (hasFlakBullets) component.gameObject.AddComponent<FlakBulletsProjectileBehaviour>();
                if (hasPiercingBullets) component.gameObject.AddComponent<PierceProjModifier>();
                if (hasRemoteBullets) component.gameObject.AddComponent<RemoteBulletsProjectileBehaviour>();
                if (hasBouncyBullets) component.gameObject.AddComponent<BounceProjModifier>();
                if (hasHomingBullets) { HomingModifier homing = component.gameObject.AddComponent<HomingModifier>(); homing.HomingRadius = 100f; }
                if (hasChaosBullets) { ChaosBulletsModifierComp chaos = component.gameObject.AddComponent<ChaosBulletsModifierComp>(); chaos.chanceOfActivatingStatusEffect = 0.5f; }
                if (hasExplosiveBullets)
                {
                    ExplosiveModifier explode = component.gameObject.AddComponent<ExplosiveModifier>();
                    explode.doExplosion = true;
                    explode.explosionData = StaticExplosionDatas.explosiveRoundsExplosion;
                }
                if (hasHungryBullets)
                {
                    HungryProjectileModifier hungry = component.gameObject.AddComponent<HungryProjectileModifier>();
                    hungry.HungryRadius = 1.5f;
                }
                if (hasOrbitalBullets)
                {
                    if (base.specRigidbody)
                    {
                        OrbitalBulletsBehaviour orbital = component.gameObject.GetOrAddComponent<OrbitalBulletsBehaviour>();
                        orbital.usesOverrideCenter = true;
                        orbital.overrideCenter = base.specRigidbody;
                        orbital.orbitalGroup = 2;
                    }
                }
                if (hasShockRounds) ApplyShockRoundsEffect(component);


                //Transmogs
                if (transmogsEnemies)
                {
                    EasyTransmogrifyComponent transmog = component.gameObject.GetOrAddComponent<EasyTransmogrifyComponent>();
                    EasyTransmogrifyComponent.TransmogData data = new EasyTransmogrifyComponent.TransmogData()
                    {
                        TargetGuid = this.transmogTargetGuid,
                        identifier = "LeadOfLife Transmog",
                        TransmogChance = this.chanceToTransmog,
                    };
                    transmog.TransmogDataList.Add(data);
                }

                //Damage scalers
                if (scalesWithCurse) component.baseData.damage *= ((0.15f * Owner.stats.GetStatValue(PlayerStats.StatType.Curse)) + 1);
                if (scalesWithCurrency) component.baseData.damage *= ((0.0038f * Owner.carriedConsumables.Currency) + 1);

                if (usesHelix) component.ConvertToHelixMotion(helixInverted);

                if (scalesDamageWithPlayerProximity)
                {
                    float distance = Vector2.Distance(base.specRigidbody.UnitCenter, Owner.CenterPosition);
                    if (scaleProxDistant)
                    {
                        float multiplier = (distance * 0.025f) + 1;
                        component.baseData.damage *= multiplier;
                        component.AdditionalScaleMultiplier *= multiplier;
                    }
                    if (scaleProxClose)
                    {
                        float distReduc = ((distance * 4) * 0.01f);
                        float multiplier = 1 - distReduc;
                        if (multiplier <= 0) multiplier = 0.001f;
                        component.baseData.damage *= multiplier;
                        component.AdditionalScaleMultiplier *= multiplier;
                    }
                }
                if (isAlpha)
                {
                    if (Owner.CurrentGun && (Owner.CurrentGun.ClipShotsRemaining == Owner.CurrentGun.ClipCapacity))
                    {
                        component.baseData.damage *= 2;
                        component.AdditionalScaleMultiplier *= 1.25f;
                    }
                }
                else if (isOmega)
                {
                    if (Owner.CurrentGun && Owner.CurrentGun.ClipShotsRemaining == 1)
                    {

                        component.baseData.damage *= 2;
                        component.AdditionalScaleMultiplier *= 1.25f;
                    }
                }
                if (hasPlatScaling)
                {
                    for (int i = 0; i < baseItem.PlatBulletsLanded; i++)
                    {
                        component.baseData.damage *= 1.0002f;
                    }
                }
                if (guidsToInstaKill != null && guidsToInstaKill.Count > 0)
                {
                    InstaKillEnemyTypeBehaviour killywilly = component.gameObject.GetOrAddComponent<InstaKillEnemyTypeBehaviour>();
                    killywilly.EnemyTypeToKill.AddRange(guidsToInstaKill);
                }
                if (doesIntersectionEffect)
                {
                    if (explodeOnIntersection)
                    {
                        AntimatterBulletsModifier mod = component.gameObject.GetOrAddComponent<AntimatterBulletsModifier>();
                        mod.explosionData = AntimatterBullets.smallPlayerSafeExplosion;
                    }
                }
                if (dmgUpOnFlyingEnemies)
                {
                    SelectiveDamageMult selectiveDMGMult = component.gameObject.GetOrAddComponent<SelectiveDamageMult>();
                    if (dmgUpOnFlyingEnemies) { selectiveDMGMult.multOnFlyingEnemies = true; selectiveDMGMult.multiplier = 1.4f; }
                }
                if (isBloodthirstyBullets)
                {
                    BloodthirstyBulletsComp comp = component.gameObject.GetOrAddComponent<BloodthirstyBulletsComp>();
                }

                if(tiedItemID == BashfulShot.BashfulShotID)
                {
                    int numberOfOtherCompanions = (baseItem.extantCompanions.Count - 1);
                    float multiplier = (-0.03f * numberOfOtherCompanions) + 2;
                    multiplier = Mathf.Max(1, multiplier);
                    component.baseData.damage *= multiplier;
                }

                component.ApplyCompanionModifierToBullet(Owner);

                if (hasShadowBullets) Owner.SpawnShadowBullet(component, true);

                component.OnHitEnemy += this.OnHitEnemy;
            }
        }

        private void OnHitEnemy(Projectile bullet, SpeculativeRigidbody enemy, bool fatal)
        {
            if (hasPlatScaling) baseItem.PlatBulletsLanded += 1;
        }
        private void DoBlank()
        {
            if (blankTypeRandomised)
            {
                int thingToDo = UnityEngine.Random.Range(1, 5);
                if (thingToDo == 1) return;
                else if (thingToDo == 2 || thingToDo == 3) Owner.DoEasyBlank(base.specRigidbody.UnitCenter, EasyBlankType.MINI);
                else if (thingToDo == 4) Owner.DoEasyBlank(base.specRigidbody.UnitCenter, EasyBlankType.FULL);
            }
            else
            {
                Owner.DoEasyBlank(base.specRigidbody.UnitCenter, blankType);
            }
        }
        private void SlowTime()
        {
            var timeSlow = new RadialSlowInterface();
            timeSlow.DoesSepia = false;
            timeSlow.RadialSlowHoldTime = 5f;
            timeSlow.RadialSlowTimeModifier = 0.25f;
            timeSlow.DoRadialSlow(base.specRigidbody.UnitCenter, base.aiActor.CurrentRoom());
        }
        private void ApplyShockRoundsEffect(Projectile projectile)
        {
            projectile.sprite.renderer.enabled = false;
            NoCollideBehaviour nocollide = projectile.gameObject.GetOrAddComponent<NoCollideBehaviour>();
            nocollide.worksOnEnemies = true;
            nocollide.worksOnProjectiles = true;

            ComplexProjectileModifier shockRounds = PickupObjectDatabase.GetById(298) as ComplexProjectileModifier;
            ChainLightningModifier orAddComponent = projectile.gameObject.GetOrAddComponent<ChainLightningModifier>();
            orAddComponent.LinkVFXPrefab = shockRounds.ChainLightningVFX;
            orAddComponent.damageTypes = shockRounds.ChainLightningDamageTypes;
            orAddComponent.maximumLinkDistance = 20;
            orAddComponent.damagePerHit = 5;
            orAddComponent.damageCooldown = shockRounds.ChainLightningDamageCooldown;
            if (shockRounds.ChainLightningDispersalParticles != null)
            {
                orAddComponent.UsesDispersalParticles = true;
                orAddComponent.DispersalParticleSystemPrefab = shockRounds.ChainLightningDispersalParticles;
                orAddComponent.DispersalDensity = shockRounds.ChainLightningDispersalDensity;
                orAddComponent.DispersalMinCoherency = shockRounds.ChainLightningDispersalMinCoherence;
                orAddComponent.DispersalMaxCoherency = shockRounds.ChainLightningDispersalMaxCoherence;
            }
            else
            {
                orAddComponent.UsesDispersalParticles = false;
            }
        }
        private void DropBomb()
        {
            if (UnityEngine.Random.value <= 0.1f && base.sprite)
            {
                GameObject BombObject = PickupObjectDatabase.GetById(108).GetComponent<SpawnObjectPlayerItem>().objectToSpawn.gameObject;
                GameObject ExtantBomb = UnityEngine.Object.Instantiate<GameObject>(BombObject, base.sprite.WorldCenter, Quaternion.identity);
                tk2dBaseSprite bombsprite = ExtantBomb.GetComponent<tk2dBaseSprite>();
                if (bombsprite) bombsprite.PlaceAtPositionByAnchor(base.sprite.WorldCenter, tk2dBaseSprite.Anchor.MiddleCenter);
            }
        }
        protected override void OnDestroy()
        {
            if (Owner)
            {
                Owner.OnRoomClearEvent -= this.OnRoomClearEffects;
                Owner.OnUsedPlayerItem -= this.OnOwnerUsedActiveItem;
            }
            base.OnDestroy();
        }
        private void OnOwnerUsedActiveItem(PlayerController player, PlayerItem item)
        {
            if (fireOnActiveItemUse)
            {
                //ETGModConsole.Log("Active item ran");

                if (item.PickupObjectId == activeItemIDToCheckFor || activeItemIDToCheckFor == -1)
                {
                    Attack();
                }
            }
        }
        private Vector2 GetAngleToFire()
        {
            return (base.specRigidbody.UnitCenter.GetPositionOfNearestEnemy(false) - base.specRigidbody.UnitCenter).normalized;
        }
        private void FireBeams(Projectile beam)
        {
            BeamToolbox.FreeFireBeamFromAnywhere(beam, Owner, base.gameObject, Vector2.zero, false, GetAngleToFire().ToAngle(), 1, true);
        }
        private IEnumerator DoKatanaBulletsChain(Vector2 startPosition, Vector2 direction)
        {
            float LCEChainDuration = Gungeon.Game.Items["katana_bullets"].GetComponent<ComplexProjectileModifier>().LCEChainDuration;
            float LCEChainDistance = Gungeon.Game.Items["katana_bullets"].GetComponent<ComplexProjectileModifier>().LCEChainDistance;
            int LCEChainNumExplosions = Gungeon.Game.Items["katana_bullets"].GetComponent<ComplexProjectileModifier>().LCEChainNumExplosions;
            ExplosionData LinearChainExplosionData = Gungeon.Game.Items["katana_bullets"].GetComponent<ComplexProjectileModifier>().LinearChainExplosionData;

            float perExplosionTime = LCEChainDuration / (float)LCEChainNumExplosions;
            float[] explosionTimes = new float[LCEChainNumExplosions];
            explosionTimes[0] = 0f;
            explosionTimes[1] = perExplosionTime;
            for (int i = 2; i < LCEChainNumExplosions; i++)
            {
                explosionTimes[i] = explosionTimes[i - 1] + perExplosionTime;
            }
            Vector2 lastValidPosition = startPosition;
            bool hitWall = false;
            int index = 0;
            float elapsed = 0f;
            lastValidPosition = startPosition;
            hitWall = false;
            Vector2 currentDirection = direction;
            RoomHandler currentRoom = startPosition.GetAbsoluteRoom();
            while (elapsed < LCEChainDuration)
            {
                elapsed += BraveTime.DeltaTime;
                while (index < LCEChainNumExplosions && elapsed >= explosionTimes[index])
                {
                    Vector2 vector = startPosition + currentDirection.normalized * LCEChainDistance;
                    Vector2 vector2 = Vector2.Lerp(startPosition, vector, ((float)index + 1f) / (float)LCEChainNumExplosions);
                    if (!this.ValidPositionForKatanaSlash(vector2))
                    {
                        hitWall = true;
                    }
                    if (!hitWall)
                    {
                        lastValidPosition = vector2;
                    }
                    Exploder.Explode(lastValidPosition, LinearChainExplosionData, currentDirection, null, false, CoreDamageTypes.None, false);
                    index++;
                }
                yield return null;
            }
            yield break;
        }
        private bool ValidPositionForKatanaSlash(Vector2 pos)
        {
            IntVector2 intVector = pos.ToIntVector2(VectorConversions.Floor);
            return GameManager.Instance.Dungeon.data.CheckInBoundsAndValid(intVector) && GameManager.Instance.Dungeon.data[intVector].type != CellType.WALL;
        }
        private void DoChanceBulletsSpawn()
        {
            List<Projectile> ValidBullets = new List<Projectile>();
            List<Projectile> ValidBeams = new List<Projectile>();
            if (Owner && Owner.inventory != null)
            {
                for (int j = 0; j < Owner.inventory.AllGuns.Count; j++)
                {
                    if (Owner.inventory.AllGuns[j] && !Owner.inventory.AllGuns[j].InfiniteAmmo)
                    {
                        ProjectileModule defaultModule = Owner.inventory.AllGuns[j].DefaultModule;
                        if (defaultModule.shootStyle == ProjectileModule.ShootStyle.Beam)
                        {
                            ValidBeams.Add(defaultModule.GetCurrentProjectile());
                        }
                        else if (defaultModule.shootStyle == ProjectileModule.ShootStyle.Charged)
                        {
                            Projectile projectile = null;
                            for (int k = 0; k < 15; k++)
                            {
                                ProjectileModule.ChargeProjectile chargeProjectile = defaultModule.chargeProjectiles[UnityEngine.Random.Range(0, defaultModule.chargeProjectiles.Count)];
                                if (chargeProjectile != null) projectile = chargeProjectile.Projectile;
                                if (projectile) break;
                            }
                            ValidBullets.Add(projectile);
                        }
                        else
                        {
                            ValidBullets.Add(defaultModule.GetCurrentProjectile());
                        }
                    }
                }

                int listsCombined = ValidBullets.Count + ValidBeams.Count;
                if (listsCombined > 0)
                {
                    int randomSelection = UnityEngine.Random.Range(0, listsCombined);
                    if (randomSelection > ValidBullets.Count) //Beams
                    {
                        FireBeams(BraveUtility.RandomElement(ValidBeams));
                    }
                    else //Projectiles
                    {
                        FireBullets(BraveUtility.RandomElement(ValidBullets));
                    }
                }
                else
                {
                    FireBullets();
                }
            }
        }
        private void OnRoomClearEffects(PlayerController guy)
        {
            if (base.specRigidbody)
            {
                timer = 1;
                if (spawnsCurrencyOnRoomClear)
                {
                    LootEngine.SpawnCurrency(base.specRigidbody.UnitCenter, UnityEngine.Random.Range(0, 4));
                }
            }
        }

        public PlayerController Owner;
        public int tiedItemID;
        private LeadOfLife baseItem;
        //Basic Stuff
        public string overrideShader;
        public bool attackOnTimer;
        public bool FiresBullets;
        public bool doesKatanaSlash;
        public bool doesBlanks;
        public int gunIDToCopyBulletsFrom;
        public bool isHelix;
        public int attacksPerActivation;
        public bool incrementallyDifferentShots;
        public int incrementForAltShots;
        public Projectile incrementShot;
        //Stats
        public float damageMultiplier;
        public float bulletSpeedMultiplier;
        public float bulletScaleMultiplier;
        public float rangeMultiplier;
        public float fireCooldown;
        public float angleVariance;
        public float jammedDamageMult;
        public float bossDamageMult;
        public float knockbackMult;
        //Bullet Colour
        public bool tintsBullets;
        public Color tintColour;
        public bool bulletsHaveCurseParticles;
        //Bullet Status Effect
        public bool inflictsPoison;
        public bool inflictsCharm;
        public bool inflictsSlow;
        public bool inflictsFreeze;
        public float inflictedStunDuration;
        public float stunInflictChance;
        //Damage Scales
        public bool scalesWithCurse;
        public bool scalesWithCurrency;
        public bool isAlpha;
        public bool isOmega;
        //Other Variables
        public bool overrideBloody9mmBullet;
        public bool overrideCritbullet;
        public bool picksRandomPlayerBullet;
        public bool hasElectricBullets;
        public bool hasAngryBullets;
        public bool hasFlakBullets;
        public bool hasPiercingBullets;
        public bool hasRemoteBullets;
        public bool hasBouncyBullets;
        public bool hasHomingBullets;
        public bool canIgniteGoop;
        public bool hasShadowBullets;
        public bool hasChaosBullets;
        public bool hasExplosiveBullets;
        public bool dropsBombs;
        public bool hasHungryBullets;
        public bool hasOrbitalBullets;
        public bool hasShockRounds;
        public bool hasPlatScaling;
        public bool slowsTime;
        public bool blankTypeRandomised;
        public EasyBlankType blankType;
        public List<string> guidsToInstaKill;
        public bool isBloodthirstyBullets;
        //Transmog
        public bool transmogsEnemies;
        public float chanceToTransmog;
        public string transmogTargetGuid;
        public string transmogSoundEffect;
        //Room Clear Stuff
        public bool spawnsCurrencyOnRoomClear;
        //Proximity Scaling
        public bool scalesDamageWithPlayerProximity;
        public bool scaleProxClose;
        public bool scaleProxDistant;
        //OnUsedActive stuff
        public bool fireOnActiveItemUse;
        public int activeItemIDToCheckFor;
        //DoGoop
        public bool doGoop;
        public GoopDefinition goopDefToSpawn;
        public bool doLineGoop;
        public bool lineGoopLength;
        public float goopRadiusOrWidth;
        //RoomDMG
        public float roomDMGAmount;
        //Intersection Effect
        public bool doesIntersectionEffect;
        public bool explodeOnIntersection;
        //SelectiveDamageUp
        public bool dmgUpOnFlyingEnemies;

        //Modded Projectiles
        public bool usesGunOfAThousandSinsBullets;
    }
}
