﻿using System;
using System.Numerics;

using IVNatives.AnimationController;
using IVNatives.TaskController;

using IVSDKDotNet;
using IVSDKDotNet.Enums;
using static IVSDKDotNet.Native.Natives;

namespace IVNatives
{
    /// <summary>
    /// Contains extensions for the <see cref="IVPed"/> class.
    /// </summary>
    public static class IVPedExtensions
    {
        #region Methods
        /// <summary>
        /// Adds ammo to the given <paramref name="weapon"/> for this <see cref="IVPed"/>.
        /// </summary>
        /// <param name="ped"></param>
        /// <param name="weapon">The weapon to fill up the ammo.</param>
        /// <param name="amount">The amount of ammo to fill up.</param>
        public static void AddAmmoToChar(this IVPed ped, eWeaponType weapon, uint amount)
        {
            if (ped == null)
                return;

            ADD_AMMO_TO_CHAR(GetHandle(ped), (uint)weapon, amount);
        }

        /// <summary>
        /// Adds armour to this <see cref="IVPed"/>.
        /// </summary>
        /// <param name="ped"></param>
        /// <param name="amount">The amount of armour to fill up.</param>
        public static void AddArmourToChar(this IVPed ped, uint amount)
        {
            if (ped == null)
                return;
            
            ADD_ARMOUR_TO_CHAR(GetHandle(ped), amount);
        }

        /// <summary>
        /// Sets if this <see cref="IVPed"/> can ragdoll.
        /// </summary>
        /// <param name="ped"></param>
        /// <param name="value">Can ragdoll or not.</param>
        public static void PreventRagdoll(this IVPed ped, bool value)
        {
            if (ped == null)
                return;

            UNLOCK_RAGDOLL(GetHandle(ped), !value);
        }

        public static void ApplyForce(this IVPed ped, Vector3 direction, Vector3 rotation)
        {
            if (ped == null)
                return;

            APPLY_FORCE_TO_PED(GetHandle(ped), 3, direction.X, direction.Y, direction.Z, rotation.X, rotation.Y, rotation.Z, 0, 0, 1, 1);
        }
        public static void ApplyForce(this IVPed ped, Vector3 direction)
        {
            if (ped == null)
                return;

            ApplyForce(ped, direction, Vector3.Zero);
        }

        public static void ApplyForceRelative(this IVPed ped, Vector3 direction, Vector3 rotation)
        {
            if (ped == null)
                return;

            APPLY_FORCE_TO_PED(GetHandle(ped), 3, direction.X, direction.Y, direction.Z, rotation.X, rotation.Y, rotation.Z, 0, 1, 1, 1);
        }
        public static void ApplyForceRelative(this IVPed ped, Vector3 direction)
        {
            if (ped == null)
                return;

            ApplyForceRelative(ped, direction, Vector3.Zero);
        }

        public static void SetVelocity(this IVPed ped, Vector3 velocity)
        {
            if (ped == null)
                return;

            SET_CHAR_VELOCITY(GetHandle(ped), velocity.X, velocity.Y, velocity.Z);
        }

