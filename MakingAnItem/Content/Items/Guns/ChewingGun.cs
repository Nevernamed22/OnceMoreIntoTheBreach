using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Reflection;
using Gungeon;
using MonoMod;
using UnityEngine;
using Alexandria.ItemAPI;
using Alexandria.Misc;
using Alexandria.Assetbundle;
using Dungeonator;

namespace NevernamedsItems
{

    public class ChewingGun : GunBehaviour
    {
        public static void Add()
        {
            Gun gun = ETGMod.Databases.Items.NewGun("Chewing Gun", "chewinggun");
            Game.Items.Rename("outdated_gun_mods:chewing_gun", "nn:chewing_gun");
            var behav = gun.gameObject.AddComponent<ChewingGun>();
            gun.SetShortDescription("How does it feel?");
            gun.SetLongDescription("Chewing gum is a common method of stress relief among experienced Gungeoneers."+"\n\nThis great wad has seen many mouths.");
            
            gun.SetGunSprites("chewinggun", 8);

            gun.SetAnimationFPS(gun.shootAnimation, 12);
            gun.SetAnimationFPS(gun.reloadAnimation, 12);

            gun.AddCustomSwitchGroup("NN_WPN_ChewingGun", "", "Play_ENM_blobulord_bubble_01");

            gun.muzzleFlashEffects = (PickupObjectDatabase.GetById(599) as Gun).muzzleFlashEffects;


            gun.AddProjectileModuleFrom(PickupObjectDatabase.GetById(86) as Gun, true, false);

            //GUN STATS
            gun.DefaultModule.ammoCost = 1;
            gun.DefaultModule.shootStyle = ProjectileModule.ShootStyle.Automatic;
            gun.DefaultModule.sequenceStyle = ProjectileModule.ProjectileSequenceStyle.Random;
            gun.reloadTime = 1.1f;
            gun.DefaultModule.cooldownTime = 0.025f;
            gun.DefaultModule.numberOfShotsInClip = 400;
            gun.barrelOffset.transform.localPosition = new Vector3(31f / 16f, 21f / 16f, 0f);
            gun.SetBaseMaxAmmo(5000);
            gun.ammo = 5000;
            gun.gunClass = GunClass.SILLY;
            gun.doesScreenShake = false;
            gun.IgnoresAngleQuantization = true;
            //BULLET STATS
            Projectile projectile = gun.DefaultModule.projectiles[0].InstantiateAndFakeprefab();
            gun.DefaultModule.projectiles[0] = projectile;
            projectile.baseData.damage = 35;
            projectile.baseData.speed *= 0.5f;
            projectile.baseData.range = 1000000000000;
            projectile.AdditionalScaleMultiplier = 0.1f;
            projectile.hitEffects = (PickupObjectDatabase.GetById(15) as Gun).DefaultModule.projectiles[0].hitEffects;
            projectile.pierceMinorBreakables = true;
            BounceProjModifier bounce = projectile.gameObject.AddComponent<BounceProjModifier>();
            bounce.numberOfBounces = 5;
            projectile.hitEffects = new ProjectileImpactVFXPool();
            projectile.gameObject.AddComponent<ChewingGunProjectile>();
            projectile.AnimateProjectileBundle("ChewingGumProjectile",
                   Initialisation.ProjectileCollection,
                   Initialisation.projectileAnimationCollection,
                   "ChewingGumProjectile",
                   MiscTools.DupeList(new IntVector2(80, 80), 4), //Pixel Sizes
                   MiscTools.DupeList(false, 8), //Lightened
                   MiscTools.DupeList(tk2dBaseSprite.Anchor.MiddleCenter, 4), //Anchors
                   MiscTools.DupeList(true, 4), //Anchors Change Colliders
                   MiscTools.DupeList(false, 4), //Fixes Scales
                   MiscTools.DupeList<Vector3?>(null, 4), //Manual Offsets
                   MiscTools.DupeList<IntVector2?>(null, 4), //Override colliders
                   MiscTools.DupeList<IntVector2?>(null, 4), //Override collider offsets
                   MiscTools.DupeList<Projectile>(null, 4)); // Override to copy from    

            gun.DefaultModule.ammoType = GameUIAmmoType.AmmoType.CUSTOM;
            gun.DefaultModule.customAmmoType = CustomClipAmmoTypeToolbox.AddCustomAmmoType("Chewing Gun Ammo", "NevernamedsItems/Resources/CustomGunAmmoTypes/chewinggun_clipfull", "NevernamedsItems/Resources/CustomGunAmmoTypes/smoker_clipempty");

            gun.quality = PickupObject.ItemQuality.B;
            ETGMod.Databases.Items.Add(gun, null, "ANY");

            ID = gun.PickupObjectId;


            popVFX = VFXToolbox.CreateVFXBundle("GumExplosion", new IntVector2(71, 71), tk2dBaseSprite.Anchor.MiddleCenter, true, 0.4f);
            gummedVFX = VFXToolbox.CreateVFXBundle("GummedEffect", new IntVector2(21, 17), tk2dBaseSprite.Anchor.LowerCenter, true, 0.4f, -1, null, true);
            gummedVFX.AddComponent<GumPile>();
        }
        public static int ID;
        public static GameObject popVFX;
        public static GameObject gummedVFX;

