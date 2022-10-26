using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace NevernamedsItems
{
    static class ProjSpawnHelper
    {
        
    
    public static void EasyAnimate(this Projectile proj, List<string> projectileNames, int frames, IntVector2 dimensions, int fps, bool light, bool loop = true)
    {
        proj.AnimateProjectile(projectileNames, fps, loop, AnimateBullet.ConstructListOfSameValues(dimensions, frames),
        AnimateBullet.ConstructListOfSameValues(light, frames),
        AnimateBullet.ConstructListOfSameValues(tk2dBaseSprite.Anchor.MiddleCenter, frames),
        AnimateBullet.ConstructListOfSameValues(true, frames),
        AnimateBullet.ConstructListOfSameValues(false, frames),
        AnimateBullet.ConstructListOfSameValues<Vector3?>(null, frames),
        AnimateBullet.ConstructListOfSameValues<IntVector2?>(null, frames),
        AnimateBullet.ConstructListOfSameValues<IntVector2?>(null, frames),
        AnimateBullet.ConstructListOfSameValues<Projectile>(null, frames));
    }
    public static void RenderTilePiercingForSeconds(this Projectile proj, float seconds) { proj.StartCoroutine(NoCollideTilesForSeconds(proj, seconds)); }
    private static IEnumerator NoCollideTilesForSeconds(Projectile proj, float time)
    {
        if (proj && proj.specRigidbody)
        {
            proj.specRigidbody.CollideWithTileMap = false;
            proj.UpdateCollisionMask();

            yield return new WaitForSeconds(time);

            if (proj && proj.specRigidbody)
            {
                proj.specRigidbody.CollideWithTileMap = true;
                proj.UpdateCollisionMask();
            }
        }
        yield break;
    }
    public static void ScaleByPlayerStats(this Projectile proj, PlayerController player)
    {
        if (player != null)
        {
            proj.baseData.damage *= player.stats.GetStatValue(PlayerStats.StatType.Damage);
            proj.baseData.speed *= player.stats.GetStatValue(PlayerStats.StatType.ProjectileSpeed);
            proj.baseData.range *= player.stats.GetStatValue(PlayerStats.StatType.RangeMultiplier);
            proj.baseData.force *= player.stats.GetStatValue(PlayerStats.StatType.KnockbackMultiplier);
            proj.BossDamageMultiplier *= player.stats.GetStatValue(PlayerStats.StatType.DamageToBosses);
            proj.UpdateSpeed();
        }
    }
    public static float GetAccuracyAngled(float startFloat, float variance, PlayerController playerToScaleAccuracyOff = null)
    {
        if (playerToScaleAccuracyOff != null) variance *= playerToScaleAccuracyOff.stats.GetStatValue(PlayerStats.StatType.Accuracy);
        float positiveVariance = variance * 0.5f;
        float negativeVariance = positiveVariance * -1f;
        float finalVariance = UnityEngine.Random.Range(negativeVariance, positiveVariance);
        return startFloat + finalVariance;
    }
    public static GameObject SpawnProjectileTowardsPoint(GameObject projectile, Vector2 startingPosition, Vector2 targetPosition, float angleOffset = 0, float angleVariance = 0, PlayerController playerToScaleAccuracyOff = null)
    {
        Vector2 dirVec = (targetPosition - startingPosition);
        if (angleOffset != 0)
        {
            dirVec = dirVec.Rotate(angleOffset);
        }
        if (angleVariance != 0)
        {
            if (playerToScaleAccuracyOff != null) angleVariance *= playerToScaleAccuracyOff.stats.GetStatValue(PlayerStats.StatType.Accuracy);
            float positiveVariance = angleVariance * 0.5f;
            float negativeVariance = positiveVariance * -1f;
            float finalVariance = UnityEngine.Random.Range(negativeVariance, positiveVariance);
            dirVec = dirVec.Rotate(finalVariance);
        }
        return SpawnManager.SpawnProjectile(projectile, startingPosition, Quaternion.Euler(0f, 0f, dirVec.ToAngle()), true);
    }

}
}
