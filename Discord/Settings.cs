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
        public string BotToken { get; set; }    = "MjIwMzk2NDk3MDA5NzcwNDk3.CrYxFA._g2bIDXcgM4g8REu4K56jPxBsL4";
        public string ClientToken { get; set; } = "Rk1guIslJH6MSk3i4XvZ7VcqRSSSi5hd";
        public string ClientID { get; set; } = "220396497009770497";
        public string CurrentGame { get; set; } = "Pokémon GO™";
        public string DiscordUrl { get; set; }  = "https://discord.gg/GTUbKSZ";
        public char Prefix { get; set; }        = '!';

        public ulong[] Staff { get; set; }  =
        {
            179135233726087168,     //Michael
            207193860797890560,     //exverse
            206553736120762369,     //Wasabi
            122000111915106305,     //Xi Cynx
            208813305169575936,     //Raitaru
            197924941616644097,     //Mysticales
        };

        public ulong[] Coords { get; set; } =                                   //Format: Server ID, Channel ID, User ID
        {
            208945864343814145,209002390257532938,220565708499582976,           //NemesisBot
            211288195462201347,221820148162494464,210568514484961280            //PokeSnipers
        };

        public static Dictionary<string, ulong> Channels;

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
