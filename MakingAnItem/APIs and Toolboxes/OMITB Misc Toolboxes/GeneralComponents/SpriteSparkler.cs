using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace NevernamedsItems
{
    public class SpriteSparkler : BraveBehaviour
    {
        //Public Variables
        private float particleCounter = 0;
        public bool doParticles = false;
        public bool doVFX = false;
        public float particlesPerSecond = 2;
        public GlobalSparksDoer.SparksType particleType;
        public string childTarget = null;
        public GameObject VFX;
        public float lifetime = -1;
        public bool randomise = false;

        //Private Variables
        private float lifeTimer = -1f;
        private bool isTiming = false;

        private void Start()
        {
            if (lifetime > 0)
            {
                lifeTimer = lifetime;
                isTiming = true;
            }
        }
        private void Update()
        {
            if (!isTiming || lifeTimer > 0f)
            {
                if (lifeTimer > 0f) { lifeTimer -= BraveTime.DeltaTime; }
                if (base.sprite && GameManager.Options.ShaderQuality != GameOptions.GenericHighMedLowOption.LOW && GameManager.Options.ShaderQuality != GameOptions.GenericHighMedLowOption.VERY_LOW)
                {
                    float midParticles = particlesPerSecond;
                    if (randomise)
                    {
                        midParticles += UnityEngine.Random.Range(-particlesPerSecond, particlesPerSecond);
                    }
                    particleCounter += BraveTime.DeltaTime * midParticles;
                    if (particleCounter > 1f)
                    {
                        int num = Mathf.FloorToInt(particleCounter);
                        particleCounter %= 1f;


                        Vector2 minpos = base.sprite.WorldBottomLeft.ToVector3ZisY(0f);
                        Vector2 maxpos = base.sprite.WorldTopRight.ToVector3ZisY(0f);
                        if (!string.IsNullOrEmpty(childTarget) && base.transform.Find(childTarget))
                        {
                            minpos = base.transform.Find(childTarget).position;
                            maxpos = base.transform.Find(childTarget).position;
                        }
                        if (doVFX)
                        {
                            for (int i = 0; i < num; i++)
                            {
                                GameObject vfx = UnityEngine.Object.Instantiate(VFX, new Vector2(UnityEngine.Random.Range(minpos.x, maxpos.x), UnityEngine.Random.Range(minpos.y, maxpos.y)), Quaternion.identity);
                                vfx.GetComponent<tk2dBaseSprite>().HeightOffGround = 0.2f;
                            }
                        }
                        if (doParticles)
                        {
                            GlobalSparksDoer.DoRandomParticleBurst(num, minpos, maxpos, Vector3.up, 90f, 0.5f, null, null, null, particleType);
                        }
                    }
                }
            }
        }
    }
}
