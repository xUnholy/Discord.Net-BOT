using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord.Commands;
using Discord.Commands.Permissions.Levels;
using Discord.Enums;
using Discord.Modules;

namespace Discord
{
    class UserConnect
    {
        private DiscordClient _client;
        private Settings _config;

        public UserConnect()
        {
            _config = new Settings();
            _client = new DiscordClient(x =>

            {
                x.AppName = "EtherealClient";
                x.LogLevel = LogSeverity.Error;
            });
            

            _client.Log.Message += (s, e) => Console.WriteLine($"[{e.Severity}] [{e.Source}] [User] {e.Message}");
            
            _client.ExecuteAndWait(async () =>
            {
                while (true)
                {
                    try
                    {
                        await _client.Connect(_config.UserToken, TokenType.User);
                        _client.SetGame(_config.CurrentGame, GameType.Twitch, _config.DiscordUrl);
                        break;
                    }
                    catch (Exception ex)
                    {
                        _client.Log.Error($"Failed to login, retrying in {_client.Config.FailedReconnectDelay}", ex);
                        await Task.Delay(_client.Config.FailedReconnectDelay);
                    }
                }
            });
        }

        public static async void _client_MsgReceived(object s, MessageEventArgs e)
        {
            ulong server = e.Server.Id;
            ulong chan = e.Channel.Id;
            ulong user = e.User.Id;
            string msg = e.Message.Text;

            Console.WriteLine($"[{server}] [{chan}] [{user}] {msg}");
        }
    }
}
