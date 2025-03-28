using Alexandria.ItemAPI;
using Alexandria.Misc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace NevernamedsItems
{
    public class BeamAttachVFXToEnd : MonoBehaviour
    {
        private Projectile projectile;
        private BeamController beamController;
        private BasicBeamController basicBeamController;

        public GameObject VFX;

        private GameObject instanceVFX;
        private bool dying;

        private void Start()
        {
            this.projectile = base.GetComponent<Projectile>();
            this.beamController = base.GetComponent<BeamController>();
            this.basicBeamController = base.GetComponent<BasicBeamController>();


        }
        private void Update()
        {
            if (!instanceVFX && !dying)
            {
                instanceVFX = SpawnManager.SpawnVFX(VFX, basicBeamController.GetBonePosition(basicBeamController.m_bones.Last.Value), Quaternion.identity, true);
            }
            if (instanceVFX)
            {
                instanceVFX.transform.position = basicBeamController.GetBonePosition(basicBeamController.m_bones.Last.Value);
            }
        }
        private void OnDestroy()
        {
            dying = true;
            if (instanceVFX)
            {
                UnityEngine.GameObject.Destroy(instanceVFX);
            }
        }
    }
}
