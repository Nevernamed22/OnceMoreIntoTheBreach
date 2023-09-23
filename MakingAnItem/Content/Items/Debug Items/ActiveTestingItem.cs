using System;
using System.Collections.Generic;
using System.Linq;
using Dungeonator;
using System.Text;

using Alexandria.ItemAPI;
using Alexandria.Misc;
using UnityEngine;
using System.Collections;
using SaveAPI;

namespace NevernamedsItems
{
    class ActiveTestingItem : PlayerItem
    {
        //Call this method from the Start() method of your ETGModule extension class
        public static void Init()
        {
            //The name of the item
            string itemName = "<WIP> Active Testing Item <WIP>";

            //Refers to an embedded png in the project. Make sure to embed your resources! Google it.
            string resourceName = "NevernamedsItems/Resources/workinprogress_icon";

            //Create new GameObject
            GameObject obj = new GameObject(itemName);

            //Add a ActiveItem component to the object
            var item = obj.AddComponent<ActiveTestingItem>();

            //Adds a tk2dSprite component to the object and adds your texture to the item sprite collection
            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);

            //Ammonomicon entry variables
            string shortDesc = "Work In Progress";
            string longDesc = "This item was created by an amateur gunsmith so that he may test different concepts instead of going the whole nine yards and making a whole new item.";

            //Adds the item to the gungeon item list, the ammonomicon, the loot table, etc.
            //"kts" here is the item pool. In the console you'd type kts:sweating_bullets
            ItemBuilder.SetupItem(item, shortDesc, longDesc, "nn");

            //Set the cooldown type and duration of the cooldown
            ItemBuilder.SetCooldownType(item, ItemBuilder.CooldownType.None, 0);

            //Adds a passive modifier, like curse, coolness, damage, etc. to the item. Works for passives and actives.

            //Set some other fields
            item.consumable = false;
            item.quality = ItemQuality.EXCLUDED;

