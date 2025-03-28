using Alexandria.ItemAPI;
using Alexandria.Misc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace NevernamedsItems
{
    public class BeamProjSpewModifier : MonoBehaviour
    {
        public BeamProjSpewModifier()
        {
            chancePerTick = 1;
            tickDelay = 0.01f;
            angleFromAim = 0;
            accuracyVariance = 7;
            positionToSpawn = SpawnPosition.BEAM_END;
            bulletToSpew = (PickupObjectDatabase.GetById(56) as Gun).DefaultModule.projectiles[0];
        }
        private void Start()
        {
            timer = tickDelay;
            this.projectile = base.GetComponent<Projectile>();
            this.beamController = base.GetComponent<BeamController>();
            this.basicBeamController = base.GetComponent<BasicBeamController>();
            if (this.projectile.Owner is PlayerController) this.owner = this.projectile.Owner as PlayerController;
            projectile.OnHitEnemy += OnHit;

        }
        private void OnHit(Projectile proj, SpeculativeRigidbody body, bool fatal)
        {
            if (tickOnHit && hitTickCool <= 0)
            {
                hitTickCool = onHitTickcooldown;
                if (body) DoTick(body.UnitCenter, body);
            }
        }
        private void Update()
        {
            if (hitTickCool > 0) { hitTickCool -= BraveTime.DeltaTime; }
            if (tickOnTimer)
            {
                if (timer > 0)
                {
                    timer -= BraveTime.DeltaTime;
                }
                if (timer <= 0)
                {
                    DoTick(Vector2.zero, null);
                    timer = tickDelay;
                }
            }
        }
        private void DoTick(Vector2 enemyContact, SpeculativeRigidbody aiactor)
        {
            if (UnityEngine.Random.value < chancePerTick)
            {
                LinkedList<BasicBeamController.BeamBone> bones = basicBeamController.m_bones;

                Vector2 Position = Vector2.zero;
                float spawnAngle = 0;

                switch (positionToSpawn)
                {
                    case SpawnPosition.BEAM_END:
                        Position = basicBeamController.GetBonePosition(bones.Last.Value);
                        spawnAngle = basicBeamController.GetFinalBoneDirection();
                        break;
                    case SpawnPosition.BEAM_START:
                        Position = basicBeamController.GetBonePosition(bones.First.Value);
                        spawnAngle = basicBeamController.Direction.ToAngle();
                        break;
                    case SpawnPosition.ENEMY_IMPACT:
                        Position = enemyContact;
                        spawnAngle = basicBeamController.Direction.ToAngle();
                        break;
                }

                FireProjectile(Position, spawnAngle, (positionToSpawn == SpawnPosition.ENEMY_IMPACT && aiactor != null) ? aiactor : null);
            }
        }
        private void FireProjectile(Vector2 pos, float angle, SpeculativeRigidbody ToIgnore)
        {
            float variance = UnityEngine.Random.Range(0, accuracyVariance);
            if (UnityEngine.Random.value <= 0.5) variance *= -1;
            float angleVaried = (angle + angleFromAim) + variance;
            GameObject spawnedBulletOBJ = SpawnManager.SpawnProjectile(bulletToSpew.gameObject, pos, Quaternion.Euler(0f, 0f, angleVaried), true);
            spawnedBulletOBJ.AddComponent<BulletIsFromBeam>();
            Projectile component = spawnedBulletOBJ.GetComponent<Projectile>();
            if (component != null)
            {
                component.Owner = projectile.ProjectilePlayerOwner();
                component.Shooter = projectile.ProjectilePlayerOwner().specRigidbody;
                owner.DoPostProcessProjectile(component);
            }
            SpeculativeRigidbody body = spawnedBulletOBJ.GetComponent<SpeculativeRigidbody>();
            if (body != null && ToIgnore != null)
            {
                body.RegisterGhostCollisionException(ToIgnore);
            }
        }
        public float chancePerTick;

        public bool tickOnHit = false;
        public float onHitTickcooldown = 0.05f;

        public bool tickOnTimer = false;
        public float tickDelay;

        public Projectile bulletToSpew;
        public float accuracyVariance;
        public float angleFromAim;

        private float timer;
        private float hitTickCool;
        private Projectile projectile;
        private BasicBeamController basicBeamController;
        private BeamController beamController;
        private PlayerController owner;

        public SpawnPosition positionToSpawn;
        public enum SpawnPosition
        {
            BEAM_END,
            BEAM_START,
            ENEMY_IMPACT
        }
    }
}
