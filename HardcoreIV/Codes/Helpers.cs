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
