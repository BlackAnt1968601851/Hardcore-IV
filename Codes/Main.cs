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
		//interval based script chalane ka tarika (kind of workaround to make timed execution of script)
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

        public static Logger log = new();

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
				// this is called based on FramePerTick (FPS based?)
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
		
		//idhar hamne islands ko unlock kardia stats native ka istamal karke
		//here we unlocked island using stat natives
        private void UnlockIslands()
        {
            var stat = GET_INT_STAT((uint)eIntStatistic.STAT_ISLANDS_UNLOCKED);
            //log.Info($"STAT_ISLAND_UNLOCKED = {stat}");
            if (stat <= 2)
                SET_INT_STAT((uint)eIntStatistic.STAT_ISLANDS_UNLOCKED, 3);
        }

        public static Random rnd = new Random();

        public static int GenerateRandomNumber(int min, int max)
        {
            return rnd.Next(min, max + 1);
        }

		//not used but will be used for reading array values
        public static string[] ToArray(string input)
        {
            string[] array = input.Split(',');
            for (int i = 0; i < array.Length; i++)
            {
                array[i] = string.Join("", array[i].Split((string[])null, StringSplitOptions.RemoveEmptyEntries));
            }
            return array;
        }
		
		//position nikal ne  ka tarika
        public static Vector3 GetPositionOnStreet(Vector3 pos, out float heading, float radius = 5f)
        {
            heading = 0;
            for (uint i = 1; i < 40; i++)
            {
                GET_NTH_CLOSEST_CAR_NODE_WITH_HEADING(pos, i, out Vector3 newPos, out var h);
                if (!IS_POINT_OBSCURED_BY_A_MISSION_ENTITY(newPos, new Vector3(radius)))
                    heading = h;
                return newPos;
            }

            return Vector3.Zero;
        }
		
		
        //  if(IVGame.CurrentEpisode!=(uint)Episode.TBoGT)
        //  IVGame.CurrentEpisode = (uint)Episode.TBoGT;
	
		//dispatching of noose//
        private static void DispatchCops()
        {
            Vector3 playerPos = Helpers.GamePlayerPed.Matrix.Pos;

            // Spawn the vehicle around 100 meters away from the player
            Vector3 vehiclePos = playerPos.Around(100);
            var pos2 = GetPositionOnStreet(vehiclePos, out var heading);

            var car = NativeWorld.SpawnVehicle("nstockade", pos2, out int handlecar, true, false);

            // Spawn SWAT members around the player, 100 meters away
            Vector3 pedSpawnPos = playerPos.Around(100);

            var ped = NativeWorld.SpawnPed("m_y_swat", pedSpawnPos, out int pedhandle, true, false);
            ped.GetTaskController().ShootAt(Helpers.GamePlayerPed, ShootMode.Burst);

            var ped2 = NativeWorld.SpawnPed("m_y_swat", pedSpawnPos, out int pedhandle2, true, false);
            var ped3 = NativeWorld.SpawnPed("m_y_swat", pedSpawnPos, out int pedhandle3, true, false);
            var ped4 = NativeWorld.SpawnPed("m_y_swat", pedSpawnPos, out int pedhandle4, true, false);
            
            var seat = -2 + 1;
            // Assigning SWAT members to vehicle seats
            ped.GetTaskController().WarpIntoVehicle(car, (uint)(seat)); // Driver seat
            ped2.GetTaskController().WarpIntoVehicle(car, (uint)0);   // Front passenger seat
            ped3.GetTaskController().WarpIntoVehicle(car, (uint)1);   // Rear left seat
            ped4.GetTaskController().WarpIntoVehicle(car, (uint)2);   // Rear right seat

            Main.log.Debug($"Spawned SWAT vehicle at: {pos2}, player position: {playerPos}. Distance to Position: {Vector3.Distance(pos2, playerPos)}.");

            GIVE_WEAPON_TO_CHAR(ped3.GetHandle(), (int)eWeaponType.WEAPON_SNIPERRIFLE, 200, false);

            car.PlaceOnGroundProperly();
            car.SetHeading(heading);

            SET_CAR_FORWARD_SPEED(car.GetHandle(), 5);

            car.MarkAsNoLongerNeeded();
            ped.MarkAsNoLongerNeeded();
            ped2.MarkAsNoLongerNeeded();
            ped3.MarkAsNoLongerNeeded();
            ped4.MarkAsNoLongerNeeded();
        }

    }
}
