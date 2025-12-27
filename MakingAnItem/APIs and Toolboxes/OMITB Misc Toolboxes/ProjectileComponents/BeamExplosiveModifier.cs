using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace NevernamedsItems
{
    public class BeamExplosiveModifier : MonoBehaviour
    {
        public BeamExplosiveModifier()
        {
            canHarmOwner = false;
            explosionData = GameManager.Instance.Dungeon.sharedSettingsPrefab.DefaultSmallExplosionData;
            chancePerTick = 1;
            tickDelay = 0.1f;
            ignoreQueues = true;
        }
        private void Start()
        {
            timer = tickDelay;
            this.projectile = base.GetComponent<Projectile>();
            this.beamController = base.GetComponent<BeamController>();
            this.basicBeamController = base.GetComponent<BasicBeamController>();
            if (this.projectile.Owner is PlayerController) this.owner = this.projectile.Owner as PlayerController;
        }
        private void Update()
        {
            if (timer > 0)
            {
                timer -= BraveTime.DeltaTime;
            }
            if (timer <= 0)
            {
                DoTick();
                timer = tickDelay;
            }
        }
        private void DoTick()
        {
            //ETGModConsole.Log("Tick Triggered");
            if (UnityEngine.Random.value < chancePerTick)
            {
                LinkedList<BasicBeamController.BeamBone> bones;
                bones = basicBeamController.m_bones;
                LinkedListNode<BasicBeamController.BeamBone> linkedListNode = bones.Last;
                Vector2 bonePosition = basicBeamController.GetBonePosition(linkedListNode.Value);
                Explode(bonePosition);

            }
        }
        private void Explode(Vector2 pos)
        {
            if (!canHarmOwner && owner != null)
            {
                for (int i = 0; i < GameManager.Instance.AllPlayers.Length; i++)
                {
                    PlayerController playerController = GameManager.Instance.AllPlayers[i];
                    if (playerController && playerController.specRigidbody)
                    {
                        this.explosionData.ignoreList.Add(playerController.specRigidbody);
                    }
                }
            }
            Exploder.Explode(pos, this.explosionData, Vector2.zero, null, this.ignoreQueues);
        }
        public bool ignoreQueues;

        public float chancePerTick;
        public float tickDelay;

        public ExplosionData explosionData;
        public bool canHarmOwner;

        private float timer;
        private Projectile projectile;
        private BasicBeamController basicBeamController;
        private BeamController beamController;
        private PlayerController owner;
    }
}
