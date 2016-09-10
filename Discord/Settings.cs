using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Discord
{
    class Settings
    {
        public string Token { get; set; }       = "MjI0MjQ5OTgwNzg3ODg0MDMy.CrXw5w.9XQhFLPR9CzLbPDDINkIoH6RmVI";
        public string CurrentGame { get; set; } = "Pokémon GO™";
        public string DiscordURL { get; set; }  = "https://discord.gg/GTUbKSZ";
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
            Channels = new Dictionary<string, ulong>();

            Channels.Add("announcements", 210510987189682176);
            Channels.Add("readme", 210516791263363073);
            Channels.Add("general", 210511123416481793);
            Channels.Add("german-only", 210511123416481793);
            Channels.Add("farm-locations", 221429793361494017);
            Channels.Add("off-topic", 210511123416481793);
            Channels.Add("donators_chat", 217687094749822977);
            Channels.Add("donators_release", 219856078450458624);
            Channels.Add("testing_feedback", 210512518076956673);
            Channels.Add("dev_tasks", 220286522543308801);
            Channels.Add("development_private", 211813818987315210);
        }
    }
}
