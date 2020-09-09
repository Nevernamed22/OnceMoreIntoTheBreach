﻿using ItemAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace NevernamedsItems
{
    public class LockdownStatusEffect
    {
        public static List<string> LockdownPaths = new List<string>()
        {
            "NevernamedsItems/Resources/lockdown_effect_icon",
        };
        public static GameObject lockdownVFXObject;
        public static void Initialise()
        {
            lockdownVFXObject = SpriteBuilder.SpriteFromResource("NevernamedsItems/Resources/lockdown_effect_icon", new GameObject("LockdownIcon"));
            lockdownVFXObject.SetActive(false);
            FakePrefab.MarkAsFakePrefab(lockdownVFXObject);
            UnityEngine.Object.DontDestroyOnLoad(lockdownVFXObject);

            tk2dSpriteAnimator animator = lockdownVFXObject.AddComponent<tk2dSpriteAnimator>();
            animator.Library = lockdownVFXObject.AddComponent<tk2dSpriteAnimation>();
            animator.Library.clips = new tk2dSpriteAnimationClip[0];

            tk2dSpriteAnimationClip clip = new tk2dSpriteAnimationClip { name = "LockdownIconClip", fps = 5, frames = new tk2dSpriteAnimationFrame[0] };
            foreach (string path in LockdownPaths)
            {
                int spriteId = SpriteBuilder.AddSpriteToCollection(path, lockdownVFXObject.GetComponent<tk2dBaseSprite>().Collection);
                tk2dSpriteAnimationFrame frame = new tk2dSpriteAnimationFrame { spriteId = spriteId, spriteCollection = lockdownVFXObject.GetComponent<tk2dBaseSprite>().Collection };
                clip.frames = clip.frames.Concat(new tk2dSpriteAnimationFrame[] { frame }).ToArray();
            }
            animator.Library.clips = animator.Library.clips.Concat(new tk2dSpriteAnimationClip[] { clip }).ToArray();
            animator.playAutomatically = true;
            animator.DefaultClipId = animator.GetClipIdByName("LockdownIconClip");
        }

    }
    public class ApplyLockdown
    {
        public static void ApplyDirectLockdown(GameActor target, float duration, Color tintColour, Color deathTintColour, EffectResistanceType resistanceType, string identifier, bool tintsEnemy, bool tintsCorpse)
        {
            GameActorSpeedEffect lockdownToApply = new GameActorSpeedEffect
            {
                duration = duration,
                TintColor = tintColour,
                DeathTintColor = deathTintColour,
                effectIdentifier = identifier,
                AppliesTint = tintsEnemy,
                AppliesDeathTint = tintsCorpse,
                resistanceType = resistanceType,
                SpeedMultiplier = 0f,

                //Eh
                OverheadVFX = LockdownStatusEffect.lockdownVFXObject,
                AffectsEnemies = true,
                AffectsPlayers = false,
                AppliesOutlineTint = false,
                OutlineTintColor = tintColour,
                PlaysVFXOnActor = false,
            };
            if (target && target.aiActor && target.healthHaver && target.healthHaver.IsAlive)
            {
                target.ApplyEffect(lockdownToApply, 1f, null);
            }
        }
    }
}
