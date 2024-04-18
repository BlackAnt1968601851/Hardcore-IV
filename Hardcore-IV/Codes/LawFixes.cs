using IVSDKDotNet;
using System;
using static IVSDKDotNet.Native.Natives;
using System.Collections.Generic;
using System.Numerics;
using IVNatives;
using IVSDKDotNet.Enums;

namespace HardCore
{
    internal class LawFixes
    {
        //private static List<IVPed> PoliceListt = new List<IVPed>();
        private static List<int> PoliceList = new List<int>();
        //private static List<int> 
        private static Logger log = Main.log;

        public static void Init(SettingsFile settings)
        {
            log.Info($"");
        }

        public static void LoadFiles()
        {
            IVCDStream.AddImage("paths", 1, -1);
            log.Info($"");
        }

        private static void AutoRemoveFromList()
        {
            //log.Info($"Removing a lot of stuff from the List [LawFixes.cs] when object not found. Total Amount was: {PoliceList.Count}");
            for (int i = 0; i < PoliceList.Count; i++)
            {
                int pedHandle = PoliceList[i];

                // Check if ped still exists
                if (!DOES_CHAR_EXIST(pedHandle))
                    PoliceList.RemoveAt(i); // Remove ped from list because they dont exists anymore
            }
        }

        public static void Tick()
        {
            //forcing game to have 6 stars unlocked at start of the game.
            GET_MAX_WANTED_LEVEL(out uint maxwl);
            if (maxwl < 6)
                SET_MAX_WANTED_LEVEL(6);
            SET_WANTED_MULTIPLIER(2f);

            //log.Info($"Initiating Ticks for LawFixes in [LawFixes.cs].");
            LawPeds();
            LawPedsBehaviour();
            //log.Info($"LawPeds() is in Action.");
        }

        public static string[] ArmouredPedsList = { "m_y_swat", "m_m_armoured" };
        //this is uint way aka the same shit as shdn does for Ped.GetAllPeds(). but idk if it works ****
       
        //Makes NOoSE, Armoured security, harder to Deal with.
        //#1 idea i think so...
        public static void LawPedsBehaviour()
        {
            IVPed[] peds = NativeWorld.GetAllPeds();
            foreach (IVPed ped in peds)
            {
                // Ignore player ped
                IVPlayerInfo ply = IVPlayerInfo.FromUIntPtr(IVPlayerInfo.FindThePlayerPed());

                if (ped == ply.Character())
                    continue;

                IVVehicle[] vehPool = NativeWorld.GetAllVehicles();
                foreach (IVVehicle veh in vehPool)
                {
                    int wl = ply.GetWantedLevel();
                    //if not less than 2 then we force the game to not make peds drive by....
                    if (wl! < 2)
                        ped.CanDoDrivebys(false);
                    //if greater than 2 then boom blast aiugwhaiguawgh87ewrg
                    else
                        ped.CanDoDrivebys(true);

                    if (ped.ModelIndex == RAGE.AtStringHash(ArmouredPedsList[0]) || ped.ModelIndex == RAGE.AtStringHash(ArmouredPedsList[1]))
                    {
                        GET_CHAR_ARMOUR(ped.GetHandle(), out uint PedArmor);
                        IVPed ArmedPed = NativeWorld.GetPedInstaceFromHandle(ped.GetHandle());

                        // Getting the index of the pedestrian's prop (if any)
                        GET_CHAR_PROP_INDEX(ped.GetHandle(), 0, out int pedPropIndex);

                        // Checking if the pedestrian is dead or alive
                        if (!IS_CHAR_DEAD(ped.GetHandle()))
                        {
                            if (PedArmor > 0)
                            {
                                // Preventing headshots and disabling ragdoll
                                ArmedPed.PedFlags.NoHeadshots = true;
                                ArmedPed.PreventRagdoll(true);

                                // If the pedestrian has no prop
                                if (pedPropIndex == -1)
                                {
                                    // Allowing headshots
                                    ArmedPed.PedFlags.NoHeadshots = false;
                                }
                                //wanted to add another check related to ped in melee combat with other ped. but seems like shit is tough
                                if (IS_CHAR_TOUCHING_VEHICLE(ped.GetHandle(), veh.GetHandle()) || IS_CHAR_ON_FIRE(ped.GetHandle()))
                                {
                                    ArmedPed.PreventRagdoll(false);
                                }
                            }
                            // If armor is 0
                            else if (PedArmor == 0)
                            {
                                // Preventing headshots and enabling ragdoll
                                ArmedPed.PedFlags.NoHeadshots = true;
                                ArmedPed.PreventRagdoll(false);

                                // If the pedestrian has no prop
                                if (pedPropIndex == -1)
                                {
                                    // Allowing headshots
                                    ArmedPed.PedFlags.NoHeadshots = false;
                                }
                            }
                        }
                    }
                }
            }
        }

