using Discord;
using Discord.Commands;
using Discord.WebSocket;
using DiscordBotTesting.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using DiscordBotTesting.Handlers;

namespace DiscordBotTesting
{
    class Program
    {
        public static void Main(string[] args)
            => new Program().MainAsync().GetAwaiter().GetResult();

        public async Task MainAsync()
        {
            using (var services = ConfigureServices())
            {
                // var _config = new DiscordSocketConfig { MessageCacheSize = 100 };
                var client = services.GetRequiredService<DiscordSocketClient>();
                

                client.Log += LogAsync;

                client.ReactionAdded += ReactionHandler.ClientOnReactionAdded;

                services.GetRequiredService<CommandService>().Log += LogAsync;

                // Get the token from config (appsettings.json)
                await client.LoginAsync(TokenType.Bot, services.GetRequiredService<IConfigurationRoot>()["DiscordToken"]);
                await client.StartAsync();

                // Here we initialize the logic required to register our commands.
                await services.GetRequiredService<CommandHandlingService>().InitializeAsync();

                await Task.Delay(Timeout.Infinite);
            }

        }

        

        private Task LogAsync(LogMessage log)
        {
            Console.WriteLine(log.ToString());
            return Task.CompletedTask;
        }

        private ServiceProvider ConfigureServices()
        {
            // Build configuration
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetParent(AppContext.BaseDirectory).FullName)
                .AddJsonFile("appsettings.json", false)
                .Build();

            return new ServiceCollection()
                .AddSingleton<DiscordSocketClient>()
                .AddSingleton<CommandService>()
                .AddSingleton<CommandHandlingService>()
                .AddSingleton<IConfigurationRoot>(configuration)
                .BuildServiceProvider();
        }
    }
}
