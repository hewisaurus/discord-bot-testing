using Discord.Commands;
using Discord.WebSocket;
using DiscordBotTesting.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DiscordBotTesting.Helpers
{
    public static class RoleHelper
    {
        public static List<string> ProtectedRoles = new List<string>
        {
            "SomeProtectedRole",
            "SomeAdminRole"
        };

        public static SocketRole GetRole(string roleName, SocketGuild guild)
        {
            return guild.Roles.FirstOrDefault(r => String.Equals(r.Name, roleName, StringComparison.CurrentCultureIgnoreCase));
        }


        public static async Task<ReturnValue> AddRole(string roleName, SocketGuild guild, SocketGuildUser user)
        {
            if (user.IsInRole(roleName))
            {
                // Already a member
                return new ReturnValue
                {
                    Success = false,
                    Message = $"Already a member of {roleName}"
                };
            }

            // Check whether it's protected (don't let users add themselves to these)
            if (ProtectedRoles.Any(r => r.ToLower() == roleName))
            {
                return new ReturnValue
                {
                    Success = false,
                    Message = "Sorry, you can't automatically add yourself to that role."
                };
            }

            // Does it exist?
            var specifiedRole = GetRole(roleName, guild);
            if (specifiedRole == null)
            {
                return new ReturnValue
                {
                    Success = false,
                    Message = $"The role {roleName} does not exist on this server."
                };
            }

            try
            {
                // Alright, go and add it
                await user.AddRoleAsync(specifiedRole);

                return new ReturnValue
                {
                    Success = true,
                    Message = "Role added successfully"
                };
            }
            catch (Exception ex)
            {
                // This exception is probably the fact that the role can't be added to (i.e. managed by Discord, like one that's only used for bots)
                return new ReturnValue
                {
                    Success = false,
                    Message = "Sorry, you can't automatically add yourself to that role."
                };
            }
        }
    }
}
