using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace Discord.Tasks
{
    public class Uptime
    {
        public Timer UpTimer;

        public Uptime()
        {
            UpTimer = new Timer(10000);
            UpTimer.Elapsed += new ElapsedEventHandler(OnElapsed);
            UpTimer.Enabled = true;
        }

        public void OnElapsed(object sender, ElapsedEventArgs e)
        {
            DateTime startup = Process.GetCurrentProcess().StartTime;
            TimeSpan uptime = DateTime.Now - startup;

            var days = uptime.Days + " day" + (uptime.Days != 1 ? "s" : "") + ", ";
            var hours = uptime.Hours + " hour" + (uptime.Hours != 1 ? "s" : "") + ", and ";
            var mins = uptime.Minutes + " min" + (uptime.Minutes != 1 ? "s" : "");
            var uptimeString = days + hours + mins;

            Console.Title = $"Ethereal Discord Bot - [{uptimeString}]";
        }
    }
}
