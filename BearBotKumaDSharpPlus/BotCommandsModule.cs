using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Interactivity;
using DSharpPlus.Entities;
using DSharpPlus.VoiceNext;
using MinTelnet;
using NAudio.Wave;
using System.IO;
using System.Diagnostics;
using GW2NET;
using GW2NET.DynamicEvents;


namespace BearBotKumaDSharpPlus
{
    public class BotCommandsModule
    {
        public static string server = "127.0.0.1";
        public static int serverPort = 25639;
        public static string apiKey = "";
        private static int accumMsgs = 0;
        private static DiscordEmbedBuilder embedBuilder;
        private static int joinUsed = 0;
        private static DiscordGuild currentGuild;
        private static VoiceNextConnection currentVConnect;
        private static int voiceBlockSize = 0;
        private static WaveFormat audioOutForm = new WaveFormat(48000, 16, WaveIn.GetCapabilities(0).Channels);
        private static BufferedWaveProvider bWP = new BufferedWaveProvider(audioOutForm);
        private static WaveInEvent srcStream;
        //private static List<String> activeBossList;


        [Command("hello"),
            Aliases("hi", "sup"),]
        public async Task RespondToHello(CommandContext Context)
        {
            if (Context.User.Id == 202215877360222208)
            {
                await Context.RespondAsync("Hello papa!");
            }
            else
            {
                await Context.RespondAsync("Hello my bearloved friend!");
            }
            InteractivityModule hiDialogue = Context.Client.GetInteractivityModule();
            var msg = await hiDialogue.WaitForMessageAsync(a => a.Author.Id == Context.User.Id && a.Content.ToLower() == "how are you" | a.Content.ToLower() == "how are you bear?" |
            a.Content.ToLower() == "how are you?", TimeSpan.FromMinutes(2));
            if (msg != null)
            {
                await Context.RespondAsync($"I'm wandering the forest, eating berries! Please tell me if you need anything.");
            }
        }

        [Command("whatcanyoudobear?"),
            Aliases("helpbear", "bearhelp", "bearcommands", "commands", "commands?")]
        public async Task RespondCanDo(CommandContext Context)
        {
            await Context.Channel.SendMessageAsync("Hi, I'm Bear. I can help relay voice from teamspeak, I can help you with Guild Wars 2 Info. I can even keep you company!");
            accumMsgs++;

        }

        [Command("choose"),
            Aliases("chooseonebear", "bearchooseone", "whichonebear", "whichonebear?", "whatshouldiplaybear", "whatshouldiplaybear?", "bearwhatshouldiplay?",
            "whichgameshouldiplaybear?", "whichgameshouldiplaybear", "pickonebear", "pick")]
        public async Task PickOne(CommandContext Context, params string[] list)
        {
            Random rng = new Random();
            for (int i = 0; i < list.Length; i++)
            {
                char[] charsToRemove = { '?', '.' };
                list[i] = list[i].Trim(charsToRemove);
            }
            GenRandom:
            int randomNum = rng.Next(0, list.Length);

            if (list.Length > 1)
            {
                if (list[randomNum].ToLower() != "or" && list[randomNum].ToLower() != "and")
                {
                    await Context.Channel.SendMessageAsync($"I choose {list[randomNum]}.");
                }
                else
                {
                    goto GenRandom;
                }
            }
            else
            {
                await Context.Channel.SendMessageAsync("You only gave me one choice :bear:");
            }


        }



