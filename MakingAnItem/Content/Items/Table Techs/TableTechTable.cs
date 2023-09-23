using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Dungeonator;
using Alexandria.ItemAPI;

namespace NevernamedsItems
{
    public class TableTechTable : TableFlipItem
    {
        public static void Init()
        {
            TableFlipItem item = ItemSetup.NewItem<TableTechTable>(
              "Table Tech Table",
              "Flip Recursion",
              "This ancient technique has a chance to create a new table whenever a table is flipped." + "\n\nChapter 8 of the \"Tabla Sutra.\" Flip unto flip unto flip unto flip unto flip unto flip unto flip unto flip unto flip. Never stop flipping.",
              "tabletechtable_icon") as TableFlipItem;
            item.quality = PickupObject.ItemQuality.C;
            item.SetTag("table_tech");
        }
        public override void Pickup(PlayerController player)
        {
            base.Pickup(player);
            player.OnTableFlipped = (Action<FlippableCover>)Delegate.Combine(player.OnTableFlipped, new Action<FlippableCover>(this.SpeedEffect));
        }
        public override DebrisObject Drop(PlayerController player)
        {
            DebrisObject result = base.Drop(player);
            player.OnTableFlipped = (Action<FlippableCover>)Delegate.Remove(player.OnTableFlipped, new Action<FlippableCover>(this.SpeedEffect));
            return result;
        }
        public override void OnDestroy()
        {
            if (Owner)
            {
                Owner.OnTableFlipped -= SpeedEffect;
            }
            base.OnDestroy();
        }

        public FlippableCover TableToSpawn;
        private void SpeedEffect(FlippableCover obj)
        {
            if (UnityEngine.Random.value < .50f)
            {
                Vector2 nearbyPoint = Owner.CenterPosition + (Owner.unadjustedAimPoint.XY() - Owner.CenterPosition).normalized;
                IntVector2? nearestAvailableCell = Owner.CurrentRoom.GetNearestAvailableCell(nearbyPoint, new IntVector2?(IntVector2.One), new CellTypes?(CellTypes.FLOOR), false, null);
                FoldingTableItem component6 = PickupObjectDatabase.GetById(644).GetComponent<FoldingTableItem>();
                GameObject gameObject5 = component6.TableToSpawn.gameObject;
                GameObject gameObject6 = UnityEngine.Object.Instantiate<GameObject>(gameObject5.gameObject, nearestAvailableCell.Value.ToVector2(), Quaternion.identity);
                SpeculativeRigidbody componentInChildren = gameObject6.GetComponentInChildren<SpeculativeRigidbody>();
                FlippableCover component7 = gameObject6.GetComponent<FlippableCover>();
                component7.transform.position.XY().GetAbsoluteRoom().RegisterInteractable(component7);
                component7.ConfigureOnPlacement(component7.transform.position.XY().GetAbsoluteRoom());
                componentInChildren.Initialize();
                PhysicsEngine.Instance.RegisterOverlappingGhostCollisionExceptions(componentInChildren, null, false);
            }
        }
    }
}
