using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace NICTERP.Models.NICT
{
    public class StudentDetail
    {
        [Key]
        public int id { get; set; }
        [DisplayName("Reg No.")]
        public int Reg_No { get; set; }

        [ForeignKey("AdmissionForm")]
        public long Form_No { get; set; }

        public string Photo { get; set; }
        [Required]
        public string Name { get; set; }
        [DisplayName("Parent Name")]
        public string Parents_Name { get; set; }
         [DisplayName("Date of Birth")]
        public string DOB { get; set; }

        [Required]
        [DisplayName("Contact No.")]
        [MaxLength(10)]
        [MinLength(10)]
        public string Contact_No { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }

        [Required]
        public string Occupation { get; set; }
        [DisplayName("College/Office Address")]
        public string College_Office_Add { get; set; }

        [DisplayName("Course Applied")]
        [ForeignKey("Courses")]
        public int Course_Applied { get; set; }
        [DisplayName("Duration Of Course")]
        public string Duration_Of_Course { get; set; }
        public string Nationality { get; set; }

        [DisplayName("Date Of Joining")]
        public System.DateTime Date_Of_Joining { get; set; }

        [Required]
        [ForeignKey("CenterName")]
        public int Center { get; set; }

        [Required]
        [DisplayName("Fee Fixed For")]
        public int Fee_Fixed { get; set; }
        
        public string Country { get; set; }

        public virtual AdmissionForm AdmissionForm { get; set; }
        public virtual Center CenterName { get; set; }
        public virtual Courses Courses { get; set; }
    }

   
}