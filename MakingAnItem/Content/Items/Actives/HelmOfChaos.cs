using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Alexandria.ItemAPI;
using Dungeonator;
using Alexandria.ChestAPI;
using SaveAPI;

namespace NevernamedsItems
{
    class HelmOfChaos : PlayerItem
    {
        public static void Init()
        {
            PlayerItem item = ItemSetup.NewItem<HelmOfChaos>(
            "Helm Of Chaos",
            "Embraced",
            "Don the helm to embrace chaos!"+ "\n\nThis helmet was created spontaneously out of loose particles colliding in the void of space via a process known as 'Boltzmann Forging'.",
            "helmofchaos_icon") as PlayerItem;


            activatedSpriteID = Initialisation.itemCollection.GetSpriteIdByName("helmofchaos_activated");
            deactivatedSpriteID = Initialisation.itemCollection.GetSpriteIdByName("helmofchaos_icon");

            item.AddPassiveStatModifier( PlayerStats.StatType.Curse, 2.5f, StatModifier.ModifyMethod.ADDITIVE);
            ItemBuilder.SetCooldownType(item, ItemBuilder.CooldownType.None, 0);
            item.quality = ItemQuality.S;
            item.SetupUnlockOnCustomFlag(CustomDungeonFlags.BEATEN_HELL_BOSS_TURBO_MODE, true);

            PossibleProj.AddRange(new List<Projectile>()
            {
                (PickupObjectDatabase.GetById(86) as Gun).DefaultModule.projectiles[0],
                (PickupObjectDatabase.GetById(56) as Gun).DefaultModule.projectiles[0],
                (PickupObjectDatabase.GetById(197) as Gun).DefaultModule.projectiles[0],
                (PickupObjectDatabase.GetById(89) as Gun).DefaultModule.projectiles[0],
                (PickupObjectDatabase.GetById(24) as Gun).DefaultModule.projectiles[0],
                (PickupObjectDatabase.GetById(18) as Gun).DefaultModule.projectiles[0],
                (PickupObjectDatabase.GetById(417) as Gun).DefaultModule.projectiles[0],
                (PickupObjectDatabase.GetById(626) as Gun).DefaultModule.projectiles[0],
                (PickupObjectDatabase.GetById(503) as Gun).DefaultModule.projectiles[0],
                (PickupObjectDatabase.GetById(512) as Gun).DefaultModule.projectiles[0],
                (PickupObjectDatabase.GetById(30) as Gun).DefaultModule.projectiles[0],
                (PickupObjectDatabase.GetById(38) as Gun).DefaultModule.projectiles[0],
                (PickupObjectDatabase.GetById(50) as Gun).DefaultModule.projectiles[0],
                (PickupObjectDatabase.GetById(223) as Gun).DefaultModule.projectiles[0],
                (PickupObjectDatabase.GetById(47) as Gun).DefaultModule.projectiles[0],
                (PickupObjectDatabase.GetById(97) as Gun).DefaultModule.projectiles[0],
                (PickupObjectDatabase.GetById(275) as Gun).DefaultModule.projectiles[0],
                (PickupObjectDatabase.GetById(550) as Gun).DefaultModule.projectiles[0],
                (PickupObjectDatabase.GetById(51) as Gun).DefaultModule.projectiles[0],
                (PickupObjectDatabase.GetById(9) as Gun).DefaultModule.projectiles[0],
                (PickupObjectDatabase.GetById(602) as Gun).DefaultModule.projectiles[0],
                (PickupObjectDatabase.GetById(599) as Gun).DefaultModule.projectiles[0],
                (PickupObjectDatabase.GetById(175) as Gun).DefaultModule.projectiles[0],
                (PickupObjectDatabase.GetById(55) as Gun).DefaultModule.projectiles[0],
                (PickupObjectDatabase.GetById(58) as Gun).DefaultModule.projectiles[0],
                (PickupObjectDatabase.GetById(406) as Gun).Volley.projectiles[1].projectiles[0],
                (PickupObjectDatabase.GetById(143) as Gun).Volley.projectiles[4].projectiles[0],
                (PickupObjectDatabase.GetById(379) as Gun).Volley.projectiles[0].projectiles[0],
                (PickupObjectDatabase.GetById(379) as Gun).Volley.projectiles[2].projectiles[0],
                (PickupObjectDatabase.GetById(379) as Gun).Volley.projectiles[3].projectiles[0],
                (PickupObjectDatabase.GetById(379) as Gun).Volley.projectiles[4].projectiles[0],
                (PickupObjectDatabase.GetById(347) as Gun).DefaultModule.projectiles[0],
                (PickupObjectDatabase.GetById(123) as Gun).DefaultModule.projectiles[0],
                (PickupObjectDatabase.GetById(404) as Gun).DefaultModule.projectiles[0],
                (PickupObjectDatabase.GetById(329) as Gun).DefaultModule.chargeProjectiles[0].Projectile,
                (PickupObjectDatabase.GetById(8) as Gun).DefaultModule.chargeProjectiles[0].Projectile,
                (PickupObjectDatabase.GetById(200) as Gun).DefaultModule.chargeProjectiles[0].Projectile,
                (PickupObjectDatabase.GetById(52) as Gun).DefaultModule.chargeProjectiles[0].Projectile,
                (PickupObjectDatabase.GetById(542) as Gun).DefaultModule.projectiles[0],
                (PickupObjectDatabase.GetById(95) as Gun).DefaultModule.projectiles[0],
                (PickupObjectDatabase.GetById(726) as Gun).DefaultModule.projectiles[0],
                (PickupObjectDatabase.GetById(598) as Gun).DefaultModule.projectiles[0],
                (PickupObjectDatabase.GetById(519) as Gun).DefaultModule.projectiles[0],
                (PickupObjectDatabase.GetById(6) as Gun).DefaultModule.projectiles[0],
                (PickupObjectDatabase.GetById(562) as Gun).DefaultModule.projectiles[0],
                (PickupObjectDatabase.GetById(29) as Gun).DefaultModule.projectiles[0],
                (PickupObjectDatabase.GetById(17) as Gun).DefaultModule.projectiles[0],
                (PickupObjectDatabase.GetById(545) as Gun).DefaultModule.projectiles[0],
                (PickupObjectDatabase.GetById(207) as Gun).DefaultModule.projectiles[0],
                (PickupObjectDatabase.GetById(146) as Gun).DefaultModule.projectiles[0],
                (PickupObjectDatabase.GetById(670) as Gun).DefaultModule.projectiles[0],
                (PickupObjectDatabase.GetById(383) as Gun).DefaultModule.projectiles[0],
                (PickupObjectDatabase.GetById(128) as Gun).DefaultModule.projectiles[0],
                (PickupObjectDatabase.GetById(142) as Gun).DefaultModule.projectiles[0],
                (PickupObjectDatabase.GetById(609) as Gun).DefaultModule.projectiles[0],
                (PickupObjectDatabase.GetById(394) as Gun).DefaultModule.projectiles[0],
                (PickupObjectDatabase.GetById(334) as Gun).DefaultModule.projectiles[0],
                (PickupObjectDatabase.GetById(91) as Gun).DefaultModule.projectiles[0],
                (PickupObjectDatabase.GetById(91) as Gun).DefaultModule.finalProjectile,
                (PickupObjectDatabase.GetById(577) as Gun).DefaultModule.projectiles[0],
                (PickupObjectDatabase.GetById(229) as Gun).DefaultModule.projectiles[0],
                (PickupObjectDatabase.GetById(724) as Gun).DefaultModule.projectiles[0],
                (PickupObjectDatabase.GetById(3) as Gun).DefaultModule.projectiles[0],
                (PickupObjectDatabase.GetById(59) as Gun).DefaultModule.projectiles[0],
                (PickupObjectDatabase.GetById(54) as Gun).DefaultModule.projectiles[0],
                (PickupObjectDatabase.GetById(178) as Gun).DefaultModule.projectiles[0],
                (PickupObjectDatabase.GetById(13) as Gun).DefaultModule.projectiles[0],
                (PickupObjectDatabase.GetById(16) as Gun).DefaultModule.projectiles[0],
                (PickupObjectDatabase.GetById(384) as Gun).DefaultModule.projectiles[0],
                (PickupObjectDatabase.GetById(145) as Gun).DefaultModule.projectiles[0],
                (PickupObjectDatabase.GetById(0) as Gun).DefaultModule.projectiles[0],
                (PickupObjectDatabase.GetById(596) as Gun).DefaultModule.projectiles[0],
                (PickupObjectDatabase.GetById(596) as Gun).DefaultModule.finalProjectile,
                (PickupObjectDatabase.GetById(199) as Gun).DefaultModule.projectiles[0],
                (PickupObjectDatabase.GetById(7) as Gun).DefaultModule.projectiles[0],
                (PickupObjectDatabase.GetById(28) as Gun).DefaultModule.projectiles[0],
                (PickupObjectDatabase.GetById(26) as Gun).DefaultModule.projectiles[0],
                (PickupObjectDatabase.GetById(27) as Gun).DefaultModule.projectiles[0],
                (PickupObjectDatabase.GetById(477) as Gun).DefaultModule.projectiles[0],
                (PickupObjectDatabase.GetById(152) as Gun).DefaultModule.projectiles[0],
                (PickupObjectDatabase.GetById(33) as Gun).DefaultModule.projectiles[0],
                (PickupObjectDatabase.GetById(90) as Gun).DefaultModule.projectiles[0],
                (PickupObjectDatabase.GetById(149) as Gun).DefaultModule.projectiles[0],
                (PickupObjectDatabase.GetById(506) as Gun).DefaultModule.projectiles[0],
                (PickupObjectDatabase.GetById(377) as Gun).DefaultModule.projectiles[0],
                (PickupObjectDatabase.GetById(444) as Gun).DefaultModule.projectiles[0],
                (PickupObjectDatabase.GetById(14) as Gun).DefaultModule.projectiles[0],
                (PickupObjectDatabase.GetById(335) as Gun).DefaultModule.projectiles[0],
                (PickupObjectDatabase.GetById(150) as Gun).DefaultModule.projectiles[0],
                (PickupObjectDatabase.GetById(176) as Gun).DefaultModule.projectiles[0],
                (PickupObjectDatabase.GetById(476) as Gun).DefaultModule.projectiles[0],
                (PickupObjectDatabase.GetById(476) as Gun).DefaultModule.projectiles[1],
            });

            PowerfulProj.AddRange(new List<Projectile>()
            {
                (PickupObjectDatabase.GetById(53) as Gun).DefaultModule.projectiles[0],
                (PickupObjectDatabase.GetById(706) as Gun).DefaultModule.projectiles[0],
                (PickupObjectDatabase.GetById(601) as Gun).DefaultModule.projectiles[0],
                (PickupObjectDatabase.GetById(357) as Gun).DefaultModule.projectiles[0],
                (PickupObjectDatabase.GetById(362) as Gun).DefaultModule.projectiles[0],
                (PickupObjectDatabase.GetById(479) as Gun).DefaultModule.projectiles[0],
                (PickupObjectDatabase.GetById(28) as Gun).DefaultModule.finalProjectile,
                (PickupObjectDatabase.GetById(27) as Gun).DefaultModule.finalProjectile,
                (PickupObjectDatabase.GetById(445) as Gun).DefaultModule.projectiles[0],
                (PickupObjectDatabase.GetById(154) as Gun).DefaultModule.projectiles[0],
                (PickupObjectDatabase.GetById(130) as Gun).DefaultModule.projectiles[0],
                (PickupObjectDatabase.GetById(292) as Gun).DefaultModule.projectiles[0],
                (PickupObjectDatabase.GetById(90) as Gun).DefaultModule.finalProjectile,
                (PickupObjectDatabase.GetById(475) as Gun).DefaultModule.projectiles[0],
                (PickupObjectDatabase.GetById(39) as Gun).DefaultModule.projectiles[0],
                (PickupObjectDatabase.GetById(81) as Gun).DefaultModule.projectiles[0],
                (PickupObjectDatabase.GetById(129) as Gun).DefaultModule.projectiles[0],
                (PickupObjectDatabase.GetById(180) as Gun).DefaultModule.projectiles[0],
                (PickupObjectDatabase.GetById(198) as Gun).DefaultModule.projectiles[0],

            });
        }

