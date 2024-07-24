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
    public class SomeFixes
    {
        private static int playerId;
        private static int currentWeapon;
        private static Logger log = Main.log;
        public static void Tick()
        {
            try
            {
				//idhar sniper ka slot change kia gya.
				//changing sniper slot so that we can move while sniped in/zoomed in
                GET_CURRENT_CHAR_WEAPON(Helpers.GamePlayerPed.GetHandle(), out currentWeapon);
                GET_WEAPONTYPE_SLOT((int)currentWeapon, out int slot);
               // bool slotchange = false;
                if (slot == 6)
                {
                    if (NativeControls.IsGameKeyPressed(0, GameKey.Aim))
                    {
                        IVWeaponInfo.GetWeaponInfo((uint)currentWeapon).WeaponSlot = 16;
                        
                    }
                    else
                    {
                        IVWeaponInfo.GetWeaponInfo((uint)currentWeapon).WeaponSlot = 6;
                        
                    }
                }
            }
            catch (Exception ex)
            {
                log.Fatal($"Error in Script [SomeFixes.cs], {ex.GetType().ToString()}, {ex.ToString()}.");
            }
        }
    }
}