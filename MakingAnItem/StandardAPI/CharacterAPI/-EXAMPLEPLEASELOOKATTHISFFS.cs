using NevernamedsItems;//oooo scary its throwing an error, just change it to your mods name space
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace CustomCharacters
{
	/*
    class example : ETGModule
    {
        public override void Exit()
        {

        }
        public override void Init()
        {

        }
        public override void Start()
        {

			CharApi.Init("Bot");


			var name = Loader.BuildCharacter("BotsMod/Characters/Lost", CustomPlayableCharacters.Lost, new Vector3(15.8f, 26.6f, 27.1f), true, new Vector3(15.3f, 24.8f, 25.3f), true, false, false, true, true, new GlowMatDoer(new Color32(255, 0, 38, 255), 4.55f, 55),
                new GlowMatDoer(new Color32(255, 69, 248, 255), 1.55f, 55), 2, true, "botfs_lost").nameInternal;

            Loader.SetupCustomAnimation(name, "dance", 12, tk2dSpriteAnimationClip.WrapMode.Loop);

            Loader.SetupCustomBreachAnimation(name, "float", 12, tk2dSpriteAnimationClip.WrapMode.Once);
            Loader.SetupCustomBreachAnimation(name, "float_hold", 5, tk2dSpriteAnimationClip.WrapMode.Loop);
            Loader.SetupCustomBreachAnimation(name, "float_out", 14, tk2dSpriteAnimationClip.WrapMode.Once);
            Loader.SetupCustomBreachAnimation(name, "select_idle", 12, tk2dSpriteAnimationClip.WrapMode.LoopFidget, 1, 4);

            Loader.AddPhase(name, new CharacterSelectIdlePhase
            {
                endVFXSpriteAnimator = null,
                vfxSpriteAnimator = null,
                holdAnimation = "float_hold",
                inAnimation = "float",
                outAnimation = "float_out",
                optionalHoldIdleAnimation = "",
                holdMax = 10,
                holdMin = 5,
                optionalHoldChance = 0,
                vfxHoldPeriod = 0,
                vfxTrigger = CharacterSelectIdlePhase.VFXPhaseTrigger.NONE,
            });

            Loader.AddCoopBlankOverride(name, LostOverrideBlank);
        }
		//the value returned here is how long of a cool down the "blank" has default is 5 i think 
		public static float LostOverrideBlank(PlayerController self)
		{
			if (ghostProj == null)
			{
				ghostProj = UnityEngine.Object.Instantiate<Projectile>((PickupObjectDatabase.GetById(15) as Gun).DefaultModule.projectiles[0]);
				ghostProj.gameObject.SetActive(false);

				ghostProj.baseData.speed = 50;
				ghostProj.baseData.UsesCustomAccelerationCurve = true;
				ghostProj.baseData.AccelerationCurve = new AnimationCurve
				{
					postWrapMode = WrapMode.Clamp,
					preWrapMode = WrapMode.Clamp,
					keys = new Keyframe[]
					{
					new Keyframe
					{
						time = 0f,
						value = 0f,
						inTangent = 0f,
						outTangent = 0f
					},
					new Keyframe
					{
						time = 1f,
						value = 1f,
						inTangent = 2f,
						outTangent = 2f
					},
					}
				};
				ghostProj.baseData.CustomAccelerationCurveDuration = 0.3f;
				ghostProj.baseData.IgnoreAccelCurveTime = 0f;
				ghostProj.shouldRotate = true;
				ghostProj.SetProjectileSpriteRight("lost_ghost_blank_proj", 9, 7, true, tk2dBaseSprite.Anchor.LowerLeft);
			}
			for (int i = 0; i < 8; i++)
			{
				//BotsModule.Log(i + ": proj hopefully spawned");
				GameObject gameObject = SpawnManager.SpawnProjectile(ghostProj.gameObject, self.specRigidbody.UnitCenter, Quaternion.Euler(0f, 0f, i * 45), true);
				gameObject.SetActive(true);
				Projectile component = gameObject.GetComponent<Projectile>();
				component.Owner = self;
				component.Shooter = self.specRigidbody;
			}
			return 5f;

		}
		static Projectile ghostProj;
	}*/
}