        [Command("channels"),
            Aliases("channels?", "bearwhatchannelsarethere?"),
            Description("Command for retrieving the names and the channel IDs of channels on TS3 that are populated with users")]
        public async Task RetrieveChannels(CommandContext Context)
        {

            TelnetConnection telCon = new TelnetConnection(server, serverPort);
            string messageAmalg = "__**Channel/ConnectedUsers**__     __**Channel ID**__ \r\n";

            Console.WriteLine(telCon.Read());

            telCon.WriteLine($"auth apikey={apiKey}");

            telCon.WriteLine("whoami");
            string clientIdNeedParse = telCon.Read();
            string[] clientIdToTrim = clientIdNeedParse.Split(' ');
            string clientId = clientIdToTrim[2].Trim('m', 's', 'g', 'o', 'k', '=', 'c', 'l', 'i', 'd', '\n', '\r');

            telCon.WriteLine("channellist");
            string channelIdNeedParse = telCon.Read();
            string[] channelIdToTrim = channelIdNeedParse.Split(' ', '|');


            List<string> channelIdList = new List<string>();
            List<string> channelNameList = new List<string>();
            List<string> connectedClientsList = new List<string>();

            int cid = 1;

            for (int i = 0; i < channelIdToTrim.Length; i++)
            {


                if (cid == 1)
                {
                    channelIdList.Add(channelIdToTrim[i].Trim('c', 'i', 'd', '='));
                    cid++;
                }

                if (cid == 6)
                {
                    cid = 0;
                }
                else
                {
                    cid++;
                }
            }

            cid = 1;

            for (int i = 0; i < channelIdToTrim.Length; i++)
            {


                if (cid == 4)
                {
                    channelNameList.Add(channelIdToTrim[i].Replace("channel_name=", " ").Replace(@"\s", " ").Trim(' '));
                    cid++;
                }
                if (cid == 6)
                {
                    cid = 0;
                }
                else
                {
                    cid++;
                }
            }

            cid = 1;

            for (int i = 0; i < channelIdToTrim.Length; i++)
            {


                if (cid == 6)
                {
                    connectedClientsList.Add(channelIdToTrim[i].Trim('t', 'o', 'a', 'l', '_', 'c', 'l', 'i', 'e', 'n', 't', 's', '=', '\\', 'n', 'r'));
                    cid = 1;
                }

                else
                {
                    cid++;
                }
            }

            for (int i = 0; i < channelIdList.Count - 1; i++)
            {
                if (Convert.ToInt32(connectedClientsList[i]) > 0)
                {
                    messageAmalg += $"Name: {channelNameList[i]} ({connectedClientsList[i]})  Channel ID: {channelIdList[i]} \r\n";
                }
            }

            //Build Embed Message
            embedBuilder = new DiscordEmbedBuilder
            {
                Color = DiscordColor.Azure,
                Title = "SBI TS3 Channels and Subchannels (Populated) \n",
                Description = messageAmalg,
            };

            DiscordEmbed finalEmbed = embedBuilder.Build();
            await Context.Message.Channel.SendMessageAsync("", embed: finalEmbed);


            accumMsgs += 2;
            joinUsed++;


        }

        [Command("bearjoin"),
            Aliases("join"),
            Description("Command to get bear to join a TS3 Channel based on the ChannelID gotten from \"channels\" command. " +
            "If you are joined to a Voice Server on Discord Bear will follow you into that channel \n\n\n Example: !join 47    <----This command will join channel AFK in SBI TS3")]
        //[RequirePermissions(Permissions.ManageMessages)]
        //[RequireUserPermission(Discord.GuildPermission.ManageMessages)]
        public async Task JoinChannel(CommandContext Context, [Description("Channel ID gotten by using the \"channels\" command")]string channelId)
        {

            joinUsed++;
            accumMsgs++;
            TelnetConnection telCon = new TelnetConnection(server, serverPort);

            Console.WriteLine(telCon.Read());

            telCon.WriteLine($"auth apikey={apiKey}");

            telCon.WriteLine("whoami");
            string clientIdNeedParse = telCon.Read();
            string[] clientIdToTrim = clientIdNeedParse.Split(' ');
            string clientId = clientIdToTrim[2].Trim('m', 's', 'g', 'o', 'k', '=', 'c', 'l', 'i', 'd', '\n', '\r');
            //
            telCon.WriteLine($"clientmove cid={channelId} clid={clientId}");
            await Task.Delay(TimeSpan.FromSeconds(.3));
            telCon.WriteLine("channelconnectinfo");

            string[] currentChannel = telCon.Read().Split('=', ' ', '\n', '\r');

            string currentChannelName = "";
            for (int i = 0; i < currentChannel.Length; i++)
            {
                if (currentChannel[i] == "path")
                {
                    currentChannelName = currentChannel[i + 1];
                    break;
                }

            }

            if (accumMsgs > 0 && joinUsed > 1)
            {
                var messages = await Context.Channel.GetMessagesAsync();
                var botMessagesToDel = messages.Where(m => m.Author.Id == Context.Client.CurrentUser.Id || m.Author.Id == Context.Message.Author.Id).Take(accumMsgs);
                await Context.Channel.DeleteMessagesAsync(botMessagesToDel);
                //Console.WriteLine("inside if");
                accumMsgs = 0;
                joinUsed = 0;
            }
            else
            {
                var messages = await Context.Channel.GetMessagesAsync();
                var botMessagesToDel = messages.Where(m => m.Author.Id == Context.Client.CurrentUser.Id || m.Author.Id == Context.Message.Author.Id).Take(1);
                await Context.Channel.DeleteMessagesAsync(botMessagesToDel);
                accumMsgs++;
                joinUsed = 0;
                //Console.WriteLine("inside else");
            }

            await Task.Delay(TimeSpan.FromSeconds(1));

            await Context.Channel.SendMessageAsync($"Connected to Channel: {currentChannelName}"
                            .Replace("\\s", " ")
                            .Replace("[cspacer1]", "")
                            .Replace("[cspacer2]", "")
                            .Replace("[cspacer3]", "")
                            .Replace("[cspacer4]", "")
                            .Replace("[cspacer5]", "")
                            .Replace("[cspacer6]", "")
                            .Replace("[cspacer7]", "")
                            .Replace("Stormbluff Isle Guilds/", ""));

            VoiceNextClient vClient = Context.Client.GetVoiceNextClient();
            VoiceNextConnection vConnect = vClient.GetConnection(Context.Guild);

            var vChannel = Context.Member?.VoiceState?.Channel;
            if (vChannel == null)
            {
                await Context.RespondAsync("Please join a Voice Channel so I can follow you! :bear:");
                accumMsgs++;
            }

            else
            {
                vConnect = await vClient.ConnectAsync(vChannel);

            }
            currentVConnect = vConnect;
            currentGuild = Context.Guild;

            //------------------------------------------------------

            voiceBlockSize = audioOutForm.AverageBytesPerSecond/50;
            byte[] buffer = new byte[voiceBlockSize];
            int byteCount = 0;
  

            srcStream = new WaveInEvent
            {
                DeviceNumber = 0,
                WaveFormat = audioOutForm,
                BufferMilliseconds = 20,
            };

            srcStream.StartRecording();
            srcStream.DataAvailable += VoiceDataAvailable;

        
            while ((byteCount = bWP.Read(buffer, 0, voiceBlockSize)) > 0)
            {

                await currentVConnect.SendSpeakingAsync(true);
                for (int i = byteCount; i < voiceBlockSize; i++)
                {
                    buffer[i] = 0;
                    Console.WriteLine(buffer[i]);
                }
                    await currentVConnect.SendAsync(buffer, 20);
            }

            await Task.Delay(-1);
            
        }

