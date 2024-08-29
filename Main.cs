using CCL.GTAIV;
using IVSDKDotNet;
using static IVSDKDotNet.Native.Natives;
using System;
using IVSDKDotNet.Enums;
using System.Numerics;

namespace HardCore
{
    public class Main : Script
    {
        private int Intervals; // Timer variable (Timer ke liye variable)
        private int CheckTimer = 15000; // Time interval in milliseconds (Samay antaraal milliseconds mein)

        public Main()
        {
            Tick += Main_Tick; // Main tick function event handler (Main tick function ke liye event handler)
            Initialized += Main_Initialized; // Initialization event handler (Initialization ke liye event handler)
            // Unused event handlers commented out (Niche wale unused handlers ko comment out kiya hai)
            // Drawing += Main_Drawing;
            // KeyDown += Main_KeyDown;
            // ProcessAutomobile += Main_ProcessAutomobile;
            // ProcessCamera += Main_ProcessCamera;
            // IngameStartup += Main_IngameStartup;
            // WaitTick += Main_WaitTick;
            // GameLoadPriority += Main_GameLoadPriority;
        }

        public static Logger log = new(); // Static logger for logging (Logging ke liye static logger)

        // Reading of INI files (INI files ko yahan read karein)
        private void Main_Initialized(object sender, EventArgs e)
        {
            /* Add code to read INI files here (Yahan INI file read karne ka code daalein) */
        }

        // Check for game timers (Game timers ko check karne ke liye property)
        public static int GameTime
        {
            get
            {
                GET_GAME_TIMER(out uint pTimer);
                return (int)pTimer;
            }
        }

        // Function that runs every frame in-game (Har frame mein run hone wala function)
        private void Main_Tick(object sender, EventArgs e)
        {
            try
            {
                // Call untimed tasks here (Yahan untimed tasks ko call karein, FPS based)
                CombatTweaks.Tick();
                Traffic.Tick();
                WorldFixes.Initiate();
                // Execute timed tasks after specific intervals (Timed tasks ko interval ke baad execute karein)
                if (GameTime > Intervals + CheckTimer)
                {
                    Intervals = GameTime; // Reset timer (Timer reset karein)

                }
            }
            catch (Exception ex)
            {
                // Log specific details of the exception (Exception details ko log karein)
                log.Fatal($"Error occurred in Script [Main.cs]: {ex.GetType().FullName}, Message: {ex.Message}, Stack Trace: {ex.StackTrace}");

                // Optionally log inner exceptions if present (Agar inner exception present ho toh usko bhi log karein)
                if (ex.InnerException != null)
                {
                    log.Fatal($"Inner Exception: {ex.InnerException.GetType().FullName}, Message: {ex.InnerException.Message}, Stack Trace: {ex.InnerException.StackTrace}");
                }
            }
        }
    }
}
