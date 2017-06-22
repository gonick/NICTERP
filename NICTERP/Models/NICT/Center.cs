using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NICTERP.Models.NICT
{
    public class Center
    {
        [Key]
        public int CenterId { get; set; }
        [DisplayName("Center Name")]
        public string CenterName { get; set; }
        [MaxLength(2)]
        public string Code { get; set; }
    }
}
