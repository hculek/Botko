using Botko.BotFiles.config;
using Discord;
using Discord.Net;
using Discord.WebSocket;
using Newtonsoft.Json;

namespace Botko.BotFiles
{
    public class Bot
    {
        private DiscordSocketClient _client;
        private ConfigJSON _configJSON;

        public async Task RunAsync() 
        {
            using (var fs = File.OpenRead("config.json"))
            {
                using (var sr = new StreamReader(fs))
                {
                    _configJSON = JsonConvert.DeserializeObject<ConfigJSON>(await sr.ReadToEndAsync().ConfigureAwait(false));
                }
            };

            _client = new DiscordSocketClient();
            
            _client.Log += Log;

            //user input or commands to bot
            _client.Ready += Client_Ready;
            _client.SlashCommandExecuted += SlashCommandHandler;


            await _client.LoginAsync(TokenType.Bot, _configJSON.Token);
            await _client.StartAsync();

            // Block this task until the program is closed.
            await Task.Delay(-1);
        }

        private async Task Client_Ready()
        {
            var globalCommand = new SlashCommandBuilder();
            globalCommand.WithName("test");
            globalCommand.WithDescription("test command");

            try
            {

                await _client.CreateGlobalApplicationCommandAsync(globalCommand.Build());
            }
            catch (ApplicationCommandException exception)
            {
                //TODO error loging
            }

        }

        private async Task SlashCommandHandler(SocketSlashCommand command)
        {
            await command.RespondAsync($"You executed {command.Data.Name}");
        }

        private Task Log(LogMessage msg)
        {
            //TODO error loging

            Console.WriteLine(msg.ToString());
            return Task.CompletedTask;
        }
    }
}
