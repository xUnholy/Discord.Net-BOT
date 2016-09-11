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
    public class Program
    {
        public static void Main(string[] args)
        {
           BotConnect bot = new BotConnect(); 
           ClientConnect clientBot = new ClientConnect(); //TODO: Make client login function with bot client.
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
    }
}