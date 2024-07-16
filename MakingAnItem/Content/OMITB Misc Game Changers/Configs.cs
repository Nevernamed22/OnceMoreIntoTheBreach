using Gunfiguration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace NevernamedsItems
{
    public static class Gunfigs
    {
        internal static Gunfig _Gunfig = null;

        //Options
        internal const string DISABLE_GUILLOTINE = "Portcullis Trap Replacements";

        internal static void Init()
        {
            _Gunfig = Gunfig.Get(modName: "Once More Into The Breach".WithColor(Color.white));

            //Toggles
            _Gunfig.AddToggle(key: DISABLE_GUILLOTINE, updateType: Gunfig.Update.Immediate);
        }
    }
}