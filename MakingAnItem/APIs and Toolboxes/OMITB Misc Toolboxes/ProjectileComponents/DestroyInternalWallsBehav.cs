
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Alexandria.Misc;
using HarmonyLib;
using Dungeonator;

namespace NevernamedsItems
{
    public class DestroyInternalWallsBehav : MonoBehaviour
    {
        private void Start()
        {
            m_projectile = base.GetComponent<Projectile>();
            body = m_projectile.GetComponent<SpeculativeRigidbody>();
            body.OnPreTileCollision += OnPreTileCollision;
        }
        public void OnPreTileCollision(SpeculativeRigidbody myRigidbody, PixelCollider myPixelCollider, PhysicsEngine.Tile tile, PixelCollider tilePixelCollider)
        {
            RoomHandler baseRoom = m_projectile.GetAbsoluteRoom();
            IntVector2 position = tile.Position;
            CellData cellData = GameManager.Instance.Dungeon.data[position];
            if (cellData != null && cellData.isRoomInternal)
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
        public Projectile m_projectile;
        public SpeculativeRigidbody body;
    }
}
