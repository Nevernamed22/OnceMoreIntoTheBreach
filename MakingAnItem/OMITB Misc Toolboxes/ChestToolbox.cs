using MonoMod.RuntimeDetour;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using UnityEngine;

namespace NevernamedsItems
{
    static class ChestToolbox
    {
        public static void Inithooks()
        {
            chestPostProcessHook = new Hook(
                typeof(Chest).GetMethod("PossiblyCreateBowler", BindingFlags.Instance | BindingFlags.NonPublic),
                typeof(ChestToolbox).GetMethod("PostProcessChest", BindingFlags.Static | BindingFlags.NonPublic)
            );
            chestPreOpenHook = new Hook(
                typeof(Chest).GetMethod("Open", BindingFlags.Instance | BindingFlags.NonPublic),
                typeof(ChestToolbox).GetMethod("ChestPreOpen", BindingFlags.Static | BindingFlags.NonPublic)
            );
            chestBrokenHook = new Hook(
                typeof(Chest).GetMethod("OnBroken", BindingFlags.Instance | BindingFlags.NonPublic),
                typeof(ChestToolbox).GetMethod("OnBroken", BindingFlags.Static | BindingFlags.NonPublic)
            );
        }
        private static Hook chestPostProcessHook;
        private static Hook chestPreOpenHook;
        private static Hook chestBrokenHook;
        public static Action<Chest> OnChestPostSpawn;
        public static Action<Chest, PlayerController> OnChestPreOpen;
        public static Action<Chest> OnChestBroken;

