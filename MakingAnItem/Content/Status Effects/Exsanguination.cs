using Alexandria.ItemAPI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace NevernamedsItems
{
    public class ExsanguinationSetup
    {
        public static void Init()
        {
            effectStackVFX = new Dictionary<int, GameObject>()
            {
                {1, VFXToolbox.CreateOverheadVFX(new List<string>()
                {
                    "NevernamedsItems/Resources/MiscVFX/StatusEffectVFX/Exsanguination/exsanguination_stackone_001.png",
                    "NevernamedsItems/Resources/MiscVFX/StatusEffectVFX/Exsanguination/exsanguination_stackone_002.png",
                }, "ExsanguinationStack1", 10)},
                {2, VFXToolbox.CreateOverheadVFX(new List<string>()
                {
                    "NevernamedsItems/Resources/MiscVFX/StatusEffectVFX/Exsanguination/exsanguination_stacktwo_001.png",
                    "NevernamedsItems/Resources/MiscVFX/StatusEffectVFX/Exsanguination/exsanguination_stacktwo_002.png",
                }, "ExsanguinationStack2", 10)},
                {3, VFXToolbox.CreateOverheadVFX(new List<string>()
                {
                    "NevernamedsItems/Resources/MiscVFX/StatusEffectVFX/Exsanguination/exsanguination_stackthree_001.png",
                    "NevernamedsItems/Resources/MiscVFX/StatusEffectVFX/Exsanguination/exsanguination_stackthree_002.png",
                    "NevernamedsItems/Resources/MiscVFX/StatusEffectVFX/Exsanguination/exsanguination_stackthree_003.png",
                }, "ExsanguinationStack3", 10)},
                {4, VFXToolbox.CreateOverheadVFX(new List<string>()
                {
                    "NevernamedsItems/Resources/MiscVFX/StatusEffectVFX/Exsanguination/exsanguination_stackfour_001.png",
                    "NevernamedsItems/Resources/MiscVFX/StatusEffectVFX/Exsanguination/exsanguination_stackfour_002.png",
                    "NevernamedsItems/Resources/MiscVFX/StatusEffectVFX/Exsanguination/exsanguination_stackfour_003.png",
                    "NevernamedsItems/Resources/MiscVFX/StatusEffectVFX/Exsanguination/exsanguination_stackfour_004.png",
                }, "ExsanguinationStack4", 10)},
                {5, VFXToolbox.CreateOverheadVFX(new List<string>()
                {
                    "NevernamedsItems/Resources/MiscVFX/StatusEffectVFX/Exsanguination/exsanguination_stackfive_001.png",
                    "NevernamedsItems/Resources/MiscVFX/StatusEffectVFX/Exsanguination/exsanguination_stackfive_002.png",
                    "NevernamedsItems/Resources/MiscVFX/StatusEffectVFX/Exsanguination/exsanguination_stackfive_003.png",
                    "NevernamedsItems/Resources/MiscVFX/StatusEffectVFX/Exsanguination/exsanguination_stackfive_004.png",
                    "NevernamedsItems/Resources/MiscVFX/StatusEffectVFX/Exsanguination/exsanguination_stackfive_005.png",
                }, "ExsanguinationStack5", 10)}
            };

            labelStackVFX = VFXToolbox.CreateOverheadVFX(new List<string>()
                {
                    "NevernamedsItems/Resources/MiscVFX/StatusEffectVFX/Exsanguination/exsanguination_stacklabel_001.png",
                    "NevernamedsItems/Resources/MiscVFX/StatusEffectVFX/Exsanguination/exsanguination_stacklabel_002.png",
                }, "ExsanguinationStackLabel", 10);


            BloodParticleDoer = FakePrefab.Clone(PickupObjectDatabase.GetById(449).GetComponent<TeleporterPrototypeItem>().TelefragVFXPrefab.gameObject);
            BloodParticleDoer.GetComponent<ParticleSystem>().emission.SetBurst(0, new ParticleSystem.Burst { count = 1, time = 0, cycleCount = 1, repeatInterval = 0.010f, maxCount = 1, minCount = 1 });

        }
        public static Dictionary<int, GameObject> effectStackVFX;
        public static GameObject labelStackVFX;
        public static GameObject BloodParticleDoer;
    }
    public class GameActorExsanguinationEffect : GameActorEffect
    {
        public GameActorExsanguinationEffect()
        {
            this.DeathTintColor = Color.red;
            this.AppliesTint = false;
            this.AppliesDeathTint = true;
            this.AffectsPlayers = false;
            this.AffectsEnemies = true;
            this.stackMode = EffectStackingMode.DarkSoulsAccumulate;
            this.effectIdentifier = "exsanguination";
            bloodAccum = 0f;
            stackedBleedTimers = new List<float>();
        }
        private int currentStackAmount;
        private GameObject extantOverheadder;
        private float bloodAccum;
        private List<float> stackedBleedTimers;
        public override void OnEffectApplied(GameActor actor, RuntimeGameActorEffectData effectData, float partialAmount = 1)
        {
            currentStackAmount = 1;
            ChangeOverheadVFX(1, actor);
            actor.healthHaver.OnDeath += OnTargetDeath;
            base.OnEffectApplied(actor, effectData, partialAmount);
        }
        private void OnTargetDeath(Vector2 dir)
        {
            if (extantOverheadder != null) { UnityEngine.Object.Destroy(extantOverheadder); }
        }
        public void ChangeOverheadVFX(int stackamount, GameActor target, bool lethal = false)
        {
            if (stackamount > 0)
            {
                if (extantOverheadder != null) { UnityEngine.Object.Destroy(extantOverheadder); }
                GameObject toSpawn = ExsanguinationSetup.labelStackVFX;
                if (stackamount <= 5) { toSpawn = ExsanguinationSetup.effectStackVFX[stackamount]; }

                GameObject spawnedVFX = UnityEngine.Object.Instantiate<GameObject>(toSpawn);
                tk2dBaseSprite spawnedVFXSprite = spawnedVFX.GetComponent<tk2dBaseSprite>();
                spawnedVFX.transform.parent = target.transform;
                if (target.healthHaver.IsBoss) { spawnedVFX.transform.position = target.specRigidbody.HitboxPixelCollider.UnitTopCenter; }
                else
                {
                    Bounds bounds = target.sprite.GetBounds();
                    Vector3 vector = target.transform.position + new Vector3((bounds.max.x + bounds.min.x) / 2f, bounds.max.y, 0f).Quantize(0.0625f);
                    spawnedVFX.transform.position = target.sprite.WorldCenter.ToVector3ZUp(0f).WithY(vector.y);
                }
                spawnedVFXSprite.HeightOffGround = 0.5f;
                target.sprite.AttachRenderer(spawnedVFXSprite);

                Color overrideRed = Color.red;
                overrideRed.a = 0.1f * Mathf.Min(stackamount, 10);
                target.RegisterOverrideColor(overrideRed, "exsanguination");
                extantOverheadder = spawnedVFX;

                if (stackamount > 5)
                {
                    RelativeLabelAttacher label = extantOverheadder.AddComponent<RelativeLabelAttacher>();
                    label.colour = ExtendedColours.carrionRed;
                    label.offset = new Vector3(0, 0.75f, 0);
                    label.labelValue = stackamount.ToString();
                }
            }
            else
            {
                if (extantOverheadder != null) { UnityEngine.Object.Destroy(extantOverheadder); }
                if (!lethal) target.DeregisterOverrideColor("exsanguination");
            }
        }
        public override void EffectTick(GameActor actor, RuntimeGameActorEffectData effectData)
        {
            Vector2 v = actor.transform.position.XY();
            if (actor.specRigidbody && actor.specRigidbody.HitboxPixelCollider != null) { v = actor.specRigidbody.HitboxPixelCollider.UnitTopCenter.Quantize(0.0625f); }
            if (extantOverheadder != null && extantOverheadder.GetComponent<tk2dBaseSprite>() != null)
            {
                extantOverheadder.transform.position = v;
                extantOverheadder.GetComponent<tk2dBaseSprite>().renderer.enabled = !actor.IsGone;
            }

            if (actor.healthHaver.IsDead && extantOverheadder != null) { ChangeOverheadVFX(0, actor, true); }

            bloodAccum += BraveTime.DeltaTime * Mathf.Min((10f * currentStackAmount), 100);
            if (bloodAccum > 1f)
            {
                int num = Mathf.FloorToInt(bloodAccum);
                bloodAccum %= 1f;
                Vector2 minpos = actor.sprite.WorldBottomLeft;
                Vector2 maxpos = actor.sprite.WorldTopRight;
                for (int i = 0; i < num; i++)
                {
                    UnityEngine.Object.Destroy(UnityEngine.Object.Instantiate(ExsanguinationSetup.BloodParticleDoer, new Vector2(UnityEngine.Random.Range(minpos.x, maxpos.x), UnityEngine.Random.Range(minpos.y, maxpos.y)), Quaternion.identity), 5);
                }
            }

            if (this.AffectsEnemies && actor is AIActor && currentStackAmount > 0)
            {
                actor.healthHaver.ApplyDamage((1.75f * currentStackAmount) * BraveTime.DeltaTime, Vector2.zero, this.effectIdentifier, CoreDamageTypes.None, DamageCategory.DamageOverTime, false, null, false);
            }

            if (stackedBleedTimers != null && stackedBleedTimers.Count > 0 && currentStackAmount > 1)
            {
                if (stackedBleedTimers[0] > 0)
                {
                    stackedBleedTimers[0] -= BraveTime.DeltaTime;
                }
                else
                {
                    currentStackAmount--;
                    stackedBleedTimers.RemoveAt(0);
                    ChangeOverheadVFX(currentStackAmount, actor);
                }
            }

            base.EffectTick(actor, effectData);
        }
        public override void OnEffectRemoved(GameActor actor, RuntimeGameActorEffectData effectData)
        {
            currentStackAmount = 0;
            ChangeOverheadVFX(0, actor);

            base.OnEffectRemoved(actor, effectData);
        }
        public void AddStackLayer(GameActor actor, float dur)
        {
            currentStackAmount++;
            ChangeOverheadVFX(currentStackAmount, actor);
            stackedBleedTimers.Add(dur);
        }
        public override void OnDarkSoulsAccumulate(GameActor actor, RuntimeGameActorEffectData effectData, float partialAmount = 1, Projectile sourceProjectile = null)
        {
            for (int i = 0; i < actor.m_activeEffects.Count; i++)
            {
                if (actor.m_activeEffects[i].effectIdentifier == effectIdentifier && actor.m_activeEffects[i] is GameActorExsanguinationEffect)
                {
                    actor.m_activeEffectData[i].elapsed -= duration;
                    (actor.m_activeEffects[i] as GameActorExsanguinationEffect).AddStackLayer(actor, duration);
                }
            }
            base.OnDarkSoulsAccumulate(actor, effectData, partialAmount, sourceProjectile);
        }
    }
}
