using Alexandria.Misc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace NevernamedsItems
{
    public class BeamBlankModifier : MonoBehaviour
    {
        public BeamBlankModifier()
        {
            blankType = EasyBlankType.MINI;
            chancePerTick = 0.25f;
            tickDelay = 1f;
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
            float chance = chancePerTick;
            if (overrideBossroomChancePerTick != -1f && owner && owner.CurrentRoom.area.PrototypeRoomCategory == PrototypeDungeonRoom.RoomCategory.BOSS) { chance = overrideBossroomChancePerTick; }
            if (UnityEngine.Random.value < chance)
            {
                LinkedList<BasicBeamController.BeamBone> bones;
                bones = basicBeamController.m_bones;
                LinkedListNode<BasicBeamController.BeamBone> linkedListNode = bones.Last;
                Vector2 bonePosition = basicBeamController.GetBonePosition(linkedListNode.Value);
                Blank(bonePosition);

            }
        }
        private void Blank(Vector2 pos)
        {
            GameObject silencerVFX = (GameObject)ResourceCache.Acquire("Global VFX/BlankVFX_Ghost");
            AkSoundEngine.PostEvent("Play_OBJ_silenceblank_small_01", base.gameObject);
            GameObject gameObject = new GameObject("silencer");
            SilencerInstance silencerInstance = gameObject.AddComponent<SilencerInstance>();
            float additionalTimeAtMaxRadius = 0.25f;
            silencerInstance.TriggerSilencer(pos, 20f, 5, silencerVFX, 0f, 3f, 3f, 3f, 30f, 3f, additionalTimeAtMaxRadius, owner, true, false);
        }
        public float chancePerTick;
        public float tickDelay;
        EasyBlankType blankType;
        public float overrideBossroomChancePerTick = -1f;

        private float timer;
        private Projectile projectile;
        private BasicBeamController basicBeamController;
        private BeamController beamController;
        private PlayerController owner;
    }
}
