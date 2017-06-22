using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace NICTERP.Models.NICT
{
    public class BatchMember
    {
        [Required]
        [Key]
        public int id { get; set; }
        [ForeignKey("Batch")]
        public int BatchId { get; set; }
        [ForeignKey("Student")]
        public int studentId { get; set; }

        public virtual StudentDetail Student { get; set; }
        public virtual Batch Batch { get; set; }
    }
}