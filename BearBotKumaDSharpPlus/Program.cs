using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.Net;
using DSharpPlus.Interactivity;
using DSharpPlus.Net.WebSocket;
using DSharpPlus.VoiceNext;
using System.IO;


namespace BearBotKumaDSharpPlus
{
    class Program
    {

        static void Main(string[] args)
        {
          MainAsync(args).ConfigureAwait(false).GetAwaiter().GetResult();

        }

        private static DiscordClient _client;
        private static CommandsNextModule commandHandler;
        private static string botToken = "MzUyNzIxNTkzNDg4NjM3OTUy.DJE4rA.Imdq9z - OTL3wyaVwKx73jhnVjXo";
        private static InteractivityModule interactions;
        private static VoiceNextClient vClient;
        private static StreamReader fileReader = null;

        public static async Task MainAsync(string[] args)
        {
            _client = new DiscordClient(new DiscordConfiguration
            {
                TokenType = TokenType.Bot,
                Token = botToken,
                LogLevel = LogLevel.Debug
            });
            _client.SetWebSocketClient <WebSocket4NetClient>();


            _client.DebugLogger.LogMessageReceived += LogMessage;

            commandHandler = _client.UseCommandsNext(new CommandsNextConfiguration
            {
                CaseSensitive = false,
                StringPrefix = "!"
            });
            commandHandler.RegisterCommands<BotCommandsModule>();

            interactions = _client.UseInteractivity();
            vClient = _client.UseVoiceNext();
            string line;
            try
            {
                fileReader = new StreamReader("tsapikey.ini");
            }
            catch
            {
                Console.WriteLine("tsapikey.ini is missing!");
            }
            while ((line = fileReader.ReadLine()) != null)
            {
                if (line.Contains("TSapikey"))
                {
                    BotCommandsModule.apiKey = line.Replace("TSapikey = ","").Replace("\"","");
                    if (BotCommandsModule.apiKey == null || BotCommandsModule.apiKey == "")
                    {
                        Console.WriteLine("You need to enter your TS3 Client Query API Key in the tsapikey.ini file!");
                        Console.ReadKey();
                    }
                }
            }

            
            await _client.ConnectAsync();

            await Task.Delay(-1);
        }

        public static void LogMessage(object sender, EventArgs e)
        {
            Console.WriteLine(e.ToString());
            //return Task.CompletedTask;
        }

    }
}
