using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Reflection;
using System.Web;

namespace NICTERP.Models.NICT
{
    public class EduQualification
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("AdmissionForm")]
        public long Form_No { get; set; }

        [Required]
        [DisplayName("Last Exam Passed")]
        public string Last_Exam_Passed { get; set; }

        [Required]
        public int Year { get; set; }

        [Required]
        [DisplayName("Board/University")]
        public string Board_Uni { get; set; }

        [Required]
        public float Percentage { get; set; }

        public virtual AdmissionForm AdmissionForm { get; set; }


    }
}