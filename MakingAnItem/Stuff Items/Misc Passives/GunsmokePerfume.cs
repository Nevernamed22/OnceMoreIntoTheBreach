using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using ItemAPI;
using MonoMod.RuntimeDetour;
using System.Reflection;
using Dungeonator;

namespace NevernamedsItems
{
    public class GunsmokePerfume : PassiveItem
    {
        public static void Init()
        {
            string itemName = "Gunsmoke Perfume";
            string resourceName = "NevernamedsItems/Resources/gunsmokeperfume_icon";
            GameObject obj = new GameObject(itemName);
            var item = obj.AddComponent<GunsmokePerfume>();
            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);
            string shortDesc = "Ode To Glock 42";
            string longDesc = "Charms enemies who get too close." + "\n\nThe enticing aroma of a battle hardened gunslinger!";

            ItemBuilder.SetupItem(item, shortDesc, longDesc, "nn");

            item.CanBeDropped = true;
            item.quality = PickupObject.ItemQuality.B;
        }
        public override void Update()
        {
            if (Owner && Owner.CurrentRoom != null)
            {
                List<AIActor> activeEnemies = Owner.CurrentRoom.GetActiveEnemies(RoomHandler.ActiveEnemyType.All);
                if (activeEnemies != null)
                {
                    for (int i = 0; i < activeEnemies.Count; i++)
                    {
                        AIActor aiactor = activeEnemies[i];
                        if (aiactor.IsNormalEnemy)
                        {
                            float range = 2;
                            if (Owner.PlayerHasActiveSynergy("Practically Pungent")) range *= 2;
                            float num = Vector2.Distance(Owner.CenterPosition, aiactor.CenterPosition);
                            if (num <= range)
                            {
                                this.AffectEnemy(aiactor);
                            }
                        }
                    }
                }
            }
            base.Update();
        }
        private void AffectEnemy(AIActor aiactor)
        {
            aiactor.ApplyEffect(StaticStatusEffects.charmingRoundsEffect);
            if (Owner.PlayerHasActiveSynergy("Regular Hottie")) aiactor.ApplyEffect(StaticStatusEffects.hotLeadEffect);
        }
        public override void Pickup(PlayerController player)
        {
            base.Pickup(player);
        }

        public override DebrisObject Drop(PlayerController player)
        {
            return base.Drop(player);
        }
    }

}
