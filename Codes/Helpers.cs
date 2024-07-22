using CCL.GTAIV;
using IVSDKDotNet;
using static IVSDKDotNet.Native.Natives;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace HardCore.Codes
{
    internal class Helpers
    {
        public static IVPed GamePlayerPed => IVPed.FromUIntPtr(IVPlayerInfo.FindThePlayerPed());
        public static IVPlayerInfo GamePlayer => IVPlayerInfo.FromUIntPtr(IVPlayerInfo.FindThePlayerPed());

        public static uint PlayerID => GET_PLAYER_ID();
        public static uint PlayerPedID => (uint)CONVERT_INT_TO_PLAYERINDEX(PlayerID);
		
		//THIS DID NOT FUCKING WORKED AS INTENTED FUCCK THIS SHIT
		
		//everything below this is trashed stuff fuck it ignore it i dont care
		//waise to scripthookdotnet ke World.GetAllPeds() jaisa kaam karna chaiye magar kam nahi kar raha hai
        public static IVPed[] GetAllPeds(int Model =0)
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


        public static IVVehicle[] GetAllVehicles(int Model=0)
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
