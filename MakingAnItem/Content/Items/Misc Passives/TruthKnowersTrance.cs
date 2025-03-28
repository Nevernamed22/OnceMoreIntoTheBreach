using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;
using Alexandria.ItemAPI;
using Dungeonator;
using Alexandria.Misc;
using Alexandria.ChestAPI;
using System.Collections;

namespace NevernamedsItems
{
    public class TruthKnowersTrance : PassiveItem
    {
        public static void Init()
        {
            PassiveItem item = ItemSetup.NewItem<TruthKnowersTrance>(
              "Truth Knowers Trance",
              "Several Truths!",
              "Coaxes low quality chests into revealing their TRUE form!\n\nA heightened state of meditative candour only attainable by the most adept and honest Brothers of the Order of Truth Knowers.",
              "truthknowerstrance_icon") as PassiveItem;
            item.quality = PickupObject.ItemQuality.B;
            ID = item.PickupObjectId;
            CustomActions.OnChestPostSpawn += TruthKnowersTrance.OnChestSpawned;

            hoveringAlbernVFX = VFXToolbox.CreateVFXBundle("HoveringAlbern", true, 5f);
        }
        public static int ID;
        public static GameObject hoveringAlbernVFX;
        public RoomHandler lastCheckedRoom;
        public static GameObject TruthChest = LoadHelper.LoadAssetFromAnywhere<GameObject>("truthchest");
        public override void Update()
        {
            if (Owner && !Dungeon.IsGenerating)
            {
                if (Owner.CurrentRoom != null && Owner.CurrentRoom != lastCheckedRoom)
                {
                    Chest[] allChests = FindObjectsOfType<Chest>();
                    foreach (Chest chest in allChests)
                    {
                        if (chest.transform.position.GetAbsoluteRoom() == Owner.CurrentRoom && !chest.IsOpen && !chest.IsBroken)
                        {
                            base.StartCoroutine(processChest(chest));
                        }
                    }
                    lastCheckedRoom = Owner.CurrentRoom;
                }
            }
        }
        public void ReactToRuntimeSpawnedChest(Chest chest)
        {
            base.StartCoroutine(processChest(chest));
        }
        public static void OnChestSpawned(Chest chest)
        {
            if (!Dungeon.IsGenerating)
            {
                if (GameManager.Instance.AnyPlayerHasPickupID(TruthKnowersTrance.ID))
                {
                    if (GameManager.Instance.PrimaryPlayer != null)
                    {
                        foreach (PassiveItem item in GameManager.Instance.PrimaryPlayer.passiveItems)
                        {
                            if (item.GetComponent<TruthKnowersTrance>() != null)
                            {
                                item.GetComponent<TruthKnowersTrance>().ReactToRuntimeSpawnedChest(chest);
                            }
                        }
                    }
                    if (GameManager.Instance.SecondaryPlayer != null)
                    {
                        foreach (PassiveItem item in GameManager.Instance.SecondaryPlayer.passiveItems)
                        {
                            if (item.GetComponent<TruthKnowersTrance>() != null)
                            {
                                item.GetComponent<TruthKnowersTrance>().ReactToRuntimeSpawnedChest(chest);
                            }
                        }
                    }
                }
            }
        }

