﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NevernamedsItems
{
    static class OMITBGunExtensions
    {
        public static PlayerController GunPlayerOwner(this Gun bullet)
        {
            if (bullet && bullet.CurrentOwner && bullet.CurrentOwner is PlayerController) return bullet.CurrentOwner as PlayerController;
            else return null;
        }
        public static ProjectileModule AddProjectileModuleToRawVolley(this Gun gun, ProjectileModule projectile)
        {
            gun.RawSourceVolley.projectiles.Add(projectile);
            return projectile;
        }
        public static ProjectileModule AddProjectileModuleToRawVolleyFrom(this Gun gun, Gun other, bool cloned = true, bool clonedProjectiles = true)
        {
            ProjectileModule defaultModule = other.DefaultModule;
            if (!cloned)
            {
                return gun.AddProjectileModuleToRawVolley(defaultModule);
            }
            ProjectileModule projectileModule = ProjectileModule.CreateClone(defaultModule, false, -1);
            projectileModule.projectiles = new List<Projectile>(defaultModule.projectiles.Capacity);
            for (int i = 0; i < defaultModule.projectiles.Count; i++)
            {
                projectileModule.projectiles.Add((!clonedProjectiles) ? defaultModule.projectiles[i] : defaultModule.projectiles[i].ClonedPrefab());
            }
            return gun.AddProjectileModuleToRawVolley(projectileModule);
        }
        public static ProjectileModule RawDefaultModule(this Gun self)
        {
            if (self.RawSourceVolley)
            {
                if (self.RawSourceVolley.ModulesAreTiers)
                {
                    for (int i = 0; i < self.RawSourceVolley.projectiles.Count; i++)
                    {
                        ProjectileModule projectileModule = self.RawSourceVolley.projectiles[i];
                        if (projectileModule != null)
                        {
                            int num = (projectileModule.CloneSourceIndex < 0) ? i : projectileModule.CloneSourceIndex;
                            if (num == self.CurrentStrengthTier)
                            {
                                return projectileModule;
                            }
                        }
                    }
                }
                return self.RawSourceVolley.projectiles[0];
            }
            return self.singleModule;
        }
        public static void AddStatToGun(this Gun item, PlayerStats.StatType statType, float amount, StatModifier.ModifyMethod method = StatModifier.ModifyMethod.ADDITIVE)
        {
            StatModifier modifier = new StatModifier
            {
                amount = amount,
                statToBoost = statType,
                modifyType = method
            };

            if (item.passiveStatModifiers == null)
                item.passiveStatModifiers = new StatModifier[] { modifier };
            else
                item.passiveStatModifiers = item.passiveStatModifiers.Concat(new StatModifier[] { modifier }).ToArray();
        }
        public static void RemoveStatFromGun(this Gun item, PlayerStats.StatType statType)
        {
            var newModifiers = new List<StatModifier>();
            for (int i = 0; i < item.passiveStatModifiers.Length; i++)
            {
                if (item.passiveStatModifiers[i].statToBoost != statType)
                    newModifiers.Add(item.passiveStatModifiers[i]);
            }
            item.passiveStatModifiers = newModifiers.ToArray();
        }
    }
}