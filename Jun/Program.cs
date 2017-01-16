using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types.Enums;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace Jun {
    class MainClass {
        static Settings config;
        static TelegramBotClient Bot;

        public static void Main(string[] args) {
            if (!LoadSettings()) return;
            #region Bot Setup
            Bot = new TelegramBotClient(config?.Token);
            //COMMAND REGISTRATION
            Bot.OnMessage += Ping;

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
                await Bot.SendTextMessageAsync(e.Message.Chat.Id, "Mhh, si sono online!");
        }

        static async void AutoUpdate(object sender, MessageEventArgs e) {
            if (e.Message == null || e.Message.Type != MessageType.TextMessage) return;
            string message = e.Message?.Text;
            if (message?.ToLower() == "/autoupdate" && e.Message.From.Id == config.MasterID) {
                await Bot.SendTextMessageAsync(e.Message.Chat.Id, "Beginning autoupdate procedure! \n *NOTE:* _This is a stub and will change soon_", parseMode: ParseMode.Markdown);

            }
        }

        static void ChatModule(object sender, MessageEventArgs e) {
            if (e.Message == null || e.Message.Type != MessageType.TextMessage) return;
            string message = e.Message?.Text;
            Parallel.ForEach(config.TriggerAnswers, (currTrigger) => {
                foreach (string trigger in currTrigger.Triggers) {
                    if (Regex.IsMatch(message, String.Format("({0})\\w+", trigger))) {
                        ChatSendResponse(currTrigger.Answers, e.Message.Chat.Id, currTrigger.Parsing);
                    }
                }
            });
        }

        private static async void ChatSendResponse(List<string> answers, long chatId, ParseMode parseMode) {
            int pick = new Random().Next(answers.Count);
            await Bot.SendTextMessageAsync(chatId, answers[pick], parseMode: parseMode);
        }


        //checks if the token file exists and reads it, otherwise creates it.
        static bool LoadSettings() {
            if (System.IO.File.Exists("bot.cfg")) {
                try {
                    string cfg = System.IO.File.ReadAllText("bot.cfg");
                    config = JsonConvert.DeserializeObject<Settings>(cfg);
                } catch (JsonReaderException) {
                    System.IO.File.Delete("bot.cfg");
                    return false;
                }
            } else {
                System.IO.File.AppendAllText("bot.cfg", JsonConvert.SerializeObject(new Settings("token here", 0), Formatting.Indented));
                return false;
            }
            return true;
        }
    }
}
