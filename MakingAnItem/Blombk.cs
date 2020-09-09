/*using System;
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
            string resourceName = "NevernamedsItems/Resources/mutagen_icon";

            //Create new GameObject
            GameObject obj = new GameObject(itemName);

            //Add a PassiveItem component to the object
            var item = obj.AddComponent<Blombk>();

            //Adds a tk2dSprite component to the object and adds your texture to the item sprite collection
            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);

            //Ammonomicon entry variables
            string shortDesc = "Rampant Mutation";
            string longDesc = "Heals a small amount whenever the afflicted individual defeats a boss." + "\n\nThis mutagen progresses in stages, just like the Gungeon itself.";

            //Adds the item to the gungeon item list, the ammonomicon, the loot table, etc.
            //Do this after ItemBuilder.AddSpriteToObject!
            ItemBuilder.SetupItem(item, shortDesc, longDesc, "nn");

            //Adds the actual passive effect to the item

            //Set the rarity of the item
            item.quality = PickupObject.ItemQuality.EXCLUDED; //B
            //item.AddToSubShop(ItemBuilder.ShopType.OldRed);

            BlombkID = item.PickupObjectId;
        }
        private static int BlombkID;

        private static Hook BombHook = new Hook(
    typeof(Exploder).GetMethod("Explode", BindingFlags.Static | BindingFlags.Public),
    typeof(Blombk).GetMethod("ExplosionHook", BindingFlags.Instance | BindingFlags.Public),
    typeof(Exploder)
);
        public void ExplosionHook(Action<Vector3, ExplosionData, Vector2, Action, bool, CoreDamageTypes, bool> orig, Vector3 position, ExplosionData data, Vector2 sourceNormal, Action onExplosionBegin = null, bool ignoreQueues = false, CoreDamageTypes damageTypes = CoreDamageTypes.None, bool ignoreDamageCaps = false)
        {
            orig(position, data, sourceNormal, onExplosionBegin, ignoreQueues, damageTypes, ignoreDamageCaps);
            //ETGModConsole.Log("If you're seeing this, then the hook was triggered by an explosion!");
            try
            {
                if (GameManager.Instance.PrimaryPlayer.HasPickupID(BlombkID) || GameManager.Instance.SecondaryPlayer.HasPickupID(BlombkID))
                {
                    Vector2 locationOfExplosion = position;
                    DoMicroBlank(locationOfExplosion);
                }
            }
            catch (Exception e)
            {
                ETGModConsole.Log(e.Message);
                ETGModConsole.Log(e.StackTrace);
            }
        }
        public delegate void Action<T1, T2, T3, T4, T5, T6, T7>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7);
        private void DoMicroBlank(Vector2 center)
        {
            PlayerController owner = base.Owner;
            GameObject silencerVFX = (GameObject)ResourceCache.Acquire("Global VFX/BlankVFX_Ghost");
            AkSoundEngine.PostEvent("Play_OBJ_silenceblank_small_01", base.gameObject);
            GameObject gameObject = new GameObject("silencer");
            SilencerInstance silencerInstance = gameObject.AddComponent<SilencerInstance>();
            float additionalTimeAtMaxRadius = 0.25f;
            silencerInstance.TriggerSilencer(center, 25f, 5f, silencerVFX, 0f, 3f, 3f, 3f, 250f, 5f, additionalTimeAtMaxRadius, owner, false, false);
        }
        public override void Pickup(PlayerController player)
        {
            base.Pickup(player);
        }
    }
}*/
