using System;
using Katil.Business.Entities.Models.Base;
using Newtonsoft.Json;

namespace Katil.Business.Entities.Models.User
{
    public class UserLoginResponse : CommonResponse
    {
        [JsonProperty("system_user_id")]
        public int Id { get; set; }

        [JsonProperty("accepts_text_messages")]
        public bool? AcceptsTextMessages { get; set; }

        [JsonProperty("account_mobile")]
        public string AccountMobile { get; set; }

        [JsonProperty("admin_access")]
        public bool AdminAccess { get; set; }

        [JsonProperty("full_name")]
        public string FullName { get; set; }

        [JsonProperty("is_active")]
        public bool? IsActive { get; set; }

        [JsonProperty("system_user_role_id")]
        public int SystemUserRoleId { get; set; }

        [JsonProperty("user_guid")]
        public Guid UserGuid { get; set; }

        [JsonProperty("username")]
        public string Username { get; set; }
    }
}
