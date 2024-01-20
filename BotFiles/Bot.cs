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
            using (var fs = File.OpenRead(Environment.CurrentDirectory + "\\BotFiles\\config\\config.json"))
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


        //Bot slash commands builder
        private async Task Client_Ready()
        {
            List<ApplicationCommandProperties> applicationCommandProperties = new();

            try
            {
                var testCommand = new SlashCommandBuilder();
                testCommand.WithName(Commands.test);
                testCommand.WithDescription("test command");
                applicationCommandProperties.Add(testCommand.Build());

                var quoteAddCommand = new SlashCommandBuilder();
                quoteAddCommand.WithName(Commands.quoteAdd);
                quoteAddCommand.WithDescription("example: 'quote add lorem ipsum dolor sit amet' to add quote");
                quoteAddCommand.AddOption("quote", ApplicationCommandOptionType.String, "example: 'quote add lorem ipsum dolor sit amet' to add quote", true, false, false, null, null, null, null, null, null, 1, null);
                applicationCommandProperties.Add(quoteAddCommand.Build());

                var quoteIdCommand = new SlashCommandBuilder();
                quoteIdCommand.WithName(Commands.quoteId);
                quoteIdCommand.WithDescription("example: 'quote-id 4' to find quote with id 4");
                quoteIdCommand.AddOption("id", ApplicationCommandOptionType.Integer, "example: 'quote-id 4' to find quote with id 4", true, false, false, null, null, null, null, null, null, 1, null);
                applicationCommandProperties.Add(quoteIdCommand.Build());

                var quoteFindCommand = new SlashCommandBuilder();
                quoteFindCommand.WithName(Commands.quoteFind);
                quoteFindCommand.WithDescription("example: 'quote find lorem' to find first quote containing phrase");
                quoteFindCommand.AddOption("keywords", ApplicationCommandOptionType.String, "example: 'quote find lorem' to find first quote containing phrase", true, false, false, null, null, null, null, null, null, 4, null);
                applicationCommandProperties.Add(quoteFindCommand.Build());

                var quoteDelCommand = new SlashCommandBuilder();
                quoteDelCommand.WithName(Commands.quoteDel);
                quoteDelCommand.WithDescription("example: 'quote-del 4' to delete quote with id 4");
                quoteDelCommand.AddOption("id", ApplicationCommandOptionType.Integer, "example: 'quote-del 4' to delete quote with id 4", true, false, false, null, null, null, null, null, null, 1, null);
                applicationCommandProperties.Add(quoteDelCommand.Build());

                var quoteRandCommand = new SlashCommandBuilder();
                quoteRandCommand.WithName(Commands.quoteRand);
                quoteRandCommand.WithDescription("posts one random quote");
                applicationCommandProperties.Add(quoteRandCommand.Build());

                //TODO implement
                //var quoteBullshitStartCommand = new SlashCommandBuilder();
                //quoteBullshitStartCommand.WithName("quote-bullshit-start");
                //quoteBullshitStartCommand.WithDescription("starts posting random quotes");

                //var quoteBullshitStopCommandCommand = new SlashCommandBuilder();
                //quoteBullshitStopCommandCommand.WithName("quote-bullshit-stop");
                //quoteBullshitStopCommandCommand.WithDescription("stops posting random quotes");

                await _client.BulkOverwriteGlobalApplicationCommandsAsync(applicationCommandProperties.ToArray());
            }
            catch (ApplicationCommandException exception)
            {
                var json = JsonConvert.SerializeObject(exception.Errors, Formatting.Indented);
                Console.WriteLine(json);
                //TODO error loging
            }

        }


        // Bot slash commands handler
        private async Task SlashCommandHandler(SocketSlashCommand command)
        {
            switch (command.Data.Name) 
            {
                case Commands.test:
                    await command.RespondAsync($"You executed {command.Data.Name} command");
                    break;
                case Commands.quoteId:
                    await command.RespondAsync($"You requested quote id {command.Data.Options.First().Value.ToString()} command");
                    break;

            }

        }

        private Task Log(LogMessage msg)
        {
            //TODO error loging

            Console.WriteLine(msg.ToString());
            return Task.CompletedTask;
        }
    }
}
