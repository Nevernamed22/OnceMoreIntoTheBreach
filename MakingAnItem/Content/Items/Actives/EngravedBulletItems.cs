using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Alexandria.ItemAPI;
using UnityEngine;
using System.Collections;
using SaveAPI;
using Alexandria.Misc;
using Dungeonator;
using Alexandria.EnemyAPI;

namespace NevernamedsItems
{
    public class UnengravedBullets : PlayerItem
    {
        public static void Init()
        {
            PlayerItem item = ItemSetup.NewItem<UnengravedBullets>(
            "Engraving Kit",
            "Waiting For The Right Moment...",
            "Can be used on the nearest enemy, once per floor. Enemies whose names have been engraved can be instantly slain.\n\nThese bullets, while unremarkable at the moment, are brimming with murderous potential.",
            "engravingkit_icon") as PlayerItem;
            ItemBuilder.SetCooldownType(item, ItemBuilder.CooldownType.Timed, 1);

            item.quality = PickupObject.ItemQuality.B;
            item.SetTag("bullet_modifier");
            item.AddToSubShop(ItemBuilder.ShopType.Trorc);
            item.SetupUnlockOnCustomFlag(CustomDungeonFlags.PURCHASED_UNENGRAVEDBULLETS, true);
            item.AddItemToTrorcMetaShop(15);
            Doug.AddToLootPool(item.PickupObjectId);
        }

        public int usesThisFloor = 0;
        public override void Pickup(PlayerController player)
        {
            player.OnNewFloorLoaded += OnNewFloor;
            base.Pickup(player);
        }
        public override void OnPreDrop(PlayerController user)
        {
            if (user) { user.OnNewFloorLoaded -= OnNewFloor; }
            base.OnPreDrop(user);
        }
        public override void OnDestroy()
        {
            if (LastOwner != null) { LastOwner.OnNewFloorLoaded -= OnNewFloor; }
            base.OnDestroy();
        }
        public void OnNewFloor(PlayerController player)
        {
            usesThisFloor = 0;
        }
        public List<string> engravedGUIDs = new List<string>();
        public override bool CanBeUsed(PlayerController user)
        {
            AIActor nearestEnemyToPosition = MathsAndLogicHelper.GetNearestEnemyToPosition(user.CenterPosition, true, RoomHandler.ActiveEnemyType.All, null,
                x => x.healthHaver && !x.healthHaver.IsBoss && !engravedGUIDs.Contains(x.EnemyGuid));
            if (nearestEnemyToPosition != null && Vector2.Distance(user.CenterPosition, nearestEnemyToPosition.Position) < 4f && usesThisFloor < MaxUsesthisFloor(user)) { return true; }
            else return false;
        }
        public int MaxUsesthisFloor(PlayerController user)
        {
            if (user.PlayerHasActiveSynergy("Crafting Supplies")) { return 2; }
            else return 1;
        }
        public override void DoEffect(PlayerController user)
        {
            AIActor nearestEnemyToPosition = MathsAndLogicHelper.GetNearestEnemyToPosition(user.CenterPosition, true, RoomHandler.ActiveEnemyType.All, null, x => x.healthHaver && !x.healthHaver.IsBoss);
            if (nearestEnemyToPosition != null)
            {
                PassiveItem engravedbullets = user.passiveItems.Find(x => x is EngravedBullets);
                if (engravedbullets == null)
                {
                    user.AcquirePassiveItemPrefabDirectly(PickupObjectDatabase.GetById(EngravedBullets.ID) as PassiveItem);
                    engravedbullets = user.passiveItems.Find(x => x is EngravedBullets);
                }

                AkSoundEngine.PostEvent("Play_WPN_Life_Orb_Capture_01", base.gameObject);
                nearestEnemyToPosition.PlayEffectOnActor(PickupObjectDatabase.GetById(595).GetComponent<LifeOrbGunModifier>().OnKilledEnemyVFX, Vector3.zero);

                EngravedBullets engravedComp = engravedbullets as EngravedBullets;
                engravedGUIDs.Add(nearestEnemyToPosition.EnemyGuid);
                engravedComp.engravedGUIDs.Add(nearestEnemyToPosition.EnemyGuid);
                foreach (List<string> possibleCluster in alikeGUIDs)
                {
                    if (possibleCluster.Contains(nearestEnemyToPosition.EnemyGuid))
                    {
                        engravedComp.engravedGUIDs.AddRange(possibleCluster);
                        engravedGUIDs.AddRange(possibleCluster);
                    }
                }

                string enemyName = "Unknown";
                if (nearestEnemyToPosition.encounterTrackable != null && nearestEnemyToPosition.encounterTrackable.journalData != null && nearestEnemyToPosition.encounterTrackable.journalData.GetPrimaryDisplayName() != null)
                {
                    enemyName = nearestEnemyToPosition.encounterTrackable.journalData.GetPrimaryDisplayName();
                }
                else if (!string.IsNullOrEmpty(nearestEnemyToPosition.ActorName)) { enemyName = nearestEnemyToPosition.ActorName; }

                GameUIRoot.Instance.notificationController.DoCustomNotification("NAME ENGRAVED", enemyName, Initialisation.itemCollection, Initialisation.itemCollection.GetSpriteIdByName("engravedbullets_improved"), UINotificationController.NotificationColor.PURPLE, false, false);

               usesThisFloor++;
            }
        }

