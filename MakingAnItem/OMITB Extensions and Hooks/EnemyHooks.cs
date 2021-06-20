using MonoMod.RuntimeDetour;
using SaveAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using UnityEngine;

namespace NevernamedsItems
{
    class EnemyHooks
    {
        public static void InitEnemyHooks()
        {
            HHDieHook = new Hook(
                typeof(HealthHaver).GetMethod("Die", BindingFlags.Instance | BindingFlags.Public),
                typeof(EnemyHooks).GetMethod("onHealthHaverDie")
            );
            ETGMod.AIActor.OnPostStart += onEnemyPostSpawn;
        }

        public static void onEnemyPostSpawn(AIActor enemy)
        {
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
        class SteedBlobController : MonoBehaviour
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
        public static void onHealthHaverDie(Action<HealthHaver, Vector2> orig, HealthHaver self, Vector2 finalDamageDir)
        {
            if (self.aiActor != null && self.specRigidbody != null)
            {
                PlayerController player1 = null;
                PlayerController player2 = null;
                if (GameManager.Instance.PrimaryPlayer != null) player1 = GameManager.Instance.PrimaryPlayer;
                if (GameManager.Instance.SecondaryPlayer != null) player2 = GameManager.Instance.SecondaryPlayer;

                if (self.aiActor.CanTargetEnemies = true && self.aiActor.CanTargetPlayers == false)
                {
                    SaveAPIManager.RegisterStatChange(CustomTrackedStats.CHARMED_ENEMIES_KILLED, 1);
                } //Count Charmed Enemy Kills
                if (self.aiActor.EnemyGuid == EnemyGuidDatabase.Entries["key_bullet_kin"])
                {
                    if (self.aiActor.IsBlackPhantom && !SaveAPIManager.GetFlag(CustomDungeonFlags.KILLEDJAMMEDKEYBULLETKIN))
                    {
                        SaveAPIManager.SetFlag(CustomDungeonFlags.KILLEDJAMMEDKEYBULLETKIN, true);
                    }
                } //Check for Jammed Keybullet kill
                if (self.aiActor.EnemyGuid == EnemyGuidDatabase.Entries["chance_bullet_kin"])
                {
                    if (self.aiActor.IsBlackPhantom && !SaveAPIManager.GetFlag(CustomDungeonFlags.KILLEDJAMMEDCHANCEKIN))
                    {
                        SaveAPIManager.SetFlag(CustomDungeonFlags.KILLEDJAMMEDCHANCEKIN, true);
                    }
                } //Check for Jammed Chance Kin Kill
                if (EasyEnemyTypeLists.TitanBulletKin.Contains(self.aiActor.EnemyGuid)) //Count Titan kills
                {
                    SaveAPIManager.RegisterStatChange(CustomTrackedStats.TITAN_KIN_KILLED, 1);
                } //Count Titan Kills
                if (EasyEnemyTypeLists.ModInclusiveMimics.Contains(self.aiActor.EnemyGuid))
                {
                    if (self.aiActor.IsBlackPhantom && !SaveAPIManager.GetFlag(CustomDungeonFlags.KILLEDJAMMEDMIMIC))
                    {
                        SaveAPIManager.SetFlag(CustomDungeonFlags.KILLEDJAMMEDMIMIC, true);
                    }
                } //Check for Jammed Mimic Kill

                //BOSS STUFF

                if (self.aiActor.EnemyGuid == EnemyGuidDatabase.Entries["dragun_advanced"])
                {
                    AdvancedDragunRewards.handleDragunRewards();
                } //Trigger advanced dragun rewards

                if (self.IsBoss && !self.IsSubboss && !GameManager.Instance.InTutorial)
                {
                    //Unlock for the Mutagen, checking if the player is under a certain hp
                    #region MutagenUnlock
                    bool player1Valid = ((player1.healthHaver.GetCurrentHealth() <= 0f && player1.healthHaver.Armor == 1) || (player1.healthHaver.GetCurrentHealth() <= 0.5f && player1.healthHaver.Armor == 0));
                    bool player2Valid = false;
                    if (player2) player2Valid = ((player2.healthHaver.GetCurrentHealth() <= 0f && player2.healthHaver.Armor == 1) || (player2.healthHaver.GetCurrentHealth() <= 0.5f && player2.healthHaver.Armor == 0));
                    if (player1Valid || player2Valid)
                    {
                        if (!SaveAPIManager.GetFlag(CustomDungeonFlags.HAS_BEATEN_BOSS_BY_SKIN_OF_TEETH))
                        {
                            SaveAPIManager.SetFlag(CustomDungeonFlags.HAS_BEATEN_BOSS_BY_SKIN_OF_TEETH, true);
                        }
                    }
                    #endregion

                    //MODE RELATED UNLOCKS
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
                    } //Turbo Unlocks
                    if (GameStatsManager.Instance.IsRainbowRun) //Rainbow Unlocks
                    {
                        if (self.aiActor.EnemyGuid == EnemyGuidDatabase.Entries["infinilich"])
                        {
                            if (!SaveAPIManager.GetFlag(CustomDungeonFlags.RAINBOW_KILLED_LICH))
                            {
                                SaveAPIManager.SetFlag(CustomDungeonFlags.RAINBOW_KILLED_LICH, true);
                            }
                        }
                    }

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
                            if (self.aiActor && self.aiActor.EnemyGuid == "4d164ba3f62648809a4a82c90fc22cae" && !SaveAPIManager.GetFlag(CustomDungeonFlags.ALLJAMMED_BEATEN_RAT))
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
                        if (player1.HasPickupID(300) || (player2 && player2.HasPickupID(300))) //Dog
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
                    }
                    if (GameManager.Instance.Dungeon.tileIndices.tilesetId == GlobalDungeonData.ValidTilesets.HELLGEON)
                    {
                        if (GameManager.Instance.CurrentGameMode != GameManager.GameMode.BOSSRUSH)
                        {
                            if (self.aiActor && self.aiActor.EnemyGuid == "7c5d5f09911e49b78ae644d2b50ff3bf")
                            {
                                if (SaveAPIManager.GetFlag(CustomDungeonFlags.ALLJAMMEDMODE_ENABLED_GENUINE) && !SaveAPIManager.GetFlag(CustomDungeonFlags.ALLJAMMED_BEATEN_HELL))
                                {
                                    SaveAPIManager.SetFlag(CustomDungeonFlags.ALLJAMMED_BEATEN_HELL, true);
                                }
                                if ((player1 && player1.characterIdentity == PlayableCharacters.Eevee) || (player2 && player2.characterIdentity == PlayableCharacters.Eevee))
                                {
                                    if (!SaveAPIManager.GetFlag(CustomDungeonFlags.UNLOCKED_MISSINGUNO))
                                    {
                                        SaveAPIManager.SetFlag(CustomDungeonFlags.UNLOCKED_MISSINGUNO, true);
                                    }
                                }
                            }
                        }
                    }



                    //BOSS KILL COUNTS
                    if (self.aiActor.EnemyGuid == EnemyGuidDatabase.Entries["beholster"])
                    {
                        SaveAPIManager.RegisterStatChange(CustomTrackedStats.BEHOLSTER_KILLS, 1);
                    }
                    else if (self.aiActor.EnemyGuid == EnemyGuidDatabase.Entries["mine_flayer"])
                    {
                        SaveAPIManager.RegisterStatChange(CustomTrackedStats.MINEFLAYER_KILLS, 1);
                    }
                }
            }
            orig(self, finalDamageDir);
        }
    }
}
