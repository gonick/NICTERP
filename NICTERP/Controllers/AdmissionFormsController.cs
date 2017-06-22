using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using NICTERP.Models;
using NICTERP.Models.View_Models;
using NICTERP.Models.NICT;
using System.IO;
using Newtonsoft.Json;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
namespace NICTERP.Controllers
{
    [Authorize]
    public class AdmissionFormsController : Controller
    {
        private NICTDbContext db = new NICTDbContext();
        protected int CenterID()
        {
            var manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
            var currentUser = manager.FindById(User.Identity.GetUserId());
            return currentUser.CenterID;
        }
        // GET: AdmissionForms
        public ActionResult Index()
        {
            List<AdmissionForm> admissionForms = db.AdmisionForms.ToList();
            return View(admissionForms);
        }

        // GET: AdmissionForms/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
                AdmissionFormViewModel myModel = db.GetViewModel((long)id);
            if (myModel == null)
            {
                return HttpNotFound();
            }
            
            return View(myModel);
        }

        // GET: AdmissionForms/Create
        public ActionResult Create()
        {
            List<SelectListItem> courses = new List<SelectListItem>();
            foreach (var course in db.Courses.ToList())
            {
                courses.Add(new SelectListItem { Value = course.Id.ToString(), Text = course.CourseName });
            }
            List<SelectListItem> items = new List<SelectListItem>();
            foreach (var center in db.Centers.ToList())
            {
                items.Add(new SelectListItem { Value = center.CenterId.ToString(), Text = center.CenterName });
            }
            ViewBag.Centers = items;
            ViewBag.Course_Applied = courses;
            AdmissionFormViewModel model = new AdmissionFormViewModel();
            return View(model);
        }

        // POST: AdmissionForms/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(AdmissionFormViewModel admissionFormViewModel)
        {

            if (ModelState.IsValid)
            {
                AdmissionForm AdmissionForm = new AdmissionForm();
                AdmissionForm.Dated = DateTime.UtcNow;
                AdmissionForm.centerId = admissionFormViewModel.StudentDetail.Center;
                db.AdmisionForms.Add(AdmissionForm);
                db.SaveChanges();
                long form_no = AdmissionForm.Form_No;

                //update formno for all tables
                admissionFormViewModel.Installment.Form_No = form_no;
                admissionFormViewModel.AboutNICT.Form_No = form_no;
                admissionFormViewModel.EduQualification.Form_No = form_no;
                admissionFormViewModel.StudentDetail.Form_No = form_no;
                //check for empty model properties
                if (!admissionFormViewModel.JobExperience.HasAllEmptyProperties())
                {
                    admissionFormViewModel.JobExperience.Form_No = form_no;
                    db.JobExperiences.Add(admissionFormViewModel.JobExperience);
                }
                if (!admissionFormViewModel.KnowledgeOfComp.HasAllEmptyProperties())
                {
                    admissionFormViewModel.KnowledgeOfComp.Form_No = form_no;
                    db.KnowledgeOfComps.Add(admissionFormViewModel.KnowledgeOfComp);
                }


                //check for uploaded files
                if (admissionFormViewModel.Files != null)
                {
                    foreach (var File in admissionFormViewModel.Files)
                    {
                        File.Form_No = form_no;
                        File.Datetime = AdmissionForm.Dated;
                        db.Files.Add(File);
                    }
                }

                admissionFormViewModel.Installment.Duedate = DateTime.UtcNow.AddDays(10);
                admissionFormViewModel.Installment.Datetime = AdmissionForm.Dated;
                admissionFormViewModel.Installment.Balance_Due = 3000 - admissionFormViewModel.Installment.Amount_Paid;

                //update student reg no
                int regNo = db.StudentDetails.Where(y => y.Center.Equals(admissionFormViewModel.StudentDetail.Center)).OrderByDescending(x => x.Reg_No).Select(s => s.Reg_No).FirstOrDefault();
                if (regNo == 0 || regNo == null)
                    regNo++;
                //set the reg number
                admissionFormViewModel.StudentDetail.Reg_No = regNo;


                //add all the models to db
                db.AboutNICTs.Add(admissionFormViewModel.AboutNICT);
                db.EduQualifications.Add(admissionFormViewModel.EduQualification);
                db.StudentDetails.Add(admissionFormViewModel.StudentDetail);
                db.Installments.Add(admissionFormViewModel.Installment);
                db.SaveChanges();

                return RedirectToAction("Details",form_no);
            }
            
            List<SelectListItem> items = new List<SelectListItem>();
            foreach (var center in db.Centers.ToList())
            {
                items.Add(new SelectListItem { Value = center.CenterId.ToString(), Text = center.CenterName });
            }
            ViewBag.Centers = items;
            return View(admissionFormViewModel);
        }

        // GET: AdmissionForms/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //AdmissionFormViewModel admissionFormViewModel = db.AdmissionFormViewModels.Find(id);
            //if (admissionFormViewModel == null)
            //{
            //    return HttpNotFound();
            //}
            return View();
        }

        // POST: AdmissionForms/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id")] AdmissionFormViewModel admissionFormViewModel)
        {
            if (ModelState.IsValid)
            {
                db.Entry(admissionFormViewModel).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(admissionFormViewModel);
        }

        // GET: AdmissionForms/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            // AdmissionFormViewModel admissionFormViewModel = db.AdmissionFormViewModels.Find(id);
            //if (admissionFormViewModel == null)
            //{
            //    return HttpNotFound();
            //}
            //return View(admissionFormViewModel);
            return View();
        }

        // POST: AdmissionForms/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            //AdmissionFormViewModel admissionFormViewModel = db.AdmissionFormViewModels.Find(id);
            // db.AdmissionFormViewModels.Remove(admissionFormViewModel);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        public PartialViewResult ShowPartialView()
        {
            return PartialView("_CaptureImagePartialView");
        }

        public string savephoto()
        {
            string imageData = Request.Form["photo"];
            string path = null;
            if (imageData != "")
            {
                DateTime date = DateTime.UtcNow;
                path = "Photo " + date.ToString() + ".png";
                path = path.Replace("/", "-");
                path = path.Replace(":", "-");
                string Pic_Path = HttpContext.Server.MapPath("~/Content/Images/" + path);
                using (FileStream fs = new FileStream(Pic_Path, FileMode.Create))
                {
                    using (BinaryWriter bw = new BinaryWriter(fs))
                    {
                        byte[] data = Convert.FromBase64String(imageData);
                        bw.Write(data);
                        bw.Close();
                    }
                }

            }
            return "../Content/Images/" + path;
        }

        public JsonResult searchStudent(string name,int batchId)
        {
            int _centerid = CenterID();
            string json= JsonConvert.SerializeObject(db.StudentDetails.Where(x=>(x.CenterName.CenterId==_centerid && (x.Name.Contains(name) || x.Reg_No.ToString().Contains(name) || x.Contact_No.Contains(name) || x.Date_Of_Joining.ToString().Contains(name)))).ToList());
            return Json(json,JsonRequestBehavior.AllowGet);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

    }
}
