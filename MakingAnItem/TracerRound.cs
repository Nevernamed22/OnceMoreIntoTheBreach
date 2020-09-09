using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;
using ItemAPI;

namespace NevernamedsItems
{
    public class TracerRound : PassiveItem
    {
        public static void Init()
        {
            //The name of the item
            string itemName = "Tracer Rounds";

            //Refers to an embedded png in the project. Make sure to embed your resources! Google it
            string resourceName = "NevernamedsItems/Resources/tracerrounds_icon";

            //Create new GameObject
            GameObject obj = new GameObject(itemName);

            //Add a PassiveItem component to the object
            var item = obj.AddComponent<TracerRound>();

            //Adds a tk2dSprite component to the object and adds your texture to the item sprite collection
            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);

            //Ammonomicon entry variables
            string shortDesc = "Follow The Red Line";
            string longDesc = "Shots have a chance to leave a trail of fire, marking their exact trajectory."+ "\n\nStandard issue for military training exercises, weapons tests, and really bad assassins.";

            //Adds the item to the gungeon item list, the ammonomicon, the loot table, etc.
            //Do this after ItemBuilder.AddSpriteToObject!
            ItemBuilder.SetupItem(item, shortDesc, longDesc, "nn");

            //Adds the actual passive effect to the item
            item.AddToSubShop(ItemBuilder.ShopType.Trorc);


            //Set the rarity of the item
            item.quality = PickupObject.ItemQuality.D;
            AssetBundle assetBundle = ResourceManager.LoadAssetBundle("shared_auto_001");
            TracerRound.goopDefs = new List<GoopDefinition>();
            foreach (string text in TracerRound.goops)
            {
                GoopDefinition goopDefinition;
                try
                {
                    GameObject gameObject = assetBundle.LoadAsset(text) as GameObject;
                    goopDefinition = gameObject.GetComponent<GoopDefinition>();
                }
                catch
                {
                    goopDefinition = (assetBundle.LoadAsset(text) as GoopDefinition);
                }
                goopDefinition.name = text.Replace("assets/data/goops/", "").Replace(".asset", "");
                TracerRound.goopDefs.Add(goopDefinition);
            }
            List<GoopDefinition> list = TracerRound.goopDefs;
        }
        public void onFired(Projectile bullet, float eventchancescaler)
        {
            if (!Owner.HasPickupID(Gungeon.Game.Items["nn:graceful_goop"].PickupObjectId))
            {
                if (UnityEngine.Random.value < 0.1f || Owner.HasPickupID(661))
                {
                    TrailFireModifier mirrorProjectileModifier = bullet.gameObject.AddComponent<TrailFireModifier>();
                    if (Owner.HasPickupID(275)) mirrorProjectileModifier.goopRadius = 1;
                    else mirrorProjectileModifier.goopRadius = 0.5f;
                    mirrorProjectileModifier.goopType = 0;
                    mirrorProjectileModifier.projectileTint = Color.red;
                    mirrorProjectileModifier.needsToUseGreenFire = false;
                }
            }

            //Camera Synergy
            if (Owner.CurrentGun.PickupObjectId == 481) spawnCameraFirePool(Owner.HasPickupID(Gungeon.Game.Items["nn:graceful_goop"].PickupObjectId));
        }
        private void spawnCameraFirePool(bool needsToUseGreenFire)
        {
            if (needsToUseGreenFire)
            {
                GoopDefinition GreenFireDef = (PickupObjectDatabase.GetById(698) as Gun).DefaultModule.projectiles[0].GetComponent<GoopModifier>().goopDefinition;
                var ddgm = DeadlyDeadlyGoopManager.GetGoopManagerForGoopType(GreenFireDef);
                ddgm.AddGoopCircle(Owner.sprite.WorldCenter, 10);
            }
            else
            {
                TracerRound.goopDefs[0].UsesGreenFire = false;
                var ddgm = DeadlyDeadlyGoopManager.GetGoopManagerForGoopType(TracerRound.goopDefs[0]);
                ddgm.AddGoopCircle(Owner.sprite.WorldCenter, 10);
            }
        }
        private void onFiredBeam(BeamController sourceBeam)
        {

        }
        public override void Pickup(PlayerController player)
        {
            player.PostProcessProjectile += this.onFired;
            player.PostProcessBeam += this.onFiredBeam;
            base.Pickup(player);
        }
        public override DebrisObject Drop(PlayerController player)
        {
            DebrisObject result = base.Drop(player);
            player.PostProcessProjectile -= this.onFired;
            player.PostProcessBeam -= this.onFiredBeam;
            return result;
        }
        protected override void OnDestroy()
        {
            Owner.PostProcessProjectile -= this.onFired;
            Owner.PostProcessBeam -= this.onFiredBeam;
            base.OnDestroy();
        }
        private int currentItems, lastItems;
        private int currentGuns, lastGuns;
        protected override void Update()
        {
            if (Owner)
            {
                CalculateStats(Owner);
            }

            else { return; }
        }
        private void CalculateStats(PlayerController player)
        {
            currentItems = player.passiveItems.Count;
            currentGuns = player.inventory.AllGuns.Count;
            if (currentItems != lastItems || currentGuns != lastGuns)
            {
                bool deservesFireimmunity = false;
                if (Owner.HasPickupID(481) || Owner.HasPickupID(275) || Owner.HasPickupID(661)) deservesFireimmunity = true;
                HandleFireImmunity(deservesFireimmunity);
                lastItems = currentItems;
                lastGuns = currentGuns;
            }
        }
        private void HandleFireImmunity(bool shouldGiveFireImmunity)
        {
            if (shouldGiveFireImmunity)
            {
                this.m_fireImmunity = new DamageTypeModifier();
                this.m_fireImmunity.damageMultiplier = 0f;
                this.m_fireImmunity.damageType = CoreDamageTypes.Fire;
                Owner.healthHaver.damageTypeModifiers.Add(this.m_fireImmunity);
            }
            else
            {
                Owner.healthHaver.damageTypeModifiers.Remove(this.m_fireImmunity);
            }
        }

