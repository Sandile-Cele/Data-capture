using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace Data_capture.Models
{
    public partial class MeasurementMetaDatum
    {
        [Key]
        [Column("mmdId")]
        public int MmdId { get; set; }
        [Column("mId")]
        public int? MId { get; set; }
        [Required]
        [Column("mmdTimeStamp")]
        public byte[] MmdTimeStamp { get; set; }
        [Column("mmdStatus")]
        public bool? MmdStatus { get; set; }

        [ForeignKey(nameof(MId))]
        [InverseProperty(nameof(Measurement.MeasurementMetaData))]
        public virtual Measurement MIdNavigation { get; set; }
    }
}
