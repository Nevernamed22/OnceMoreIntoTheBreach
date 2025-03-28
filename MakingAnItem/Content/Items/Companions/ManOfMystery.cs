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
    public class ManOfMystery : ExtendedCompanionItem
    {
        public static void Init()
        {
            ManOfMystery companionItem = ItemSetup.NewItem<ManOfMystery>(
            "Man of Mystery",
            "International",
            "A mysterious companion.\n\nWhile their goals and motivations remain unknown, it's clear they're in this for the long haul.",
            "manofmystery_icon") as ManOfMystery;
            companionItem.quality = PickupObject.ItemQuality.C;

            companionItem.CompanionGuid = RandomisedBuddyController.guid;

            ID = companionItem.PickupObjectId;
        }

        public override void Pickup(PlayerController player)
        {
            if (!m_pickedUpThisRun)
            {
                Recalculate();     
            }
            base.Pickup(player);
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