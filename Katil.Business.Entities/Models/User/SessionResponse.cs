using Newtonsoft.Json;

namespace Katil.Business.Entities.Models.User
{
    public class SessionResponse
    {
        [JsonProperty("session_time_remaining")]
        public int SessionTimeRemaining { get; set; }
    }
}
