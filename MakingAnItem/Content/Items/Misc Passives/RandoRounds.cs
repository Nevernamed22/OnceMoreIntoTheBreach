using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;
using Alexandria.ItemAPI;
using SaveAPI;
using System.Security.Policy;
using System.Collections;

namespace NevernamedsItems
{
    public class RandoRounds : PassiveItem
    {
        public static void Init()
        {
            PickupObject item = ItemSetup.NewItem<RandoRounds>(
             "Rando Rounds",
             "Something Up",
             "Increases two random bullet related stats by 15%." + "\n\nThese shells were hand-crafted by Chancellot, the disgraced Ex-Knight of the Octagonal Table." + "\n\nOne of the order's most popular members, he was cast out after being caught in Princess Gunivere's chambers.",
             "rando6_icon");
            item.quality = PickupObject.ItemQuality.C;
            item.SetTag("bullet_modifier");
            item.SetupUnlockOnCustomFlag(CustomDungeonFlags.PURCHASED_RANDOROUNDS, true);
            item.AddItemToDougMetaShop(19);
            Doug.AddToLootPool(item.PickupObjectId);
        }
        List<int> chosenStats = null;
        public override void Pickup(PlayerController player)
        {
            if (!m_pickedUpThisRun)
            {
                int one = UnityEngine.Random.Range(0, 6);
                int two = one;
                while (two == one) { two = UnityEngine.Random.Range(0, 6); }
                chosenStats = new List<int>() { one, two };

                this.AddPassiveStatModifier(Stats[chosenStats[0]]);
                this.AddPassiveStatModifier(Stats[chosenStats[1]]);
            }
            player.StartCoroutine(ShowText(player));
            base.Pickup(player);
        }
        public IEnumerator ShowText(PlayerController player)
        {
            VFXToolbox.DoRisingStringFade(StatLabels[chosenStats[0]], player.sprite.WorldCenter + new Vector2(0f, 1f), Color.red);
            yield return new WaitForSeconds(1f);
            VFXToolbox.DoRisingStringFade(StatLabels[chosenStats[1]], player.sprite.WorldCenter + new Vector2(0f, 1f), Color.red);
            yield break;
        }
        public static readonly List<StatModifier> Stats = new List<StatModifier>()
        {
            new StatModifier()
            {
                 statToBoost = PlayerStats.StatType.Accuracy,
                 amount = 0.85f,
                 modifyType = StatModifier.ModifyMethod.MULTIPLICATIVE,
            },
            new StatModifier()
            {
                 statToBoost = PlayerStats.StatType.ReloadSpeed,
                 amount = 0.85f,
                 modifyType = StatModifier.ModifyMethod.MULTIPLICATIVE,
            },
            new StatModifier()
            {
                 statToBoost = PlayerStats.StatType.Damage,
                 amount = 1.15f,
                 modifyType = StatModifier.ModifyMethod.MULTIPLICATIVE,
            },
            new StatModifier()
            {
                 statToBoost = PlayerStats.StatType.PlayerBulletScale,
                 amount = 1.15f,
                 modifyType = StatModifier.ModifyMethod.MULTIPLICATIVE,
            },
            new StatModifier()
            {
                 statToBoost = PlayerStats.StatType.ProjectileSpeed,
                 amount = 1.15f,
                 modifyType = StatModifier.ModifyMethod.MULTIPLICATIVE,
            },
            new StatModifier()
            {
                 statToBoost = PlayerStats.StatType.RateOfFire,
                 amount = 1.15f,
                 modifyType = StatModifier.ModifyMethod.MULTIPLICATIVE,
            }
        };
        public static readonly List<string> StatLabels = new List<string>()
        {
            "ACCURACY UP",
            "RELOAD SPEED UP",
            "DAMAGE UP",
            "BULLET SIZE UP",
            "SHOT SPEED UP",
            "FIRERATE UP"
        };
    }
}
