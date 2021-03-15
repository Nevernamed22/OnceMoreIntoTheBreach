using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;
using ItemAPI;

namespace NevernamedsItems
{
    public class BlightShell : PassiveItem
    {
        public static void Init()
        {
            //The name of the item
            string itemName = "Blight Shell";

            //Refers to an embedded png in the project. Make sure to embed your resources! Google it
            string resourceName = "NevernamedsItems/Resources/blightshell_icon";

            //Create new GameObject
            GameObject obj = new GameObject(itemName);

            //Add a PassiveItem component to the object
            var item = obj.AddComponent<BlightShell>();

            //Adds a tk2dSprite component to the object and adds your texture to the item sprite collection
            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);

            //Ammonomicon entry variables
            string shortDesc = "Apocryphal Armoury";
            string longDesc = "Grants a free shotgun every floor, as well as a free curse."+"\n\nShotgun Gundead are often neglected in death, despite their noble status. This artefact collects their souls, because nobody else will.";

            //Adds the item to the gungeon item list, the ammonomicon, the loot table, etc.
            //Do this after ItemBuilder.AddSpriteToObject!
            ItemBuilder.SetupItem(item, shortDesc, longDesc, "nn");

            //Adds the actual passive effect to the item
            //ItemBuilder.AddPassiveStatModifier(item, PlayerStats.StatType.Curse, 1f, StatModifier.ModifyMethod.ADDITIVE);

            //Set the rarity of the item
            item.quality = PickupObject.ItemQuality.B;
            item.AddToSubShop(ItemBuilder.ShopType.Cursula);
        }
        private void OnNewFloor()
        {
            PlayerController player = this.Owner;
            giveNewShotgun();
        }

        private void giveNewShotgun()
        {
            int selectedShotgun = BraveUtility.RandomElement(shotgunIDs);
            if (Owner.HasPickupID(selectedShotgun) || selectedShotgun == 0)
            {
                giveNewShotgun();
            }
            else
            {
                var gameGun = (PickupObjectDatabase.GetById(selectedShotgun) as Gun);
                var gun = UnityEngine.Object.Instantiate<Gun>(gameGun);

                gun.AddPassiveStatModifier(PlayerStats.StatType.Curse, 1, StatModifier.ModifyMethod.ADDITIVE);

                Owner.inventory.AddGunToInventory(gun, true);
            }
        }

        public static List<int> shotgunIDs = new List<int>()
        {
            347, //Shotgrub
            404, //Siren
            55, //Void Shotgun
            61, //Bundle of Wands
            93, //Old Goldie
            126, //Shotbow
            1, //Winchester
            202, //Sawed Off
            151, //The Membrane
            346, //Huntsman
            51, //Regular Shotgun
            175, //Tangler
            143, //Shotgun Full of Hate
            379, //Shotgun Full of Love
            122, //Blunderbuss
            601, //Big Shotgun
            541, //Casey
            550, //Knight's Gun
            512, //Shell
            363, //Trick Gun
            18, //Blooper
            82, //Elephant Gun
            231, //Gilded Hydra
            225, //Ice Breaker
            365, //Mass Shotgun
            123, //Pulse Cannon
            406, //Rattler
            329, //Zilla Shotgun
            JusticeGun.JusticeID, //Justice
            Orgun.OrgunID, //Orgun
            Octagun.OctagunID, //Octagun
            ClownShotgun.ClownShotgunID,
            Ranger.RangerID,
        };

        public override void Pickup(PlayerController player)
        {
            base.Pickup(player);
            bool hasntAlreadyBeenCollected = !this.m_pickedUpThisRun;
            if (hasntAlreadyBeenCollected)
            {
                giveNewShotgun();
            }
            GameManager.Instance.OnNewLevelFullyLoaded += this.OnNewFloor;
        }
        public override DebrisObject Drop(PlayerController player)
        {
            DebrisObject result = base.Drop(player);
            GameManager.Instance.OnNewLevelFullyLoaded -= this.OnNewFloor;
            return result;
        }
        protected override void OnDestroy()
        {
            GameManager.Instance.OnNewLevelFullyLoaded -= this.OnNewFloor;
            base.OnDestroy();
        }
    }
}
