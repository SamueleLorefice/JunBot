using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types.Enums;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.Diagnostics;

namespace Jun {
    static class MainClass {
        static Settings config;
        static TelegramBotClient Bot;

        public static void Main(string[] args) {
            if (!LoadSettings()) return;
            Bot = new TelegramBotClient(config?.Token);
            //COMMAND REGISTRATION
            Bot.OnMessage += ChatModule;
            Bot.OnMessage += ChatModuleAdministration;
            //Startup
            var me = Bot.GetMeAsync().Result;
            Console.Title = me.Username;
            Bot.StartReceiving();
            Console.ReadLine();
            Bot.StopReceiving();
        }

        static void ChatModule(object sender, MessageEventArgs e) {
            if (e.Message == null || e.Message.Type != MessageType.TextMessage) return;
            string message = e.Message?.Text;
            if (config.TriggerAnswers == null) return;
            Parallel.ForEach(config.TriggerAnswers, (currTrigger) => {
                foreach (string trigger in currTrigger.Triggers) {
                    if (Regex.IsMatch(message, String.Format("({0})", trigger))) {
                        if (currTrigger.MasterOnly == CommandRestiction.All) {
                            ChatSendResponse(currTrigger.Answers, e.Message.Chat.Id, currTrigger.Parsing);
                        } else if (currTrigger.MasterOnly == CommandRestiction.MasterOnly && config.MasterID == e.Message.From.Id) {
                            ChatSendResponse(currTrigger.Answers, e.Message.Chat.Id, currTrigger.Parsing);
                        } else if (currTrigger.MasterOnly == CommandRestiction.NotMaster && config.MasterID != e.Message.From.Id) {
                            ChatSendResponse(currTrigger.Answers, e.Message.Chat.Id, currTrigger.Parsing);
                        }
                    }
                }
            });
        }

        private static async void ChatSendResponse(List<string> answers, long chatId, ParseMode parseMode) {
            int pick = new Random().Next(answers.Count);
            await Bot.SendTextMessageAsync(chatId, answers[pick], parseMode: parseMode);
        }

        static async void ChatModuleAdministration(object sender, MessageEventArgs e) {
            //                                       triggers                        answers                 
            //syntax: /chatmodule <command> <argument list separed by \> | <argument list separed by \> | <master only> / <parse mode>
            //ADD: (\/chatmodule )(add )([^|]+)(\|[^|]+)(\|true|\|false){0,1}(\|none|\|html|\|markdown){0,1}
            Stopwatch s = new Stopwatch();
            if (e.Message == null || e.Message.Type != MessageType.TextMessage || e.Message.From.Id != config.MasterID) return;
            s.Start();
            #region ADD
            if (Regex.IsMatch(e.Message.Text, @"(\/chatmodule add )([^|]+)\|([^|]+)\|{0,1}(true|false){0,1}\|(none|html|markdown){0,1}", RegexOptions.IgnoreCase)) {
                Match match = Regex.Match(e.Message.Text, @"(\/chatmodule add )([^|]+)\|([^|]+)\|{0,1}(all|masteronly|notmaster){0,1}\|(none|html|markdown){0,1}", RegexOptions.IgnoreCase);
                config.TriggerAnswers.Add(new TriggerAnswer {
                    Triggers = new List<string>(match.Groups[2].Value.Split('\\')),
                    Answers = new List<string>(match.Groups[3].Value.Split('\\')),
                    MasterOnly = match.Groups[4].Value.ToRestriction(),
                    Parsing = match.Groups[5].Value.ToParseMode()
                });
                System.IO.File.WriteAllText("bot.cfg", JsonConvert.SerializeObject(config, Formatting.Indented));
                await Bot.SendTextMessageAsync(e.Message.Chat.Id, "Aggiunto alla configurazione con successo!, Tempo richiesto per l'operazione: "+s.ElapsedMilliseconds+"ms");
            }
            #endregion
            s.Restart();
            #region Reload
            if (Regex.IsMatch(e.Message.Text, @"(\/chatmodule reload)", RegexOptions.IgnoreCase)) {
                await Bot.SendTextMessageAsync(e.Message.Chat.Id, "Ok master, ricarico subito la configurazione!");
                LoadSettings();
                await Bot.SendTextMessageAsync(e.Message.Chat.Id, "Fatto!, Tempo richiesto per l'operazione: " + s.ElapsedMilliseconds+"ms");
            }
            #endregion
            s.Stop();
        }

        static async void AutoUpdate(object sender, MessageEventArgs e) {
            if (e.Message == null || e.Message.Type != MessageType.TextMessage) return;
            string message = e.Message?.Text;
            if (message?.ToLower() == "/autoupdate" && e.Message.From.Id == config.MasterID) {
                await Bot.SendTextMessageAsync(e.Message.Chat.Id, "Beginning autoupdate procedure! \n *NOTE:* _This is a stub and will change soon_", parseMode: ParseMode.Markdown);

            }
        }

        //checks if the token file exists and reads it, otherwise creates it.
        static bool LoadSettings() {
            if (System.IO.File.Exists("bot.cfg")) {
                try {
                    string cfg = System.IO.File.ReadAllText("bot.cfg");
                    config = JsonConvert.DeserializeObject<Settings>(cfg);
                    if (config.TriggerAnswers == null) config.TriggerAnswers = new List<TriggerAnswer>();
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


        #region string extensions
        static CommandRestiction ToRestriction(this string s) {
            switch (s.ToLower()) {
                case "masteronly":
                    return CommandRestiction.MasterOnly;
                case "notmaster":
                    return CommandRestiction.NotMaster;
                default:
                    return CommandRestiction.All;
            }
        }
        static ParseMode ToParseMode(this string s) {
            switch (s.ToLower()) {
                case "html":
                    return ParseMode.Html;
                case "markdown":
                    return ParseMode.Markdown;
                default:
                    return ParseMode.Default;
            }
        }
        #endregion
    }
}
