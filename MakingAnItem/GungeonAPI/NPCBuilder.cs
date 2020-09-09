using System;
using System.Collections.Generic;
using System.Linq;
using ItemAPI;
using UnityEngine;

namespace GungeonAPI
{
	// Token: 0x02000009 RID: 9
	public static class NPCBuilder
	{
		// Token: 0x06000052 RID: 82 RVA: 0x00004A28 File Offset: 0x00002C28
		public static tk2dSpriteAnimationClip AddAnimation(this GameObject obj, string name, string spriteDirectory, int fps, NPCBuilder.AnimationType type, DirectionalAnimation.DirectionType directionType = DirectionalAnimation.DirectionType.None, DirectionalAnimation.FlipType flipType = DirectionalAnimation.FlipType.None)
		{
			obj.AddComponent<tk2dSpriteAnimator>();
			AIAnimator aianimator = obj.GetComponent<AIAnimator>();
			bool flag = !aianimator;
			if (flag)
			{
				aianimator = NPCBuilder.CreateNewAIAnimator(obj);
			}
			DirectionalAnimation directionalAnimation = aianimator.GetDirectionalAnimation(name, directionType, type);
			bool flag2 = directionalAnimation == null;
			if (flag2)
			{
				directionalAnimation = new DirectionalAnimation
				{
					AnimNames = new string[0],
					Flipped = new DirectionalAnimation.FlipType[0],
					Type = directionType,
					Prefix = string.Empty
				};
			}
			directionalAnimation.AnimNames = directionalAnimation.AnimNames.Concat(new string[]
			{
				name
			}).ToArray<string>();
			directionalAnimation.Flipped = directionalAnimation.Flipped.Concat(new DirectionalAnimation.FlipType[]
			{
				flipType
			}).ToArray<DirectionalAnimation.FlipType>();
			aianimator.AssignDirectionalAnimation(name, directionalAnimation, type);
			return NPCBuilder.BuildAnimation(aianimator, name, spriteDirectory, fps);
		}

		// Token: 0x06000053 RID: 83 RVA: 0x00004B04 File Offset: 0x00002D04
		private static AIAnimator CreateNewAIAnimator(GameObject obj)
		{
			AIAnimator aianimator = obj.AddComponent<AIAnimator>();
			aianimator.FlightAnimation = NPCBuilder.CreateNewDirectionalAnimation();
			aianimator.HitAnimation = NPCBuilder.CreateNewDirectionalAnimation();
			aianimator.IdleAnimation = NPCBuilder.CreateNewDirectionalAnimation();
			aianimator.TalkAnimation = NPCBuilder.CreateNewDirectionalAnimation();
			aianimator.MoveAnimation = NPCBuilder.CreateNewDirectionalAnimation();
			aianimator.OtherAnimations = new List<AIAnimator.NamedDirectionalAnimation>();
			aianimator.IdleFidgetAnimations = new List<DirectionalAnimation>();
			aianimator.OtherVFX = new List<AIAnimator.NamedVFXPool>();
			return aianimator;
		}

		// Token: 0x06000054 RID: 84 RVA: 0x00004B78 File Offset: 0x00002D78
		private static DirectionalAnimation CreateNewDirectionalAnimation()
		{
			return new DirectionalAnimation
			{
				AnimNames = new string[0],
				Flipped = new DirectionalAnimation.FlipType[0],
				Type = DirectionalAnimation.DirectionType.None
			};
		}

