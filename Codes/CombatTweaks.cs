using CCL.GTAIV;
using CCL.GTAIV.Extensions;
using IVSDKDotNet;
using static IVSDKDotNet.Native.Natives;
using System;
using System.Collections.Generic;
using HardCore.Codes;
using IVSDKDotNet.Enums;

namespace HardCore
{
    internal class CombatTweaks
    {
        private static List<IVPed> PoliceList = new List<IVPed>();
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
            for (int i =0;i<PoliceList.Count;i++)
            {
                if (!DOES_CHAR_EXIST(PoliceList[i].GetHandle()) || IS_CHAR_DEAD(PoliceList[i].GetHandle()))
                {
                    PoliceList.RemoveAt(i);    
                }
            }
        }

       

        public static void Tick()
        {
            LawPeds();
            //LawPedsBehaviour();
        }

        public static string[] ArmouredPedsList = { "m_m_armoured" };
        public static string[] SwatAndFbiPedsList = { "m_y_swat", "m_m_fbi" };

        public static void LawPedsBehaviour()
        {
            IVPed[] peds = Helpers.GetAllPeds();
            foreach (IVPed ped in peds)
            {
                // Check if ped is SWAT or armored guard
                if (ped.ModelIndex == RAGE.AtStringHash(ArmouredPedsList[0]) || ped.ModelIndex == RAGE.AtStringHash(ArmouredPedsList[1]))
                {
                    // Apply buffs specific to SWAT and armored guards
                    SET_CHAR_WILL_DO_DRIVEBYS(ped.GetHandle(), true);
                    IVPed ArmedPed = ped;

                    // Getting the index of the pedestrian's prop (if any)
                    GET_CHAR_PROP_INDEX(ped.GetHandle(), 0, out int pedPropIndex);

                    // Checking if the pedestrian is dead or alive
                    if (!IS_CHAR_DEAD(ped.GetHandle()))
                    {
                        ArmedPed.PedFlags.NoHeadshots = true;
                        if (pedPropIndex == -1)
                        {
                            // Allowing headshots if no prop (helmet) is worn
                            ArmedPed.PedFlags.NoHeadshots = false;
                        }
                    }
                }
                else
                {
                    // Optionally handle other peds if needed
                }
            }
        }

        public static void LawPeds()
        {
            AutoRemoveFromList();

            IVPed[] peds = Helpers.GetAllPeds();
            foreach (IVPed ped in peds)
            {
                if (ped != Helpers.GamePlayerPed && !IS_CHAR_DEAD(ped.GetHandle()))
                {
                    SET_CHAR_ACCURACY(ped.GetHandle(), 100);
                    SET_CHAR_SHOOT_RATE(ped.GetHandle(), 300);
                    SET_CHAR_WILL_USE_CARS_IN_COMBAT(ped.GetHandle(), true);
                    SET_PED_PATH_WILL_AVOID_DYNAMIC_OBJECTS(ped.GetHandle(), true);
                    SET_HOT_WEAPON_SWAP(true);
                    SET_PED_PATH_MAY_USE_CLIMBOVERS(ped.GetHandle(), true);
                    SET_PED_PATH_MAY_USE_LADDERS(ped.GetHandle(), true);
                    SET_PED_DENSITY_MULTIPLIER(2.0f);

                    if (Array.Exists(ArmouredPedsList, model => ped.ModelIndex == RAGE.AtStringHash(model)))
                    {
                        if (!PoliceList.Contains(ped))
                        {
                            BuffArmouredPed(ped);
                        }
                    }
                    else if (Array.Exists(SwatAndFbiPedsList, model => ped.ModelIndex == RAGE.AtStringHash(model)))
                    {
                        if (!PoliceList.Contains(ped))
                        {
                            BuffSwatAndFbiPed(ped);
                        }
                    }
                    else if (!Array.Exists(SwatAndFbiPedsList, model => ped.ModelIndex == RAGE.AtStringHash(model)) && !Array.Exists(ArmouredPedsList, model => ped.ModelIndex == RAGE.AtStringHash(model)))
                    {
                        if (!PoliceList.Contains(ped))
                        {
                            SET_CHAR_MAX_HEALTH(ped.GetHandle(), 200);
                            SET_CHAR_HEALTH(ped.GetHandle(), 200);
                            PoliceList.Add(ped);
                        }
                    }
                }
            }
        }

        private static void BuffArmouredPed(IVPed ped)
        {
            ADD_ARMOUR_TO_CHAR(ped.GetHandle(), 200);
            SET_CHAR_MAX_HEALTH(ped.GetHandle(), 200);
            SET_CHAR_HEALTH(ped.GetHandle(), 200);

            int primaryWeapon = Main.GenerateRandomNumber(0, 3); // Random number between 0 and 2
            int secondaryWeapon = Main.GenerateRandomNumber(0, 2); // Random number between 0 and 1

            REMOVE_ALL_CHAR_WEAPONS(ped.GetHandle());

            switch (primaryWeapon)
            {
                case 0:
                    GIVE_WEAPON_TO_CHAR(ped.GetHandle(), (int)eWeaponType.WEAPON_MP5, 300, true);
                    break;
                case 1:
                    GIVE_WEAPON_TO_CHAR(ped.GetHandle(), (int)eWeaponType.WEAPON_M4, 300, true);
                    break;
                case 2:
                    GIVE_WEAPON_TO_CHAR(ped.GetHandle(), (int)eWeaponType.WEAPON_AK47, 300, true);
                    break;
            }

            switch (secondaryWeapon)
            {
                case 0:
                    GIVE_WEAPON_TO_CHAR(ped.GetHandle(), (int)eWeaponType.WEAPON_DEAGLE, 50, false);
                    break;
                case 1:
                    GIVE_WEAPON_TO_CHAR(ped.GetHandle(), (int)eWeaponType.WEAPON_PISTOL, 50, false);
                    break;
            }

            PoliceList.Add(ped);
        }

        private static void BuffSwatAndFbiPed(IVPed ped)
        {
            ADD_ARMOUR_TO_CHAR(ped.GetHandle(), 200);
            SET_CHAR_MAX_HEALTH(ped.GetHandle(), 200);
            SET_CHAR_HEALTH(ped.GetHandle(), 200);

            int primaryWeapon = Main.GenerateRandomNumber(0, 3); // Random number between 0 and 2
            int secondaryWeapon = Main.GenerateRandomNumber(0, 2); // Random number between 0 and 1

            REMOVE_ALL_CHAR_WEAPONS(ped.GetHandle());

            switch (primaryWeapon)
            {
                case 0:
                    GIVE_WEAPON_TO_CHAR(ped.GetHandle(), (int)eWeaponType.WEAPON_M4, 300, true);
                    break;
                case 1:
                    GIVE_WEAPON_TO_CHAR(ped.GetHandle(), (int)eWeaponType.WEAPON_MP5, 300, true);
                    break;
                case 2:
                    GIVE_WEAPON_TO_CHAR(ped.GetHandle(), (int)eWeaponType.WEAPON_SHOTGUN, 150, true);
                    break;
            }

            switch (secondaryWeapon)
            {
                case 0:
                    GIVE_WEAPON_TO_CHAR(ped.GetHandle(), (int)eWeaponType.WEAPON_DEAGLE, 50, false);
                    break;
                case 1:
                    GIVE_WEAPON_TO_CHAR(ped.GetHandle(), (int)eWeaponType.WEAPON_PISTOL, 50, false);
                    break;
            }

            PoliceList.Add(ped);
        }
    }
}
