using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Dungeonator;
using UnityEngine;
using Alexandria.ItemAPI;

namespace NevernamedsItems
{
    class GoldenArmour : PassiveItem
    {
        public static void Init()
        {
            PickupObject item = ItemSetup.NewItem<GoldenArmour>(
            "Golden Armour",
            "You Can't Take It With You",
            "If the bearer has money, and takes damage that would kill them, the cash has a chance to take the hit instead." + "\n\nGold is a soft metal, and doesn't make for very good protection. Whoever decided to make gold armour probably isn't very smart." + "\n\nActivation chance is equal to cash amount. More than 100 cash guarantees a safety net.",
            "goldenarmour_improved");
            item.quality = PickupObject.ItemQuality.D;
            GoldenArmourID = item.PickupObjectId;

            vfx = VFXToolbox.CreateVFXBundle("GoldenArmourBreak", new IntVector2(32, 22), tk2dBaseSprite.Anchor.MiddleCenter, true, 2f);
        }
        public static int GoldenArmourID;
        public static GameObject vfx;
        private void ModifyIncomingDamage(HealthHaver source, HealthHaver.ModifyDamageEventArgs args)
        {
            if (source.GetCurrentHealth() <= args.InitialDamage && source.Armor <= (Owner.ForceZeroHealthState ? 1 : 0))
            {
                int chance = UnityEngine.Random.Range(1, 101);
                if (Owner.carriedConsumables.Currency >= chance)
                {
                    args.ModifiedDamage = 0f;
                    Owner.PlayEffectOnActor(vfx, new Vector3(0.5f, 0), true, true, false);
                    AkSoundEngine.PostEvent("Play_OBJ_item_purchase_01", base.gameObject);
                    if (source.shakesCameraOnDamage) GameManager.Instance.MainCameraController.DoScreenShake(source.cameraShakeOnDamage, new Vector2?(Owner.specRigidbody.UnitCenter), false);
                    Owner.ForceBlank(25f, 0.5f, false, true, null, true, -1f);

                    //Lose Currency
                    if (Owner.PlayerHasActiveSynergy("Gold Reserves")) { Owner.carriedConsumables.Currency -= UnityEngine.Random.Range(1, Mathf.Min(Owner.carriedConsumables.Currency, chance)); }
                    else { Owner.carriedConsumables.Currency -= chance; }
                    Owner.carriedConsumables.Currency = Math.Max(Owner.carriedConsumables.Currency, 0);

                    if (Owner.PlayerHasActiveSynergy("Zilvered Up"))
                    {
                        LootEngine.GivePrefabToPlayer(PickupObjectDatabase.GetById(120).gameObject, Owner);
                        LootEngine.GivePrefabToPlayer(PickupObjectDatabase.GetById(120).gameObject, Owner);
                    }
                }
            }
        }
        public override void Pickup(PlayerController player)
        {
            if (player && player.healthHaver)
            {
                player.healthHaver.ModifyDamage += ModifyIncomingDamage;
                if (!m_pickedUpThisRun) { LootEngine.GivePrefabToPlayer(PickupObjectDatabase.GetById(120).gameObject, player); }
            }
            base.Pickup(player);
        }
        public override void DisableEffect(PlayerController player)
        {
            if (player && player.healthHaver)
            {
                player.healthHaver.ModifyDamage -= ModifyIncomingDamage;
            }
            base.DisableEffect(player);
        }
    }
}
