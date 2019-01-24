using System;
using System.Collections.Generic;
using Katil.Business.Entities.Models.Base;
using Newtonsoft.Json;

namespace Katil.Business.Entities.Models.User
{
    public class UserResponse : CommonResponse
    {
        [JsonProperty("user_id")]
        public int Id { get; set; }

        [JsonProperty("user_name")]
        public string Username { get; set; }

        [JsonProperty("name")]
        public string FullName { get; set; }

        [JsonProperty("mobile")]
        public string AccountMobile { get; set; }

        [JsonProperty("role_id")]
        public int SystemUserRoleId { get; set; }

        [JsonProperty("user_admin")]
        public byte AdminAccess { get; set; }

        [JsonProperty("is_active")]
        public bool IsActive { get; set; }

        [JsonProperty("scheduler")]
        public bool Scheduler { get; set; }

        [JsonProperty("service_office_id")]
        public int? ServiceOfficeId { get; set; }
    }
}