        public class ChestIsFading : MonoBehaviour { }
        public IEnumerator processChest(Chest chest)
        {
            bool isValidTier = chest.GetChestTier() == ChestUtility.ChestTier.BROWN || (Owner && Owner.PlayerHasActiveSynergy("Several Truths") && chest.GetChestTier() == ChestUtility.ChestTier.BLUE);

            if (!isValidTier || Chest.m_IsCoopMode || chest.IsMirrorChest || chest.GetComponent<ChestIsFading>() != null) { yield break; }

            RoomHandler currentRoom = chest.GetAbsoluteParentRoom();

            chest.ForceKillFuse();
            chest.PreventFuse = true;
            chest.majorBreakable.TemporarilyInvulnerable = true;
            chest.m_temporarilyUnopenable = true;
            chest.gameObject.AddComponent<ChestIsFading>();
            currentRoom.DeregisterInteractable(chest);
            SpriteOutlineManager.RemoveOutlineFromSprite(chest.sprite, true);
            chest.DeregisterChestOnMinimap();
            yield return new WaitForSeconds(1f);

            GameObject albern = UnityEngine.Object.Instantiate(hoveringAlbernVFX, chest.sprite.WorldCenter + new Vector2(0f, 1f), Quaternion.identity);

            yield return new WaitForSeconds(0.5f);

            float elapsed = 0f;
            float duration = 5f;

            float sparkleAccum = 0f;

            //Prepare Chest to Fade
            chest.sprite.renderer.material.shader = ShaderCache.Acquire("Brave/Internal/SimpleAlphaFadeUnlit");
            if (chest.LockAnimator != null)
            {
                chest.LockAnimator.renderer.material.shader = ShaderCache.Acquire("Brave/Internal/SimpleAlphaFadeUnlit");
            }

            //Create Truth Chest
            GameObject truthChest = UnityEngine.Object.Instantiate<GameObject>(TruthChest, chest.transform.position + new Vector3(0.25f, 0f, 0f), Quaternion.identity);
            Chest truthChestComponent = truthChest.GetComponent<Chest>();
            truthChestComponent.m_room = currentRoom;
            truthChestComponent.sprite.UpdateZDepth();
            truthChestComponent.majorBreakable.TemporarilyInvulnerable = true;
            truthChestComponent.m_temporarilyUnopenable = true;
            truthChestComponent.Initialize();
            SpriteOutlineManager.RemoveOutlineFromSprite(truthChestComponent.sprite, true);
            PhysicsEngine.Instance.RegisterOverlappingGhostCollisionExceptions(truthChestComponent.specRigidbody, null, false);
            Shader TruthChestRevertShader = truthChestComponent.sprite.renderer.material.shader;
            truthChestComponent.sprite.renderer.material.shader = ShaderCache.Acquire("Brave/Internal/SimpleAlphaFadeUnlit");
            if (truthChestComponent.LockAnimator != null)
            {
                truthChestComponent.LockAnimator.renderer.material.shader = ShaderCache.Acquire("Brave/Internal/SimpleAlphaFadeUnlit");
            }

            while (elapsed < duration)
            {
                elapsed += BraveTime.DeltaTime;

                sparkleAccum += BraveTime.DeltaTime * 3;
                if (sparkleAccum > 1f)
                {
                    int num = Mathf.FloorToInt(sparkleAccum);
                    sparkleAccum %= 1f;
                    Vector2 minpos = chest.sprite.WorldBottomLeft;
                    Vector2 maxpos = chest.sprite.WorldTopRight;
                    for (int i = 0; i < num; i++)
                    {
                        GameObject piss = UnityEngine.Object.Instantiate(SharedVFX.GoldenSparkle, new Vector2(UnityEngine.Random.Range(minpos.x, maxpos.x), UnityEngine.Random.Range(minpos.y, maxpos.y)), Quaternion.identity);
                        piss.transform.parent = chest.transform;
                        piss.GetComponent<tk2dBaseSprite>().HeightOffGround = 0.2f;
                        chest.sprite.AttachRenderer(piss.GetComponent<tk2dBaseSprite>());
                    }
                }

                chest.sprite.renderer.material.SetFloat("_Fade", Mathf.Lerp(1, 0, elapsed / duration));
                if (chest.LockAnimator != null)
                {
                    chest.LockAnimator.renderer.material.SetFloat("_Fade", Mathf.Lerp(1, 0, elapsed / duration));
                }

                truthChestComponent.sprite.renderer.material.SetFloat("_Fade", Mathf.Lerp(0, 1, elapsed / duration));
                if (truthChestComponent.LockAnimator != null)
                {
                    truthChestComponent.LockAnimator.renderer.material.SetFloat("_Fade", Mathf.Lerp(0, 1, elapsed / duration));
                }

                yield return null;
            }
            UnityEngine.Object.Destroy(chest.gameObject);

            //Make the Truth Chest real
            currentRoom.RegisterInteractable(truthChestComponent);
            SpriteOutlineManager.AddOutlineToSprite(truthChestComponent.sprite, Color.black);
            truthChestComponent.majorBreakable.TemporarilyInvulnerable = false;
            truthChestComponent.m_temporarilyUnopenable = false;
            truthChestComponent.IsLocked = false;
            truthChestComponent.IsSealed = false;
            truthChestComponent.RegisterChestOnMinimap(currentRoom);

            yield return new WaitForSeconds(0.5f);

            if (truthChestComponent.LockAnimator != null)
            {
                truthChestComponent.LockAnimator.PlayAndDestroyObject("truth_lock_open", null);
            }


            truthChestComponent.sprite.renderer.material.shader = TruthChestRevertShader;
            if (truthChestComponent.LockAnimator != null) { truthChestComponent.LockAnimator.renderer.material.shader = TruthChestRevertShader; }

            if (Chest.m_IsCoopMode)
            {
                truthChestComponent.BecomeCoopChest();
            }

            //Fade albern from existence
            float albernelapsed = 0f;
            float albernduration = 4f;
            tk2dBaseSprite albernSprite = albern.GetComponent<tk2dBaseSprite>();
            albernSprite.renderer.material.shader = ShaderCache.Acquire("Brave/Internal/SimpleAlphaFadeUnlit");
            albernSprite.usesOverrideMaterial = true;
            while (albernelapsed < albernduration)
            {
                albernelapsed += BraveTime.DeltaTime;

                albernSprite.renderer.material.SetFloat("_Fade", Mathf.Lerp(1, 0, albernelapsed / albernduration));
                yield return null;
            }

            UnityEngine.Object.Destroy(albern);
            yield break;
        }
    }
}
