using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace NICTERP.Models.NICT
{
    public class Installment
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("AdmissionForm")]
        public long Form_No { get; set; }

        [Required]
        [DisplayName("Amount Paid")]
        public int Amount_Paid { get; set; }

        [Required]
        [DisplayName("Balance Due")]
        public int Balance_Due { get; set; }

        [Required]
        public System.DateTime Datetime { get; set; }

      
        [DisplayName("Due Date")]
        public System.DateTime? Duedate { get; set; }

        public virtual AdmissionForm AdmissionForm { get; set; }
    }
}