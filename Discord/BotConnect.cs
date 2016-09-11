using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord.Commands;
using Discord.Commands.Permissions.Levels;
using Discord.Enums;
using Discord.Modules;
using Discord.Extensions;

namespace Discord
{
    class BotConnect
    {
        private DiscordClient _client;
        private Settings _config;

        public BotConnect()
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

            _client.Log.Message += (s, e) => Console.WriteLine($"[{e.Severity}] [{e.Source}] [Bot] {e.Message}");

            _client.AddModule<Modules.Commands>("Commands", ModuleFilter.None);

            _client.UserJoined += _client_UserJoined;

            _client.UserUpdated += _client_UserUpdated;

            _client.RoleUpdated += _client_RoleUpdated;

            //_client.MessageReceived += _client_MsgReceived;

            _client.ExecuteAndWait(async () =>
            {
                while (true)
                {
                    try
                    {
                        await _client.Connect(_config.BotToken, TokenType.Bot);
                        _client.SetGame(_config.CurrentGame, GameType.Twitch, _config.DiscordUrl);
                        UserConnect userConnect = new UserConnect();
                        return;
                    }
                    catch (Exception ex)
                    {
                        _client.Log.Error("Failed to log in", ex);
                        await Task.Delay(_client.Config.FailedReconnectDelay);
                    }
                }
            });
        }

        public async void _client_RoleUpdated(object s, RoleUpdatedEventArgs e)
        {
            foreach (var role in e.After.Server.Roles)
                if (role.Name.ContainsAny("Donator"))
                {
                    await e.After.Client.GetChannel(_config.Channels["donators_chat"]).SendMessage(
                    $"Welcome to {e.Server.GetChannel(_config.Channels["donators_chat"]).Mention}, {e.After.Mention}!");
                }
        }

        public async void _client_UserUpdated(object s, UserUpdatedEventArgs e)
        {
            if (e.Before.Status.Value == "offline" &&
                e.After.Status.Value == "online" &&
                e.After.ServerPermissions.ManageChannels == true)
            {
                await e.After.Server.GetChannel(_config.Channels["general"]).SendMessage($"Welcome back {e.After.Mention}!");
            }
        }

        public async void _client_UserJoined(object s, UserEventArgs e)
        {
            await e.User.SendMessage($"Welcome {e.User.Name} to the Ethereal Bot Discord Server!" + Environment.NewLine +
                                     $"Please check out {e.Server.GetChannel(_config.Channels["announcements"]).Mention} and " +
                                     $"{e.Server.GetChannel(_config.Channels["readme"]).Mention}");
        }

        public async void _client_MsgReceived(object s, MessageEventArgs e)
        {
                ulong server = e.Server.Id;
                ulong chan = e.Channel.Id;
                ulong user = e.User.Id;
                var msg = e.Message.Text;

                Console.WriteLine(msg);
        }

        private Permissions GetPermission(User u, Channel c)
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
