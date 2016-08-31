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
            _client = new DiscordClient(x =>
            {
                x.AppName = "Ethereal";
                x.AppUrl = "https://discordapp.com/oauth2/authorize?client_id=220396497009770497&scope=bot&permissions=0";
                x.LogLevel = LogSeverity.Info;
                x.LogHandler = Log;
            });

            _client.UsingCommands(x =>
            {
                x.PrefixChar = '!';
                x.AllowMentionPrefix = true;
                x.HelpMode = HelpMode.Public;
            });

            var token = "<Enter Token>"; 

            CreateCommands();

            _client.ExecuteAndWait(async () =>
            {
                await _client.Connect(token);
            });
        }

        public void CreateCommands()
        {
            var cService = _client.GetService<CommandService>();

            cService.CreateCommand("report")
                .Description("-- Report An Issue With Ethereal Bot")
                .Do(async (e) =>
                {
                    await e.Channel.SendMessage($"{e.User.Mention} : Thank you for your report!");
                    MyErrorLogs.Add(new Logs()
                    {
                        Time = $"{DateTime.Now.Date.ToString($"dd/MM/yyyy")}",
                        User = $"{e.User}",
                        Logged = $"{e.Message.Text}"
                    });
                });

            cService.CreateCommand("request")
                .Description("Request A Feature In Ethereal Bot")
                .Parameter("RequestedMessage", ParameterType.Unparsed)  //how to make this accept "!request bla bla"
                .Do(async (e) =>
                {
                        await e.Channel.SendMessage($"{e.User.Mention} : Thank you for your request!");
                        MyRequestLogs.Add(new Logs()
                        {
                            Time = $"{DateTime.Now.Date.ToString($"dd/MM/yyyy")}",
                            User = $"{e.User}",
                            Logged = $"{e.Message.Text}"
                        });                 
                });

            cService.CreateCommand("reportlog")
                .Description("This Will List All Logged Errors")
                .Do(async (e) =>
                {   
                    foreach (var log in MyErrorLogs)
                    {
                        await e.User.SendMessage($"{log.Time + " *" + log.User +"* **Error** -- "+ log.Logged}");
                    }
                     
                });

            cService.CreateCommand("requestlog")
                .Description("This will list all logged requests")
                .Do(async (e) =>
                {
                    foreach (var log in MyRequestLogs)
                    {
                        await e.User.SendMessage($"{log.Time + " *" + log.User + "* **Request** -- " + log.Logged}");
                    }
                });

            cService.CreateCommand("about")
                .Description("About this bot")
                .Do(async (e) =>
                {
                    await e.User.SendMessage($"This bot was created by -- ***xUnholy***");
                });

            cService.CreateCommand("dump")
              .Description("Dump Logs")
              .Do(async (e) =>
              {
                  var singleRole = e.Server.Roles.FirstOrDefault(x => x.Name == "Lead Developer");
                  if (e.User.HasRole(singleRole))
                  {
                      await e.User.SendMessage($"All error logs have been dumped");
                      string directory = @"c:\Users\Michael\Desktop\Dump.txt";
                      using (StreamWriter file = new StreamWriter($"{directory }"))
                      {
                          foreach (var log in MyErrorLogs)
                          {
                              file.WriteLine($"{log.Time}  {log.User} -- {log.Logged}");
                          }
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
