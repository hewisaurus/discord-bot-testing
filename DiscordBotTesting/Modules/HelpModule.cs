using System.Threading.Tasks;
using Discord;
using Discord.Commands;

namespace DiscordBotTesting.Modules
{
    public class HelpModule : ModuleBase<SocketCommandContext>
    {
        [Command("help")]
        public async Task HelpAsync()
        {
            var embed = new EmbedBuilder()
                .WithDescription("Bot commands ~~for those that have no idea~~")
                .WithColor(new Color(0xE55B30))
                .WithAuthor(author => {
                    author
                        .WithName("bot stuff")
                        .WithUrl("https://discordapp.com")
                        .WithIconUrl("https://cdn.discordapp.com/embed/avatars/0.png");
                })
                .AddField("Role management", "!role [arguments]")
                .AddField("List current roles", "`!role list`", true)
                .AddField("Join a role", "`!role add [rolename]`", true)
                .AddField("Leave a role", "`!role leave [rolename]`", true)
                .Build();
            await ReplyAsync(embed: embed);
        }
    }
}
