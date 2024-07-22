using System;
using System.Windows.Forms;

using IVSDKDotNet;
using IVSDKDotNet.Enums;
using static IVSDKDotNet.Native.Natives;

using CCL.GTAIV;
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
                //IVMenuManager.HudOn = false;
                //IVMenuManager.RadarMode = 0;
                IVPed playerPed = IVPed.FromUIntPtr(IVPlayerInfo.FindThePlayerPed());
                playerId = IVPedExtensions.GetHandle(playerPed);

                
                GET_CURRENT_CHAR_WEAPON(playerId, out currentWeapon);
                GET_WEAPONTYPE_SLOT((int)currentWeapon, out int slot);
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