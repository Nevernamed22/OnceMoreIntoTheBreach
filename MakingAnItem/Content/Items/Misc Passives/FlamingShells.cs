using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using Alexandria.ItemAPI;
using Alexandria.Misc;
using UnityEngine;
using MonoMod.RuntimeDetour;
using System.Reflection;
using SaveAPI;

namespace NevernamedsItems
{
    public class FlamingShells : PassiveItem
    {
        public static void Init()
        {
            PickupObject item = ItemSetup.NewItem<FlamingShells>(
            "Flaming Shells",
            "Hothothothothothothot",
            "Adds a spurt of flames to every gunshot...\n\nThese ingenious shells were created by attaching a primer directly to an open flame. Why didn't we do this earlier?!",
            "flamingshells_icon");
            item.quality = PickupObject.ItemQuality.B;
            ID = item.PickupObjectId;

            item.SetupUnlockOnCustomStat(CustomTrackedStats.BEGGAR_TOTAL_DONATIONS, 34, DungeonPrerequisite.PrerequisiteOperation.GREATER_THAN);
            Doug.AddToLootPool(item.PickupObjectId);
        }
        public static int ID;

        public static List<Projectile> options = new List<Projectile>();
        private Gun lastCheckedGun;
        public override void Update()
        {
            if (look)
            {
                if (Owner && Owner.CurrentGun != null && Owner.CurrentGun != lastCheckedGun)
                {
                    if (lastCheckedGun != null) { lastCheckedGun.OnPostFired -= OnFiredGun; }
                    Owner.CurrentGun.OnPostFired += OnFiredGun;
                    lastCheckedGun = Owner.CurrentGun;
                }
            }
            base.Update();
        }
        public void OnFiredGun(PlayerController shooter, Gun gun)
        {
            //ETGModConsole.Log("Running");
            if (shooter && gun && Owner && shooter == Owner)
            {
                float num2 = 1f / gun.DefaultModule.cooldownTime;

                float x = 8f;
                if (gun.DefaultModule.shootStyle == ProjectileModule.ShootStyle.Beam) { x = 0.1f; }

                float finalChance = x / num2;
                finalChance = Mathf.Max(0.1f, finalChance);

                List<Projectile> toFire = new List<Projectile>();

                float remainingPoints = finalChance;
                while (remainingPoints > 0)
                {
                    //ETGModConsole.Log(remainingPoints);
                    if (remainingPoints >= 1f)
                    {
                        toFire.Add(BraveUtility.RandomElement(options));
                        remainingPoints -= 1f;
                    }
                    else 
                    {
                        if (UnityEngine.Random.value <= remainingPoints) { toFire.Add(BraveUtility.RandomElement(options)); }
                        remainingPoints = 0;
                    }
                }

                if (toFire.Count > 0)
                {
                    foreach (Projectile proj in toFire)
                    {
                        Projectile fired = proj.InstantiateAndFireInDirection(gun.barrelOffset.position, gun.CurrentAngle, 45f, shooter).GetComponent<Projectile>();
                        fired.Owner = shooter;
                        fired.Shooter = shooter.specRigidbody;
                        fired.ScaleByPlayerStats(shooter);
                        shooter.DoPostProcessProjectile(fired);

                        fired.baseData.speed *= UnityEngine.Random.Range(0.5f, 1f);
                        fired.UpdateSpeed();
                        fired.baseData.range = UnityEngine.Random.Range(3f, 5f);
                    }
                }
            }
        }
        public bool look = false;
        private void PostProcessBeamTick(BeamController bemcont, SpeculativeRigidbody beam, float effectChanceScalar)
        {
            if (base.Owner)
            {
                Projectile fired = BraveUtility.RandomElement(options).InstantiateAndFireInDirection(bemcont.Origin, bemcont.Direction.ToAngle(), 45f, base.Owner).GetComponent<Projectile>();
                fired.Owner = base.Owner;
                fired.Shooter = base.Owner.specRigidbody;
                fired.ScaleByPlayerStats(base.Owner);
                base.Owner.DoPostProcessProjectile(fired);

                fired.baseData.speed *= UnityEngine.Random.Range(0.5f, 1f);
                fired.baseData.damage *= 0.5f;
                fired.UpdateSpeed();
                fired.baseData.range = UnityEngine.Random.Range(3f, 5f);
                if (fired.gameObject.GetComponent<SlowDownOverTimeModifier>() != null) { fired.gameObject.GetComponent<SlowDownOverTimeModifier>().timeTillKillAfterCompleteStop = 5f; }
            }
        }
        public override void Pickup(PlayerController player)
        {
            if (options.Count == 0)
            {
                options.Add((PickupObjectDatabase.GetById(336) as Gun).DefaultModule.projectiles[0]);
                options.Add((PickupObjectDatabase.GetById(336) as Gun).DefaultModule.projectiles[0]);
                options.Add(StandardisedProjectiles.smoke);
                options.Add(StandardisedProjectiles.flamethrower);
                options.Add(StandardisedProjectiles.flamethrower);
            }
           // player.PostProcessBeamTick += this.PostProcessBeamTick;
            look = true;
            base.Pickup(player);
        }
        public override void DisableEffect(PlayerController player)
        {
            look = false;
            if (lastCheckedGun != null)
            {
                lastCheckedGun.OnPostFired -= OnFiredGun;
                lastCheckedGun = null;
            }
           // if (player) { player.PostProcessBeamTick -= this.PostProcessBeamTick; }
            base.DisableEffect(player);
        }
    }
}
