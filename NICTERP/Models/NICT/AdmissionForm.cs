using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace NICTERP.Models.NICT
{
    [Table("AdmissionForms")]
    public class AdmissionForm
    {
        [Key]
        [DisplayName("Form Number")]
        public long Form_No { get; set; }

        [DefaultValue(false)]
        [DisplayName("Form Complete")]
        public bool Form_Complete { get; set; }
        [Required]
        public System.DateTime Dated { get; set; }
        [ForeignKey("Center")]
        public int centerId { get; set; }
        public virtual Center Center { get; set; }
    }
}