        public static void SetSenseRange(this IVPed ped, float range)
        {
            if (ped == null)
                return;

            SET_SENSE_RANGE(GetHandle(ped), range);
        }
        public static void BlockAmbientAnims(this IVPed ped, bool block)
        {
            if (ped == null)
                return;

            BLOCK_CHAR_AMBIENT_ANIMS(GetHandle(ped), block);
        }
        public static void BlockGestureAnims(this IVPed ped, bool block)
        {
            if (ped == null)
                return;

            BLOCK_CHAR_GESTURE_ANIMS(GetHandle(ped), block);
        }
        public static void BlockPermanentEvents(this IVPed ped, bool block)
        {
            if (ped == null)
                return;

            SET_BLOCKING_OF_NON_TEMPORARY_EVENTS(GetHandle(ped), block);
        }
        public static void SetCharWillFlyThroughWindscreen(this IVPed ped, bool set)
        {
            if (ped == null)
                return;

            SET_CHAR_WILL_FLY_THROUGH_WINDSCREEN(GetHandle(ped), set);
        }
        public static void SetCurrentRoom(this IVPed ped, NativeRoom room)
        {
            if (ped == null)
                return;
            if (room == null)
                return;
            
            SET_ROOM_FOR_CHAR_BY_KEY(GetHandle(ped), (uint)room.Room);
        }
        public static void SetAlwaysDiesOnLowHealth(this IVPed ped, bool set)
        {
            if (ped == null)
                return;

            int handle = GetHandle(ped);
            SET_CHAR_WILL_MOVE_WHEN_INJURED(handle, !set);
            SET_PED_DIES_WHEN_INJURED(handle, set);
        }
        public static void SetFireDamageMultiplier(this IVPed ped, float multiplier)
        {
            if (ped == null)
                return;

            SET_CHAR_FIRE_DAMAGE_MULTIPLIER(GetHandle(ped), multiplier);
        }
        public static void SetCharDrownsInWater(this IVPed ped, bool set)
        {
            if (ped == null)
                return;

            SET_CHAR_DROWNS_IN_WATER(GetHandle(ped), set);
        }
        public static void SayAmbientSpeech(this IVPed ped, string phraseID)
        {
            if (ped == null)
                return;

            SAY_AMBIENT_SPEECH(ped.GetHandle(), phraseID, true, true, 0);
        }
        public static void CancelAmbientSpeech(this IVPed ped)
        {
            if (ped == null)
                return;

            CANCEL_CURRENTLY_PLAYING_AMBIENT_SPEECH(ped.GetHandle());
        }

        public static void SetHeading(this IVPed ped, float heading)
        {
            if (ped == null)
                return;

            SET_CHAR_HEADING(GetHandle(ped), heading);
        }
        public static void SetHealth(this IVPed ped, uint health)
        {
            if (ped == null)
                return;

            SET_CHAR_HEALTH(GetHandle(ped), health);
        }

        public static void SetAsMissionChar(this IVPed ped)
        {
            if (ped == null)
                return;

            SET_CHAR_AS_MISSION_CHAR(GetHandle(ped));
        }
        public static void MarkAsNoLongerNeeded(this IVPed ped)
        {
            if (ped == null)
                return;

            MARK_CHAR_AS_NO_LONGER_NEEDED(GetHandle(ped));
        }
        public static void Delete(this IVPed ped)
        {
            if (ped == null)
                return;

            int handle = GetHandle(ped);
            DELETE_CHAR(ref handle);
        }
        #endregion

        #region Functions
        /// <summary>
        /// Gets the handle for this <see cref="IVPed"/>.
        /// <para>Can be used for calling native functions like <see cref="EXPLODE_CHAR_HEAD(int)"/> which requires a ped handle.</para>
        /// </summary>
        /// <param name="ped"></param>
        /// <returns>If successful, the handle of this <see cref="IVPed"/> is returned. Otherwise, 0.</returns>
        public static int GetHandle(this IVPed ped)
        {
            if (ped == null)
                return 0;

            return (int)IVPools.GetPedPool().GetIndex(ped.GetUIntPtr());
        }

        /// <summary>
        /// Gets the <see cref="Rectangle3D"/> (or bounds) of this <see cref="IVPed"/> with their current model.
        /// </summary>
        /// <param name="ped"></param>
        /// <returns>If successful, the <see cref="Rectangle3D"/> of this <see cref="IVPed"/> is returned. Otherwise, <see cref="Rectangle3D.Empty()"/> is returned.</returns>
        public static Rectangle3D GetModelRect3D(this IVPed ped)
        {
            if (ped == null)
                return Rectangle3D.Empty();

            int handle = GetHandle(ped);
            GET_CHAR_MODEL(handle, out uint pModel);

            if (pModel == 0)
                return Rectangle3D.Empty();

            GET_MODEL_DIMENSIONS(pModel, out Vector3 min, out Vector3 max);
            return new Rectangle3D(ped.Matrix.Pos, max);
        }

