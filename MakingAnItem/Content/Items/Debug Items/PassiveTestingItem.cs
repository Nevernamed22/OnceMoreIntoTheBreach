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
using HarmonyLib;
using System.Collections.ObjectModel;

namespace NevernamedsItems
{
    [HarmonyPatch(typeof(MajorBreakable))]
    [HarmonyPatch("Break", MethodType.Normal)]

    [HarmonyPatch]
    public class MajorBreakableBreakCatcher
    {
        [HarmonyPrefix]
        public static void HarmonyPrefix(MajorBreakable __instance, Vector2 sourceDirection)
        {

            if (__instance != null && !__instance.m_isBroken && __instance.gameObject && __instance.gameObject.GetComponentInParent<FlippableCover>() != null)
            {
                if (PassiveTestingItem.TableBroken != null)
                {

                    PassiveTestingItem.TableBroken(__instance.gameObject);
                }
            }
        }
    }

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
        public static Action<GameObject> TableBroken;

        public static int DebugPassiveID;
        public void onFired(Projectile bullet, float eventchancescaler)
        {
            //numfired++;
            // ETGModConsole.Log("Fired bullet; " + numfired);
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
            if (cellData != null)
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
            TableBroken += BreakAllTables;
            //ETGModConsole.Log("-------------------------------");
            //numfired = 0;
            //player.PostProcessProjectile += this.onFired;
            base.Pickup(player);
        }

        public void BreakAllTables(GameObject table)
        {
            base.StartCoroutine(SequentialBreak(table));
        }
        public IEnumerator SequentialBreak(GameObject table)
        {
                if (table.GetComponent<BrokenByBreakFlocking>()) { yield break; }
                table.AddComponent<BrokenByBreakFlocking>();        

            RoomHandler currentRoom = base.Owner.CurrentRoom;
            ReadOnlyCollection<IPlayerInteractable> roomInteractables = currentRoom.GetRoomInteractables();
            List<FlippableCover> tables = new List<FlippableCover>();

            List<MajorBreakable> allMajorBreakables = StaticReferenceManager.AllMajorBreakables;
            for (int k = allMajorBreakables.Count - 1; k >= 0; k--)
            {
                MajorBreakable indiv = allMajorBreakables[k];

                if (indiv.gameObject && indiv.gameObject.transform.parent && indiv.gameObject.transform.parent.gameObject)
                {
                    GameObject parent = indiv.gameObject.transform.parent.gameObject;

                    if (parent && parent.GetComponent<FlippableCover>() && indiv.transform.position.GetAbsoluteRoom() == currentRoom)
                    {
                        tables.Add(parent.GetComponent<FlippableCover>());
                    }
                }
            }

            if (Owner.passiveItems.Exists(x => x is TableFlipItem && (x as TableFlipItem).TableFlocking))
            {
                for (int i = 0; i < roomInteractables.Count; i++)
                {
                    if (currentRoom.IsRegistered(roomInteractables[i]))
                    {
                        FlippableCover flippableCover = roomInteractables[i] as FlippableCover;
                        if (flippableCover != null && !flippableCover.IsFlipped)
                        {
                            if (flippableCover.flipStyle == FlippableCover.FlipStyle.ANY)
                            {
                                List<DungeonData.Direction> dirs = new List<DungeonData.Direction>() { DungeonData.Direction.NORTH, DungeonData.Direction.SOUTH, DungeonData.Direction.EAST, DungeonData.Direction.WEST };
                                flippableCover.ForceSetFlipper(base.Owner);
                                flippableCover.Flip(BraveUtility.RandomElement(dirs));
                            }
                            else if (flippableCover.flipStyle == FlippableCover.FlipStyle.ONLY_FLIPS_LEFT_RIGHT)
                            {
                                flippableCover.ForceSetFlipper(base.Owner);
                                flippableCover.Flip((UnityEngine.Random.value <= 0.5f) ? DungeonData.Direction.WEST : DungeonData.Direction.EAST);
                            }
                            else if (flippableCover.flipStyle == FlippableCover.FlipStyle.ONLY_FLIPS_UP_DOWN)
                            {

                                flippableCover.ForceSetFlipper(base.Owner);
                                flippableCover.Flip((UnityEngine.Random.value <= 0.5f) ? DungeonData.Direction.NORTH : DungeonData.Direction.SOUTH);
                            }
                        }
                    }
                }
                yield return new WaitForSeconds(2f);
            }

            for (int i = 0; i < tables.Count; i++)
            {
                if (tables[i] != null && !tables[i].IsBroken)
                {
                    MajorBreakable breakable = tables[i].GetComponentInChildren<MajorBreakable>();
                    breakable.gameObject.AddComponent<BrokenByBreakFlocking>();
                    breakable.Break(Vector2.zero);
                }
            }

            yield break;
        }
        public class BrokenByBreakFlocking : MonoBehaviour { };

        int numfired = 0;
        public override void Update()
        {
            base.Update();
        }
        public override DebrisObject Drop(PlayerController player)
        {
            TableBroken -= BreakAllTables;
            //player.PostProcessProjectile -= this.onFired;
            DebrisObject result = base.Drop(player);
            return result;
        }

    }
}