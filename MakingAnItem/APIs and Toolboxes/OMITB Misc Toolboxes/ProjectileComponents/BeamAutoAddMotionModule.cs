using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NevernamedsItems
{
    class BeamAutoAddMotionModule : BraveBehaviour
    {
        private void Start()
        {
            if (base.projectile && base.GetComponent<BasicBeamController>() != null)
            {
                BasicBeamController basicBeam = base.GetComponent<BasicBeamController>();
                if (Orbit)
                {
                    basicBeam.PenetratesCover = true;
                    basicBeam.penetration += 10;
                    OrbitProjectileMotionModule orbitProjectileMotionModule = new OrbitProjectileMotionModule();
                    orbitProjectileMotionModule.BeamOrbitRadius = beamOrbitRadius;
                    orbitProjectileMotionModule.RegisterAsBeam(basicBeam);

                    if (Helix)
                    {
                        orbitProjectileMotionModule.StackHelix = true;
                        orbitProjectileMotionModule.ForceInvert = invertHelix;
                    }

                    base.projectile.OverrideMotionModule = orbitProjectileMotionModule;
                }
                else if (Helix)
                {
                    HelixProjectileMotionModule helixProjectileMotionModule = new HelixProjectileMotionModule();
                    helixProjectileMotionModule.ForceInvert = invertHelix;
                    base.projectile.OverrideMotionModule = helixProjectileMotionModule;
                }
                basicBeam.ProjectileAndBeamMotionModule = (base.projectile.OverrideMotionModule as ProjectileAndBeamMotionModule);
            }
        }
        public bool Helix;
        public bool invertHelix;
        public bool Orbit;
        public float beamOrbitRadius = 5f;
    }
}
