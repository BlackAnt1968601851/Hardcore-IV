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
            for (int i = 0; i < PoliceList.Count; i++)
            {
                if (!DOES_CHAR_EXIST(PoliceList[i].GetHandle()) || IS_CHAR_DEAD(PoliceList[i].GetHandle()))
                {
                    PoliceList.RemoveAt(i);
                    i--; // Adjust index after removal
                }
            }
        }

        public static void Tick()
        {
            LawPeds();
            LawPedsBehaviour();
            AutoRemoveFromList();
        }

        public static string[] ArmouredPedsList = { "m_m_armoured" };
        public static string[] SwatAndFbiPedsList = { "m_y_swat", "m_m_fbi" };

        public static void LawPedsBehaviour()
        {
            IVPed[] peds = Helpers.GetAllPeds(SwatAndFbiPedsList);
            foreach (IVPed ped in peds)
            {
                GET_CHAR_PROP_INDEX(ped.GetHandle(), 0, out int pedPropIndex);

                if (!IS_CHAR_DEAD(ped.GetHandle()))
                {
                    if (pedPropIndex == -1)
                    {
                        ped.PedFlags.NoHeadshots = false;
                    }
                    else
                    {
                        ped.PedFlags.NoHeadshots = true;
                    }
                }
            }
        }

        public static void LawPeds()
        {
            IVPed[] peds = Helpers.GetAllPeds();
            foreach (IVPed ped in peds)
            {
                if (ped != Helpers.GamePlayerPed && !IS_CHAR_DEAD(ped.GetHandle()))
                {
                    if (ped.GetCharModel() == RAGE.AtStringHash(ArmouredPedsList[0]) ||
                        ped.GetCharModel() == RAGE.AtStringHash(SwatAndFbiPedsList[0]) ||
                        ped.GetCharModel() == RAGE.AtStringHash(SwatAndFbiPedsList[1]))
                    {
                        if (!PoliceList.Contains(ped))
                        {
                            BuffPed(ped);
                        }
                    }
                }
            }
        }

        private static void BuffPed(IVPed ped)
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
                    if (ped.GetCharModel() == RAGE.AtStringHash(SwatAndFbiPedsList[0]) || ped.GetCharModel() == RAGE.AtStringHash(SwatAndFbiPedsList[1]))
                    {
                        GIVE_WEAPON_TO_CHAR(ped.GetHandle(), (int)eWeaponType.WEAPON_SHOTGUN, 150, true);
                    }
                    else
                    {
                        GIVE_WEAPON_TO_CHAR(ped.GetHandle(), (int)eWeaponType.WEAPON_AK47, 300, true);
                    }
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