            magicCircle = new GameObject();
            magicCircle.MakeFakePrefab();
            SlowingCircle circ = magicCircle.AddComponent<SlowingCircle>();
            circ.emitsParticles = true;
            circ.autoEnableAutoDisableTimer = 10f;
        }
        public static GameObject magicCircle;
        public override bool CanBeUsed(PlayerController user)
        {
            return true;
        }
        //float duration = 20f;
        public override void DoEffect(PlayerController user)
        {
            //IntVector2 bestRewardLocation = user.CurrentRoom.GetBestRewardLocation(IntVector2.One * 3, RoomHandler.RewardLocationStyle.PlayerCenter, true);
            //ChestToolbox.ChestTier tier = RandomEnum<ChestToolbox.ChestTier>.Get();
            //ChestToolbox.SpawnChestEasy(bestRewardLocation, tier, true, Chest.GeneralChestType.UNSPECIFIED);
            //SpawnObjectManager.SpawnObject(ExoticPlaceables.SteelTableHorizontal, user.specRigidbody.UnitCenter, null);

            /*   IPlayerInteractable nearestInteractable = user.CurrentRoom.GetNearestInteractable(user.CenterPosition, 1f, user);
               if (!(nearestInteractable is Chest)) return;

               Chest rerollChest = nearestInteractable as Chest;
               if (rerollChest.IsMimic)
               {
                   rerollChest.ForceOpen(user);
                   return;
               }
               rerollChest.contents = new List<PickupObject>()
               {
                   PickupObjectDatabase.GetById(51)
               };*/

            //ProjectileImpactVFXPool hit = (PickupObjectDatabase.GetById(178) as Gun).GetComponent<FireOnReloadSynergyProcessor>().DirectedBurstSettings.ProjectileInterface.SpecifiedProjectile.hitEffects;


            //GameManager.Instance.MainCameraController.Camera.transform.rotation = Quaternion.Euler(0, 0, 180);
            // CurseManager.AddCurse("Curse of Butterfingers", true);
            /* AkSoundEngine.PostEvent("Play_ClownHonk", user.gameObject);
             foreach(Projectile proj in StaticReferenceManager.AllProjectiles)
             {

                 ETGModConsole.Log($"<color=#ff0000ff>{proj.gameObject.name}</color>");
                 bool isBem = proj.GetComponent<BasicBeamController>() != null;

                 ETGModConsole.Log($"<color=#ff0000ff>Is Beam: </color>{isBem}");
                 if (isBem)
                 {
                     ETGModConsole.Log("Bone Count: " + proj.GetComponent<BasicBeamController>().GetBoneCount());
                     ETGModConsole.Log("UsesBones: " + proj.GetComponent<BasicBeamController>().UsesBones);
                     ETGModConsole.Log("Interpolate: " + proj.GetComponent<BasicBeamController>().interpolateStretchedBones);
                 }
                 ETGModConsole.Log("<color=#ff0000ff>Components</color>");
                 ETGModConsole.Log("<color=#ff0000ff>Children</color>");
                 foreach (Component component in proj.GetComponentsInChildren<Component>())
                 {
                     ETGModConsole.Log(component.GetType().ToString());
                 }
                 ETGModConsole.Log("<color=#ff0000ff>Parents</color>");
                 foreach (Component component in proj.GetComponentsInParent<Component>())
                 {
                     ETGModConsole.Log(component.GetType().ToString());
                 }
                 ETGModConsole.Log("<color=#ff0000ff>---------------------------------</color>");
             }*/



            Vector3 place = user.GetCursorPosition(4);
            GameObject carto = UnityEngine.Object.Instantiate<GameObject>(Initialisation.toSpawn, place, Quaternion.identity);
            if (carto.GetComponent<DungeonPlaceable>()) DungeonPlaceableUtility.InstantiateDungeonPlaceable(carto, user.CurrentRoom, ((Vector2)carto.transform.position).ToIntVector2(), false);
            //SaveAPIManager.SetFlag(CustomDungeonFlags.CHEATED_DEATH_SHADE, true);
            // CurseManager.AddCurse("Curse of Butterfingers");
            /*  ChamberGunProcessor processor = user.CurrentGun.GetComponentInChildren<ChamberGunProcessor>();
              if (processor)
              {
                  ETGModConsole.Log("Keep: " + processor.CastleGunID);
                  ETGModConsole.Log("Oub: " + processor.OublietteGunID);
                  ETGModConsole.Log("GP: " + processor.GungeonGunID);
                  ETGModConsole.Log("Abbey: " + processor.AbbeyGunID);
                  ETGModConsole.Log("Mines: " + processor.MinesGunID);
                  ETGModConsole.Log("Rat: " + processor.RatgeonGunID);
                  ETGModConsole.Log("Hollow: " + processor.HollowGunID);
                  ETGModConsole.Log("R&G: " + processor.OfficeGunID);
                  ETGModConsole.Log("Forge: " + processor.ForgeGunID);
                  ETGModConsole.Log("BulletHell: " + processor.HellGunID);
              }*/


            //BeamToolbox.FreeFireBeamFromAnywhere(LaserBullets.SimpleRedBeam, user, user.gameObject, Vector2.zero, false, user.CurrentGun.CurrentAngle, 10, true);

            /* Vector2 yourPosition = user.sprite.WorldCenter;
             List<AIActor> activeEnemies = user.CurrentRoom.GetActiveEnemies(RoomHandler.ActiveEnemyType.All);
             if (activeEnemies != null)
             {
                 for (int i = 0; i < activeEnemies.Count; i++)
                 {
                     AIActor aiactor = activeEnemies[i];



                    aiactor.ApplyEffect(new GameActorExsanguinationEffect()
                    {
                      duration = 10,
                      effectIdentifier = "exsanguination",
                      stackMode = GameActorEffect.EffectStackingMode.DarkSoulsAccumulate,

                    });

                    aiactor.ApplyEffect(new GameActorJarateEffect()
                      {
                        duration = 10,
                        effectIdentifier = "jarated",
                        stackMode = GameActorEffect.EffectStackingMode.Stack,
                        HealthMultiplier = 0.66f,
                        SpeedMultiplier = 0.9f,                      
                      });
              }
          }*/
            //EasyVFXDatabase.TeleporterPrototypeTelefragVFX.GetComponent<ParticleSystem>()

            //DeadlyDeadlyGoopManager.GetGoopManagerForGoopType(EasyGoopDefinitions.BulletKingWine).TimedAddGoopCircle(user.sprite.WorldCenter, 10, 1, false);


            // var blood = FakePrefab.Clone(PickupObjectDatabase.GetById(449).GetComponent<TeleporterPrototypeItem>().TelefragVFXPrefab.gameObject).GetComponent<ParticleSystem>();
            // blood.emission.SetBurst(0, new ParticleSystem.Burst { count = 1, time = 0, cycleCount = 1, repeatInterval = 0.010f, maxCount = 1, minCount = 1 });
            // UnityEngine.Object.Instantiate(blood, user.specRigidbody.UnitCenter, Quaternion.identity);


            //GameObject obj = UnityEngine.Object.Instantiate(magicCircle, user.specRigidbody.UnitCenter, Quaternion.identity);

            //VFXToolbox.DoRisingStringFade("Test String Wowza", user.sprite.WorldTopCenter, Color.white);
            //VFXToolbox.DoStringSquirt("Test String Wow3", user.specRigidbody.UnitTopCenter, Color.red);

        }


