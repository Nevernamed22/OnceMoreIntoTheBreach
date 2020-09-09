using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using ItemAPI;
using Dungeonator;
using System.Collections;

namespace NevernamedsItems
{
    public class AdvancedTransformGunSynergyProcessor : MonoBehaviour
    {
        public AdvancedTransformGunSynergyProcessor()
        {
            this.NonSynergyGunId = -1;
            this.SynergyGunId = -1;
        }

        private void Awake()
        {
            this.m_gun = base.GetComponent<Gun>();
        }

        private void Update()
        {
            if (Dungeon.IsGenerating || Dungeon.ShouldAttemptToLoadFromMidgameSave)
            {
                return;
            }
            if (this.m_gun && this.m_gun.CurrentOwner is PlayerController)
            {
                PlayerController playerController = this.m_gun.CurrentOwner as PlayerController;
                if (!this.m_gun.enabled)
                {
                    return;
                }
                if (playerController.PlayerHasActiveSynergy(this.SynergyToCheck) && !this.m_transformed)
                {
                    this.m_transformed = true;
                    this.m_gun.TransformToTargetGun(PickupObjectDatabase.GetById(this.SynergyGunId) as Gun);
                    if (this.ShouldResetAmmoAfterTransformation)
                    {
                        this.m_gun.ammo = this.ResetAmmoCount;
                    }
                }
                else if (!playerController.PlayerHasActiveSynergy(this.SynergyToCheck) && this.m_transformed)
                {
                    this.m_transformed = false;
                    this.m_gun.TransformToTargetGun(PickupObjectDatabase.GetById(this.NonSynergyGunId) as Gun);
                    if (this.ShouldResetAmmoAfterTransformation)
                    {
                        this.m_gun.ammo = this.ResetAmmoCount;
                    }
                }
            }
            else if (this.m_gun && !this.m_gun.CurrentOwner && this.m_transformed)
            {
                this.m_transformed = false;
                this.m_gun.TransformToTargetGun(PickupObjectDatabase.GetById(this.NonSynergyGunId) as Gun);
                if (this.ShouldResetAmmoAfterTransformation)
                {
                    this.m_gun.ammo = this.ResetAmmoCount;
                }
            }
            this.ShouldResetAmmoAfterTransformation = false;
        }

