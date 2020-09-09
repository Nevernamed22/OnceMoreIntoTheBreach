using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace NevernamedsItems
{
    public class EasyStatusEffectAccess
    {
        public static GameActorHealthEffect irradiatedLeadEffect = Gungeon.Game.Items["irradiated_lead"].GetComponent<BulletStatusEffectItem>().HealthModifierEffect;
        public static GameActorFreezeEffect freezeModifierEffect = PickupObjectDatabase.GetById(278).GetComponent<BulletStatusEffectItem>().FreezeModifierEffect;
    }
    public class ExtendedColours
    {
        public static Color freezeBlue = EasyStatusEffectAccess.freezeModifierEffect.TintColor;
        public static Color poisonGreen = EasyStatusEffectAccess.irradiatedLeadEffect.TintColor;
        public static Color pink = new Color(242f / 255f, 116f / 255f, 225f / 255f);
        public static Color lime = new Color(111f / 255f, 252f / 255f, 3f / 255f);
        public static Color brown = new Color(122f / 255f, 71f / 255f, 16f / 255f);
        public static Color orange = new Color(240f / 255f, 160f / 255f, 22f / 255f);
        public static Color purple = new Color(171f / 255f, 22f / 255f, 240f / 255f);
        public static Color skyblue = new Color(130f / 255f, 230f / 255f, 2255f / 255f);
        public static Color honeyYellow = new Color(255f / 255f, 180f / 255f, 18f / 255f);
    }
}
