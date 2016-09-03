using System;
using System.Collections.Generic;
using System.Configuration;

namespace Discord
{
    public static class Settings
    {
        public static ulong ClientID { get; set; }              = Convert.ToUInt64(ConfigurationManager.AppSettings["ClientID"]);
        public static ulong ServerID { get; set; }              = Convert.ToUInt64(ConfigurationManager.AppSettings["ServerID"]);
        public static string BotToken { get; set; }             = ConfigurationManager.AppSettings["BotToken"];
        public static string DefaultChan { get; set; }          = ConfigurationManager.AppSettings["DefaultChan"];
        public static string DiscordURL { get; set; }           = ConfigurationManager.AppSettings["DiscordURL"];
        public static string CurrentGame { get; set; }          = ConfigurationManager.AppSettings["CurrentGame"];
        public static string AnnouncerInterval { get; set; }    = ConfigurationManager.AppSettings["AnnouncerInterval"];

        public static readonly List<ulong> Channels = new List<ulong>()
        {
            //210511123416481793,     //general
            //210516791263363073,     //readme
            //210510987189682176,     //announcements
            217687094749822977,     //donators_chat
            210512518076956673,     //testing_feedback
            220286522543308801,     //dev_tasks
            211813818987315210      //development_private
        };
    }

    public class Logs
    {
        public string Date { get; set; }
        public string User { get; set; }
        public string Logged { get; set; }
    }
}

