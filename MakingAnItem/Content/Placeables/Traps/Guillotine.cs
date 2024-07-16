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
using System.Collections;
using Pathfinding;

namespace NevernamedsItems
{
    public class GuillotineTrap : BraveBehaviour
    {
        public static void Init()
        {
            Guillotine = ItemBuilder.SpriteFromBundle("guillotine_idle_001", Initialisation.TrapCollection.GetSpriteIdByName("guillotine_idle_001"), Initialisation.TrapCollection, new GameObject("Guillotine"));
            Guillotine.MakeFakePrefab();
            var GuillotineBody = Guillotine.GetComponent<tk2dSprite>().SetUpSpeculativeRigidbody(new IntVector2(0, -2), new IntVector2(16, 18));
            GuillotineBody.CollideWithTileMap = false;
            GuillotineBody.CollideWithOthers = true;
            Guillotine.GetComponent<tk2dSprite>().GetComponent<tk2dSprite>().HeightOffGround = -1f;
            Guillotine.GetComponent<tk2dSprite>().renderer.material.shader = ShaderCache.Acquire("Brave/LitTk2dCustomFalloffTilted");
            Guillotine.GetComponent<tk2dSprite>().usesOverrideMaterial = true;

            GuillotineBody.PixelColliders = new List<PixelCollider>()
            {
                new PixelCollider()
                {
                   ColliderGenerationMode = PixelCollider.PixelColliderGeneration.Manual,
                   CollisionLayer = CollisionLayer.LowObstacle,
                   ManualWidth = 5,
                   ManualHeight = 6,
                   ManualOffsetX = 0,
                   ManualOffsetY = -1,
                },
                new PixelCollider()
                {
                   ColliderGenerationMode = PixelCollider.PixelColliderGeneration.Manual,
                   CollisionLayer = CollisionLayer.LowObstacle,
                   ManualWidth = 5,
                   ManualHeight = 6,
                   ManualOffsetX = 27,
                   ManualOffsetY = -1,
                },
                new PixelCollider()
                {
                   ColliderGenerationMode = PixelCollider.PixelColliderGeneration.Manual,
                   CollisionLayer = CollisionLayer.BulletBlocker,
                   ManualWidth = 5,
                   ManualHeight = 5,
                   ManualOffsetX = 0,
                   ManualOffsetY = 8,
                },
                new PixelCollider()
                {
                   ColliderGenerationMode = PixelCollider.PixelColliderGeneration.Manual,
                   CollisionLayer = CollisionLayer.BulletBlocker,
                   ManualWidth = 5,
                   ManualHeight = 5,
                   ManualOffsetX = 27,
                   ManualOffsetY = 8,
                },
                new PixelCollider()
                {
                   ColliderGenerationMode = PixelCollider.PixelColliderGeneration.Manual,
                   CollisionLayer = CollisionLayer.LowObstacle,
                   ManualWidth = 32,
                   ManualHeight = 6,
                   ManualOffsetX = 0,
                   ManualOffsetY = -1,
                   Enabled = false,
                }
            };

            var shadowobj = ItemBuilder.SpriteFromBundle("guillotine_shadow", Initialisation.TrapCollection.GetSpriteIdByName("guillotine_shadow"), Initialisation.TrapCollection, new GameObject("Shadow"));
            shadowobj.transform.SetParent(Guillotine.transform);
            shadowobj.transform.localPosition = new Vector3(-1f / 16f, -2f / 16f, 50f);
            tk2dSprite shadow = shadowobj.GetComponent<tk2dSprite>();
            shadow.HeightOffGround = -2f;
            shadow.SortingOrder = 0;
            shadow.IsPerpendicular = false;
            shadow.renderer.material.shader = ShaderCache.Acquire("Brave/LitTk2dCustomFalloffTilted");
            shadow.usesOverrideMaterial = true;

            GameObject triggerOBJ = new GameObject("TriggerBox");
            SpeculativeRigidbody triggerOBJRigidBody = triggerOBJ.GetOrAddComponent<SpeculativeRigidbody>();
            PixelCollider pixelCollider = new PixelCollider();
            pixelCollider.ColliderGenerationMode = PixelCollider.PixelColliderGeneration.Manual;
            pixelCollider.CollisionLayer = CollisionLayer.LowObstacle;
            pixelCollider.IsTrigger = true;
            pixelCollider.ManualWidth = 22;
            pixelCollider.ManualHeight = 14;
            pixelCollider.ManualOffsetX = 5;
            pixelCollider.ManualOffsetY = -5;
            triggerOBJRigidBody.PixelColliders = new List<PixelCollider>
            {
                pixelCollider
            };
            triggerOBJ.transform.SetParent(Guillotine.transform);


            tk2dSpriteAnimator animator = Guillotine.GetOrAddComponent<tk2dSpriteAnimator>();
            animator.Library = Initialisation.trapAnimationCollection;
            animator.defaultClipId = Initialisation.trapAnimationCollection.GetClipIdByName("guillotine_idle");
            animator.DefaultClipId = Initialisation.trapAnimationCollection.GetClipIdByName("guillotine_idle");
            animator.playAutomatically = true;

            Guillotine.AddComponent<GuillotineTrap>();

            Guillotine.SetLayerRecursively(LayerMask.NameToLayer("FG_Critical"));

            DungeonPlaceable guillotinePlaceable = BreakableAPIToolbox.GenerateDungeonPlaceable(new Dictionary<GameObject, float>() { { Guillotine.gameObject, 1f } });
            guillotinePlaceable.isPassable = true;
            guillotinePlaceable.width = 2;
            guillotinePlaceable.height = 1;
            guillotinePlaceable.variantTiers[0].unitOffset = new Vector2(0f, 7f / 16f);
            StaticReferences.StoredDungeonPlaceables.Add("guillotinetrap", guillotinePlaceable);
            Alexandria.DungeonAPI.StaticReferences.customPlaceables.Add("nn:guillotinetrap", guillotinePlaceable);


            GameObject portcullis = FakePrefab.Clone(Guillotine);
            portcullis.name = "Portcullis";
            portcullis.MakeFakePrefab();
            tk2dSpriteAnimator portcullisanimator = portcullis.GetOrAddComponent<tk2dSpriteAnimator>();
            portcullisanimator.Library = Initialisation.trapAnimationCollection;
            portcullisanimator.defaultClipId = Initialisation.trapAnimationCollection.GetClipIdByName("portcullis_idle");
            portcullisanimator.DefaultClipId = Initialisation.trapAnimationCollection.GetClipIdByName("portcullis_idle");
            portcullisanimator.playAutomatically = true;
            portcullis.GetComponent<GuillotineTrap>().isPortcullis = true;

            DungeonPlaceable portcullisPlaceable = BreakableAPIToolbox.GenerateDungeonPlaceable(new Dictionary<GameObject, float>() { { portcullis.gameObject, 1f } });
            portcullisPlaceable.isPassable = true;
            portcullisPlaceable.width = 2;
            portcullisPlaceable.height = 1;
            portcullisPlaceable.variantTiers[0].unitOffset = new Vector2(0f, 7f / 16f);
            StaticReferences.StoredDungeonPlaceables.Add("portcullis", portcullisPlaceable);
            Alexandria.DungeonAPI.StaticReferences.customPlaceables.Add("nn:portcullis", portcullisPlaceable);

            Dustup = VFXToolbox.CreateVFXBundle("GuillotineDust", new IntVector2(35, 27), tk2dBaseSprite.Anchor.LowerLeft, true, 0.1f);

        }
        public static GameObject Guillotine;
        public static GameObject Dustup;
        public void Start()
        {
            currentRoom = base.transform.position.GetAbsoluteRoom();
            base.specRigidbody.PixelColliders[4].Enabled = false;
            triggerOBJ = base.transform.Find("TriggerBox").gameObject;
            triggerOBJ.GetComponent<SpeculativeRigidbody>().OnTriggerCollision += HandleTrigger;

            if (!isPortcullis && Gunfigs._Gunfig.Enabled(Gunfigs.DISABLE_GUILLOTINE)) { isPortcullis = true; }
            if (isPortcullis)
            {
                if (GameManager.Instance.Dungeon.tileIndices.tilesetId == GlobalDungeonData.ValidTilesets.CASTLEGEON)
                {
                    base.spriteAnimator.defaultClipId = Initialisation.trapAnimationCollection.GetClipIdByName("portculliskeep_idle");
                    base.spriteAnimator.DefaultClipId = Initialisation.trapAnimationCollection.GetClipIdByName("portculliskeep_idle");
                    base.spriteAnimator.Play("portculliskeep_idle");
                    fallAnimation = "portculliskeep_fall";
                    triggersound = "Play_OBJ_chainpot_drop_01";
                }
                else
                {
                    base.spriteAnimator.defaultClipId = Initialisation.trapAnimationCollection.GetClipIdByName("portcullis_idle");
                    base.spriteAnimator.DefaultClipId = Initialisation.trapAnimationCollection.GetClipIdByName("portcullis_idle");
                    base.spriteAnimator.Play("portcullis_idle");
                    fallAnimation = "portcullis_fall";
                    triggersound = "Play_OBJ_chainpot_drop_01";
                }
            }
        }
        public bool isPortcullis = false;
        public string fallAnimation = "guillotine_fall";
        public string triggersound = "Play_WPN_crossbow_shot_01";
        public bool Dropped;
        public RoomHandler currentRoom;
        public GameObject triggerOBJ;
        private void HandleTrigger(SpeculativeRigidbody specRigidbody, SpeculativeRigidbody sourceSpecRigidbody, CollisionData collisionData)
        {
            if (!Dropped)
            {
                Dropped = true;
                base.StartCoroutine(Drop());
            }
        }
        public IEnumerator Drop()
        {
            AkSoundEngine.PostEvent(triggersound, base.gameObject);

            base.spriteAnimator.Play(fallAnimation);
            yield return new WaitForSeconds(0.2f);
            List<SpeculativeRigidbody> overlappingRigidbodies = new List<SpeculativeRigidbody>();
            List<ICollidableObject> collidables = PhysicsEngine.Instance.GetOverlappingCollidableObjects(triggerOBJ.GetComponent<SpeculativeRigidbody>().UnitBottomLeft, triggerOBJ.GetComponent<SpeculativeRigidbody>().UnitTopRight, false, true, null, false);
            foreach (ICollidableObject collidable in collidables)
            {
                if (collidable is SpeculativeRigidbody) { overlappingRigidbodies.Add(collidable as SpeculativeRigidbody); }
            }

            //ETGModConsole.Log("overlappingRigidbodies: " + overlappingRigidbodies.Count);
            for (int i = 0; i < overlappingRigidbodies.Count; i++)
            {
                if (overlappingRigidbodies[i].gameActor)
                {
                    // ETGModConsole.Log("actor: " + overlappingRigidbodies[i].gameActor.name);
                    Vector2 direction = overlappingRigidbodies[i].UnitCenter - triggerOBJ.GetComponent<SpeculativeRigidbody>().UnitCenter;
                    if (overlappingRigidbodies[i].healthHaver)
                    {
                        overlappingRigidbodies[i].healthHaver.ApplyDamage((overlappingRigidbodies[i].gameActor is PlayerController) ? 0.5f : 50f, direction, StringTableManager.GetEnemiesString("#TRAP", -1), CoreDamageTypes.None, DamageCategory.Normal, false, null, false);
                    }
                    if (overlappingRigidbodies[i].knockbackDoer) { overlappingRigidbodies[i].knockbackDoer.ApplyKnockback(direction, 20f, false); }
                }
            }
            base.specRigidbody.PixelColliders[4].Enabled = true;
            PhysicsEngine.Instance.RegisterOverlappingGhostCollisionExceptions(base.specRigidbody, null, false);
            this.m_allOccupiedCells = new List<OccupiedCells>(1);
            this.m_allOccupiedCells.Add(new OccupiedCells(base.specRigidbody, base.specRigidbody.PixelColliders[4], currentRoom));

            SpawnManager.SpawnVFX(Dustup, base.transform.position + new Vector3(-3f / 16f, -14f / 16f), Quaternion.identity);

            AkSoundEngine.PostEvent("Play_obj_katana_slash_01", base.gameObject);

            yield break;
        }
        private List<OccupiedCells> m_allOccupiedCells;
       public override void OnDestroy()
        {
            if (GameManager.HasInstance && Pathfinder.HasInstance && base.specRigidbody && this.m_allOccupiedCells != null)
            {
                for (int i = 0; i < this.m_allOccupiedCells.Count; i++)
                {
                    OccupiedCells occupiedCells = this.m_allOccupiedCells[i];
                    if (occupiedCells != null)
                    {
                        occupiedCells.Clear();
                    }
                }
            }
            base.OnDestroy();
        }
    }
}
