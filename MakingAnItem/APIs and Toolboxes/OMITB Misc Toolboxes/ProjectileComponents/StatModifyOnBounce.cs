using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace NevernamedsItems
{
    public class StatModifyOnBounce : MonoBehaviour
    {
        public void Start()
        {
            self = base.GetComponent<Projectile>();
            bouncer = base.GetComponent<BounceProjModifier>();

            if (bouncer) { bouncer.OnBounceContext += OnBounceContext; }
        }

        public void OnBounceContext(BounceProjModifier modifier, SpeculativeRigidbody body)
        {
            foreach(Modifiers mod in mods)
            {
                switch (mod.stattype)
                {
                    case ProjectileStatType.DAMAGE:
                        if (mod.additive) { self.baseData.damage += mod.amount; }
                        else { self.baseData.damage *= mod.amount; }
                        break;
                    case ProjectileStatType.SPEED:
                        if (mod.additive) { self.baseData.speed += mod.amount; }
                        else { self.baseData.speed *= mod.amount; }
                        self.UpdateSpeed();
                        break;
                    case ProjectileStatType.RANGE:
                        if (mod.additive) { self.baseData.range += mod.amount; }
                        else { self.baseData.range *= mod.amount; }
                        break;
                    case ProjectileStatType.SCALE:
                        self.RuntimeUpdateScale(mod.amount);
                        break;
                    case ProjectileStatType.KNOCKBACK:
                        if (mod.additive) { self.baseData.force += mod.amount; }
                        else { self.baseData.force *= mod.amount; }
                        break;
                    case ProjectileStatType.BOSSDAMAGE:
                        if (mod.additive) { self.BossDamageMultiplier += mod.amount; }
                        else { self.BossDamageMultiplier *= mod.amount; }
                        break;
                    case ProjectileStatType.JAMMEDDAMAGE:
                        if (mod.additive) { self.BlackPhantomDamageMultiplier += mod.amount; }
                        else { self.BlackPhantomDamageMultiplier *= mod.amount; }
                        break;
                };
            }
        }
        public enum ProjectileStatType
        {
            DAMAGE,
            SPEED,
            RANGE,
            SCALE,
            KNOCKBACK,
            BOSSDAMAGE,
            JAMMEDDAMAGE,
        }
        public List<Modifiers> mods = new List<Modifiers>();
        public class Modifiers { public ProjectileStatType stattype; public float amount; public bool additive;  }
        public Projectile self;
        public BounceProjModifier bouncer;
    }
}
