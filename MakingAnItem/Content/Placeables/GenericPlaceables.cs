using Alexandria.BreakableAPI;
using Alexandria.DungeonAPI;
using Alexandria.ItemAPI;
using Dungeonator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace NevernamedsItems
{
    class GenericPlaceables
    {
        public static void Init()
        {
            megaStatueBase = SpriteBuilder.SpriteFromResource("NevernamedsItems/Resources/PlaceableObjects/megastatue_base.png", new GameObject("MegaStatue_base"));
            megaStatueBase.SetActive(false);
            FakePrefab.MarkAsFakePrefab(megaStatueBase);
            megaStatueBase.GetComponent<tk2dSprite>().HeightOffGround = 0.1f;
            megaStatueBase.AddComponent<PlacedBlockerConfigurable>();

            var megaStatueBaseBody = megaStatueBase.GetComponent<tk2dSprite>().SetUpSpeculativeRigidbody(new IntVector2(0, -3), new IntVector2(32, 35));
            megaStatueBaseBody.CollideWithTileMap = false;
            megaStatueBaseBody.CollideWithOthers = true;
            megaStatueBaseBody.PrimaryPixelCollider.CollisionLayer = CollisionLayer.HighObstacle;


            var megaStatueShadow = SpriteBuilder.SpriteFromResource("NevernamedsItems/Resources/PlaceableObjects/megastatue_shadow.png", new GameObject("megastatue_shadow"));
            megaStatueShadow.transform.SetParent(megaStatueBase.transform);
            megaStatueShadow.transform.localPosition = new Vector3(-0.1875f, -0.1875f, 50f);
            tk2dSprite shadow = megaStatueShadow.GetComponent<tk2dSprite>();
            shadow.HeightOffGround = 0f;
            shadow.SortingOrder = 0;
            shadow.IsPerpendicular = false;
            shadow.renderer.material.shader = ShaderCache.Acquire("Brave/LitTk2dCustomFalloffTilted");
            shadow.usesOverrideMaterial = true;

            Dictionary<GameObject, float> dict = new Dictionary<GameObject, float>()
            {
                { megaStatueBase.gameObject, 1f },
            };
            DungeonPlaceable placeable = BreakableAPIToolbox.GenerateDungeonPlaceable(dict);
            placeable.isPassable = false;
            placeable.width = 2;
            placeable.height = 2;
            StaticReferences.StoredDungeonPlaceables.Add("megastatue_base", placeable);
            Alexandria.DungeonAPI.StaticReferences.customPlaceables.Add("nn:megastatue_base", placeable);


            
            var megaStatuePoseStatue = SpriteBuilder.SpriteFromResource("NevernamedsItems/Resources/PlaceableObjects/megastatue_pose.png", new GameObject("megastatue_pose"));
            megaStatuePose = FakePrefab.Clone(megaStatueBase);
            megaStatuePoseStatue.transform.SetParent(megaStatuePose.transform);
            megaStatuePoseStatue.transform.localPosition = new Vector3(-2.125f, 0.8125f, 50f);
            megaStatuePoseStatue.GetComponent<tk2dSprite>().HeightOffGround = 10f;
            DungeonPlaceable megastatueposeplaceable = BreakableAPIToolbox.GenerateDungeonPlaceable(new Dictionary<GameObject, float>(){{ megaStatuePose.gameObject, 1f }});
            megastatueposeplaceable.isPassable = false;
            megastatueposeplaceable.width = 2;
            megastatueposeplaceable.height = 2;
            StaticReferences.StoredDungeonPlaceables.Add("megastatue_pose", megastatueposeplaceable);
            Alexandria.DungeonAPI.StaticReferences.customPlaceables.Add("nn:megastatue_pose", megastatueposeplaceable);
            
            var megaStatueDiscusStatue = SpriteBuilder.SpriteFromResource("NevernamedsItems/Resources/PlaceableObjects/megastatue_discus.png", new GameObject("megastatue_discus"));
            megaStatueDiscus = FakePrefab.Clone(megaStatueBase);
            megaStatueDiscusStatue.transform.SetParent(megaStatueDiscus.transform);
            megaStatueDiscusStatue.transform.localPosition = new Vector3(-1f, 1.0625f, 50f);
            megaStatueDiscusStatue.GetComponent<tk2dSprite>().HeightOffGround = 10f;
            DungeonPlaceable megastatueDiscusplaceable = BreakableAPIToolbox.GenerateDungeonPlaceable(new Dictionary<GameObject, float>() { { megaStatueDiscus.gameObject, 1f } });
            megastatueDiscusplaceable.isPassable = false;
            megastatueDiscusplaceable.width = 2;
            megastatueDiscusplaceable.height = 2;
            StaticReferences.StoredDungeonPlaceables.Add("megastatue_discus", megastatueDiscusplaceable);
            Alexandria.DungeonAPI.StaticReferences.customPlaceables.Add("nn:megastatue_discus", megastatueDiscusplaceable);

            var megaStatueShotputStatue = SpriteBuilder.SpriteFromResource("NevernamedsItems/Resources/PlaceableObjects/megastatue_shotput.png", new GameObject("megastatue_shotput"));
            megaStatueShotput = FakePrefab.Clone(megaStatueBase);
            megaStatueShotputStatue.transform.SetParent(megaStatueShotput.transform);
            megaStatueShotputStatue.transform.localPosition = new Vector3(-1.375f, 1.0625f, 50f);
            megaStatueShotputStatue.GetComponent<tk2dSprite>().HeightOffGround = 10f;
            DungeonPlaceable megastatueshotputplaceable = BreakableAPIToolbox.GenerateDungeonPlaceable(new Dictionary<GameObject, float>() { { megaStatueShotput.gameObject, 1f } });
            megastatueshotputplaceable.isPassable = false;
            megastatueshotputplaceable.width = 2;
            megastatueshotputplaceable.height = 2;
            StaticReferences.StoredDungeonPlaceables.Add("megastatue_shotput", megastatueshotputplaceable);
            Alexandria.DungeonAPI.StaticReferences.customPlaceables.Add("nn:megastatue_shotput", megastatueshotputplaceable);

            var megaStatueBrokenStatue = SpriteBuilder.SpriteFromResource("NevernamedsItems/Resources/PlaceableObjects/megastatue_broken.png", new GameObject("megastatue_broken"));
            megaStatueBroken = FakePrefab.Clone(megaStatueBase);
            megaStatueBrokenStatue.transform.SetParent(megaStatueBroken.transform);
            megaStatueBrokenStatue.transform.localPosition = new Vector3(-1f, 1.0625f, 50f);
            megaStatueBrokenStatue.GetComponent<tk2dSprite>().HeightOffGround = 10f;
            DungeonPlaceable megastatuebrokenplaceable = BreakableAPIToolbox.GenerateDungeonPlaceable(new Dictionary<GameObject, float>() { { megaStatueBroken.gameObject, 1f } });
            megastatuebrokenplaceable.isPassable = false;
            megastatuebrokenplaceable.width = 2;
            megastatuebrokenplaceable.height = 2;
            StaticReferences.StoredDungeonPlaceables.Add("megastatue_broken", megastatuebrokenplaceable);
            Alexandria.DungeonAPI.StaticReferences.customPlaceables.Add("nn:megastatue_broken", megastatuebrokenplaceable);


            DungeonPlaceable megastatuerandomplaceable = BreakableAPIToolbox.GenerateDungeonPlaceable(new Dictionary<GameObject, float>() 
            {
                { megaStatuePose.gameObject, 1f },
                { megaStatueShotput.gameObject, 1f },
                { megaStatueDiscus.gameObject, 1f },
                { megaStatueBroken.gameObject, 1f }
            });
            megastatuerandomplaceable.isPassable = false;
            megastatuerandomplaceable.width = 2;
            megastatuerandomplaceable.height = 2;
            StaticReferences.StoredDungeonPlaceables.Add("megastatue_random", megastatuerandomplaceable);
            Alexandria.DungeonAPI.StaticReferences.customPlaceables.Add("nn:megastatue_random", megastatuerandomplaceable);
        }

        public static GameObject megaStatueBase;
        public static GameObject megaStatuePose;
        public static GameObject megaStatueShotput;
        public static GameObject megaStatueDiscus;
        public static GameObject megaStatueBroken;
        public static GameObject megaStatueRandom;
    }
}
