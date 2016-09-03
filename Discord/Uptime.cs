using System;
using System.Timers;
using System.Diagnostics;

namespace Discord
{
    class Uptime
    {
        private Timer _upTimer;

        public Uptime()
        {
            _upTimer = new Timer(10000);
            _upTimer.Elapsed += new ElapsedEventHandler(OnElapsed);
            _upTimer.Enabled = true;
        }

        public void OnElapsed(object sender, ElapsedEventArgs e)
        {
            DateTime Startup = Process.GetCurrentProcess().StartTime;
            TimeSpan Uptime = DateTime.Now - Startup;

            string days = Uptime.Days + " day" + (Uptime.Days != 1 ? "s" : "") + ", ";
            string hours = Uptime.Hours + " hour" + (Uptime.Hours != 1 ? "s" : "") + ", and ";
            string mins = Uptime.Minutes + " min" + (Uptime.Minutes != 1 ? "s" : "");
            string UptimeString = days + hours + mins;

            Console.Title = "Ethereal Discord Bot - [" + UptimeString + "]";
        }
    }
}