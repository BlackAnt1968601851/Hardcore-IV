/*using CCL.GTAIV;
using IVSDKDotNet;
using static IVSDKDotNet.Native.Natives;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace HardCore.Codes
{
    public class PlayerCoreStuff
    {

        // still to do.....

        private static int playerId;
        private static bool arrest = false;
        private static Logger log = Main.log;
        private static int Intervals;
        private static int CheckTimer = 3000;

        public static void Tick()
        {
            try
            { 
                if (Main.GameTime > Intervals + CheckTimer)
                {
                    Intervals = Main.GameTime;

                    if (arrest == true)
                    {
                        arrest = false;
                        SET_PLAYER_CONTROL(playerId, true);
                    }
                }

                if(IS_PLAYER_BEING_ARRESTED())
                {
                    arrest = true;
                    SET_PLAYER_CONTROL(playerId, false);
                }

                    GET_PLAYER_MAX_HEALTH(playerId, out int maxhl);
                    if (maxhl > 150)
                        SET_CHAR_MAX_HEALTH(plyped, 150);
                    PRINT_STRING_WITH_LITERAL_STRING_NOW($"PLAYER Health :{maxhl} and may change.", "STRING", 10, true);
                    //Checking for Player's Total money.
                    
                    if (HAS_CHAR_BEEN_ARRESTED(plyped) && arrest == true)
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
                    }

                    if (IS_CHAR_DEAD(Helpers.GamePlayerPed.GetHandle()))
                {
                    REMOVE_ALL_CHAR_WEAPONS(Helpers.GamePlayerPed.GetHandle());
                }
            }
            catch (Exception ex)
            {
                log.Fatal($"Error in Script [PlayerCoreStuff.cs], {ex.GetType().ToString()}, {ex.ToString()}.");
            }
        }
    }
}
*/


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
    public class PlayerCoreStuff
    {
        private static int playerId;
        private static bool isArrested = false;
        private static bool isDead = false;
        private static Logger log = Main.log;
        private static int Intervals;
        private static int CheckTimer = 7000;

        public static void Tick()
        {
            try
            {
                // Check timer interval
                if (Main.GameTime > Intervals + CheckTimer)
                {
                    Intervals = Main.GameTime;

                    // Reset player control if previously arrested
                    if (isArrested)
                    {
                        isArrested = false;
                        SET_PLAYER_CONTROL(playerId, true);
                    }

                    // Reset player control if previously dead
                    if (isDead)
                    {
                        isDead = false;
                        SET_PLAYER_CONTROL(playerId, true);
                    }
                }

                // Check if player is being arrested
                if (IS_PLAYER_BEING_ARRESTED())
                {
                    isArrested = true;
                    SET_PLAYER_CONTROL(Helpers.GamePlayer.PlayerId, false);
                }

                // Check if player is dead
                if (IS_CHAR_DEAD(Helpers.GamePlayerPed.GetHandle()))
                {
                    if (!isDead)
                    {
                        isDead = true;
                        REMOVE_ALL_CHAR_WEAPONS(Helpers.GamePlayerPed.GetHandle());
                    }
                }
            }
            catch (Exception ex)
            {
                log.Fatal($"Error in Script [PlayerCoreStuff.cs], {ex.GetType().ToString()}, {ex.ToString()}.");
            }
        }
    }
}
