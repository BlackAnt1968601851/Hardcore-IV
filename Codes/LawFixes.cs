using IVSDKDotNet;
using System;
using static IVSDKDotNet.Native.Natives;
using System.Collections.Generic;
using System.Numerics;
using CCL.GTAIV;
using IVSDKDotNet.Enums;

namespace HardCore
{
    internal class LawFixes
    {
        private static List<IVPed> PoliceListt = new List<IVPed>();
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
            //GET_MAX_WANTED_LEVEL(out uint maxwl);
            //if (maxwl < 6)
            //    SET_MAX_WANTED_LEVEL(6);
            //SET_WANTED_MULTIPLIER(2f);

            //log.Info($"Initiating Ticks for LawFixes in [LawFixes.cs].");
            LawPeds();
            LawPedsBehaviour();
            //log.Info($"LawPeds() is in Action.");
        }

        public static IVPed[] GetAllPeds()
        {
            //Letting the same as SHDN
            //Memory Access- grabbing total spawned peds.
            IVPool PedPool = IVPools.GetPedPool();
            //List to store them
            List<IVPed> PedList = new List<IVPed>();
            //Loop we do so that we can get total amount of peds spawned and their count
            for (int i = 0; i < PedPool.Count; i++)
            {
                //game ptr value check
                UIntPtr ptr = PedPool.Get(i);
                if (ptr != UIntPtr.Zero)
                {
                    //getting handle from pool ptr
                    int pedHandle = (int)PedPool.GetIndex(ptr);
                    //changing the handle (uint) to IVPed
                    IVPed getped = NativeWorld.GetPedInstaceFromHandle(pedHandle);
                    if (DOES_CHAR_EXIST(IVPedExtensions.GetHandle(getped)))
                    {
                        GET_CHAR_MODEL(IVPedExtensions.GetHandle(getped), out int model);
                        if (model != 0)
                        {
                            //adding those IVPed Values.
                            PedList.Add(getped);
                        }
                    }
                }
            }
            //return them as array for useful work.
            return PedList.ToArray();
        }



        public static void SnipeTeam()
        {
            //probably #2 idea stuff-

        }

        public static void Guarding()
        {
            //may be make police and sniper team to guard the govt properties and the early game bridges?? #3 idea stuff?
        }

        public static string[] ArmouredPedsList = { "m_y_swat", "m_m_armoured" };

        //this is uint way aka the same shit as shdn does for Ped.GetAllPeds(). but idk if it works ****
        public static uint[] GetAllPedFromPools()
        {
            //Letting the same as SHDN
            //Memory Access
            IVPool PedPool = IVPools.GetPedPool();
            //List
            List<uint> PedList = new List<uint>();
            //Loop
            for (int i = 0; i < PedPool.Count; i++)
            {
                UIntPtr ptr = PedPool.Get(i);
                if (ptr != UIntPtr.Zero)
                {
                    PedList.Add(PedPool.GetIndex(ptr));
                }
            }
            return PedList.ToArray();
        }

        //this is uint way aka the same shit as shdn does for Ped.GetAllPeds(). but idk if it works ****
        public static uint[] GetAllPedFromPools_Models()
        {
            //Letting the same as SHDN
            //Memory Access
            IVPool PedPool = IVPools.GetPedPool();
            //List
            List<uint> PedList = new List<uint>();
            //Loop
            for (int i = 0; i < PedPool.Count; i++)
            {
                UIntPtr ptr = PedPool.Get(i);
                if (ptr != UIntPtr.Zero)
                {
                    int pedHandle = (int)PedPool.GetIndex(ptr);
                    if (DOES_CHAR_EXIST(pedHandle))
                    {
                        GET_CHAR_MODEL(pedHandle, out int model);
                        if (model != 0)
                        {
                            PedList.Add((uint)pedHandle);
                        }
                    }
                }
            }
            return PedList.ToArray();
        }

