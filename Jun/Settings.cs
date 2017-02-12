using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace Jun {

    public class Settings {

        //--------------------------
        [JsonProperty(PropertyName = "bot_token", Required = Required.AllowNull)]
        public string Token { get; set; }

        [JsonProperty(PropertyName = "owner_id", Required = Required.AllowNull)]
        public long MasterID { get; set; }

        //--------------------------
        [JsonProperty(PropertyName = "trigger_answers", Required = Required.AllowNull)]
        public List<TriggerAnswer> TriggerAnswers { get; set; }

        //--------------------------
        [JsonProperty(PropertyName = "blacklist", Required = Required.AllowNull)]
        public List<BlackListedUser> BlackList { get; set; }

        //--------------------------
        //Constructor
        public Settings(string tok, long ownID) {
            Token = tok;
            MasterID = ownID;
        }
    }

    public class TriggerAnswer {

        [JsonProperty(PropertyName = "triggers_list", Required = Required.Always)]
        public List<string> Triggers { get; set; } = new List<string>();

        [JsonProperty(PropertyName = "answers_list", Required = Required.Always)]
        public List<string> Answers { get; set; } = new List<string>();

        [JsonProperty(PropertyName = "parse_mode", Required = Required.AllowNull)]
        public ParseMode Parsing { get; set; }

        [JsonProperty(PropertyName = "master_only", Required = Required.Always)]
        public CommandRestiction MasterOnly { get; set; }
    }

    public class BlackListedUser : User {

        [JsonProperty(PropertyName = "first_name", Required = Required.Always)]
        public new string FirstName { get; set; }

        [JsonProperty(PropertyName = "id", Required = Required.Always)]
        public new int Id { get; set; }

        [JsonProperty(PropertyName = "last_name", Required = Required.Default)]
        public new string LastName { get; set; }

        [JsonProperty(PropertyName = "username", Required = Required.Default)]
        public new string Username { get; set; }

        [JsonProperty(PropertyName = "ban_ends", Required = Required.AllowNull)]
        private DateTime BanEnd { get; set; }
    }

    public enum CommandRestiction {
        All,
        MasterOnly,
        NotMaster
    }
}