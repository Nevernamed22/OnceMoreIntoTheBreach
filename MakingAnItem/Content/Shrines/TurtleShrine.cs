using GungeonAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Gungeon;
using Alexandria.ItemAPI;
using Dungeonator;
using System.Reflection;
using MonoMod.RuntimeDetour;
using System.Collections;

namespace NevernamedsItems
{
    public class TurtleShrine : GenericShrine
    {
        public static GameObject Setup(GameObject pedestal)
        {
            var shrineobj = ItemBuilder.SpriteFromBundle("shrine_turtle", Initialisation.NPCCollection.GetSpriteIdByName("shrine_turtle"), Initialisation.NPCCollection, new GameObject("Shrine Turtle Statue"));
            shrineobj.GetComponent<tk2dSprite>().HeightOffGround = 1.25f;
            shrineobj.GetComponent<tk2dSprite>().renderer.material.shader = ShaderCache.Acquire("Brave/LitTk2dCustomFalloffTiltedCutout");
            shrineobj.GetComponent<tk2dSprite>().usesOverrideMaterial = true;
            pedestal.AddComponent<TurtleShrine>();
            GameObject talkpoint = new GameObject("talkpoint");
            talkpoint.transform.SetParent(pedestal.transform);
            talkpoint.transform.localPosition = new Vector3(1f, 36f / 16f, 0f);
            return shrineobj;
        }
        public override bool CanAccept(PlayerController interactor)
        {
            if (interactor.characterIdentity == OMITBChars.Shade)
            {
                if (interactor.carriedConsumables.Currency >= 20) return true;
                else return false;
            }
            if (interactor.ForceZeroHealthState && interactor.healthHaver.Armor > 1) { return true; }
            else if (interactor.healthHaver.GetCurrentHealth() > 0.5f) { return true; }
            return false;
        }
        public override void OnAccept(PlayerController Interactor)
        {
            GameUIRoot.Instance.notificationController.DoCustomNotification(
                    "Turtle Power",
                    "A new friend?",
                    Initialisation.NPCCollection,
                    Initialisation.NPCCollection.GetSpriteIdByName("turtle_icon"),
                    UINotificationController.NotificationColor.SILVER,
                    true,
                    false
                    );

            float SST = UnityEngine.Random.value;
            TurtleShrineEffectHandler eff = Interactor.gameObject.GetOrAddComponent<TurtleShrineEffectHandler>();

            if (Interactor.characterIdentity == PlayableCharacters.Robot)
            {
                Interactor.healthHaver.Armor -= 1;
                if (SST <= 0.01f)
                {
                    PickupObject byId = PickupObjectDatabase.GetById(301);
                    Interactor.AcquirePassiveItemPrefabDirectly(byId as PassiveItem);
                }
                else
                {
                    eff.SpawnNewTurtle();
                    eff.SpawnNewTurtle();
                    eff.SpawnNewTurtle();
                    eff.SpawnNewTurtle();
                }
            }
            else
            {
                if (Interactor.characterIdentity == OMITBChars.Shade) { Interactor.carriedConsumables.Currency -= 20; }
                else { Interactor.healthHaver.ApplyHealing(-0.5f); }
                if (SST <= 0.005f)
                {
                    PickupObject byId = PickupObjectDatabase.GetById(301);
                    Interactor.AcquirePassiveItemPrefabDirectly(byId as PassiveItem);
                }
                else
                {
                    eff.SpawnNewTurtle();
                    eff.SpawnNewTurtle();
                }
            }
            AkSoundEngine.PostEvent("Play_OBJ_shrine_accept_01", base.gameObject);
        }
        public override string AcceptText(PlayerController interactor)
        {
            if (interactor.characterIdentity == OMITBChars.Shade) { return "Beat your wallet against the statue <Lose 20[sprite \"ui_coin\"]>"; }
            if (interactor.ForceZeroHealthState) { return $"Beat your head against the statue <Lose 1 [sprite \"armor_money_icon_001\"]>"; }
            return $"Beat your head against the statue <Lose half a heart>";
        }
        public override string DeclineText(PlayerController Interactor)
        {
            return "Leave";
        }
        public override string PanelText(PlayerController Interactor)
        {
            return "A shrine to a bizarre Gungeoneer, whose psychopathy was rewarded by a bloodthirsty reptilian horde.";
        }
    }
    public class TurtleShrineEffectHandler : MonoBehaviour
    {
        public TurtleShrineEffectHandler()
        {

        }
        private void Start()
        {
            player = base.GetComponent<PlayerController>();
            FloorGenTools.OnDungeonLoadingEnd += LevelLoadStart;
            FloorGenTools.OnDungeonLoadingStart += LevelLoadFinish;
            activeTurtles = new List<GameObject>();
        }
        private void OnDestroy()
        {
            FloorGenTools.OnDungeonLoadingEnd -= LevelLoadStart;
            FloorGenTools.OnDungeonLoadingStart -= LevelLoadFinish;
        }
        private void LevelLoadStart()
        {
            CountForNextFloor = activeTurtles.Count;
            for (int i = (activeTurtles.Count - 1); i >= 0; i--)
            {
                UnityEngine.Object.Destroy(activeTurtles[i]);
            }
            activeTurtles.Clear();
        }
        private void LevelLoadFinish()
        {
            if (CountForNextFloor > 0)
            {
                for (int i = 0; i < CountForNextFloor; i++)
                {
                    SpawnNewTurtle();
                }
            }
        }
        public void SpawnNewTurtle()
        {
            string guid = PickupObjectDatabase.GetById(645).GetComponent<MulticompanionItem>().CompanionGuid;
            AIActor orLoadByGuid = EnemyDatabase.GetOrLoadByGuid(guid);
            Vector3 vector = player.transform.position;
            GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(orLoadByGuid.gameObject, vector, Quaternion.identity);
            CompanionController orAddComponent = gameObject.GetOrAddComponent<CompanionController>();
            if (activeTurtles == null) { activeTurtles = new List<GameObject>(); }
            this.activeTurtles.Add(gameObject);
            orAddComponent.Initialize(player);
            if (orAddComponent.specRigidbody)
            {
                PhysicsEngine.Instance.RegisterOverlappingGhostCollisionExceptions(orAddComponent.specRigidbody, null, false);
            }
            HealthHaver helf = gameObject.GetComponent<HealthHaver>();
            if (helf != null)
            {
                float helfNew = helf.GetMaxHealth() * 3f;
                helf.SetHealthMaximum(helfNew);
                helf.ForceSetCurrentHealth(helfNew);
            }
        }
        private int CountForNextFloor;
        private List<GameObject> activeTurtles;
        private PlayerController player;
    }
}