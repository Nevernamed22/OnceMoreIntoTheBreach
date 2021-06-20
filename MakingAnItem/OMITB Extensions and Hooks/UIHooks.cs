using MonoMod.RuntimeDetour;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace NevernamedsItems
{
    class UIHooks
    {
        public static void Init()
        {
            updateLabelHook = new Hook(
                typeof(GameUIItemController).GetMethod("UpdateItem", BindingFlags.Instance | BindingFlags.Public),
                typeof(UIHooks).GetMethod("UpdateCustomLabel")
            );
        }
        public static Hook updateLabelHook;
        public static void UpdateCustomLabel(Action<GameUIItemController, PlayerItem, List<PlayerItem>> orig, GameUIItemController self, PlayerItem current, List<PlayerItem> items)
        {
            orig(self, current, items);
            if (current && current is LabelablePlayerItem)
            {
                LabelablePlayerItem labelable = current as LabelablePlayerItem;
                if (!string.IsNullOrEmpty(labelable.currentLabel))
                {
                    self.ItemCountLabel.IsVisible = true;
                    self.ItemCountLabel.Text = labelable.currentLabel;
                }
            }
        }
    }
}
