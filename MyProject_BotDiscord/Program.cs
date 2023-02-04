using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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
            client = new DiscordSocketClient(new DiscordSocketConfig
            {
                LogLevel = LogSeverity.Debug,
                GatewayIntents = GatewayIntents.All
            });

            commands = new CommandService();

            client.Log += Log;

            client.Ready += () =>
            {
                Console.WriteLine("Je suis prêt");
                return Task.CompletedTask;
            };

            await InstallCommandsAsync();

            await client.LoginAsync(TokenType.Bot, Environment.GetEnvironmentVariable("DiscordToken",EnvironmentVariableTarget.User));
            await client.StartAsync();

            await Task.Delay(-1);
        }

        public async Task InstallCommandsAsync()
        {
            client.MessageReceived += HandleCommandAsync;
            await commands.AddModulesAsync(Assembly.GetEntryAssembly(), null);
        }

        private async Task HandleCommandAsync(SocketMessage pMessage)
        {
            var message = pMessage as SocketUserMessage;

            if (message == null) return;

            int argPos = 0; // Definition du ! comme commande
            if (!message.HasCharPrefix('!', ref argPos)) return;

            var context = new SocketCommandContext(client, message);

            var result = await commands.ExecuteAsync(context, argPos, null);

            // Si il y a une erreur dans la commande alors envoyer un message d'erreur
            if (!result.IsSuccess) 
                await context.Channel.SendMessageAsync(result.ErrorReason);

        }

        private Task Log(LogMessage arg)
        {
            Console.WriteLine(arg.ToString());
            return Task.CompletedTask;
        }
    }
}