        public override void OnPlayerPickup(PlayerController playerOwner)
        {
            base.OnPlayerPickup(playerOwner);
            playerOwner.OnUsedPlayerItem += ActiveItemUsed;
        }
        public override void DisableEffectPlayer(PlayerController player)
        {
            player.OnUsedPlayerItem -= ActiveItemUsed;
            base.DisableEffectPlayer(player);
        }
        private void ActiveItemUsed(PlayerController player, PlayerItem item)
        {
            if (item.PickupObjectId == 203 && player.PlayerHasActiveSynergy("Addiction Breaker"))
            {
                GameObject splash = SpawnManager.SpawnVFX(ChewingGun.popVFX, player.specRigidbody.UnitCenter, Quaternion.identity);
                AkSoundEngine.PostEvent("Play_MouthPopSound", base.gameObject);

                List<AIActor> activeEnemies = player.CurrentRoom.GetActiveEnemies(RoomHandler.ActiveEnemyType.All);
                if (activeEnemies != null)
                {
                    for (int i = 0; i < activeEnemies.Count; i++)
                    {
                        AIActor aiactor = activeEnemies[i];
                        if (aiactor.IsNormalEnemy && aiactor.healthHaver && aiactor.healthHaver.IsAlive)
                        {
                            aiactor.healthHaver.ApplyDamage(5, Vector2.zero, "Gum");
                            if (aiactor.behaviorSpeculator) { aiactor.behaviorSpeculator.Stun(2f); }

                            GameObject gumpile = SpawnManager.SpawnVFX(ChewingGun.gummedVFX, true);
                            tk2dBaseSprite component = gumpile.GetComponent<tk2dBaseSprite>();

                            gumpile.transform.position = new Vector2(aiactor.sprite.WorldBottomCenter.x + 0.5f, aiactor.sprite.WorldBottomCenter.y);
                            gumpile.transform.parent = aiactor.transform;
                            component.HeightOffGround = 0.2f;
                            aiactor.sprite.AttachRenderer(component);

                            GumPile pile = gumpile.GetComponent<GumPile>();
                            if (pile) { pile.lifetime = 20f; pile.target = aiactor.specRigidbody; }
                        }
                    }
                }
            }
        }
        public override void OnReloadPressed(PlayerController player, Gun gun, bool manual)
        {
            base.OnReloadPressed(player, gun, manual);
            if (gun.ClipShotsRemaining == 0 && player.PlayerHasActiveSynergy("Bazooka Joe") && timeSinceBazooka <= 0)
            {
                timeSinceBazooka = 2f;
                Projectile projectile = (PickupObjectDatabase.GetById(NNBazooka.BazookaID) as Gun).DefaultModule.projectiles[0];
                GameObject gameObject = projectile.InstantiateAndFireInDirection(gun.barrelOffset.position, gun.CurrentAngle, 0);
                Projectile component = gameObject.GetComponent<Projectile>();
                if (component != null)
                {
                    component.Owner = player;
                    component.Shooter = player.specRigidbody;
                    component.ScaleByPlayerStats(player);
                    player.DoPostProcessProjectile(component);
                    component.GetComponent<FuckingExplodeYouCunt>().spawnedBySynergy = true;
                    HomingModifier homing = component.gameObject.GetComponent<HomingModifier>();
                    if (homing == null)
                    {
                        homing = component.gameObject.AddComponent<HomingModifier>();
                        homing.HomingRadius = 0f;
                        homing.AngularVelocity = 0f;
                    }
                    homing.HomingRadius += 7f;
                    homing.AngularVelocity += 360f;
                }
            }
        }

        public override void Update()
        {
            base.Update();
            if (timeSinceBazooka > 0) { timeSinceBazooka -= BraveTime.DeltaTime; }
        }
        public float timeSinceBazooka;
    }
    public class ChewingGunProjectile : MonoBehaviour
    {
        private Projectile self;
        private PlayerController owner;
        public bool inflating = true;
        public Gun lastframeGun;
        private float origSpeed = 0;
        private int inflationLevel;
        private bool released = false;