        //Makes NOoSE, Armoured security, harder to Deal with.
        //#1 idea i think so...
        public static void LawPedsBehaviour()
        {
            IVPool pedPool = IVPools.GetPedPool();
            for (int i = 0; i < pedPool.Count; i++)
            {
                UIntPtr ptr = pedPool.Get(i);

                if (ptr != UIntPtr.Zero)
                {
                    // Ignore player ped
                    if (ptr == IVPlayerInfo.FindThePlayerPed())
                        continue;

                    int playerID = CONVERT_INT_TO_PLAYERINDEX(GET_PLAYER_ID());
                    GET_PLAYER_CHAR(playerID, out int playerPed);

                    int pedHandle = (int)pedPool.GetIndex(ptr);

                    IVPool vehPool = IVPools.GetVehiclePool();
                    for (int j = 0; j < vehPool.Count; j++)
                    {
                        UIntPtr ptr2 = vehPool.Get(j);
                        if (ptr2 != UIntPtr.Zero)
                        {
                            int vehhandle = (int)vehPool.GetIndex(ptr2);

                            STORE_WANTED_LEVEL(playerID, out uint wl);
                            GET_CHAR_MODEL(pedHandle, out uint pedModel);

                            //if not less than 2 then we force the game to not make peds drive by....
                            if (wl! < 2)
                                SET_CHAR_WILL_DO_DRIVEBYS(pedHandle, false);
                            //if greater than 2 then boom blast aiugwhaiguawgh87ewrg
                            else
                                SET_CHAR_WILL_DO_DRIVEBYS(pedHandle, true);

                            if (pedModel == RAGE.AtStringHash(ArmouredPedsList[0]) || pedModel == RAGE.AtStringHash(ArmouredPedsList[1]))
                            {
                                GET_CHAR_ARMOUR(pedHandle, out uint PedArmor);
                                IVPed ArmedPed = NativeWorld.GetPedInstaceFromHandle(pedHandle);

                                // Getting the index of the pedestrian's prop (if any)
                                GET_CHAR_PROP_INDEX(pedHandle, 0, out int pedPropIndex);

                                // Checking if the pedestrian is dead or alive
                                if (!IS_CHAR_DEAD(pedHandle))
                                {
                                    // If the pedestrian has no prop
                                    if (pedPropIndex == -1)
                                    {
                                        // Allowing headshots
                                        ArmedPed.PedFlags.NoHeadshots = true;
                                    }
                                    else
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

                            GIVE_WEAPON_TO_CHAR(pedHandle, (int)eWeaponType.WEAPON_KNIFE, 1, false);
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
                                        GIVE_WEAPON_TO_CHAR(pedHandle, (int)eWeaponType.WEAPON_M4, 300, true);
                                        GIVE_WEAPON_TO_CHAR(pedHandle, (int)eWeaponType.WEAPON_PISTOL, 50, false);
                                    }
                                    //SMG and Handgun
                                    else if (toss == 1)
                                    {
                                        GIVE_WEAPON_TO_CHAR(pedHandle, (int)eWeaponType.WEAPON_MP5, 300, true);
                                        GIVE_WEAPON_TO_CHAR(pedHandle, (int)eWeaponType.WEAPON_DEAGLE, 50, false);
                                    }
                                    //Shotgun and Handgun
                                    else if (toss == 2)
                                    {
                                        GIVE_WEAPON_TO_CHAR(pedHandle, (int)eWeaponType.WEAPON_SHOTGUN, 150, true);
                                        GIVE_WEAPON_TO_CHAR(pedHandle, (int)eWeaponType.WEAPON_PISTOL, 50, false);
                                    }
                                    //Sniper and Handgun
                                    else if (toss == 3)
                                    {
                                        GIVE_WEAPON_TO_CHAR(pedHandle, (int)eWeaponType.WEAPON_SNIPERRIFLE, 200, true);
                                        GIVE_WEAPON_TO_CHAR(pedHandle, (int)eWeaponType.WEAPON_DEAGLE, 50, false);
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
                                GIVE_WEAPON_TO_CHAR(pedHandle, (int)eWeaponType.WEAPON_M4, 120, true);
                                GIVE_WEAPON_TO_CHAR(pedHandle, (int)eWeaponType.WEAPON_PISTOL, 40, false);
                            }
                            //SMG and Handgun
                            else if (toss == 1)
                            {
                                GIVE_WEAPON_TO_CHAR(pedHandle, (int)eWeaponType.WEAPON_MP5, 120, true);
                                GIVE_WEAPON_TO_CHAR(pedHandle, (int)eWeaponType.WEAPON_DEAGLE, 40, false);
                            }
                            //Shotgun and Handgun
                            else if (toss == 2)
                            {
                                GIVE_WEAPON_TO_CHAR(pedHandle, (int)eWeaponType.WEAPON_MP5, 60, true);
                                GIVE_WEAPON_TO_CHAR(pedHandle, (int)eWeaponType.WEAPON_PISTOL, 80, false);
                            }
                            //weapon
                            PoliceList.Add(pedHandle);
                        }
                    }
                    else
                    {
                        if (!PoliceList.Contains(pedHandle))
                        {
                            PoliceList.Add(pedHandle);
                        }
                    }
                }
            }
        }
    }
}