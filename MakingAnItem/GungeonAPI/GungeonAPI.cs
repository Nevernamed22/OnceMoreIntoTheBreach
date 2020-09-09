using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GungeonAPI
{
    public static class GungeonAP
    {
        public static void Init()
        {
            Tools.Init();
            StaticReferences.Init();
            ShrineFakePrefabHooks.Init();
            ShrineFactory.Init();
            //DungeonHandler.Init();
        }
    }
}
