using MonoMod.RuntimeDetour;
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
                if (GameManager.Instance.SecondaryPlayer != null && GameManager.Instance.SecondaryPlayer == m_attachedPlayer) { isSecondaryPlayer = true; }
            }
            DoubleDamageStatMod = new StatModifier();
            DoubleDamageStatMod.statToBoost = PlayerStats.StatType.Damage;
            DoubleDamageStatMod.amount = 2f;
            DoubleDamageStatMod.modifyType = StatModifier.ModifyMethod.MULTIPLICATIVE;
        }
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

        private PlayerController m_attachedPlayer;
        private bool isSecondaryPlayer;
    }
}
