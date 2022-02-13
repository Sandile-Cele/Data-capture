using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace Data_capture.Models
{
    [Index(nameof(UserName), Name = "UserNameIndex", IsUnique = true)]
    public partial class AspNetUser
    {
        public AspNetUser()
        {
            AspNetUserClaims = new HashSet<AspNetUserClaim>();
            AspNetUserLogins = new HashSet<AspNetUserLogin>();
            AspNetUserRoles = new HashSet<AspNetUserRole>();
            EntryCategories = new HashSet<EntryCategory>();
            Measurements = new HashSet<Measurement>();
        }

        [Key]
        [StringLength(128)]
        public string Id { get; set; }
        [StringLength(256)]
        public string Email { get; set; }
        public bool EmailConfirmed { get; set; }
        public string PasswordHash { get; set; }
        public string SecurityStamp { get; set; }
        public string PhoneNumber { get; set; }
        public bool PhoneNumberConfirmed { get; set; }
        public bool TwoFactorEnabled { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? LockoutEndDateUtc { get; set; }
        public bool LockoutEnabled { get; set; }
        public int AccessFailedCount { get; set; }
        [Required]
        [StringLength(256)]
        public string UserName { get; set; }

        [InverseProperty(nameof(AspNetUserClaim.User))]
        public virtual ICollection<AspNetUserClaim> AspNetUserClaims { get; set; }
        [InverseProperty(nameof(AspNetUserLogin.User))]
        public virtual ICollection<AspNetUserLogin> AspNetUserLogins { get; set; }
        [InverseProperty(nameof(AspNetUserRole.User))]
        public virtual ICollection<AspNetUserRole> AspNetUserRoles { get; set; }
        [InverseProperty(nameof(EntryCategory.User))]
        public virtual ICollection<EntryCategory> EntryCategories { get; set; }
        [InverseProperty(nameof(Measurement.User))]
        public virtual ICollection<Measurement> Measurements { get; set; }
    }
}
