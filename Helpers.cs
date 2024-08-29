using CCL.GTAIV;
using IVSDKDotNet;
using static IVSDKDotNet.Native.Natives;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Numerics;

namespace HardCore
{
    internal class Helpers
    {
        //position grabber
        public static Vector3 GetPositionOnStreet(Vector3 pos, out float heading, float radius = 5f)
        {
            heading = 0;
            for (uint i = 1; i < 40; i++)
            {
                GET_NTH_CLOSEST_CAR_NODE_WITH_HEADING(pos, i, out Vector3 newPos, out var h);
                if (!IS_POINT_OBSCURED_BY_A_MISSION_ENTITY(newPos, new Vector3(radius)))
                {
                    heading = h;
                    return newPos;
                }
            }

            return Vector3.Zero; // Return zero vector if no valid position is found (Koi valid position na mile toh zero vector return karein)
        }

        public static IVPed GamePlayerPed => IVPed.FromUIntPtr(IVPlayerInfo.FindThePlayerPed());

        public static IVPlayerInfo GamePlayer => IVPlayerInfo.FromUIntPtr(IVPlayerInfo.FindThePlayerPed());

        public static uint PlayerID => GET_PLAYER_ID();

        public static uint PlayerPedID => (uint)CONVERT_INT_TO_PLAYERINDEX(PlayerID);

        public static Random rnd = new Random(); // Random number generator (Random number generate karne wala object)

        // Function to generate random numbers (Random number generate karne ka function)
        public static int GenerateRandomNumber(int min, int max)
        {
            return rnd.Next(min, max + 1);
        }

        // Method to convert a string to an array (String ko array mein convert karne ka method)
        public static string[] ToArray(string input)
        {
            string[] array = input.Split(',');
            for (int i = 0; i < array.Length; i++)
            {
                array[i] = string.Join("", array[i].Split((string[])null, StringSplitOptions.RemoveEmptyEntries));
            }
            return array;
        }

        //THIS DID NOT FUCKING WORKED AS INTENTED FUCCK THIS SHIT

        //everything below this is trashed stuff fuck it ignore it i dont care
        //waise to scripthookdotnet ke World.GetAllPeds() jaisa kaam karna chaiye magar kam nahi kar raha hai
        public static IVPed[] GetAllPeds(int Model = 0)
        {
            // List to store the valid peds
            List<IVPed> List = new List<IVPed>();

            var model = 0; var findmodel = Model;

            // Getting the total number of spawned peds
            IVPool PedPool = IVPools.GetPedPool();

            // Loop through the ped pool to get all peds
            for (int i = 0; i < PedPool.Count; i++)
            {
                // Get the ped pointer from the pool
                UIntPtr ptr = PedPool.Get(i);

                if (ptr == UIntPtr.Zero)
                    continue;

                // Get the ped handle from the pointer
                int handle = (int)PedPool.GetIndex(ptr);

                if (!DOES_CHAR_EXIST(handle))
                    continue;

                // Get the model of the ped
                GET_CHAR_MODEL(handle, out model);

                // Check if the model is valid and matches any of the specified model hashes
                if (model != 0 && (findmodel == 0 || findmodel == model))
                {
                    // Add the ped to the list
                    // Get the IVPed instance from the handle
                    IVPed gethandle = NativeWorld.GetPedInstaceFromHandle(handle);
                    List.Add(gethandle);
                }
            }
            // Return the list of valid peds as an array
            return [.. List];
        }

        public static IVVehicle[] GetAllVehicles(int Model = 0)
        {
            // List to store the valid cars
            List<IVVehicle> List = new();

            uint model = 0; var findmodel = Model;

            // Getting the total number of spawned cars
            IVPool VehiclePool = IVPools.GetVehiclePool();

            // Loop through the car pool to get all car
            for (int i = 0; i < VehiclePool.Count; i++)
            {
                // Get the car pointer from the pool
                UIntPtr ptr = VehiclePool.Get(i);

                if (ptr == UIntPtr.Zero)
                    continue;

                // Get the car handle from the pointer
                int handle = (int)VehiclePool.GetIndex(ptr);

                if (!DOES_VEHICLE_EXIST(handle))
                    continue;

                // Get the model of the car
                GET_CAR_MODEL(handle, out model);

                // Check if the model is valid and matches any of the specified model hashes
                if (model != 0 && (findmodel == 0 || findmodel == (int)model))
                {
                    // Add the ped to the list
                    // Get the IVVehicle instance from the handle
                    IVVehicle gethandle = NativeWorld.GetVehicleInstaceFromHandle(handle);
                    List.Add(gethandle);
                }
            }
            // Return the list of valid cars as an array
            return [.. List];
        }

        public static IVObject[] GetAllObjects()
        {
            IVPool ObjectPool = IVPools.GetObjectPool();
            List<IVObject> ObjectList = new List<IVObject>();
            for (int i = 0; i < ObjectPool.Count; i++)
            {
                UIntPtr ptr = ObjectPool.Get(i);
                if (ptr != UIntPtr.Zero)
                {
                    int objHandle = (int)ObjectPool.GetIndex(ptr);
                    IVObject getobj = NativeWorld.GetObjectInstaceFromHandle(objHandle);
                    if (DOES_CHAR_EXIST(IVObjectExtensions.GetHandle(getobj)))
                    {
                        GET_CHAR_MODEL(IVObjectExtensions.GetHandle(getobj), out int model);
                        if (model != 0)
                        {
                            ObjectList.Add(getobj);
                        }
                    }
                }
            }
            return ObjectList.ToArray();
        }
    }
}