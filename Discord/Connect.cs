using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Discord.Commands;

namespace Discord
{
    public class Connect
    {
        public static DiscordClient _client;

        public List<Logs> MyErrorLogs = new List<Logs>();
        public List<Logs> MyRequestLogs = new List<Logs>();

        public Connect()
        {
            _client = new DiscordClient(x =>
            {
                x.AppName = "Ethereal";
                x.AppUrl = "https://discordapp.com/oauth2/authorize?client_id=" + Settings.ClientID + "&scope=bot&permissions=0";
                x.LogLevel = LogSeverity.Info;
                x.LogHandler = Log;
            });

            _client.UsingCommands(x =>
            {
                x.PrefixChar = '!';
                x.AllowMentionPrefix = true;
                x.HelpMode = HelpMode.Private;
            });

            _client.UserJoined += _client_UserJoined;

            _client.UserUpdated += _client_UserOnline;

            CreateCommands();

            _client.ExecuteAndWait(async () =>
            {
                while (true)
                {
                    try
                    {
                        await _client.Connect("jinz.ytube@gmail.com", "Labyrinth56");
                        _client.SetGame(Settings.CurrentGame, GameType.Twitch, Settings.DiscordURL);
                        break;
                    }
                    catch
                    {
                        await Task.Delay(3000);
                    }
                }
            });
        }

        private static async void _client_UserOnline(object sender, UserUpdatedEventArgs e)
        {
            if (e.Before.Status.Value == "offline" && e.After.Status.Value == "online" && e.After.ServerPermissions.ManageChannels == true)
            {
                await Task.Delay(5000);
                await e.After.Server.FindChannels(Settings.DefaultChan).FirstOrDefault().SendMessage($"Welcome back {e.After.Mention}!");
            }
        }

        private static async void _client_UserJoined(object sender, UserEventArgs e)
        {
            await Task.Delay(5000);
            await e.User.SendMessage($"Welcome {e.User.Name} to the Ethereal Bot Discord Server!" + $"\n" +
                                     $"\nPlease check out #announcements and #readme");
        }

        public void CreateCommands()
        {
            var cmdService = _client.GetService<CommandService>();

            cmdService.CreateCommand("report")
                .Description("Use this function to report a new bug or error with Ethereal Bot")
                .Parameter("RepMessage", ParameterType.Unparsed)
                .AddCheck((c, u, ch) =>  Settings.Channels.Contains(ch.Id))
                .Do(async (e) =>
                {
                    await e.Channel.SendMessage($"{e.User.Mention} Thank you, your report has been logged.");
                    MyErrorLogs.Add(new Logs()
                    {
                        Date = $"{DateTime.Now.Date.ToString($"dd.MM.yy")}",
                        User = $"{e.User}",
                        Logged = $"{e.GetArg("RepMessage")}"
                    });
                });

            cmdService.CreateCommand("request")
                .Description("Use this function to request a new feature or modification to Ethereal Bot")
                .AddCheck((c, u, ch) => Settings.Channels.Contains(ch.Id))
                .Parameter("ReqMessage", ParameterType.Unparsed)
                .Do(async (e) =>
                {
                    await e.Channel.SendMessage($"{e.User.Mention} Thank you, your request has been logged.");
                    MyRequestLogs.Add(new Logs()
                    {
                        Date = $"{DateTime.Now.Date.ToString($"dd.MM.yy")}",
                        User = $"{e.User}",
                        Logged = $"{e.GetArg("ReqMessage")}"
                    });
                });

            cmdService.CreateCommand("reportlog")
                .Description("Receive a PM with the current list of reported bugs & errors")
                .AddCheck((c, u, ch) => Settings.Channels.Contains(ch.Id))
                .Do(async (e) =>
                {
                        foreach (var log in MyErrorLogs)
                        {
                            await e.User.SendMessage($"[{log.Date}] [{log.User}] [Error] -- {log.Logged}");
                        }
                });

            cmdService.CreateCommand("requestlog")
                .Description("Receive a PM with the current list of requested features and changes")
                .AddCheck((c, u, ch) => Settings.Channels.Contains(ch.Id))
                .Do(async (e) =>
                {
                        foreach (var log in MyRequestLogs)
                        {
                            await e.User.SendMessage($"[{log.Date}] [{log.User}] [Request] -- {log.Logged}");
                        }
                });

            cmdService.CreateCommand("about")
                .Description("About this bot")
                .Do(async (e) =>
                {
                    await e.User.SendMessage($"This bot was created by -- ***xUnholy***");
                });

            cmdService.CreateCommand("kick")
                .Parameter("UserToKick", ParameterType.Required)
                .AddCheck((c, u, ch) => u.GetPermissions(ch).ManageChannel)
                .Do(async (e) =>
                {
                    var _user = e.Server.FindUsers(e.GetArg("UserToKick")).FirstOrDefault();
                    if (_user != null)
                    {
                        await _user.Kick();
                        await e.User.SendMessage($"Kicked: " + _user);
                    }
                });

            cmdService.CreateCommand("getchan")
                .AddCheck((c, u, ch) => u.GetPermissions(ch).ManageChannel)
                .Do(async (e) =>
                {
                    string chanId = e.Channel.Id.ToString();
                    await e.User.SendMessage("Channel ID for #" + e.Channel + " is " + chanId);
                });

            cmdService.CreateCommand("donate")
                .AddCheck((c, u, ch) => u.GetPermissions(ch).ManageChannel)
                .Parameter("DonationUser",ParameterType.Required)
                .Do(async (e) =>
                {
                    var _user = e.Server.FindUsers(e.GetArg("DonationUser")).FirstOrDefault().Name;
                    if (_user != null)
                        await e.User.SendMessage($"Hi, thanks for asking about donations!\n" +
                                             $"Please PM {e.Server.FindUsers("Mysticales").FirstOrDefault().Name} for details on how you can support us." +
                                             $"\n ~Ethereal Staff");
                });

            cmdService.CreateCommand("dump")
              .Description("Create a local dump of collected error and request logs")
              .AddCheck((c, u, ch) => u.ServerPermissions.ManageChannels)
              .Do(async (e) =>
              {
                    string dumpDirectory = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory) + @"\DumpErrors.txt";
                    using (var file = new StreamWriter(dumpDirectory, true))
                    {
                        foreach (var log in MyErrorLogs)
                        {
                            file.WriteLine($"[{log.Date}] [{log.User}] -- {log.Logged}");
                        }
                        file.Close();
                    }
                    using (var file = new StreamWriter(dumpDirectory, true))
                    {
                        foreach (var log in MyRequestLogs)
                        {
                            file.WriteLine($"[{log.Date}] [{log.User}] -- {log.Logged}");
                        }
                        file.Close();
                    }
                    await e.User.SendMessage($"All error logs have been dumped.");
              });
        }

        public void Log(object sender, LogMessageEventArgs e)
        {
            Console.WriteLine($"[{e.Severity}] [{e.Source}] {e.Message}");
        }
    }
}