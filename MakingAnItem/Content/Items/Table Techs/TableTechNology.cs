using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using System.Collections;
using System.Reflection;
using MonoMod.RuntimeDetour;
using Dungeonator;
using Alexandria.Misc;
using Alexandria.ItemAPI;
using Alexandria.Assetbundle;
using Gungeon;

namespace NevernamedsItems
{
    public class TableTechNology : PassiveItem
    {
        public static void Init()
        {
            PickupObject item = ItemSetup.NewItem<TableTechNology>(
              "Table Tech-Nology",
              "T-Tech",
              "Laser-powered scroll-readers like this one are employed by elderly followers of the way of the flip- in lieu of glasses, which tend to wind up broken.",
              "tabletechnology_icon") as PickupObject;

            item.quality = PickupObject.ItemQuality.C;


            Projectile projectile = ProjectileUtility.SetupProjectile(86);
            BasicBeamController beamComp = projectile.GenerateAnchoredBeamPrefabBundle(
                    "vortexbeam_mid_001",
                    Initialisation.ProjectileCollection,
                    Initialisation.projectileAnimationCollection,
                    "VortexBeam",
                    new Vector2(17, 11),
                    new Vector2(0, -3),
                    impactVFXAnimationName: "CrimsonVortex",
                    impactVFXColliderDimensions: new Vector2(3, 3),
                    impactVFXColliderOffsets: new Vector2(-1, -1)
                    );
            projectile.gameObject.name = "Vortex Beam";
            EmmisiveBeams emission = projectile.gameObject.GetOrAddComponent<EmmisiveBeams>();
            emission.EmissivePower = 50;
            emission.EmissiveColorPower = 50;
            emission.EmissiveColor = new Color(254f / 255f, 144f / 255f, 128f / 255f);

            projectile.baseData.damage = 20f;
            projectile.baseData.force *= 1f;
            projectile.baseData.range *= 5;
            projectile.baseData.speed *= 10f;
            beamComp.boneType = BasicBeamController.BeamBoneType.Straight;

            beamComp.endAudioEvent = "Stop_WPN_All";
            beamComp.startAudioEvent = "Play_WPN_moonscraperLaser_shot_01";

            laser = projectile;

            var vortex = ItemBuilder.SpriteFromBundle("crimsonvortex_001", Initialisation.ProjectileCollection.GetSpriteIdByName("crimsonvortex_001"), Initialisation.ProjectileCollection, new GameObject("Vortex"));
            vortex.SetActive(false);
            FakePrefab.MarkAsFakePrefab(vortex);

            tk2dSpriteAnimator vortexAnimator = vortex.GetOrAddComponent<tk2dSpriteAnimator>();
            vortexAnimator.Library = Initialisation.projectileAnimationCollection;
            vortexAnimator.defaultClipId = Initialisation.projectileAnimationCollection.GetClipIdByName("CrimsonVortex");
            vortexAnimator.DefaultClipId = Initialisation.projectileAnimationCollection.GetClipIdByName("CrimsonVortex");
            vortexAnimator.playAutomatically = true;

            tk2dBaseSprite sp = vortex.GetComponent<tk2dBaseSprite>();
            sp.HeightOffGround = 3;

            sp.usesOverrideMaterial = true;
            sp.renderer.material.shader = ShaderCache.Acquire("Brave/LitTk2dCustomFalloffTiltedCutoutEmissive");
            sp.renderer.material.EnableKeyword("BRIGHTNESS_CLAMP_ON");
            sp.renderer.material.SetFloat("_EmissivePower", 50);
            sp.renderer.material.SetFloat("_EmissiveColorPower", 50);
            sp.renderer.material.SetColor("_EmissiveColor", new Color(254f / 255f, 144f / 255f, 128f / 255f));

            vortex.gameObject.AddComponent<TableVortex>();

            var vortexBody = vortex.GetComponent<tk2dSprite>().SetUpSpeculativeRigidbody(new IntVector2(-1, -2), new IntVector2(3, 3));
            vortexBody.CollideWithTileMap = false;
            vortexBody.CollideWithOthers = false;

            CrimsonVortex = vortex;
            item.SetTag("table_tech");
            //Game.Items.Rename("nn:table_tech-nology", "nn:mr_fahrenheit");
        }
        public static Projectile laser;
        public static GameObject CrimsonVortex;
        public class TableVortex : BraveBehaviour
        {
            private void Start()
            {
                LootEngine.DoDefaultPurplePoof(base.transform.position);
            }
            public MajorBreakable table;
            public PlayerController owner;
            public Vector2 direction;
            public FlippableCover flipper;
            private float timeActive;

            bool started = false;
            private void Update()
            {
                if (specRigidbody) { specRigidbody.Reinitialize(); }
                if (!started && table != null && !table.m_isBroken)
                {
                    //ETGModConsole.Log("Test: " + (gameObject.GetComponent<SpeculativeRigidbody>() is SpeculativeRigidbody body1));
                    Projectile toSpawn = laser;
                    if (owner.PlayerHasActiveSynergy("Flippity Beoooow!") && UnityEngine.Random.value <= 0.2f) { toSpawn = (PickupObjectDatabase.GetById(107) as Gun).DefaultModule.projectiles[0]; }
                    if (owner.PlayerHasActiveSynergy("The Two Tables"))
                    {
                        BeamController b1 = BeamAPI.FreeFireBeamFromAnywhere(toSpawn, owner, gameObject, Vector2.zero, direction.ToAngle() + 35f, 20, true);
                        if (b1 is BasicBeamController) { (b1 as BasicBeamController).reflections++; }
                        BeamController b2 = BeamAPI.FreeFireBeamFromAnywhere(toSpawn, owner, gameObject, Vector2.zero, direction.ToAngle() - 35f, 20, true);
                        if (b2 is BasicBeamController) { (b2 as BasicBeamController).reflections++; }
                    }
                    else
                    {
                        BeamAPI.FreeFireBeamFromAnywhere(toSpawn, owner, gameObject, Vector2.zero, direction.ToAngle(), 20, true);
                    }
                    started = true;
                }
                timeActive += BraveTime.DeltaTime;
                if (!table || table.m_isBroken || timeActive > 20.5f)
                {
                    EndBeam();
                }
            }
            private void EndBeam()
            {
                LootEngine.DoDefaultPurplePoof(base.transform.position);
                UnityEngine.Object.Destroy(base.gameObject);
            }
        }
        public override void Pickup(PlayerController player)
        {
            base.Pickup(player);
            player.OnTableFlipCompleted += this.DoLaser;
        }
        public override void DisableEffect(PlayerController player)
        {
            if (player) player.OnTableFlipCompleted -= this.DoLaser;
            base.DisableEffect(player);
        }
        private void DoLaser(FlippableCover obj)
        {
            Vector2 vector = DungeonData.GetIntVector2FromDirection(obj.DirectionFlipped).ToVector2();

            GameObject instVortex = UnityEngine.Object.Instantiate(CrimsonVortex, obj.sprite.WorldCenter + vector, Quaternion.identity);
            TableVortex comp = instVortex.GetComponent<TableVortex>();
            comp.owner = base.Owner;
            comp.table = obj.GetComponentInChildren<MajorBreakable>();
            comp.direction = vector;
            comp.flipper = obj;
            instVortex.transform.SetParent(obj.transform.GetChild(0));
        }

    }
}