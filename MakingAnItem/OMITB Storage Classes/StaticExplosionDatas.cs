using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NevernamedsItems
{
    class StaticExplosionDatas
    {
        public static ExplosionData explosiveRoundsExplosion = Gungeon.Game.Items["explosive_rounds"].GetComponent<ComplexProjectileModifier>().ExplosionData;
        public static ExplosionData genericSmallExplosion = GameManager.Instance.Dungeon.sharedSettingsPrefab.DefaultSmallExplosionData;
        public static ExplosionData genericLargeExplosion = GameManager.Instance.Dungeon.sharedSettingsPrefab.DefaultExplosionData;
        public static ExplosionData customDynamiteExplosion = new ExplosionData()
        {
            effect = genericLargeExplosion.effect,
            ignoreList = genericLargeExplosion.ignoreList,
            ss = genericLargeExplosion.ss,
            damageRadius = 5f,
            damageToPlayer = 0f,
            doDamage = true,
            damage = 45,
            doDestroyProjectiles = true,
            doForce = true,
            debrisForce = 30f,
            preventPlayerForce = true,
            explosionDelay = 0.1f,
            usesComprehensiveDelay = false,
            doScreenShake = true,
            playDefaultSFX = true,
        };
    }
}
