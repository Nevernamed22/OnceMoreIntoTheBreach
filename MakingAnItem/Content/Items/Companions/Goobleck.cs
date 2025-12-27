using System;
using System.Collections;
using System.Collections.Generic;
using Dungeonator;
using Gungeon;
using Alexandria.ItemAPI;
using UnityEngine;
using SaveAPI;

namespace NevernamedsItems
{
    public class Goobleck : ExtendedCompanionItem
    {
        public static void Init()
        {
            Goobleck companionItem = ItemSetup.NewItem<Goobleck>(
            "Goobleck",
            "A Friend In There Somewhere",
            "A friendly glob of psychoputty.\n\nShapes itself around concepts blindly pulled from the noosphere of the collective unconscious, but it cannot maintain one form for too long.",
            "goobleck_icon") as Goobleck;
            companionItem.quality = PickupObject.ItemQuality.C;

            companionItem.CompanionGuid = RandomisedBuddyController.guid;
            companionItem.AddToSubShop(ItemBuilder.ShopType.Goopton);

            ID = companionItem.PickupObjectId;
        }

        public override void Pickup(PlayerController player)
        {
            player.OnNewFloorLoaded += NewLevel;
            player.OnRollStarted += Roll;
            if (!m_pickedUpThisRun)
            {
                Recalculate();
            }
            base.Pickup(player);
        }
        private void Roll(PlayerController p, Vector2 v)
        {
            if (p && p.PlayerHasActiveSynergy("Re Roll We Roll"))
            {
                Recalculate();
                base.ForceCompanionRegeneration(p, null);
            }
        }
        public override void DisableEffect(PlayerController player)
        {
            if (player)
            {
                player.OnNewFloorLoaded -= NewLevel;
                player.OnRollStarted -= Roll;
            }
            base.DisableEffect(player);
        }
        public void NewLevel(PlayerController pl)
        {
            Recalculate();
        }
        public void Recalculate()
        {
            companionIdentity = BraveUtility.RandomElement(RandomisedBuddyController.validForms);
            damageMultiplier = UnityEngine.Random.Range(0.8f, 1.2f);
            angleVariance = UnityEngine.Random.Range(0f, 10f);
            cooldowntime = UnityEngine.Random.Range(0.35f, 1f);
            reloadTime = UnityEngine.Random.Range(1f, 3f);
            shotSpeedMult = UnityEngine.Random.Range(0.5f, 1.5f);
            clipsize = UnityEngine.Random.Range(1, 8);
            soundEvent = BraveUtility.RandomElement(RandomisedBuddyController.fireSoundEffects);
            muzzleFlash = BraveUtility.RandomElement(RandomisedBuddyController.validMuzzleFlashes);
            chosenProjectile = BraveUtility.RandomElement(HelmOfChaos.PossibleProj);
        }
        public override void OnCompanionCreation(PlayerController owner)
        {
            if (m_extantCompanion && m_extantCompanion.GetComponent<RandomisedBuddyController>() && m_extantCompanion.GetComponent<RandomisedBuddyController>().currentForm != companionIdentity)
            {
                RandomisedBuddyController budController = m_extantCompanion.GetComponent<RandomisedBuddyController>();
                budController.currentForm = companionIdentity;

                budController.shotsInClip = clipsize;
                budController.reloadTime = reloadTime;
                budController.cooldownTime = cooldowntime;
                budController.soundEvent = soundEvent;
                budController.MuzzleFlash = muzzleFlash;
                budController.chosenProjectile = chosenProjectile;
                budController.angleVariance = angleVariance;
                budController.setUp = true;
            }
            base.OnCompanionCreation(owner);
        }

        public string soundEvent;
        public VFXPool muzzleFlash;
        public Projectile chosenProjectile;
        public string companionIdentity;
        public float damageMultiplier = 1;
        public float angleVariance = 5;
        public float cooldowntime = 0.25f;
        public int clipsize = 5;
        public float reloadTime = 1;
        public float shotSpeedMult = 1f;
        public static int ID;
    }
}