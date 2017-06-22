using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace NICTERP.Models.NICT
{
    public class Courses
    {
        [Key]
        public int Id { get; set; }
        [DisplayName("Course Name")]
        public string CourseName { get; set; }
        [DisplayName("Fee")]
        public int CourseFee { get; set; }
    }
}