using MonoMod.RuntimeDetour;
using SaveAPI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NevernamedsItems
{
    internal class EnemyHooks
    {
        public static void InitEnemyHooks()
        {
            HHDieHook = new Hook(
                typeof(HealthHaver).GetMethod(nameof(HealthHaver.Die)),
                typeof(EnemyHooks).GetMethod(nameof(EnemyHooks.OnHealthHaverDie))
            );
            ETGMod.AIActor.OnPostStart += onEnemyPostSpawn;
        }

        public static void onEnemyPostSpawn(AIActor enemy)
        {
            if (enemy && !string.IsNullOrEmpty(enemy.EnemyGuid) && enemy.healthHaver != null)
            {
                if (enemy.EnemyGuid == EnemyGuidDatabase.Entries["ammoconda_ball"])
                {
                    if (enemy.healthHaver.GetMaxHealth() > (15 * AIActor.BaseLevelHealthModifier))
                    {
                        float newHP = 15 * AIActor.BaseLevelHealthModifier;
                        enemy.healthHaver.ForceSetCurrentHealth(newHP);
                        enemy.healthHaver.SetHealthMaximum(newHP);
                    }
                }
                if (enemy.EnemyGuid == EnemyGuidDatabase.Entries["black_skusket"])
                {
                    if (enemy.healthHaver.GetMaxHealth() > (10 * AIActor.BaseLevelHealthModifier))
                    {
                        float newHP = 10 * AIActor.BaseLevelHealthModifier;
                        enemy.healthHaver.ForceSetCurrentHealth(newHP);
                        enemy.healthHaver.SetHealthMaximum(newHP);
                    }
                }
            }
            /*
            if (enemy && enemy.healthHaver)
            {
                if (enemy.GetComponent<SteedBlobController>() == null)
                {
                    var steedPrefab = EnemyDatabase.GetOrLoadByGuid(EnemyGuidDatabase.Entries["blobulon"]);
                    if (enemy.healthHaver.GetMaxHealth() <= 12) steedPrefab = EnemyDatabase.GetOrLoadByGuid(EnemyGuidDatabase.Entries["blobuloid"]);
                    AIActor steed = AIActor.Spawn(steedPrefab.aiActor, enemy.Position, GameManager.Instance.Dungeon.data.GetAbsoluteRoomFromPosition(((Vector2)enemy.Position).ToIntVector2()), true, AIActor.AwakenAnimationType.Default, true);

                    PhysicsEngine.Instance.RegisterOverlappingGhostCollisionExceptions(steed.specRigidbody, null, false);
                    steed.gameObject.AddComponent<KillOnRoomClear>();
                    SteedBlobController steedController = steed.gameObject.AddComponent<SteedBlobController>();
                    if (steed.gameObject.GetComponent<SpawnEnemyOnDeath>()) UnityEngine.Object.Destroy(steed.gameObject.GetComponent<SpawnEnemyOnDeath>());

                    if (enemy.IsFlying) steed.SetIsFlying(true, "flyingSteed", true, true);

                    if (enemy.GetComponent<KillOnRoomClear>()) steed.gameObject.GetOrAddComponent<KillOnRoomClear>();
                    if (enemy.IgnoreForRoomClear) steed.IgnoreForRoomClear = true;

                    steed.healthHaver.SetHealthMaximum((steed.healthHaver.GetMaxHealth() * 0.8f), null, true);
                    steed.MovementSpeed *= 1.2f;

                    steedController.riderSpeedToRestore = enemy.MovementSpeed;
                    steedController.riderKnockbackToRestore = enemy.knockbackDoer.knockbackMultiplier;
                    enemy.MovementSpeed = 0;
                    enemy.knockbackDoer.knockbackMultiplier = 0;

                    steedController.rider = enemy;
                }
            }
            */
        }

        private class SteedBlobController : MonoBehaviour
        {
            public SteedBlobController()
            {
                rider = null;
                riderSpeedToRestore = 1;
            }

            private void Start()
            {
                steed = base.GetComponent<AIActor>();
                if (steed && steed.healthHaver)
                {
                    steed.healthHaver.OnDeath += this.OnDie;
                    if (rider && rider.specRigidbody)
                    {
                        Vector2 riderHalfWidth = rider.specRigidbody.UnitBottomCenter - rider.specRigidbody.UnitBottomLeft;
                        Vector2 riderPosition = steed.sprite.WorldTopCenter + new Vector2((riderHalfWidth.x * -1), 16f);
                        holdPosition = new GameObject("holdPosition");
                        holdPosition.transform.parent = steed.transform;
                        holdPosition.transform.position = riderPosition;
                        rider.specRigidbody.Position = new Position(holdPosition.transform.position);
                        rider.transform.parent = holdPosition.transform;

                        rider.specRigidbody.Reinitialize();

                        rider.specRigidbody.OnPreMovement += this.OnPreRiderMove;
                    }
                }
            }

            private void OnPreRiderMove(SpeculativeRigidbody riderBody)
            {
                riderBody.Velocity *= 0;
            }

            private void Update()
            {
                riderTeleportBehavs = rider.behaviorSpeculator.FindAttackBehaviors<TeleportBehavior>();
                if (steed && steed.healthHaver && steed.healthHaver.IsAlive)
                {
                    if (riderTeleportBehavs.Count > 0)
                    {
                        for (int i = 0; i < riderTeleportBehavs.Count; i++)
                        {
                            if (riderTeleportBehavs[i] != null)
                            {
                                riderTeleportBehavs[i].InitialCooldown = 100;
                            }
                        }
                    }
                    rider.specRigidbody.Position = new Position(holdPosition.transform.position);
                    rider.specRigidbody.Reinitialize();
                }
                //rider.ClearPath();
            }

            private void OnDie(Vector2 Direction)
            {
                if (rider != null && rider.healthHaver)
                {
                    rider.specRigidbody.OnPreMovement -= OnPreRiderMove;
                    rider.transform.parent = null;
                    rider.MovementSpeed = riderSpeedToRestore;
                    rider.knockbackDoer.knockbackMultiplier = riderKnockbackToRestore;
                    rider.specRigidbody.Reinitialize();

                    if (riderTeleportBehavs.Count > 0)
                    {
                        for (int i = 0; i < riderTeleportBehavs.Count; i++)
                        {
                            if (riderTeleportBehavs[i] != null)
                            {
                                riderTeleportBehavs[i].InitialCooldown = 1;
                            }
                        }
                    }
                }
            }

            private AIActor steed;
            public AIActor rider;
            private GameObject holdPosition;
            private List<TeleportBehavior> riderTeleportBehavs;
            public float riderSpeedToRestore;
            public float riderKnockbackToRestore;
        }

        public static Hook HHDieHook;

        public static void OnHealthHaverDie(Action<HealthHaver, Vector2> orig, HealthHaver self, Vector2 finalDamageDir)
        {
            if (self.aiActor && self.specRigidbody)
            {
                string enemyGuid = self.aiActor.EnemyGuid;
                bool isJammed = self.aiActor.IsBlackPhantom;
                bool isCharmed = self.aiActor.CanTargetEnemies && !self.aiActor.CanTargetPlayers;
                bool isFloorBossOutsideTutorial = self.IsBoss && !self.IsSubboss && !GameManager.Instance.InTutorial;
                bool isValidForMutagen = false;
                bool usingDog = false;
                bool usingShade = false;
                Vector2 deathPosition = self.specRigidbody.UnitCenter;
                float deceasedEnemyMaxHealth = self.healthHaver.GetMaxHealth();

                List<PlayableCharacters> activeCharacters = new List<PlayableCharacters>();
                foreach (var item in GameManager.Instance.AllPlayers)
                {
                    if (!item)
                    {
                        continue;
                    }
                    isValidForMutagen |= item.healthHaver && ((item.healthHaver.GetCurrentHealth() <= 0f && item.healthHaver.Armor == 1) || (item.healthHaver.GetCurrentHealth() <= 0.5f && item.healthHaver.Armor == 0));
                    activeCharacters.Add(item.characterIdentity);
                    usingDog |= item.HasPickupID(300); // dog
                    usingShade |= item.ModdedCharacterIdentity() == ModdedCharacterID.Shade; // this also asks player2 but that doesn't hurt

                }
                ETGMod.StartGlobalCoroutine(SaveDeathsForUnlocks(enemyGuid, isJammed, isCharmed, isFloorBossOutsideTutorial, isValidForMutagen, usingDog, usingShade, activeCharacters));
                if (!isCharmed) ETGMod.StartGlobalCoroutine(HandleCurseDeathEffects(deathPosition, enemyGuid, deceasedEnemyMaxHealth, isJammed));
            }

            orig(self, finalDamageDir);
        }
        public static IEnumerator HandleCurseDeathEffects(Vector2 position, string guid, float maxHP, bool isJammed)
        {
            if (CurseManager.CurseIsActive("Curse of Infestation"))
            {
                if (!string.IsNullOrEmpty(guid) && !EasyEnemyTypeLists.SmallBullats.Contains(guid))
                {
                    int amt = UnityEngine.Random.Range(-1, 4);
                    if (amt > 0)
                    {
                        for (int i = 0; i < amt; i++)
                        {
                            if (GameManager.Instance.AnyPlayerHasActiveSynergy("The Last Crusade"))
                            {
                                AIActor targetActor = CompanionisedEnemyUtility.SpawnCompanionisedEnemy(GameManager.Instance.PrimaryPlayer, BraveUtility.RandomElement(EasyEnemyTypeLists.SmallBullats), position.ToIntVector2(), false, Color.red, 5, 2, false, false);
                                PhysicsEngine.Instance.RegisterOverlappingGhostCollisionExceptions(targetActor.specRigidbody, null, false);

                            }
                            else
                            {
                                var enemyToSpawn = EnemyDatabase.GetOrLoadByGuid(BraveUtility.RandomElement(EasyEnemyTypeLists.SmallBullats));
                                AIActor TargetActor = AIActor.Spawn(enemyToSpawn, position, GameManager.Instance.Dungeon.data.GetAbsoluteRoomFromPosition(position.ToIntVector2()), true, AIActor.AwakenAnimationType.Default, true);
                                PhysicsEngine.Instance.RegisterOverlappingGhostCollisionExceptions(TargetActor.specRigidbody, null, false);
                                TargetActor.PreventBlackPhantom = true;
                            }
                        }
                    }
                }
            }
            if (CurseManager.CurseIsActive("Curse of Sludge"))
            {
                if (maxHP > 0)
                {
                    DeadlyDeadlyGoopManager goop = null;
                    if (GameManager.Instance.AnyPlayerHasActiveSynergy("The Last Crusade")) goop = DeadlyDeadlyGoopManager.GetGoopManagerForGoopType(EasyGoopDefinitions.PlayerFriendlyPoisonGoop);
                    else goop = DeadlyDeadlyGoopManager.GetGoopManagerForGoopType(EasyGoopDefinitions.EnemyFriendlyPoisonGoop);
                    float hpMod = maxHP;
                    if (isJammed) hpMod /= 3.5f;
                    hpMod /= AIActor.BaseLevelHealthModifier; 
                    float radius = Math.Min((hpMod / 7.5f), 10);
                    goop.TimedAddGoopCircle(position, radius, 0.75f, true);
                }
            }
            if (CurseManager.CurseIsActive("Curse of The Hive"))
            {
                if (maxHP > 0)
                {
                    DeadlyDeadlyGoopManager goop = null;
                    if (GameManager.Instance.AnyPlayerHasActiveSynergy("The Last Crusade")) goop = DeadlyDeadlyGoopManager.GetGoopManagerForGoopType(EasyGoopDefinitions.PlayerFriendlyHoneyGoop);
                    else goop = DeadlyDeadlyGoopManager.GetGoopManagerForGoopType(EasyGoopDefinitions.HoneyGoop);
                    float hpMod = maxHP;
                    if (isJammed) hpMod /= 3.5f;
                    hpMod /= AIActor.BaseLevelHealthModifier; 
                    float radius = Math.Min((hpMod / 5), 10);
                    goop.TimedAddGoopCircle(position, radius, 0.75f, true);
                }
            }
            yield break;
        }
        public static IEnumerator SaveDeathsForUnlocks(string enemyGuid, bool isJammed, bool isCharmed, bool isFloorBossOutsideTutorial, bool isValidForMutagen, bool usingDog, bool usingShade, List<PlayableCharacters> activeCharacters)
        {
            if (isCharmed)
            {
                //Count Charmed Enemy Kills
                SaveAPIManager.RegisterStatChange(CustomTrackedStats.CHARMED_ENEMIES_KILLED, 1);
            }
            if (enemyGuid == EnemyGuidDatabase.Entries["key_bullet_kin"])
            {
                if (isJammed && !SaveAPIManager.GetFlag(CustomDungeonFlags.KILLEDJAMMEDKEYBULLETKIN))
                {
                    //Check for Jammed Keybullet kill
                    SaveAPIManager.SetFlag(CustomDungeonFlags.KILLEDJAMMEDKEYBULLETKIN, true);
                }
            }
            if (enemyGuid == EnemyGuidDatabase.Entries["chance_bullet_kin"])
            {
                if (isJammed && !SaveAPIManager.GetFlag(CustomDungeonFlags.KILLEDJAMMEDCHANCEKIN))
                {
                    //Check for Jammed Chance Kin Kill
                    SaveAPIManager.SetFlag(CustomDungeonFlags.KILLEDJAMMEDCHANCEKIN, true);
                }
            }
            if (EasyEnemyTypeLists.TitanBulletKin.Contains(enemyGuid)) //Count Titan kills
            {
                //Count Titan Kills
                SaveAPIManager.RegisterStatChange(CustomTrackedStats.TITAN_KIN_KILLED, 1);
            }
            if (EasyEnemyTypeLists.ModInclusiveMimics.Contains(enemyGuid))
            {
                if (isJammed && !SaveAPIManager.GetFlag(CustomDungeonFlags.KILLEDJAMMEDMIMIC))
                {
                    //Check for Jammed Mimic Kill
                    SaveAPIManager.SetFlag(CustomDungeonFlags.KILLEDJAMMEDMIMIC, true);
                }
            }

            //BOSS STUFF
            if (isFloorBossOutsideTutorial)
            {
                // TODO I pulled advanced dragun in here and pulled beholster and mine flayer up from the very bottom because it made sense in my opinion, you cna revert it if you want to
                if (enemyGuid == EnemyGuidDatabase.Entries["dragun_advanced"])
                {
                    //Trigger advanced dragun rewards
                    AdvancedDragunRewards.handleDragunRewards();
                }
                //BOSS KILL COUNTS
                if (enemyGuid == EnemyGuidDatabase.Entries["beholster"])
                {
                    SaveAPIManager.RegisterStatChange(CustomTrackedStats.BEHOLSTER_KILLS, 1);
                }
                else if (enemyGuid == EnemyGuidDatabase.Entries["mine_flayer"])
                {
                    SaveAPIManager.RegisterStatChange(CustomTrackedStats.MINEFLAYER_KILLS, 1);
                }

                //Unlock for the Mutagen, checking if the player is under a certain hp

                #region MutagenUnlock
                if (isValidForMutagen)
                {
                    if (!SaveAPIManager.GetFlag(CustomDungeonFlags.HAS_BEATEN_BOSS_BY_SKIN_OF_TEETH))
                    {
                        SaveAPIManager.SetFlag(CustomDungeonFlags.HAS_BEATEN_BOSS_BY_SKIN_OF_TEETH, true);
                    }
                }
                #endregion MutagenUnlock

                //MODE RELATED UNLOCKS

                //Turbo Unlocks
                if (GameStatsManager.Instance.isTurboMode)
                {
                    if (!SaveAPIManager.GetFlag(CustomDungeonFlags.BEATEN_ANY_BOSS_TURBO_MODE))
                    {
                        SaveAPIManager.SetFlag(CustomDungeonFlags.BEATEN_ANY_BOSS_TURBO_MODE, true);
                    }
                    if (GameManager.Instance.Dungeon.tileIndices.tilesetId == GlobalDungeonData.ValidTilesets.MINEGEON)
                    {
                        if (!SaveAPIManager.GetFlag(CustomDungeonFlags.BEATEN_MINES_BOSS_TURBO_MODE))
                        {
                            SaveAPIManager.SetFlag(CustomDungeonFlags.BEATEN_MINES_BOSS_TURBO_MODE, true);
                        }
                    }
                    if (GameManager.Instance.Dungeon.tileIndices.tilesetId == GlobalDungeonData.ValidTilesets.CATACOMBGEON)
                    {
                        if (!SaveAPIManager.GetFlag(CustomDungeonFlags.BEATEN_HOLLOW_BOSS_TURBO_MODE))
                        {
                            SaveAPIManager.SetFlag(CustomDungeonFlags.BEATEN_HOLLOW_BOSS_TURBO_MODE, true);
                        }
                    }
                }

                //Rainbow Unlocks
                if (GameStatsManager.Instance.IsRainbowRun)
                {
                    if (enemyGuid == EnemyGuidDatabase.Entries["infinilich"])
                    {
                        if (!SaveAPIManager.GetFlag(CustomDungeonFlags.RAINBOW_KILLED_LICH))
                        {
                            SaveAPIManager.SetFlag(CustomDungeonFlags.RAINBOW_KILLED_LICH, true);
                        }
                    }
                }

                //Bossrush Unlocks
                if (GameManager.Instance.CurrentGameMode == GameManager.GameMode.BOSSRUSH)
                {
                    if (GameManager.Instance.Dungeon.tileIndices.tilesetId == GlobalDungeonData.ValidTilesets.FORGEGEON)
                    {
                        if (usingShade)
                        {
                            if (!SaveAPIManager.GetFlag(CustomDungeonFlags.BOSSRUSH_SHADE))
                            {
                                SaveAPIManager.SetFlag(CustomDungeonFlags.BOSSRUSH_SHADE, true);
                            }
                        }
                        if (activeCharacters.Contains(PlayableCharacters.Eevee))
                        {
                            if (!SaveAPIManager.GetFlag(CustomDungeonFlags.BOSSRUSH_PARADOX))
                            {
                                SaveAPIManager.SetFlag(CustomDungeonFlags.BOSSRUSH_PARADOX, true);
                            }
                        }
                        if (activeCharacters.Contains(PlayableCharacters.Convict))
                        {
                            if (!SaveAPIManager.GetFlag(CustomDungeonFlags.BOSSRUSH_CONVICT))
                            {
                                SaveAPIManager.SetFlag(CustomDungeonFlags.BOSSRUSH_CONVICT, true);
                            }
                        }
                        if (activeCharacters.Contains(PlayableCharacters.Pilot))
                        {
                            if (!SaveAPIManager.GetFlag(CustomDungeonFlags.BOSSRUSH_PILOT))
                            {
                                SaveAPIManager.SetFlag(CustomDungeonFlags.BOSSRUSH_PILOT, true);
                            }
                        }
                        if (activeCharacters.Contains(PlayableCharacters.Gunslinger))
                        {
                            if (!SaveAPIManager.GetFlag(CustomDungeonFlags.BOSSRUSH_GUNSLINGER))
                            {
                                SaveAPIManager.SetFlag(CustomDungeonFlags.BOSSRUSH_GUNSLINGER, true);
                            }
                        }
                        if (activeCharacters.Contains(PlayableCharacters.Guide))
                        {
                            if (!SaveAPIManager.GetFlag(CustomDungeonFlags.BOSSRUSH_HUNTER))
                            {
                                SaveAPIManager.SetFlag(CustomDungeonFlags.BOSSRUSH_HUNTER, true);
                            }
                        }
                        if (activeCharacters.Contains(PlayableCharacters.Soldier))
                        {
                            if (!SaveAPIManager.GetFlag(CustomDungeonFlags.BOSSRUSH_MARINE))
                            {
                                SaveAPIManager.SetFlag(CustomDungeonFlags.BOSSRUSH_MARINE, true);
                            }
                        }
                        if (activeCharacters.Contains(PlayableCharacters.Robot))
                        {
                            if (!SaveAPIManager.GetFlag(CustomDungeonFlags.BOSSRUSH_ROBOT))
                            {
                                SaveAPIManager.SetFlag(CustomDungeonFlags.BOSSRUSH_ROBOT, true);
                            }
                        }
                        if (activeCharacters.Contains(PlayableCharacters.Bullet))
                        {
                            if (!SaveAPIManager.GetFlag(CustomDungeonFlags.BOSSRUSH_BULLET))
                            {
                                SaveAPIManager.SetFlag(CustomDungeonFlags.BOSSRUSH_BULLET, true);
                            }
                        }
                    }
                }

                //AllJammed
                if (GameManager.Instance.Dungeon.tileIndices.tilesetId == GlobalDungeonData.ValidTilesets.CASTLEGEON)
                {
                    if (GameManager.Instance.CurrentGameMode != GameManager.GameMode.BOSSRUSH && SaveAPIManager.GetFlag(CustomDungeonFlags.ALLJAMMEDMODE_ENABLED_GENUINE))
                    {
                        if (!SaveAPIManager.GetFlag(CustomDungeonFlags.ALLJAMMED_BEATEN_KEEP))
                        {
                            SaveAPIManager.SetFlag(CustomDungeonFlags.ALLJAMMED_BEATEN_KEEP, true);
                        }
                    }
                }
                if (GameManager.Instance.Dungeon.tileIndices.tilesetId == GlobalDungeonData.ValidTilesets.SEWERGEON)
                {
                    if (GameManager.Instance.CurrentGameMode != GameManager.GameMode.BOSSRUSH && SaveAPIManager.GetFlag(CustomDungeonFlags.ALLJAMMEDMODE_ENABLED_GENUINE))
                    {
                        if (!SaveAPIManager.GetFlag(CustomDungeonFlags.ALLJAMMED_BEATEN_OUB))
                        {
                            SaveAPIManager.SetFlag(CustomDungeonFlags.ALLJAMMED_BEATEN_OUB, true);
                        }
                    }
                }
                if (GameManager.Instance.Dungeon.tileIndices.tilesetId == GlobalDungeonData.ValidTilesets.GUNGEON)
                {
                    if (GameManager.Instance.CurrentGameMode != GameManager.GameMode.BOSSRUSH && SaveAPIManager.GetFlag(CustomDungeonFlags.ALLJAMMEDMODE_ENABLED_GENUINE))
                    {
                        if (!SaveAPIManager.GetFlag(CustomDungeonFlags.ALLJAMMED_BEATEN_PROPER))
                        {
                            SaveAPIManager.SetFlag(CustomDungeonFlags.ALLJAMMED_BEATEN_PROPER, true);
                        }
                    }
                }
                if (GameManager.Instance.Dungeon.tileIndices.tilesetId == GlobalDungeonData.ValidTilesets.CATHEDRALGEON)
                {
                    if (GameManager.Instance.CurrentGameMode != GameManager.GameMode.BOSSRUSH && SaveAPIManager.GetFlag(CustomDungeonFlags.ALLJAMMEDMODE_ENABLED_GENUINE))
                    {
                        if (!SaveAPIManager.GetFlag(CustomDungeonFlags.ALLJAMMED_BEATEN_ABBEY))
                        {
                            SaveAPIManager.SetFlag(CustomDungeonFlags.ALLJAMMED_BEATEN_ABBEY, true);
                        }
                    }
                }
                if (GameManager.Instance.Dungeon.tileIndices.tilesetId == GlobalDungeonData.ValidTilesets.MINEGEON)
                {
                    if (GameManager.Instance.CurrentGameMode != GameManager.GameMode.BOSSRUSH && SaveAPIManager.GetFlag(CustomDungeonFlags.ALLJAMMEDMODE_ENABLED_GENUINE))
                    {
                        if (!SaveAPIManager.GetFlag(CustomDungeonFlags.ALLJAMMED_BEATEN_MINES))
                        {
                            SaveAPIManager.SetFlag(CustomDungeonFlags.ALLJAMMED_BEATEN_MINES, true);
                        }
                    }
                }
                if (GameManager.Instance.Dungeon.tileIndices.tilesetId == GlobalDungeonData.ValidTilesets.RATGEON)
                {
                    if (GameManager.Instance.CurrentGameMode != GameManager.GameMode.BOSSRUSH && SaveAPIManager.GetFlag(CustomDungeonFlags.ALLJAMMEDMODE_ENABLED_GENUINE))
                    {
                        if (enemyGuid == "4d164ba3f62648809a4a82c90fc22cae" && !SaveAPIManager.GetFlag(CustomDungeonFlags.ALLJAMMED_BEATEN_RAT))
                        {
                            SaveAPIManager.SetFlag(CustomDungeonFlags.ALLJAMMED_BEATEN_RAT, true);
                        }
                    }
                }
                if (GameManager.Instance.Dungeon.tileIndices.tilesetId == GlobalDungeonData.ValidTilesets.CATACOMBGEON)
                {
                    if (Challenges.CurrentChallenge == ChallengeType.KEEP_IT_COOL)
                    {
                        if (!SaveAPIManager.GetFlag(CustomDungeonFlags.CHALLENGE_KEEPITCOOL_BEATEN))
                        {
                            SaveAPIManager.SetFlag(CustomDungeonFlags.CHALLENGE_KEEPITCOOL_BEATEN, true);
                        }
                    }

                    if (GameManager.Instance.CurrentGameMode != GameManager.GameMode.BOSSRUSH && SaveAPIManager.GetFlag(CustomDungeonFlags.ALLJAMMEDMODE_ENABLED_GENUINE))
                    {
                        if (!SaveAPIManager.GetFlag(CustomDungeonFlags.ALLJAMMED_BEATEN_HOLLOW))
                        {
                            SaveAPIManager.SetFlag(CustomDungeonFlags.ALLJAMMED_BEATEN_HOLLOW, true);
                        }
                    }
                }
                if (GameManager.Instance.Dungeon.tileIndices.tilesetId == GlobalDungeonData.ValidTilesets.OFFICEGEON)
                {
                    if (GameManager.Instance.CurrentGameMode != GameManager.GameMode.BOSSRUSH && SaveAPIManager.GetFlag(CustomDungeonFlags.ALLJAMMEDMODE_ENABLED_GENUINE))
                    {
                        if (!SaveAPIManager.GetFlag(CustomDungeonFlags.ALLJAMMED_BEATEN_OFFICE))
                        {
                            SaveAPIManager.SetFlag(CustomDungeonFlags.ALLJAMMED_BEATEN_OFFICE, true);
                        }
                    }
                }

                //SPECIFIC BOSS KILLING UNLOCKS
                if (GameManager.Instance.Dungeon.tileIndices.tilesetId == GlobalDungeonData.ValidTilesets.FORGEGEON)
                {
                    if (GameManager.Instance.CurrentGameMode != GameManager.GameMode.BOSSRUSH)
                    {
                        if (usingShade)
                        {
                            if (!SaveAPIManager.GetFlag(CustomDungeonFlags.DRAGUN_BEATEN_SHADE))
                            {
                                SaveAPIManager.SetFlag(CustomDungeonFlags.DRAGUN_BEATEN_SHADE, true);
                            }
                        }

                        if (usingDog)
                        {
                            if (!SaveAPIManager.GetFlag(CustomDungeonFlags.KILLED_DRAGUN_WITH_DOG))
                            {
                                SaveAPIManager.SetFlag(CustomDungeonFlags.KILLED_DRAGUN_WITH_DOG, true);
                            }
                        }

                        if (Challenges.CurrentChallenge == ChallengeType.WHAT_ARMY)
                        {
                            if (!SaveAPIManager.GetFlag(CustomDungeonFlags.CHALLENGE_WHATARMY_BEATEN))
                            {
                                SaveAPIManager.SetFlag(CustomDungeonFlags.CHALLENGE_WHATARMY_BEATEN, true);
                            }
                        }

                        if (Challenges.CurrentChallenge == ChallengeType.TOIL_AND_TROUBLE)
                        {
                            if (!SaveAPIManager.GetFlag(CustomDungeonFlags.CHALLENGE_TOILANDTROUBLE_BEATEN))
                            {
                                SaveAPIManager.SetFlag(CustomDungeonFlags.CHALLENGE_TOILANDTROUBLE_BEATEN, true);
                            }
                        }

                        if (Challenges.CurrentChallenge == ChallengeType.INVISIBLEO)
                        {
                            if (!SaveAPIManager.GetFlag(CustomDungeonFlags.CHALLENGE_INVISIBLEO_BEATEN))
                            {
                                SaveAPIManager.SetFlag(CustomDungeonFlags.CHALLENGE_INVISIBLEO_BEATEN, true);
                            }
                        }

                        if (GameManager.Instance.CurrentGameMode != GameManager.GameMode.BOSSRUSH && SaveAPIManager.GetFlag(CustomDungeonFlags.ALLJAMMEDMODE_ENABLED_GENUINE))
                        {
                            if (!SaveAPIManager.GetFlag(CustomDungeonFlags.ALLJAMMED_BEATEN_FORGE))
                            {
                                SaveAPIManager.SetFlag(CustomDungeonFlags.ALLJAMMED_BEATEN_FORGE, true);
                            }
                        }


                        if (enemyGuid == "05b8afe0b6cc4fffa9dc6036fa24c8ec") //ADVANCED DRAGUN
                        {
                            if (usingShade)
                            {
                                if (!SaveAPIManager.GetFlag(CustomDungeonFlags.ADVDRAGUN_KILLED_SHADE))
                                {
                                    SaveAPIManager.SetFlag(CustomDungeonFlags.ADVDRAGUN_KILLED_SHADE, true);
                                }
                            }
                        }

                    }

                }
                if (GameManager.Instance.Dungeon.tileIndices.tilesetId == GlobalDungeonData.ValidTilesets.HELLGEON)
                {
                    if (GameManager.Instance.CurrentGameMode != GameManager.GameMode.BOSSRUSH)
                    {
                        if (enemyGuid == "7c5d5f09911e49b78ae644d2b50ff3bf")
                        {
                            if (SaveAPIManager.GetFlag(CustomDungeonFlags.ALLJAMMEDMODE_ENABLED_GENUINE) && !SaveAPIManager.GetFlag(CustomDungeonFlags.ALLJAMMED_BEATEN_HELL))
                            {
                                SaveAPIManager.SetFlag(CustomDungeonFlags.ALLJAMMED_BEATEN_HELL, true);
                            }

                            if (activeCharacters.Contains(PlayableCharacters.Eevee))
                            {
                                if (!SaveAPIManager.GetFlag(CustomDungeonFlags.UNLOCKED_MISSINGUNO))
                                {
                                    SaveAPIManager.SetFlag(CustomDungeonFlags.UNLOCKED_MISSINGUNO, true);
                                }
                            }
                            if (usingShade)
                            {
                                if (!SaveAPIManager.GetFlag(CustomDungeonFlags.LICH_BEATEN_SHADE))
                                {
                                    SaveAPIManager.SetFlag(CustomDungeonFlags.LICH_BEATEN_SHADE, true);
                                }
                            }
                        }
                    }
                }
            }

            yield break;
        }
    }
}