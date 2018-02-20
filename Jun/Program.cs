using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types.Enums;

namespace Jun {

    internal static class MainClass {//TODO general cleanup is required here.
        private static Settings config;
        private static TelegramBotClient Bot;
        private static Stopwatch Uptime = new Stopwatch();

        public static void Main(string[] args) {
            Console.WriteLine("Starting TelegramBot");
            if (!LoadSettings()) return;

            Bot = new TelegramBotClient(config?.Token);//COMMAND REGISTRATION
            Console.WriteLine("Registering commands");
            Bot.OnMessage += ChatModule;
            Bot.OnMessage += ChatModuleAdministration;
            Bot.OnMessage += UptimeCmd;
            Console.WriteLine("Completing startup");
            Uptime.Start();
            //Startup
            var me = Bot.GetMeAsync().Result;
            Console.Title = me.Username;
            Bot.StartReceiving();
            Console.ReadLine();
            Bot.StopReceiving();
        }

        //checks if the token file exists and reads it, otherwise creates it.
        private static bool LoadSettings() {
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
        }//TODO move this to a settings manager class

        private static async void AutoUpdate(object sender, MessageEventArgs e) {
            if (e.Message == null || e.Message.Type != MessageType.Text) return;
            string message = e.Message?.Text;
            if (message?.ToLower() == "/autoupdate" && e.Message.From.Id == config.MasterID) {
                await Bot.SendTextMessageAsync(e.Message.Chat.Id, "Beginning autoupdate procedure! \n *NOTE:* _This is a stub and will change soon_", parseMode: ParseMode.Markdown);
            }
        }//TODO move this to a "self-update" class

        private static void ChatModule(object sender, MessageEventArgs e) {
            if (e.Message == null || e.Message.Type != MessageType.Text) return;
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
        }//TODO move this and  ChatMouleAdministration method to a "ChatModule" class

        private static async void ChatModuleAdministration(object sender, MessageEventArgs e) {
            var message = e.Message.Text.ToLower();
            Stopwatch s = new Stopwatch();
            if (e.Message == null || e.Message.Type != MessageType.Text || e.Message.From.Id != config.MasterID) return;

            s.Start();

            #region ADD

            //                                       triggers                        answers
            //syntax: /chatmodule <command> <argument list separed by \> | <argument list separed by \> | <master only> / <parse mode>
            if (Regex.IsMatch(message, @"(\/chatmodule add )([^|]+)\|([^|]+)\|{0,1}(true|false){0,1}\|(none|html|markdown){0,1}", RegexOptions.IgnoreCase)) {
                var match = Regex.Match(e.Message.Text, @"(\/chatmodule add )([^|]+)\|([^|]+)\|{0,1}(all|masteronly|notmaster){0,1}\|(none|html|markdown){0,1}", RegexOptions.IgnoreCase);
                config.TriggerAnswers.Add(new TriggerAnswer {
                    Triggers = new List<string>(match.Groups[2].Value.Split('\\')),
                    Answers = new List<string>(match.Groups[3].Value.Split('\\')),
                    MasterOnly = match.Groups[4].Value.ToRestriction(),
                    Parsing = match.Groups[5].Value.ToParseMode()
                });
                System.IO.File.WriteAllText("bot.cfg", JsonConvert.SerializeObject(config, Formatting.Indented));
                await Bot.SendTextMessageAsync(e.Message.Chat.Id, "Aggiunto alla configurazione con successo!, Tempo richiesto per l'operazione: " + s.ElapsedMilliseconds + "ms");
            }

            #endregion ADD

            s.Restart();

            #region Reload

            if (Regex.IsMatch(message, @"(\/chatmodule reload)", RegexOptions.IgnoreCase)) {
                await Bot.SendTextMessageAsync(e.Message.Chat.Id, "Ok master, ricarico subito la configurazione!");
                LoadSettings();
                await Bot.SendTextMessageAsync(e.Message.Chat.Id, "Fatto!, Tempo richiesto per l'operazione: " + s.ElapsedMilliseconds + "ms");
            }

            #endregion Reload

            s.Restart();

            #region Remove

            if (Regex.IsMatch(message, @"(\/chatmodule remove )([^|]+)\|", RegexOptions.IgnoreCase)) {
                var match = Regex.Match(e.Message.Text, @"(\/chatmodule remove )([^|]+)\|", RegexOptions.IgnoreCase);
                for (int i = 0; i < config.TriggerAnswers.Count - 1; i++) {
                    foreach (var trigger in config.TriggerAnswers[i].Triggers) {
                        if (trigger == match.Groups[2].Value) {
                            config.TriggerAnswers.Remove(config.TriggerAnswers[i]);
                            await Bot.SendTextMessageAsync(e.Message.Chat.Id, "Rimosso dalla configurazione con successo!, Tempo richiesto per l'operazione: " + s.ElapsedMilliseconds + "ms");
                        }
                    }
                }
                System.IO.File.WriteAllText("bot.cfg", JsonConvert.SerializeObject(config, Formatting.Indented));
            }

            #endregion Remove

            s.Stop();

            #region Stats

            if (Regex.IsMatch(message, @"(\/chatmodule stats)", RegexOptions.IgnoreCase)) {
                int total_triggers = 0;
                int total_answers = 0;
                long config_size = new System.IO.FileInfo("bot.cfg").Length;
                foreach (var pair in config.TriggerAnswers) {
                    total_answers += pair.Answers.Count;
                    total_triggers += pair.Triggers.Count;
                }
                await Bot.SendTextMessageAsync(e.Message.Chat.Id, String.Format("Momento di statistica...\n*Numero di trigger* `{0}`\n*Numero di risposte* `{1}`\n*Dimensione file configurazione* `{2}`B", total_triggers, total_answers, config_size), parseMode: ParseMode.Markdown);
            }

            #endregion Stats
        }

        private static async void ChatSendResponse(List<string> answers, long chatId, ParseMode parseMode) {
            int pick = new Random().Next(answers.Count);
            await Bot.SendTextMessageAsync(chatId, answers[pick], parseMode: parseMode);
        }

        private static async void UptimeCmd(object sender, MessageEventArgs e) {
            if (e.Message == null || e.Message.Type != MessageType.Text) return;
            string message = e.Message?.Text;
            if (message == "/uptime") {
                await Bot.SendTextMessageAsync(e.Message.Chat.Id, String.Format("Sono online da: `{0}`d `{1}`h `{2}`m `{3}`s", Uptime.Elapsed.Days, Uptime.Elapsed.Hours, Uptime.Elapsed.Minutes, Uptime.Elapsed.Seconds), parseMode: ParseMode.Markdown);
            }
        }

        #region string extensions

        private static CommandRestiction ToRestriction(this string s) {
            switch (s.ToLower()) {
                case "masteronly":
                    return CommandRestiction.MasterOnly;

                case "notmaster":
                    return CommandRestiction.NotMaster;

                default:
                    return CommandRestiction.All;
            }
        }

        private static ParseMode ToParseMode(this string s) {
            switch (s.ToLower()) {
                case "html":
                    return ParseMode.Html;

                case "markdown":
                    return ParseMode.Markdown;

                default:
                    return ParseMode.Default;
            }
        }

        #endregion string extensions
    }
}
