using System;
using System.IO;
//using Newtonsoft.Json;

//BotApi specific
using Telegram.Bot;
using Telegram.Bot.Args;
//using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
//using Telegram.Bot.Types.InlineQueryResults;
//using Telegram.Bot.Types.InputMessageContents;
//using Telegram.Bot.Types.ReplyMarkups;


namespace Jun
{
	static class MainClass
	{
		//static string settingsFile = "Settings.Json";
		static Settings settings;
        static TelegramBotClient Bot;

        public static void Main (string[] args)
		{
            //checks if the token file exists and reads it, otherwise creates it.
            if (File.Exists("token"))
            {
                string token = System.IO.File.ReadAllText("token");
                if (token == string.Empty){
                    System.Console.WriteLine("No token found in the file. Please put your bot token on the token file and restart this program.");
                    Console.ReadLine();
                    return;
                }

                Bot = new TelegramBotClient(token);

                //delegates here
                Bot.OnMessage += Ping;
                Bot.OnMessage += AutoUpdate;

                var me = Bot.GetMeAsync().Result;
                Console.Title = me.Username;

                Bot.StartReceiving();
                Console.ReadLine();
                Bot.StopReceiving();
            }else{
                File.Create("token");
                System.Console.WriteLine("Please put your bot token on the token file and restart this program.");
                Console.ReadLine();
            }
		}

        static async void Ping(object sender, MessageEventArgs e) {
            if (e.Message == null || e.Message.Type != MessageType.TextMessage) return;
            string message = e.Message?.Text;
            if (message?.ToLower() == "ping")
                await Bot.SendTextMessageAsync(e.Message.Chat.Id, "I'm up and running!");
        }

        static async void AutoUpdate(object sender, MessageEventArgs e) {
          if (e.Message == null || e.Message.Type != MessageType.TextMessage) return;
            string message = e.Message?.Text;
            if (message?.ToLower() == "/autoupdate"){
                await Bot.SendTextMessageAsync(e.Message.Chat.Id, "Beginning autoupdate procedure! \n *NOTE:* _This is a stub and will change soon_", parseMode:ParseMode.Markdown);
            }
         }
    }
}
