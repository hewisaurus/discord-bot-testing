using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DiscordBotTesting.Modules
{
    public class RoleModule : ModuleBase<SocketCommandContext>
    {
        #region Commands

        [Command("role")]
        public async Task RoleAsync(params string[] paramList)
        {
            if (paramList.Length == 0)
            {
                await ReplyAsync(ErrorMessages.Role.NoArguments);
                return;
            }

            switch (paramList[0])
            {
                case "list":
                    var roles = ((SocketGuildUser)Context.User).Roles.Select(r => r.Name.Replace("@", ""));
                    await ReplyAsync($"You're a member of these roles: {string.Join(", ", roles)}");
                    break;
                case "add":
                    if (paramList.Length != 2)
                    {
                        await ReplyAsync(ErrorMessages.Role.AddArgumentInvalid);
                    }
                    else
                    {
                        var addResult = await AddRole(paramList[1]);
                        await ReplyAsync(addResult.Message);
                    }
                    break;
                case "leave":
                    if (paramList.Length != 2)
                    {
                        await ReplyAsync(ErrorMessages.Role.AddArgumentInvalid);
                    }
                    else
                    {
                        var leaveResult = await LeaveRole(paramList[1]);
                        await ReplyAsync(leaveResult.Message);
                    }
                    break;
                default:
                    await ReplyAsync("I have no idea what that command was supposed to do...");
                    break;
            }
        }

        #endregion

        #region Properties

        private static List<string> ProtectedRoles = new List<string>
        {
            "SomeProtectedRole",
            "SomeAdminRole"
        };


        #endregion

        #region Methods

        private async Task<ReturnValue> AddRole(string roleName)
        {
            // Force it to lowercase, cause case sensitivity is hard for some people
            roleName = roleName.ToLower();

            // Check whether the user is already a member of the role
            if (IsInRole(roleName))
            {
                return new ReturnValue
                {
                    Success = false,
                    Message = $"You're already a member of **{roleName}**. But you knew that, right?"
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
            var specifiedRole = Context.Guild.Roles.FirstOrDefault(r => r.Name.ToLower() == roleName);
            if (specifiedRole == null)
            {
                return new ReturnValue
                {
                    Success = false,
                    Message = $"The role **{roleName}** does not exist on this server."
                };
            }

            try
            {
                // Alright, go and add it
                await ((SocketGuildUser)Context.User).AddRoleAsync(specifiedRole);

                return new ReturnValue
                {
                    Success = true,
                    Message = $"Success! You now have the role **{roleName}**."
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

        private async Task<ReturnValue> LeaveRole(string roleName)
        {
            // Force it to lowercase, cause case sensitivity is hard for some people
            roleName = roleName.ToLower();

            if (roleName == "@everyone" || roleName == "@here")
            {
                return new ReturnValue
                {
                    Success = false,
                    Message = "You can't be removed from that role."
                };
            }

            // Check whether the user is already a member of the role
            if (!IsInRole(roleName))
            {
                return new ReturnValue
                {
                    Success = false,
                    Message = $"You can't leave a role you're not already a member of."
                };
            }

            // Does it exist?
            var specifiedRole = Context.Guild.Roles.FirstOrDefault(r => r.Name.ToLower() == roleName);
            if (specifiedRole == null)
            {
                return new ReturnValue
                {
                    Success = false,
                    Message = $"The role **{roleName}** does not exist on this server."
                };
            }

            try
            {
                // Alright, go and leave it
                await ((SocketGuildUser)Context.User).RemoveRoleAsync(specifiedRole);

                return new ReturnValue
                {
                    Success = true,
                    Message = $"You no longer have the role **{roleName}**."
                };
            }
            catch (Exception ex)
            {
                // Sopmething went wrong...
                return new ReturnValue
                {
                    Success = false,
                    Message = $"Something went wrong... {ex.Message}"
                };
            }
        }

        private bool IsInRole(string roleName)
        {
            return ((SocketGuildUser)Context.User).Roles.Any(r => r.Name.ToLower() == roleName);
        }
        #endregion
    }
}
