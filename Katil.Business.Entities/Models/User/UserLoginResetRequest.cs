using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace Katil.Business.Entities.Models.User
{
    public class UserLoginResetRequest
    {
        [JsonProperty("password")]
        [StringLength(maximumLength: 250, MinimumLength = 8)]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
