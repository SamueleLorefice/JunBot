using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Jun {
    class BotDatabase {[JsonProperty(PropertyName = "GroupDB", Required = Required.AllowNull)]
        public List<GroupEntry> GroupDB { get; set; }
        [JsonProperty(PropertyName = "UserDB", Required = Required.AllowNull)]
        public List<UserEntry> UserDB { get; set; }
    }

    class UserEntry {
        [JsonProperty(PropertyName = "ID", Required = Required.Always)]
        public long ID { get; set; }
        [JsonProperty(PropertyName = "FirstName", Required = Required.Always)]
        public string FirstName { get; set; }
        [JsonProperty(PropertyName = "LastName", Required = Required.AllowNull)]
        public string LastName { get; set; }
        [JsonProperty(PropertyName = "NickName", Required = Required.AllowNull)]
        public string Nickname { get; set; }
        [JsonProperty(PropertyName = "Subscribed", Required = Required.Always)]
        public bool Subcribed { get; set; }
        //public bool FilthyPeasant;
    }

    class GroupEntry {
        [JsonProperty(PropertyName = "ID", Required = Required.Always)]
        public long ID { get; set; }
        [JsonProperty(PropertyName = "Name", Required = Required.Always)]
        public string GroupName { get; set; }
        [JsonProperty(PropertyName = "Description", Required = Required.AllowNull)]
        public string GroupDescription { get; set; }
        [JsonProperty(PropertyName = "Users", Required = Required.Always)]
        public List<UserEntry> UsersList { get; set; }
    }
}
