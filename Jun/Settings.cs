using System;
using System.Collections.Generic;
using Telegram.Bot.Types;

namespace Jun
{
	public class Settings
	{
        //--------------------------
        public string token {get; set;}
        public User BotIdentity { get; set; }
        //--------------------------
        public EPErsonality BotPersonality { get; set; }
        public SPersonalityStats BotStats { get; set; }
        //id of the creator
        public long MasterID { get; set; }
        public List<SUserStats> UserList { get; set; }
        //--------------------------
        //Constructor
        public Settings(string tok, long ownID){
			token = tok;
			ownID = ownerID;
		}
	}
}
