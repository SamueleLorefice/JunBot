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

		public static void Main (string[] args)
		{
			//configuration down here
			if (System.IO.File.Exists(settingsFile)){
				//gets token and other infos
				settings = JsonConvert.DeserializeObject<Settings>(settingsFile);
			}else{
				try{
					settings = new Settings("", 0);
					string _out = JsonConvert.SerializeObject(settings, Formatting.Indented);
					System.IO.File.Create(settingsFile);
					System.IO.File.WriteAllText(settingsFile, _out);
				}catch(Exception e){
					Console.WriteLine("Unable to create/save settings file.");
					Environment.Exit(1);
				}
			}

			//telegram.bot related down here
			TelegramBotClient Bot = new TelegramBotClient(settings.token);
			var me = Bot.GetMeAsync().Result;
            Console.Title = me.Username;
		}
	}
}
