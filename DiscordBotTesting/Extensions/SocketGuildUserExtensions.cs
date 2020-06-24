using Discord.WebSocket;
using System;
using System.Linq;

namespace DiscordBotTesting.Extensions
{
    public static class SocketGuildUserExtensions
    {
        public static bool IsInRole(this SocketGuildUser user, string roleName)
        {
            return user.Roles.Any(r => String.Equals(r.Name, roleName, StringComparison.CurrentCultureIgnoreCase));
        }
    }
}
