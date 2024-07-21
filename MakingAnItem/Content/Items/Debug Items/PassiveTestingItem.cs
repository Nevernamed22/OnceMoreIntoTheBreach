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
            string itemName = "PassiveTestingItem";
            string resourceName = "NevernamedsItems/Resources/workinprogress_icon";
            GameObject obj = new GameObject();
            var item = obj.AddComponent<PassiveTestingItem>();
            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);


            string shortDesc = "wip";
            string longDesc = "Did you seriously give yourself a testing item just to read the flavour text?";


            ItemBuilder.SetupItem(item, shortDesc, longDesc, "nn");

            item.quality = PickupObject.ItemQuality.EXCLUDED;
            item.sprite.IsPerpendicular = true;
            item.CanBeDropped = true;
            item.CanBeSold = true;
            DebugPassiveID = item.PickupObjectId;

           
        }
        
        public static int DebugPassiveID;
        public void onFired(Projectile bullet, float eventchancescaler)
        {
            /* if (!bullet.GetComponent<HasBeenDoubleProcessed>())
             {
                 StartCoroutine(doLateFrameProcessing(bullet));
                 bullet.gameObject.AddComponent<HasBeenDoubleProcessed>();
             }*/
            // if (bullet.sprite) bullet.sprite.renderer.enabled = false;
            bullet.baseData.range = 10;
            //bullet.specRigidbody.OnPreTileCollision += OnPreTileCollision;
        }
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
        private IEnumerator doLateFrameProcessing(Projectile projectile)
        {
            yield return null;
            //if (projectile.ProjectilePlayerOwner()) projectile.ProjectilePlayerOwner().DoPostProcessProjectile(projectile);
            yield break;
        }
        private void PostProcessBeam(BeamController beam)
        {
            if (beam.GetComponent<BeamSplittingModifier>())
            {
                ETGModConsole.Log("Split found");
                beam.GetComponent<BeamSplittingModifier>().amtToSplitTo++;
            }
        }
        private void OnRoll(PlayerController player, Vector2 vec)
        {
            ETGModConsole.Log(player.specRigidbody.UnitCenter.ToString());
        }
        public override void Pickup(PlayerController player)
        {
            //player.SetIsStealthed(true, "blehp");
            player.PostProcessProjectile += this.onFired;
            //player.PostProcessThrownGun += PostProcessGun;
            // player.OnRollStarted += OnRoll;
            // player.PostProcessBeam += this.PostProcessBeam;
            base.Pickup(player);
        }
        private void PostProcessGun(Projectile fucker)
        {
        }
        public float num = 0;
        public override void Update()
        {
            if (num < 0.25f) { num += BraveTime.DeltaTime; }
            else
            {
                if (Owner && Owner.CurrentGun)
                {
                    Gun g = Owner.CurrentGun;
                    float dps = (g.DefaultModule.numberOfShotsInClip * (g.DefaultModule.projectiles[0].baseData.damage * g.Volley.projectiles.Count())) / ((g.DefaultModule.numberOfShotsInClip - 1) * g.DefaultModule.cooldownTime + g.reloadTime);
                   // VFXToolbox.DoRisingStringFade(dps.ToString(), Owner.CurrentGun.barrelOffset.position, Color.red);
                }
                num = 0;
            }
            /*if (Owner && Owner.SuperAutoAimTarget != null)
            {
                Instantiate(EasyVFXDatabase.GreenLaserCircleVFX, Owner.SuperAutoAimTarget.AimCenter, Quaternion.identity);
            }
            if (Owner && Owner.SuperDuperAimTarget != null)
            {
                Instantiate(EasyVFXDatabase.BlueLaserCircleVFX, Owner.SuperAutoAimTarget.AimCenter, Quaternion.identity);
            }*/
            base.Update();
        }
        public override DebrisObject Drop(PlayerController player)
        {
           // player.PostProcessThrownGun -= PostProcessGun;
           // player.PostProcessProjectile -= this.onFired;
            // player.PostProcessBeam -= this.PostProcessBeam;

            DebrisObject result = base.Drop(player);
            return result;
        }
        public class HasBeenDoubleProcessed : MonoBehaviour { }

    }
    public class JamPlayerBulletModifier : MonoBehaviour
    {
        private void Awake()
        {
            this.m_projectile = base.GetComponent<Projectile>();
        }
        private void Update()
        {
            if (!m_projectile.IsBlackBullet)
            {
                m_projectile.BecomeBlackBullet();
            }
        }
        private Projectile m_projectile;
    }
}