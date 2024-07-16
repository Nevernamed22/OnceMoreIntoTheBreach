
using SaveAPI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Alexandria.ItemAPI;
using Dungeonator;

namespace NevernamedsItems
{
    class Jarate : SpawnObjectPlayerItem
    {
        public static void Init()
        {
            Jarate item = ItemSetup.NewItem<Jarate>(
           "Jarate",
           "Good Job",
           "Throws a jar of miracle fluids which weakens the Gundead. \n\nGungeoneering can be a long and tedious process. The ancient art of Jarate was derived as an ingenious solution to both combat and excrement reprocessing.",
           "jarate_icon") as Jarate;
          
            ItemBuilder.SetCooldownType(item, ItemBuilder.CooldownType.Damage, 800);
            item.consumable = false;
            item.objectToSpawn = BuildPrefab();
            item.tossForce = 12;
            item.canBounce = false;
            item.IsCigarettes = false;
            item.RequireEnemiesInRoom = false;
            item.SpawnRadialCopies = false;
            item.RadialCopiesToSpawn = 0;
            item.AudioEvent = null;
            item.IsKageBunshinItem = false;
            item.quality = PickupObject.ItemQuality.C;
        }


        public static GameObject BuildPrefab()
        {

            var bomb = SpriteBuilder.SpriteFromResource("NevernamedsItems/Resources/ThrowableActives/Jarate/jarate_toss_001.png", new GameObject("JarateToss"));
            bomb.MakeFakePrefab();

            var animator = bomb.AddComponent<tk2dSpriteAnimator>();
            var collection = (PickupObjectDatabase.GetById(108) as SpawnObjectPlayerItem).objectToSpawn.GetComponent<tk2dSpriteAnimator>().Library.clips[0].frames[0].spriteCollection;

            //DEPLOYMENT
            var deployAnimation = SpriteBuilder.AddAnimation(animator, collection, new List<int>
            {
                SpriteBuilder.AddSpriteToCollection("NevernamedsItems/Resources/ThrowableActives/Jarate/jarate_toss_004.png", collection),
            }, "deploy", tk2dSpriteAnimationClip.WrapMode.Once);
            deployAnimation.fps = 12;

            var explodeAnimation = SpriteBuilder.AddAnimation(animator, collection, new List<int>
            {
                SpriteBuilder.AddSpriteToCollection("NevernamedsItems/Resources/ThrowableActives/Jarate/jarate_break_001.png", collection),
                SpriteBuilder.AddSpriteToCollection("NevernamedsItems/Resources/ThrowableActives/Jarate/jarate_break_002.png", collection),
                SpriteBuilder.AddSpriteToCollection("NevernamedsItems/Resources/ThrowableActives/Jarate/jarate_break_003.png", collection),
            }, "break", tk2dSpriteAnimationClip.WrapMode.Once);
            explodeAnimation.fps = 16;

            var armedAnimation = SpriteBuilder.AddAnimation(animator, collection, new List<int>
            {
                SpriteBuilder.AddSpriteToCollection("NevernamedsItems/Resources/ThrowableActives/Jarate/jarate_toss_001.png", collection),
                SpriteBuilder.AddSpriteToCollection("NevernamedsItems/Resources/ThrowableActives/Jarate/jarate_toss_002.png", collection),
                SpriteBuilder.AddSpriteToCollection("NevernamedsItems/Resources/ThrowableActives/Jarate/jarate_toss_003.png", collection),
                SpriteBuilder.AddSpriteToCollection("NevernamedsItems/Resources/ThrowableActives/Jarate/jarate_toss_004.png", collection)
            }, "toss", tk2dSpriteAnimationClip.WrapMode.LoopSection);
            armedAnimation.fps = 12f;
            armedAnimation.loopStart = 0;

            CustomThrowableObject throwable = new CustomThrowableObject
            {
                doEffectOnHitGround = true,
                OnThrownAnimation = "deploy",
                OnHitGroundAnimation = "break",
                DefaultAnim = "toss",
                destroyOnHitGround = false,
                thrownSoundEffect = "Play_OBJ_item_throw_01",
                effectSoundEffect = "Play_OBJ_glassbottle_shatter_01",
            };
            bomb.AddComponent<CustomThrowableObject>(throwable);
            bomb.AddComponent<JarateSmashEffect>();
            return bomb;
        }
        public class JarateSmashEffect : CustomThrowableEffectDoer
        {
            public override void OnEffect(GameObject obj)
            {
                tk2dBaseSprite sprite = obj.GetComponent<tk2dBaseSprite>();
                GameObject splash = SpawnManager.SpawnVFX(EasyVFXDatabase.JarateExplosion, sprite.WorldCenter, Quaternion.identity);
                if (GameManager.Instance.PrimaryPlayer && Vector2.Distance(GameManager.Instance.PrimaryPlayer.CenterPosition, sprite.WorldCenter) < 4)
                {
                    GameManager.Instance.PrimaryPlayer.CurrentFireMeterValue = 0;
                    GameManager.Instance.PrimaryPlayer.IsOnFire = false;
                }
                if (GameManager.Instance.SecondaryPlayer && Vector2.Distance(GameManager.Instance.SecondaryPlayer.CenterPosition, sprite.WorldCenter) < 4)
                {
                    GameManager.Instance.SecondaryPlayer.CurrentFireMeterValue = 0;
                    GameManager.Instance.SecondaryPlayer.IsOnFire = false;
                }
                DeadlyDeadlyGoopManager goop = DeadlyDeadlyGoopManager.GetGoopManagerForGoopType(EasyGoopDefinitions.JarateGoop);
                goop.TimedAddGoopCircle(sprite.WorldCenter, 6f, 0.75f, true);
                if (sprite.WorldCenter.GetAbsoluteRoom() != null)
                {
                    List<AIActor> activeEnemies = sprite.WorldCenter.GetAbsoluteRoom().GetActiveEnemies(RoomHandler.ActiveEnemyType.All);
                    if (activeEnemies != null)
                    {
                        for (int i = 0; i < activeEnemies.Count; i++)
                        {
                            AIActor aiactor = activeEnemies[i];
                            if (Vector2.Distance(aiactor.Position, sprite.WorldCenter) < 5)
                            {
                                if (aiactor.healthHaver)
                                {
                                    aiactor.ApplyEffect(new GameActorJarateEffect()
                                    {
                                        duration = 10,
                                        stackMode = GameActorEffect.EffectStackingMode.Refresh,
                                        HealthMultiplier = aiactor.healthHaver.IsBoss ? 0.75f : 0.66f,
                                        SpeedMultiplier = 0.9f,
                                    });
                                }
                            }
                        }
                    }
                }
                StartCoroutine(Kill(obj));
            }
            private IEnumerator Kill(GameObject obj)
            {
                yield return new WaitForSeconds(0.25f);
                UnityEngine.Object.Destroy(obj);
                yield break;
            }
        }

    }
}