        public static void SetChestCachedSpriteID(this Chest self, int desiredVal)
        {
            OMITBReflectionHelpers.ReflectSetField<int>(typeof(Chest), "m_cachedSpriteForCoop", desiredVal, self);
        }
        private static int GetChestCachedSpriteID(this Chest self)
        {
            int idleID = OMITBReflectionHelpers.ReflectGetField<int>(typeof(Chest), "m_cachedSpriteForCoop", self);
            return idleID;
        }
        private static void PostProcessChest(Action<Chest, bool> orig, Chest self, bool uselssVar)
        {
            Debug.Log("PostProcessChest ran!");
            if (OnChestPostSpawn != null)
            {
                OnChestPostSpawn(self);
            }

            if (self.GetChestCachedSpriteID() == -1)
            {
                self.SetChestCachedSpriteID(self.sprite.spriteId);
            }

            orig(self, uselssVar);
        }
        public static ChestTier GetChestTier(this Chest chest)
        {
            if (chest.IsGlitched) return ChestTier.GLITCHED;
            //ETGModConsole.Log(chest.breakAnimName);
            if (chest.breakAnimName.Contains("wood_"))
            {
                if (chest.IsRainbowChest) return ChestTier.SECRETRAINBOW;
                else return ChestTier.BROWN;
            }
            if (chest.breakAnimName.Contains("silver_")) return ChestTier.BLUE;
            else if (chest.breakAnimName.Contains("green_")) return ChestTier.GREEN;
            else if (chest.breakAnimName.Contains("redgold_"))
            {
                if (chest.IsRainbowChest) return ChestTier.RAINBOW;
                else return ChestTier.RED;
            }
            else if (chest.breakAnimName.Contains("blackbone_")) return ChestTier.BLACK;
            else if (chest.breakAnimName.Contains("synergy_")) return ChestTier.SYNERGY;
            else if (chest.breakAnimName.Contains("truth_")) return ChestTier.TRUTH;
            else if (chest.breakAnimName.Contains("rat_")) return ChestTier.RAT;
            return ChestTier.OTHER;
        }
        public static void AddFuse(this Chest chest)
        {
            if (chest.GetFuse() == null)
            {
                var chestType = typeof(Chest);
                var func = chestType.GetMethod("TriggerCountdownTimer", BindingFlags.Instance | BindingFlags.NonPublic);
                var ret = func.Invoke(chest, null);
                AkSoundEngine.PostEvent("Play_OBJ_fuse_loop_01", chest.gameObject);
            }
        }
        public static ChestFuseController GetFuse(this Chest chest)
        {
            ChestFuseController fuse = OMITBReflectionHelpers.ReflectGetField<ChestFuseController>(typeof(Chest), "extantFuse", chest);
            return fuse;
        }
        public static Chest SpawnChestEasy(IntVector2 location, ChestTier tier, bool locked, Chest.GeneralChestType type = Chest.GeneralChestType.UNSPECIFIED, ThreeStateValue mimic = ThreeStateValue.UNSPECIFIED, ThreeStateValue fused = ThreeStateValue.UNSPECIFIED)
        {
            GameObject chestPrefab = null;
            switch (tier)
            {
                case ChestTier.BLACK:
                    chestPrefab = GameManager.Instance.RewardManager.S_Chest.gameObject;
                    break;
                case ChestTier.BLUE:
                    chestPrefab = GameManager.Instance.RewardManager.C_Chest.gameObject;
                    break;
                case ChestTier.BROWN:
                    chestPrefab = GameManager.Instance.RewardManager.D_Chest.gameObject;
                    break;
                case ChestTier.GREEN:
                    chestPrefab = GameManager.Instance.RewardManager.B_Chest.gameObject;
                    break;
                case ChestTier.RED:
                    chestPrefab = GameManager.Instance.RewardManager.A_Chest.gameObject;
                    break;
                case ChestTier.SYNERGY:
                    chestPrefab = GameManager.Instance.RewardManager.Synergy_Chest.gameObject;
                    break;
                case ChestTier.RAINBOW:
                    chestPrefab = GameManager.Instance.RewardManager.Rainbow_Chest.gameObject;
                    break;
                case ChestTier.SECRETRAINBOW:
                    chestPrefab = GameManager.Instance.RewardManager.D_Chest.gameObject;
                    break;
                case ChestTier.GLITCHED:
                    chestPrefab = GameManager.Instance.RewardManager.B_Chest.gameObject;
                    break;
                case ChestTier.RAT:
                    chestPrefab = LoadHelper.LoadAssetFromAnywhere<GameObject>("chest_rat");
                    break;
                case ChestTier.TRUTH:
                    Debug.LogError("ERROR: Chest Toolbox cannot spawn Truth Chest.");
                    break;
                case ChestTier.OTHER:
                    Debug.LogError("ERROR: Chest Toolbox cannot spawn 'Other' Chest.");
                    break;
            }
            if (chestPrefab != null)
            {
                Chest spawnedChest = Chest.Spawn(chestPrefab.GetComponent<Chest>(), location);
                if (locked) spawnedChest.IsLocked = true;
                else spawnedChest.IsLocked = false;
                if (tier == ChestTier.GLITCHED)
                {
                    spawnedChest.BecomeGlitchChest();
                }
                if (tier == ChestTier.SECRETRAINBOW)
                {
                    spawnedChest.IsRainbowChest = true;
                    spawnedChest.ChestIdentifier = Chest.SpecialChestIdentifier.SECRET_RAINBOW;
                }
                if (type == Chest.GeneralChestType.ITEM)
                {
                    spawnedChest.lootTable.lootTable = GameManager.Instance.RewardManager.ItemsLootTable;
                }
                else if (type == Chest.GeneralChestType.WEAPON)
                {
                    spawnedChest.lootTable.lootTable = GameManager.Instance.RewardManager.GunsLootTable;
                }
                else if (type == Chest.GeneralChestType.UNSPECIFIED)
                {
                    bool IsAGun = UnityEngine.Random.value <= 0.5f;
                    spawnedChest.lootTable.lootTable = (IsAGun ? GameManager.Instance.RewardManager.GunsLootTable : GameManager.Instance.RewardManager.ItemsLootTable);
                }

                if (mimic == ThreeStateValue.FORCEYES) spawnedChest.overrideMimicChance = 100;
                if (mimic == ThreeStateValue.FORCENO) spawnedChest.overrideMimicChance = 0;
                spawnedChest.MaybeBecomeMimic();

                if (fused == ThreeStateValue.FORCEYES)
                {
                    spawnedChest.AddFuse();
                }
                if (fused == ThreeStateValue.FORCENO) spawnedChest.PreventFuse = true;

                spawnedChest.RegisterChestOnMinimap(spawnedChest.GetAbsoluteParentRoom());

                return spawnedChest;
            }
            else return null;
        }
        public enum ThreeStateValue
        {
            FORCEYES,
            FORCENO,
            UNSPECIFIED
        }
        public enum ChestTier
        {
            BROWN,
            BLUE,
            GREEN,
            RED,
            BLACK,
            RAINBOW,
            SECRETRAINBOW,
            RAT,
            SYNERGY,
            TRUTH,
            GLITCHED,
            OTHER
        }
        private static void ChestPreOpen(Action<Chest, PlayerController> orig, Chest self, PlayerController opener)
        {
            if (OnChestPreOpen != null)
            {
                OnChestPreOpen(self, opener);
            }
            orig(self, opener);
        }
        private static void OnBroken(Action<Chest> orig, Chest self)
        {
            if (OnChestBroken != null)
            {
                OnChestBroken(self);
            }
            orig(self);
        }
    }
}