        private Vector2 BarrelSticky()
        {
            if (!owner) { return Vector2.zero; }
            if (!owner.CurrentGun) { return Vector2.zero; }
            Vector2 barrel = new Vector2(owner.CurrentGun.barrelOffset.position.x, owner.CurrentGun.barrelOffset.position.y);

            float width = self.sprite.GetBounds().size.x / 2f;

            Vector2 vector2 = owner.CurrentGun.CurrentAngle.DegreeToVector2().normalized * width;

            return barrel + vector2;
        }

        private void Start()
        {
            this.self = base.GetComponent<Projectile>();
            owner = self.ProjectilePlayerOwner();
            self.OnDestruction += Destruction;
            self.OnHitEnemy += OnHit;
            if (currentBlowingBubble != null)
            {
                inflating = false;
                self.sprite.renderer.enabled = false;
                currentBlowingBubble.Inflate();
                UnityEngine.Object.Destroy(self.gameObject);
            }
            else
            {
                base.gameObject.SetLayerRecursively(LayerMask.NameToLayer("Default"));
                if (owner && owner.CurrentGun) { lastframeGun = owner.CurrentGun; }
                self.specRigidbody.CollideWithOthers = false;
                self.specRigidbody.CollideWithTileMap = false;
                self.specRigidbody.Reinitialize();
                origSpeed = self.baseData.speed;
                self.baseData.speed = 0.001f;
                self.UpdateSpeed();
                AkSoundEngine.PostEvent("Play_WPN_superdowser_shot_01", self.gameObject);
                currentBlowingBubble = this;
            }
        }

        private void Update()
        {
            if (owner && owner.CurrentGun && inflating)
            {
                self.baseData.speed = 0.001f;
                self.UpdateSpeed();

                Vector2 pos = BarrelSticky();
                if (pos != Vector2.zero)
                {
                    self.specRigidbody.Position = new Position(BarrelSticky());
                    self.sprite.PlaceAtPositionByAnchor(BarrelSticky(), tk2dBaseSprite.Anchor.MiddleCenter);
                }
                if (owner.CurrentGun != lastframeGun || owner.IsDodgeRolling) { inflating = false; Pop(); }
                if (!BraveInput.GetInstanceForPlayer(owner.PlayerIDX).GetButton(GungeonActions.GungeonActionType.Shoot))
                {
                    Release();
                }
            }
        }
        private void OnHit(Projectile self, SpeculativeRigidbody enemy, bool fatal)
        {
            if (!fatal && enemy && enemy.aiActor)
            {
                GumEnemy(enemy.aiActor, inflationLevel / 50f);
            }
        }
        private void Destruction(Projectile self)
        {
            if (released)
            {
                Pop(false);
            }
            else
            {
                AkSoundEngine.PostEvent("Stop_WPN_superdowser_loop_01", self.gameObject);
            }
        }
        private void Release()
        {
            AkSoundEngine.PostEvent("Stop_WPN_superdowser_loop_01", self.gameObject);
            currentBlowingBubble = null;
            released = true;
            inflating = false;
            Vector2 pos = BarrelSticky();
            if (pos != Vector2.zero)
            {
                self.specRigidbody.Position = new Position(BarrelSticky());
            }
            self.specRigidbody.CollideWithOthers = true;
            self.specRigidbody.CollideWithTileMap = true;
            self.baseData.speed = origSpeed;
            self.UpdateSpeed();

            self.specRigidbody.Reinitialize();
            self.SendInDirection(owner.CurrentGun.CurrentAngle.DegreeToVector2(), true, false);

            self.baseData.damage *= inflationLevel / 50f;

            SlowDownOverTimeModifier slowDown = self.gameObject.AddComponent<SlowDownOverTimeModifier>();
            slowDown.extendTimeByRangeStat = true;
            slowDown.activateDriftAfterstop = true;
            slowDown.doRandomTimeMultiplier = true;
            slowDown.killAfterCompleteStop = false;
            slowDown.timeTillKillAfterCompleteStop = 1f;
            slowDown.timeToSlowOver = 0.5f;
            slowDown.targetSpeed = self.baseData.speed * 0.1f;

            DriftModifier drift = self.gameObject.AddComponent<DriftModifier>();
            drift.startInactive = true;
            drift.DriftTimer = 0.5f;
            drift.degreesOfVariance = 40f;
        }


