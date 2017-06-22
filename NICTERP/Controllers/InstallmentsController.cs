using NICTERP.Models;
using NICTERP.Models.NICT;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;

namespace NICTERP.Controllers
{
    public class InstallmentsController : Controller
    {
        private NICTDbContext db = new NICTDbContext();
        // GET: Installments

        public ActionResult Index()
        {
            return View();
        }

        // GET: Installments/Details/5
        public PartialViewResult Details(long form_no)
        {
            List<Installment> i = db.Installments.Where(x => x.Form_No == form_no).OrderByDescending(x => x.Datetime).ToList();
            return PartialView("_InstallmentsPartialView", i);
        }

        // GET: Installments/Create
        public PartialViewResult Create(long formno)
        {
            ViewBag.FormNo = formno;
            return PartialView("_AddInstallmentPartialView", new Installment() { Form_No = formno });
        }

        // POST: Installments/Create
        [HttpPost]
        public ActionResult Create(Installment i)
        {
            try
            {
                if (i.Form_No != 0 && i.Amount_Paid > 0)
                {
                    int balanceDue = db.Installments.Where(x => x.Form_No == i.Form_No).OrderByDescending(x => x.Datetime).First().Balance_Due;
                    if (i.Amount_Paid > balanceDue)
                        return Json("{ \"success\":false,\"errorMsg\":\"Please enter a valid amount \"}", JsonRequestBehavior.AllowGet);
                    if (balanceDue > 0)
                    {
                        i.Balance_Due = balanceDue - i.Amount_Paid;
                        i.Datetime = DateTime.UtcNow;
                        if (i.Balance_Due == 0)
                        { //i.Duedate = ""; 
                            //check if form is complete
                            long formno = i.Form_No;
                            int stuId = db.StudentDetails.Where(w => w.Form_No == formno).Select(s => s.id).ToArray()[0];
                            if (db.Installments.Where(w => w.Form_No == formno).OrderByDescending(o => o.Datetime).Select(s => s.Balance_Due).Take(1).ToArray()[0] == 0
                                    || db.BatchMember.Any(a => a.studentId == stuId))
                            {
                                AdmissionForm ad = db.AdmisionForms.Find(formno);
                                ad.Form_Complete = true;
                            }
                        }
                        else
                            i.Duedate = i.Datetime.AddDays(7);
                        db.Installments.Add(i);
                        db.SaveChanges();
                        return Json("{ \"success\":true,\"id\":\"" + i.Id + "\"}", JsonRequestBehavior.AllowGet);
                    }
                    return Json("{ \"success\":false,\"errorMsg\":\"Balance due is zero\"}", JsonRequestBehavior.AllowGet);
                }

                return Json("{ \"success\":false,\"errorMsg\":\"Amount Paid should be > 0 \"}", JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json("{ \"success\":false,\"errorMsg\":\"Exception Occured: " + ex.Message + " \"}", JsonRequestBehavior.AllowGet);
            }
        }

        // GET: Installments/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Installments/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Installments/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Installments/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
