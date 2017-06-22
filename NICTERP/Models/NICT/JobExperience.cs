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
    public class JobExperience
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("AdmissionForm")]
        public long Form_No { get; set; }

        public string Organization { get; set; }

        public string Duration { get; set; }
        [DisplayName("Nature of Work")]
        public string Nature_of_Work { get; set; }

        public string Designation { get; set; }

        public virtual AdmissionForm AdmissionForm { get; set; }
        public bool HasAllEmptyProperties()
        {
            var type = GetType();
            var properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            var hasProperty = properties.Select(x => x.GetValue(this, null))
                                        .Any(y => y != null && !String.IsNullOrWhiteSpace(y.ToString()) && y.ToString() != "0");
            return !hasProperty;
        }
    }
}