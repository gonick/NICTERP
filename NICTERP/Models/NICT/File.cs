using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace NICTERP.Models.NICT
{
    public class File
    {
        [Key]
        public int id { get; set; }

        [ForeignKey("AdmissionForm")]
        public long Form_No { get; set; }

        [Required]
        public string File_LOC { get; set; }

        [Required]
        public System.DateTime Datetime { get; set; }

        public virtual AdmissionForm AdmissionForm { get; set; }
    }
}