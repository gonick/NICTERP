using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NICTERP.Models;
using NICTERP.Models.View_Models;
using NICTERP.Models.NICT;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Newtonsoft.Json;
using System.Text;
namespace NICTERP.Controllers
{
    [Authorize]
    public class DashboardController : Controller
    {
        private NICTDbContext db = new NICTDbContext();
        // GET: Dashboard
        protected int CenterID()
        {
            var manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
            var currentUser = manager.FindById(User.Identity.GetUserId());
            return currentUser.CenterID;
        }
        public ActionResult Index()
        {
            return View();
        }

        public PartialViewResult IncompleteForms()
        {
            int _centerid = CenterID();
            List<StudentDetail> students = db.StudentDetails.Where(x => x.AdmissionForm.Form_Complete == false && x.CenterName.CenterId == _centerid).OrderByDescending(y => y.AdmissionForm.Dated).Take(5).ToList();
            ViewBag.Count = db.StudentDetails.Where(x => x.AdmissionForm.Form_Complete == false && x.CenterName.CenterId == _centerid).Count();
            return PartialView("IncompleteForms", students);
        }

        public PartialViewResult PendingBalances()
        {
            int _centerid = CenterID();
            var a = from p in db.Installments
                    where p.AdmissionForm.Center.CenterId == _centerid
                    group p.Balance_Due by p.Form_No;
            int count = 0;
            foreach (var b in a)
            {
                long length = b.Count();
                var c = b.ToArray();
                if (c[length - 1] != 0)
                    count++;
            }
            @ViewBag.Count = count;
            List<long> formNos = db.AdmisionForms.Where(w => w.Center.CenterId == _centerid).OrderByDescending(o => o.Dated).Take(5).Select(x => x.Form_No).ToList();
            List<PendingBalances> pendingBalances = new List<PendingBalances>();
            foreach (long formno in formNos)
            {
                PendingBalances pendingBalance = new PendingBalances();
                pendingBalance.Installment = db.Installments.Where(w => w.Form_No == formno && w.AdmissionForm.Center.CenterId == _centerid).OrderByDescending(x => x.Datetime).First();
                pendingBalance.StudentDetail = db.StudentDetails.Where(w => w.Form_No == formno && w.CenterName.CenterId == _centerid).FirstOrDefault();
                pendingBalances.Add(pendingBalance);
            }
            return PartialView(pendingBalances);
        }

        public ActionResult TabIncompleteForms()
        {
            int _centerid = CenterID();
            List<StudentDetail> students = db.StudentDetails.Where(x => x.AdmissionForm.Form_Complete == false && x.CenterName.CenterId == _centerid).OrderByDescending(y => y.AdmissionForm.Dated).Take(5).ToList();
            string json = JsonConvert.SerializeObject(students);
            var sb = new StringBuilder();
            sb.AppendFormat("retry: 300000\n\ndata: {0}\n\n", json);
            return Content(sb.ToString(), "text/event-stream");
        }

        public ActionResult TabPendingBalances()
        {
            int _centerId = CenterID();
            List<long> formNos = db.AdmisionForms.Where(w => w.Center.CenterId == _centerId && w.Form_Complete==false).OrderByDescending(o => o.Dated).Select(x => x.Form_No).ToList();
            List<PendingBalances> _pendingBalances = new List<PendingBalances>();
            foreach (long formno in formNos)
            {
                PendingBalances pendingBalance = new PendingBalances();
                pendingBalance.Installment = db.Installments.Where(w => w.Form_No == formno && w.AdmissionForm.Center.CenterId == _centerId).OrderByDescending(x => x.Datetime).First();
                pendingBalance.StudentDetail = db.StudentDetails.Where(w => w.Form_No == formno && w.CenterName.CenterId == _centerId).FirstOrDefault();
                if (pendingBalance.Installment.Balance_Due != 0)
                    _pendingBalances.Add(pendingBalance);
            }
            string json = JsonConvert.SerializeObject(_pendingBalances);
            var sb = new StringBuilder();
            sb.AppendFormat("retry: 300000\n\ndata: {0}\n\n", json);
            return Content(sb.ToString(), "text/event-stream");
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
