using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CsvHelper.Configuration.Attributes;
using Newtonsoft.Json;

namespace Katil.Data.Model
{
    public class User : BaseEntity
    {
        [Key]
        public int Id { get; set; }

        [StringLength(100)]
        [Required]
        public string Email { get; set; }

        [StringLength(250)]
        public string Password { get; set; }

        [StringLength(100)]
        public string FullName { get; set; }

        [StringLength(15)]
        public string AccountMobile { get; set; }

        public SystemUserRole SystemUserRole { get; set; }

        public int SystemUserRoleId { get; set; }

        public bool? IsActive { get; set; }

        [NotMapped]
        public string Token { get; set; }
    }
}
