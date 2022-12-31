using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using Dungeonator;
using Alexandria.ItemAPI;
using Alexandria.Misc;
using UnityEngine;

namespace NevernamedsItems
{
    class LaserKnife : PlayerItem
    {
        public static void Init()
        {
            string itemName = "Laser Knife";
            string resourceName = "NevernamedsItems/Resources/NeoActiveSprites/laserknife_icon";
            GameObject obj = new GameObject(itemName);
            var item = obj.AddComponent<LaserKnife>();
            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);
            string shortDesc = "He Couldn't See The Stars";
            string longDesc = "Vaporises the nearest enemy. \n\nA standard issue military pocket plasma blade for hand-to-hand combat.";
            ItemBuilder.SetupItem(item, shortDesc, longDesc, "nn");
            ItemBuilder.SetCooldownType(item, ItemBuilder.CooldownType.Damage, 600);

            laserSlashVFX = VFXToolbox.CreateVFX("LaserSlashVFX",
                  new List<string>()
                  {
                    "NevernamedsItems/Resources/MiscVFX/laserslashundertale_vfx_001",
                    "NevernamedsItems/Resources/MiscVFX/laserslashundertale_vfx_002",
                    "NevernamedsItems/Resources/MiscVFX/laserslashundertale_vfx_003",
                    "NevernamedsItems/Resources/MiscVFX/laserslashundertale_vfx_004",
                    "NevernamedsItems/Resources/MiscVFX/laserslashundertale_vfx_005",
                    "NevernamedsItems/Resources/MiscVFX/laserslashundertale_vfx_006",
                  },
                 14, //FPS
                  new IntVector2(74, 13), //Dimensions
                  tk2dBaseSprite.Anchor.MiddleCenter, //Anchor
                  false, //Uses a Z height off the ground
                  0 //The Z height, if used
                    );

            item.quality = ItemQuality.C;
        }
        public static GameObject laserSlashVFX;
        public override void DoEffect(PlayerController user)
        {
            AIActor enemy = user.CenterPosition.GetNearestEnemyToPosition();
            UnityEngine.Object.Instantiate<GameObject>(laserSlashVFX, enemy.sprite.WorldCenter, Quaternion.identity);
            if (enemy && (!enemy.healthHaver || !enemy.healthHaver.IsBoss))
            {
                GameManager.Instance.Dungeon.StartCoroutine(HandleEnemyDeath(enemy, user.CenterPosition.GetVectorToNearestEnemy()));
            }
            else if (enemy && enemy.healthHaver && enemy.healthHaver.IsBoss)
            {
                enemy.healthHaver.ApplyDamage(100, Vector2.zero, "Laser Knife", CoreDamageTypes.None,  DamageCategory.Unstoppable, true, null, true);
            }
            AkSoundEngine.PostEvent("Play_WPN_bountyhunterarm_shot_03", user.gameObject);
        }
        public static IEnumerator HandleEnemyDeath(AIActor target, Vector2 motionDirection)
        {
            CombineEvaporateEffect origEvap = (PickupObjectDatabase.GetById(519) as Gun).alternateVolley.projectiles[0].projectiles[0].GetComponent<CombineEvaporateEffect>();

            target.EraseFromExistenceWithRewards(false);
            Transform copyTransform = CreateEmptySprite(target);
            tk2dSprite copySprite = copyTransform.GetComponentInChildren<tk2dSprite>();
            GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(origEvap.ParticleSystemToSpawn, copySprite.WorldCenter.ToVector3ZisY(0f), Quaternion.identity);
            ParticleSystem component = gameObject.GetComponent<ParticleSystem>();
            gameObject.transform.parent = copyTransform;
            if (copySprite)
            {
                gameObject.transform.position = copySprite.WorldCenter;
                Bounds bounds = copySprite.GetBounds();
                var shapeVar = component.shape;
                shapeVar.scale = new Vector3(bounds.extents.x * 2f, bounds.extents.y * 2f, 0.125f);
            }
            float elapsed = 0f;
            float duration = 2.5f;
            copySprite.renderer.material.DisableKeyword("TINTING_OFF");
            copySprite.renderer.material.EnableKeyword("TINTING_ON");
            copySprite.renderer.material.DisableKeyword("EMISSIVE_OFF");
            copySprite.renderer.material.EnableKeyword("EMISSIVE_ON");
            copySprite.renderer.material.DisableKeyword("BRIGHTNESS_CLAMP_ON");
            copySprite.renderer.material.EnableKeyword("BRIGHTNESS_CLAMP_OFF");
            copySprite.renderer.material.SetFloat("_EmissiveThresholdSensitivity", 5f);
            copySprite.renderer.material.SetFloat("_EmissiveColorPower", 1f);
            int emId = Shader.PropertyToID("_EmissivePower");
            while (elapsed < duration)
            {
                elapsed += BraveTime.DeltaTime;
                float t = elapsed / duration;
                copySprite.renderer.material.SetFloat(emId, Mathf.Lerp(1f, 10f, t));
                copySprite.renderer.material.SetFloat("_BurnAmount", t);
                copyTransform.position += motionDirection.ToVector3ZisY(0f).normalized * BraveTime.DeltaTime * 1f;
                yield return null;
            }
            UnityEngine.Object.Destroy(copyTransform.gameObject);
            yield break;
        }
        private static Transform CreateEmptySprite(AIActor target)
        {
            GameObject gameObject = new GameObject("suck image");
            gameObject.layer = target.gameObject.layer;
            tk2dSprite tk2dSprite = gameObject.AddComponent<tk2dSprite>();
            gameObject.transform.parent = SpawnManager.Instance.VFX;
            tk2dSprite.SetSprite(target.sprite.Collection, target.sprite.spriteId);
            tk2dSprite.transform.position = target.sprite.transform.position;
            GameObject gameObject2 = new GameObject("image parent");
            gameObject2.transform.position = tk2dSprite.WorldCenter;
            tk2dSprite.transform.parent = gameObject2.transform;
            tk2dSprite.usesOverrideMaterial = true;
            if (target.optionalPalette != null)
            {
                tk2dSprite.renderer.material.SetTexture("_PaletteTex", target.optionalPalette);
            }
            return gameObject2.transform;
        }

        public override bool CanBeUsed(PlayerController user)
        {
            if (user.CenterPosition.GetNearestEnemyToPosition() != null && (Vector2.Distance(user.CenterPosition.GetNearestEnemyToPosition().Position, user.CenterPosition) <= 5)) return true;
            return false;
        }
    }
}
