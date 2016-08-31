using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord.Commands;

namespace Discord
{
    class BotConnect
    {
        public List<Logs> MyErrorLogs = new List<Logs>();
        public List<Logs> MyRequestLogs = new List<Logs>();

        private DiscordClient _client;

        public BotConnect()
        {
            var botToken = "MjIwNTAzODU2MjA1OTIyMzA0.CqhPzA.qpdc9mkxOJLrhpICJ0QMzTGKx6A";

            _client = new DiscordClient(x =>
            {
                x.AppName = "Ethereal Discord Bot";
                x.AppUrl = "https://discordapp.com/oauth2/authorize?client_id=220503856205922304&scope=bot&permissions=0";
                x.LogLevel = LogSeverity.Info;
                x.LogHandler = Log;
            });

            _client.UsingCommands(x =>
            {
                x.PrefixChar = '!';
                x.AllowMentionPrefix = true;
                x.HelpMode = HelpMode.Public;
            });

            CreateCommands();

            _client.ExecuteAndWait(async () =>
            {
                await _client.Connect(botToken);
            });
        }

        public void CreateCommands()
        {
            var cService = _client.GetService<CommandService>();

            cService.CreateCommand("report")
                .Description("Use this function to report an error or bug with Ethereal Bot")
                .Parameter("ReportedMessage", ParameterType.Unparsed)
                .Do(async (e) =>
                {
                    await e.Channel.SendMessage($"{e.User.Mention} Thank you for your report!");
                    MyErrorLogs.Add(new Logs()
                    {
                        Date = $"{DateTime.Now.Date.ToString($"dd.MM.yy")}",
                        User = $"{e.User}",
                        Logged = $"{e.GetArg("ReportedMessage")}"
                    });
                });

            cService.CreateCommand("request")
                .Description("Use this function to request a new feature or modification to Ethereal Bot")
                .Parameter("RequestedMessage", ParameterType.Unparsed)
                .Do(async (e) =>
                {
                    await e.Channel.SendMessage($"{e.User.Mention} Thank you for your request!");
                    MyRequestLogs.Add(new Logs()
                    {
                        Date = $"{DateTime.Now.Date.ToString($"dd.MM.yy")}",
                        User = $"{e.User}",
                        Logged = $"{e.GetArg("RequestedMessage")}"
                    });                 
                });

            cService.CreateCommand("reportlog")
                .Description("Receive a PM with the current list of reported bugs & errors")
                .Do(async (e) =>
                {
                    foreach (var log in MyErrorLogs)
                    {
                        await e.User.SendMessage($"[{log.Date + "] [" + log.User +"] [Error] -- "+ log.Logged}");
                    }
                });

            cService.CreateCommand("requestlog")
                .Description("Receive a PM with the current list of requested features and changes")
                .Do(async (e) =>
                {
                    foreach (var log in MyRequestLogs)
                    {
                        await e.User.SendMessage($"[{log.Date + "] [" + log.User + "] [Request] -- " + log.Logged}");
                    }
                });

            cService.CreateCommand("about")
                .Description("About this bot")
                .Do(async (e) =>
                {
                    await e.User.SendMessage($"This bot was created by -- ***xUnholy***");
                });

            cService.CreateCommand("dump")
                .Description("Create a local dump of all logs gathered by the bot")
                .Do(async (e) =>
                {
                    var singleRole = e.Server.Roles.FirstOrDefault(x => x.Name == "Lead Developer");
                    if (e.User.HasRole(singleRole))
                    {
                        await e.User.SendMessage($"All logs have been dumped in " + Environment.CurrentDirectory);
                        string directory = (Environment.CurrentDirectory + @"\ErrorLog.txt");
                        using (StreamWriter sw = new StreamWriter(directory))
                        {
                            foreach (var log in MyErrorLogs)
                            {
                                sw.WriteLine($"[{log.Date}] [{log.User}] [Error] -- {log.Logged}");
                            }
                            sw.Close();
                        }
                        directory = (Environment.CurrentDirectory + @"\RequestLog.txt");
                        using (StreamWriter sw = new StreamWriter(directory))
                        {
                            foreach (var log in MyRequestLogs)
                            {
                                sw.WriteLine($"[{log.Date}] [{log.User}] [Request] -- {log.Logged}");
                            }
                            sw.Close();
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
