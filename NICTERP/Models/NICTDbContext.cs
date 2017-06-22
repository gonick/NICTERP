using NICTERP.Models.NICT;
using NICTERP.Models.View_Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace NICTERP.Models
{
    public class NICTDbContext : DbContext
    {
        public NICTDbContext()
            : base("DefaultConnection")
        { }

        public DbSet<AdmissionForm> AdmisionForms { get; set; }
        public DbSet<AboutNICT> AboutNICTs { get; set; }
        public DbSet<EduQualification> EduQualifications { get; set; }
        public DbSet<File> Files { get; set; }
        public DbSet<Installment> Installments { get; set; }
        public DbSet<JobExperience> JobExperiences { get; set; }
        public DbSet<KnowledgeOfComp> KnowledgeOfComps { get; set; }
        public DbSet<StudentDetail> StudentDetails { get; set; }
        public DbSet<Center> Centers { get; set; }
        public DbSet<Batch> Batches { get; set; }
        public DbSet<BatchMember> BatchMember { get; set; }
        public DbSet<Courses> Courses { get; set; }
        public AdmissionFormViewModel GetViewModel(long formno)
        {
            try
            {
                NICTDbContext db = new NICTDbContext();
                AdmissionFormViewModel myModel = new AdmissionFormViewModel()
                {
                    AboutNICT = db.AboutNICTs.SingleOrDefault(x => x.Form_No == formno),
                    AdmissionForm = db.AdmisionForms.SingleOrDefault(x => x.Form_No == formno),
                    EduQualification = db.EduQualifications.SingleOrDefault(x => x.Form_No == formno),
                    Files = db.Files.Where(x => x.Form_No == formno).ToList(),
                    Installment = db.Installments.Where(x => x.Form_No == formno).OrderByDescending(y => y.Datetime).First(),
                    JobExperience = db.JobExperiences.SingleOrDefault(x => x.Form_No == formno),
                    KnowledgeOfComp = db.KnowledgeOfComps.SingleOrDefault(x => x.Form_No == formno),
                    StudentDetail = db.StudentDetails.SingleOrDefault(x => x.Form_No == formno),
                };
                return myModel;
            }
            catch(Exception ex)
            {
                return null;
            }
        }


    }
}