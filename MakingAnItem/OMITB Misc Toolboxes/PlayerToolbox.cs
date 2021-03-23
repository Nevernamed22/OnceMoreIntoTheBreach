using MonoMod.RuntimeDetour;
using SaveAPI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using UnityEngine;

namespace NevernamedsItems
{
    class PlayerToolsSetup
    {
        public static void Init()
        {
            playerStartHook = new Hook(
                typeof(PlayerController).GetMethod("orig_Start", BindingFlags.Public | BindingFlags.Instance),
                typeof(PlayerToolsSetup).GetMethod("DoSetup"));
        }
        public static void DoSetup(Action<PlayerController> action, PlayerController player)
        {
            action(player);
            if (player.GetComponent<HatController>() == null) player.gameObject.AddComponent<HatController>();
            if (player.GetComponent<PlayerToolbox>() == null) player.gameObject.AddComponent<PlayerToolbox>();
        }
        public static Hook playerStartHook;
    }
    class PlayerToolbox : MonoBehaviour
    {
        private void Start()
        {
            m_attachedPlayer = base.GetComponent<PlayerController>();
            if (m_attachedPlayer)
            {
                m_attachedPlayer.OnAnyEnemyReceivedDamage += this.OnPlayerDamagedEnemy;
                m_attachedPlayer.PostProcessThrownGun += this.PostProcessThrownGun;
                if (GameManager.Instance.SecondaryPlayer != null && GameManager.Instance.SecondaryPlayer == m_attachedPlayer) { isSecondaryPlayer = true; }
            }
            DoubleDamageStatMod = new StatModifier();
            DoubleDamageStatMod.statToBoost = PlayerStats.StatType.Damage;
            DoubleDamageStatMod.amount = 2f;
            DoubleDamageStatMod.modifyType = StatModifier.ModifyMethod.MULTIPLICATIVE;
        }
        private void OnPlayerDamagedEnemy(float huh, bool fatal, HealthHaver enemy)
        {
            if (isSecondaryPlayer && GameManager.Instance.PrimaryPlayer && !GameManager.Instance.PrimaryPlayer.IsGhost) return;
            if (fatal)
            {
                if (enemy.aiActor)
                {
                    if (enemy.aiActor.CanTargetEnemies = true && enemy.aiActor.CanTargetPlayers == false)
                    {
                        SaveAPIManager.RegisterStatChange(CustomTrackedStats.CHARMED_ENEMIES_KILLED, 1);
                    }
                    if (enemy.aiActor.EnemyGuid == EnemyGuidDatabase.Entries["key_bullet_kin"])
                    {
                        if (enemy.aiActor.IsBlackPhantom && !SaveAPIManager.GetFlag(CustomDungeonFlags.KILLEDJAMMEDKEYBULLETKIN))
                        {
                            SaveAPIManager.SetFlag(CustomDungeonFlags.KILLEDJAMMEDKEYBULLETKIN, true);
                        }
                    }
                    if (enemy.aiActor.EnemyGuid == EnemyGuidDatabase.Entries["chance_bullet_kin"])
                    {
                        if (enemy.aiActor.IsBlackPhantom && !SaveAPIManager.GetFlag(CustomDungeonFlags.KILLEDJAMMEDCHANCEKIN))
                        {
                            SaveAPIManager.SetFlag(CustomDungeonFlags.KILLEDJAMMEDCHANCEKIN, true);
                        }
                    }
                    if (enemy.aiActor.EnemyGuid == EnemyGuidDatabase.Entries["titan_bullet_kin"] || enemy.aiActor.EnemyGuid == EnemyGuidDatabase.Entries["titan_bullet_kin_boss"] || enemy.aiActor.EnemyGuid == EnemyGuidDatabase.Entries["titaness_bullet_kin_boss"])
                    {
                        SaveAPIManager.RegisterStatChange(CustomTrackedStats.TITAN_KIN_KILLED, 1);
                    }
                    if (EasyEnemyTypeLists.ModInclusiveMimics.Contains(enemy.aiActor.EnemyGuid))
                    {
                        if (enemy.aiActor.IsBlackPhantom && !SaveAPIManager.GetFlag(CustomDungeonFlags.KILLEDJAMMEDMIMIC))
                        {
                            SaveAPIManager.SetFlag(CustomDungeonFlags.KILLEDJAMMEDMIMIC, true);
                        }
                    }
                    if (enemy.healthHaver && enemy.healthHaver.IsBoss && !enemy.healthHaver.IsSubboss && !m_attachedPlayer.IsGhost)
                    {
                        bool flag1 = m_attachedPlayer.healthHaver.GetCurrentHealth() <= 0f && m_attachedPlayer.healthHaver.Armor == 1;
                        bool flag2 = m_attachedPlayer.healthHaver.GetCurrentHealth() <= 0.5f && m_attachedPlayer.healthHaver.Armor == 0;
                        if (flag1 || flag2)
                        {
                            if (!SaveAPIManager.GetFlag(CustomDungeonFlags.HAS_BEATEN_BOSS_BY_SKIN_OF_TEETH))
                            {
                                SaveAPIManager.SetFlag(CustomDungeonFlags.HAS_BEATEN_BOSS_BY_SKIN_OF_TEETH, true);
                            }
                        }
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
                        }
                        if (GameStatsManager.Instance.IsRainbowRun)
                        {
                            if (enemy.aiActor.EnemyGuid == EnemyGuidDatabase.Entries["infinilich"])
                            {
                                if (!SaveAPIManager.GetFlag(CustomDungeonFlags.RAINBOW_KILLED_LICH))
                                {
                                    SaveAPIManager.SetFlag(CustomDungeonFlags.RAINBOW_KILLED_LICH, true);
                                }
                            }
                        }
                        //SPECIFIC BOSS KILLING UNLOCKS
                        if (GameManager.Instance.Dungeon.tileIndices.tilesetId == GlobalDungeonData.ValidTilesets.FORGEGEON)
                        {
                            if (m_attachedPlayer.HasPickupID(300)) //Dog
                            {
                                if (!SaveAPIManager.GetFlag(CustomDungeonFlags.KILLED_DRAGUN_WITH_DOG))
                                {
                                    SaveAPIManager.SetFlag(CustomDungeonFlags.KILLED_DRAGUN_WITH_DOG, true);
                                }
                            }
                        }
                        if (enemy.aiActor.EnemyGuid == EnemyGuidDatabase.Entries["beholster"])
                        {
                            SaveAPIManager.RegisterStatChange(CustomTrackedStats.BEHOLSTER_KILLS, 1);
                        }
                        else if (enemy.aiActor.EnemyGuid == EnemyGuidDatabase.Entries["mine_flayer"])
                        {
                            SaveAPIManager.RegisterStatChange(CustomTrackedStats.MINEFLAYER_KILLS, 1);
                        }
                    }
                }
            }
        }
        private void PostProcessThrownGun(Projectile gun)
        {
            gun.OnHitEnemy += this.OnThrownGunHitEnemy;
        }
        private void OnThrownGunHitEnemy(Projectile gun, SpeculativeRigidbody enemy, bool fatal)
        {
            if (enemy && enemy.healthHaver && fatal)
            {
                if (!SaveAPIManager.GetFlag(CustomDungeonFlags.KILLEDENEMYWITHTHROWNGUN))
                {
                    SaveAPIManager.SetFlag(CustomDungeonFlags.KILLEDENEMYWITHTHROWNGUN, true);
                }
            }
        }
        private void Update()
        {
            if (m_attachedPlayer.healthHaver.Armor != armourLastChecked)
            {
                if (!SaveAPIManager.GetFlag(CustomDungeonFlags.PLAYERHELDMORETHANFIVEARMOUR))
                {
                    int threshHold = 5;
                    if (m_attachedPlayer.characterIdentity == PlayableCharacters.Robot) threshHold = 11;
                    if (m_attachedPlayer.healthHaver.Armor >= threshHold) SaveAPIManager.SetFlag(CustomDungeonFlags.PLAYERHELDMORETHANFIVEARMOUR, true);
                }
                armourLastChecked = (int)m_attachedPlayer.healthHaver.Armor;
            }
            if (m_attachedPlayer.stats.GetStatValue(PlayerStats.StatType.Health) != hpStatLastChecked)
            {
                SaveAPIManager.UpdateMaximum(CustomTrackedMaximums.MAX_HEART_CONTAINERS_EVER, m_attachedPlayer.stats.GetStatValue(PlayerStats.StatType.Health));
                hpStatLastChecked = (int)m_attachedPlayer.stats.GetStatValue(PlayerStats.StatType.Health);
            }
            if (m_attachedPlayer.passiveItems.Count != itemCountLastChecked)
            {
                OnInventoryItemsChanged();
                itemCountLastChecked = m_attachedPlayer.passiveItems.Count;
            }
        }

