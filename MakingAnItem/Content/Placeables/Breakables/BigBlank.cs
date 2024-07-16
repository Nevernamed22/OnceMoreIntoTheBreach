using Alexandria.BreakableAPI;
using Alexandria.DungeonAPI;
using Alexandria.ItemAPI;
using Alexandria.Misc;
using Dungeonator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace NevernamedsItems
{
    public class BigBlank : BraveBehaviour
    {
        public static GameObject vfx;
        public static void Init()
        {
            vfx = VFXToolbox.CreateVFXBundle("BigBlankBurst", new IntVector2(40, 38), tk2dBaseSprite.Anchor.LowerLeft, true, 2f);
            MinorBreakable breakable = Breakables.GenerateMinorBreakable("Big_Blank", Initialisation.EnvironmentCollection, Initialisation.environmentAnimationCollection,
                "bigblank_idle_001",
                "bigblank_idle",
                "bigblank_break",
                "Play_OBJ_silenceblank_use_01",
                10, 10,
                8, 2,
                vfx,
                null);

            breakable.gameObject.MakeFakePrefab();

            var shadowobj = ItemBuilder.SpriteFromBundle("genericbarrel_shadow_001", Initialisation.EnvironmentCollection.GetSpriteIdByName("genericbarrel_shadow_001"), Initialisation.EnvironmentCollection, new GameObject("Shadow"));
            shadowobj.transform.SetParent(breakable.transform);
            shadowobj.transform.localPosition = new Vector3(4f/16f,0);
            tk2dSprite shadow = shadowobj.GetComponent<tk2dSprite>();
            shadow.HeightOffGround = -5f;
            shadow.SortingOrder = 0;
            shadow.IsPerpendicular = false;
            shadow.renderer.material.shader = ShaderCache.Acquire("Brave/LitTk2dCustomFalloffTilted");
            shadow.usesOverrideMaterial = true;

            breakable.gameObject.AddComponent<PlacedBlockerConfigurable>();

            breakable.stopsBullets = true;
            breakable.OnlyPlayerProjectilesCanBreak = true;
            breakable.OnlyBreaksOnScreen = true;
            breakable.resistsExplosions = false;
            breakable.canSpawnFairy = false;
            breakable.chanceToRain = 0;
            breakable.dropCoins = false;
            breakable.goopsOnBreak = false;
            breakable.gameObject.layer = 22;
            breakable.sprite.HeightOffGround = -1f;
            breakable.IsDecorativeOnly = false;
            breakable.isInvulnerableToGameActors = true;

            BigBlank trapComp = breakable.gameObject.AddComponent<BigBlank>();

            DungeonPlaceable Placeable = BreakableAPIToolbox.GenerateDungeonPlaceable(new Dictionary<GameObject, float>() { { breakable.gameObject, 1f } });
            Placeable.isPassable = false;
            Placeable.width = 1;
            Placeable.height = 1;
            Placeable.variantTiers[0].unitOffset = new Vector2(-4f / 16f, 2f/16f);
            StaticReferences.StoredDungeonPlaceables.Add("big_blank", Placeable);
            Alexandria.DungeonAPI.StaticReferences.customPlaceables.Add("nn:big_blank", Placeable);
        }
        private void Start()
        {
            base.minorBreakable.OnBreak += OnBreak;
        }
        private void OnBreak()
        {
            GameObject bigSilencerVFX = (GameObject)ResourceCache.Acquire("Global VFX/BlankVFX");
            GameObject gameObject = new GameObject("silencer");
            SilencerInstance silencerInstance = gameObject.AddComponent<SilencerInstance>();
            silencerInstance.TriggerSilencer(base.specRigidbody.UnitCenter, 50f, 25f, bigSilencerVFX, 0.15f, 0.2f, 50f, 10f, 140f, 15f, 0.5f, GameManager.Instance.PrimaryPlayer != null ? GameManager.Instance.PrimaryPlayer : null, true, false);
        }
    }
}
