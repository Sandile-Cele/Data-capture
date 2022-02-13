using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace Data_capture.Models
{
    [Table("Measurement")]
    public partial class Measurement
    {
        public Measurement()
        {
            MeasurementMetaData = new HashSet<MeasurementMetaDatum>();
        }

        [Key]
        [Column("mId")]
        public int MId { get; set; }
        [Required]
        [Column("userId")]
        [StringLength(128)]
        public string UserId { get; set; }
        [Column("ecId")]
        public int EcId { get; set; }
        [Column("mTemperature", TypeName = "decimal(6, 3)")]
        public decimal MTemperature { get; set; }
        [Column("mHumidity")]
        public int MHumidity { get; set; }
        [Column("mWeight", TypeName = "decimal(10, 3)")]
        public decimal MWeight { get; set; }
        [Column("mWidth", TypeName = "decimal(10, 3)")]
        public decimal MWidth { get; set; }
        [Column("mLength", TypeName = "decimal(10, 3)")]
        public decimal MLength { get; set; }
        [Column("mDepth", TypeName = "decimal(6, 3)")]
        public decimal MDepth { get; set; }

        [ForeignKey(nameof(EcId))]
        [InverseProperty(nameof(EntryCategory.Measurements))]
        public virtual EntryCategory Ec { get; set; }
        [ForeignKey(nameof(UserId))]
        [InverseProperty(nameof(AspNetUser.Measurements))]
        public virtual AspNetUser User { get; set; }
        [InverseProperty(nameof(MeasurementMetaDatum.MIdNavigation))]
        public virtual ICollection<MeasurementMetaDatum> MeasurementMetaData { get; set; }
    }
}
