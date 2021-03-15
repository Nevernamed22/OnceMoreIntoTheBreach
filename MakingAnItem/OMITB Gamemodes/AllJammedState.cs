using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Gungeon;
using System.Reflection;
using Brave.BulletScript;
using System.Collections;
using MonoMod.RuntimeDetour;

namespace NevernamedsItems
{
    public class AllJammedState
    {
        public static bool allJammedActive;
        public static void Init()
        {
            allJammedActive = false;
            ETGMod.AIActor.OnPreStart += makeJammed;
        }
        private static void makeJammed(AIActor enemy)
        {
            if (allJammedActive)
            {
                enemy.BecomeBlackPhantom();
            }
        }
        Hook sreaperStartHook = new Hook(
  typeof(SuperReaperController).GetMethod("Start", BindingFlags.NonPublic | BindingFlags.Instance),
  typeof(AllJammedState).GetMethod("SreaperAwakeHook")
);

        public static void SreaperAwakeHook(Action<SuperReaperController> orig, SuperReaperController self)
        {
            orig(self);
            if (allJammedActive)
            {
                self.BecomeJammedLord();
            }
        }
    }
    static class JamTheLord
    {
        public static void BecomeJammedLord(this SuperReaperController controller)
        {
            if (controller.GetComponent<JammedLordController>() != null)
            {
                return;
            }
            controller.gameObject.AddComponent<JammedLordController>().Initialize(controller);
        }

        public static void UnbecomeJammedLord(this SuperReaperController controller)
        {
            if (controller.GetComponent<JammedLordController>() != null)
            {
                controller.GetComponent<JammedLordController>().Uninitialize();
            }
        }
    }
    public class JammedLordController : MonoBehaviour
    {
        public void Initialize(SuperReaperController controller)
        {
            this.m_controller = controller;
            this.m_cachedAttack = controller.BulletScript.scriptTypeName;
            tk2dBaseSprite tk2dBaseSprite = base.GetComponent<tk2dBaseSprite>();
            tk2dBaseSprite.usesOverrideMaterial = true;
            Material material = tk2dBaseSprite.renderer.material;
            material.shader = ShaderCache.Acquire("Brave/LitCutoutUberPhantom");
            material.SetFloat("_PhantomGradientScale", 0.75f);
            material.SetFloat("_PhantomContrastPower", 1.3f);
            controller.BulletScript.scriptTypeName = typeof(JammedCircleBurst12).AssemblyQualifiedName;
            controller.MinSpeed *= 1.5f;
            controller.MaxSpeed *= 1.5f;
            SetParticlesPerSecond(GetParticlesPerSecond() * 2);
        }

        public void Uninitialize()
        {
            this.m_controller.BulletScript.scriptTypeName = this.m_cachedAttack;
            tk2dBaseSprite tk2dBaseSprite = base.GetComponent<tk2dBaseSprite>();
            tk2dBaseSprite.usesOverrideMaterial = false;
            this.m_controller.MinSpeed /= 1.5f;
            this.m_controller.MaxSpeed /= 1.5f;
            SetParticlesPerSecond(GetParticlesPerSecond() / 2);
            Destroy(this);
        }

        private int GetParticlesPerSecond()
        {
            return (int)info.GetValue(this.m_controller);
        }

        private void SetParticlesPerSecond(int value)
        {
            info.SetValue(this.m_controller, value);
        }

        private string m_cachedAttack;
        private SuperReaperController m_controller;
        private static FieldInfo info = typeof(SuperReaperController).GetField("c_particlesPerSecond", BindingFlags.NonPublic | BindingFlags.Instance);
    }

    public class JammedCircleBurst12 : Script
    {
        protected override IEnumerator Top()
        {
            float num = base.RandomAngle();
            float num2 = 30f;
            for (int i = 0; i < 12; i++)
            {
                base.Fire(new Direction(num + (float)i * num2, DirectionType.Absolute, -1f), new Speed(9f, SpeedType.Absolute), new Bullet(null, false, false, true));
            }
            return null;
        }
    }
}