        private void OnInventoryItemsChanged()
        {
            int amountOfJunk = 0;
            foreach (PassiveItem item in m_attachedPlayer.passiveItems)
            {
                if (item.PickupObjectId == 127) amountOfJunk++;
            }
            if (amountOfJunk >= 5 && m_attachedPlayer.characterIdentity == PlayableCharacters.Robot)
            {
                if (!SaveAPIManager.GetFlag(CustomDungeonFlags.ROBOT_HELD_FIVE_JUNK))
                {
                    SaveAPIManager.SetFlag(CustomDungeonFlags.ROBOT_HELD_FIVE_JUNK, true);
                }
            }
        }

        #region RageCode
        public void Enrage(float dur)
        {
            if (remainingRageTime > 0) { remainingRageTime += dur; }
            else m_attachedPlayer.StartCoroutine(HandleRageDur(dur));
        }
        private IEnumerator HandleRageDur(float dur)
        {
            remainingRageTime = dur;
            m_attachedPlayer.stats.RecalculateStats(m_attachedPlayer, true, false);
            this.instanceVFX = m_attachedPlayer.PlayEffectOnActor(RageVFX, new Vector3(0f, 1.375f, 0f), true, true, false);
            ///ETGModConsole.Log("Applied VFX");
            m_attachedPlayer.ownerlessStatModifiers.Add(DoubleDamageStatMod);
            m_attachedPlayer.stats.RecalculateStats(m_attachedPlayer, true, false);
            // ETGModConsole.Log("Applied DMG");

            float elapsed = 0f;
            float particleCounter = 0f;
            while (elapsed < remainingRageTime)
            {
                elapsed += BraveTime.DeltaTime;
                m_attachedPlayer.baseFlatColorOverride = this.flatColorOverride.WithAlpha(Mathf.Lerp(this.flatColorOverride.a, 0f, Mathf.Clamp01(elapsed - (remainingRageTime - 1f))));
                if (GameManager.Options.ShaderQuality != GameOptions.GenericHighMedLowOption.LOW && GameManager.Options.ShaderQuality != GameOptions.GenericHighMedLowOption.VERY_LOW && m_attachedPlayer && m_attachedPlayer.IsVisible && !m_attachedPlayer.IsFalling)
                {
                    particleCounter += BraveTime.DeltaTime * 40f;
                    if (this.instanceVFX && elapsed > 1f)
                    {
                        this.instanceVFX.GetComponent<tk2dSpriteAnimator>().PlayAndDestroyObject("rage_face_vfx_out", null);
                        this.instanceVFX = null;
                    }
                    if (particleCounter > 1f)
                    {
                        int num = Mathf.FloorToInt(particleCounter);
                        particleCounter %= 1f;
                        GlobalSparksDoer.DoRandomParticleBurst(num, m_attachedPlayer.sprite.WorldBottomLeft.ToVector3ZisY(0f), m_attachedPlayer.sprite.WorldTopRight.ToVector3ZisY(0f), Vector3.up, 90f, 0.5f, null, null, null, GlobalSparksDoer.SparksType.BLACK_PHANTOM_SMOKE);
                    }
                }
                yield return null;
            }
            if (this.instanceVFX) this.instanceVFX.GetComponent<tk2dSpriteAnimator>().PlayAndDestroyObject("rage_face_vfx_out", null);
            m_attachedPlayer.ownerlessStatModifiers.Remove(DoubleDamageStatMod);
            m_attachedPlayer.stats.RecalculateStats(m_attachedPlayer, true, false);
            remainingRageTime = 0;
        }
        float remainingRageTime;
        public static GameObject RageVFX = PickupObjectDatabase.GetById(353).GetComponent<RagePassiveItem>().OverheadVFX.gameObject;
        private GameObject instanceVFX;
        public static StatModifier DoubleDamageStatMod;
        public Color flatColorOverride = new Color(0.5f, 0f, 0f, 0.75f);
        #endregion

