using MonoMod.RuntimeDetour;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace NevernamedsItems
{
    class PedestalHooks
    {
        public static void Init()
        {
            pedestalSpawnHook = new Hook(
                typeof(RewardPedestal).GetMethod("MaybeBecomeMimic", BindingFlags.Instance | BindingFlags.Public),
                typeof(PedestalHooks).GetMethod("PostProcessPedestal", BindingFlags.Static | BindingFlags.Public)
            );
        }
        public static Hook pedestalSpawnHook;
        public static void PostProcessPedestal(Action<RewardPedestal> orig, RewardPedestal self)
        {
            //ETGModConsole.Log("Pedestal spawned");
            orig(self);
            if (GameManager.Instance.AnyPlayerHasPickupID(ScrollOfExactKnowledge.ScrollOfExactKnowledgeID))
            {
                if (GameManager.Instance.PrimaryPlayer != null)
                {
                    foreach (PassiveItem item in GameManager.Instance.PrimaryPlayer.passiveItems)
                    {
                        if (item.GetComponent<ScrollOfExactKnowledge>() != null)
                        {
                            item.GetComponent<ScrollOfExactKnowledge>().ReactToSpawnedPedestal(self);
                        }
                    }
                }
                if (GameManager.Instance.SecondaryPlayer != null)
                {
                    foreach (PassiveItem item in GameManager.Instance.SecondaryPlayer.passiveItems)
                    {
                        if (item.GetComponent<ScrollOfExactKnowledge>() != null)
                        {
                            item.GetComponent<ScrollOfExactKnowledge>().ReactToSpawnedPedestal(self);
                        }
                    }
                }
            }
        }
    }
}
