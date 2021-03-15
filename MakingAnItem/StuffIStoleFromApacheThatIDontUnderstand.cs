using Dungeonator;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using UnityEngine;
using ItemAPI;


namespace NevernamedsItems
{
    public class TeleportNonsense
    {
        public static bool CanBlinkToPoint(PlayerController Owner, Vector2 point, Vector2 centerOffset)
        {
            bool flag = Owner.IsValidPlayerPosition(point + centerOffset);
            if (flag && Owner.CurrentRoom != null)
            {
                CellData cellData = GameManager.Instance.Dungeon.data[point.ToIntVector2(VectorConversions.Floor)];
                if (cellData == null) { return false; }
                RoomHandler nearestRoom = cellData.nearestRoom;
                if (cellData.type != CellType.FLOOR) { flag = false; }
                if (Owner.CurrentRoom.IsSealed && nearestRoom != Owner.CurrentRoom) { flag = false; }
                if (Owner.CurrentRoom.IsSealed && cellData.isExitCell) { flag = false; }
                if (nearestRoom.visibility == RoomHandler.VisibilityStatus.OBSCURED || nearestRoom.visibility == RoomHandler.VisibilityStatus.REOBSCURED) { flag = false; }
            }
            if (Owner.CurrentRoom == null) { flag = false; }
            if (Owner.IsDodgeRolling | Owner.IsFalling | Owner.IsCurrentlyCoopReviving | Owner.IsInMinecart | Owner.IsInputOverridden) { return false; }
            return flag;
        }
        public static Vector2 AdjustInputVector(Vector2 rawInput, float cardinalMagnetAngle, float ordinalMagnetAngle)
        {
            float num = BraveMathCollege.ClampAngle360(BraveMathCollege.Atan2Degrees(rawInput));
            float num2 = num % 90f;
            float num3 = (num + 45f) % 90f;
            float num4 = 0f;
            if (cardinalMagnetAngle > 0f)
            {
                if (num2 < cardinalMagnetAngle)
                {
                    num4 = -num2;
                }
                else if (num2 > 90f - cardinalMagnetAngle)
                {
                    num4 = 90f - num2;
                }
            }
            if (ordinalMagnetAngle > 0f)
            {
                if (num3 < ordinalMagnetAngle)
                {
                    num4 = -num3;
                }
                else if (num3 > 90f - ordinalMagnetAngle)
                {
                    num4 = 90f - num3;
                }
            }
            num += num4;
            return (Quaternion.Euler(0f, 0f, num) * Vector3.right).XY() * rawInput.magnitude;
        }
    }
    public class TeleportPlayerToCursorPosition : MonoBehaviour
    {
        private static Vector2 m_cachedBlinkPosition;
        private static Vector2 lockedDodgeRollDirection;
        public static BlinkPassiveItem m_BlinkPassive = PickupObjectDatabase.GetById(436).GetComponent<BlinkPassiveItem>();
        public GameObject BlinkpoofVfx = m_BlinkPassive.BlinkpoofVfx;
        public static void StartTeleport(PlayerController user)
        {
            GungeonActions m_activeActions = OMITBReflectionHelpers.ReflectGetField<GungeonActions>(typeof(PlayerController), "m_activeActions", user);
            Vector2 currentDirection = TeleportNonsense.AdjustInputVector(m_activeActions.Move.Vector, BraveInput.MagnetAngles.movementCardinal, BraveInput.MagnetAngles.movementOrdinal);
            Vector2 cachedBlinkPosition = m_cachedBlinkPosition;

            bool IsKeyboardAndMouse = BraveInput.GetInstanceForPlayer(user.PlayerIDX).IsKeyboardAndMouse(false);
            if (IsKeyboardAndMouse) { m_cachedBlinkPosition = user.unadjustedAimPoint.XY() - (user.CenterPosition - user.specRigidbody.UnitCenter); }
            else { if (m_activeActions != null) { m_cachedBlinkPosition += m_activeActions.Aim.Vector.normalized * BraveTime.DeltaTime * 15f; } }

            m_cachedBlinkPosition = BraveMathCollege.ClampToBounds(m_cachedBlinkPosition, GameManager.Instance.MainCameraController.MinVisiblePoint, GameManager.Instance.MainCameraController.MaxVisiblePoint);

            user.healthHaver.TriggerInvulnerabilityPeriod(0.001f);
            user.DidUnstealthyAction();
            BlinkToPoint(user, m_cachedBlinkPosition);
        }
        private static void BlinkToPoint(PlayerController Owner, Vector2 targetPoint)
        {
            m_cachedBlinkPosition = targetPoint;
            Vector2 centerOffset = Owner.transform.position.XY() - Owner.specRigidbody.UnitCenter;
            lockedDodgeRollDirection = (m_cachedBlinkPosition - Owner.specRigidbody.UnitCenter).normalized;
            bool flag = TeleportNonsense.CanBlinkToPoint(Owner, m_cachedBlinkPosition, centerOffset);

            if (flag)
            {
                //m_CurrentlyBlinking = true;
                StaticCoroutine.Start(HandleBlinkTeleport(Owner, m_cachedBlinkPosition, lockedDodgeRollDirection));
            }
            else
            {
                Vector2 a = Owner.specRigidbody.UnitCenter - m_cachedBlinkPosition;
                float num = a.magnitude;
                Vector2? vector = null;
                float num2 = 0f;
                a = a.normalized;
                while (num > 0f)
                {
                    num2 += 1f;
                    num -= 1f;
                    Vector2 vector2 = m_cachedBlinkPosition + a * num2;
                    if (TeleportNonsense.CanBlinkToPoint(Owner, vector2 + new Vector2(1, 0), centerOffset))
                    {
                        vector = new Vector2?(vector2);
                        break;
                    }
                }
                if (vector != null)
                {
                    Vector2 normalized = (vector.Value - Owner.specRigidbody.UnitCenter).normalized;
                    float num3 = Vector2.Dot(normalized, lockedDodgeRollDirection);
                    if (num3 > 0f)
                    {
                        m_cachedBlinkPosition = vector.Value;
                        //m_CurrentlyBlinking = true;
                        StaticCoroutine.Start(HandleBlinkTeleport(Owner, m_cachedBlinkPosition, lockedDodgeRollDirection));
                    }
                }
            }
        }
        private static IEnumerator HandleBlinkTeleport(PlayerController Owner, Vector2 targetPoint, Vector2 targetDirection)
        {

            targetPoint = (targetPoint - new Vector2(0.40f, 0.125f));

            Owner.PlayEffectOnActor(EasyVFXDatabase.BloodiedScarfPoofVFX, Vector3.zero, false, true, false);

            AkSoundEngine.PostEvent("Play_ENM_wizardred_vanish_01", Owner.gameObject);
            List<AIActor> m_rollDamagedEnemies = OMITBReflectionHelpers.ReflectGetField<List<AIActor>>(typeof(PlayerController), "m_rollDamagedEnemies", Owner);
            if (m_rollDamagedEnemies != null)
            {
                m_rollDamagedEnemies.Clear();
                FieldInfo m_rollDamagedEnemiesClear = typeof(PlayerController).GetField("m_rollDamagedEnemies", BindingFlags.Instance | BindingFlags.NonPublic);
                m_rollDamagedEnemiesClear.SetValue(Owner, m_rollDamagedEnemies);
            }

            if (Owner.knockbackDoer) { Owner.knockbackDoer.ClearContinuousKnockbacks(); }
            Owner.IsEthereal = true;
            Owner.IsVisible = false;
            float RecoverySpeed = GameManager.Instance.MainCameraController.OverrideRecoverySpeed;
            bool IsLerping = GameManager.Instance.MainCameraController.IsLerping;
            yield return new WaitForSeconds(0.1f);
            GameManager.Instance.MainCameraController.OverrideRecoverySpeed = 80f;
            GameManager.Instance.MainCameraController.IsLerping = true;
            if (Owner.IsPrimaryPlayer)
            {
                GameManager.Instance.MainCameraController.UseOverridePlayerOnePosition = true;
                GameManager.Instance.MainCameraController.OverridePlayerOnePosition = targetPoint;
                yield return new WaitForSeconds(0.12f);
                Owner.specRigidbody.Velocity = Vector2.zero;
                Owner.specRigidbody.Position = new Position(targetPoint);
                GameManager.Instance.MainCameraController.UseOverridePlayerOnePosition = false;
            }
            else
            {
                GameManager.Instance.MainCameraController.UseOverridePlayerTwoPosition = true;
                GameManager.Instance.MainCameraController.OverridePlayerTwoPosition = targetPoint;
                yield return new WaitForSeconds(0.12f);
                Owner.specRigidbody.Velocity = Vector2.zero;
                Owner.specRigidbody.Position = new Position(targetPoint);
                GameManager.Instance.MainCameraController.UseOverridePlayerTwoPosition = false;
            }
            GameManager.Instance.MainCameraController.OverrideRecoverySpeed = RecoverySpeed;
            GameManager.Instance.MainCameraController.IsLerping = IsLerping;
            Owner.IsEthereal = false;
            Owner.IsVisible = true;
            Owner.PlayEffectOnActor(EasyVFXDatabase.BloodiedScarfPoofVFX, Vector3.zero, false, true, false);
            //m_CurrentlyBlinking = false;
            if (Owner.CurrentFireMeterValue <= 0f) { yield break; }
            Owner.CurrentFireMeterValue = Mathf.Max(0f, Owner.CurrentFireMeterValue -= 0.5f);
            if (Owner.CurrentFireMeterValue == 0f)
            {
                Owner.IsOnFire = false;
                yield break;
            }
            yield break;
        }
    }
}







