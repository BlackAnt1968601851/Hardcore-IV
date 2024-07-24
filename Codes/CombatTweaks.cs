using CCL.GTAIV;
using CCL.GTAIV.Extensions;
using IVSDKDotNet;
using static IVSDKDotNet.Native.Natives;
using System;
using System.Collections.Generic;
using HardCore.Codes;
using IVSDKDotNet.Enums;
using System.Numerics;
using System.Reflection;
using System.Drawing;
using IVSDKDotNet.Native;

namespace HardCore
{
    internal class CombatTweaks
    {
        private static List<int> PoliceList = new List<int>();
        private static Logger log = Main.log;

        public static void Init(SettingsFile settings)
        {
            log.Info("Initialization of CombatTweaks with settings.");
        }

        public static void LoadFiles()
        {
            IVCDStream.AddImage("paths", 1, -1);
            log.Info("Loading files completed.");
        }

        private static void AutoRemoveFromList()
        {
            for (int i = 0; i < PoliceList.Count; i++)
            {
                if (!DOES_CHAR_EXIST(PoliceList[i]) || IS_CHAR_DEAD(PoliceList[i]))
                {
                    PoliceList.RemoveAt(i);
                    i--; // Adjust index after removal
                }
            }
        }

        public static void Tick()
        {
            LawPeds();
            ArmouredPeds();
            AutoRemoveFromList();
        }

        public static string[] ArmouredPedsList = { "m_m_armoured" };
        public static string[] SwatAndFbiPedsList = { "m_y_swat", "m_m_fbi" };

		
		/********************************************
		* This Code Makes Armoured Peds- M_M_ARMOURED, and M_Y_SWAT
		* to prevent headshots if they got helmet equipped.
		* If they lost the helmet when fallen down or other means, they'll get shot on head and dead.
		********************************************/
        public static void ArmouredPeds()
        {
            IVPool PedPool = IVPools.GetPedPool();

            // Loop through the ped pool to get all peds
            for (int i = 0; i < PedPool.Count; i++)
            {
                // Get the ped pointer from the pool
                UIntPtr ptr = PedPool.Get(i);

                if (ptr == UIntPtr.Zero)
                    continue;

                int handle = (int)PedPool.GetIndex(ptr);

                if (handle == Helpers.GamePlayerPed.GetHandle())
                    continue;

                if (IS_CHAR_DEAD(handle))
                    continue;

                GET_CHAR_MODEL(handle, out int model);

                if (model != 0)
                {
                    GET_CHAR_PROP_INDEX(handle, 0, out int pedPropIndex);

                    if (!IS_CHAR_DEAD(handle))
                    {
                        if (
                            model == RAGE.AtStringHash(ArmouredPedsList[0]) ||
                        model == RAGE.AtStringHash(SwatAndFbiPedsList[0]) //||
                        //model == RAGE.AtStringHash(SwatAndFbiPedsList[1])
                        )
                        {
                            IVPed gethandle = NativeWorld.GetPedInstaceFromHandle(handle);

                            if (pedPropIndex == -1)
                            {
                                gethandle.PedFlags.NoHeadshots = false;
                            }
                            else
                            {
                                gethandle.PedFlags.NoHeadshots = true;
                            }
                        }
                    }
                }
            }
        }

		/******************************************************************
		* This Code makes different set of peds buffed up depending on the requirment of mods.
		* SWAT, FBI, Gruppe6 has set of weapons usable.
		* And a lot of Buffs like DRIVE_BY and all.
		*******************************************************************/
        public static void LawPeds()
        {
            IVPool PedPool = IVPools.GetPedPool();

            // Loop through the ped pool to get all peds
            for (int i = 0; i < PedPool.Count; i++)
            {
                // Get the ped pointer from the pool
                UIntPtr ptr = PedPool.Get(i);

                if (ptr == UIntPtr.Zero)
                    continue;

                int ped = (int)PedPool.GetIndex(ptr);

                if (ped == Helpers.GamePlayerPed.GetHandle())
                    continue;

                if (IS_CHAR_DEAD(ped))
                    continue;

                GET_CHAR_MODEL(ped, out int model);

                if (model != 0)
                {
                    if (PoliceList.Contains(ped))
                        continue;

                    

                    //Function.Call()
                    if (model == RAGE.AtStringHash(ArmouredPedsList[0]) ||
                        model == RAGE.AtStringHash(SwatAndFbiPedsList[0]) ||
                        model == RAGE.AtStringHash(SwatAndFbiPedsList[1]))
                    {
                        SET_PED_PATH_MAY_DROP_FROM_HEIGHT(ped, true);
                        SET_PED_PATH_MAY_USE_CLIMBOVERS(ped, true);
                        SET_PED_PATH_MAY_USE_LADDERS(ped, true);
                        SET_PED_PATH_WILL_AVOID_DYNAMIC_OBJECTS(ped, true);
                        SET_CHAR_WILL_DO_DRIVEBYS(ped, true);
                        SET_CHAR_WEAPON_SKILL(ped, 5);
                        SET_CHAR_WILL_USE_CARS_IN_COMBAT(ped, true);
                        SET_CHAR_ACCURACY(ped, 100);
                        SET_CHAR_SHOOT_RATE(ped, 300);
                        SET_CHAR_GRAVITY(ped, 2);
                        SET_CHAR_SIGNAL_AFTER_KILL(ped, true);
                        NativeWorld.GetPedInstaceFromHandle(ped).GetTaskController().FightAgainstHatedTargets(100, 100000);

                        BuffPed(ped, model);
                    }
                    else
                    {
                        SET_PED_PATH_MAY_DROP_FROM_HEIGHT(ped, true);
                        SET_PED_PATH_MAY_USE_CLIMBOVERS(ped, true);
                        SET_PED_PATH_MAY_USE_LADDERS(ped, true);
                        SET_PED_PATH_WILL_AVOID_DYNAMIC_OBJECTS(ped, true);
                        SET_CHAR_WILL_DO_DRIVEBYS(ped, true);
                        SET_CHAR_WEAPON_SKILL(ped, 5);
                        SET_CHAR_WILL_USE_CARS_IN_COMBAT(ped, true);
                        SET_CHAR_ACCURACY(ped, 100);
                        SET_CHAR_SHOOT_RATE(ped, 300);
                        SET_CHAR_GRAVITY(ped, 2);
                        SET_CHAR_SIGNAL_AFTER_KILL(ped, true);
                        NativeWorld.GetPedInstaceFromHandle(ped).GetTaskController().FightAgainstHatedTargets(100, 100000);

                        // Buff generic law enforcement ped
                        SET_CHAR_MAX_HEALTH(ped, 200);
                        SET_CHAR_HEALTH(ped, 200);
                        GIVE_WEAPON_TO_CHAR(ped, (int)eWeaponType.WEAPON_MP5, 300, true);

                        PoliceList.Add(ped);
                    }
                }
            }
		}
		
