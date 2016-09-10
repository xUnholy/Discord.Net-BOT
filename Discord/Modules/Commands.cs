using Discord;
using Discord.Commands;
using Discord.Commands.Permissions.Levels;
using Discord.Modules;
using Discord.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Discord.Modules
{
    public class Commands : IModule
    {
        private ModuleManager _manager;
        private DiscordClient _client;

        List<Logs> MyRequestLogs = new List<Logs>();
        List<Logs> MyErrorLogs = new List<Logs>();

        void IModule.Install(ModuleManager manager)
        {
            _manager = manager;
            _client = manager.Client;

            manager.CreateCommands("", cmd =>
            {
                cmd.CreateCommand("report")
                    .Description("Use this function to report a new bug or error with Ethereal Bot")
                    .Parameter("Report", ParameterType.Unparsed)
                    //.AddCheck((c, u, ch) => Settings.Channels.Contains(ch.Id))
                    .Do(async (e) =>
                    {
                        if (e.GetArg("Report") != "")
                            Logs.AddToLog(e.User.Name, e.GetArg("Report"), MyErrorLogs, "Error");
                            await e.Channel.SendMessage($"{e.User.Mention} Thank you, your report has been logged.");
                    });

                cmd.CreateCommand("request")
                    .Description("Use this function to request a new feature or modification to Ethereal Bot")
                    //.AddCheck((c, u, ch) => Settings.Channels.Contains(ch.Id))
                    .Parameter("RequestMsg", ParameterType.Unparsed)
                    .Do(async (e) =>
                    {
                        Logs.AddToLog(e.User.Name, e.GetArg("RequestMsg"), MyRequestLogs, "Request");
                        await e.Channel.SendMessage($"{e.User.Mention} Thank you, your request has been logged.");
                    });

                cmd.CreateCommand("pokestops")
                    .Description(@"Get information regarding *No Pokestops Found* error")
                    .Do(async (e) =>
                    {
                        await e.User.SendMessage(@"**No Pokestops Found** possible causes:" + Environment.NewLine +
                                                 $"1) Account has reached the maximum limit for number of pokestops allowed in a 23 hour period (2000)." + Environment.NewLine +
                                                 $"2) Account has been IP banned (approximately 4 hour duration)" + Environment.NewLine +
                                                 $"3) Bot cannot see any Pokestops in range of your defined coordinates - please try a new location.");
                    });

                cmd.CreateCommand("about")
                    .Description("About this Discord bot")
                    .Do(async (e) =>
                    {
                        await e.User.SendMessage($"Ethereal Discord bot was developed by Broly & xUnholy");
                    });

                cmd.CreateCommand("responses")
                    .Description(@"Get information regarding *Expected 6 responses, but got 0 responses* exception")
                    .Do(async (e) =>
                    {
                        await e.User.SendMessage($"Please confirm the Pokémon GO™ servers are stable " + 
                                                  "and try restarting Ethereal Bot.");
                    });

                cmd.CreateCommand("kick")
                    .Parameter("UserToKick", ParameterType.Required)
                    .AddCheck((c, u, ch) => u.GetPermissions(ch).ManageChannel)
                    .Do(async (e) =>
                    {
                        var _user = e.Server.FindUsers(e.Args[0]).FirstOrDefault();
                        if (_user != null)
                        {
                            await _user.Kick();
                            await e.User.SendMessage($"Kicked: " + _user);
                        }
                    });

                cmd.CreateCommand("device")
                    .Description("Get information about providing your anonymous Device Package information")
                    .Do(async (e) =>
                    {
                        await e.Channel.SendMessage(@"@everyone" + Environment.NewLine +
                                                    @"We are gathering phone device package information:" + Environment.NewLine + Environment.NewLine +
                                                    @"1) Download Pok Device Info app from the Google Play Store: http://goo.gl/F7WzhF" + Environment.NewLine +
                                                    @"2) Click the Share button on the app(top right)" + Environment.NewLine +
                                                    $"3) Click Discord and select private message to {e.Server.FindUsers("Xi.Cynx ツ").FirstOrDefault().Mention}" + Environment.NewLine +
                                                    @"4) Remove your DeviceID (this ensures your anonymity)" + Environment.NewLine +
                                                    @"5) Send!" + Environment.NewLine +
                                                    @"This will better help us develop a secured system with many phone types included to choose from." + Environment.NewLine +
                                                    @"Be sure to include the Advertised Brand and Model (e.g. Samsung Galaxy S6 Edge) after you send your info." + Environment.NewLine +
                                                    @"Please only send actual physical phone info and not virtual or emulated information.  Thanks!");
                    });

                cmd.CreateCommand("donate")
                    .Description("Get information regarding donations")
                    .Do(async (e) =>
                    {
                        await e.User.SendMessage($"Please PM {e.Server.FindUsers("Mysticales").FirstOrDefault().Mention} " +
                                                  "for information about supporting the team");
                    });

                cmd.CreateCommand("download")
                    .Description("Get information regarding where to download Ethereal bot")
                    .Do(async (e) =>
                    {
                        await e.User.SendMessage($"Please see {e.Server.GetChannel(Settings.Channels["announcements"]).Mention} " +
                                                  "for details on the current release and where to get it");
                    });

                cmd.CreateCommand("pinned")
                    .Description("Get information regarding pinned messages")
                    .Do(async (e) =>
                    {
                        //await e.User.SendFile("http://i.imgur.com/d8E8urd.png");
                        await e.User.SendMessage($"Pinned messages are only available to view on the desktop version of " +
                                                 @"Discord, please go to https://discordapp.com/download for the latest " +
                                                 $"release.");
                    });

                cmd.CreateCommand("gui")
                    .Description("Get information regarding the WebUI version of Ethereal Bot")
                    .Do(async (e) =>
                    {
                        await e.User.SendMessage($"A WebUI version of Ethereal bot is currently in " +
                                                 $"development and will only be available to donators." + Environment.NewLine +
                                                 $"Updates will be posted in " +
                                                 $"{e.Server.GetChannel(Settings.Channels["announcements"]).Mention}");
                    });

                cmd.CreateCommand("pushbullet")
                    .Description("Get information regarding PushBullet notifications")
                    .Do(async (e) =>
                    {
                        await e.User.SendMessage($"PushBullet is an API that allows you to receive updates about the bot " + 
                                                 $"on your phone. Please visit " + @"https://www.pushbullet.com/ " + 
                                                 $"to sign up and obtain your API key.");
                    });

                cmd.CreateCommand("encrypt")
                    .Description("Get information regarding encrypt.dll")
                    .Do(async (e) =>
                    {
                        await e.User.SendMessage($"See {e.Server.GetChannel(Settings.Channels["readme"]).Mention} for details " + Environment.NewLine +
                                                 $"about where to get encrypt.dll." + Environment.NewLine + 
                                                 $"Please place in the same folder as your .exe");
                    });

                cmd.CreateCommand("dump")
                  .MinPermissions((int)Permissions.ServerAdmin)
                  .Do(async (e) =>
                  {
                      string dumpDirectory = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory) +
                                             $"\\LogDump_{DateTime.Now.Date.ToString($"dd_MM_yy")}.txt";

                      Logs.WriteToLogFile(dumpDirectory, MyErrorLogs);
                      Logs.WriteToLogFile(dumpDirectory, MyRequestLogs);

                      await e.User.SendMessage($"All logs have been dumped.");
                  });
            });
        }
    }

    public static class StringExtensions
    {
        public static bool ContainsAny(this string input, params string[] str)
        {
            foreach (string toFind in str)
            {
                if (input.Contains(toFind))
                    return true;
            }
            return false;
        }
    }
}

