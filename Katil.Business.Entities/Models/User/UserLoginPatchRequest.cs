using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace Katil.Business.Entities.Models.User
{
    public class UserLoginPatchRequest
    {
        [JsonProperty("accepts_text_messages")]
        public bool AcceptsTextMessages { get; set; }

        [JsonProperty("account_mobile")]
        [StringLength(15)]
        public string AccountMobile { get; set; }

        [JsonProperty("full_name")]
        [StringLength(100)]
        public string FullName { get; set; }

        [JsonProperty("is_active")]
        public bool IsActive { get; set; }

        [JsonProperty("username")]
        public string Username { get; set; }

        [JsonProperty("scheduler")]
        public bool Scheduler { get; set; }

        [JsonProperty("service_office_id")]
        public int? ServiceOfficeId { get; set; }
    }
}
