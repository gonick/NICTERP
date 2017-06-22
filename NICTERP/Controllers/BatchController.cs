using NICTERP.Models;
using NICTERP.Models.NICT;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NICTERP.Controllers
{
    [Authorize]
    public class BatchController : Controller
    {
        private NICTDbContext db = new NICTDbContext();

        // GET: Batch
        public ActionResult Index()
        {
            List<Batch> batch = db.Batches.Where(w=>w.CreatedBy==User.Identity.Name && w.isDisabled==false).ToList();
            return View(batch);
        }
        [HttpGet]
        public ActionResult Create()
        {

            return View(new Batch() { BatchName="Batch 1 "+DateTime.Now.ToString("MMM")+" "+DateTime.Now.Year});
        }
        [HttpPost]
        public ActionResult Create(Batch batch)
        {
            if (ModelState.IsValid)
            {
                batch.isDisabled = false;
                batch.DateCreated = DateTime.UtcNow;
                batch.CreatedBy = User.Identity.Name;
                db.Batches.Add(batch);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View();
        }

        public ActionResult AddStudent(int id)
        {
            ViewBag.Batch = db.Batches.Find(id);
            return View();
        }
        
        public JsonResult AddStudentAsync(int[] studentIds, int BatchId)
        {
            try 
            {
                foreach(int id in studentIds)
                {
                    BatchMember batchmember = new BatchMember() { BatchId = BatchId, studentId = id };
                    db.BatchMember.Add(batchmember);
                    db.SaveChanges();
                    long formno = db.StudentDetails.Find(id).Form_No;
                    if (db.Installments.Where(w => w.Form_No == formno).OrderByDescending(o => o.Datetime).Select(s => s.Balance_Due).Take(1).ToArray()[0] == 0
                        && db.BatchMember.Any(a=>a.studentId==id))
                    {
                        AdmissionForm ad = db.AdmisionForms.Find(formno);
                        ad.Form_Complete = true;
                        db.SaveChanges();
                    }
                }
               
             return Json("{ \"success\":true}");
            }
            catch(Exception ex)
            {
             return Json("{ \"success\":false}");
            }
           
        }

        public PartialViewResult BatchMembers(int id)
        {
            List<BatchMember> _batchMembers = db.BatchMember.Where(x => x.BatchId == id).ToList();
            return PartialView("_BatchMembers", _batchMembers);
        }

        public bool DeleteMember(int id)
        {
            BatchMember _bm = db.BatchMember.Find(id);
            long formno = _bm.Student.Form_No;
            db.BatchMember.Remove(_bm);
            bool result = Convert.ToBoolean(db.SaveChanges());
            if (db.Installments.Where(w => w.Form_No == formno).OrderByDescending(o => o.Datetime).Select(s => s.Balance_Due).Take(1).ToArray()[0] != 0
                        || !db.BatchMember.Any(a => a.studentId == id))
            {
                AdmissionForm ad = db.AdmisionForms.Find(formno);
                ad.Form_Complete = false;
                db.SaveChanges();
            }
            return result;
        }

        public RedirectToRouteResult Disable(int id)
        {
            Batch batch = db.Batches.Find(id);
            batch.isDisabled = true;
            db.SaveChanges();

            return RedirectToAction("Index");
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