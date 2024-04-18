using IVSDKDotNet;
using System;
using static IVSDKDotNet.Native.Natives;
using System.Collections.Generic;
using System.Numerics;
using IVNatives;
using IVSDKDotNet.Enums;

namespace HardCore
{
    internal class LawFixes_Three
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
            GET_MAX_WANTED_LEVEL(out uint maxwl);
            if (maxwl < 6)
                SET_MAX_WANTED_LEVEL(6);
            SET_WANTED_MULTIPLIER(2f);

            //log.Info($"Initiating Ticks for LawFixes in [LawFixes.cs].");
            Guarding();
            //log.Info($"LawPeds() is in Action.");
        }

        public static void SnipeTeam()
        {
            //probably #2 idea stuff-

        }

        public static void Guarding()
        {
            //may be make police and sniper team to guard the govt properties and the early game bridges?? #3 idea stuff?
        }
    }
}