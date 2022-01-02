using Dungeonator;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace NevernamedsItems
{
    static class OMITBMathsAndLogicExtensions
    {
        public static Vector2 GetCenteredLookingPosForObj(this Vector2 originalValue, SpeculativeRigidbody rigidBody, bool centerX = true, bool centerY = false)
        {
            float UnitX = rigidBody.UnitDimensions.x;
            float ReturnX = originalValue.x;
            if (centerX) ReturnX -= (UnitX * 0.5f);

            float UnitY = rigidBody.UnitDimensions.y;
            float ReturnY = originalValue.y;
            if (centerY) ReturnY -= (UnitY * 0.5f);

            return new Vector2(ReturnX, ReturnY);
        }
        public static bool isEven(this float number)
        {
            if (number % 2 == 0) return true;
            else return false;
        }
        public static bool isEven(this int number)
        {
            if (number % 2 == 0) return true;
            else return false;
        }
        public static bool IsBetweenRange(this float numberToCheck, float bottom, float top)
        {
            return (numberToCheck >= bottom && numberToCheck <= top);
        }
        public static T KeyByValue<T, W>(this Dictionary<T, W> dict, W val)
        {
            T key = default;
            foreach (KeyValuePair<T, W> pair in dict)
            {
                if (EqualityComparer<W>.Default.Equals(pair.Value, val))
                {
                    key = pair.Key;
                    break;
                }
            }
            return key;
        }
        public static Vector2 GetPositionOfNearestEnemy(this Vector2 startPosition, bool canTargetNonRoomClear, bool targetSprite = false, List<AIActor> excludedActors = null)
        {
            List<AIActor> exclude = new List<AIActor>();
            if (excludedActors != null && excludedActors.Count > 0) exclude.AddRange(excludedActors);
            Func<AIActor, bool> isValid = (AIActor a) => a && a.HasBeenEngaged && a.healthHaver && a.healthHaver.IsVulnerable && a.healthHaver.IsAlive && !a.IsGone &&!exclude.Contains(a);
            IntVector2 intVectorStartPos = startPosition.ToIntVector2();
            RoomHandler.ActiveEnemyType enemyType = RoomHandler.ActiveEnemyType.RoomClear;
            if (canTargetNonRoomClear) enemyType = RoomHandler.ActiveEnemyType.All;
            AIActor closestToPosition = BraveUtility.GetClosestToPosition<AIActor>(GameManager.Instance.Dungeon.data.GetAbsoluteRoomFromPosition(intVectorStartPos).GetActiveEnemies(enemyType), startPosition, isValid, new AIActor[] { });
            if (closestToPosition == null) return Vector2.zero;
            if (targetSprite && closestToPosition.sprite) return closestToPosition.sprite.WorldCenter;
            else return closestToPosition.specRigidbody.UnitCenter;
        }

        public static Vector2 GetVectorToNearestEnemy(this Vector2 bulletPosition, float angleFromAim = 0, float angleVariance = 0, PlayerController playerToScaleAccuracyOff = null, List<AIActor> excludedActors = null)
        {
            List<AIActor> exclude = new List<AIActor>();
            if (excludedActors != null && excludedActors.Count > 0) exclude.AddRange(excludedActors);
            Vector2 dirVec = UnityEngine.Random.insideUnitCircle;
            Func<AIActor, bool> isValid = (AIActor a) => a && a.HasBeenEngaged && a.healthHaver && a.healthHaver.IsVulnerable && !exclude.Contains(a);
            IntVector2 bulletPositionIntVector2 = bulletPosition.ToIntVector2();
            AIActor closestToPosition = BraveUtility.GetClosestToPosition<AIActor>(GameManager.Instance.Dungeon.data.GetAbsoluteRoomFromPosition(bulletPositionIntVector2).GetActiveEnemies(RoomHandler.ActiveEnemyType.All), bulletPosition, isValid, new AIActor[]
            {

            });
            if (closestToPosition)
            {
                dirVec = closestToPosition.CenterPosition - bulletPosition;
            }
            if (angleFromAim != 0)
            {
                dirVec = dirVec.Rotate(angleFromAim);
            }
            if (angleVariance != 0)
            {
                if (playerToScaleAccuracyOff != null) angleVariance *= playerToScaleAccuracyOff.stats.GetStatValue(PlayerStats.StatType.Accuracy);
                float positiveVariance = angleVariance * 0.5f;
                float negativeVariance = positiveVariance * -1f;
                float finalVariance = UnityEngine.Random.Range(negativeVariance, positiveVariance);
                dirVec = dirVec.Rotate(finalVariance);
            }
            return dirVec;
        }
        public static Vector2 CalculateVectorBetween(this Vector2 startVector, Vector2 endVector)
        {
            Vector2 dirVec = endVector - startVector;
            return dirVec;
        }
        public static Vector2 CalculateVectorBetween(this Vector3 startVector, Vector3 endVector)
        {
            Vector2 dirVec = endVector - startVector;
            return dirVec;
        }
        public static Vector2 GetVectorToNearestEnemy(this Vector3 bulletPos, float angleFromAim = 0, float angleVariance = 0, PlayerController playerToScaleAccuracyOff = null)
        {
            Vector2 dirVec = UnityEngine.Random.insideUnitCircle;
            Vector2 bulletPosition = bulletPos;
            Func<AIActor, bool> isValid = (AIActor a) => a && a.HasBeenEngaged && a.healthHaver && a.healthHaver.IsVulnerable;
            IntVector2 bulletPositionIntVector2 = bulletPosition.ToIntVector2();
            AIActor closestToPosition = BraveUtility.GetClosestToPosition<AIActor>(GameManager.Instance.Dungeon.data.GetAbsoluteRoomFromPosition(bulletPositionIntVector2).GetActiveEnemies(RoomHandler.ActiveEnemyType.All), bulletPosition, isValid, new AIActor[]
            {

            });
            if (closestToPosition)
            {
                dirVec = closestToPosition.CenterPosition - bulletPosition;
            }
            if (angleFromAim != 0)
            {
                dirVec = dirVec.Rotate(angleFromAim);
            }
            if (angleVariance != 0)
            {
                if (playerToScaleAccuracyOff != null) angleVariance *= playerToScaleAccuracyOff.stats.GetStatValue(PlayerStats.StatType.Accuracy);
                float positiveVariance = angleVariance * 0.5f;
                float negativeVariance = positiveVariance * -1f;
                float finalVariance = UnityEngine.Random.Range(negativeVariance, positiveVariance);
                dirVec = dirVec.Rotate(finalVariance);
            }
            return dirVec;
        }
        public static bool PositionBetweenRelativeValidAngles(this Vector2 positionToCheck, Vector2 startPosition, float centerAngle, float range, float validAngleVariation)
        {
            if (Vector2.Distance(positionToCheck, startPosition) < range)
            {
                float num7 = BraveMathCollege.Atan2Degrees(positionToCheck - startPosition);
                float minRawAngle = Math.Min(validAngleVariation, -validAngleVariation);
                float maxRawAngle = Math.Max(validAngleVariation, -validAngleVariation);
                bool isInRange = false;
                float actualMaxAngle = centerAngle + maxRawAngle;
                float actualMinAngle = centerAngle + minRawAngle;

                if (num7.IsBetweenRange(actualMinAngle, actualMaxAngle)) isInRange = true;
                if (actualMaxAngle > 180)
                {
                    float Overflow = actualMaxAngle - 180;
                    if (num7.IsBetweenRange(-180, (-180 + Overflow))) isInRange = true;
                }
                if (actualMinAngle < -180)
                {
                    float Underflow = actualMinAngle + 180;
                    if (num7.IsBetweenRange((180 + Underflow), 180)) isInRange = true;
                }
                return isInRange;
            }
            return false;
        }
        public static Vector2 RadianToVector2(this float radian)
        {
            return new Vector2(Mathf.Cos(radian), Mathf.Sin(radian));
        }
        public static Vector2 DegreeToVector2(this float degree)
        {
            return (degree * Mathf.Deg2Rad).RadianToVector2();
        }
        public static Vector2 DegreeToVector2(this int degree)
        {
            return (degree * Mathf.Deg2Rad).RadianToVector2();
        }
        public static List<int> RemoveInvalidIDListEntries(this List<int> starterList, bool checkPlayerInventories = true, bool checkUnlocked = true)
        {
            List<int> returnList = new List<int>();
            returnList.AddRange(starterList);
            for (int i = returnList.Count; i > 0; i--)
            {
                int ID = returnList[i - 1];
                if (checkPlayerInventories)
                {
                    if (GameManager.Instance.PrimaryPlayer && GameManager.Instance.PrimaryPlayer.HasPickupID(ID))
                    {
                        returnList.RemoveAt(i - 1);
                    }
                    else if (GameManager.Instance.SecondaryPlayer && GameManager.Instance.SecondaryPlayer.HasPickupID(ID))
                    {
                        returnList.RemoveAt(i - 1);
                    }
                }
                if (checkUnlocked)
                {
                    PickupObject itemByID = PickupObjectDatabase.GetById(ID);
                    if (!itemByID.PrerequisitesMet())
                    {
                        returnList.RemoveAt(i - 1);
                    }
                }
            }
            return returnList;
        }
        public static PickupObject.ItemQuality GetRandomQuality(this PickupObjectDatabase dat, float dChance = 0.35f, float cChance = 0.32f, float bChance = 0.2f, float aChance = 0.09f, float sChance = 0.04f)
        {
            float random = UnityEngine.Random.value;
            if (random <= sChance) { return PickupObject.ItemQuality.S; }
            else if (random <= aChance) { return PickupObject.ItemQuality.A; }
            else if (random <= bChance) { return PickupObject.ItemQuality.B; }
            else if (random <= cChance) { return PickupObject.ItemQuality.C; }
            else { return PickupObject.ItemQuality.D; }
        }
        public static void SetForSeconds(this bool boolToSet, bool targetVal, float time)
        {
            GameManager.Instance.StartCoroutine(HandleTimedBool(boolToSet, targetVal, time));
        }
        private static IEnumerator HandleTimedBool(bool boolToSet, bool targetVal, float time)
        {
            bool origVal = boolToSet;
            boolToSet = targetVal;
            yield return new WaitForSeconds(time);
            boolToSet = origVal;
            yield break;
        }
    }
    public static class RandomEnum<T>
    {
        static T[] m_Values;
        static RandomEnum()
        {
            var values = System.Enum.GetValues(typeof(T));
            m_Values = new T[values.Length];
            for (int i = 0; i < m_Values.Length; i++)
                m_Values[i] = (T)values.GetValue(i);
        }
        public static T Get()
        {
            return m_Values[UnityEngine.Random.Range(0, m_Values.Length)];
        }
    }
}
