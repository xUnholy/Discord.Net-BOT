using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Discord
{
    public class Logs
    {
        public string Date { get; set; }
        public string User { get; set; }
        public string LogType { get; set; }
        public string Logged { get; set; }

        public static void WriteToLogFile(string logPath, List<Logs> logName)
        {
            using (var file = new StreamWriter(logPath, true))
            {
                foreach (var log in logName)
                {
                    file.WriteLine($"[{log.Date}] [{log.LogType}] [{log.User}] -- {log.Logged}");
                }
                file.Close();
            }
        }

        public static void AddToLog(string user, string msg, List<Logs> log, string type)
        {
            log.Add(new Logs()
            {
                Date        = $"{DateTime.Now.Date.ToString($"dd.MM.yy")}",
                LogType     = $"{type}",
                User        = $"{user}",
                Logged      = $"{msg}"
            });
        }
    }
}
