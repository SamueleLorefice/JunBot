using Newtonsoft.Json;

namespace Jun {
    public class LoggedUser : Telegram.Bot.Types.User {
        //
        // Riepilogo:
        //     User's or bot's first name
        [JsonProperty(PropertyName = "first_name", Required = Required.Always)]
        public new string FirstName { get; set; }
        //
        // Riepilogo:
        //     Unique identifier for this user or bot
        [JsonProperty(PropertyName = "id", Required = Required.Always)]
        public new int Id { get; set; }
        //
        // Riepilogo:
        //     Optional. User's or bot's last name
        [JsonProperty(PropertyName = "last_name", Required = Required.Default)]
        public new string LastName { get; set; }
        //
        // Riepilogo:
        //     Optional. User's or bot's username
        [JsonProperty(PropertyName = "username", Required = Required.Default)]
        public new string Username { get; set; }
    }
}