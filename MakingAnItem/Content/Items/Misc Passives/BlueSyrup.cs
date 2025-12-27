using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Alexandria.ItemAPI;
using Alexandria.Misc;

namespace NevernamedsItems
{
    public class BlueSyrup : PassiveItem
    {
        public static int ID;
        public static void Init()
        {
            PickupObject item = ItemSetup.NewItem<BlueSyrup>(
            "Blue Syrup",
            "Blank Label",
            "Chance to regurgitate a blanking blue bubble. Goes down smooth, comes up rough.\n\nAn old fashioned cure-all for whatever ails you. It tastes like gasoline.",
            "bluesyrup_icon");
            
            item.quality = PickupObject.ItemQuality.B;
            item.AddToSubShop(ItemBuilder.ShopType.Goopton);
            item.AddToSubShop(ItemBuilder.ShopType.OldRed);

            ID = item.PickupObjectId;
        }
        public float countdownToNextBubble = 10f;
        public override void Update()
        {
            if (Owner && Owner.IsInCombat)
            {
                if (countdownToNextBubble > 0f) { countdownToNextBubble -= BraveTime.DeltaTime; }
                else
                {
            AkSoundEngine.PostEvent("Play_WPN_Bubbler_Drink_01", base.gameObject);
                    GameObject bubble = (PickupObjectDatabase.GetById(599) as Gun).DefaultModule.projectiles[UnityEngine.Random.Range(0,3)].InstantiateAndFireInDirection(Owner.CenterPosition, Owner.CurrentGun ? Owner.CurrentGun.CurrentAngle : 0f, 10, null);
                    Projectile comp = bubble.GetComponent<Projectile>();
                    if (bubble && comp)
                    {
                        comp.Owner = Owner;
                        comp.Shooter = Owner.specRigidbody;

                        bubble.AddComponent<BlankProjModifier>().blankType = EasyBlankType.FULL;

                        DistortionWaveDamager dist = bubble.AddComponent<DistortionWaveDamager>();
                        dist.Damage = 5f;
                        dist.Range = 3.5f;
                        dist.stunDuration = 5f;

                        HomingModifier homing = bubble.GetOrAddComponent<HomingModifier>();
                        homing.AngularVelocity = 1000f;
                        homing.HomingRadius = 1000f;

                        comp.AdjustPlayerProjectileTint(Color.blue, 1);

                        comp.baseData.speed *= 3;
                        comp.UpdateSpeed();
                    }
                    countdownToNextBubble = UnityEngine.Random.Range(10f, 20f);
                }
            }
            base.Update();
        }
    }
}
