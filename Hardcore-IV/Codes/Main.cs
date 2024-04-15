using System;
using System.Windows.Forms;

using IVSDKDotNet;
using IVSDKDotNet.Enums;
using static IVSDKDotNet.Native.Natives;

using IVNatives;
using HardCore.Codes;

namespace HardCore
{
    public class Main : Script
    {

        #region Variables
        //private int playerPed;
        #endregion

        #region Timers
        private int Intervals;
        private int CheckTimer = 50;
        #endregion

        #region Constructor
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
        #endregion

        public static Logger log = new ();
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
                //SET_MONEY_CARRIED_BY_ALL_NEW_PEDS(Main.GenerateRandomNumber(1, 24950));

                //Call stuff Untimed here:
                LawFixes.Tick();
                PlayerCoreStuff.Tick();
                SomeFixes.Tick();

                //We call the Timed Stuff after this:
                if (GameTime > Intervals + CheckTimer)
                {
                    Intervals = GameTime;
                    //Call stuffs Timed here:

                }
            }
            catch (Exception ex)
            {
                log.Fatal($"Error in Script [Main.cs], {ex.GetType().ToString()}, {ex.ToString()}.");
            }
        }

        public static Random rnd = new Random();

        public static int GenerateRandomNumber(int min, int max)
        {
            return rnd.Next(min, max + 1);
        }

        public static string[] ToArray(string input)
        {
            string[] array = input.Split(',');
            for (int i = 0; i < array.Length; i++)
            {
                array[i] = string.Join("", array[i].Split((string[])null, StringSplitOptions.RemoveEmptyEntries));
            }
            return array;
        }
    } 
}
