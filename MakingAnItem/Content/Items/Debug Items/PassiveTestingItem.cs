using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Alexandria.ItemAPI;
using Alexandria.Misc;
using UnityEngine;
using Gungeon;
using System.Collections;
using UnityEngine.Events;
using Dungeonator;

namespace NevernamedsItems
{
    class PassiveTestingItem : PassiveItem
    {
        public static void Init()
        {
            PassiveTestingItem item = ItemSetup.NewItem<PassiveTestingItem>(
              "PassiveTestingItem",
              "Work In Progress",
              "Did you seriously give yourself a testing item just to read the flavour text?",
              "workinprogress_icon") as PassiveTestingItem;
            item.quality = PickupObject.ItemQuality.EXCLUDED;
            item.sprite.IsPerpendicular = true;
            item.CanBeDropped = true;
            item.CanBeSold = true;
            DebugPassiveID = item.PickupObjectId;        
        }
        
        public static int DebugPassiveID;
        public void onFired(Projectile bullet, float eventchancescaler)
        {
            numfired++;
            ETGModConsole.Log("Fired bullet; " + numfired);
         /*  bullet.statusEffectsToApply.Add(new GameActorGildedEffect()
            {
                duration = 10,
                stackMode = GameActorEffect.EffectStackingMode.Refresh,
            });

            //Assigns to the tile destroyer
            //bullet.specRigidbody.OnPreTileCollision += OnPreTileCollision;*/
        }
        //Break tiles
        public void OnPreTileCollision(SpeculativeRigidbody myRigidbody, PixelCollider myPixelCollider, PhysicsEngine.Tile tile, PixelCollider tilePixelCollider)
        {
            RoomHandler baseRoom = myRigidbody.projectile.GetAbsoluteRoom();
            IntVector2 position = tile.Position;
            CellData cellData = GameManager.Instance.Dungeon.data[position];
            if (cellData != null )
            {
                cellData.breakable = true;
                cellData.occlusionData.overrideOcclusion = true;
                cellData.occlusionData.cellOcclusionDirty = true;
                tk2dTileMap tilemap = GameManager.Instance.Dungeon.DestroyWallAtPosition(position.x, position.y, true);

                baseRoom.Cells.Add(cellData.position);
                baseRoom.CellsWithoutExits.Add(cellData.position);
                baseRoom.RawCells.Add(cellData.position);
                Pixelator.Instance.MarkOcclusionDirty();
                Pixelator.Instance.ProcessOcclusionChange(baseRoom.Epicenter, 1f, baseRoom, false);
                if (tilemap) { GameManager.Instance.Dungeon.RebuildTilemap(tilemap); }
            }
        }
        public override void Pickup(PlayerController player)
        {
            ETGModConsole.Log("-------------------------------");
            numfired = 0;
            player.PostProcessProjectile += this.onFired;
            base.Pickup(player);
        }
        int numfired = 0;
        public override void Update()
        {
            base.Update();
        }
        public override DebrisObject Drop(PlayerController player)
        {
           player.PostProcessProjectile -= this.onFired;
           DebrisObject result = base.Drop(player);
            return result;
        }

    }
}