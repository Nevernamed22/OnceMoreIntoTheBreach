using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace NevernamedsItems
{
    class Challenges
    {
        public static void Init()
        {
            CurrentChallenge = ChallengeType.NONE;

            ETGModConsole.Commands.AddGroup("nnchallenges", delegate (string[] args)
            {
                ETGModConsole.Log("<size=100><color=#ff0000ff>List of Custom Challenges</color></size>", false);
                ETGModConsole.Log("Custom challenges present in Once More Into The Breach.", false);
                ETGModConsole.Log("Type 'nnchallenges [challengeid]' to start the challenge (can only be done from the Breach).", false);
                ETGModConsole.Log("Toil and Trouble:  [id]<color=#ff0000ff>toilandtrouble</color> - All enemy spawns are doubled. Non-doubleable bosses gain a health boost", false);
                ETGModConsole.Log("What Army?:  [id]<color=#ff0000ff>whatarmy</color> - All enemy spawns are randomised.", false);
                ETGModConsole.Log("Invisible-O:  [id]<color=#ff0000ff>invisibleo</color> - The player, their gun, and their bullets are all completely invisible!", false);
                ETGModConsole.Log("Keep It Cool:  [id]<color=#ff0000ff>keepitcool</color> - Permanent ice physics. 45% chance to fire freezing shots.", false);
                ETGModConsole.Log("Challenges will be automatically disabled if Blessed Mode or Rainbow Mode are enabled, if a shortcut is taken from the Breach, or if the player is the Gunslinger or Paradox.", false);
            });

            ETGModConsole.Commands.GetGroup("nnchallenges").AddUnit("clear", delegate (string[] args)
            {
                if (GameManager.Instance.IsFoyer)
                {
                    ETGModConsole.Log("Challenge Removed");
                    CurrentChallenge = ChallengeType.NONE;
                }
                else
                {
                    ETGModConsole.Log("<color=#ff0000ff>Challenges can only be activated or deactivated from the Breach.</color>", false);
                }
            });
            ETGModConsole.Commands.GetGroup("nnchallenges").AddUnit("whatarmy", delegate (string[] args)
            {
                string failure = FetchFailureType();
                if (failure == "none")
                {
                    ETGModConsole.Log("You are now playing; What Army?");
                    CurrentChallenge = ChallengeType.WHAT_ARMY;
                }
                else
                {
                    ETGModConsole.Log(string.Format("<color=#ff0000ff>{0}</color>", failure), false);
                }
            });
            ETGModConsole.Commands.GetGroup("nnchallenges").AddUnit("toilandtrouble", delegate (string[] args)
            {
                if (GameManager.Instance.IsFoyer)
                {
                    ETGModConsole.Log("You are now playing; Toil and Trouble");
                    CurrentChallenge = ChallengeType.TOIL_AND_TROUBLE;
                }
                else
                {
                    ETGModConsole.Log("<color=#ff0000ff>Challenges can only be activated or deactivated from the Breach.</color>", false);
                }
            });
            ETGModConsole.Commands.GetGroup("nnchallenges").AddUnit("invisibleo", delegate (string[] args)
            {
                if (GameManager.Instance.IsFoyer)
                {
                    ETGModConsole.Log("You are now playing; Invisible-O");
                    CurrentChallenge = ChallengeType.INVISIBLEO;
                }
                else
                {
                    ETGModConsole.Log("<color=#ff0000ff>Challenges can only be activated or deactivated from the Breach.</color>", false);
                }
            });
            ETGModConsole.Commands.GetGroup("nnchallenges").AddUnit("keepitcool", delegate (string[] args)
            {
                if (GameManager.Instance.IsFoyer)
                {
                    ETGModConsole.Log("You are now playing; Keep it Cool");
                    CurrentChallenge = ChallengeType.KEEP_IT_COOL;
                }
                else
                {
                    ETGModConsole.Log("<color=#ff0000ff>Challenges can only be activated or deactivated from the Breach.</color>", false);
                }
            });

            ETGMod.AIActor.OnPostStart += Challenges.AIActorPostSpawn;
        }
        public static ChallengeType CurrentChallenge;
        public static void AIActorPostSpawn(AIActor AIActor)
        {
            if (CurrentChallenge == ChallengeType.WHAT_ARMY)
            {
                if (AIActor && AIActor.healthHaver && !AIActor.healthHaver.IsBoss && !AIActor.healthHaver.IsSubboss && !AIActor.IsSecretlyTheMineFlayer())
                {
                    if (AIActor.gameObject.GetComponent<HasBeenAffectedByCurrentChallenge>() == null && AIActor.gameObject.GetComponent<CompanionController>() == null)
                    {
                        float proc = 1;
                        if (AIActor.GetAbsoluteParentRoom().area.PrototypeRoomCategory == PrototypeDungeonRoom.RoomCategory.BOSS && AIActor.GetAbsoluteParentRoom().RoomContainsMineFlayer()) proc = 0.2f;
                        if (UnityEngine.Random.value <= proc)
                        {
                            List<string> ChaosPalette = GenerateChaosPalette();
                            string guid = BraveUtility.RandomElement(ChaosPalette);
                            var enemyPrefab = EnemyDatabase.GetOrLoadByGuid(guid);
                            AIActor aiactor = AIActor.Spawn(enemyPrefab, AIActor.gameActor.CenterPosition.ToIntVector2(VectorConversions.Floor), AIActor.GetAbsoluteParentRoom(), true, AIActor.AwakenAnimationType.Default, true);
                            aiactor.gameObject.AddComponent<HasBeenAffectedByCurrentChallenge>();
                            aiactor.AssignedCurrencyToDrop = AIActor.AssignedCurrencyToDrop;
                            aiactor.AdditionalSafeItemDrops = AIActor.AdditionalSafeItemDrops;
                            aiactor.AdditionalSimpleItemDrops = AIActor.AdditionalSimpleItemDrops;
                            aiactor.CanTargetEnemies = AIActor.CanTargetEnemies;
                            aiactor.CanTargetPlayers = AIActor.CanTargetPlayers;
                            if (AIActor.IsInReinforcementLayer) aiactor.HandleReinforcementFallIntoRoom(0f);
                            if (aiactor.EnemyGuid == "22fc2c2c45fb47cf9fb5f7b043a70122") aiactor.CollisionDamage = 0f;
                            if (AIActor.GetComponent<ExplodeOnDeath>() != null) UnityEngine.Object.Destroy(AIActor.GetComponent<ExplodeOnDeath>());
                            AIActor.EraseFromExistence(true);
                        }
                    }
                }
            }
            else if (CurrentChallenge == ChallengeType.TOIL_AND_TROUBLE)
            {
                if (AIActor && AIActor.healthHaver && !AIActor.healthHaver.IsBoss && !AIActor.healthHaver.IsSubboss && !AIActor.IsSecretlyTheMineFlayer())
                {
                    if (AIActor.GetComponent<CompanionController>() == null && AIActor.GetComponent<HasBeenAffectedByCurrentChallenge>() == null)
                    {
                        string guid = AIActor.EnemyGuid;
                        var enemyPrefab = EnemyDatabase.GetOrLoadByGuid(guid);
                        AIActor aiactor = AIActor.Spawn(enemyPrefab, AIActor.gameActor.CenterPosition.ToIntVector2(VectorConversions.Floor), AIActor.GetAbsoluteParentRoom(), true, AIActor.AwakenAnimationType.Default, true);

                        HasBeenAffectedByCurrentChallenge challengitude = aiactor.gameObject.AddComponent<HasBeenAffectedByCurrentChallenge>();
                        challengitude.linkedOther = AIActor;
                        HasBeenAffectedByCurrentChallenge challengitude2 = AIActor.gameObject.AddComponent<HasBeenAffectedByCurrentChallenge>();
                        challengitude2.linkedOther = aiactor;
                        aiactor.procedurallyOutlined = false;
                        AIActor.procedurallyOutlined = false;
                        aiactor.AssignedCurrencyToDrop = AIActor.AssignedCurrencyToDrop;
                        aiactor.AdditionalSafeItemDrops = AIActor.AdditionalSafeItemDrops;
                        aiactor.AdditionalSimpleItemDrops = AIActor.AdditionalSimpleItemDrops;
                        aiactor.CanTargetEnemies = AIActor.CanTargetEnemies;
                        aiactor.CanTargetPlayers = AIActor.CanTargetPlayers;
                        if (AIActor.IsInReinforcementLayer) aiactor.HandleReinforcementFallIntoRoom(0f);
                        if (aiactor.EnemyGuid == "22fc2c2c45fb47cf9fb5f7b043a70122")
                        {
                            aiactor.CollisionDamage = 0f;
                            aiactor.specRigidbody.AddCollisionLayerIgnoreOverride(CollisionMask.LayerToMask(CollisionLayer.PlayerHitBox));
                            aiactor.specRigidbody.AddCollisionLayerIgnoreOverride(CollisionMask.LayerToMask(CollisionLayer.Projectile));
                        }
                        else if (aiactor.EnemyGuid == "249db525a9464e5282d02162c88e0357")
                        {
                            if (aiactor.gameObject.GetComponent<SpawnEnemyOnDeath>())
                            {
                                UnityEngine.Object.Destroy(aiactor.gameObject.GetComponent<SpawnEnemyOnDeath>());
                            }
                        }

                        //Shrinky
                        int cachedLayer = aiactor.gameObject.layer;
                        int cachedOutlineLayer = cachedLayer;
                        aiactor.gameObject.layer = LayerMask.NameToLayer("Unpixelated");
                        cachedOutlineLayer = SpriteOutlineManager.ChangeOutlineLayer(aiactor.sprite, LayerMask.NameToLayer("Unpixelated"));
                        aiactor.EnemyScale = TargetScale;
                        aiactor.gameObject.layer = cachedLayer;
                        SpriteOutlineManager.ChangeOutlineLayer(aiactor.sprite, cachedOutlineLayer);


                        int cachedLayer2 = AIActor.gameObject.layer;
                        int cachedOutlineLayer2 = cachedLayer2;
                        AIActor.gameObject.layer = LayerMask.NameToLayer("Unpixelated");
                        cachedOutlineLayer2 = SpriteOutlineManager.ChangeOutlineLayer(AIActor.sprite, LayerMask.NameToLayer("Unpixelated"));
                        AIActor.EnemyScale = TargetScale;
                        AIActor.gameObject.layer = cachedLayer2;
                        SpriteOutlineManager.ChangeOutlineLayer(AIActor.sprite, cachedOutlineLayer2);

                        aiactor.specRigidbody.Reinitialize();
                    }
                }
                else if (AIActor && AIActor.healthHaver && (AIActor.healthHaver.IsBoss || AIActor.healthHaver.IsSubboss) && !AIActor.IsSecretlyTheMineFlayer())
                {
                    if (AIActor.GetComponent<HasBeenAffectedByCurrentChallenge>() == null)
                    {
                        if (ValidDoubleableBosses.Contains(AIActor.EnemyGuid))
                        {
                            string guid = AIActor.EnemyGuid;
                            var enemyPrefab = EnemyDatabase.GetOrLoadByGuid(guid);
                            AIActor aiactor = AIActor.Spawn(enemyPrefab, AIActor.gameActor.CenterPosition.ToIntVector2(VectorConversions.Floor), AIActor.GetAbsoluteParentRoom(), true, AIActor.AwakenAnimationType.Default, true);

                            HasBeenAffectedByCurrentChallenge challengitude = aiactor.gameObject.AddComponent<HasBeenAffectedByCurrentChallenge>();
                            challengitude.linkedOther = AIActor;
                            HasBeenAffectedByCurrentChallenge challengitude2 = AIActor.gameObject.AddComponent<HasBeenAffectedByCurrentChallenge>();
                            challengitude2.linkedOther = aiactor;
                            aiactor.AssignedCurrencyToDrop = AIActor.AssignedCurrencyToDrop;
                            aiactor.AdditionalSafeItemDrops = AIActor.AdditionalSafeItemDrops;
                            aiactor.AdditionalSimpleItemDrops = AIActor.AdditionalSimpleItemDrops;

                            if (AIActor.GetComponent<BroController>())
                            {
                                aiactor.gameObject.GetOrAddComponent<BroController>();
                            }


                            float actorOrigHP = AIActor.healthHaver.GetMaxHealth();
                            float actorNewHP = aiactor.healthHaver.GetMaxHealth();
                            AIActor.healthHaver.SetHealthMaximum(actorOrigHP * 0.5f);
                            AIActor.healthHaver.ForceSetCurrentHealth(actorOrigHP * 0.5f);
                            aiactor.healthHaver.SetHealthMaximum(actorNewHP * 0.5f);
                            aiactor.healthHaver.ForceSetCurrentHealth(actorNewHP * 0.5f);
                        }
                        else
                        {
                            float actorHP = AIActor.healthHaver.GetMaxHealth();
                            AIActor.healthHaver.SetHealthMaximum(actorHP * 1.5f);
                            AIActor.healthHaver.ForceSetCurrentHealth(actorHP * 1.5f);
                        }
                    }
                }
            }
        }
        public static Vector2 TargetScale = new Vector2(0.75f, 0.75f);
        public static void OnLevelLoaded()
        {
            if (CurrentChallenge != ChallengeType.NONE)
            {
                //ETGModConsole.Log("Challenge was not null!");
                PlayerController player1 = GameManager.Instance.PrimaryPlayer;
                if (player1 == null) ETGModConsole.Log("ERRA PLAYA NULLA");
                PlayerController player2 = null;
                if (GameManager.Instance.SecondaryPlayer != null) player2 = GameManager.Instance.SecondaryPlayer;
                if (GameStatsManager.Instance.IsRainbowRun)
                {
                    ETGModConsole.Log("<color=#ff0000ff>Challenge Voided: Played Rainbow Run</color>", false);
                    TextBubble.DoAmbientTalk(player1.transform, new Vector3(1, 2, 0), "Challenge Voided: Played Rainbow Run", 4f);
                    CurrentChallenge = ChallengeType.NONE;
                }
                else if (player1.CharacterUsesRandomGuns || (player2 && player2.CharacterUsesRandomGuns))
                {
                    ETGModConsole.Log("<color=#ff0000ff>Challenge Voided: Played Blessed Run</color>", false);
                    CurrentChallenge = ChallengeType.NONE;
                    TextBubble.DoAmbientTalk(player1.transform, new Vector3(1, 2, 0), "Challenge Voided: Played Blessed Run", 4f);
                }
                else if (player1.characterIdentity == PlayableCharacters.Gunslinger || (player2 && player2.characterIdentity == PlayableCharacters.Gunslinger))
                {
                    ETGModConsole.Log("<color=#ff0000ff>Challenge Voided: Played as Gunslinger</color>", false);
                    CurrentChallenge = ChallengeType.NONE;
                    TextBubble.DoAmbientTalk(player1.transform, new Vector3(1, 2, 0), "Challenge Voided: Played as Gunslinger", 4f);
                }
                else if (player1.characterIdentity == PlayableCharacters.Eevee || (player2 && player2.characterIdentity == PlayableCharacters.Eevee))
                {
                    ETGModConsole.Log("<color=#ff0000ff>Challenge Voided: Played as Paradox</color>", false);
                    CurrentChallenge = ChallengeType.NONE;
                    TextBubble.DoAmbientTalk(player1.transform, new Vector3(1, 2, 0), "Challenge Voided: Played as Paradox", 4f);
                }
                else if (GameManager.Instance.CurrentGameMode == GameManager.GameMode.SHORTCUT)
                {
                    ETGModConsole.Log("<color=#ff0000ff>Challenge Voided: Took a Shortcut</color>", false);
                    CurrentChallenge = ChallengeType.NONE;
                    TextBubble.DoAmbientTalk(player1.transform, new Vector3(1, 2, 0), "Challenge Voided: Took a Shortcut", 4f);
                }
                else if (GameManager.Instance.CurrentGameMode == GameManager.GameMode.BOSSRUSH)
                {
                    ETGModConsole.Log("<color=#ff0000ff>Challenge Voided: Entered Bossrush</color>", false);
                    CurrentChallenge = ChallengeType.NONE;
                    TextBubble.DoAmbientTalk(player1.transform, new Vector3(1, 2, 0), "Challenge Voided: Entered Bossrush", 4f);
                }
                else if (GameManager.Instance.InTutorial)
                {
                    ETGModConsole.Log("<color=#ff0000ff>Challenge Voided: Entered Tutorial</color>", false);
                    CurrentChallenge = ChallengeType.NONE;
                    TextBubble.DoAmbientTalk(player1.transform, new Vector3(1, 2, 0), "Challenge Voided: Entered Tutorial", 4f);
                }
            }
        }
        public static List<string> GenerateChaosPalette()
        {
            List<string> ChaosPalette = new List<string>();
            ChaosPalette.AddRange(MagickeCauldron.chaosEnemyPalette);
            if (ModInstallFlags.ExpandTheGungeonInstalled) ChaosPalette.AddRange(MagickeCauldron.ExpandTheGungeonChaosPalette);
            if (ModInstallFlags.FrostAndGunfireInstalled) ChaosPalette.AddRange(MagickeCauldron.FrostAndGunfireChaosPalette);
            if (ModInstallFlags.PlanetsideOfGunymededInstalled) ChaosPalette.AddRange(MagickeCauldron.PlanetsideOfGunymedeChaosPalette);
            return ChaosPalette;
        }
        private static string FetchFailureType()
        {
            string failureMessage = "none";
            bool skipChecks = false;
            PlayerController player1 = GameManager.Instance.PrimaryPlayer;
            if (player1 == null)
            {
                ETGModConsole.Log("ERRA PLAYA NULLA");
                failureMessage = "Please select a character before enabling the challenge.";
                skipChecks = true;
            }
            PlayerController player2 = null;
            if (GameManager.Instance.SecondaryPlayer != null) player2 = GameManager.Instance.SecondaryPlayer;

            if (!skipChecks)
            {
                if (!GameManager.Instance.IsFoyer) failureMessage = "Challenges can only be enabled or disabled from the Breach";
                else if (GameStatsManager.Instance.IsRainbowRun) failureMessage = "Challenges cannot be played in Rainbow Mode";
                else if (player1.CharacterUsesRandomGuns || (player2 && player2.CharacterUsesRandomGuns))
                {
                    failureMessage = "Challenges cannot be played in Blessed Mode";
                }
                else if (player1.characterIdentity == PlayableCharacters.Gunslinger || (player2 && player2.characterIdentity == PlayableCharacters.Gunslinger))
                {
                    failureMessage = "Challenges cannot be played as the Gunslinger";
                }
                else if (player1.characterIdentity == PlayableCharacters.Eevee || (player2 && player2.characterIdentity == PlayableCharacters.Eevee))
                {
                    failureMessage = "Challenges cannot be played as the Paradox";
                }
            }
            return failureMessage;
        }
        public class HasBeenAffectedByCurrentChallenge : MonoBehaviour
        {
            public HasBeenAffectedByCurrentChallenge()
            {
                linkedOther = null;
            }
            private void Start()
            {
                self = base.GetComponent<AIActor>();
                if (self && self.specRigidbody && CurrentChallenge == ChallengeType.TOIL_AND_TROUBLE)
                {
                    self.specRigidbody.OnPreRigidbodyCollision += this.OnPreCollide;
                }
            }
            private void OnPreCollide(SpeculativeRigidbody myRigidbody, PixelCollider myCollider, SpeculativeRigidbody otherRigidbody, PixelCollider otherCollider)
            {
                if (CurrentChallenge == ChallengeType.TOIL_AND_TROUBLE && linkedOther != null & linkedOther.specRigidbody != null)
                {
                    if (otherRigidbody == linkedOther.specRigidbody)
                    {
                        PhysicsEngine.SkipCollision = true;
                    }
                }
            }
            private AIActor self;
            public AIActor linkedOther;
        }
        private static List<string> ValidDoubleableBosses = new List<string>()
        {
            EnemyGuidDatabase.Entries["blockner_rematch"],
            EnemyGuidDatabase.Entries["fuselier"],
            EnemyGuidDatabase.Entries["shadow_agunim"],
            EnemyGuidDatabase.Entries["gatling_gull"],
            EnemyGuidDatabase.Entries["bullet_king"],
            EnemyGuidDatabase.Entries["blobulord"],
            EnemyGuidDatabase.Entries["beholster"],
            EnemyGuidDatabase.Entries["gorgun"],
            EnemyGuidDatabase.Entries["ammoconda"],
            EnemyGuidDatabase.Entries["old_king"],
            EnemyGuidDatabase.Entries["treadnaught"],
            EnemyGuidDatabase.Entries["mine_flayer"],
            //EnemyGuidDatabase.Entries["cannonbalrog"],
            EnemyGuidDatabase.Entries["door_lord"],
            EnemyGuidDatabase.Entries["helicopter_agunim"],
            "6c43fddfd401456c916089fdd1c99b1c", //High Priest
            "ea40fcc863d34b0088f490f4e57f8913", //Smiley
        "c00390483f394a849c36143eb878998f", //shades
        };
    }

    public enum ChallengeType
    {
        NONE,
        TOIL_AND_TROUBLE,
        WHAT_ARMY,
        INVISIBLEO,
        KEEP_IT_COOL,
    }
}
