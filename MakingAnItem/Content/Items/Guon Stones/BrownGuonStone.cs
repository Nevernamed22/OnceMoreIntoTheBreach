using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using System.Collections;
using System.Reflection;
using MonoMod.RuntimeDetour;
using Alexandria.ItemAPI;

namespace NevernamedsItems
{
    class BrownGuonStone : AdvancedPlayerOrbitalItem
    {

        public static Hook guonHook;
        public static PlayerOrbital orbitalPrefab;

        public static void Init()
        {
            AdvancedPlayerOrbitalItem item = ItemSetup.NewItem<BrownGuonStone>(
            "Brown Guon Stone",
            "Humble Stone",
            "This simple river rock was given meagre magical abilities by the mad wizard Alben Smallbore as part of his experiments. While it can’t do fancy things like heal it’s bearer’s wounds, slow time, or create black holes, it doesn’t mind." + "\n\nGets excited at the appearance of other relics of a similar calibre to itself.",
            "brownguonstone_icon") as AdvancedPlayerOrbitalItem;
            item.quality = PickupObject.ItemQuality.D;
            item.SetTag("guon_stone");

            BuildPrefab();
            item.OrbitalPrefab = orbitalPrefab;
        }

        public static void BuildPrefab()
        {
            

            if (BrownGuonStone.orbitalPrefab != null) return;
            GameObject prefab = ItemBuilder.SpriteFromBundle("BrownGuonOrbital", Initialisation.itemCollection.GetSpriteIdByName("brownguonstone_ingame"), Initialisation.itemCollection);
            prefab.name = "Brown Guon Orbital";
            var body = prefab.GetComponent<tk2dSprite>().SetUpSpeculativeRigidbody(IntVector2.Zero, new IntVector2(7, 13));
            body.CollideWithTileMap = false;
            body.CollideWithOthers = true;
            body.PrimaryPixelCollider.CollisionLayer = CollisionLayer.EnemyBulletBlocker;

            orbitalPrefab = prefab.AddComponent<PlayerOrbital>();
            orbitalPrefab.motionStyle = PlayerOrbital.OrbitalMotionStyle.ORBIT_PLAYER_ALWAYS;
            orbitalPrefab.shouldRotate = false;
            orbitalPrefab.orbitRadius = 5f;
            orbitalPrefab.orbitDegreesPerSecond = 40f;
            orbitalPrefab.SetOrbitalTier(0);

            GameObject.DontDestroyOnLoad(prefab);
            FakePrefab.MarkAsFakePrefab(prefab);
            prefab.SetActive(false);
        }

        public override void Pickup(PlayerController player)
        {
            guonHook = new Hook(
                typeof(PlayerOrbital).GetMethod("Initialize"),
                typeof(BrownGuonStone).GetMethod("GuonInit")
            );
            GameManager.Instance.OnNewLevelFullyLoaded += this.OnNewFloor;
            Invoke("RecalculateSpeed", 1f);
            base.Pickup(player);
        }
        public static PlayerOrbital brownGuonOrbital;
        public override DebrisObject Drop(PlayerController player)
        {
            guonHook.Dispose();
            GameManager.Instance.OnNewLevelFullyLoaded -= this.OnNewFloor;
            return base.Drop(player);
        }
        public override void OnDestroy()
        {
            guonHook.Dispose();
            GameManager.Instance.OnNewLevelFullyLoaded -= this.OnNewFloor;
            base.OnDestroy();
        }
        private void OnNewFloor()
        {
            RecalculateSpeed();
        }
        public static void GuonInit(Action<PlayerOrbital, PlayerController> orig, PlayerOrbital self, PlayerController player)
        {
            orig(self, player);
            if (self.name == "Brown Guon Orbital(Clone)")
            {
                brownGuonOrbital = self;
            }
        }
        private int currentItems, lastItems;
        private int currentGuns, lastGuns;
        public override void Update()
        {
            if (Owner)
            {
                CheckItems(Owner);
            }

            else { return; }
        }
        private void RecalculateSpeed()
        {
            float orbitalSpeed = 40f;
            brownGuonOrbital.orbitDegreesPerSecond = orbitalSpeed;
            foreach (PassiveItem item in Owner.passiveItems)
            {
                if (item.quality == PickupObject.ItemQuality.D || item.PickupObjectId == 127)
                {
                    orbitalSpeed += 10f;
                }
            }
            foreach (Gun gun in Owner.inventory.AllGuns)
            {
                if (gun.quality == PickupObject.ItemQuality.D)
                {
                    orbitalSpeed += 10f;
                }
            }
            brownGuonOrbital.orbitDegreesPerSecond = orbitalSpeed;
        }
        private void CheckItems(PlayerController player)
        {
            currentItems = player.passiveItems.Count;
            currentGuns = player.inventory.AllGuns.Count;
            bool itemsChanged = currentItems != lastItems;
            bool gunsChanged = currentGuns != lastGuns;
            if (itemsChanged || gunsChanged)
            {
                RecalculateSpeed();
                lastItems = currentItems;
                lastGuns = currentGuns;
            }
        }
    }
}