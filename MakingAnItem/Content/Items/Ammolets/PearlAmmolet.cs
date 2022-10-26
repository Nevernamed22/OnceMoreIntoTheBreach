using UnityEngine;
using Alexandria.ItemAPI;
using Dungeonator;
using System.Collections;
using System;
using MonoMod.RuntimeDetour;
using System.Reflection;
using Gungeon;
using System.Collections.Generic;
using SaveAPI;
using Alexandria.Misc;

namespace NevernamedsItems
{
    public class PearlAmmolet : BlankModificationItem
    {
        public static void Init()
        {
            string itemName = "Pearl Ammolet";
            string resourceName = "NevernamedsItems/Resources/Ammolets/pearlammolet_icon";
            GameObject obj = new GameObject(itemName);
            var item = obj.AddComponent<PearlAmmolet>();
            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);
            string shortDesc = "Blanks Bubble";
            string longDesc = "Blanks convert enemy bullets into bubbles."+"\n\n Stolen from the Mother Clam, in a daring heist along the floor of a bottomless ocean.";
            ItemBuilder.SetupItem(item, shortDesc, longDesc, "nn");


            item.quality = PickupObject.ItemQuality.C;
            ItemBuilder.AddPassiveStatModifier(item, PlayerStats.StatType.AdditionalBlanksPerFloor, 1f, StatModifier.ModifyMethod.ADDITIVE);
            item.AddToSubShop(ItemBuilder.ShopType.OldRed);
            ID = item.PickupObjectId;
            item.SetTag("ammolet");
        }

        private static int ID;
        public static void OnBlankedProjectile(Projectile proj)
        {
            if (GameManager.Instance.PrimaryPlayer)
            {
                GameObject gameObject = SpawnManager.SpawnProjectile((PickupObjectDatabase.GetById(599) as Gun).DefaultModule.projectiles[0].gameObject, proj.transform.position + UnityEngine.Random.insideUnitCircle.ToVector3ZisY(0f), Quaternion.identity, true);
                Projectile component = gameObject.GetComponent<Projectile>();
                component.Owner = GameManager.Instance.PrimaryPlayer;
                component.Shooter = GameManager.Instance.PrimaryPlayer.specRigidbody;
                if (GameManager.Instance.AnyPlayerHasActiveSynergy("Bubble Blowing, Baby!")) { component.RuntimeUpdateScale(1.3f); component.baseData.damage *= 1.5f; }
                component.collidesWithPlayer = false;
                component.collidesWithEnemies = true;
                component.collidesWithProjectiles = false;
                component.StartCoroutine(OnBlankedDelay(component, proj.Direction));
                component.gameObject.AddComponent<DieWhenOwnerNotInRoom>();
            }
        }
        public static IEnumerator OnBlankedDelay(Projectile component, Vector2 direction)
        {
            yield return new WaitForEndOfFrame();
            if (component) component.SendInDirection(direction, true);
            yield break;
        }
        public override void Pickup(PlayerController player)
        {
            player.GetExtComp().OnBlankModificationItemProcessed += OnBlankModTriggered;
            base.Pickup(player);
        }
        public override void DisableEffect(PlayerController player)
        {
            player.GetExtComp().OnBlankModificationItemProcessed -= OnBlankModTriggered;
            base.DisableEffect(player);
        }
        private void OnBlankModTriggered(PlayerController user, SilencerInstance blank, Vector2 pos, BlankModificationItem item)
        {
            if (item is PearlAmmolet)
            {
                blank.UsesCustomProjectileCallback = true;
                blank.OnCustomBlankedProjectile += OnBlankedProjectile;
            }
        }
    }
}