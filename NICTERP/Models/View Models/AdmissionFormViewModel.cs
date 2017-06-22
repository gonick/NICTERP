using NICTERP.Models.NICT;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NICTERP.Models.View_Models
{
    public class AdmissionFormViewModel
    {
        public int id { get; set; }
        public AdmissionForm AdmissionForm { get; set; }
        public StudentDetail StudentDetail { get; set; }
        public JobExperience JobExperience { get; set; }
        public AboutNICT AboutNICT { get; set; }
        public EduQualification EduQualification { get; set; }
        public List<File> Files { get; set; }
        public KnowledgeOfComp KnowledgeOfComp { get; set; }
        public Installment Installment { get; set; }
    }
}