        //Gives Armour and Health to Peds.
        //Tougher peds.
        public static void LawPeds()
        {
            AutoRemoveFromList();

            // Grab all peds
            IVPool pedPool = IVPools.GetPedPool();
            for (int i = 0; i < pedPool.Count; i++)
            {
                UIntPtr ptr = pedPool.Get(i);

                if (ptr != UIntPtr.Zero)
                {
                    // Ignore player ped
                    if (ptr == IVPlayerInfo.FindThePlayerPed())
                        continue;

                    // Grab player ID & ped
                    IVPed playerPed = IVPed.FromUIntPtr(IVPlayerInfo.FindThePlayerPed());
                    var playerId = IVPedExtensions.GetHandle(playerPed);

                    int pedHandle = (int)pedPool.GetIndex(ptr);

                    GET_CHAR_MODEL(pedHandle, out uint pedModel);

                    // Check if ped is dead
                    if (IS_CHAR_DEAD(pedHandle))
                        continue;

                    //Set Accuracy and ROF for everyone 100%
                    SET_CHAR_ACCURACY(pedHandle, 100);
                    SET_CHAR_SHOOT_RATE(pedHandle, 100);
                    //SET_CHAR_WILL_DO_DRIVEBYS(pedHandle, true);
                    SET_CHAR_WILL_USE_CARS_IN_COMBAT(pedHandle, true);

                    SET_PED_PATH_WILL_AVOID_DYNAMIC_OBJECTS(pedHandle, true);
                    SET_HOT_WEAPON_SWAP(true);

                    // Check if NOoSE gets new weapons
                    if (pedModel == RAGE.AtStringHash("m_y_swat"))
                    {
                        if (!PoliceList.Contains(pedHandle))
                        {

                            ADD_ARMOUR_TO_CHAR(pedHandle, 200);
                            SET_CHAR_MAX_HEALTH(pedHandle, 300);
                            SET_CHAR_HEALTH(pedHandle, 300);

                            GIVE_WEAPON_TO_CHAR(pedHandle, (uint)eWeaponType.WEAPON_KNIFE, 1, false);
                            SET_CHAR_FIRE_DAMAGE_MULTIPLIER(pedHandle, 2.5f);
                            BLOCK_PED_WEAPON_SWITCHING(pedHandle, false);

                            //if (Main.HardThree==true)
                            {
                                int toss = Main.GenerateRandomNumber(0, 3);
                                REMOVE_ALL_CHAR_WEAPONS(pedHandle);
                                if (!IS_CHAR_IN_ANY_HELI(pedHandle))
                                {
                                    //Rifle and Handgun
                                    if (toss == 0)
                                    {
                                        GIVE_WEAPON_TO_CHAR(pedHandle, (uint)eWeaponType.WEAPON_M4, 300, true);
                                        GIVE_WEAPON_TO_CHAR(pedHandle, (uint)eWeaponType.WEAPON_PISTOL, 50, false);
                                    }
                                    //SMG and Handgun
                                    else if (toss == 1)
                                    {
                                        GIVE_WEAPON_TO_CHAR(pedHandle, (uint)eWeaponType.WEAPON_MP5, 300, true);
                                        GIVE_WEAPON_TO_CHAR(pedHandle, (uint)eWeaponType.WEAPON_DEAGLE, 50, false);
                                    }
                                    //Shotgun and Handgun
                                    else if (toss == 2)
                                    {
                                        GIVE_WEAPON_TO_CHAR(pedHandle, (uint)eWeaponType.WEAPON_SHOTGUN, 150, true);
                                        GIVE_WEAPON_TO_CHAR(pedHandle, (uint)eWeaponType.WEAPON_PISTOL, 50, false);
                                    }
                                    //Sniper and Handgun
                                    else if (toss == 3)
                                    {
                                        GIVE_WEAPON_TO_CHAR(pedHandle, (uint)eWeaponType.WEAPON_SNIPERRIFLE, 200, true);
                                        GIVE_WEAPON_TO_CHAR(pedHandle, (uint)eWeaponType.WEAPON_DEAGLE, 50, false);
                                    }
                                }
                            }
                            PoliceList.Add(pedHandle);
                        }
                    }
                    else if (pedModel == RAGE.AtStringHash("m_m_armoured"))
                    {
                        if (!PoliceList.Contains(pedHandle))
                        {
                            ADD_ARMOUR_TO_CHAR(pedHandle, 100);
                            SET_CHAR_MAX_HEALTH(pedHandle, 300);
                            SET_CHAR_HEALTH(pedHandle, 300);
                            SET_CHAR_PROP_INDEX(pedHandle, 0, (uint)Main.GenerateRandomNumber(0, 1));

                            int toss = Main.GenerateRandomNumber(0, 2);
                            //Rifle and Handgun
                            if (toss == 0)
                            {
                                GIVE_WEAPON_TO_CHAR(pedHandle, (uint)eWeaponType.WEAPON_M4, 120, true);
                                GIVE_WEAPON_TO_CHAR(pedHandle, (uint)eWeaponType.WEAPON_PISTOL, 40, false);
                            }
                            //SMG and Handgun
                            else if (toss == 1)
                            {
                                GIVE_WEAPON_TO_CHAR(pedHandle, (uint)eWeaponType.WEAPON_MP5, 120, true);
                                GIVE_WEAPON_TO_CHAR(pedHandle, (uint)eWeaponType.WEAPON_DEAGLE, 40, false);
                            }
                            //Shotgun and Handgun
                            else if (toss == 2)
                            {
                                GIVE_WEAPON_TO_CHAR(pedHandle, (uint)eWeaponType.WEAPON_MP5, 60, true);
                                GIVE_WEAPON_TO_CHAR(pedHandle, (uint)eWeaponType.WEAPON_PISTOL, 80, false);
                            }
                            //weapon
                            PoliceList.Add(pedHandle);
                        }
                    }
                    else
                    {
                        if (!PoliceList.Contains(pedHandle))
                        {
                            SET_CHAR_MAX_HEALTH(pedHandle, 300);
                            SET_CHAR_HEALTH(pedHandle, 300);

                            PoliceList.Add(pedHandle);
                        }
                    }
                }
            }
        }
    }
}