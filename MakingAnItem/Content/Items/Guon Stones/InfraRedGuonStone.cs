﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using System.Collections;
using System.Reflection;
using MonoMod.RuntimeDetour;
using Alexandria.Misc;
using Alexandria.ItemAPI;

namespace NevernamedsItems
{
    class InfraredGuonStone : AdvancedPlayerOrbitalItem
    {

        public static PlayerOrbital orbitalPrefab;
        public static PlayerOrbital upgradeOrbitalPrefab;
        public static void Init()
        {
            AdvancedPlayerOrbitalItem item = ItemSetup.NewItem<InfraredGuonStone>(
            "Infrared Guon Stone",
            "Under the Radar",
            "Expels photonic exhaust away from the user." + "\n\nStand well back.",
            "infraredguonstone_icon") as AdvancedPlayerOrbitalItem;
            item.quality = PickupObject.ItemQuality.C;

            BuildPrefab();
            item.OrbitalPrefab = orbitalPrefab;
            BuildSynergyPrefab();

            item.HasAdvancedUpgradeSynergy = true;
            item.AdvancedUpgradeSynergy = "Infraredder Guon Stone";
            item.AdvancedUpgradeOrbitalPrefab = InfraredGuonStone.upgradeOrbitalPrefab.gameObject;
            item.SetTag("guon_stone");

            List<string> BeamAnimPaths = new List<string>()
            {
                "NevernamedsItems/Resources/BeamSprites/redbeam_seg_001",
                "NevernamedsItems/Resources/BeamSprites/redbeam_seg_002",
                "NevernamedsItems/Resources/BeamSprites/redbeam_seg_003",
                "NevernamedsItems/Resources/BeamSprites/redbeam_seg_004"
            };
            List<string> ImpactAnimPaths = new List<string>()
            {
                "NevernamedsItems/Resources/BeamSprites/redbeam_impact_001",
                "NevernamedsItems/Resources/BeamSprites/redbeam_impact_002",
                "NevernamedsItems/Resources/BeamSprites/redbeam_impact_003",
                "NevernamedsItems/Resources/BeamSprites/redbeam_impact_004",
            };

            Projectile projectile = ProjectileUtility.SetupProjectile(86);

            BasicBeamController beamComp = projectile.GenerateBeamPrefab(
                "NevernamedsItems/Resources/BeamSprites/redbeam_seg_001", new Vector2(18, 2), new Vector2(0, 8), BeamAnimPaths, 8,
                ImpactAnimPaths, 13, new Vector2(4, 4), new Vector2(7, 7));

            projectile.baseData.damage = 50;
            projectile.baseData.range = 100;
            projectile.baseData.speed = 25;
            beamComp.boneType = BasicBeamController.BeamBoneType.Straight;
            beamComp.interpolateStretchedBones = false;
            beamComp.endAudioEvent = "Stop_WPN_All";
            beamComp.startAudioEvent = "Play_WPN_radiationlaser_shot_01";
            InfraredBeam = projectile;
        }
        public static Projectile InfraredBeam;
        public static void BuildPrefab()
        {
            if (InfraredGuonStone.orbitalPrefab != null) return;
            GameObject prefab = ItemBuilder.SpriteFromBundle("InfraredGuonOrbital", Initialisation.itemCollection.GetSpriteIdByName("infraredguonstone_ingame"), Initialisation.itemCollection);
            prefab.name = "Infrared Guon Orbital";
            var body = prefab.GetComponent<tk2dSprite>().SetUpSpeculativeRigidbody(IntVector2.Zero, new IntVector2(7, 7));
            body.CollideWithTileMap = false;
            body.CollideWithOthers = true;
            body.PrimaryPixelCollider.CollisionLayer = CollisionLayer.EnemyBulletBlocker;

            orbitalPrefab = prefab.AddComponent<PlayerOrbital>();
            orbitalPrefab.motionStyle = PlayerOrbital.OrbitalMotionStyle.ORBIT_PLAYER_ALWAYS;
            orbitalPrefab.shouldRotate = false;
            orbitalPrefab.perfectOrbitalFactor = 10f;
            orbitalPrefab.orbitRadius = 2.5f;
            orbitalPrefab.orbitDegreesPerSecond = 120f;
            orbitalPrefab.SetOrbitalTier(0);

            GameObject.DontDestroyOnLoad(prefab);
            FakePrefab.MarkAsFakePrefab(prefab);
            prefab.SetActive(false);
        }
        public static void BuildSynergyPrefab()
        {
            if (InfraredGuonStone.upgradeOrbitalPrefab == null)
            {
                GameObject gameObject = ItemBuilder.SpriteFromBundle("InfraredGuonOrbitalSynergy", Initialisation.itemCollection.GetSpriteIdByName("infraredguonstone_synergy"), Initialisation.itemCollection);
                gameObject.name = "Infrared Guon Orbital Synergy Form";
                SpeculativeRigidbody speculativeRigidbody = gameObject.GetComponent<tk2dSprite>().SetUpSpeculativeRigidbody(IntVector2.Zero, new IntVector2(10, 10));
                InfraredGuonStone.upgradeOrbitalPrefab = gameObject.AddComponent<PlayerOrbital>();
                speculativeRigidbody.CollideWithTileMap = false;
                speculativeRigidbody.CollideWithOthers = true;
                speculativeRigidbody.PrimaryPixelCollider.CollisionLayer = CollisionLayer.EnemyBulletBlocker;
                InfraredGuonStone.upgradeOrbitalPrefab.shouldRotate = false;
                InfraredGuonStone.upgradeOrbitalPrefab.orbitRadius = 2.5f;
                InfraredGuonStone.upgradeOrbitalPrefab.orbitDegreesPerSecond = 60f;
                InfraredGuonStone.upgradeOrbitalPrefab.perfectOrbitalFactor = 10f;
                InfraredGuonStone.upgradeOrbitalPrefab.SetOrbitalTier(0);

                UnityEngine.Object.DontDestroyOnLoad(gameObject);
                FakePrefab.MarkAsFakePrefab(gameObject);
                gameObject.SetActive(false);
            }
        }
        private BeamController extantBeam;
        public override void Update()
        {
            if (this.m_extantOrbital != null & Owner)
            {
                if (Owner.IsInCombat && extantBeam == null)
                {
                    extantBeam = BeamAPI.FreeFireBeamFromAnywhere(InfraredBeam, Owner, this.m_extantOrbital, Vector2.zero,  0, float.MaxValue);
                    extantBeam.projectile.gameObject.AddComponent<AlwaysPointAwayFromPlayerBeam>();
                    if (Owner.PlayerHasActiveSynergy("Infraredder Guon Stone")) extantBeam.projectile.baseData.damage *= 2;
                    if (Owner.PlayerHasActiveSynergy("Xenochrome"))
                    {
                        extantBeam.GetComponent<BasicBeamController>().penetration++;
                        extantBeam.GetComponent<BasicBeamController>().reflections++;
                    }
                }

                else if (!Owner.IsInCombat && extantBeam != null)
                {
                    extantBeam.CeaseAttack();
                    extantBeam = null;
                }
            }
            else if (((m_extantOrbital == null) || (Owner == null)) && extantBeam != null)
            {
                extantBeam.CeaseAttack();
                extantBeam = null;
            }
            base.Update();
        }
        public GameObject GimmeOrbital()
        {
            if (m_extantOrbital) return m_extantOrbital;
            else return null;
        }
        public override void Pickup(PlayerController player)
        {
            base.Pickup(player);
        }
        public override DebrisObject Drop(PlayerController player)
        {
            if (extantBeam)
            {
                extantBeam.CeaseAttack();
                extantBeam = null;
            }
            return base.Drop(player);
        }
        public override void OnDestroy()
        {
            if (extantBeam)
            {
                extantBeam.CeaseAttack();
                extantBeam = null;
            }
            base.OnDestroy();
        }
    }
}