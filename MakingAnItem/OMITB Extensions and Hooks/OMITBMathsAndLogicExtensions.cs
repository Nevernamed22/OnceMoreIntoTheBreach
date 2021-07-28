using Dungeonator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace NevernamedsItems
{
   static class OMITBMathsAndLogicExtensions
    {
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
        public static Vector2 GetPositionOfNearestEnemy(this Vector2 startPosition, bool canTargetNonRoomClear, bool targetSprite = false)
        {
            Func<AIActor, bool> isValid = (AIActor a) => a && a.HasBeenEngaged && a.healthHaver && a.healthHaver.IsVulnerable;
            IntVector2 intVectorStartPos = startPosition.ToIntVector2();
            RoomHandler.ActiveEnemyType enemyType = RoomHandler.ActiveEnemyType.RoomClear;
            if (canTargetNonRoomClear) enemyType = RoomHandler.ActiveEnemyType.All;
            AIActor closestToPosition = BraveUtility.GetClosestToPosition<AIActor>(GameManager.Instance.Dungeon.data.GetAbsoluteRoomFromPosition(intVectorStartPos).GetActiveEnemies(enemyType), startPosition, isValid, new AIActor[] { });
            if (closestToPosition == null) return Vector2.zero;
            if (targetSprite && closestToPosition.sprite) return closestToPosition.sprite.WorldCenter;
            else return closestToPosition.specRigidbody.UnitCenter;
        }
        public static Vector2 GetVectorToNearestEnemy(this Vector2 bulletPosition, float angleFromAim = 0, float angleVariance = 0, PlayerController playerToScaleAccuracyOff = null)
        {
            Vector2 dirVec = UnityEngine.Random.insideUnitCircle;
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