		// Token: 0x06000055 RID: 85 RVA: 0x00004BB0 File Offset: 0x00002DB0
		public static tk2dSpriteAnimationClip BuildAnimation(AIAnimator aiAnimator, string name, string spriteDirectory, int fps)
		{
			tk2dSpriteCollectionData tk2dSpriteCollectionData = aiAnimator.GetComponent<tk2dSpriteCollectionData>();
			bool flag = !tk2dSpriteCollectionData;
			if (flag)
			{
				tk2dSpriteCollectionData = SpriteBuilder.ConstructCollection(aiAnimator.gameObject, aiAnimator.name + "_collection");
			}
			string[] resourceNames = ResourceExtractor.GetResourceNames();
			List<int> list = new List<int>();
			for (int i = 0; i < resourceNames.Length; i++)
			{
				bool flag2 = resourceNames[i].StartsWith(spriteDirectory.Replace('/', '.'), StringComparison.OrdinalIgnoreCase);
				if (flag2)
				{
					list.Add(SpriteBuilder.AddSpriteToCollection(resourceNames[i], tk2dSpriteCollectionData));
				}
			}
			bool flag3 = list.Count == 0;
			if (flag3)
			{
				Tools.PrintError<string>("No sprites found for animation " + name, "FF0000");
			}
			tk2dSpriteAnimationClip tk2dSpriteAnimationClip = SpriteBuilder.AddAnimation(aiAnimator.spriteAnimator, tk2dSpriteCollectionData, list, name, tk2dSpriteAnimationClip.WrapMode.Loop);
			tk2dSpriteAnimationClip.fps = (float)fps;
			return tk2dSpriteAnimationClip;
		}

		// Token: 0x06000056 RID: 86 RVA: 0x00004C8C File Offset: 0x00002E8C
		public static DirectionalAnimation GetDirectionalAnimation(this AIAnimator aiAnimator, string name, DirectionalAnimation.DirectionType directionType, NPCBuilder.AnimationType type)
		{
			DirectionalAnimation directionalAnimation = null;
			switch (type)
			{
				case NPCBuilder.AnimationType.Move:
					directionalAnimation = aiAnimator.MoveAnimation;
					break;
				case NPCBuilder.AnimationType.Idle:
					directionalAnimation = aiAnimator.IdleAnimation;
					break;
				case NPCBuilder.AnimationType.Flight:
					directionalAnimation = aiAnimator.FlightAnimation;
					break;
				case NPCBuilder.AnimationType.Hit:
					directionalAnimation = aiAnimator.HitAnimation;
					break;
				case NPCBuilder.AnimationType.Talk:
					directionalAnimation = aiAnimator.TalkAnimation;
					break;
			}
			bool flag = directionalAnimation != null;
			DirectionalAnimation result;
			if (flag)
			{
				result = directionalAnimation;
			}
			else
			{
				result = null;
			}
			return result;
		}

		// Token: 0x06000057 RID: 87 RVA: 0x00004D04 File Offset: 0x00002F04
		public static void AssignDirectionalAnimation(this AIAnimator aiAnimator, string name, DirectionalAnimation animation, NPCBuilder.AnimationType type)
		{
			switch (type)
			{
				case NPCBuilder.AnimationType.Move:
					aiAnimator.MoveAnimation = animation;
					break;
				case NPCBuilder.AnimationType.Idle:
					aiAnimator.IdleAnimation = animation;
					break;
				case NPCBuilder.AnimationType.Fidget:
					aiAnimator.IdleFidgetAnimations.Add(animation);
					break;
				case NPCBuilder.AnimationType.Flight:
					aiAnimator.FlightAnimation = animation;
					break;
				case NPCBuilder.AnimationType.Hit:
					aiAnimator.HitAnimation = animation;
					break;
				case NPCBuilder.AnimationType.Talk:
					aiAnimator.TalkAnimation = animation;
					break;
				default:
					aiAnimator.OtherAnimations.Add(new AIAnimator.NamedDirectionalAnimation
					{
						anim = animation,
						name = name
					});
					break;
			}
		}

		// Token: 0x02000020 RID: 32
		public enum AnimationType
		{
			// Token: 0x04000091 RID: 145
			Move,
			// Token: 0x04000092 RID: 146
			Idle,
			// Token: 0x04000093 RID: 147
			Fidget,
			// Token: 0x04000094 RID: 148
			Flight,
			// Token: 0x04000095 RID: 149
			Hit,
			// Token: 0x04000096 RID: 150
			Talk,
			// Token: 0x04000097 RID: 151
			Other
		}
	}
}