        public static void VoiceDataAvailable(object sender, WaveInEventArgs e)
        {
            try
            {
                bWP.AddSamples(e.Buffer, 0, e.BytesRecorded);
            }
            catch
            {
                bWP.ClearBuffer();
            }
        }


        
        [Command("leave")]
        public async Task LeaveChannel(CommandContext Context)
        {
            TelnetConnection telCon = new TelnetConnection(server, serverPort);

            Console.WriteLine(telCon.Read());

            telCon.WriteLine($"auth apikey={apiKey}");

            telCon.WriteLine("whoami");
            string clientIdNeedParse = telCon.Read();
            string[] clientIdToTrim = clientIdNeedParse.Split(' ');
            string clientId = clientIdToTrim[2].Trim('m', 's', 'g', 'o', 'k', '=', 'c', 'l', 'i', 'd', '\n', '\r');
            telCon.WriteLine($"clientmove cid=47 clid={clientId}");
            VoiceNextClient vClient = Context.Client.GetVoiceNextClient();
            VoiceNextConnection vConnect = vClient.GetConnection(Context.Guild);

            if (vConnect == null)
            {
                await Context.RespondAsync("connects null brolloio");
            }
            
            else
            {
                vConnect.Disconnect();
                srcStream.StopRecording();
                bWP.ClearBuffer();
            }
            

            var messages = await Context.Channel.GetMessagesAsync();
            var botMessageToDel = messages.Where(m => m.Content == "!leave").Take(1);
            await Context.Channel.DeleteMessagesAsync(botMessageToDel);
        }

        [Command("gw2boss"),
            Description("This command will output any GW2 world boss events that are either already active (for up to 10minutes) or upcoming events within the next 30 minutes.")]
        public async Task DisplayBosses(CommandContext Context)
        {
            var repository = GW2.Local.EventRotations;
            var eventRotations = repository.GetDynamicEventRotations();
            string activeBossList = "__**Boss Name**__       __**Waypoint**__     __**Time Until Start**__ \r\n";
            //Console.WriteLine($"{activeBossList}");
            foreach (var bossEvent in eventRotations.Values)
            {
                foreach (var timeSlot in bossEvent.Shifts)
                {
                    if (timeSlot.LocalDateTime - DateTime.Now <= TimeSpan.FromMinutes(30.00))
                    {
                        WorldBossInfo boss = WorldBossIds.GetBossInfo(bossEvent.EventId.ToString().ToUpper());
                        if (boss != null)
                        {
                            if ((timeSlot.LocalDateTime - DateTime.Now).Duration() <= TimeSpan.FromMinutes(1) || timeSlot.LocalDateTime - DateTime.Now > TimeSpan.FromMinutes(-10) && timeSlot.LocalDateTime - DateTime.Now < TimeSpan.FromMinutes(0))
                            {
                                activeBossList += $"{boss.Name}    {boss.Waypoint}    Meta Active \n";
                            }
                            else if (timeSlot.LocalDateTime - DateTime.Now > TimeSpan.FromSeconds(0))
                            {
                                activeBossList += $"{boss.Name}    {boss.Waypoint}    {(timeSlot.LocalDateTime - DateTime.Now).Minutes}m{(timeSlot.LocalDateTime - DateTime.Now).Seconds}s \n";
                            }
                        }
                    }
                }
            }
            Console.WriteLine("After foreach");
            var embedBuilderBoss = new DiscordEmbedBuilder
            {
                Color = DiscordColor.IndianRed,
                Title = "**World Bosses in GW2(Active and Upcoming)**",
                Description = activeBossList,
            };

            DiscordEmbed finalEmbed = embedBuilderBoss.Build();
            await Context.Message.Channel.SendMessageAsync("", embed: finalEmbed);


        }
        
    }
}
