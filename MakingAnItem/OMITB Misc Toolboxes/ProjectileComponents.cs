using Dungeonator;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace NevernamedsItems
{
    //CUSTOM PROJECTILE COMPONENTS
    public class AutoDoShadowChainOnSpawn : MonoBehaviour
    {
        public AutoDoShadowChainOnSpawn()
        {
            this.NumberInChain = 1;
            this.pauseLength = 0.2f;
            this.chainScaleMult = 1;
            this.overrideProjectile = null;
            this.randomChainMin = 1;
            this.randomChainMax = 4;
            this.randomiseChainNum = false;
        }
        private void Start()
        {
            this.m_projectile = base.GetComponent<Projectile>();

            if (randomiseChainNum)
            {
                int selectednum = UnityEngine.Random.Range(randomChainMin, randomChainMax + 1);
                if (selectednum > 0)
                {
                    this.m_projectile.SpawnChainedShadowBullets(selectednum, pauseLength, chainScaleMult, overrideProjectile);
                }
            }
            else
            {
                this.m_projectile.SpawnChainedShadowBullets(NumberInChain, pauseLength, chainScaleMult, overrideProjectile);
            }
        }
        public Projectile overrideProjectile;
        public int NumberInChain;
        public float pauseLength;
        public float chainScaleMult;
        public bool randomiseChainNum;
        public int randomChainMin;
        public int randomChainMax;
        private Projectile m_projectile;
    } //Spawns a chain of shadow bullets after the host projectile.
    public class CollideWithPlayerBehaviour : MonoBehaviour
    {
        public CollideWithPlayerBehaviour()
        {
        }

        public void Start()
        {
            try
            {
                this.m_projectile = base.GetComponent<Projectile>();

                this.m_projectile.allowSelfShooting = true;
                this.m_projectile.collidesWithEnemies = true;
                this.m_projectile.collidesWithPlayer = true;
                this.m_projectile.SetNewShooter(this.m_projectile.Shooter);
                this.m_projectile.UpdateCollisionMask();
            }
            catch (Exception e)
            {
                ETGModConsole.Log(e.Message);
                ETGModConsole.Log(e.StackTrace);
            }
        }
        private Projectile m_projectile;
    } //Allows the projectile to hit players.
    public class HomeInOnPlayerModifyer : BraveBehaviour
    {
        private void Start()
        {
            if (!this.m_projectile)
            {
                this.m_projectile = base.GetComponent<Projectile>();
            }
            Projectile projectile = this.m_projectile;
            if (projectile.Owner && projectile.Owner is PlayerController)
            {
                projectile.ModifyVelocity = (Func<Vector2, Vector2>)Delegate.Combine(projectile.ModifyVelocity, new Func<Vector2, Vector2>(this.ModifyVelocity));
            }
        }
        public void AssignProjectile(Projectile source)
        {
            this.m_projectile = source;
        }
        private Vector2 ModifyVelocity(Vector2 inVel)
        {
            Vector2 vector = inVel;
            RoomHandler absoluteRoomFromPosition = GameManager.Instance.Dungeon.data.GetAbsoluteRoomFromPosition(this.m_projectile.LastPosition.IntXY(VectorConversions.Floor));
            float num = float.MaxValue;
            Vector2 vector2 = Vector2.zero;

            PlayerController target = m_projectile.Owner as PlayerController;

            Vector2 vector3 = (!base.sprite) ? base.transform.position.XY() : base.sprite.WorldCenter;

            Vector2 vector4 = target.CenterPosition - vector3;
            float sqrMagnitude = vector4.sqrMagnitude;
            if (sqrMagnitude < num)
            {
                vector2 = vector4;
                num = sqrMagnitude;

            }

            num = Mathf.Sqrt(num);
            if (num < this.HomingRadius)
            {
                float num2 = 1f - num / this.HomingRadius;
                float num3 = vector2.ToAngle();
                float num4 = inVel.ToAngle();
                float num5 = this.AngularVelocity * num2 * this.m_projectile.LocalDeltaTime;
                float num6 = Mathf.MoveTowardsAngle(num4, num3, num5);
                if (this.m_projectile is HelixProjectile)
                {
                    float angleDiff = num6 - num4;
                    (this.m_projectile as HelixProjectile).AdjustRightVector(angleDiff);
                }
                else
                {
                    if (this.m_projectile.shouldRotate)
                    {
                        base.transform.rotation = Quaternion.Euler(0f, 0f, num6);
                    }
                    vector = BraveMathCollege.DegreesToVector(num6, inVel.magnitude);
                }
                if (this.m_projectile.OverrideMotionModule != null)
                {
                    this.m_projectile.OverrideMotionModule.AdjustRightVector(num6 - num4);
                }
            }
            if (vector == Vector2.zero || float.IsNaN(vector.x) || float.IsNaN(vector.y))
            {
                return inVel;
            }
            return vector;
        }

        protected override void OnDestroy()
        {
            if (this.m_projectile)
            {
                Projectile projectile = this.m_projectile;
                projectile.ModifyVelocity = (Func<Vector2, Vector2>)Delegate.Remove(projectile.ModifyVelocity, new Func<Vector2, Vector2>(this.ModifyVelocity));
            }
            base.OnDestroy();
        }

        public float HomingRadius = 2f;
        public float AngularVelocity = 180f;
        protected Projectile m_projectile;
    } //Causes the projectile to home in on the Playercontroller that fired it.
    public class NoCollideBehaviour : MonoBehaviour
    {
        public NoCollideBehaviour()
        {
            worksOnProjectiles = false;
            worksOnEnemies = true;
        }

        public void Start()
        {
            try
            {
                this.m_projectile = base.GetComponent<Projectile>();
                this.m_projectile.specRigidbody.OnPreRigidbodyCollision += this.HandlePreCollision;
            }
            catch (Exception e)
            {
                ETGModConsole.Log(e.Message);
                ETGModConsole.Log(e.StackTrace);
            }
        }

        private void HandlePreCollision(SpeculativeRigidbody myRigidbody, PixelCollider myPixelCollider, SpeculativeRigidbody otherRigidbody, PixelCollider otherPixelCollider)
        {
            try
            {
                if (otherRigidbody)
                {
                    if (otherRigidbody.aiActor != null && otherRigidbody.healthHaver != null)
                    {
                        if (worksOnEnemies)
                        {
                            PhysicsEngine.SkipCollision = true;
                        }
                    }
                    else if (otherRigidbody.projectile != null && otherRigidbody.projectile.collidesWithProjectiles)
                    {
                        if (worksOnProjectiles)
                        {
                            PhysicsEngine.SkipCollision = true;
                        }
                    }
                    else
                    {
                        PhysicsEngine.SkipCollision = true;
                    }
                }
            }
            catch (Exception e)
            {
                ETGModConsole.Log(e.Message);
                ETGModConsole.Log(e.StackTrace);
            }
        }
        private Projectile m_projectile;

        public bool worksOnEnemies = false;
        public bool worksOnProjectiles = false;
    } //Prevents the projectile from colliding with other rigid bodies.
    public class TickDamageBehaviour : MonoBehaviour
    {
        public TickDamageBehaviour()
        {
            damageSource = "Tick Damage";
            starterDamage = 2f;
        }

        public void Start()
        {
            try
            {
                this.m_projectile = base.GetComponent<Projectile>();
                this.m_sprite = this.m_projectile.sprite;
                if (this.m_projectile.Owner is PlayerController)
                {
                    this.owner = this.m_projectile.Owner as PlayerController;
                }
                this.m_projectile.specRigidbody.OnPreRigidbodyCollision += this.HandlePreCollision;
                this.m_projectile.OnHitEnemy += this.OnHitEnemy;
                //this.m_projectile.OnPostUpdate += this.HandlePostUpdate;

            }
            catch (Exception e)
            {
                ETGModConsole.Log(e.Message);
                ETGModConsole.Log(e.StackTrace);
            }
        }
        private PlayerController owner;
        public List<HealthHaver> FirstStrikeEnemies = new List<HealthHaver>() { };
        private void OnHitEnemy(Projectile bullet, SpeculativeRigidbody enemy, bool fatal)
        {
            if (enemy.healthHaver)
            {
                if (!FirstStrikeEnemies.Contains(enemy.healthHaver))
                {
                    FirstStrikeEnemies.Add(enemy.healthHaver);
                }
            }
        }
        private void HandlePreCollision(SpeculativeRigidbody myRigidbody, PixelCollider myPixelCollider, SpeculativeRigidbody otherRigidbody, PixelCollider otherPixelCollider)
        {
            try
            {
                if (otherRigidbody.healthHaver != null && FirstStrikeEnemies.Contains(otherRigidbody.healthHaver))
                {
                    PlayerController playerness = otherRigidbody.gameObject.GetComponent<PlayerController>();
                    if (playerness == null)
                    {
                        float damageToDeal = starterDamage;
                        damageToDeal *= this.owner.stats.GetStatValue(PlayerStats.StatType.Damage);
                        if (otherRigidbody.healthHaver.IsBoss) damageToDeal *= this.owner.stats.GetStatValue(PlayerStats.StatType.DamageToBosses);
                        if (otherRigidbody.aiActor && otherRigidbody.aiActor.IsBlackPhantom) damageToDeal *= this.m_projectile.BlackPhantomDamageMultiplier;
                        otherRigidbody.healthHaver.ApplyDamage(damageToDeal, Vector2.zero, damageSource, CoreDamageTypes.None, DamageCategory.Unstoppable, true, null, true);
                        PhysicsEngine.SkipCollision = true;
                    }
                }
            }
            catch (Exception e)
            {
                ETGModConsole.Log(e.Message);
                ETGModConsole.Log(e.StackTrace);
            }
        }
        private Projectile m_projectile;
        private tk2dBaseSprite m_sprite;
        public float EffectRadius = 1f;
        public string damageSource;
        public float starterDamage;
    } //Causes the projectile to deal tick damage to enemies it overlaps. Requires SuperPierceProjectile.
    public class RandomProjectileStatsComponent : MonoBehaviour
    {
        public RandomProjectileStatsComponent()
        {
            this.randomDamage = false;
            this.highDMGPercent = 200;
            this.lowDMGPercent = 10;

            this.randomSpeed = false;
            this.highSpeedPercent = 200;
            this.lowSpeedPercent = 10;

            this.randomKnockback = false;
            this.highKnockbackPercent = 200;
            this.lowKnockbackPercent = 10;

            this.randomRange = false;
            this.highRangePercent = 200;
            this.lowRangePercent = 10;

            this.randomScale = false;
            this.highScalePercent = 200;
            this.lowScalePercent = 10;

            this.randomJammedDMG = false;
            this.highJammedDMGPercent = 200;
            this.lowJammedDMGPercent = 10;

            this.randomBossDMG = false;
            this.highBossDMGPercent = 200;
            this.lowBossDMGPercent = 10;

            this.scaleBasedOnDamage = false;
        }
        private void Start()
        {
            this.m_projectile = base.GetComponent<Projectile>();

            if (randomSpeed) this.m_projectile.baseData.speed *= (UnityEngine.Random.Range(lowSpeedPercent, highSpeedPercent + 1)) / 100f;
            if (randomKnockback) this.m_projectile.baseData.force *= (UnityEngine.Random.Range(lowKnockbackPercent, highKnockbackPercent + 1)) / 100f;
            if (randomRange) this.m_projectile.baseData.range *= (UnityEngine.Random.Range(lowRangePercent, highRangePercent + 1)) / 100f;
            if (randomBossDMG) this.m_projectile.BossDamageMultiplier *= (UnityEngine.Random.Range(lowBossDMGPercent, highBossDMGPercent + 1)) / 100f;
            if (randomJammedDMG) this.m_projectile.BlackPhantomDamageMultiplier *= (UnityEngine.Random.Range(lowJammedDMGPercent, highJammedDMGPercent + 1)) / 100f;
            float damageMult = (UnityEngine.Random.Range(lowDMGPercent, highDMGPercent + 1)) / 100f;
            if (randomDamage) this.m_projectile.baseData.damage *= damageMult;
            if (randomScale) this.m_projectile.RuntimeUpdateScale((UnityEngine.Random.Range(lowScalePercent, highScalePercent + 1)) / 100f);
            if (scaleBasedOnDamage) this.m_projectile.RuntimeUpdateScale(damageMult);
            this.m_projectile.UpdateSpeed();
        }
        private Projectile m_projectile;

        public bool randomDamage;
        public int highDMGPercent;
        public int lowDMGPercent;

        public bool randomSpeed;
        public int highSpeedPercent;
        public int lowSpeedPercent;

        public bool randomKnockback;
        public int highKnockbackPercent;
        public int lowKnockbackPercent;

        public bool randomRange;
        public int highRangePercent;
        public int lowRangePercent;

        public bool randomScale;
        public int highScalePercent;
        public int lowScalePercent;

        public bool randomBossDMG;
        public int highBossDMGPercent;
        public int lowBossDMGPercent;

        public bool randomJammedDMG;
        public int highJammedDMGPercent;
        public int lowJammedDMGPercent;

        public bool scaleBasedOnDamage;
    } //Randomises the stats of the attached projectile.
    public class RandomiseProjectileColourComponent : MonoBehaviour
    {
        public RandomiseProjectileColourComponent()
        {
            ListOfColours = new List<Color>
        {
            ExtendedColours.pink,
            Color.red,
            ExtendedColours.orange,
            Color.yellow,
            Color.green,
            Color.blue,
            ExtendedColours.purple,
            Color.cyan,
        };
            ApplyColourToHitEnemies = false;
            tintPriority = 1;
        }
        public static List<Color> ListOfColours;
        public bool ApplyColourToHitEnemies;
        public int tintPriority;
        private void Start()
        {
            this.m_projectile = base.GetComponent<Projectile>();
            selectedColour = BraveUtility.RandomElement(ListOfColours);
            m_projectile.AdjustPlayerProjectileTint(selectedColour, tintPriority);
            if (ApplyColourToHitEnemies) m_projectile.OnHitEnemy += this.OnHitEnemy;
        }
        private Projectile m_projectile;
        private Color selectedColour;
        private void OnHitEnemy(Projectile bullet, SpeculativeRigidbody enemy, bool what)
        {
            GameActorHealthEffect tint = new GameActorHealthEffect()
            {
                TintColor = selectedColour,
                DeathTintColor = selectedColour,
                AppliesTint = true,
                AppliesDeathTint = true,
                AffectsEnemies = true,
                DamagePerSecondToEnemies = 0f,
                duration = 10000000,
                effectIdentifier = "ProjectileAppliedTint",
            };
            enemy.aiActor.ApplyEffect(tint);
        }
        private void Update()
        {

        }
    } //Randomises the colour of the projectile, and can make it apply the colour to enemies.
    public class PierceDeadActors : MonoBehaviour
    {
        public PierceDeadActors()
        {
        }
        private void Start()
        {
            this.m_projectile = base.GetComponent<Projectile>();
            this.m_projectile.specRigidbody.OnPreRigidbodyCollision += this.PreCollision;
        }
        private void PreCollision(SpeculativeRigidbody myRigidbody, PixelCollider myPixelCollider, SpeculativeRigidbody otherRigidbody, PixelCollider otherPixelCollider)
        {
            if (myRigidbody != null && otherRigidbody != null)
            {
                if (otherRigidbody.healthHaver != null && otherRigidbody.healthHaver.IsDead)
                {
                    PhysicsEngine.SkipCollision = true;
                }
            }
        }
        private Projectile m_projectile;
    } //Causes the projectiles to piece enemies that have already died, but still have a hitbox.
    public class BulletLifeTimer : MonoBehaviour
    {
        public BulletLifeTimer()
        {
            this.secondsTillDeath = 1;
            this.eraseInsteadOfDie = false;
        }
        private void Start()
        {
            timer = secondsTillDeath;
            this.m_projectile = base.GetComponent<Projectile>();

        }
        private void FixedUpdate()
        {
            if (this.m_projectile != null)
            {
                if (timer > 0)
                {
                    timer -= BraveTime.DeltaTime;
                }
                if (timer <= 0)
                {
                    if (eraseInsteadOfDie) UnityEngine.Object.Destroy(this.m_projectile.gameObject);
                    else this.m_projectile.DieInAir();
                }
            }
        }
        public float secondsTillDeath;
        public bool eraseInsteadOfDie;
        private float timer;
        private Projectile m_projectile;
    } //Gives the projectile a time-based lifespan, after which it will die.
    public class InstantDestroyProjOnSpawn : MonoBehaviour
    {
        public InstantDestroyProjOnSpawn()
        {
        }
        private void Start()
        {
            this.m_projectile = base.GetComponent<Projectile>();
            UnityEngine.Object.DestroyImmediate(this.m_projectile.gameObject);
        }
        private Projectile m_projectile;
    } //Prevents the projectile from spawning.
    public class AdvancedMirrorProjectileModifier : MonoBehaviour
    {
        public bool tintsBullets;
        public Color tintColour;
        public bool projectileSurvives;
        public bool postProcessReflectedBullets;
        public bool allowSurvivalIfPiercing;
        public float baseReflectedDMG;
        public float baseRelfectedSpeed;
        public float baseReflectedSpread;
        public float baseReflectedScaleMod;
        public bool retarget;
        public string sfx;
        public int maxMirrors;
        public bool RapidRiposteWeebshitSynergy;
        public AdvancedMirrorProjectileModifier()
        {
            this.tintsBullets = false;
            this.tintColour = Color.white;
            this.projectileSurvives = false;
            this.postProcessReflectedBullets = false;
            this.allowSurvivalIfPiercing = true;
            this.baseReflectedDMG = 15;
            this.baseRelfectedSpeed = 10;
            this.baseReflectedSpread = 0;
            this.baseReflectedScaleMod = 1;
            this.retarget = true;
            this.sfx = null;
            this.maxMirrors = -1;
            this.TimesMirrored = 0;
            this.RapidRiposteWeebshitSynergy = false;
        }
        private void Start()
        {
            this.m_projectile = base.GetComponent<Projectile>();
            if (tintsBullets) { this.m_projectile.AdjustPlayerProjectileTint(tintColour, 2, 0f); }
            this.m_projectile.collidesWithProjectiles = true;
            this.m_projectile.UpdateCollisionMask();
            SpeculativeRigidbody specRigidbody = this.m_projectile.specRigidbody;
            specRigidbody.OnPreRigidbodyCollision += this.HandlePreCollision;
        }
        private void HandlePreCollision(SpeculativeRigidbody myRigidbody, PixelCollider myPixelCollider, SpeculativeRigidbody otherRigidbody, PixelCollider otherPixelCollider)
        {
            if (otherRigidbody && otherRigidbody.projectile)
            {
                if (TimesMirrored < maxMirrors || maxMirrors == -1)
                {
                    if (otherRigidbody.projectile.Owner is AIActor)
                    {
                        if (!projectileSurvives)
                        {
                            if (allowSurvivalIfPiercing)
                            {
                                PierceProjModifier piercing = myRigidbody.projectile.GetComponent<PierceProjModifier>();
                                if (piercing != null)
                                {
                                    if (piercing.penetration > 0) piercing.penetration -= 1;
                                    else myRigidbody.projectile.DieInAir(false, true, true, false);
                                }
                                else myRigidbody.projectile.DieInAir(false, true, true, false);
                            }
                            else myRigidbody.projectile.DieInAir(false, true, true, false);
                        }
                        TimesMirrored++;
                        otherRigidbody.projectile.ReflectBullet(retarget, myRigidbody.projectile.Owner, baseRelfectedSpeed, postProcessReflectedBullets, baseReflectedScaleMod, baseReflectedDMG, baseReflectedSpread, sfx);
                        if (RapidRiposteWeebshitSynergy) otherRigidbody.projectile.OnHitEnemy += this.HandleGunie;
                    }
                }
                PhysicsEngine.SkipCollision = true;
            }
        }
        private void HandleGunie(Projectile proj, SpeculativeRigidbody enemy, bool fatal)
        {
            if (!fatal && enemy && enemy.aiActor && this.m_projectile.Owner && this.m_projectile.Owner is PlayerController)
            {
                if (UnityEngine.Random.value <= 0.5f)
                {
                    enemy.aiActor.DoGeniePunch((this.m_projectile.Owner as PlayerController));
                }
            }
        }
        private int TimesMirrored = 0;
        private Projectile m_projectile;
    } //Causes the projectile to reflect bullets, mirror bullets style, but with more variety.
    public class RandomRoomPosBehaviour : MonoBehaviour
    {
        public RandomRoomPosBehaviour()
        {
        }
        private void Start()
        {
            this.m_projectile = base.GetComponent<Projectile>();
            IntVector2 newPosition = this.m_projectile.GetAbsoluteRoom().GetRandomVisibleClearSpot(3, 3);
            this.m_projectile.transform.position = newPosition.ToVector3();
            this.m_projectile.specRigidbody.Reinitialize();
        }
        private Projectile m_projectile;
    } //Teleports the bullet to a random position in the room on spawn.
    public class GravitronBulletsBehaviour : MonoBehaviour
    {
        public GravitronBulletsBehaviour()
        {
            this.orbitalLifespan = 15;
            this.cappedOrbiters = 20;
            this.orbitersCollideWithTilemap = false;
            this.orbitalGroup = 3;

            this.resetTravelledDistanceOnOrbit = true;
            this.alterProjRangeOnOrbit = true;
            this.baseProjectileRangePrioritisedIfLarger = true;
            this.resetSpeedIfOverCappedValueOnOrbit = true;
            this.speedCap = 50;
            this.speedResetValue = 20;
            this.targetRange = 500;
            this.damageMultOnOrbitStart = 2;

            this.minOrbitalRadius = 2f;
            this.maxOrbitalRadius = 5f;

            this.hasAlreadyOrbited = false;
        }
        public void Start()
        {
            try
            {
                this.m_projectile = base.GetComponent<Projectile>();

                bool canDo = true;
                if (this.m_projectile is InstantDamageOneEnemyProjectile) canDo = false;
                if (this.m_projectile is InstantlyDamageAllProjectile) canDo = false;
                if (this.m_projectile.GetComponent<ArtfulDodgerProjectileController>()) canDo = false;


                if (canDo)
                {
                    PierceProjModifier orAddComponent = this.m_projectile.gameObject.GetOrAddComponent<PierceProjModifier>();
                    orAddComponent.penetration = Mathf.Max(orAddComponent.penetration, 20);
                    orAddComponent.penetratesBreakables = true;
                    this.m_projectile.OnHitEnemy += this.HandleStartOrbit;
                }
            }
            catch (Exception e)
            {
                ETGModConsole.Log(e.Message);
                ETGModConsole.Log(e.StackTrace);
            }
        }
        private void HandleStartOrbit(Projectile proj, SpeculativeRigidbody enemy, bool fatal)
        {
            if (!fatal && !this.hasAlreadyOrbited)
            {
                int orbitersInGroup = OrbitProjectileMotionModule.GetOrbitersInGroup(this.orbitalGroup);
                if (orbitersInGroup >= cappedOrbiters)
                {
                    return;
                }
                proj.specRigidbody.CollideWithTileMap = this.orbitersCollideWithTilemap;
                if (resetTravelledDistanceOnOrbit) proj.ResetDistance();
                if (alterProjRangeOnOrbit)
                {
                    if (baseProjectileRangePrioritisedIfLarger) proj.baseData.range = Mathf.Max(proj.baseData.range, this.targetRange);
                    else proj.baseData.range = this.targetRange;
                }
                if (resetSpeedIfOverCappedValueOnOrbit && proj.baseData.speed > speedCap)
                {
                    proj.baseData.speed = speedResetValue;
                    proj.UpdateSpeed();
                }
                proj.baseData.damage *= this.damageMultOnOrbitStart;

                OrbitProjectileMotionModule orbitProjectileMotionModule = new OrbitProjectileMotionModule();
                orbitProjectileMotionModule.lifespan = this.orbitalLifespan;
                orbitProjectileMotionModule.MinRadius = this.minOrbitalRadius;
                orbitProjectileMotionModule.MaxRadius = this.maxOrbitalRadius;
                orbitProjectileMotionModule.usesAlternateOrbitTarget = true;
                orbitProjectileMotionModule.OrbitGroup = this.orbitalGroup;
                orbitProjectileMotionModule.alternateOrbitTarget = enemy;
                if (proj.OverrideMotionModule != null && proj.OverrideMotionModule is HelixProjectileMotionModule)
                {
                    orbitProjectileMotionModule.StackHelix = true;
                    orbitProjectileMotionModule.ForceInvert = (proj.OverrideMotionModule as HelixProjectileMotionModule).ForceInvert;
                }
                proj.OverrideMotionModule = orbitProjectileMotionModule;
                this.hasAlreadyOrbited = true;
            }
        }
        private bool hasAlreadyOrbited;
        private Projectile m_projectile;
        public bool orbitersCollideWithTilemap;
        public bool resetTravelledDistanceOnOrbit;
        public bool alterProjRangeOnOrbit;
        public float targetRange;
        public bool baseProjectileRangePrioritisedIfLarger;
        public bool resetSpeedIfOverCappedValueOnOrbit;
        public float speedCap;
        public float speedResetValue;
        public int cappedOrbiters;
        public float orbitalLifespan;
        public int orbitalGroup;
        public float minOrbitalRadius;
        public float maxOrbitalRadius;
        public float damageMultOnOrbitStart;
    } //Causes the Bullets to orbit enemies they hit.
    public class EasyTransmogrifyComponent : MonoBehaviour
    {
        public EasyTransmogrifyComponent()
        {

        }
        private void Start()
        {
            self = base.GetComponent<Projectile>();
            if (self) self.OnHitEnemy += this.OnHitEnemy;
        }
        private void OnHitEnemy(Projectile bullet, SpeculativeRigidbody enemy, bool fatal)
        {
            if (bullet && enemy && enemy.aiActor && enemy.healthHaver && !fatal && !enemy.healthHaver.IsBoss)
            {
                List<TransmogData> RandomisedList = RandomiseListOrder(TransmogDataList);
                foreach (TransmogData data in RandomisedList)
                {
                    if (UnityEngine.Random.value <= data.TransmogChance)
                    {
                        enemy.aiActor.Transmogrify(EnemyDatabase.GetOrLoadByGuid(data.TargetGuid), (GameObject)ResourceCache.Acquire("Global VFX/VFX_Item_Spawn_Poof"));
                        return;
                    }
                }
            }
        }
        private List<TransmogData> RandomiseListOrder(List<TransmogData> oldList)
        {
            List<TransmogData> oldList2 = new List<TransmogData>();
            oldList2.AddRange(oldList);
            List<TransmogData> newList = new List<TransmogData>();
            int oldListcount = oldList2.Count;
            for (int i = 0; i < oldListcount; i++)
            {
                TransmogData selectedData = BraveUtility.RandomElement(oldList2);
                newList.Add(selectedData);
                oldList2.Remove(selectedData);
            }
            return newList;
        }
        private Projectile self;
        public List<TransmogData> TransmogDataList = new List<TransmogData>();
        public class TransmogData
        {
            public string TargetGuid;
            public float TransmogChance;
            public string identifier;
        }
    } //Allows for much easier transmogrification 
    public class InstaKillEnemyTypeBehaviour : MonoBehaviour
    {
        public InstaKillEnemyTypeBehaviour()
        {
            bossBonusDMG = 1;
        }
        public void Start()
        {
            this.m_projectile = base.GetComponent<Projectile>();
            if (m_projectile && EnemyTypeToKill != null && EnemyTypeToKill.Count > 0)
            {
                this.m_projectile.OnHitEnemy += this.OnHitEnemy;
            }
        }
        private void OnHitEnemy(Projectile bullet, SpeculativeRigidbody enemy, bool fatal)
        {
            if (enemy && enemy.aiActor && enemy.healthHaver)
            {
                if (!fatal)
                {
                    if (EnemyTypeToKill.Contains(enemy.aiActor.EnemyGuid) && !BossesToBonusDMG.Contains(enemy.aiActor.EnemyGuid))
                    {
                        enemy.healthHaver.ApplyDamage(1E+07f, Vector2.zero, "Erasure", CoreDamageTypes.None, DamageCategory.Unstoppable, true, null, false);
                    }
                    if (BossesToBonusDMG.Contains(enemy.aiActor.EnemyGuid))
                    {
                        enemy.healthHaver.ApplyDamage(bossBonusDMG, Vector2.zero, "Erasure", CoreDamageTypes.None, DamageCategory.Environment, true, null, false);
                    }
                }
            }
        }
        public float bossBonusDMG;
        private Projectile m_projectile;
        public List<string> EnemyTypeToKill = new List<string>();
        public List<string> BossesToBonusDMG = new List<string>();
    } //Allows for easy insta-killing
    public class ProjectileSlashingBehaviour : MonoBehaviour
    {
        public ProjectileSlashingBehaviour()
        {
            DestroyBaseAfterFirstSlash = false;
            timeBetweenSlashes = 1;
            DoSound = true;
            slashKnockback = 5;
            SlashDamage = 15;
            playerKnockback = 1;
            SlashDamageUsesBaseProjectileDamage = true;
            InteractMode = SlashDoer.ProjInteractMode.IGNORE;
        }
        private void Start()
        {
            this.m_projectile = base.GetComponent<Projectile>();
            if (this.m_projectile.Owner && this.m_projectile.Owner is PlayerController) this.owner = this.m_projectile.Owner as PlayerController;
        }
        private void Update()
        {
            if (this.m_projectile)
            {
                if (timer > 0)
                {
                    timer -= BraveTime.DeltaTime;
                }
                if (timer <= 0)
                {
                    this.m_projectile.StartCoroutine(DoSlash(0, 0));
                    if (doSpinAttack)
                    {
                        this.m_projectile.StartCoroutine(DoSlash(90, 0.15f));
                        this.m_projectile.StartCoroutine(DoSlash(180, 0.30f));
                        this.m_projectile.StartCoroutine(DoSlash(-90, 0.45f));
                    }
                    timer = timeBetweenSlashes;
                }
            }
        }
        private IEnumerator DoSlash(float angle, float delay)
        {
            yield return new WaitForSeconds(delay);
            float actDamage = this.SlashDamage;
            float actKnockback = this.slashKnockback;
            float bossDMGMult = 1;
            float jammedDMGMult = 1;

            Projectile proj = this.m_projectile;
            List<GameActorEffect> effects = new List<GameActorEffect>();
            effects.AddRange(proj.statusEffectsToApply);
            if (proj.AppliesFire && UnityEngine.Random.value <= proj.FireApplyChance) effects.Add(proj.fireEffect);
            if (proj.AppliesCharm && UnityEngine.Random.value <= proj.CharmApplyChance) effects.Add(proj.charmEffect);
            if (proj.AppliesCheese && UnityEngine.Random.value <= proj.CheeseApplyChance) effects.Add(proj.cheeseEffect);
            if (proj.AppliesBleed && UnityEngine.Random.value <= proj.BleedApplyChance) effects.Add(proj.bleedEffect);
            if (proj.AppliesFreeze && UnityEngine.Random.value <= proj.FreezeApplyChance) effects.Add(proj.freezeEffect);
            if (proj.AppliesPoison && UnityEngine.Random.value <= proj.PoisonApplyChance) effects.Add(proj.healthEffect);
            if (proj.AppliesSpeedModifier && UnityEngine.Random.value <= proj.SpeedApplyChance) effects.Add(proj.speedEffect);


            if (SlashDamageUsesBaseProjectileDamage)
            {
                actDamage = this.m_projectile.baseData.damage;
                bossDMGMult = this.m_projectile.BossDamageMultiplier;
                jammedDMGMult = this.m_projectile.BlackPhantomDamageMultiplier;
                actKnockback = this.m_projectile.baseData.force;
            }
            SlashDoer.DoSwordSlash(this.m_projectile.specRigidbody.UnitCenter, (this.m_projectile.Direction.ToAngle() + angle), owner, playerKnockback, this.InteractMode, actDamage, actKnockback, effects, null, jammedDMGMult, bossDMGMult);
            if (DoSound) AkSoundEngine.PostEvent("Play_WPN_blasphemy_shot_01", this.m_projectile.gameObject);
            if (DestroyBaseAfterFirstSlash) Invoke("Suicide", 0.01f);
            yield break;
        }
        private void Suicide() { UnityEngine.Object.Destroy(this.m_projectile.gameObject); }
        private float timer;
        public float timeBetweenSlashes;
        public bool doSpinAttack;
        public float playerKnockback;
        public float slashKnockback;
        public bool DoSound;
        public float SlashDamage;
        public bool SlashDamageUsesBaseProjectileDamage;
        public SlashDoer.ProjInteractMode InteractMode;
        public bool DestroyBaseAfterFirstSlash;
        private Projectile m_projectile;
        private PlayerController owner;
    } //Causes the projectile to slash it's way through the air
    public class SpawnEnemyOnBulletSpawn : MonoBehaviour
    {
        public SpawnEnemyOnBulletSpawn()
        {
            this.procChance = 1;
            this.deleteProjAfterSpawn = true;
            this.companioniseEnemy = true;
            this.ignoreSpawnedEnemyForGoodMimic = true;
            this.killSpawnedEnemyOnRoomClear = true;
            this.doPostProcessOnEnemyBullets = true;
            this.scaleEnemyDamage = true;
            this.scaleEnemyProjSize = true;
            this.scaleEnemyProjSpeed = true;
            this.enemyBulletDamage = 10f;
        }
        private void Start()
        {
            this.m_projectile = base.GetComponent<Projectile>();
            if (this.m_projectile.Owner is PlayerController) { this.projOwner = this.m_projectile.Owner as PlayerController; }
            GameManager.Instance.StartCoroutine(handleSpawn());
        }
        private IEnumerator handleSpawn()
        {
            yield return null;
            if (UnityEngine.Random.value <= this.procChance)
            {
                if (guidToSpawn != null)
                {
                    var enemyToSpawn = EnemyDatabase.GetOrLoadByGuid(guidToSpawn);
                    var position = this.m_projectile.specRigidbody.UnitCenter;
                    Instantiate<GameObject>(EasyVFXDatabase.SpiratTeleportVFX, position, Quaternion.identity);

                    AIActor TargetActor = AIActor.Spawn(enemyToSpawn, position, GameManager.Instance.Dungeon.data.GetAbsoluteRoomFromPosition(position.ToIntVector2()), true, AIActor.AwakenAnimationType.Default, true);

                    PhysicsEngine.Instance.RegisterOverlappingGhostCollisionExceptions(TargetActor.specRigidbody, null, false);

                    if (ignoreSpawnedEnemyForGoodMimic)
                    {
                        CustomEnemyTagsSystem tags = TargetActor.gameObject.GetOrAddComponent<CustomEnemyTagsSystem>();
                        tags.ignoreForGoodMimic = true;
                    }

                    if (companioniseEnemy && this.projOwner != null)
                    {
                        CompanionController orAddComponent = TargetActor.gameObject.GetOrAddComponent<CompanionController>();
                        orAddComponent.companionID = CompanionController.CompanionIdentifier.NONE;
                        orAddComponent.Initialize(this.projOwner);

                        CompanionisedEnemyBulletModifiers companionisedBullets = TargetActor.gameObject.GetOrAddComponent<CompanionisedEnemyBulletModifiers>();
                        companionisedBullets.jammedDamageMultiplier = 2f;
                        companionisedBullets.TintBullets = true;
                        companionisedBullets.TintColor = ExtendedColours.honeyYellow;
                        companionisedBullets.baseBulletDamage = enemyBulletDamage;
                        companionisedBullets.scaleDamage = this.scaleEnemyDamage;
                        companionisedBullets.doPostProcess = this.doPostProcessOnEnemyBullets;
                        companionisedBullets.scaleSize = this.scaleEnemyProjSize;
                        companionisedBullets.scaleSpeed = this.scaleEnemyProjSpeed;
                        companionisedBullets.enemyOwner = this.projOwner;
                    }

                    if (killSpawnedEnemyOnRoomClear)
                    {
                        TargetActor.gameObject.AddComponent<KillOnRoomClear>();
                    }

                    TargetActor.IsHarmlessEnemy = true;
                    TargetActor.IgnoreForRoomClear = true;

                    if (TargetActor.gameObject.GetComponent<SpawnEnemyOnDeath>())
                    {
                        Destroy(TargetActor.gameObject.GetComponent<SpawnEnemyOnDeath>());
                    }
                    if (deleteProjAfterSpawn) { Destroy(this.m_projectile.gameObject); }
                }
            }
        }
        private Projectile m_projectile;
        private PlayerController projOwner;
        public float procChance;
        public float enemyBulletDamage;
        public bool companioniseEnemy;
        public bool killSpawnedEnemyOnRoomClear;
        public bool deleteProjAfterSpawn;
        public bool ignoreSpawnedEnemyForGoodMimic;
        public string guidToSpawn;
        public bool scaleEnemyDamage;
        public bool scaleEnemyProjSize;
        public bool scaleEnemyProjSpeed;
        public bool doPostProcessOnEnemyBullets;
    } //Causes the projectile to spawn an enemy upon it's creation.

    //PASSIVE ITEM EFFECTS AS COMPONENTS
    public class AngryBulletsProjectileBehaviour : MonoBehaviour
    {
        public AngryBulletsProjectileBehaviour()
        {
            this.ApplyRandomBounceOffEnemy = true;
            this.ChanceToSeekEnemyOnBounce = 0.5f;
            this.ActivationsPerSecond = 1f;
            this.MinActivationChance = 0.05f;
        }

        public void Start()
        {
            try
            {
                this.m_projectile = base.GetComponent<Projectile>();
                this.m_projectile.OnHitEnemy += this.HandleProjectileHitEnemy;
            }
            catch (Exception e)
            {
                ETGModConsole.Log(e.Message);
                ETGModConsole.Log(e.StackTrace);
            }
        }
        private void HandleProjectileHitEnemy(Projectile obj, SpeculativeRigidbody enemy, bool killed)
        {
            if (this.ApplyRandomBounceOffEnemy)
            {
                PierceProjModifier orAddComponent = obj.gameObject.GetOrAddComponent<PierceProjModifier>();
                orAddComponent.penetratesBreakables = true;
                orAddComponent.penetration++;
                HomingModifier component = obj.gameObject.GetComponent<HomingModifier>();
                if (component)
                {
                    component.AngularVelocity *= 0.75f;
                }
                Vector2 dirVec = UnityEngine.Random.insideUnitCircle;
                float num = this.ChanceToSeekEnemyOnBounce;
                Gun possibleSourceGun = obj.PossibleSourceGun;
                if (this.NormalizeAcrossFireRate && possibleSourceGun)
                {
                    float num2 = 1f / possibleSourceGun.DefaultModule.cooldownTime;
                    if (possibleSourceGun.Volley != null && possibleSourceGun.Volley.UsesShotgunStyleVelocityRandomizer)
                    {
                        num2 *= (float)Mathf.Max(1, possibleSourceGun.Volley.projectiles.Count);
                    }
                    num = Mathf.Clamp01(this.ActivationsPerSecond / num2);
                    num = Mathf.Max(this.MinActivationChance, num);
                }
                if (UnityEngine.Random.value < num && enemy.aiActor)
                {
                    Func<AIActor, bool> isValid = (AIActor a) => a && a.HasBeenEngaged && a.healthHaver && a.healthHaver.IsVulnerable;
                    AIActor closestToPosition = BraveUtility.GetClosestToPosition<AIActor>(enemy.aiActor.ParentRoom.GetActiveEnemies(RoomHandler.ActiveEnemyType.All), enemy.UnitCenter, isValid, new AIActor[]
                    {
                    enemy.aiActor
                    });
                    if (closestToPosition)
                    {
                        dirVec = closestToPosition.CenterPosition - obj.transform.position.XY();
                    }
                }
                obj.SendInDirection(dirVec, false, true);
            }
        }
        private Projectile m_projectile;
        public bool ApplyRandomBounceOffEnemy;
        public float ChanceToSeekEnemyOnBounce;
        public bool NormalizeAcrossFireRate;
        public float ActivationsPerSecond;
        public float MinActivationChance;
    } //The Angry Bullets Effect as a Projectile Component.
    public class FlakBulletsProjectileBehaviour : MonoBehaviour
    {
        public FlakBulletsProjectileBehaviour()
        {
            this.ScaleMod = 0.5f;
            this.InheritsAppearance = true;
            this.NumberOfFlakBullets = 3;
            this.projectileToSpawn = Gungeon.Game.Items["flak_bullets"].GetComponent<ComplexProjectileModifier>().CollisionSpawnProjectile;
        }

        public void Start()
        {
            try
            {
                this.m_projectile = base.GetComponent<Projectile>();

                SpawnProjModifier spawnProjModifier = this.m_projectile.gameObject.AddComponent<SpawnProjModifier>();
                spawnProjModifier.SpawnedProjectilesInheritAppearance = this.InheritsAppearance;
                spawnProjModifier.SpawnedProjectileScaleModifier = this.ScaleMod;
                spawnProjModifier.SpawnedProjectilesInheritData = true;
                spawnProjModifier.spawnProjectilesOnCollision = true;
                spawnProjModifier.spawnProjecitlesOnDieInAir = true;
                spawnProjModifier.doOverrideObjectCollisionSpawnStyle = true;
                spawnProjModifier.startAngle = UnityEngine.Random.Range(0, 180);
                int numberToSpawnOnCollison = this.NumberOfFlakBullets;
                if (this.m_projectile.SpawnedFromOtherPlayerProjectile) numberToSpawnOnCollison = 2;

                spawnProjModifier.numberToSpawnOnCollison = numberToSpawnOnCollison;
                spawnProjModifier.projectileToSpawnOnCollision = this.projectileToSpawn;
                spawnProjModifier.collisionSpawnStyle = SpawnProjModifier.CollisionSpawnStyle.FLAK_BURST;
            }
            catch (Exception e)
            {
                ETGModConsole.Log(e.Message);
                ETGModConsole.Log(e.StackTrace);
            }
        }
        private Projectile m_projectile;
        public int NumberOfFlakBullets;
        public float ScaleMod;
        public bool InheritsAppearance;
        public Projectile projectileToSpawn;
    } //The Flak Bullets Effect as a Projectile Component.
    public class RemoteBulletsProjectileBehaviour : MonoBehaviour
    {
        public RemoteBulletsProjectileBehaviour()
        {
            this.trackingSpeed = Gungeon.Game.Items["remote_bullets"].GetComponent<GuidedBulletsPassiveItem>().trackingSpeed;
            this.trackingCurve = Gungeon.Game.Items["remote_bullets"].GetComponent<GuidedBulletsPassiveItem>().trackingCurve;
            this.trackingTime = Gungeon.Game.Items["remote_bullets"].GetComponent<GuidedBulletsPassiveItem>().trackingTime;
        }

        public void Start()
        {
            try
            {
                this.m_projectile = base.GetComponent<Projectile>();
                this.m_projectile.PreMoveModifiers += this.PreMoveProjectileModifier;
            }
            catch (Exception e)
            {
                ETGModConsole.Log(e.Message);
                ETGModConsole.Log(e.StackTrace);
            }
        }
        private void PreMoveProjectileModifier(Projectile p)
        {
            if (p && p.Owner is PlayerController)
            {
                BraveInput instanceForPlayer = BraveInput.GetInstanceForPlayer((p.Owner as PlayerController).PlayerIDX);
                if (instanceForPlayer == null) return;
                Vector2 vector = Vector2.zero;
                if (instanceForPlayer.IsKeyboardAndMouse(false))
                {
                    vector = (p.Owner as PlayerController).unadjustedAimPoint.XY() - p.specRigidbody.UnitCenter;
                }
                else
                {
                    if (instanceForPlayer.ActiveActions == null)
                    {
                        return;
                    }
                    vector = instanceForPlayer.ActiveActions.Aim.Vector;
                }
                float num = vector.ToAngle();
                float num2 = BraveMathCollege.Atan2Degrees(p.Direction);
                float num3 = 0f;
                if (p.ElapsedTime < this.trackingTime)
                {
                    num3 = this.trackingCurve.Evaluate(p.ElapsedTime / this.trackingTime) * this.trackingSpeed;
                }
                float num4 = Mathf.MoveTowardsAngle(num2, num, num3 * BraveTime.DeltaTime);
                Vector2 vector2 = Quaternion.Euler(0f, 0f, Mathf.DeltaAngle(num2, num4)) * p.Direction;
                if (p is HelixProjectile)
                {
                    HelixProjectile helixProjectile = p as HelixProjectile;
                    helixProjectile.AdjustRightVector(Mathf.DeltaAngle(num2, num4));
                }
                if (p.OverrideMotionModule != null)
                {
                    p.OverrideMotionModule.AdjustRightVector(Mathf.DeltaAngle(num2, num4));
                }
                p.Direction = vector2.normalized;
                if (p.shouldRotate)
                {
                    p.transform.eulerAngles = new Vector3(0f, 0f, p.Direction.ToAngle());
                }
            }
        }
        private Projectile m_projectile;
        public float trackingSpeed = 45f;
        public float trackingTime = 6f;
        [CurveRange(0f, 0f, 1f, 1f)]
        public AnimationCurve trackingCurve;
    } //The Remote Bullets effect as a Projectile Component.
    public class OrbitalBulletsBehaviour : MonoBehaviour
    {
        public OrbitalBulletsBehaviour()
        {
            this.orbitalLifespan = 15;
            this.cappedOrbiters = 20;
            this.orbitersCollideWithTilemap = false;
            this.orbitalGroup = -1;

            this.resetTravelledDistanceOnOrbit = true;
            this.alterProjRangeOnOrbit = true;
            this.baseProjectileRangePrioritisedIfLarger = true;
            this.resetSpeedIfOverCappedValueOnOrbit = true;
            this.speedCap = 50;
            this.speedResetValue = 20;
            this.targetRange = 500;

            this.minOrbitalRadius = 2f;
            this.maxOrbitalRadius = 5f;

            this.usesOverrideCenter = false;
            this.overrideCenter = null;
        }
        public void Start()
        {
            try
            {
                this.m_projectile = base.GetComponent<Projectile>();

                bool canDo = true;
                if (this.m_projectile is InstantDamageOneEnemyProjectile) canDo = false;
                if (this.m_projectile is InstantlyDamageAllProjectile) canDo = false;
                if (this.m_projectile.GetComponent<ArtfulDodgerProjectileController>()) canDo = false;

                if (canDo)
                {
                    BounceProjModifier orAddComponent = this.m_projectile.gameObject.GetOrAddComponent<BounceProjModifier>();
                    orAddComponent.numberOfBounces = Mathf.Max(orAddComponent.numberOfBounces, 1);
                    orAddComponent.onlyBounceOffTiles = true;
                    BounceProjModifier bounceProjModifier = orAddComponent;
                    bounceProjModifier.OnBounceContext += this.HandleStartOrbit;
                }
            }
            catch (Exception e)
            {
                ETGModConsole.Log(e.Message);
                ETGModConsole.Log(e.StackTrace);
            }
        }
        private void HandleStartOrbit(BounceProjModifier mod, SpeculativeRigidbody srb)
        {
            int orbitersInGroup = OrbitProjectileMotionModule.GetOrbitersInGroup(this.orbitalGroup);
            if (orbitersInGroup >= cappedOrbiters)
            {
                return;
            }
            mod.projectile.specRigidbody.CollideWithTileMap = this.orbitersCollideWithTilemap;
            if (resetTravelledDistanceOnOrbit) mod.projectile.ResetDistance();
            if (alterProjRangeOnOrbit)
            {
                if (baseProjectileRangePrioritisedIfLarger) mod.projectile.baseData.range = Mathf.Max(mod.projectile.baseData.range, this.targetRange);
                else mod.projectile.baseData.range = this.targetRange;
            }
            if (resetSpeedIfOverCappedValueOnOrbit && mod.projectile.baseData.speed > speedCap)
            {
                mod.projectile.baseData.speed = speedResetValue;
                mod.projectile.UpdateSpeed();
            }

            OrbitProjectileMotionModule orbitProjectileMotionModule = new OrbitProjectileMotionModule();
            orbitProjectileMotionModule.lifespan = this.orbitalLifespan;
            orbitProjectileMotionModule.MinRadius = this.minOrbitalRadius;
            orbitProjectileMotionModule.MaxRadius = this.maxOrbitalRadius;
            orbitProjectileMotionModule.OrbitGroup = this.orbitalGroup;
            orbitProjectileMotionModule.usesAlternateOrbitTarget = this.usesOverrideCenter;
            orbitProjectileMotionModule.alternateOrbitTarget = this.overrideCenter;
            if (mod.projectile.OverrideMotionModule != null && mod.projectile.OverrideMotionModule is HelixProjectileMotionModule)
            {
                orbitProjectileMotionModule.StackHelix = true;
                orbitProjectileMotionModule.ForceInvert = (mod.projectile.OverrideMotionModule as HelixProjectileMotionModule).ForceInvert;
            }
            mod.projectile.OverrideMotionModule = orbitProjectileMotionModule;
        }
        private Projectile m_projectile;
        public bool orbitersCollideWithTilemap;
        public bool resetTravelledDistanceOnOrbit;
        public bool alterProjRangeOnOrbit;
        public float targetRange;
        public bool baseProjectileRangePrioritisedIfLarger;
        public bool resetSpeedIfOverCappedValueOnOrbit;
        public float speedCap;
        public float speedResetValue;
        public int cappedOrbiters;
        public float orbitalLifespan;
        public int orbitalGroup;
        public float minOrbitalRadius;
        public float maxOrbitalRadius;
        public bool usesOverrideCenter;
        public SpeculativeRigidbody overrideCenter;
    } //Orbital Bullets as a Projectile Component.
    public class ChaosBulletsModifierComp : MonoBehaviour
    {
        public ChaosBulletsModifierComp()
        {
            //BASIC STATS
            effectScaler = 1;
            chanceToAddPierce = 0.1f;
            chanceToAddBounce = 0.1f;
            chanceToAddFat = 0.1f;
            minFatScale = 1.25f;
            maxFatScale = 1.75f;
            usesVelocityModificationCurve = true;
            VelocityModificationCurve = Gungeon.Game.Items["chaos_bullets"].GetComponent<ChaosBulletsItem>().VelocityModificationCurve;
            chanceOfActivatingStatusEffect = 0.1f;

            //--------------------------------MODIFIERS
            //SPEED
            speedModifierWeight = 2;
            speedModifierTintColour = Gungeon.Game.Items["chaos_bullets"].GetComponent<ChaosBulletsItem>().SpeedTintColor;
            speedModifierEffect = Gungeon.Game.Items["chaos_bullets"].GetComponent<ChaosBulletsItem>().SpeedModifierEffect;
            //POISON
            poisonModifierWeight = 2;
            poisonModifierTintColour = Gungeon.Game.Items["chaos_bullets"].GetComponent<ChaosBulletsItem>().PoisonTintColor;
            poisonModifierEffect = Gungeon.Game.Items["chaos_bullets"].GetComponent<ChaosBulletsItem>().HealthModifierEffect;
            //FIRE
            fireModifierWeight = 2;
            fireModifierTintColour = Gungeon.Game.Items["chaos_bullets"].GetComponent<ChaosBulletsItem>().FireTintColor;
            fireModifierEffect = Gungeon.Game.Items["chaos_bullets"].GetComponent<ChaosBulletsItem>().FireModifierEffect;
            //CHARM
            charmModifierWeight = 1;
            charmModifierTintColour = Gungeon.Game.Items["chaos_bullets"].GetComponent<ChaosBulletsItem>().CharmTintColor;
            charmModifierEffect = Gungeon.Game.Items["chaos_bullets"].GetComponent<ChaosBulletsItem>().CharmModifierEffect;
            //FREEZE
            freezeModifierWeight = 1;
            freezeModifierTintColour = Gungeon.Game.Items["chaos_bullets"].GetComponent<ChaosBulletsItem>().FreezeTintColor;
            freezeModifierEffect = Gungeon.Game.Items["chaos_bullets"].GetComponent<ChaosBulletsItem>().FreezeModifierEffect;
            //TRANSMOG
            transmogModifierWeight = 0.05f;
            transmogModifierTintColour = Gungeon.Game.Items["chaos_bullets"].GetComponent<ChaosBulletsItem>().TransmogrifyTintColor;
            transmogTargetGuid = "76bc43539fc24648bff4568c75c686d1";
        }

        public void Start()
        {
            try
            {
                this.m_projectile = base.GetComponent<Projectile>();
                DoEffects(this.m_projectile);
            }
            catch (Exception e)
            {
                ETGModConsole.Log(e.Message);
                ETGModConsole.Log(e.StackTrace);
            }
        }
        private void DoEffects(Projectile projectile)
        {
            if (usesVelocityModificationCurve)
            {
                projectile.baseData.speed *= this.VelocityModificationCurve.Evaluate(UnityEngine.Random.value);
            }
            int num = 0;
            while (UnityEngine.Random.value < this.chanceToAddBounce && num < 10)
            {
                num++;
                BounceProjModifier bounceProjModifier = projectile.GetComponent<BounceProjModifier>();
                if (bounceProjModifier == null)
                {
                    bounceProjModifier = projectile.gameObject.AddComponent<BounceProjModifier>();
                    bounceProjModifier.numberOfBounces = 1;
                }
                else
                {
                    bounceProjModifier.numberOfBounces++;
                }
            }
            num = 0;
            while (UnityEngine.Random.value < this.chanceToAddPierce && num < 10)
            {
                num++;
                PierceProjModifier pierceProjModifier = projectile.GetComponent<PierceProjModifier>();
                if (pierceProjModifier == null)
                {
                    pierceProjModifier = projectile.gameObject.AddComponent<PierceProjModifier>();
                    pierceProjModifier.penetration = 2;
                    pierceProjModifier.penetratesBreakables = true;
                    pierceProjModifier.BeastModeLevel = PierceProjModifier.BeastModeStatus.NOT_BEAST_MODE;
                }
                else
                {
                    pierceProjModifier.penetration += 2;
                }
            }
            if (UnityEngine.Random.value < this.chanceToAddFat)
            { projectile.AdditionalScaleMultiplier *= UnityEngine.Random.Range(this.minFatScale, this.maxFatScale); }

            if (this.chanceOfActivatingStatusEffect < 1f)
            {
                this.chanceOfActivatingStatusEffect *= effectScaler;
            }
            if (UnityEngine.Random.value < chanceOfActivatingStatusEffect)
            {
                Color targetTintColor = Color.white;
                float combinedWeights = this.speedModifierWeight + this.poisonModifierWeight + this.freezeModifierWeight + this.charmModifierWeight + this.fireModifierWeight + this.transmogModifierWeight;
                float selectedWeight = combinedWeights * UnityEngine.Random.value;
                if (selectedWeight < this.speedModifierWeight)
                {
                    //SPEED
                    targetTintColor = this.speedModifierTintColour;
                    projectile.statusEffectsToApply.Add(this.speedModifierEffect);
                }
                else if (selectedWeight < this.speedModifierWeight + this.poisonModifierWeight)
                {
                    //POISON
                    targetTintColor = this.poisonModifierTintColour;
                    projectile.statusEffectsToApply.Add(this.poisonModifierEffect);
                }
                else if (selectedWeight < this.speedModifierWeight + this.poisonModifierWeight + this.freezeModifierWeight)
                {
                    //FREEZE
                    targetTintColor = this.freezeModifierTintColour;
                    projectile.statusEffectsToApply.Add(freezeModifierEffect);
                }
                else if (selectedWeight < this.speedModifierWeight + this.poisonModifierWeight + this.freezeModifierWeight + this.charmModifierWeight)
                {
                    //CHARM
                    targetTintColor = this.charmModifierTintColour;
                    projectile.statusEffectsToApply.Add(this.charmModifierEffect);
                }
                else if (selectedWeight < this.speedModifierWeight + this.poisonModifierWeight + this.freezeModifierWeight + this.charmModifierWeight + this.fireModifierWeight)
                {
                    //FIRE
                    targetTintColor = this.fireModifierTintColour;
                    projectile.statusEffectsToApply.Add(this.fireModifierEffect);
                }
                else if (selectedWeight < this.speedModifierWeight + this.poisonModifierWeight + this.freezeModifierWeight + this.charmModifierWeight + this.fireModifierWeight + this.transmogModifierWeight)
                {
                    //TRANSMOGRIFY
                    targetTintColor = this.transmogModifierTintColour;
                    projectile.CanTransmogrify = true;
                    projectile.ChanceToTransmogrify = 1f;
                    projectile.TransmogrifyTargetGuids = new string[1];
                    projectile.TransmogrifyTargetGuids[0] = this.transmogTargetGuid;
                }

                projectile.AdjustPlayerProjectileTint(targetTintColor, 6, 0f);

            }
        }

        private Projectile m_projectile;
        //Non status effect things
        public float effectScaler;
        public float chanceToAddPierce;
        public float chanceToAddBounce;
        public float chanceToAddFat;
        public float maxFatScale;
        public float minFatScale;
        //Velocity
        public bool usesVelocityModificationCurve;
        public AnimationCurve VelocityModificationCurve;
        //Effect weights
        public float chanceOfActivatingStatusEffect;
        //Speed
        public float speedModifierWeight;
        public GameActorSpeedEffect speedModifierEffect;
        public Color speedModifierTintColour;
        //Poison
        public float poisonModifierWeight;
        public GameActorHealthEffect poisonModifierEffect;
        public Color poisonModifierTintColour;
        //Fire
        public float fireModifierWeight;
        public GameActorFireEffect fireModifierEffect;
        public Color fireModifierTintColour;
        //Charm
        public float charmModifierWeight;
        public GameActorCharmEffect charmModifierEffect;
        public Color charmModifierTintColour;
        //Freeze
        public float freezeModifierWeight;
        public GameActorFreezeEffect freezeModifierEffect;
        public Color freezeModifierTintColour;
        //Transmog
        public float transmogModifierWeight;
        public string transmogTargetGuid;
        public Color transmogModifierTintColour;

    } //Chaos Bullets as a Projectile Component.

    //BEAM COMPONENTS!
    public class SpawnProjectileAtBeamPoint : MonoBehaviour
    {
        public SpawnProjectileAtBeamPoint()
        {
            this.tickCooldown = 0.1f;
            this.maxDistance = 1;
            this.minDistance = 0;
            this.pickRandomPosition = true;
            this.chanceToFirePerTick = 0.5f;
            this.projectileToFire = UnityEngine.Object.Instantiate(((Gun)ETGMod.Databases.Items[86]).DefaultModule.projectiles[0]);
            this.doPostProcess = true;
            this.addFromBulletWithGunComponent = false;
        }
        public float minDistance;
        public float maxDistance;
        public bool pickRandomPosition;
        public float tickCooldown;
        public float chanceToFirePerTick;
        public Projectile projectileToFire;
        public bool doPostProcess;
        public bool addFromBulletWithGunComponent;
        private void Start()
        {
            this.m_projectile = base.GetComponent<Projectile>();
            this.m_beam = base.GetComponent<BeamController>();
            this.m_basicBeam = base.GetComponent<BasicBeamController>();
            if (this.m_projectile.Owner is PlayerController) this.m_owner = this.m_projectile.Owner as PlayerController;
        }
        private Projectile m_projectile;
        private BeamController m_beam;
        private BasicBeamController m_basicBeam;
        private PlayerController m_owner;
        private void DoProjectileSpawn()
        {
            Vector2 projectileSpawnPosition = this.m_basicBeam.GetPointOnBeam(UnityEngine.Random.value);
            Vector2 nearestEnemyPosition = projectileSpawnPosition.GetPositionOfNearestEnemy(true, true);
            if (nearestEnemyPosition != Vector2.zero)
            {
                GameObject gameObject = ProjSpawnHelper.SpawnProjectileTowardsPoint(this.projectileToFire.gameObject, projectileSpawnPosition, nearestEnemyPosition, 0, 5);
                Projectile component = gameObject.GetComponent<Projectile>();
                if (component != null)
                {
                    component.Owner = this.m_owner;
                    component.TreatedAsNonProjectileForChallenge = true;
                    component.Shooter = this.m_owner.specRigidbody;
                    component.collidesWithPlayer = false;
                    if (this.addFromBulletWithGunComponent) component.gameObject.GetOrAddComponent<BulletsWithGuns.BulletFromBulletWithGun>();
                    //Stats
                    if (this.doPostProcess)
                    {
                        component.baseData.damage *= this.m_owner.stats.GetStatValue(PlayerStats.StatType.Damage);
                        component.baseData.speed *= this.m_owner.stats.GetStatValue(PlayerStats.StatType.ProjectileSpeed);
                        component.baseData.range *= this.m_owner.stats.GetStatValue(PlayerStats.StatType.RangeMultiplier);
                        component.AdditionalScaleMultiplier *= this.m_owner.stats.GetStatValue(PlayerStats.StatType.PlayerBulletScale); ;
                        component.baseData.force *= this.m_owner.stats.GetStatValue(PlayerStats.StatType.KnockbackMultiplier);
                        component.BossDamageMultiplier *= this.m_owner.stats.GetStatValue(PlayerStats.StatType.DamageToBosses);
                        this.m_owner.DoPostProcessProjectile(component);
                    }
                }
            }
        }
        private void FixedUpdate()
        {
            if (this.m_beam != null)
            {
                if (timer > 0)
                {
                    timer -= BraveTime.DeltaTime;
                }
                if (timer <= 0)
                {
                    if (UnityEngine.Random.value <= this.chanceToFirePerTick) DoProjectileSpawn();
                    timer = tickCooldown;
                }
            }
        }
        private float timer;
    } //Makes the beam spawn projectiles at a position along it's length on a timer.

    //TECHNICALLY NOT NEW COMPONENTS, BUT NEW TYPES OF PROJECTILE
    public class SuperPierceProjectile : Projectile
    {
        public SuperPierceProjectile()
        {
            this.shouldRemoveCooldown = true;
        }
        public bool shouldRemoveCooldown;
        protected override void OnRigidbodyCollision(CollisionData rigidbodyCollision)
        {
            base.OnRigidbodyCollision(rigidbodyCollision);
            if (shouldRemoveCooldown)
            {
                //remove the cooldown
                base.specRigidbody.DeregisterTemporaryCollisionException(rigidbodyCollision.OtherRigidbody);
                rigidbodyCollision.OtherRigidbody.DeregisterTemporaryCollisionException(base.specRigidbody);
            }
        }
    } //A projectile with no cooldown on how many times it can hit an enemy per second.
}