        public static List<Projectile> PossibleProj = new List<Projectile>();
        public static List<Projectile> PowerfulProj = new List<Projectile>();


        private static int activatedSpriteID;
        private static int deactivatedSpriteID;

        private bool isActivated = false;

        public override void OnPreDrop(PlayerController user)
        {
            if (isActivated) { Deactivate(user); }
        }
        public override void OnDestroy()
        {
            if (LastOwner && isActivated) { Deactivate(LastOwner); }
        }
        public override void DoEffect(PlayerController user)
        {
            if (isActivated)
            {
                Deactivate(user);
            }
            else
            {
                Activate(user);
            }
        }
        public void Activate(PlayerController pl)
        {
            isActivated = true;
            if (pl) { pl.OnPreFireProjectileModifier += PreFireProj; }
            base.sprite.SetSprite(activatedSpriteID);
        }
        public void Deactivate(PlayerController pl)
        {
            isActivated = false;
            if (pl) { pl.OnPreFireProjectileModifier -= PreFireProj; }
            base.sprite.SetSprite(deactivatedSpriteID);

        }
        public Projectile PreFireProj(Gun gu, Projectile proj)
        {
            if (LastOwner && !gu.InfiniteAmmo)
            {
                int chosen = 0;
                int bounds = PossibleProj.Count -1 ;
                if (LastOwner.PlayerHasActiveSynergy("Liber Null")) { bounds += PowerfulProj.Count -1; }
                chosen = UnityEngine.Random.Range(0, bounds + 1);
                if (chosen > PossibleProj.Count - 1)
                {
                    return PowerfulProj[chosen - PossibleProj.Count - 1];
                }
                else
                {
                    return PossibleProj[chosen];
                }
            }
            return proj;
        }

        public override bool CanBeUsed(PlayerController user)
        {
            return base.CanBeUsed(user);
        }
    }
}