using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Dungeonator;
using UnityEngine;
using Alexandria.ItemAPI;
using Alexandria.Misc;
using System.Collections;

namespace NevernamedsItems
{
    class HEVSuit : PassiveItem
    {
        public static void Init()
        {
            PassiveItem item = ItemSetup.NewItem<HEVSuit>(
               "Hazardous Environment Suit Mk. 4",
               "You've Earned It",
               "A highly advanced protective suit for scientific research and surveying in hazardous environments." + "\n\nDeveloped by Primerdyne R&D, this suit was presumed lost in the incident- until it appeared in the Gungeon under unknown circumstances...",
               "hevsuit_icon") as PassiveItem;
            item.quality = PickupObject.ItemQuality.A;
            item.ArmorToGainOnInitialPickup = 1;

        }

        private void OnDamaged(PlayerController user)
        {
            int remainingHP = Mathf.RoundToInt(user.healthHaver.currentHealth / 0.5f);
            remainingHP += (int)user.healthHaver.Armor;

            List<string> lines = new List<string>() { };

            lines.Add("HEV_beep");
            lines.Add(BraveUtility.RandomElement(regDamageResponses));
            if (remainingHP <= 1)
            {
                lines.Add("HEV_deathimminent");
            }
            else if (remainingHP <= 4)
            {
                lines.Add(BraveUtility.RandomElement(criticalDamageFollowups));
            }
            else
            {
                lines.Add(BraveUtility.RandomElement(damageFollowups));
            }

            if (remainingHP > 0)
            {
                if (timeSinceLastDamageMessage > 5)
                {
                    timeSinceLastDamageMessage = 0;
                    Speak(lines);
                }
            }
            else
            {
                speaking = true;
                AkSoundEngine.PostEvent("HEV_death", (user != null) ? user.gameObject : base.gameObject);
            }
        }
        public static List<string> criticalDamageFollowups = new List<string>()
        {
            "HEV_vitalsdropping",
            "HEV_vitalscritical",
            "HEV_seekmedical"
        };
        public static List<string> damageFollowups = new List<string>()
        {
            "HEV_automed",
            "HEV_morphine"
        };
        public static List<string> poisonDialogue = new List<string>()
        {
            "HEV_hazchem",
            "HEV_biohazard"
        };
        public static List<string> regDamageResponses = new List<string>()
        {
            "HEV_bloodloss",
            "HEV_majorlacerations",
            "HEV_minorlacerations",
            "HEV_minorfracture",
            "HEV_majorfracture"
        };
        public Dictionary<string, float> Dialogue = new Dictionary<string, float>()
        {
            { "HEV_intro", 13f },
            { "HEV_ammo", 2f },
            { "HEV_death", 6f },
            { "HEV_beep", 1.5f },
            { "HEV_bloodloss", 2.25f },
            { "HEV_automed", 3.5f },
            { "HEV_vitalsdropping", 3f },
            { "HEV_vitalscritical", 3.25f },
            { "HEV_deathimminent", 3.75f },
            { "HEV_majorlacerations", 3f },
            { "HEV_minorlacerations", 3f },
            { "HEV_minorfracture", 2.5f },
            { "HEV_morphine", 2f },
            { "HEV_seekmedical", 2.5f },
            { "HEV_majorfracture", 2.5f },
            { "HEV_hazchem", 3.5f },
            { "HEV_fire", 4f },
            { "HEV_biohazard", 3.25f },
        };
        private static bool speaking = false;
        private void Speak(List<string> lines, float initidalDelay = 0)
        {
            if (!speaking)
            {
                GameObject source = (Owner != null) ? Owner.gameObject : base.gameObject;
                GameManager.Instance.StartCoroutine(SayLines(lines, source, initidalDelay));
            }
        }
        private IEnumerator SayLines(List<string> lines, GameObject audioSource, float initialDelay = 0)
        {
            speaking = true;
            yield return new WaitForSeconds(initialDelay);
            foreach (string line in lines)
            {
                if (audioSource != null)
                {
                    AkSoundEngine.PostEvent(line, audioSource);
                    yield return new WaitForSeconds(Dialogue[line]);
                }
            }
            speaking = false;
            yield break;
        }
        public override void Pickup(PlayerController player)
        {
            if (!m_pickedUpThisRun)
            {
                Speak(new List<string>() { "HEV_intro" });
            }
            if (fireImmunity == null)
            {
                fireImmunity = new DamageTypeModifier();
                fireImmunity.damageMultiplier = 0.2f;
                fireImmunity.damageType = CoreDamageTypes.Fire;
            }
            if (poisonImmunity == null)
            {
                poisonImmunity = new DamageTypeModifier();
                poisonImmunity.damageMultiplier = 0f;
                poisonImmunity.damageType = CoreDamageTypes.Poison;
            }
            if (electricImmunity == null)
            {
                electricImmunity = new DamageTypeModifier();
                electricImmunity.damageMultiplier = 0f;
                electricImmunity.damageType = CoreDamageTypes.Electric;
            }
            player.OnReceivedDamage += OnDamaged;
            if (player.healthHaver)
            {
                player.healthHaver.damageTypeModifiers.Add(fireImmunity);
                player.healthHaver.damageTypeModifiers.Add(poisonImmunity);
                player.healthHaver.damageTypeModifiers.Add(electricImmunity);
                player.healthHaver.ModifyDamage += ModifyDamage;
            }
            base.Pickup(player);
        }
        private void ModifyDamage(HealthHaver player, HealthHaver.ModifyDamageEventArgs args)
        {
            if (player.gameActor is PlayerController && UnityEngine.Random.value <= 0.15f)
            {
                PlayerController wearer = (player.gameActor as PlayerController);
                args.ModifiedDamage = 0;
                wearer.DoEasyBlank(wearer.CenterPosition, EasyBlankType.FULL);
                if (wearer.GetExtComp() != null)
                {
                    wearer.GetExtComp().TriggerInvulnerableFrames(0.8f, true);
                }
            }
        }
        private DamageTypeModifier fireImmunity;
        private DamageTypeModifier poisonImmunity;
        private DamageTypeModifier electricImmunity;
        public override void DisableEffect(PlayerController player)
        {
            if (player) player.OnReceivedDamage -= OnDamaged;
            if (player.healthHaver)
            {
                player.healthHaver.damageTypeModifiers.Remove(fireImmunity);
                player.healthHaver.damageTypeModifiers.Remove(poisonImmunity);
                player.healthHaver.damageTypeModifiers.Remove(electricImmunity);
                player.healthHaver.ModifyDamage -= ModifyDamage;
            }
            base.DisableEffect(player);
        }

