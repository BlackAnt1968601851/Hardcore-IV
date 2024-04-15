using System;
using System.Windows.Forms;

using IVSDKDotNet;
using IVSDKDotNet.Enums;
using static IVSDKDotNet.Native.Natives;

using IVNatives;

namespace HardCore.Codes
{
    public class PlayerCoreStuff
    {

        // still to do.....

        private static int playerId;
        private static bool arrest = false;
        private static Logger log = Main.log;

        public static void Tick()
        {
            try
            {
                IVPed playerPed = IVPed.FromUIntPtr(IVPlayerInfo.FindThePlayerPed());
                var plyped = playerPed.GetHandle();
                playerId = IVPedExtensions.GetHandle(playerPed);

                /*GET_PLAYER_MAX_HEALTH(playerId, out int maxhl);
                if (maxhl > 150)
                    SET_CHAR_MAX_HEALTH(plyped, 150);
                PRINT_STRING_WITH_LITERAL_STRING_NOW($"PLAYER Health :{maxhl} and may change.", "STRING", 10, true);
                //Checking for Player's Total money.
                */
                /*if (HAS_CHAR_BEEN_ARRESTED(plyped) && arrest == true)
                {
                    arrest = false;
                    STORE_SCORE(playerId, out uint val);
                    ADD_SCORE(playerId, -(int)(val * 35 / 100));
                    REMOVE_ALL_CHAR_WEAPONS(plyped);
                }

                if (HAS_DEATHARREST_EXECUTED())
                {

                }

                if (IS_CHAR_DEAD(plyped))
                {
                    STORE_SCORE(playerId, out uint val);
                    ADD_SCORE(playerId, -(int)(val * 5 / 100));
                    REMOVE_ALL_CHAR_WEAPONS(plyped);
                    
                }
                if (IS_PLAYER_BEING_ARRESTED() && !arrest)
                {
                    //SET_PLAYER_CONTROL(playerId, false);

                    arrest = true;
                }*/
            }
            catch (Exception ex)
            {
                log.Fatal($"Error in Script [PlayerCoreStuff.cs], {ex.GetType().ToString()}, {ex.ToString()}.");
            }
        }
    }
}