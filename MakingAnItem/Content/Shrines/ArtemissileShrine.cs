
using GungeonAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Gungeon;
using Alexandria.ItemAPI;
using Dungeonator;
using System.Reflection;
using MonoMod.RuntimeDetour;
using Alexandria.Misc;

namespace NevernamedsItems
{
    public class ArtemissileShrine : GenericShrine
    {
        public static GameObject Setup(GameObject pedestal)
        {
            var shrineobj = ItemBuilder.SpriteFromBundle("shrine_artemissile", Initialisation.NPCCollection.GetSpriteIdByName("shrine_artemissile"), Initialisation.NPCCollection, new GameObject("Shrine Artemissile Statue"));
            shrineobj.GetComponent<tk2dSprite>().HeightOffGround = 1.25f;
            shrineobj.GetComponent<tk2dSprite>().renderer.material.shader = ShaderCache.Acquire("Brave/LitTk2dCustomFalloffTiltedCutout");
            shrineobj.GetComponent<tk2dSprite>().usesOverrideMaterial = true;
            pedestal.AddComponent<ArtemissileShrine>();
            GameObject talkpoint = new GameObject("talkpoint");
            talkpoint.transform.SetParent(pedestal.transform);
            talkpoint.transform.localPosition = new Vector3(1f, 36f / 16f, 0f);
            return shrineobj;
        }
        public override bool CanAccept(PlayerController interactor)
        {
            if (timesAccepted > 0) { return false; }
            if (interactor.characterIdentity == OMITBChars.Shade) return true;
            if (interactor.ForceZeroHealthState && interactor.healthHaver.Armor > 2) { return true; }
            else if (interactor.healthHaver.GetMaxHealth() > 1) { return true; }
            return false;
        }
        public override void OnAccept(PlayerController Interactor)
        {
            if (Interactor.ForceZeroHealthState)
            {
                if (Interactor.characterIdentity != OMITBChars.Shade)
                {
                    Interactor.healthHaver.Armor -= 2;
                }
            }
            else
            {
                StatModifier HP = new StatModifier
                {
                    statToBoost = PlayerStats.StatType.Health,
                    amount = -1f,
                    modifyType = StatModifier.ModifyMethod.ADDITIVE
                };
                Interactor.ownerlessStatModifiers.Add(HP);
                Interactor.stats.RecalculateStats(Interactor);
            }
            StatModifier statModifier = new StatModifier
            {
                statToBoost = PlayerStats.StatType.Curse,
                amount = 2.5f,
                modifyType = StatModifier.ModifyMethod.ADDITIVE
            };
            Interactor.ownerlessStatModifiers.Add(statModifier);
            Interactor.stats.RecalculateStats(Interactor);

            Gun gun = LootEngine.GetItemOfTypeAndQuality<Gun>(PickupObjectDatabase.Instance.GetRandomQuality(), GameManager.Instance.RewardManager.GunsLootTable, false);

            Debug.Log($"Tryget gun for Artemissile, ID: {gun.PickupObjectId}");

            GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(ResourceCache.Acquire("Global Prefabs/HoveringGun") as GameObject, Interactor.CenterPosition.ToVector3ZisY(0f), Quaternion.identity);
            gameObject.transform.parent = Interactor.transform;
            HoveringGunController hover = gameObject.GetComponent<HoveringGunController>();
            hover.ConsumesTargetGunAmmo = false;
            hover.ChanceToConsumeTargetGunAmmo = 0f;
            hover.Position = HoveringGunController.HoverPosition.CIRCULATE;
            hover.Aim = HoveringGunController.AimType.PLAYER_AIM;
            hover.Trigger = HoveringGunController.FireType.ON_RELOAD;
            hover.CooldownTime = GetProperShootingSpeed(gun);
            hover.ShootDuration = GetProperShootDuration(gun);
            hover.OnlyOnEmptyReload = false;
            hover.Initialize(gun, Interactor);

            Interactor.stats.RecalculateStats(Interactor, false, false);
            DeregisterMapIcon();

            GameUIRoot.Instance.notificationController.DoCustomNotification(
                    "Enchanted Gun",
                    "Blessing Of The Hunt",
                    Initialisation.NPCCollection,
                    Initialisation.NPCCollection.GetSpriteIdByName("artemissile_popup"),
                    UINotificationController.NotificationColor.SILVER,
                    true,
                    false
                    );
            AkSoundEngine.PostEvent("Play_OBJ_shrine_accept_01", base.gameObject);
        }
        public override string AcceptText(PlayerController interactor)
        {
            if (interactor.characterIdentity == OMITBChars.Shade) { return "Prove your devotion to the hunt <Lose Nothing>"; }
            if (interactor.ForceZeroHealthState) { return $"Prove your devotion to the hunt <Lose 2 [sprite \"armor_money_icon_001\"]>"; }
            return $"Prove your devotion to the hunt <Lose 1 [sprite \"heart_big_idle_001\"] Container>";
        }
        public override string DeclineText(PlayerController Interactor)
        {
            return "Leave";
        }
        public override string PanelText(PlayerController Interactor)
        {
            return timesAccepted == 0 ? "A shrine to Artemissile, goddess of the eternal hunt. She grants enchanted arms to her most devout followers." : "The spirits inhabiting this shrine have departed...";
        }

        public static float GetProperShootingSpeed(Gun gun)
        {
            float start = gun.DefaultModule.cooldownTime;
            if (gun.DefaultModule.shootStyle == ProjectileModule.ShootStyle.Charged)
            {
                if (gun.DefaultModule.chargeProjectiles != null)
                {
                    start += gun.DefaultModule.chargeProjectiles[0].ChargeTime;
                }
            }
            if (gun.DefaultModule.numberOfShotsInClip <= 1)
            {
                start += gun.reloadTime;
            }
            return start;
        }
        public static float GetProperShootDuration(Gun gun)
        {
            float start = 1;
            if (gun.DefaultModule != null && gun.DefaultModule.projectiles != null && gun.DefaultModule.projectiles[0] != null && gun.DefaultModule.projectiles[0].GetComponent<BeamController>() != null)
            {
                if (gun.DefaultModule.projectiles[0].GetComponent<BeamController>().usesChargeDelay == true)
                {
                    start += gun.DefaultModule.projectiles[0].GetComponent<BeamController>().chargeDelay;
                }
            }
            return start;
        }
    }
}

