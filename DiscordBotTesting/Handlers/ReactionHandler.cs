using Discord;
using Discord.WebSocket;
using DiscordBotTesting.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DiscordBotTesting.Helpers;
using Microsoft.VisualBasic;

namespace DiscordBotTesting.Handlers
{
    public static class ReactionHandler
    {
        private static Dictionary<string, string> TranslateEmoji = new Dictionary<string, string>
        {
            {new Emoji("\U0001f1e6").Name, ":regional_indicator_a:"},
            {new Emoji("\U0001f1e7").Name, ":regional_indicator_b:"},
            {new Emoji("\U0001f1e8").Name, ":regional_indicator_c:"},
            {new Emoji("\U0001f1e9").Name, ":regional_indicator_d:"},
            {new Emoji("\U0001f1ea").Name, ":regional_indicator_e:"},
            {new Emoji("\U0001f1eb").Name, ":regional_indicator_f:"},
            {new Emoji("\U0001f1ec").Name, ":regional_indicator_g:"},
            {new Emoji("\U0001f1ed").Name, ":regional_indicator_h:"},
            {new Emoji("\U0001f1ee").Name, ":regional_indicator_i:"},
            {new Emoji("\U0001f1ef").Name, ":regional_indicator_j:"},
            {new Emoji("\U0001f1f0").Name, ":regional_indicator_k:"},
            {new Emoji("\U0001f1f1").Name, ":regional_indicator_l:"},
            {new Emoji("\U0001f1f2").Name, ":regional_indicator_m:"},
            {new Emoji("\U0001f1f3").Name, ":regional_indicator_n:"},
            {new Emoji("\U0001f1f4").Name, ":regional_indicator_o:"},
            {new Emoji("\U0001f1f5").Name, ":regional_indicator_p:"},
            {new Emoji("\U0001f1f6").Name, ":regional_indicator_q:"},
            {new Emoji("\U0001f1f7").Name, ":regional_indicator_r:"},
            {new Emoji("\U0001f1f8").Name, ":regional_indicator_s:"},
            {new Emoji("\U0001f1f9").Name, ":regional_indicator_t:"},
            {new Emoji("\U0001f1fa").Name, ":regional_indicator_u:"},
            {new Emoji("\U0001f1fb").Name, ":regional_indicator_v:"},
            {new Emoji("\U0001f1fc").Name, ":regional_indicator_w:"},
            {new Emoji("\U0001f1fd").Name, ":regional_indicator_x:"},
            {new Emoji("\U0001f1fe").Name, ":regional_indicator_y:"},
            {new Emoji("\U0001f1ff").Name, ":regional_indicator_z:"},
        };

        public static async Task ClientOnReactionAdded(Cacheable<IUserMessage, ulong> arg1, ISocketMessageChannel arg2, SocketReaction arg3)
        {
            // Get the message that was reacted to
            var msg = await arg1.DownloadAsync();

            // Ignore the reaction if the message wasn't posted by a bot
            if (msg.Source != MessageSource.Bot)
            {
                return;
            }

            // Verify permissions - must be in this role for reactions to be parsed
            if (!((SocketGuildUser) msg.Author).IsInRole("RoleBasedReactions"))
            {
                return;
            }
            
            // Do we recognise the emote/emoji?
            var translated = "";
            if (TranslateEmoji.ContainsKey(arg3.Emote.Name))
            {
                translated = TranslateEmoji[arg3.Emote.Name];
            }
            else
            {
                Console.WriteLine($"Unrecognised reaction: {arg3.Emote.Name}");
                return;
            }

            // Sanity check - make sure there is an embed in the message, that it has fields and found a match
            if (msg.Embeds.Count == 0 
                || msg.Embeds.First().Fields.Length == 0
                || msg.Embeds.First().Fields.All(f => f.Name.Contains(translated) == false))
            {
                return;
            }


            var matchingField = msg.Embeds.First().Fields.FirstOrDefault(f => f.Name.Contains(translated));

            // Sanity check - make sure there's an @ in the field name, so that we can split on it.
            // This is added by the embed builder so it should be there, but check just in case someone borks something.
            if (!matchingField.Name.Contains("@"))
            {
                return;
            }

            var roleToAdd = matchingField.Name.Split("@")[1];
            var addResult = await RoleHelper.AddRole(roleToAdd, ((SocketTextChannel) msg.Channel).Guild,
                (SocketGuildUser) arg3.User.Value);
            if (addResult.Success == false)
            {
                Console.WriteLine($"Failed to add role.. {addResult.Message}");
            }
        }
    }
}