        public string SynergyToCheck;
        public int NonSynergyGunId;
        public int SynergyGunId;
        private Gun m_gun;
        private bool m_transformed;
        public bool ShouldResetAmmoAfterTransformation;
        public int ResetAmmoCount;
    }
    public class PermaCharmBulletBehaviour : MonoBehaviour
    {
        public PermaCharmBulletBehaviour()
        {
            this.tintColour = ExtendedColours.pink;
            this.useSpecialTint = true;
            this.procChance = 1;
        }
        private void Start()
        {
            this.m_projectile = base.GetComponent<Projectile>();
            if (useSpecialTint)
            {
                m_projectile.AdjustPlayerProjectileTint(tintColour, 2);
            }
            m_projectile.OnHitEnemy += this.OnHitEnemy;
        }
        private void OnHitEnemy(Projectile bullet, SpeculativeRigidbody enemy, bool fatal)
        {
            if (UnityEngine.Random.value <= procChance)
            {
                enemy.aiActor.ApplyEffect(GameManager.Instance.Dungeon.sharedSettingsPrefab.DefaultPermanentCharmEffect, 1f, null);
            }
        }
        private Projectile m_projectile;
        public Color tintColour;
        public bool useSpecialTint;
        public int procChance;
    }
    public class SimpleFreezingBulletBehaviour : MonoBehaviour
    {
        public SimpleFreezingBulletBehaviour()
        {
            this.tintColour = ExtendedColours.skyblue;
            this.useSpecialTint = true;
            this.procChance = 1;
        }
        private void Start()
        {
            this.m_projectile = base.GetComponent<Projectile>();
            if (useSpecialTint)
            {
                m_projectile.AdjustPlayerProjectileTint(tintColour, 2);
            }
            m_projectile.OnHitEnemy += this.OnHitEnemy;
        }
        private void OnHitEnemy(Projectile bullet, SpeculativeRigidbody enemy, bool fatal)
        {
            if (UnityEngine.Random.value <= procChance)
            {
                BulletStatusEffectItem component = PickupObjectDatabase.GetById(278).GetComponent<BulletStatusEffectItem>();
                GameActorFreezeEffect freezeModifierEffect = component.FreezeModifierEffect;
                if (enemy.healthHaver.IsBoss) EasyApplyDirectFreeze.ApplyDirectFreeze(enemy.gameActor, freezeModifierEffect.duration, freezeAmountForBosses, freezeModifierEffect.UnfreezeDamagePercent, freezeModifierEffect.TintColor, freezeModifierEffect.DeathTintColor, EffectResistanceType.Freeze, "NNs Freeze", true, true);
                else EasyApplyDirectFreeze.ApplyDirectFreeze(enemy.gameActor, freezeModifierEffect.duration, freezeAmount, freezeModifierEffect.UnfreezeDamagePercent, freezeModifierEffect.TintColor, freezeModifierEffect.DeathTintColor, EffectResistanceType.Freeze, "NNs Freeze", true, true);
            }
        }
        private Projectile m_projectile;
        public Color tintColour;
        public bool useSpecialTint;
        public int procChance;
        public int freezeAmount;
        public int freezeAmountForBosses;
    }
    public class EasyApplyDirectFreeze
    {
        public static void ApplyDirectFreeze(GameActor target, float duration, float freezeAmount, float damageToDealOnUnfreeze, Color tintColour, Color deathTintColour, EffectResistanceType resistanceType, string identifier, bool tintsEnemy, bool tintsCorpse)
        {
            try
            {
                //ETGModConsole.Log("Attempted to apply direct freeze");
                GameActorFreezeEffect freezeModifierEffect = PickupObjectDatabase.GetById(278).GetComponent<BulletStatusEffectItem>().FreezeModifierEffect;
                GameActorFreezeEffect freezeToApply = new GameActorFreezeEffect
                {
                    duration = duration,
                    TintColor = tintColour,
                    DeathTintColor = deathTintColour,
                    effectIdentifier = identifier,
                    AppliesTint = tintsEnemy,
                    AppliesDeathTint = tintsCorpse,
                    resistanceType = resistanceType,
                    FreezeAmount = freezeAmount,
                    UnfreezeDamagePercent = damageToDealOnUnfreeze,
                    crystalNum = freezeModifierEffect.crystalNum,
                    crystalRot = freezeModifierEffect.crystalRot,
                    crystalVariation = freezeModifierEffect.crystalVariation,
                    FreezeCrystals = freezeModifierEffect.FreezeCrystals,
                    debrisAngleVariance = freezeModifierEffect.debrisAngleVariance,
                    debrisMaxForce = freezeModifierEffect.debrisMaxForce,
                    debrisMinForce = freezeModifierEffect.debrisMinForce,
                    OverheadVFX = freezeModifierEffect.OverheadVFX,
                    vfxExplosion = freezeModifierEffect.vfxExplosion,
                    stackMode = freezeModifierEffect.stackMode,
                    maxStackedDuration = freezeModifierEffect.maxStackedDuration,
                    AffectsEnemies = true,
                    AffectsPlayers = false,
                    AppliesOutlineTint = false,
                    OutlineTintColor = tintColour,
                    PlaysVFXOnActor = true,
                };
                target.ApplyEffect(freezeToApply, 1f, null);

            }
            catch (Exception e)
            {
                ETGModConsole.Log(e.Message);
                ETGModConsole.Log(e.StackTrace);
            }
        }
    }
    public class ExtremelySimpleFireBulletBehaviour : MonoBehaviour
    {
        public ExtremelySimpleFireBulletBehaviour()
        {
            this.tintColour = Color.red;
            this.useSpecialTint = true;
            this.procChance = 1;
        }
        GameActorFireEffect fireEffect = Gungeon.Game.Items["hot_lead"].GetComponent<BulletStatusEffectItem>().FireModifierEffect;
        private void Start()
        {
            this.m_projectile = base.GetComponent<Projectile>();
            if (useSpecialTint)
            {
                m_projectile.AdjustPlayerProjectileTint(tintColour, 2);
            }
            m_projectile.OnHitEnemy += this.OnHitEnemy;
        }
        private void OnHitEnemy(Projectile bullet, SpeculativeRigidbody enemy, bool fatal)
        {
            if (UnityEngine.Random.value <= procChance)
            {
                enemy.gameActor.ApplyEffect(this.fireEffect, 1f, null);
            }
        }
        private Projectile m_projectile;
        public Color tintColour;
        public bool useSpecialTint;
        public int procChance;
    }
    public class ExtremelySimplePoisonBulletBehaviour : MonoBehaviour
    {
        public ExtremelySimplePoisonBulletBehaviour()
        {
            this.tintColour = Color.green;
            this.useSpecialTint = true;
            this.procChance = 1;
        }
        private void Start()
        {
            this.m_projectile = base.GetComponent<Projectile>();
            if (useSpecialTint)
            {
                m_projectile.AdjustPlayerProjectileTint(tintColour, 2);
            }
            m_projectile.OnHitEnemy += this.OnHitEnemy;
        }
        private void OnHitEnemy(Projectile bullet, SpeculativeRigidbody enemy, bool fatal)
        {
            if (enemy && enemy.gameActor && enemy.healthHaver)
            {
                if (UnityEngine.Random.value <= procChance)
                {
                    GameActorHealthEffect irradiatedLeadEffect = Gungeon.Game.Items["irradiated_lead"].GetComponent<BulletStatusEffectItem>().HealthModifierEffect;
                    GameActorHealthEffect poisonToApply = new GameActorHealthEffect
                    {
                        duration = irradiatedLeadEffect.duration,
                        DamagePerSecondToEnemies = irradiatedLeadEffect.DamagePerSecondToEnemies,
                        TintColor = tintColour,
                        DeathTintColor = tintColour,
                        effectIdentifier = irradiatedLeadEffect.effectIdentifier,
                        AppliesTint = true,
                        AppliesDeathTint = true,
                        resistanceType = EffectResistanceType.Poison,

                        //Eh
                        OverheadVFX = irradiatedLeadEffect.OverheadVFX,
                        AffectsEnemies = true,
                        AffectsPlayers = false,
                        AppliesOutlineTint = false,
                        ignitesGoops = false,
                        OutlineTintColor = tintColour,
                        PlaysVFXOnActor = false,
                    };
                    if (duration > 0) poisonToApply.duration = duration;
                    enemy.gameActor.ApplyEffect(poisonToApply, 1f, null);
                    //ETGModConsole.Log("Applied poison");
                }
            }
            else
            {
                ETGModConsole.Log("Target could not be poisoned");
            }
        }