        private DamageTypeModifier m_fireImmunity;
        private static List<GoopDefinition> goopDefs;

        private static string[] goops = new string[]
        {
            "assets/data/goops/napalmgoopthatworks.asset",
            "assets/data/goops/napalmgoopthatworks.asset",
        };
    }

    /// <summary>
    /// ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// </summary>
    public class TrailFireModifier : MonoBehaviour
    {
        // Token: 0x06007293 RID: 29331 RVA: 0x002CA2F4 File Offset: 0x002C84F4
        public int goopType = 0;
        public float goopRadius = 0.5f;
        public bool needsToUseGreenFire = false;
        public Color projectileTint = Color.white;
        public TrailFireModifier()
        {
            AssetBundle assetBundle = ResourceManager.LoadAssetBundle("shared_auto_001");
            TrailFireModifier.goopDefs = new List<GoopDefinition>();
            foreach (string text in TrailFireModifier.goops)
            {
                GoopDefinition goopDefinition;
                try
                {
                    GameObject gameObject = assetBundle.LoadAsset(text) as GameObject;
                    goopDefinition = gameObject.GetComponent<GoopDefinition>();
                }
                catch
                {
                    goopDefinition = (assetBundle.LoadAsset(text) as GoopDefinition);
                }
                goopDefinition.name = text.Replace("assets/data/goops/", "").Replace(".asset", "");
                TrailFireModifier.goopDefs.Add(goopDefinition);
            }
            List<GoopDefinition> list = TrailFireModifier.goopDefs;
        }

        // Token: 0x06007294 RID: 29332 RVA: 0x002CA328 File Offset: 0x002C8528
        private void Awake()
        {
            this.m_projectile = base.GetComponent<Projectile>();
            this.m_projectile.AdjustPlayerProjectileTint(projectileTint, 1, 0f);
        }

        // Token: 0x06007295 RID: 29333 RVA: 0x002CA3A0 File Offset: 0x002C85A0
        private void Update()
        {
            if (goopType == 0 && needsToUseGreenFire)
            {
                GoopDefinition GreenFireDef = (PickupObjectDatabase.GetById(698) as Gun).DefaultModule.projectiles[0].GetComponent<GoopModifier>().goopDefinition;
                var ddgm = DeadlyDeadlyGoopManager.GetGoopManagerForGoopType(GreenFireDef);
                ddgm.AddGoopCircle(m_projectile.sprite.WorldCenter, goopRadius);
            }
            else
            {
                var ddgm = DeadlyDeadlyGoopManager.GetGoopManagerForGoopType(TrailFireModifier.goopDefs[goopType]);
                ddgm.AddGoopCircle(m_projectile.sprite.WorldCenter, goopRadius);
            }
        }

        private Projectile m_projectile;

        private static List<GoopDefinition> goopDefs;

        private static string[] goops = new string[]
        {
            "assets/data/goops/napalmgoopthatworks.asset",
            "assets/data/goops/poison goop.asset",
            "assets/data/goops/water goop.asset",
            "assets/data/goops/napalmgoopthatworks.asset",
        };
    }

}