using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace NevernamedsItems
{
    public class DoNothingAttackBehaviour : AttackBehaviorBase
    {
        //This needs to solely exist to jank my way around some bullshit with sequential attack behaviour groups, and because BasicAttackBehaviour is abstract!
        public override bool IsReady() { return true; }
        public override float GetMinReadyRange() { return -1f; }
        public override float GetMaxRange() { return -1f; }
    }
}