        private void Pop(bool kill = true)
        {
            GameObject splash = SpawnManager.SpawnVFX(ChewingGun.popVFX, self.sprite.WorldCenter, Quaternion.identity);
            splash.transform.localScale = self.sprite.scale * 1.5f;
            AkSoundEngine.PostEvent("Play_MouthPopSound", base.gameObject);

            List<AIActor> activeEnemies = self.GetAbsoluteRoom().GetActiveEnemies(RoomHandler.ActiveEnemyType.All);
            if (activeEnemies != null)
            {
                for (int i = 0; i < activeEnemies.Count; i++)
                {
                    AIActor aiactor = activeEnemies[i];
                    if (aiactor.IsNormalEnemy)
                    {
                        float num = Vector2.Distance(self.LastPosition, aiactor.CenterPosition);
                        if (num <= 5f)
                        {
                            if (aiactor.healthHaver)
                            {
                                aiactor.healthHaver.ApplyDamage(5 * (inflationLevel / 50f), Vector2.zero, "Gum");
                            }
                            GumEnemy(aiactor, (inflationLevel / 50f) * 0.9f);
                        }
                    }
                }
            }
            if (owner.PlayerHasActiveSynergy("Gumzookie"))
            {
                Projectile projectile = (PickupObjectDatabase.GetById(519) as Gun).DefaultModule.projectiles[0];
                GameObject gameObject = projectile.InstantiateAndFireTowardsPosition(self.LastPosition, self.sprite.WorldCenter.GetPositionOfNearestEnemy(ActorCenter.SPRITE), 0, 0, null);               
                Projectile component = gameObject.GetComponent<Projectile>();
                if (component != null)
                {
                    component.Owner = owner;
                    component.Shooter = owner.specRigidbody;
                    component.baseData.damage = 20 * (this.inflationLevel / 50f);
                    component.ScaleByPlayerStats(owner);
                    owner.DoPostProcessProjectile(component);
                    component.RuntimeUpdateScale(this.inflationLevel / 50f);
                    component.gameObject.AddComponent<PierceDeadActors>();
                }
            }

            if (kill) { self.DieInAir(); }
        }
        public void Inflate()
        {
            if (inflating)
            {
                inflationLevel++;
                self.RuntimeUpdateScale(1.05f);
                if (inflationLevel > 50) { inflating = false; Pop(); }
            }
        }
        public void GumEnemy(AIActor target, float gumamount)
        {
            if (target && target.aiActor && target.healthHaver && target.healthHaver.IsAlive)
            {

                if (target.behaviorSpeculator) { target.behaviorSpeculator.Stun(2f * gumamount); }

                GameObject gumpile = SpawnManager.SpawnVFX(ChewingGun.gummedVFX, true);
                tk2dBaseSprite component = gumpile.GetComponent<tk2dBaseSprite>();

                gumpile.transform.position = new Vector2(target.sprite.WorldBottomCenter.x + 0.5f, target.sprite.WorldBottomCenter.y);
                gumpile.transform.parent = target.transform;
                component.HeightOffGround = 0.2f;
                target.sprite.AttachRenderer(component);

                GumPile pile = gumpile.GetComponent<GumPile>();
                if (pile) { pile.lifetime = 20f * gumamount; pile.target = target.specRigidbody; }

            }
        }
        public static ChewingGunProjectile currentBlowingBubble;
    }
    public class GumPile : MonoBehaviour
    {
        public SpeculativeRigidbody target;
        public float lifetime = 1;
        public float elapsed;
        private void Update()
        {
            elapsed += BraveTime.DeltaTime;
            if (elapsed > lifetime)
            {
                End();
                GameObject splash = SpawnManager.SpawnVFX(ChewingGun.popVFX, base.transform.position, Quaternion.identity);
                splash.transform.localScale = new Vector2(0.25f, 0.25f);
                UnityEngine.Object.Destroy(base.gameObject);
            }
        }
        public void Start()
        {
            if (target)
            {
                foreach (Component component in target.gameObject.GetComponentsInChildren<Component>())
                {
                    if (component is GumPile && (component as GumPile) != this)
                    {
                        lifetime += (component as GumPile).lifetime - (component as GumPile).elapsed;
                        (component as GumPile).End();
                        UnityEngine.Object.Destroy(component.gameObject);
                    }
                }
                target.OnPreMovement += ModifyVelocity;
            }
        }
        public void End() { if (target) { target.OnPreMovement -= ModifyVelocity; } }
        public void ModifyVelocity(SpeculativeRigidbody myRigidbody) { myRigidbody.Velocity *= 0; }
    }
}
