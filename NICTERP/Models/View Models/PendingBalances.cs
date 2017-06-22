using NICTERP.Models.NICT;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NICTERP.Models.View_Models
{
    public class PendingBalances
    {
        public int id { get; set; }
        public StudentDetail StudentDetail { get; set; }
        public Installment Installment { get; set; }
    }
}