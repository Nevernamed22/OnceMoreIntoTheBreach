using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Dungeonator;
using ItemAPI;
using UnityEngine;

namespace NevernamedsItems
{
    class DoctorsRemote : PlayerItem
    {
        public static void Init()
        {
            string itemName = "Doctors Remote";
            string resourceName = "NevernamedsItems/Resources/portablehole_icon";
            GameObject obj = new GameObject(itemName);
            var item = obj.AddComponent<DoctorsRemote>();
            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);
            string shortDesc = "The Hole Nine Yards";
            string longDesc = "Not only is this hole portable, it's almost bottomless. And depressed. I mean, you would be too if you didn't have a bottom.";
            ItemBuilder.SetupItem(item, shortDesc, longDesc, "nn");
            ItemBuilder.SetCooldownType(item, ItemBuilder.CooldownType.Damage, 600);

            item.consumable = false;
            item.quality = ItemQuality.EXCLUDED; //D         
        }
        
        protected override void DoEffect(PlayerController user)
        {
            AIActor Enemy = EnemyDatabase.GetOrLoadByGuid(EnemyGuidDatabase.Entries["gatling_gull"]);
            GatlingGullRocketBehavior rocketbehav = Enemy.GetComponentInChildren<GatlingGullRocketBehavior>();

if (rocketbehav != null) { ETGModConsole.Log("ITS WORKING"); }

            //SkyRocket component = SpawnManager.SpawnProjectile(this.Rocket, user.specRigidbody.UnitTopCenter, Quaternion.identity, true).GetComponent<SkyRocket>();

            //component.Target = this.m_aiActor.TargetRigidbody;            
        }
    }
}