        public override void Update()
        {
            base.Update();
        }
        public override void Pickup(PlayerController player)
        {
            base.Pickup(player);

        }
        class SlowingCircle : MagicCircle
        {
            public override void TickOnEnemy(AIActor enemy)
            {
                enemy.gameActor.ApplyEffect(EasyGoopDefinitions.HoneyGoop.SpeedModifierEffect);
                base.TickOnEnemy(enemy);
            }
            public override void EnemyEnteredCircle(AIActor enemy)
            {
                enemy.DeregisterOverrideColor("magiccircle");
                enemy.RegisterOverrideColor(Color.green, "magiccircle");
                base.EnemyEnteredCircle(enemy);
            }
            public override void EnemyLeftCircle(AIActor enemy)
            {
                enemy.DeregisterOverrideColor("magiccircle");
                enemy.RegisterOverrideColor(Color.red, "magiccircle");
                base.EnemyLeftCircle(enemy);
            }
        }

    }


    class MagicCircle : MonoBehaviour
    {
        public static List<MagicCircle> AllMagicCircles = new List<MagicCircle>();
        public MagicCircle()
        {
            enabled = false;
            emitsParticles = false;
            colour = Color.white;
            radius = 3f;
            destroyOnDisable = true;
            autoEnableOnStart = true;
            autoEnableAutoDisableTimer = -1f;
            preventMagicIndicator = false;
        }
        private void Start()
        {
            AllMagicCircles.Add(this);
            if (autoEnableOnStart) { Enable(autoEnableAutoDisableTimer); }
        }
        private void OnDestroy() { AllMagicCircles.Remove(this); }
        public void Enable(float disableAfterSeconds = -1)
        {
            if (!enabled)
            {
                if (!preventMagicIndicator)
                {
                    if (m_radialIndicator != null) { m_radialIndicator.EndEffect(); }
                    m_radialIndicator = ((GameObject)UnityEngine.Object.Instantiate(ResourceCache.Acquire("Global VFX/HeatIndicator"), base.gameObject.transform.position, Quaternion.identity)).GetComponent<HeatIndicatorController>();
                    m_radialIndicator.CurrentColor = colour;
                    m_radialIndicator.IsFire = emitsParticles;
                    m_radialIndicator.CurrentRadius = radius;
                    m_radialIndicator.transform.parent = this.transform;
                }
                OnEnabled();
                enabled = true;

                if (disableAfterSeconds > 0) { base.StartCoroutine(DisableManager(disableAfterSeconds)); }
            }
            else { Debug.LogWarning("Alexandria (MagicCircleDoer): Cannot enable a magic circle which is already enabled."); }
        }
        private IEnumerator DisableManager(float time)
        {
            yield return new WaitForSeconds(time);
            Disable();
            yield break;
        }
        public void Disable()
        {
            if (m_radialIndicator != null) { m_radialIndicator.EndEffect(); m_radialIndicator = null; }
            for (int i = actorsInCircle.Count - 1; i >= 0; i--)
            {
                if (actorsInCircle[i] != null)
                {
                    EnemyLeftCircle(actorsInCircle[i]);
                }
            }
            actorsInCircle.Clear();
            OnDisabled();
            enabled = false;
            if (destroyOnDisable) { UnityEngine.Object.Destroy(gameObject); }
        }

        public void UpdateRadius(float newRadius)
        {
            radius = newRadius;
            if (m_radialIndicator)
            {
                m_radialIndicator.CurrentRadius = radius;
                m_radialIndicator.m_materialInst.SetFloat(m_radialIndicator.m_radiusID, radius);
            }
            OnRadiusUpdated();
        }

        public float radius;
        public bool preventMagicIndicator;
        public Color colour;
        public bool destroyOnDisable;
        public bool emitsParticles;
        public bool autoEnableOnStart;
        public float autoEnableAutoDisableTimer;

        public virtual void OnEnabled() { }
        public virtual void OnDisabled() { }
        public virtual void OnRadiusUpdated() { }
        public virtual void TickOnEnemy(AIActor enemy) { }
        public virtual void EnemyEnteredCircle(AIActor enemy) { }
        public virtual void EnemyLeftCircle(AIActor enemy) { }

        private void Update()
        {
            if (enabled && !GameManager.Instance.IsLoadingLevel && GameManager.Instance.Dungeon != null)
            {
                for (int i = StaticReferenceManager.AllEnemies.Count - 1; i >= 0; i--)
                {
                    if (StaticReferenceManager.AllEnemies[i] != null)
                    {
                        if (Vector2.Distance(StaticReferenceManager.AllEnemies[i].Position, base.gameObject.transform.position) <= radius)
                        {
                            if (!actorsInCircle.Contains(StaticReferenceManager.AllEnemies[i]))
                            {
                                EnemyEnteredCircle(StaticReferenceManager.AllEnemies[i]);
                                actorsInCircle.Add(StaticReferenceManager.AllEnemies[i]);
                            }
                            TickOnEnemy(StaticReferenceManager.AllEnemies[i]);
                        }
                        else if (actorsInCircle.Contains(StaticReferenceManager.AllEnemies[i]))
                        {
                            EnemyLeftCircle(StaticReferenceManager.AllEnemies[i]);
                            actorsInCircle.Remove(StaticReferenceManager.AllEnemies[i]);
                        }
                    }
                }
                for (int i = actorsInCircle.Count - 1; i >= 0; i--)
                {
                    if (actorsInCircle[i] == null) actorsInCircle.RemoveAt(i);
                }
            }
        }
        private bool enabled;
        private List<AIActor> actorsInCircle = new List<AIActor>();
        private HeatIndicatorController m_radialIndicator;


    }
}

