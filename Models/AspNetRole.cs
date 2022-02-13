using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace Data_capture.Models
{
    [Index(nameof(Name), Name = "RoleNameIndex", IsUnique = true)]
    public partial class AspNetRole
    {
        public AspNetRole()
        {
            AspNetUserRoles = new HashSet<AspNetUserRole>();
        }

        [Key]
        [StringLength(128)]
        public string Id { get; set; }
        [Required]
        [StringLength(256)]
        public string Name { get; set; }

        [InverseProperty(nameof(AspNetUserRole.Role))]
        public virtual ICollection<AspNetUserRole> AspNetUserRoles { get; set; }
    }
}
