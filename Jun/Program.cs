using System;
//Settings File specific
using Newtonsoft.Json;
//BotApi specific
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.InlineQueryResults;
using Telegram.Bot.Types.InputMessageContents;
using Telegram.Bot.Types.ReplyMarkups;


namespace Jun
{
	static class MainClass
	{
		static string settingsFile = "Settings.Json";
		static Settings settings;
        static TelegramBotClient Bot;

        public static void Main (string[] args)
		{
            string token = System.IO.File.ReadAllText("token");
            Bot = new TelegramBotClient(token);

            //delegates here
            Bot.OnMessage += Ping;

            var me = Bot.GetMeAsync().Result;
            Console.Title = me.Username;

            Bot.StartReceiving();
            Console.ReadLine();
            Bot.StopReceiving();
		}

        static async void Ping(object sender, MessageEventArgs e) {
            if (e.Message == null || e.Message.Type != MessageType.TextMessage) return;
            string message = e.Message?.Text;
            if (message?.ToLower() == "ping")
                await Bot.SendTextMessageAsync(e.Message.Chat.Id, "YEEEEEEY I''m ALIVE!");
        }
    }
}