        #region TimedStatModifier
        public void DoTimedStatModifier(PlayerStats.StatType statToBoost, float amount, float time, StatModifier.ModifyMethod modifyMethod  = StatModifier.ModifyMethod.MULTIPLICATIVE)
        {
            m_attachedPlayer.StartCoroutine(HandleTimedStatModifier(statToBoost, amount, time, modifyMethod));
        }
        private IEnumerator HandleTimedStatModifier(PlayerStats.StatType statToBoost, float amount, float dur, StatModifier.ModifyMethod method)
        {
            StatModifier timedMod = new StatModifier()
            {
                amount = amount,
                statToBoost = statToBoost,
                modifyType = method,
            };
            m_attachedPlayer.ownerlessStatModifiers.Add(timedMod);
            m_attachedPlayer.stats.RecalculateStats(m_attachedPlayer);
            yield return new WaitForSeconds(dur);
            m_attachedPlayer.ownerlessStatModifiers.Remove(timedMod);
            m_attachedPlayer.stats.RecalculateStats(m_attachedPlayer);
            yield break;
        }
        #endregion

        private PlayerController m_attachedPlayer;
        private bool isSecondaryPlayer;
        private int armourLastChecked;
        private int hpStatLastChecked;
        private int itemCountLastChecked;
    }
}
