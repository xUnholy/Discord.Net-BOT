using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Discord
{
    internal class Settings
    {
        public string BotToken { get; set; }    = "";
<<<<<<< HEAD
        public string UserToken { get; set; }   = "";
        public string CurrentGame { get; set; } = "Pokémon GO™";
        public string DiscordUrl { get; set; }  = "https://discord.gg/GTUbKSZ";
=======
        public string ClientToken { get; set; } = "";
        public string CurrentGame { get; set; } = "";
        public string DiscordUrl { get; set; }  = "";
>>>>>>> refs/remotes/origin/master
        public char Prefix { get; set; }        = '!';

        public ulong[] Staff { get; set; }  =
        {
            179135233726087168,     //Mi
            207193860797890560,     //ex
            206553736120762369,     //Wa
            122000111915106305,     //Xi
            208813305169575936,     //Ra
            197924941616644097,     //My
        };

<<<<<<< HEAD
        public Dictionary<string, ulong> Channels;
=======
        public static Dictionary<string, ulong> Channels;
>>>>>>> refs/remotes/origin/master

        public void CreateChannelList()
        {
            Channels = new Dictionary<string, ulong>
            {
                {"announcements", 210510987189682176},
                {"readme", 210516791263363073},
                {"general", 210511123416481793},
                {"german-only", 210511123416481793},
                {"farm-locations", 221429793361494017},
                {"off-topic", 210511123416481793},
                {"donators_chat", 217687094749822977},
                {"donators_release", 219856078450458624},
                {"testing_feedback", 210512518076956673},
                {"dev_tasks", 220286522543308801},
                {"development_private", 211813818987315210}
            };
        }
    }
}
