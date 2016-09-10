using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord.Commands;
using Discord.Commands.Permissions.Levels;
using Discord.Modules;
using Discord.Enums;

namespace Discord
{
    class Program
    {
        static void Main(string[] args) => new Program().BotConnect();

        private DiscordClient _client;
        private Settings _config;

        public void BotConnect()
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
                        await _client.Connect(_config.Token, TokenType.User);
                        _client.SetGame(_config.CurrentGame, GameType.Twitch, _config.DiscordURL);
                        break;
                    }
                    catch (Exception ex)
                    {
                        _client.Log.Error("Failed to log in", ex);
                        await Task.Delay(_client.Config.FailedReconnectDelay);
                    }
                }
            });
        }

        private async void _client_UserUpdated(object s, UserUpdatedEventArgs e)
        {
            /* if (e.After.GetPermissions(e.get))
             {
                 await e.After.Client.GetChannel(Settings.Channels["donators_chat"]).SendMessage(   TODO: Welcome new donators
                     @"Welcome to @Donators, " + $"{e.After.Mention}!");
             }*/

            if (e.Before.Status.Value == "offline" &&
                e.After.Status.Value == "online" &&
                e.After.ServerPermissions.ManageChannels == true)
            {
                await e.After.Server.GetChannel(Settings.Channels["general"]).SendMessage(
                                                 $"Welcome back {e.After.Mention}!");
            }
        }

        private async void _client_UserJoined(object s, UserEventArgs e)
        {
            await e.User.SendMessage($"Welcome {e.User.Name} to the Ethereal Bot Discord Server!" + Environment.NewLine +
                                     $"Please check out {e.Server.GetChannel(Settings.Channels["announcements"]).Mention} and " + 
                                     $"{e.Server.GetChannel(Settings.Channels["readme"]).Mention}");
        }

        /*private async void _client_MsgReceived(object s, MessageEventArgs e)
        {
            if (!e.Message.IsAuthor)
            {
                ulong server = e.Server.Id;
                ulong chan = e.Channel.Id;
                ulong user = e.User.Id;
                var msg = e.Message.Text;

                Console.WriteLine(msg);
            }
        }*/

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
