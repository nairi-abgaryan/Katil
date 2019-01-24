using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Katil.Data.Model
{
    public class SystemUserRole
    {
        public int SystemUserRoleId { get; set; }

        [Required]
        [StringLength(50)]
        public string RoleName { get; set; }

        [Required]
        [StringLength(255)]
        public string RoleDescritption { get; set; }

        [Required]
        [DefaultValue(900)]
        public int SessionDuration { get; set; }
    }
}
