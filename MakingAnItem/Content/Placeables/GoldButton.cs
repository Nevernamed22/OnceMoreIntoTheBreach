using Alexandria.BreakableAPI;
using Alexandria.DungeonAPI;
using Alexandria.ItemAPI;
using Dungeonator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Alexandria.Misc;

namespace NevernamedsItems
{
    class GoldButton : BasicTrapController
    {
        public static void Init()
        {
            GameObject buttonobj = ItemBuilder.SpriteFromBundle("goldbutton_idle_001", Initialisation.TrapCollection.GetSpriteIdByName("goldbutton_idle_001"), Initialisation.TrapCollection, new GameObject("Gold Button"));
            buttonobj.MakeFakePrefab();
            tk2dSprite sprite = buttonobj.GetComponent<tk2dSprite>();
            sprite.GetComponent<tk2dSprite>().HeightOffGround = -5f;
            sprite.usesOverrideMaterial = true;
            sprite.SortingOrder = 0;
            sprite.IsPerpendicular = false;
            buttonobj.GetComponent<MeshRenderer>().lightProbeUsage = UnityEngine.Rendering.LightProbeUsage.Off;
            buttonobj.SetLayerRecursively(LayerMask.NameToLayer("FG_Critical"));
            buttonobj.GetComponent<MeshRenderer>().material.shader = ShaderCache.Acquire("Brave/LitTk2dCustomFalloffTilted");

            GoldButton buttoncomp = buttonobj.AddComponent<GoldButton>();
            buttoncomp.isPassable = true;
            buttoncomp.placeableHeight = 2;
            buttoncomp.placeableWidth = 2;
            buttoncomp.triggerMethod = TriggerMethod.PlaceableFootprint;
            buttoncomp.resetDelay = float.MaxValue;
            buttoncomp.triggerDelay = 0;
            buttoncomp.triggerAnimName = "goldbutton_press";
            //buttoncomp.damagesFlyingPlayers = true;

            tk2dSpriteAnimator animator = buttonobj.GetOrAddComponent<tk2dSpriteAnimator>();
            animator.Library = Initialisation.trapAnimationCollection;
            animator.defaultClipId = Initialisation.trapAnimationCollection.GetClipIdByName("goldbutton_idle");
            animator.DefaultClipId = Initialisation.trapAnimationCollection.GetClipIdByName("goldbutton_idle");
            animator.playAutomatically = true;

            DungeonPlaceable buttonPlaceable = BreakableAPIToolbox.GenerateDungeonPlaceable(new Dictionary<GameObject, float>() { { buttonobj, 1f } });
            buttonPlaceable.isPassable = true;
            buttonPlaceable.width = 2;
            buttonPlaceable.height = 2;
            StaticReferences.StoredDungeonPlaceables.Add("goldbutton", buttonPlaceable);
            Alexandria.DungeonAPI.StaticReferences.customPlaceables.Add("nn:goldbutton", buttonPlaceable);
        }
        public override void TriggerTrap(SpeculativeRigidbody target)
        {
            base.TriggerTrap(target);

            List<AIActor> activeEnemies = base.m_parentRoom.GetActiveEnemies(RoomHandler.ActiveEnemyType.All);
            GameManager.Instance.MainCameraController.DoScreenShake(StaticExplosionDatas.genericLargeExplosion.ss, null, false);
            Pixelator.Instance.FadeToColor(0.1f, Color.white, true, 0.1f);
            Exploder.DoDistortionWave(base.sprite.WorldCenter, 0.4f, 0.15f, 10f, 0.4f);
            AkSoundEngine.PostEvent("Play_VO_lichA_cackle_01", base.gameObject);

            GameObject gameObject = new GameObject("silencer");
            SilencerInstance silencerInstance = gameObject.AddComponent<SilencerInstance>();
            silencerInstance.TriggerSilencer(base.sprite.WorldCenter, 25f, 3.5f, null, 0f, 3f, 3f, 3f, 250f, 5f, 0.25f, null, false, false);

            if (activeEnemies != null)
            {
                for (int i = 0; i < activeEnemies.Count; i++)
                {
                    AIActor aiactor = activeEnemies[i];
                    if (aiactor.healthHaver)
                    {
                        aiactor.healthHaver.ApplyDamage(aiactor.healthHaver.IsBoss ? 100 : 1E+07f, Vector2.zero, string.Empty, CoreDamageTypes.None, DamageCategory.Normal, false, null, false);
                    }
                }
            }
        }
    }
}