        List<List<string>> alikeGUIDs = new List<List<string>>()
        {
            new List<string>() //Tankers
            {
                 EnemyGuidDatabase.Tanker,
                 EnemyGuidDatabase.Tanker_Summoned
            }
        };
    }

    public class EngravedBullets : PassiveItem
    {
        public static void Init()
        {
            PickupObject item = ItemSetup.NewItem<EngravedBullets>(
            "Engraved Bullets",
            "Bullet With Your Name On It",
            "These bullets are tailored to annihilate specific foes.\n\nThey may run. They may hide. But you will find them.",
            "engravedbullets_improved");
            item.SetTag("bullet_modifier");
            item.quality = PickupObject.ItemQuality.SPECIAL;
            item.RemovePickupFromLootTables();
            ID = item.PickupObjectId;
        }
        public static int ID;
        public List<string> engravedGUIDs = new List<string>();
        public override void Pickup(PlayerController player)
        {
            base.Pickup(player);
            player.PostProcessProjectile += this.PostProcessProjectile;
            player.PostProcessBeam += this.PostProcessBeam;
        }
        private void PostProcessBeam(BeamController sourceBeam)
        {
            if (sourceBeam.projectile) this.PostProcessProjectile(sourceBeam.projectile, 1);
        }
        private void PostProcessProjectile(Projectile sourceProjectile, float effectChanceScalar)
        {
            ProjectileInstakillBehaviour instakill = sourceProjectile.gameObject.GetOrAddComponent<ProjectileInstakillBehaviour>();
            instakill.enemyGUIDsToKill.AddRange(engravedGUIDs);
            instakill.soundEvent = "Play_WPN_kthulu_blast_01";
            instakill.vfx = PickupObjectDatabase.GetById(595).GetComponent<LifeOrbGunModifier>().OnBurstDamageVFX;
            instakill.extraKnockback = 30f;
            instakill.onInstaKill += OnInstaKill;
        }
        public void OnInstaKill(Projectile bullet, AIActor target)
        {
            if (base.Owner)
            {
                if (base.Owner.PlayerHasActiveSynergy("In-Grave"))
                {
                    GameObject gemy = StandardisedProjectiles.ghost.InstantiateAndFireInDirection(
                      target.specRigidbody.UnitCenter, BraveUtility.RandomAngle());
                    if (bullet.Owner)
                    {
                        Projectile proj = gemy.GetComponent<Projectile>();
                        proj.Owner = bullet.Owner;
                        proj.Shooter = bullet.Owner.specRigidbody;
                        if (bullet.ProjectilePlayerOwner())
                        {
                            bullet.ProjectilePlayerOwner().DoPostProcessProjectile(proj);
                            proj.ScaleByPlayerStats(bullet.ProjectilePlayerOwner());
                        }
                        proj.specRigidbody.RegisterGhostCollisionException(target.specRigidbody);
                    }
                }
                if (base.Owner.PlayerHasActiveSynergy("Excellence in Journalism"))
                {
                    List<AIActor> activeEnemies = target.ParentRoom.GetActiveEnemies(RoomHandler.ActiveEnemyType.All);
                    if (activeEnemies != null)
                    {
                        for (int i = 0; i < activeEnemies.Count; i++)
                        {
                            AIActor aiactor = activeEnemies[i];
                            if (aiactor.IsNormalEnemy && aiactor != target)
                            {
                                float distance = Vector2.Distance(target.ClosestPointOnEnemy(aiactor.CenterPosition), aiactor.ClosestPointOnEnemy(target.CenterPosition));
                                if (distance <= 4f)
                                {
                                    Vector2 vec = new Vector2(target.Position.x, target.Position.y).CalculateVectorBetween(aiactor.Position);
                                    if (aiactor.knockbackDoer) { aiactor.knockbackDoer.ApplyKnockback(vec, 7f); }
                                    if (aiactor.healthHaver) { aiactor.healthHaver.ApplyDamage(10f, vec, "Synergy", CoreDamageTypes.Void); }
                                }
                            }
                        }
                    }
                }
                if (base.Owner.PlayerHasActiveSynergy("Come To Collect"))
                {
                    LootEngine.SpawnCurrency(target.specRigidbody.UnitCenter, UnityEngine.Random.Range(1, 3));
                }

            }
        }
        public override void DisableEffect(PlayerController player)
        {
            player.PostProcessProjectile -= this.PostProcessProjectile;
            player.PostProcessBeam -= this.PostProcessBeam;
            base.DisableEffect(player);
        }
    }
}
