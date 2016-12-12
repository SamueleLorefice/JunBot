using System;

namespace Jun
{
	public class Settings
	{
		//token of the bot
		public string token {get; set;}
		//id of the creator
		public long ownerID {get; set;}

		public Settings(string tok, long ownID){
			token = tok;
			ownID = ownerID;
		}
	}
}
