using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ItemAPI;
using UnityEngine;

namespace NevernamedsItems
{
    class ExistantGunModifiers
    {
        public static void Init()
        {
            (PickupObjectDatabase.GetById(748) as Gun).gameObject.AddComponent<SunlightJavelinModifiers>();
            (PickupObjectDatabase.GetById(539) as Gun).gameObject.AddComponent<BoxingGloveModifiers>();
            (PickupObjectDatabase.GetById(506) as Gun).gameObject.AddComponent<ReallySpecialLuteModifiers>();
            (PickupObjectDatabase.GetById(93) as Gun).gameObject.AddComponent<OldGoldieModifiers>();
        }
    }
    public class OldGoldieModifiers : GunBehaviour
    {
        public override void PostProcessProjectile(Projectile projectile)
        {
            PlayerController playerController = this.gun.CurrentOwner as PlayerController;
            base.PostProcessProjectile(projectile);
            if (playerController.PlayerHasActiveSynergy("The Classics"))
            {
                projectile.baseData.range *= 2;
            }
        }
    }
    public class ReallySpecialLuteModifiers : GunBehaviour
    {
        public override void PostProcessProjectile(Projectile projectile)
        {
            PlayerController playerController = this.gun.CurrentOwner as PlayerController;
            base.PostProcessProjectile(projectile);
            if (playerController.PlayerHasActiveSynergy("Eternal Prose"))
            {
                projectile.baseData.range *= 10;
                projectile.baseData.speed *= 2;
            }
        }
    }
    public class BoxingGloveModifiers : GunBehaviour
    {
        public override void PostProcessProjectile(Projectile projectile)
        {
            PlayerController playerController = this.gun.CurrentOwner as PlayerController;
            base.PostProcessProjectile(projectile);
            if (playerController.PlayerHasActiveSynergy("Gun Punch Man")) projectile.baseData.damage *= 2;
        }
    }
    public class SunlightJavelinModifiers : GunBehaviour
    {
        public override void PostProcessProjectile(Projectile projectile)
        {
            PlayerController playerController = this.gun.CurrentOwner as PlayerController;
            if (playerController.PlayerHasActiveSynergy("Grease Lightning")) projectile.baseData.damage *= 2;
            if (playerController.PlayerHasActiveSynergy("Gunderbolts and Lightning")) projectile.OnHitEnemy += this.AddFearEffect;
            base.PostProcessProjectile(projectile);
        }
        private FleePlayerData fleeData;
        private void AddFearEffect(Projectile arg1, SpeculativeRigidbody arg2, bool arg3)
        {
            PlayerController playerController = arg1.Owner as PlayerController;
            if (arg2 != null && arg2.aiActor != null && playerController != null && arg2 != null && arg2.healthHaver.IsAlive)
            {
                if (arg2.aiActor.EnemyGuid != "465da2bb086a4a88a803f79fe3a27677" && arg2.aiActor.EnemyGuid != "05b8afe0b6cc4fffa9dc6036fa24c8ec")
                {
                    StartCoroutine(HandleFear(playerController, arg2));
                }
            }
        }
        private IEnumerator HandleFear(PlayerController user, SpeculativeRigidbody enemy)
        {
            if (this.fleeData == null || this.fleeData.Player != user)
            {
                this.fleeData = new FleePlayerData();
                this.fleeData.Player = user;
                this.fleeData.StartDistance *= 2f;
            }
            if (enemy.aiActor.behaviorSpeculator != null)
            {
                enemy.aiActor.behaviorSpeculator.FleePlayerData = this.fleeData;
                FleePlayerData fleePlayerData = new FleePlayerData();
                yield return new WaitForSeconds(10f);
                enemy.aiActor.behaviorSpeculator.FleePlayerData.Player = null;
            }
        }
    }
}
