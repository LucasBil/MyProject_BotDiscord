using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;

namespace MyProject_BotDiscord
{
    class Program
    {
        private DiscordSocketClient client;
        private CommandService commands;

        static void Main(string[] args) => new Program().RunBotAsync().GetAwaiter().GetResult();

        public async Task RunBotAsync()
        {
            client = new DiscordSocketClient();

            client.Ready += () =>
            {
                Console.WriteLine("Je suis prêt");
                return Task.CompletedTask;
            };

            await client.LoginAsync(TokenType.Bot, Environment.GetEnvironmentVariable("DiscordToken",EnvironmentVariableTarget.User));
            await client.StartAsync();

            await Task.Delay(-1);
        }
    }
}
