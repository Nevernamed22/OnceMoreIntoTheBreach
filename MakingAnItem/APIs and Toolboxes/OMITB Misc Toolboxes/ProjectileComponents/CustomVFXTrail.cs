using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace NevernamedsItems
{
    public class CustomVFXTrail : MonoBehaviour
    {
        public CustomVFXTrail()
        {
            anchor = Anchor.Center;
            timeBetweenSpawns = 0.1f;
            fixedHeightOffGround = -1f;
            inheritVelocity = false;
            heightOffset = 0;
            parentSelf = false;
            inheritRotation = false;
        }
        private void Start()
        {
            self = base.GetComponent<Projectile>();
            timer = timeBetweenSpawns;
            if (self && fixedHeightOffGround == -1f)
            {
                if (Projectile.CurrentProjectileDepth != 0.8f)
                {
                    fixedHeightOffGround = Projectile.CurrentProjectileDepth;
                }
                else { fixedHeightOffGround = self.sprite ? self.sprite.HeightOffGround : 0f; }
            }
            fixedHeightOffGround += heightOffset;

        }
        public VFXPool VFX;
        public float timeBetweenSpawns;
        private float timer;
        public float fixedHeightOffGround;
        public float heightOffset;
        public bool inheritVelocity;
        public bool inheritRotation;
        public bool parentSelf;
        public Anchor anchor;
        private Projectile self;
        public enum Anchor
        {
            ChildTransform,
            Center
        }
        private void Update()
        {
            if (self)
            {
                if (timer > 0)
                {
                    timer -= BraveTime.DeltaTime;
                }
                else
                {
                    if (VFX != null)
                    {
                        VFX.SpawnAtPosition(
                            GetAnchoredPosition(),
                            inheritRotation ? self.Direction.ToAngle() : 0f,
                            parentSelf ? self.transform : null,
                            new Vector2?(Vector2.zero),
                            inheritVelocity ? new Vector2?(self.specRigidbody.Velocity) : new Vector2?(Vector2.zero),
                            fixedHeightOffGround,
                            false, null, null, false);
                    }
                    timer = timeBetweenSpawns;
                }
            }
        }
        private Vector3 GetAnchoredPosition()
        {
            Vector2 found = self.LastPosition;
            switch (anchor)
            {
                case Anchor.Center:
                    if (self.specRigidbody != null) { found = self.specRigidbody.UnitCenter; }
                    break;
                case Anchor.ChildTransform:
                    if (self.transform.Find("CustomVFXSpawnpoint") != null) { found = self.transform.Find("CustomVFXSpawnpoint").position; } 
                    break;
            }
            return found;
        }
    }
}
