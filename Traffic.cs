using CCL.GTAIV;
using IVSDKDotNet;
using static IVSDKDotNet.Native.Natives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IVSDKDotNet.Enums;
using System.Numerics;

namespace HardCore
{
    public class Traffic
    {
        private static int Intervals;
        private static int CheckTimer =10;

        public static void Tick()
        {
            TrafficSecurity(); 
        }

        // Function to spawn Gruppe6 team and vehicle
        private static void TrafficSecurity()
        {
            Vector3 playerPos = Helpers.GamePlayerPed.Matrix.Pos; // Get player position 
            Vector3 pos2;
            float heading;
            float distanceToPlayer;

            // Find a position that is between 70 and 180 meters from the player
            do
            {
                float randomDistance = Helpers.GenerateRandomNumber(70, 180); // Generate random distance 
                Vector3 vehiclePos = playerPos.Around(randomDistance); // Find a new position around the player 

                pos2 = Helpers.GetPositionOnStreet(vehiclePos, out heading); // Find a valid street position
                distanceToPlayer = Vector3.Distance(pos2, playerPos); // Calculate distance to player
            }
            while (distanceToPlayer < 70 || distanceToPlayer > 180); // Repeat if position is not within range

            var car = NativeWorld.SpawnVehicle("stockade", pos2, out int handlecar, true, false); // Spawn vehicle

            // Spawn security members around the player 
            Vector3 pedSpawnPos = playerPos.Around(distanceToPlayer); // SWAT spawn position 
            var ped = NativeWorld.SpawnPed("m_M_armoured", pedSpawnPos, out int pedhandle, true, false);

            var ped2 = NativeWorld.SpawnPed("m_M_armoured", pedSpawnPos, out int pedhandle2, true, false);
            var ped3 = NativeWorld.SpawnPed("m_M_armoured", pedSpawnPos, out int pedhandle3, true, false);
            var ped4 = NativeWorld.SpawnPed("m_M_armoured", pedSpawnPos, out int pedhandle4, true, false);

            var seat = -2 + 1;
            // Assign SWAT members to vehicle seats 
            ped.GetTaskController().WarpIntoVehicle(car, (uint)seat); // Driver seat
            ped2.GetTaskController().WarpIntoVehicle(car, (uint)0);   // Front passenger seat
            ped3.GetTaskController().WarpIntoVehicle(car, (uint)1);   // Rear left seat
            ped4.GetTaskController().WarpIntoVehicle(car, (uint)2);   // Rear right seat

            Main.log.Debug($"Spawned SWAT vehicle at: {pos2}, player position: {playerPos}. Distance to Position: {distanceToPlayer}."); // Log spawning details (Spawning details log karein)

            //GIVE_WEAPON_TO_CHAR(ped3.GetHandle(), (int)eWeaponType.WEAPON_SNIPERRIFLE, 200, false); // Assign weapon (Weapon assign karein)

            car.PlaceOnGroundProperly(); // Place vehicle properly on ground (Vehicle ko sahi jagah par rakhte hain)
            car.SetHeading(heading); // Set vehicle heading (Heading set karein)

            SET_CAR_FORWARD_SPEED(car.GetHandle(), 5); // Set vehicle speed (Vehicle speed set karein)

            // Unmark entities to save memory (Entities ko unmark karein memory bachane ke liye)
            car.MarkAsNoLongerNeeded();
            ped.MarkAsNoLongerNeeded();
            ped2.MarkAsNoLongerNeeded();
            ped3.MarkAsNoLongerNeeded();
            ped4.MarkAsNoLongerNeeded();
        }
    }
}
