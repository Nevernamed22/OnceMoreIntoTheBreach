using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;
using Alexandria.ItemAPI;
using Alexandria.Misc;
using SaveAPI;
using System.Collections;
using Brave.BulletScript;
using Alexandria.PrefabAPI;

namespace NevernamedsItems
{
    class NitroBullets : PassiveItem
    {
        public static void Init()
        {
            PickupObject item = ItemSetup.NewItem<NitroBullets>(
            "Nitro Bullets",
            "Badda Bing...",
            "50% chance for enemies to explode violently on death." + "\n\nMade by a lunatic who loved the way the ground shook when he used his special brand of... making things go away." + "\n\nYou are not immune to these explosions. You have been warned.",
            "nitrobullets_improved");
            item.quality = PickupObject.ItemQuality.C;
            item.AddToSubShop(ItemBuilder.ShopType.Trorc);
            item.SetTag("bullet_modifier");
            item.SetupUnlockOnCustomFlag(CustomDungeonFlags.PURCHASED_NITROBULLETS, true);
            item.AddItemToDougMetaShop(15);
            ID = item.PickupObjectId;
            Doug.AddToLootPool(item.PickupObjectId);

            gunDrop = Breakables.GenerateDebrisObject(Initialisation.itemCollection, "nitrobullets_icon", AngularVelocity: 100f);
        }
        public static int ID;
        public static DebrisObject gunDrop;
        private void OnEnemyDamaged(float damage, bool fatal, HealthHaver enemyHealth)
        {
            if (fatal)
            {
                float ch = 0.5f;
                if (Owner && Owner.PlayerHasActiveSynergy("...Badda Boom!")) { ch = 1f; }
                if (UnityEngine.Random.value <= ch && enemyHealth && enemyHealth.aiActor && !enemyHealth.IsBoss && enemyHealth.aiActor.IsNormalEnemy)
                {
                    base.StartCoroutine(Detonate(enemyHealth.aiActor));
                }
            }
        }
        private IEnumerator Detonate(AIActor target)
        {
            if (target && target.sprite != null && target.sprite.CurrentSprite != null)
            {
                //Create a Fake enemy sprite
                GameObject dummy = new GameObject("Detonation Dummy");
                dummy.layer = target.gameObject.layer;
                tk2dSprite dummySprite = dummy.AddComponent<tk2dSprite>();
                dummySprite.SetSprite(target.sprite.Collection, target.sprite.spriteId);
                dummySprite.IsPerpendicular = true;
                dummySprite.transform.position = target.sprite.transform.position;
                dummySprite.HeightOffGround = target.sprite.HeightOffGround;
                dummySprite.UpdateZDepth();
                dummySprite.renderer.material.shader = ShaderCache.Acquire("Brave/LitCutoutUber");
                dummySprite.renderer.material.shaderKeywords = new string[] { "BRIGHTNESS_CLAMP_ON", "EMISSIVE_OFF", "TINTING_ON" };
                SpriteOutlineManager.AddOutlineToSprite(dummySprite, Color.black);

                if (target.ShadowObject)
                {
                    GameObject dummyShadow = new GameObject("DummyShadow");
                    dummyShadow.layer = target.ShadowObject.layer;
                    tk2dSprite dummyShadowSprite = dummyShadow.AddComponent<tk2dSprite>();
                    dummyShadowSprite.SetSprite(target.ShadowObject.GetComponent<tk2dBaseSprite>().Collection, target.ShadowObject.GetComponent<tk2dBaseSprite>().spriteId);
                    dummyShadowSprite.transform.position = target.ShadowObject.GetComponent<tk2dBaseSprite>().transform.position;
                    dummyShadowSprite.renderer.material.shader = target.ShadowObject.GetComponent<tk2dBaseSprite>().renderer.material.shader;
                    dummyShadowSprite.renderer.material.shaderKeywords = target.ShadowObject.GetComponent<tk2dBaseSprite>().renderer.material.shaderKeywords;
                    dummyShadowSprite.IsPerpendicular = target.ShadowObject.GetComponent<tk2dBaseSprite>().IsPerpendicular;
                    dummyShadow.transform.SetParent(dummy.transform);
                }



                if (target.optionalPalette != null)
                {
                    dummySprite.renderer.material.SetTexture("_PaletteTex", target.optionalPalette);
                }
                Transform outlineSprite = dummy.transform.Find("BraveOutlineSprite");

                Vector3 startPosition = target.sprite.WorldBottomCenter;

                float elapsed = 0f;
                float duration = 1f;
                float blinkInterval = 0f;
                int beeps = 0;

                if (target)
                {
                    target.HandleRewards();
                    target.StealthDeath = true;
                    if (target.behaviorSpeculator) { target.behaviorSpeculator.InterruptAndDisable(); }

                    if (target.CurrentGun != null)
                    {
                        Vector2 positionToSpawn = target.CurrentGun.transform.position;
                        if (target.aiShooter)
                        {
                            positionToSpawn = target.CurrentGun.sprite.FlipY ? target.aiShooter.attachPointCachedFlippedPosition : target.aiShooter.attachPointCachedPosition;
                        }
                        GameObject gunDebris = SpawnManager.SpawnDebris(gunDrop.gameObject, target.CurrentGun.transform.position, Quaternion.Euler(0f, 0f, target.CurrentGun.CurrentAngle));
                        gunDebris.GetComponent<tk2dSprite>().SetSprite(target.CurrentGun.sprite.collection, target.CurrentGun.sprite.spriteId);
                        DebrisObject component = gunDebris.GetComponent<DebrisObject>();
                        if (component)
                        {
                            component.Trigger(BraveUtility.RandomAngle().DegreeToVector2().normalized * 2f, 1f, 1f);
                        }
                    }
                }
                yield return null;
                if (target && target.gameObject) { UnityEngine.Object.Destroy(target.gameObject); }

                //Erase



                while (elapsed < duration)
                {
                    float t = elapsed / duration;

                    elapsed += BraveTime.DeltaTime;
                    if (blinkInterval > 0f) { blinkInterval -= BraveTime.DeltaTime; }
                    else if (dummy)
                    {
                        AkSoundEngine.PostEvent("Play_OBJ_mine_beep_01", dummy);
                        blinkInterval = Mathf.Lerp(0.25f, 0.05f, t);
                        beeps++;
                    }
                    float boundsX = dummySprite.GetBounds().size.x;
                    if (dummy && dummySprite)
                    {
                        Test(dummySprite, startPosition);
                        dummy.transform.localScale = Vector3.Lerp(Vector3.one, new Vector3(1.8f, 1.2f, 1.2f), t);
                        Color white = new Color(1, 0, 0, 1);
                        white.a = Mathf.Lerp(0f, 1f, t);
                        dummySprite.usesOverrideMaterial = true;
                        dummySprite.renderer.material.SetColor("_OverrideColor", white);
                    }

                    yield return null;
                }
                if (dummy)
                {
                    Exploder.DoDefaultExplosion(dummySprite.WorldCenter, new Vector2());
                    UnityEngine.Object.Destroy(dummy);
                }
            }
            else if (target)
            {
                Exploder.DoDefaultExplosion(target.transform.position, new Vector2());
            }
            yield break;
        }
        public static void Test(tk2dSprite spr, Vector3 pos)
        {
            Bounds bounds = spr.GetBounds();
            Vector2 result = bounds.min;
            result.x += bounds.extents.x * spr.transform.localScale.x;
            spr.transform.position = pos - result.ToVector3ZUp(0f);
        }
        public override void Pickup(PlayerController player)
        {
            base.Pickup(player);
            player.OnAnyEnemyReceivedDamage += this.OnEnemyDamaged;
        }
        public override DebrisObject Drop(PlayerController player)
        {
            player.OnAnyEnemyReceivedDamage -= this.OnEnemyDamaged;
            return base.Drop(player);
        }
        public override void OnDestroy()
        {
            if (Owner) Owner.OnAnyEnemyReceivedDamage -= this.OnEnemyDamaged;
            base.OnDestroy();
        }
    }
}
