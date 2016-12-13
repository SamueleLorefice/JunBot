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
			#region Configuration file
			//configuration down here
			if (System.IO.File.Exists(settingsFile)){
				//gets token and other infos
				try {
					settings = JsonConvert.DeserializeObject<Settings>(settingsFile);
				} catch (JsonException e) {
					Console.WriteLine ("Error deserializing settings file.\n"+e.Message);
				}
			}else{
				try{
					settings = new Settings("", 0);
					string _out = JsonConvert.SerializeObject(settings, Formatting.Indented);
					//System.IO.File.Create(settingsFile);
					System.IO.File.WriteAllText(settingsFile, _out);
				}catch(Exception e){
					Console.WriteLine("Unable to create/save settings file.\n"+e.Message);
				}finally{
					Environment.Exit (1);
				}
			}
			#endregion

			//telegram.bot related down here
			TelegramBotClient Bot = new TelegramBotClient(settings.token);

			//dekegates here
			Bot.OnMessage += Ping;

			var me = Bot.GetMeAsync().Result;
            Console.Title = me.Username;
		}

		static void Ping(object sender, MessageEventArgs e)
		{
			
		}
	}
}