        private float timeSinceLastDamageMessage = 0;
        private float timeSinceLastGoopMessage = 0;
        private bool wasInGoopLastFrame = false;
        private bool wasOnFireLastFrame = false;
        private Gun lastGun;
        private int lastAmmo;
        public override void Update()
        {
            if (Owner)
            {
                timeSinceLastDamageMessage += BraveTime.DeltaTime;
                timeSinceLastGoopMessage += BraveTime.DeltaTime;
                if (Owner.CurrentGun && Owner.CurrentGun == lastGun)
                {
                    if (Owner.CurrentGun.ammo != lastAmmo)
                    {
                        lastAmmo = Owner.CurrentGun.ammo;
                        if (Owner.CurrentGun.ammo == 0)
                        {
                            Speak(new List<string>() { "HEV_ammo" });
                        }
                    }
                }
                lastGun = Owner.CurrentGun;

                if (Owner.IsOnFire && !wasOnFireLastFrame)
                {
                    Speak(new List<string>() { "HEV_beep", "HEV_fire" });
                }
                wasOnFireLastFrame = Owner.IsOnFire;

                if (Owner.CurrentGoop != null && !wasInGoopLastFrame && timeSinceLastGoopMessage > 20)
                {
                    if (Owner.CurrentGoop.AppliesDamageOverTime && Owner.CurrentGoop.HealthModifierEffect != null)
                    {
                        Speak(new List<string>() { BraveUtility.RandomElement(poisonDialogue) });
                        timeSinceLastGoopMessage = 0;
                    }
                }
                wasInGoopLastFrame = Owner.CurrentGoop != null;


            }
            base.Update();
        }

    }
}