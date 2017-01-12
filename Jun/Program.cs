﻿using System;
using System.IO;
using System.Collections.Generic;
using System.Text.RegularExpressions;
//using Newtonsoft.Json;


using KonachanSharp;
//BotApi specific
using Telegram.Bot;
using Telegram.Bot.Args;
//using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types;
using Newtonsoft.Json;
//using Telegram.Bot.Types.InlineQueryResults;
//using Telegram.Bot.Types.InputMessageContents;
//using Telegram.Bot.Types.ReplyMarkups;


namespace Jun {
    static class MainClass {
        //static string settingsFile = "Settings.Json";
        static Settings settings;
        static TelegramBotClient Bot;
        static Queue<long> KonachanQueueId = new Queue<long>();

        public static void Main(string[] args) {
            #region Settings
            //checks if the token file exists and reads it, otherwise creates it.
            if (System.IO.File.Exists("bot.cfg")) {
                try {
                    string cfg = System.IO.File.ReadAllText("token");
                    Settings CFG = JsonConvert.DeserializeObject<Settings>(cfg);
                } catch (JsonReaderException) {
                    System.IO.File.Delete("bot.cfg");
                    return;
                }
            }else {
                System.IO.File.AppendAllText("bot.cfg", JsonConvert.SerializeObject(new Settings("token here", 0), Formatting.Indented));
                return;
            }

                Bot = new TelegramBotClient(settings?.token);

                //delegates here
                Bot.OnMessage += Ping;
                Bot.OnMessage += AutoUpdate;
                Bot.OnMessage += Konachan;

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
            if (message?.ToLower() == "ping")
                await Bot.SendTextMessageAsync(e.Message.Chat.Id, "I'm up and running!");
        }

        static async void AutoUpdate(object sender, MessageEventArgs e) {
            if (e.Message == null || e.Message.Type != MessageType.TextMessage) return;
            string message = e.Message?.Text;
            if (message?.ToLower() == "/autoupdate" && e.Message.From.Id == settings.MasterID) {
                await Bot.SendTextMessageAsync(e.Message.Chat.Id, "Beginning autoupdate procedure! \n *NOTE:* _This is a stub and will change soon_", parseMode: ParseMode.Markdown);
            }
        }

        static void Konachan(object sender, MessageEventArgs e) {
            if (e.Message == null || e.Message.Type != MessageType.TextMessage) return;
            string message = e.Message?.Text;
            if (Regex.IsMatch(message, @"^\/konachan")) {
                Console.WriteLine("Konachan command started.");
                KonachanService srv = new KonachanService();
                KonachanQueueId.Enqueue(e.Message.Chat.Id);
                srv.PostReceived += KonachanEnd;
                srv.GetPost(0, new string[0], Rating.Any);

            }
        }

        static void KonachanEnd(object sender, PostEventArgs e) {
            Bot.SendTextMessageAsync(KonachanQueueId.Dequeue(), e.FetchedPost.file_url);
        }
    }
}
