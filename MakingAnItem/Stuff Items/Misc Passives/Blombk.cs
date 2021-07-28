using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using static MonoMod.Cil.RuntimeILReferenceBag.FastDelegateInvokers;
using UnityEngine;
using ItemAPI;
using MonoMod.RuntimeDetour;
using System.Reflection;

namespace NevernamedsItems
{
    class Blombk : PassiveItem
    {
        public static void Init()
        {
            //The name of the item
            string itemName = "Blombk";

            //Refers to an embedded png in the project. Make sure to embed your resources! Google it
            string resourceName = "NevernamedsItems/Resources/blombk_icon";

            //Create new GameObject
            GameObject obj = new GameObject(itemName);

            //Add a PassiveItem component to the object
            var item = obj.AddComponent<Blombk>();

            //Adds a tk2dSprite component to the object and adds your texture to the item sprite collection
            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);

            //Ammonomicon entry variables
            string shortDesc = "Boomer Blanks";
            string longDesc = "Triggers a small blank whenever an explosion goes off." + "\n\nA Fuselier egg painted blue.";

            //Adds the item to the gungeon item list, the ammonomicon, the loot table, etc.
            //Do this after ItemBuilder.AddSpriteToObject!
            ItemBuilder.SetupItem(item, shortDesc, longDesc, "nn");

            //Adds the actual passive effect to the item

            //Set the rarity of the item
            item.quality = PickupObject.ItemQuality.C;
            item.AddToSubShop(ItemBuilder.ShopType.OldRed);

            BlombkID = item.PickupObjectId;
        }
        public static int BlombkID;

        private static Hook BombHook = new Hook(
    typeof(Exploder).GetMethod("Explode", BindingFlags.Static | BindingFlags.Public),
    typeof(Blombk).GetMethod("ExplosionHook", BindingFlags.Instance | BindingFlags.Public),
    typeof(Exploder)
);
        public void ExplosionHook(Action<Vector3, ExplosionData, Vector2, Action, bool, CoreDamageTypes, bool> orig, Vector3 position, ExplosionData data, Vector2 sourceNormal, Action onExplosionBegin = null, bool ignoreQueues = false, CoreDamageTypes damageTypes = CoreDamageTypes.None, bool ignoreDamageCaps = false)
        {
            orig(position, data, sourceNormal, onExplosionBegin, ignoreQueues, damageTypes, ignoreDamageCaps);
            try
            {
                if (GameManager.Instance.PrimaryPlayer.HasPickupID(BlombkID))
                {
                    if (GameManager.Instance.PrimaryPlayer.PlayerHasActiveSynergy("Atomic Blombk") && (UnityEngine.Random.value < 0.2f)) DoMegaBlank(GameManager.Instance.PrimaryPlayer, position);
                    else DoMicroBlank(GameManager.Instance.PrimaryPlayer, position);
                }
                else if (GameManager.Instance.SecondaryPlayer != null && GameManager.Instance.SecondaryPlayer.HasPickupID(BlombkID))
                {
                    if (GameManager.Instance.SecondaryPlayer.PlayerHasActiveSynergy("Atomic Blombk") && (UnityEngine.Random.value < 0.2f)) DoMegaBlank(GameManager.Instance.SecondaryPlayer, position);
                    else DoMicroBlank(GameManager.Instance.SecondaryPlayer, position);
                }
            }
            catch (Exception e)
            {
                ETGModConsole.Log(e.Message);
                ETGModConsole.Log(e.StackTrace);
            }
        }
        public delegate void Action<T1, T2, T3, T4, T5, T6, T7>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7);
        public static void DoMicroBlank(PlayerController owner, Vector2 center)
        {
            GameObject silencerVFX = (GameObject)ResourceCache.Acquire("Global VFX/BlankVFX_Ghost");
            AkSoundEngine.PostEvent("Play_OBJ_silenceblank_small_01", owner.gameObject);
            GameObject gameObject = new GameObject("silencer");
            SilencerInstance silencerInstance = gameObject.AddComponent<SilencerInstance>();
            float additionalTimeAtMaxRadius = 0.25f;
            silencerInstance.TriggerSilencer(center, 25f, 5f, silencerVFX, 0f, 3f, 3f, 3f, 250f, 5f, additionalTimeAtMaxRadius, owner, false, false);
        }
        public override void Pickup(PlayerController player)
        {
            base.Pickup(player);
        }
        public static void DoMegaBlank(PlayerController player, Vector2 center)
        {
            GameObject bigSilencerVFX = (GameObject)ResourceCache.Acquire("Global VFX/BlankVFX");
            AkSoundEngine.PostEvent("Play_OBJ_silenceblank_use_01", player.gameObject);
            GameObject gameObject = new GameObject("silencer");
            SilencerInstance silencerInstance = gameObject.AddComponent<SilencerInstance>();
            silencerInstance.TriggerSilencer(center, 50f, 25f, bigSilencerVFX, 0.15f, 0.2f, 50f, 10f, 140f, 15f, 0.5f, player, true, false);
        }
    }
}

