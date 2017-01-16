using System;
using System.Collections.Generic;
using Newtonsoft.Json;
//BotApi specific
using Telegram.Bot;
using Telegram.Bot.Args;
//using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;


namespace Jun {
    class MainClass {
        //static string settingsFile = "Settings.Json";
        static Settings CFG;
        static TelegramBotClient Bot;
        static Queue<long> KonachanQueueId = new Queue<long>();

        public static void Main(string[] args) {
            #region Settings
            //checks if the token file exists and reads it, otherwise creates it.
            if (System.IO.File.Exists("bot.cfg")) {
                try {
                    string cfg = System.IO.File.ReadAllText("bot.cfg");
                    CFG = JsonConvert.DeserializeObject<Settings>(cfg);
                } catch (JsonReaderException) {
                    System.IO.File.Delete("bot.cfg");
                    return;
                }
            } else {
                System.IO.File.AppendAllText("bot.cfg", JsonConvert.SerializeObject(new Settings("token here", 0), Formatting.Indented));
                return;
            }
            #endregion
            #region Bot Setup
            Bot = new TelegramBotClient(CFG?.token);
            //COMMAND REGISTRATION
            Bot.OnMessage += Ping;
            Bot.OnMessage += AutoUpdate;
            //Startup
            var me = Bot.GetMeAsync().Result;
            Console.Title = me.Username;
            Bot.StartReceiving();
            Console.ReadLine();
            Bot.StopReceiving();
            #endregion
        }

        static async void Ping(object sender, MessageEventArgs e) {
            if (e.Message == null || e.Message.Type != MessageType.TextMessage) return;
            string message = e.Message?.Text;
            if (message?.ToLower() == "ping" || message?.ToLower() == "/ping")
                await Bot.SendTextMessageAsync(e.Message.Chat.Id, "I'm up and running!");
        }

        static async void AutoUpdate(object sender, MessageEventArgs e) {
            if (e.Message == null || e.Message.Type != MessageType.TextMessage) return;
            string message = e.Message?.Text;
            if (message?.ToLower() == "/autoupdate" && e.Message.From.Id == CFG.MasterID) {
                await Bot.SendTextMessageAsync(e.Message.Chat.Id, "Beginning autoupdate procedure! \n *NOTE:* _This is a stub and will change soon_", parseMode: ParseMode.Markdown);

            }
        }
    }
}