        /// <summary>
        /// Gets if there are any chars near this <see cref="IVPed"/>.
        /// </summary>
        /// <param name="ped"></param>
        /// <param name="radius">The radius in which to search for.</param>
        /// <returns>True if there are any chars in the area around this <see cref="IVPed"/>. Otherwise, false.</returns>
        public static bool AreAnyCharsNearChar(this IVPed ped, float radius)
        {
            if (ped == null)
                return false;

            return ARE_ANY_CHARS_NEAR_CHAR(GetHandle(ped), radius);
        }

        /// <summary>
        /// Gets if the <see cref="IVPed"/> is sitting in any vehicle.
        /// </summary>
        /// <param name="ped"></param>
        /// <returns>True if this <see cref="IVPed"/> is sitting in any vehicle. Otherwise, false.</returns>
        public static bool IsInVehicle(this IVPed ped)
        {
            if (ped == null)
                return false;

            return IS_CHAR_SITTING_IN_ANY_CAR(GetHandle(ped));
        }

        /// <summary>
        /// Gets the model of this <see cref="IVPed"/>.
        /// </summary>
        /// <param name="ped"></param>
        /// <returns>The model of this <see cref="IVPed"/>.</returns>
        public static uint GetCharModel(this IVPed ped)
        {
            if (ped == null)
                return 0;

            GET_CHAR_MODEL(GetHandle(ped), out uint pModel);
            return pModel;
        }

        /// <summary>
        /// Gets the gender of this <see cref="IVPed"/>.
        /// </summary>
        /// <param name="ped"></param>
        /// <returns>One of the 3 items in the <see cref="PedGender"/> enum.</returns>
        public static PedGender GetCharGender(this IVPed ped)
        {
            if (ped == null)
                return PedGender.Unkown;

            return IS_CHAR_MALE(GetHandle(ped)) ? PedGender.Male : PedGender.Female;
        }

        /// <summary>
        /// Gets the height above ground of this <see cref="IVPed"/>.
        /// </summary>
        /// <param name="ped"></param>
        /// <returns>The height above ground value.</returns>
        public static float GetHeightAboveGround(this IVPed ped)
        {
            if (ped == null)
                return 0f;

            GET_CHAR_HEIGHT_ABOVE_GROUND(GetHandle(ped), out float val);
            return val;
        }

        public static float GetHeading(this IVPed ped)
        {
            if (ped == null)
                return 0f;

            GET_CHAR_HEADING(GetHandle(ped), out float val);
            return val;
        }
        public static uint GetHealth(this IVPed ped)
        {
            if (ped == null)
                return 0;

            GET_CHAR_HEALTH(GetHandle(ped), out uint health);
            return health;
        }

        public static float GetSpeed(this IVPed ped)
        {
            if (ped == null)
                return 0f;
            
            GET_CHAR_SPEED(GetHandle(ped), out float speed);
            return speed;
        }
        public static Vector3 GetVelocity(this IVPed ped)
        {
            if (ped == null)
                return Vector3.Zero;

            GET_CHAR_VELOCITY(GetHandle(ped), out float x, out float y, out float z);
            return new Vector3(x, y, z);
        }

        public static NativeRoom GetCurrentRoom(this IVPed ped)
        {
            if (ped == null)
                return null;

            return NativeRoom.FromPed(ped);
        }

        public static bool Exists(this IVPed ped)
        {
            if (ped == null)
                return false;

            return DOES_CHAR_EXIST(GetHandle(ped));
        }

        public static PedAnimationController GetAnimationController(this IVPed ped)
        {
            if (ped == null)
                return null;

            return new PedAnimationController(ped);
        }
        public static PedTaskController GetTaskController(this IVPed ped)
        {
            if (ped == null)
                return null;

            return new PedTaskController(ped);
        }

        /// <summary>
        /// Attaches a <see cref="NativeBlip"/> to this <see cref="IVPed"/>.
        /// </summary>
        /// <param name="ped"></param>
        /// <returns>If successful, the attached <see cref="NativeBlip"/> is returned. Otherwise, <see langword="null"/>.</returns>
        public static NativeBlip AttachBlip(this IVPed ped)
        {
            if (ped == null)
                return null;

            return NativeBlip.AddBlip(ped);
        }
        #endregion
    }

}