		//Set of Weapons for them.

        private static eWeaponType[] SwatWeaponList = {
            eWeaponType.WEAPON_M4,
            eWeaponType.WEAPON_MP5,
            eWeaponType.WEAPON_MICRO_UZI,
            eWeaponType.WEAPON_SHOTGUN,
            eWeaponType.WEAPON_BARETTA,
            eWeaponType.WEAPON_SNIPERRIFLE
        };

        private static eWeaponType[] FbiWeaponList = {
            eWeaponType.WEAPON_M4,
            eWeaponType.WEAPON_MP5,
            eWeaponType.WEAPON_BARETTA,
            eWeaponType.WEAPON_SHOTGUN
        };

        private static eWeaponType[] ArmouredWeaponList = {
            eWeaponType.WEAPON_AK47,
            eWeaponType.WEAPON_M4,
            eWeaponType.WEAPON_MP5,
            eWeaponType.WEAPON_MICRO_UZI
        };

        private static eWeaponType[] Handguns = {
            eWeaponType.WEAPON_DEAGLE,
            eWeaponType.WEAPON_PISTOL
        };

        private static void BuffPed(int ped, int model)
        {
            ADD_ARMOUR_TO_CHAR(ped, 200);
            SET_CHAR_MAX_HEALTH(ped, 200);
            SET_CHAR_HEALTH(ped, 200);

            BLOCK_PED_WEAPON_SWITCHING(ped, false);

            if (model == RAGE.AtStringHash(SwatAndFbiPedsList[0]))
            {
                // SWAT
                SET_CHAR_PROP_INDEX(ped, 0, (uint)Main.GenerateRandomNumber(0, 2));
                var primaryWeapon = SwatWeaponList[Main.GenerateRandomNumber(0, SwatWeaponList.Length)];
                var secondaryWeapon = Handguns[Main.GenerateRandomNumber(0, Handguns.Length)];
				
                if (!IS_CHAR_IN_ANY_HELI(ped))
					REMOVE_ALL_CHAR_WEAPONS(ped);
                {   GIVE_WEAPON_TO_CHAR(ped, (int)primaryWeapon, 300, true);
                    GIVE_WEAPON_TO_CHAR(ped, (int)secondaryWeapon, 50, false);
                }
            }
            else if (model == RAGE.AtStringHash(SwatAndFbiPedsList[1]))
            {
                // FBI
				REMOVE_ALL_CHAR_WEAPONS(ped);
                SET_CHAR_PROP_INDEX(ped, 0, (uint)Main.GenerateRandomNumber(0, 2));
                var primaryWeapon = FbiWeaponList[Main.GenerateRandomNumber(0, FbiWeaponList.Length)];
                var secondaryWeapon = Handguns[Main.GenerateRandomNumber(0, Handguns.Length)];
                GIVE_WEAPON_TO_CHAR(ped, (int)primaryWeapon, 300, true);
                GIVE_WEAPON_TO_CHAR(ped, (int)secondaryWeapon, 50, false);
            }
            else if (model == RAGE.AtStringHash(ArmouredPedsList[0]))
            {
                // Armoured
				REMOVE_ALL_CHAR_WEAPONS(ped);
                SET_CHAR_PROP_INDEX(ped, 0, (uint)Main.GenerateRandomNumber(0, 1));
                var primaryWeapon = ArmouredWeaponList[Main.GenerateRandomNumber(0, ArmouredWeaponList.Length)];
                var secondaryWeapon = Handguns[Main.GenerateRandomNumber(0, Handguns.Length)];
                GIVE_WEAPON_TO_CHAR(ped, (int)primaryWeapon, 300, true);
                GIVE_WEAPON_TO_CHAR(ped, (int)secondaryWeapon, 50, false);
            }

            PoliceList.Add(ped);
        }
    }
}
