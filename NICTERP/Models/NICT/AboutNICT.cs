using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace NICTERP.Models.NICT
{
    [Table("AboutNICTs")]
    public class AboutNICT
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("AdmissionForm")]
        public long Form_No { get; set; }
        public bool Newspaper { get; set; }
        public bool Magazine { get; set; }
        public bool Posters { get; set; }
        public bool Friends { get; set; }
        public bool Hoardings { get; set; }
        public bool Handouts { get; set; }
        [DisplayName("Any Other")]
        public bool Any_Other { get; set; }

        public virtual AdmissionForm AdmissionForm { get; set; }
    }
}