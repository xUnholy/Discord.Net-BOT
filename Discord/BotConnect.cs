using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord.Commands;
using Discord.Commands.Permissions.Levels;

namespace Discord
{
    class BotConnect
    {
        public string Token = "<Token>";

        public List<Logs> MyErrorLogs = new List<Logs>();
        public List<Logs> MyRequestLogs = new List<Logs>();


        private DiscordClient _client;
        public BotConnect()
        {
            _client = new DiscordClient(x =>
            {
                x.AppName = "Ethereal";
                x.AppUrl = "https://discordapp.com/oauth2/authorize?client_id=220065941067792384&scope=bot&permissions=0";
                x.LogLevel = LogSeverity.Info;
                x.LogHandler = Log;
            });

            _client.UsingCommands(x =>
            {
                x.PrefixChar = '!';
                x.AllowMentionPrefix = true;
                x.HelpMode = HelpMode.Private;
            });

            //Welcome new users to the server
            _client.UserJoined += _client_UserJoined; 

            //Check if a user is staff, were offline, and now online, and greet them
            _client.UserUpdated += _client_UserOnline;
 
            CreateCommands();

            _client.ExecuteAndWait(async () =>
            {
                await _client.Connect(Token);
            });
        }

        private static async void _client_UserOnline(object sender, UserUpdatedEventArgs e)
        {
            var isStaff = false;
            var userRoles = e.After.Roles;
            foreach (var role in userRoles)
            {
                if (role.Name.Contains("Lead Developer") || role.Name.Contains("Fearless Leader"))
                {
                    isStaff = true;
                }
            }
            if (e.Before.Status.Value == "offline" && e.After.Status.Value == "online" && isStaff)
            {
                await e.After.SendMessage($"Welcome {e.After.Name}!");
            }
        }

        private static async void _client_UserJoined(object sender, UserEventArgs e)
        {
            await e.User.SendMessage($"Welcome {e.User.Mention} to the Ethereal Bot Discord Server!" +
                                     "\n" +
                                     "\nPlease check out #announcements and #readme");

        }

        public void CreateCommands()
        {
            var cService = _client.GetService<CommandService>();

            cService.CreateCommand("report")
                .Description("-- Report an issue with Ethereal Bot")
                .AddCheck((c, u, ch) => ch.Id == 210512518076956673)
                .Parameter("RequestedMessage", ParameterType.Unparsed)
                .Do(async (e) =>
                {
                    await e.Channel.SendMessage($"{e.User.Mention} Thank you for your report!");
                    MyErrorLogs.Add(new Logs()
                    {
                        Date = $"{DateTime.Now.Date.ToString($"dd/MM/yyyy")}",
                        User = $"{e.User}",
                        Logged = $"{e.GetArg("RequestedMessage")}"
                    });
                });

            cService.CreateCommand("request")
                .Description("Use this function to request a new feature or modification to Ethereal Bot")
                .AddCheck((c, u, ch) => ch.Id == 210512518076956673)
                .Parameter("RequestedMessage", ParameterType.Unparsed)
                .Do(async (e) =>
                {
                    await e.Channel.SendMessage($"{e.User.Mention} Thank you for your request!");
                    MyRequestLogs.Add(new Logs()
                    {
                        Date = $"{DateTime.Now.Date.ToString($"dd/MM/yyyy")}",
                        User = $"{e.User}",
                        Logged = $"{e.GetArg("RequestedMessage")}"
                    });
                });

            cService.CreateCommand("reportlog")
                .Description("Receive a PM with the current list of reported bugs & errors")
                .AddCheck((c, u, ch) => ch.Id == 210512518076956673 || ch.Id == 210512518076956673)
                .Do(async (e) =>
                {
                    if (e.Channel.Id == 210512518076956673)
                    {
                        foreach (var log in MyErrorLogs)
                        {
                            await e.Channel.SendMessage($"[{log.Date}] [*{log.User}*] [**Error**] -- {log.Logged}");
                        }
                    }
                    else
                    {
                        foreach (var log in MyErrorLogs)
                        {
                            await e.User.SendMessage($"[{log.Date}] [*{log.User}*] [**Error**] -- {log.Logged}");
                        }
                    }
                });

            cService.CreateCommand("requestlog")
                .Description("Receive a PM with the current list of requested features and changes")
                .AddCheck((c, u, ch) => ch.Id == 210512518076956673 || ch.Id == 220286522543308801)
                .Do(async (e) =>
                {
                    if (e.Channel.Id == 210512518076956673)
                    {
                        foreach (var log in MyRequestLogs)
                        {
                            await e.Channel.SendMessage($"[{log.Date}] [*{log.User}*] [**Error**] -- {log.Logged}");
                        }
                    }
                    else
                    {
                        foreach (var log in MyRequestLogs)
                        {
                            await e.User.SendMessage($"[{log.Date}] [*{log.User}*] [**Error**] -- {log.Logged}");
                        }
                    }
                });

            cService.CreateCommand("about")
                .Description("About this bot")
                .AddCheck((c, u, ch) => ch.Id == 210512518076956673)
                .Do(async (e) =>
                {
                    await e.User.SendMessage($"This bot was created by -- ***xUnholy***");
                });

            cService.CreateCommand("dump")
              .Description("Dump Logs")
              .AddCheck((c, u, ch) => ch.Id == 210512518076956673)
              .Do(async (e) =>
              {
                  var singleRole = e.Server.Roles.FirstOrDefault(x => x.Name == "Lead Developer");
                  var secondRole = e.Server.Roles.FirstOrDefault(x => x.Name == "Fearless Leader");
                  if (e.User.HasRole(singleRole) || e.User.HasRole(secondRole))
                  {
                      await e.User.SendMessage($"All error logs have been dumped");

                      const string errorDirectory = @"c:\Users\Michael\Desktop\DumpErrors.txt";
                      using (var file = new StreamWriter($"{errorDirectory }"))
                      {
                          foreach (var log in MyErrorLogs)
                          {
                              file.WriteLine($"[{log.Date}]  [{log.User}] -- [{log.Logged}]");
                          }
                          file.Close();
                      }
                      const string requestDirectory = @"c:\Users\Michael\Desktop\DumpErrors.txt";
                      using (var file = new StreamWriter($"{requestDirectory }"))
                      {
                          foreach (var log in MyRequestLogs)
                          {
                              file.WriteLine($"[{log.Date}]  [{log.User}] -- {log.Logged}");
                          }
                          file.Close();
                      }
                  }
              });
        }

        public void Log(object sender, LogMessageEventArgs e)
        {
            Console.WriteLine($"[{e.Severity}] [{e.Source}] {e.Message}");
        }
    }
}
