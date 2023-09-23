using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using System.Collections;
using System.Reflection;
using MonoMod.RuntimeDetour;
using Dungeonator;
using SaveAPI;
using Alexandria.ItemAPI;
using Alexandria.Misc;

namespace NevernamedsItems
{
    class LimeGuonStone : AdvancedPlayerOrbitalItem
    {

        public static void Init()
        {
            AdvancedPlayerOrbitalItem item = ItemSetup.NewItem<LimeGuonStone>(
            "Lime Guon Stone",
            "Bright and Trig",
            "Releases orbital energy when struck. \n\nThis guon stone has been somewhat overenchanted, and is unable to fully constrain all of it's rotational magic.",
            "limeguonstone_icon") as AdvancedPlayerOrbitalItem;
            item.quality = PickupObject.ItemQuality.C;

            BuildPrefab();
            item.OrbitalPrefab = orbitalPrefab;
            BuildSynergyPrefab();

            item.SetTag("guon_stone");

            item.HasAdvancedUpgradeSynergy = true;
            item.AdvancedUpgradeSynergy = "Limer Guon Stone";
            item.AdvancedUpgradeOrbitalPrefab = upgradeOrbitalPrefab.gameObject;

            orbitalShot = ((Gun)PickupObjectDatabase.GetById(86)).DefaultModule.projectiles[0].InstantiateAndFakeprefab();
            orbitalShot.SetProjectileSpriteRight("limebullet", 5, 5, true, tk2dBaseSprite.Anchor.MiddleCenter, 3, 3);
            orbitalShot.baseData.range = 1000;
            orbitalShot.gameObject.AddComponent<PierceProjModifier>();
            orbitalShot.gameObject.AddComponent<PierceDeadActors>();
        }
        public static PlayerOrbital orbitalPrefab;
        public static PlayerOrbital upgradeOrbitalPrefab;
        public static Projectile orbitalShot;
        public static void BuildPrefab()
        {
            if (orbitalPrefab != null) return;
            GameObject prefab = ItemBuilder.SpriteFromBundle("LimeGuonOrbital", Initialisation.itemCollection.GetSpriteIdByName("limeguonstone_ingame"), Initialisation.itemCollection);
            prefab.name = "Lime Guon Orbital";
            var body = prefab.GetComponent<tk2dSprite>().SetUpSpeculativeRigidbody(IntVector2.Zero, new IntVector2(8, 8));

            body.CollideWithTileMap = false;
            body.CollideWithOthers = true;
            body.PrimaryPixelCollider.CollisionLayer = CollisionLayer.EnemyBulletBlocker;
            prefab.AddComponent<LimeGuonStoneController>();

            orbitalPrefab = prefab.AddComponent<PlayerOrbital>();
            orbitalPrefab.motionStyle = PlayerOrbital.OrbitalMotionStyle.ORBIT_PLAYER_ALWAYS;
            orbitalPrefab.perfectOrbitalFactor = 0f;
            orbitalPrefab.shouldRotate = false;
            orbitalPrefab.orbitRadius = 2.5f;
            orbitalPrefab.orbitDegreesPerSecond = 120f;
            orbitalPrefab.SetOrbitalTier(0);


            prefab.MakeFakePrefab();
        }       
        public static void BuildSynergyPrefab()
        {
            if (upgradeOrbitalPrefab != null) return;
            GameObject gameObject = ItemBuilder.SpriteFromBundle("LimeGuonOrbitalSynergy", Initialisation.itemCollection.GetSpriteIdByName("limeguonstone_synergy"), Initialisation.itemCollection);
            gameObject.name = "Lime Guon Orbital Synergy Form";
            SpeculativeRigidbody speculativeRigidbody = gameObject.GetComponent<tk2dSprite>().SetUpSpeculativeRigidbody(IntVector2.Zero, new IntVector2(12, 12));
            gameObject.AddComponent<LimeGuonStoneController>();

            upgradeOrbitalPrefab = gameObject.AddComponent<PlayerOrbital>();
            speculativeRigidbody.CollideWithTileMap = false;
            speculativeRigidbody.CollideWithOthers = true;
            speculativeRigidbody.PrimaryPixelCollider.CollisionLayer = CollisionLayer.EnemyBulletBlocker;
            upgradeOrbitalPrefab.shouldRotate = false;
            upgradeOrbitalPrefab.orbitRadius = 2.5f;
            upgradeOrbitalPrefab.perfectOrbitalFactor = 10f;
            upgradeOrbitalPrefab.orbitDegreesPerSecond = 120f;
            upgradeOrbitalPrefab.SetOrbitalTier(0);


            gameObject.MakeFakePrefab();


        }
    }
    public class LimeGuonStoneController : MonoBehaviour
    {
        private void Start()
        {
            self = base.GetComponent<PlayerOrbital>();
            rigidbody = base.GetComponent<SpeculativeRigidbody>();
            owner = base.GetComponent<PlayerOrbital>().Owner;

            rigidbody.OnPreRigidbodyCollision += OnGuonHitByBullet;         
        }
        public List<Projectile> registered = new List<Projectile>();
        public List<Projectile> orbiters = new List<Projectile>();
        private void OnGuonHitByBullet(SpeculativeRigidbody myRigidbody, PixelCollider myCollider, SpeculativeRigidbody other, PixelCollider otherCollider)
        {
            if (owner && other.projectile && other.projectile.Owner && !(other.projectile.Owner is PlayerController) && !registered.Contains(other.projectile))
            {
                registered.Add(other.projectile);
                int num = 1;
                if (owner.PlayerHasActiveSynergy("Limer Guon Stone")) num++;
                for (int i = 0; i < num; i++)
                {
                    int orbitersInGroup = OrbitProjectileMotionModule.GetOrbitersInGroup(-138760);
                    if (orbitersInGroup >= 30) { return; }

                    Projectile newBullet = LimeGuonStone.orbitalShot.InstantiateAndFireInDirection(rigidbody.UnitCenter, UnityEngine.Random.insideUnitCircle.ToAngle(), 0, null).GetComponent<Projectile>();

                    newBullet.specRigidbody.CollideWithTileMap = false;

                    OrbitProjectileMotionModule orbitProjectileMotionModule = new OrbitProjectileMotionModule();
                    orbitProjectileMotionModule.lifespan = 150;
                    orbitProjectileMotionModule.MinRadius = 2f;
                    orbitProjectileMotionModule.MaxRadius = 5f;
                    orbitProjectileMotionModule.usesAlternateOrbitTarget = true;
                    orbitProjectileMotionModule.OrbitGroup = -138760;
                    orbitProjectileMotionModule.alternateOrbitTarget = rigidbody;
                    if (newBullet.OverrideMotionModule != null && newBullet.OverrideMotionModule is HelixProjectileMotionModule)
                    {
                        orbitProjectileMotionModule.StackHelix = true;
                        orbitProjectileMotionModule.ForceInvert = (newBullet.OverrideMotionModule as HelixProjectileMotionModule).ForceInvert;
                    }
                    newBullet.OverrideMotionModule = orbitProjectileMotionModule;



                    newBullet.Owner = owner;
                    newBullet.Shooter = owner.specRigidbody;
                    newBullet.baseData.damage *= owner.stats.GetStatValue(PlayerStats.StatType.Damage);
                    newBullet.baseData.speed *= owner.stats.GetStatValue(PlayerStats.StatType.ProjectileSpeed);
                    newBullet.baseData.force *= owner.stats.GetStatValue(PlayerStats.StatType.KnockbackMultiplier);
                    newBullet.BossDamageMultiplier *= owner.stats.GetStatValue(PlayerStats.StatType.DamageToBosses);
                    owner.DoPostProcessProjectile(newBullet); 

                    orbiters.Add(newBullet);
                }
            }
        }
        private void OnDestroy()
        {
            for (int i = orbiters.Count - 1; i >= 0; i--)
            {
                if (orbiters[i] != null) { orbiters[i].DieInAir(); }
            }
            orbiters.Clear();
        }
        private PlayerOrbital self;
        private SpeculativeRigidbody rigidbody;
        private PlayerController owner;
    }
}
