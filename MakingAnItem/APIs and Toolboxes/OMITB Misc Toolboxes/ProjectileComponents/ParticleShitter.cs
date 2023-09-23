using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Alexandria.Misc;
using Alexandria.ItemAPI;

namespace NevernamedsItems
{
    public class ParticleShitter : MonoBehaviour
    {
        private void Start()
        {
            m_projectile = base.GetComponent<Projectile>();
        }
        private void Update()
        {
            particleCounter += BraveTime.DeltaTime * particlesPerSecond;
            if (particleCounter > 1f)
            {
                int num = Mathf.FloorToInt(particleCounter);
                particleCounter %= 1f;

                if (m_projectile.GetComponent<BeamController>())
                {
                    BasicBeamController beam = m_projectile.GetComponent<BasicBeamController>();
                    Vector2 position = beam.GetIndexedBonePosition(UnityEngine.Random.Range( 0, beam.GetBoneCount()));

                    for (int i = 0; i < num; i++)
                    {
                        Vector3 direction2 = Quaternion.Euler(0f, 0f, UnityEngine.Random.Range(-90, 90)) * (Vector3.up.normalized * UnityEngine.Random.Range(Vector3.up.magnitude - 0.5f, Vector3.up.magnitude + 0.5f));
                        GlobalSparksDoer.DoSingleParticle(position, direction2, null, null, null, particleType);
                    }
                }
                else
                {
                    GlobalSparksDoer.DoRandomParticleBurst(num, m_projectile.sprite.WorldBottomLeft.ToVector3ZisY(0f), m_projectile.sprite.WorldTopRight.ToVector3ZisY(0f), Vector3.up, 90f, 0.5f, null, null, null, particleType);
                }
            }
        }
        private float particleCounter = 0;
        private Projectile m_projectile;
        public GlobalSparksDoer.SparksType particleType;
        public float particlesPerSecond = 40f;
    }
}
