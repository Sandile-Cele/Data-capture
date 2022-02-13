using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace Data_capture.Models
{
    [Table("EntryCategory")]
    public partial class EntryCategory
    {
        public EntryCategory()
        {
            Measurements = new HashSet<Measurement>();
        }

        [Key]
        [Column("ecId")]
        public int EcId { get; set; }
        [Required]
        [Column("userId")]
        [StringLength(128)]
        public string UserId { get; set; }
        [Required]
        [Column("ecName")]
        [StringLength(255)]
        public string EcName { get; set; }

        [ForeignKey(nameof(UserId))]
        [InverseProperty(nameof(AspNetUser.EntryCategories))]
        public virtual AspNetUser User { get; set; }
        [InverseProperty(nameof(Measurement.Ec))]
        public virtual ICollection<Measurement> Measurements { get; set; }
    }
}
