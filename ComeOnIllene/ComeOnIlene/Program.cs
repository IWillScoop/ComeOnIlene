using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;
using System.IO;
using System.Linq;
using System.Net;
using HtmlAgilityPack;
using System.Net.Http;

namespace ComeOnIlene
{
    class Program
    {
        public static void Main(string[] args)
            => new Program().MainAsync().GetAwaiter().GetResult();
        public async Task MainAsync()
        {
            var client = new DiscordSocketClient();
            client.Log += Log;
            client.MessageReceived += MessageReceived;
            string token = "ABC";
            //string token = File.ReadLines("Token.txt").First();
            await client.LoginAsync(TokenType.Bot, token);
            await client.StartAsync();

            await Task.Delay(-1);
        }
        private Task Log(LogMessage msg)
        {
            Console.WriteLine(msg.ToString());
            return Task.CompletedTask;
        }
        private async Task MessageReceived(SocketMessage message)
        {
            switch(message.Content)
            {
                case "!ping":
                    await message.Channel.SendMessageAsync("Pong! <:lndhi:361580218923876352> ");
                    break;
                case "!latest":
                    //return latest comic
                    await message.Channel.SendMessageAsync("<:lndhi:361580218923876352> Check out the latest comic here: https://tapas.io/series/Light-and-Dark");
                    break;

            }
            if (message.Content.ToLower() == "hi ilene")
            {
                await message.Channel.SendMessageAsync("Hiiiiiii! <:lndhi:361580218923876352>");
            }
        }
        private async Task GetLatestComic()
        {
            try
            {
                Console.WriteLine("Getting Latest comic");
                string url = "https://tapas.io/series/Light-and-Dark";
                HttpClient hc = new HttpClient();
                HttpResponseMessage result = await hc.GetAsync(url);

                Stream stream = await result.Content.ReadAsStreamAsync();
                HtmlDocument doc = new HtmlDocument  ();

                doc.Load(stream);

                var episodes = doc.DocumentNode.SelectNodes("//a[@class='episode-box comics']");
                Console.WriteLine(episodes.Last().ToString());
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

        }
    }
}
