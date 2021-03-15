using ItemAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace NevernamedsItems
{
    class PlagueStatusEffectSetup
    {
        public static GameObject PlagueOverheadVFX;

        public static void Init()
        {
            //The VFX overhead for the Plague Effect
            #region VFXSetup
            //Setting up the Overhead Plague VFX
            GameObject plagueVFXObject = SpriteBuilder.SpriteFromResource("NevernamedsItems/Resources/StatusEffectVFX/plaguevfxframe_001", new GameObject("PlagueIcon"));
            plagueVFXObject.SetActive(false);
            tk2dBaseSprite plaguevfxSprite = plagueVFXObject.GetComponent<tk2dBaseSprite>();
            plaguevfxSprite.GetCurrentSpriteDef().ConstructOffsetsFromAnchor(tk2dBaseSprite.Anchor.LowerCenter, plaguevfxSprite.GetCurrentSpriteDef().position3);
            FakePrefab.MarkAsFakePrefab(plagueVFXObject);
            UnityEngine.Object.DontDestroyOnLoad(plagueVFXObject);

            //Animating the overhead
            tk2dSpriteAnimator plagueanimator = plagueVFXObject.AddComponent<tk2dSpriteAnimator>();
            plagueanimator.Library = plagueVFXObject.AddComponent<tk2dSpriteAnimation>();
            plagueanimator.Library.clips = new tk2dSpriteAnimationClip[0];

            tk2dSpriteAnimationClip clip = new tk2dSpriteAnimationClip { name = "PlagueIconClip", fps = 7, frames = new tk2dSpriteAnimationFrame[0] };
            foreach (string path in PlagueVFXPaths)
            {
                int spriteId = SpriteBuilder.AddSpriteToCollection(path, plagueVFXObject.GetComponent<tk2dBaseSprite>().Collection);

                plagueVFXObject.GetComponent<tk2dBaseSprite>().Collection.spriteDefinitions[spriteId].ConstructOffsetsFromAnchor(tk2dBaseSprite.Anchor.LowerCenter);

                tk2dSpriteAnimationFrame frame = new tk2dSpriteAnimationFrame { spriteId = spriteId, spriteCollection = plagueVFXObject.GetComponent<tk2dBaseSprite>().Collection };
                clip.frames = clip.frames.Concat(new tk2dSpriteAnimationFrame[] { frame }).ToArray();
            }
            plagueanimator.Library.clips = plagueanimator.Library.clips.Concat(new tk2dSpriteAnimationClip[] { clip }).ToArray();
            plagueanimator.playAutomatically = true;
            plagueanimator.DefaultClipId = plagueanimator.GetClipIdByName("PlagueIconClip");
            PlagueOverheadVFX = plagueVFXObject;
            #endregion

            //Setup the standard Plague effect
            GameActorPlagueEffect StandPlague = StatusEffectHelper.GeneratePlagueEffect(100, 2, true, ExtendedColours.plaguePurple, true, ExtendedColours.plaguePurple);
            StaticStatusEffects.StandardPlagueEffect = StandPlague;
        }

        public static List<string> PlagueVFXPaths = new List<string>()
        {
            "NevernamedsItems/Resources/StatusEffectVFX/plaguevfxframe_001",
            "NevernamedsItems/Resources/StatusEffectVFX/plaguevfxframe_002",
            "NevernamedsItems/Resources/StatusEffectVFX/plaguevfxframe_003",
            "NevernamedsItems/Resources/StatusEffectVFX/plaguevfxframe_004",
            "NevernamedsItems/Resources/StatusEffectVFX/plaguevfxframe_005",
        };
    }
    public class GameActorPlagueEffect : GameActorHealthEffect
    {
        public GameActorPlagueEffect()
        {
            this.DamagePerSecondToEnemies = 1f;
            this.TintColor = ExtendedColours.plaguePurple;
            this.DeathTintColor = ExtendedColours.plaguePurple;
            this.AppliesTint = true;
            this.AppliesDeathTint = true;
        }
        public override void EffectTick(GameActor actor, RuntimeGameActorEffectData effectData)
        {
            if (EasyGoopDefinitions.PlagueGoop != null)
            {
                DeadlyDeadlyGoopManager goop = DeadlyDeadlyGoopManager.GetGoopManagerForGoopType(EasyGoopDefinitions.PlagueGoop);
                goop.TimedAddGoopCircle(actor.specRigidbody.UnitCenter, 1.5f, 0.75f, true);
            }
            base.EffectTick(actor, effectData);
        }

    }
}