        private Projectile m_projectile;
        public Color tintColour;
        public bool useSpecialTint;
        public int procChance;
        public int duration;
    }
    public static class ApplyOffsetStuff
    {
        public static void ApplyOffset(this tk2dSpriteDefinition def, Vector2 offset)
        {
            float xOffset = offset.x;
            float yOffset = offset.y;
            def.position0 += new Vector3(xOffset, yOffset, 0);
            def.position1 += new Vector3(xOffset, yOffset, 0);
            def.position2 += new Vector3(xOffset, yOffset, 0);
            def.position3 += new Vector3(xOffset, yOffset, 0);
            def.boundsDataCenter += new Vector3(xOffset, yOffset, 0);
            def.boundsDataExtents += new Vector3(xOffset, yOffset, 0);
            def.untrimmedBoundsDataCenter += new Vector3(xOffset, yOffset, 0);
            def.untrimmedBoundsDataExtents += new Vector3(xOffset, yOffset, 0);
        }
    }
    public class EasyApplyDirectPoison
    {
        public static void ApplyDirectPoison(GameActor target, float duration, float dps, Color tintColour, Color deathTintColour, EffectResistanceType resistanceType, string identifier, bool tintsEnemy, bool tintsCorpse)
        {
            GameActorHealthEffect irradiatedLeadEffect = Gungeon.Game.Items["irradiated_lead"].GetComponent<BulletStatusEffectItem>().HealthModifierEffect;
            GameActorHealthEffect poisonToApply = new GameActorHealthEffect
            {
                duration = duration,
                DamagePerSecondToEnemies = dps,
                TintColor = tintColour,
                DeathTintColor = deathTintColour,
                effectIdentifier = identifier,
                AppliesTint = tintsEnemy,
                AppliesDeathTint = tintsCorpse,
                resistanceType = resistanceType,

                //Eh
                OverheadVFX = irradiatedLeadEffect.OverheadVFX,
                AffectsEnemies = true,
                AffectsPlayers = false,
                AppliesOutlineTint = false,
                ignitesGoops = false,
                OutlineTintColor = tintColour,
                PlaysVFXOnActor = false,
            };
            if (target && target.aiActor && target.healthHaver && target.healthHaver.IsAlive)
            {
                target.ApplyEffect(poisonToApply, 1f, null);
            }
        }
    }
    public class EasyApplyDirectSlow
    {
        public static void ApplyDirectSlow(GameActor target, float duration, float speedMultiplier, Color tintColour, Color deathTintColour, EffectResistanceType resistanceType, string identifier, bool tintsEnemy, bool tintsCorpse)
        {
            Gun gun = ETGMod.Databases.Items["triple_crossbow"] as Gun;
            GameActorSpeedEffect gameActorSpeedEffect = gun.DefaultModule.projectiles[0].speedEffect;
            GameActorSpeedEffect speedToApply = new GameActorSpeedEffect
            {
                duration = duration,
                TintColor = tintColour,
                DeathTintColor = deathTintColour,
                effectIdentifier = identifier,
                AppliesTint = tintsEnemy,
                AppliesDeathTint = tintsCorpse,
                resistanceType = resistanceType,
                SpeedMultiplier = speedMultiplier,

                //Eh
                OverheadVFX = gameActorSpeedEffect.OverheadVFX,
                AffectsEnemies = true,
                AffectsPlayers = false,
                AppliesOutlineTint = false,
                OutlineTintColor = tintColour,
                PlaysVFXOnActor = false,
            };
            if (target && target.aiActor && target.healthHaver && target.healthHaver.IsAlive)
            {
                target.ApplyEffect(speedToApply, 1f, null);
            }
        }
    }
    public class BulletStunModifier : MonoBehaviour
    {
        public BulletStunModifier()
        {
            chanceToStun = 0f;
            stunLength = 1f;
            doVFX = true;
        }
        private void Start()
        {
            this.m_projectile = base.GetComponent<Projectile>();
            if (UnityEngine.Random.value <= chanceToStun)
            {
                this.m_projectile.OnHitEnemy += this.ApplyStun;
            }
        }
        private void ApplyStun(Projectile bullet, SpeculativeRigidbody enemy, bool fatal)
        {
            enemy.behaviorSpeculator.Stun(stunLength, doVFX);
        }
        private Projectile m_projectile;
        public float chanceToStun;
        public bool doVFX;
        public float stunLength;
    }
    public class AdjustGunPosition
    {
        public static List<tk2dSpriteAnimationClip> GetGunAnimationClips(Gun gun)
        {
            List<tk2dSpriteAnimationClip> clips = new List<tk2dSpriteAnimationClip>();
            if (!string.IsNullOrEmpty(gun.shootAnimation) && gun.GetComponent<tk2dSpriteAnimator>().GetClipByName(gun.shootAnimation) != null)
            {
                clips.Add(gun.GetComponent<tk2dSpriteAnimator>().GetClipByName(gun.shootAnimation));
            }
            if (!string.IsNullOrEmpty(gun.reloadAnimation) && gun.GetComponent<tk2dSpriteAnimator>().GetClipByName(gun.reloadAnimation) != null)
            {
                clips.Add(gun.GetComponent<tk2dSpriteAnimator>().GetClipByName(gun.reloadAnimation));
            }
            if (!string.IsNullOrEmpty(gun.emptyReloadAnimation) && gun.GetComponent<tk2dSpriteAnimator>().GetClipByName(gun.emptyReloadAnimation) != null)
            {
                clips.Add(gun.GetComponent<tk2dSpriteAnimator>().GetClipByName(gun.emptyReloadAnimation));
            }
            if (!string.IsNullOrEmpty(gun.idleAnimation) && gun.GetComponent<tk2dSpriteAnimator>().GetClipByName(gun.idleAnimation) != null)
            {
                clips.Add(gun.GetComponent<tk2dSpriteAnimator>().GetClipByName(gun.idleAnimation));
            }
            if (!string.IsNullOrEmpty(gun.chargeAnimation) && gun.GetComponent<tk2dSpriteAnimator>().GetClipByName(gun.chargeAnimation) != null)
            {
                clips.Add(gun.GetComponent<tk2dSpriteAnimator>().GetClipByName(gun.chargeAnimation));
            }
            if (!string.IsNullOrEmpty(gun.dischargeAnimation) && gun.GetComponent<tk2dSpriteAnimator>().GetClipByName(gun.dischargeAnimation) != null)
            {
                clips.Add(gun.GetComponent<tk2dSpriteAnimator>().GetClipByName(gun.dischargeAnimation));
            }
            if (!string.IsNullOrEmpty(gun.emptyAnimation) && gun.GetComponent<tk2dSpriteAnimator>().GetClipByName(gun.emptyAnimation) != null)
            {
                clips.Add(gun.GetComponent<tk2dSpriteAnimator>().GetClipByName(gun.emptyAnimation));
            }
            if (!string.IsNullOrEmpty(gun.introAnimation) && gun.GetComponent<tk2dSpriteAnimator>().GetClipByName(gun.introAnimation) != null)
            {
                clips.Add(gun.GetComponent<tk2dSpriteAnimator>().GetClipByName(gun.introAnimation));
            }
            if (!string.IsNullOrEmpty(gun.finalShootAnimation) && gun.GetComponent<tk2dSpriteAnimator>().GetClipByName(gun.finalShootAnimation) != null)
            {
                clips.Add(gun.GetComponent<tk2dSpriteAnimator>().GetClipByName(gun.finalShootAnimation));
            }
            if (!string.IsNullOrEmpty(gun.enemyPreFireAnimation) && gun.GetComponent<tk2dSpriteAnimator>().GetClipByName(gun.enemyPreFireAnimation) != null)
            {
                clips.Add(gun.GetComponent<tk2dSpriteAnimator>().GetClipByName(gun.enemyPreFireAnimation));
            }
            if (!string.IsNullOrEmpty(gun.outOfAmmoAnimation) && gun.GetComponent<tk2dSpriteAnimator>().GetClipByName(gun.outOfAmmoAnimation) != null)
            {
                clips.Add(gun.GetComponent<tk2dSpriteAnimator>().GetClipByName(gun.outOfAmmoAnimation));
            }
            if (!string.IsNullOrEmpty(gun.criticalFireAnimation) && gun.GetComponent<tk2dSpriteAnimator>().GetClipByName(gun.criticalFireAnimation) != null)
            {
                clips.Add(gun.GetComponent<tk2dSpriteAnimator>().GetClipByName(gun.criticalFireAnimation));
            }
            if (!string.IsNullOrEmpty(gun.dodgeAnimation) && gun.GetComponent<tk2dSpriteAnimator>().GetClipByName(gun.dodgeAnimation) != null)
            {
                clips.Add(gun.GetComponent<tk2dSpriteAnimator>().GetClipByName(gun.dodgeAnimation));
            }
            return clips;
        }
    }
}
