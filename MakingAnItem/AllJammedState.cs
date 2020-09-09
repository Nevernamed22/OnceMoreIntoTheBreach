using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Gungeon;

namespace NevernamedsItems
{
    public class AllJammedState : ETGModule
    {
        public override void Exit()
        {
        }
        public override void Start()
        {
        }
        public static bool allJammedActive;
        public override void Init()
        {
            allJammedActive = false;
            ETGMod.AIActor.OnPreStart += makeJammed;
        }
        private void makeJammed(AIActor enemy)
        {
            if (allJammedActive)
            {
                enemy.BecomeBlackPhantom();
            }
        }
    }
}
