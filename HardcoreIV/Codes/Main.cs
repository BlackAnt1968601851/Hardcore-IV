using CCL.GTAIV;
using IVSDKDotNet;
using static IVSDKDotNet.Native.Natives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HardCore.Codes;
using IVSDKDotNet.Enums;
using System.Numerics;

namespace HardCore
{
    public class Main : Script
    {

       
        private int Intervals;
        private int CheckTimer = 15000;

        public Main()
        {
            Tick += Main_Tick;
            Initialized += Main_Initialized;
            //we arent using anything here so its uselsess shit
            //Drawing += Main_Drawing;
            //KeyDown += Main_KeyDown;
            //ProcessAutomobile += Main_ProcessAutomobile;
            //ProcessCamera += Main_ProcessCamera;
            //IngameStartup += Main_IngameStartup;
            //WaitTick += Main_WaitTick;
            //GameLoadPriority += Main_GameLoadPriority;
        }

        public static Logger log = new ();

        //Reading of INIs here
        private void Main_Initialized(object sender, EventArgs e)
        {
            /* update, add the inis here */
            
        }

        public static bool HardOne;
        public static bool HardTwo;
        public static bool HardThree;

        //Check for Game Timers.
        public static int GameTime
        {
            get
            {
                GET_GAME_TIMER(out uint pTimer);
                return (int)pTimer;
            }
        }

        // Runs every frame when in-game
        private void Main_Tick(object sender, EventArgs e)
        {
            try
            {
                // Call untimed tasks here:
                CombatTweaks.Tick();
                PlayerCoreStuff.Tick();
                SomeFixes.Tick();
                UnlockIslands();

                // Call timed tasks after a specific interval:
                if (GameTime > Intervals + CheckTimer)
                {
                    Intervals = GameTime;

                    // Perform timed tasks here:
                    int moneyAmount = Main.GenerateRandomNumber(1, 24950);
                    SET_MONEY_CARRIED_BY_ALL_NEW_PEDS(moneyAmount);
                    DispatchCops();

                    // Log the timed tasks
                    log.Info($"Performed timed tasks successfully: SET_MONEY_CARRIED_BY_ALL_NEW_PEDS({moneyAmount}), DispatchCops()");
                }
            }
            catch (Exception ex)
            {
                // Log specific details of the exception
                log.Fatal($"Error occurred in Script [Main.cs]: {ex.GetType().FullName}, Message: {ex.Message}, Stack Trace: {ex.StackTrace}");

                // Optionally, you can log Inner Exceptions if present
                if (ex.InnerException != null)
                {
                    log.Fatal($"Inner Exception: {ex.InnerException.GetType().FullName}, Message: {ex.InnerException.Message}, Stack Trace: {ex.InnerException.StackTrace}");
                }
            }
        }

        private void UnlockIslands()
        {
            var stat= GET_INT_STAT((uint)eIntStatistic.STAT_ISLANDS_UNLOCKED);
            log.Info($"STAT_ISLAND_UNLOCKED = {stat}");
            if (stat <= 2)
                SET_INT_STAT((uint)eIntStatistic.STAT_ISLANDS_UNLOCKED, 4);
        }

        public static Random rnd = new Random();

        public static int GenerateRandomNumber(int min, int max)
        {
            return rnd.Next(min, max + 1);
        }

        public static string[] ToArray(string input)
        {
            string[] array = input.Split(',');
            for (int i = 0; i < array.Length; i++)
            {
                array[i] = string.Join("", array[i].Split((string[])null, StringSplitOptions.RemoveEmptyEntries));
            }
            return array;
        }

        private static void DispatchCops()
        {
            Vector3 pos = Helpers.GamePlayerPed.Matrix.Pos;
            var pos2 = NativeWorld.GetPositionOnStreet(pos, 5);
            var car = NativeWorld.SpawnVehicle("pstockade", pos.Around(70), out int handlecar, true, false);
            var ped = NativeWorld.SpawnPed("m_y_swat", pos.Around(90), out int pedhandle, true, false);
            var ped2 = NativeWorld.SpawnPed("m_y_swat", pos.Around(90), out int pedhandle2, true, false);
            var ped3= NativeWorld.SpawnPed("m_y_swat", pos.Around(90), out int pedhandle3, true, false);
 var ped4= NativeWorld.SpawnPed("m_y_swat", pos.Around(90), out int pedhandle4, true, false);

            int seat = (-2 + 1);
            ped.GetTaskController().WarpIntoVehicle(car, (uint)seat);
            ped2.GetTaskController().WarpIntoVehicle(car, (uint)0);
            ped3.GetTaskController().WarpIntoVehicle(car, (uint)1);
            ped4.GetTaskController().WarpIntoVehicle(car, (uint)2);

            Main.log.Debug("spawn success 4 swat at pos ");
            GIVE_WEAPON_TO_CHAR(ped3.GetHandle(), (int)eWeaponType.WEAPON_SNIPERRIFLE, 200, false);
            
            SET_CAR_COORDINATES(car.GetHandle(), pos2);
            car.PlaceOnGroundProperly();
            SET_CAR_FORWARD_SPEED(car.GetHandle(), 10);
            car.MarkAsNoLongerNeeded();
            ped.MarkAsNoLongerNeeded();

ped2.MarkAsNoLongerNeeded();
ped3.MarkAsNoLongerNeeded();
ped4.MarkAsNoLongerNeeded();

          //  if(IVGame.CurrentEpisode!=(uint)Episode.TBoGT)
          //   IVGame.CurrentEpisode = (uint)Episode.TBoGT;
            

        }
    } 
}
