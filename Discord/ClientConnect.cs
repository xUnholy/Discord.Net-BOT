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
    class ClientConnect
    {
        private DiscordClient _client;
        private Settings _config;
        public ClientConnect()
        {
            _config = new Settings();

            _client = new DiscordClient(x =>

            {
                x.AppName = "Ethereal";
                x.LogLevel = LogSeverity.Info;
            })
            .UsingCommands(x =>
            {
                x.PrefixChar = _config.Prefix;
                x.HelpMode = HelpMode.Public;
            })
            .UsingPermissionLevels((u, c) => (int)GetPermission(u, c))
            .UsingModules();

            _client.Log.Message += (s, e) => Console.WriteLine($"[{e.Severity}] [{e.Source}] {e.Message}");

            _client.AddModule<Modules.Commands>("Commands", ModuleFilter.None);

            //_client.UserJoined += _client_UserJoined;

            //_client.UserUpdated += _client_UserUpdated;

            //_client.MessageReceived += _client_MsgReceived;

            _client.ExecuteAndWait(async () =>
            {
                while (true)
                {
                    try
                    {
                        await _client.Connect(_config.ClientToken, TokenType.User); //TODO: Throwing Client Login Here
                        _client.SetGame(_config.CurrentGame, GameType.Twitch, _config.DiscordUrl);
                        Console.WriteLine("Client Login succesful");
                        break;
                    }
                    catch (Exception ex)
                    {
                        _client.Log.Error("Failed Client login", ex);
                        await Task.Delay(_client.Config.FailedReconnectDelay);
                    }
                }
            });
        }
        private Permissions GetPermission(User u, Channel c) //TODO: Create this into its own class
        {
            if (u.IsBot)
                return Permissions.Blocked;

            if (_config.Staff.Contains(u.Id))
                return Permissions.BotOwner;

            if (!c.IsPrivate)
            {
                if (u == c.Server.Owner)
                    return Permissions.ServerOwner;

                if (u.ServerPermissions.Administrator)
                    return Permissions.ServerAdmin;

                if (u.GetPermissions(c).ManageChannel)
                    return Permissions.ChannelAdmin;
            }

            return Permissions.User;
        }
    }
}
