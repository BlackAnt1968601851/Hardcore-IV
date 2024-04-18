﻿using IVSDKDotNet;
using System;
using static IVSDKDotNet.Native.Natives;

namespace IVNatives
{
    /// <summary>
    /// Contains extensions for the <see cref="IVPlayerInfo"/> class.
    /// </summary>
    public static class IVPlayerInfoExtensions
    {
        #region Methods
        /// <summary>
        /// Sets the money of the player to the given amount.
        /// </summary>
        /// <param name="info"></param>
        /// <param name="amount">The new amount.</param>
        public static void SetMoney(this IVPlayerInfo info, int amount)
        {
            if (info == null)
                return;
            if (amount < 0)
                return;

            STORE_SCORE(info.PlayerId, out uint oldMoney);
            ADD_SCORE(info.PlayerId, (int)(amount - oldMoney));
        }
        /// <summary>
        /// Adds money to the current player money.
        /// </summary>
        /// <param name="info"></param>
        /// <param name="amount">The amount to add.</param>
        public static void AddMoney(this IVPlayerInfo info, int amount)
        {
            if (info == null)
                return;
            if (amount <= 0)
                return;

            ADD_SCORE(info.PlayerId, amount);
        }

        /// <summary>
        /// Removes the money of the player by the given amount.
        /// </summary>
        /// <param name="info"></param>
        /// <param name="amount">The amount to remove.</param>
        public static void RemoveMoney(this IVPlayerInfo info, int amount)
        {
            if (info == null)
                return;
            if (amount < 0)
                return;

            ADD_SCORE(info.PlayerId, -1 * amount);
        }
        public static void SetWantedLevel(this IVPlayerInfo info, int level)
        {
            ALTER_WANTED_LEVEL(info.PlayerId, (uint)level);
        }
        #endregion

        #region Functions
        /// <summary>
        /// Gets the money amount of the player.
        /// </summary>
        /// <param name="info"></param>
        /// <returns>The amount of money the player has.</returns>
        public static int GetMoney(this IVPlayerInfo info)
        {
            if (info == null)
                return 0;

            STORE_SCORE(info.PlayerId, out uint money);
            return (int)money;
        }
        
        public static IVPed Character(this IVPlayerInfo info)
        {
            if (info == null)
                return null;
            
            return IVPed.FromUIntPtr(IVPlayerInfo.FindThePlayerPed());
        }

        public static int GetWantedLevel(this IVPlayerInfo info)
        {
             STORE_WANTED_LEVEL(info.PlayerId, out uint level);
            return (int)level;
        }
       

        #endregion
    }
}
