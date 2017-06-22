using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace NICTERP.Models.NICT
{
    public class Batch
    {
        [Required]
        [Key]
        public int id { get; set; }
        [Required]
        [DisplayName("Batch Name")]
        public String BatchName { get; set; }
        [Required]
        [DisplayName("Date Created")]
        public DateTime DateCreated { get; set; }
        [Required]
        public string From { get; set; }
        [Required]
        public string To { get; set; }
        public string  CreatedBy { get; set; }
        [DefaultValue(false)]
        public bool isDisabled { get; set; }
    }
}