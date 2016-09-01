using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Discord
{
    public enum PermissionLevel
    {
        Ignored = -1,
        User = 0,
        ChannelModerator, //Manage Messages (Channel)
        ChannelAdmin, //Manage Permissions (Channel)
        ServerModerator, //Manage Messages, Kick, Ban (Server)
        ServerAdmin, //Manage Roles (Server)
        ServerOwner, //Owner (Server)
        BotOwner //Bot Owner (Global)
    }
}
