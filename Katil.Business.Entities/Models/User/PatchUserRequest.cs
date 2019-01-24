using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace Katil.Business.Entities.Models.User
{
    public class PatchUserRequest
    {
        ////[JsonProperty("user_id")]
        ////public int UserId { get; set; }

        [JsonProperty("is_active")]
        [Required]
        public bool IsActive { get; set; }
    }
}
