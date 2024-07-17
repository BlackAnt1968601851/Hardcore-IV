using CCL.GTAIV;
using IVSDKDotNet;
using static IVSDKDotNet.Native.Natives;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HardCore.Codes
{
    internal class Helpers
    {
       public static IVPed GamePlayerPed => IVPed.FromUIntPtr(IVPlayerInfo.FindThePlayerPed());
        public static IVPlayerInfo GamePlayer => IVPlayerInfo.FromUIntPtr(IVPlayerInfo.FindThePlayerPed());

        public static IVPed[] GetAllPeds(string[] modelNames = null)
        {
            // Convert model names to model hashes (uint) and then to int
            int[] modelHashes = null;
            if (modelNames != null && modelNames.Length > 0)
            {
                modelHashes = new int[modelNames.Length];
                for (int i = 0; i < modelNames.Length; i++)
                {
                    uint hash = RAGE.AtStringHash(modelNames[i]);
                    modelHashes[i] = (int)hash;
                }
            }

            // Getting the total number of spawned peds
            IVPool PedPool = IVPools.GetPedPool();

            // List to store the valid peds
            List<IVPed> PedList = new List<IVPed>();

            // Loop through the ped pool to get all peds
            for (int i = 0; i < PedPool.Count; i++)
            {
                // Get the ped pointer from the pool
                UIntPtr ptr = PedPool.Get(i);
                if (ptr != UIntPtr.Zero)
                {
                    // Get the ped handle from the pointer
                    int pedHandle = (int)PedPool.GetIndex(ptr);

                    // Get the IVPed instance from the handle
                    IVPed getped = NativeWorld.GetPedInstaceFromHandle(pedHandle);

                    if (DOES_CHAR_EXIST(getped.GetHandle()))
                    {
                        // Get the model of the ped
                        GET_CHAR_MODEL(getped.GetHandle(), out int model);

                        // Check if the model is valid and matches any of the specified model hashes
                        if (model != 0 && (modelHashes == null || Array.Exists(modelHashes, hash => hash == model)))
                        {
                            // Add the ped to the list
                            PedList.Add(getped);
                        }
                    }
                }
            }

            // Return the list of valid peds as an array
            return PedList.ToArray();
        }


        public static IVVehicle[] GetAllVehicles()
        {
            //Letting the same as SHDN
            //Memory Access- grabbing total spawned veh.
            IVPool VehiclePool = IVPools.GetVehiclePool();
            //List to store them
            List<IVVehicle> VehicleList = new List<IVVehicle>();
            //Loop we do so that we can get total amount of vehs spawned and their count
            for (int i = 0; i < VehiclePool.Count; i++)
            {
                //game ptr value check
                UIntPtr ptr = VehiclePool.Get(i);
                if (ptr != UIntPtr.Zero)
                {
                    //getting handle from pool ptr
                    int VehicleHandle = (int)VehiclePool.GetIndex(ptr);
                    //changing the handle (uint) to IVVehicle
                    IVVehicle getveh = NativeWorld.GetVehicleInstaceFromHandle(VehicleHandle);
                    if (DOES_CHAR_EXIST(IVVehicleExtensions.GetHandle(getveh)))
                    {
                        GET_CHAR_MODEL(IVVehicleExtensions.GetHandle(getveh), out int model);
                        if (model != 0)
                        {
                            //adding those IVVehicle Values.
                            VehicleList.Add(getveh);
                        }
                    }
                }
            }
            //return them as array for useful work.
            return VehicleList.ToArray();
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
