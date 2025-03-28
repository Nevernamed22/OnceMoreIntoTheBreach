using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using UnityEngine;
using Alexandria.ItemAPI;
using Alexandria.Misc;
using Dungeonator;

namespace NevernamedsItems
{
    class Pyromania : PassiveItem
    {
        public static void Init()
        {
            PassiveItem item = ItemSetup.NewItem<Pyromania>(
               "Pyromania",
               "Reign of Fire",
               "Spawns additional explosive barrels in each room." + "\n\nSome people just want to watch the world burn. And you're one of them.",
               "pyromania_icon") as PassiveItem;
            item.quality = PickupObject.ItemQuality.D;
            ID = item.PickupObjectId;
        }
        public static int ID;
        public override void Pickup(PlayerController player)
        {
            player.OnEnteredCombat += this.OnEnteredCombat;
            base.Pickup(player);
        }
        public override void DisableEffect(PlayerController player)
        {
            player.OnEnteredCombat -= this.OnEnteredCombat;
            base.DisableEffect(player);
        }
        private void OnEnteredCombat()
        {
            if (Owner && Owner.CurrentRoom != null) StartCoroutine(placeBarrels());
        }
        private IEnumerator placeBarrels()
        {
            int num = 5;
            if (Owner.PlayerHasActiveSynergy("Meet The Pyro")) { num += 2; }
            RoomHandler room = Owner.CurrentRoom;

            yield return new WaitForSeconds(1f);
            for (int j = 0; j < 5; j++)
            {
                IntVector2 position = room.GetRandomVisibleClearSpot(2, 2);
                GameObject barrel = UnityEngine.Object.Instantiate<GameObject>(EasyPlaceableObjects.ExplosiveBarrel, position.ToVector3(), Quaternion.identity);

                SpeculativeRigidbody ObjectSpecRigidBody = barrel.GetComponentInChildren<SpeculativeRigidbody>();
                Component[] componentsInChildren = barrel.GetComponentsInChildren(typeof(IPlayerInteractable));
                for (int i = 0; i < componentsInChildren.Length; i++)
                {
                    IPlayerInteractable interactable = componentsInChildren[i] as IPlayerInteractable;
                    if (interactable != null) { room.RegisterInteractable(interactable); }
                }
                Component[] componentsInChildren2 = barrel.GetComponentsInChildren(typeof(IPlaceConfigurable));
                for (int i = 0; i < componentsInChildren2.Length; i++)
                {
                    IPlaceConfigurable placeConfigurable = componentsInChildren2[i] as IPlaceConfigurable;
                    if (placeConfigurable != null) { placeConfigurable.ConfigureOnPlacement(room); }
                }

                ObjectSpecRigidBody.Initialize();
                PhysicsEngine.Instance.RegisterOverlappingGhostCollisionExceptions(ObjectSpecRigidBody, null, false);

                //ETGModConsole.Log("log1");
                UnityEngine.Object.Instantiate<GameObject>((PickupObjectDatabase.GetById(328) as Gun).DefaultModule.chargeProjectiles[0].Projectile.hitEffects.overrideMidairDeathVFX, position.ToVector2() + new Vector2(0.5f, 0.5f), Quaternion.identity);
                

                yield return new WaitForSeconds(0.1f);
            }
            yield break;
        }
    }
}