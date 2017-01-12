using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using Telegram.Bot.Types;

namespace Jun
{
	public class Settings
	{
        //--------------------------
        [JsonProperty(PropertyName = "bot_token", Required = Required.AllowNull)] public string token {get; set;}
        public User BotIdentity { get; set; }
        //--------------------------
        [JsonProperty(PropertyName = "bot_personality", Required = Required.AllowNull) ]public EPErsonality BotPersonality { get; set; }
        [JsonProperty(PropertyName = "bot_stats", Required = Required.AllowNull)] public SPersonalityStats BotStats { get; set; }
        //id of the creator
        [JsonProperty(PropertyName = "master_id", Required = Required.AllowNull)] public long MasterID { get; set; }
        [JsonProperty(PropertyName = "personality_user_list", Required = Required.Default)] public List<SUserStats> PUserList { get; set; }
        //--------------------------

        //Constructor
        public Settings(string tok, long ownID){
			token = tok;
			ownID = MasterID;
		}
	}
}
