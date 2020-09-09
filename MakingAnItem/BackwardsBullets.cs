using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;
using ItemAPI;

namespace NevernamedsItems
{
    public class BackwardsBullets : PassiveItem
    {
        public static void Init()
        {
            //The name of the item
            string itemName = "<WIP> Backwards Bullets <WIP>";

            //Refers to an embedded png in the project. Make sure to embed your resources! Google it
            string resourceName = "NevernamedsItems/Resources/backwardsbullets_icon";

            //Create new GameObject
            GameObject obj = new GameObject(itemName);

            //Add a PassiveItem component to the object
            var item = obj.AddComponent<BackwardsBullets>();

            //Adds a tk2dSprite component to the object and adds your texture to the item sprite collection
            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);

            //Ammonomicon entry variables
            string shortDesc = "gnaB gnaB ytoohS";
            string longDesc = "Your shots don't know where to go, but they are much stronger.\n\n" + "Nobody knows why these rounds were made and nobody dares to ask";

            //Adds the item to the gungeon item list, the ammonomicon, the loot table, etc.
            //Do this after ItemBuilder.AddSpriteToObject!
            ItemBuilder.SetupItem(item, shortDesc, longDesc, "nn");

            //Adds the actual passive effect to the item
            ItemBuilder.AddPassiveStatModifier(item, PlayerStats.StatType.Damage, 2f, StatModifier.ModifyMethod.MULTIPLICATIVE);
            //ItemBuilder.AddPassiveStatModifier(item, PlayerStats.StatType.ProjectileSpeed, -1f, StatModifier.ModifyMethod.MULTIPLICATIVE);
            //Set the rarity of the item
            item.quality = PickupObject.ItemQuality.EXCLUDED;
        }
        protected override void Update()
        {
            /*if (Owner)
            {
                //Vector2 vector = Owner.CenterPosition;
                //Vector2 aimPoint = (Owner.unadjustedAimPoint.XY() - vector).normalized;
                Owner.unadjustedAimPoint *= -1;
            }*/
        }
    }
}
