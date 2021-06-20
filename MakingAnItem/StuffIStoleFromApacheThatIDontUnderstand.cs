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
        public static bool CanBlinkToPoint(PlayerController Owner, Vector2 point)
        {
            bool flag = Owner.IsValidPlayerPosition(point);
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
        public static bool PositionIsInBounds(PlayerController Owner, Vector2 point)
        {
            bool flag = true;
            if (Owner.CurrentRoom != null)
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
        private static Vector2 lockedDodgeRollDirection;
        public static BlinkPassiveItem m_BlinkPassive = PickupObjectDatabase.GetById(436).GetComponent<BlinkPassiveItem>();
        public GameObject BlinkpoofVfx = m_BlinkPassive.BlinkpoofVfx;
        public static void StartTeleport(PlayerController user, Vector2 newPosition)
        {
            user.healthHaver.TriggerInvulnerabilityPeriod(0.001f);
            user.DidUnstealthyAction();
            Vector2 clampedPosition = BraveMathCollege.ClampToBounds(newPosition, GameManager.Instance.MainCameraController.MinVisiblePoint, GameManager.Instance.MainCameraController.MaxVisiblePoint);
            BlinkToPoint(user, clampedPosition);
        }
        private static void BlinkToPoint(PlayerController Owner, Vector2 targetPoint)
        {

            lockedDodgeRollDirection = (targetPoint - Owner.specRigidbody.UnitCenter).normalized;

            Vector2 playerPos = Owner.transform.position;

            int x0 = (int)targetPoint.x, y0 = (int)targetPoint.y, x1 = (int)playerPos.x, y1 = (int)playerPos.y;
            int dx = Math.Abs(x1 - x0), sx = x0 < x1 ? 1 : -1;
            int dy = -Math.Abs(y1 - y0), sy = y0 < y1 ? 1 : -1;
            int err = dx + dy, e2; /* error value e_xy */
            int maxiterations = 600;
            while (maxiterations > 0)
            {  /* loop */

                if (x0 == x1 && y0 == y1) break;
                if (CanBlinkToPoint(new Vector2(x0, y0), Owner))
                {
                    StaticCoroutine.Start(HandleBlinkTeleport(Owner, new Vector2(x0, y0), lockedDodgeRollDirection));
                    return;
                }
                e2 = 2 * err;
                if (e2 >= dy) { err += dy; x0 += sx; } /* e_xy+e_x > 0 */
                if (e2 <= dx) { err += dx; y0 += sy; } /* e_xy+e_y < 0 */

                maxiterations--;
            }
        }
        static bool CanBlinkToPoint(Vector2 point, PlayerController owner)
        {
            RoomHandler CurrentRoom = owner.CurrentRoom;
            bool flag = owner.IsValidPlayerPosition(point);
            if (flag && CurrentRoom != null)
            {
                CellData cellData = GameManager.Instance.Dungeon.data[point.ToIntVector2(VectorConversions.Floor)];
                if (cellData == null)
                {
                    return false;
                }
                RoomHandler nearestRoom = cellData.nearestRoom;
                if (cellData.type != CellType.FLOOR)
                {
                    flag = false;
                }
                if (CurrentRoom.IsSealed && nearestRoom != CurrentRoom)
                {
                    flag = false;
                }
                if (CurrentRoom.IsSealed && cellData.isExitCell)
                {
                    flag = false;
                }
                if (nearestRoom.visibility == RoomHandler.VisibilityStatus.OBSCURED || nearestRoom.visibility == RoomHandler.VisibilityStatus.REOBSCURED)
                {
                    flag = false;
                }
            }
            if (CurrentRoom == null)
            {
                flag = false;
            }
            return flag;
        }
        private static IEnumerator HandleBlinkTeleport(PlayerController Owner, Vector2 targetPoint, Vector2 targetDirection)
        {

            //targetPoint = (targetPoint - new Vector2(0.30f, 0.125f));

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
            // yield return null;
            //CorrectForWalls(Owner);
            yield break;
        }
        public static void CorrectForWalls(PlayerController portal)
        {
            bool flag = PhysicsEngine.Instance.OverlapCast(portal.specRigidbody, null, true, false, null, null, false, null, null, new SpeculativeRigidbody[0]);
            if (flag)
            {
                Vector2 vector = portal.transform.position.XY();
                IntVector2[] cardinalsAndOrdinals = IntVector2.CardinalsAndOrdinals;
                int num = 0;
                int num2 = 1;
                for (; ; )
                {
                    for (int i = 0; i < cardinalsAndOrdinals.Length; i++)
                    {
                        //portal.transform.position = vector + PhysicsEngine.PixelToUnit(cardinalsAndOrdinals[i] * num2);
                        portal.specRigidbody.Position = new Position(vector + PhysicsEngine.PixelToUnit(cardinalsAndOrdinals[i] * num2));
                        portal.specRigidbody.Reinitialize();
                        if (!PhysicsEngine.Instance.OverlapCast(portal.specRigidbody, null, true, false, null, null, false, null, null, new SpeculativeRigidbody[0]))
                        {
                            return;
                        }
                    }
                    num2++;
                    num++;
                    if (num > 200)
                    {
                        goto Block_4;
                    }
                }
                return;
            Block_4:
                Debug.LogError("FREEZE AVERTED!  TELL RUBEL!  (you're welcome) 147");
                return;
            }
        }
    }
}







