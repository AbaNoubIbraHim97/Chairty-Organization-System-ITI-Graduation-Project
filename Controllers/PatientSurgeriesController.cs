using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    
    public class PatientSurgeriesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        [Authorize(Roles = "Administrator,المسئولون,المشرفون")]

        // GET: PatientSurgeries
        public ActionResult Index()
        {
            ViewBag.PatientID = new SelectList(db.Patients, "ID", "Name");
            ViewBag.SurgeryID = new SelectList(db.Surgeries, "ID", "Name");

            ViewBag.BalanceID = db.Balances.Where(b => b.ID == 3).FirstOrDefault().TotalValue;
            var patientSurgeries = db.PatientSurgeries.Include(p => p.Patient).Include(p => p.Surgery);
            return View(patientSurgeries.ToList());
        }
        [Authorize(Roles = "Administrator,المسئولون,المشرفون")]

        public ActionResult IndexMain()
        {
            ViewBag.BalanceID = db.Balances.Where(b => b.ID == 3).FirstOrDefault().TotalValue;
            ViewBag.PatientID = new SelectList(db.Patients, "ID", "Name");
            ViewBag.SurgeryID = new SelectList(db.Surgeries, "ID", "Name");
            var patientSurgeries = db.PatientSurgeries.Include(p => p.Surgery).Include(p => p.Patient);
            return View(patientSurgeries.ToList());
        }
        [Authorize(Roles = "Administrator,المسئولون,المشرفون")]

        public ActionResult Search(int PatientID)
        {
            ViewBag.BalanceID = db.Balances.Where(b => b.ID == 3).FirstOrDefault().TotalValue;
            ViewBag.PatientID = new SelectList(db.Patients, "ID", "Name");
            ViewBag.SurgeryID = new SelectList(db.Surgeries, "ID", "Name");
            List<PatientSurgery> Patients = db.PatientSurgeries.Where(c => c.PatientID.Equals(PatientID)).ToList();
            if (Patients == null)
            {
               
                ViewBag.NotFounf = "من فضلك أختر اسم مريض";
                return View("Index", Patients);
            }
            if (Patients.Count > 0)
            {
                return View("Index", Patients);
            }

            else
            {

                ViewBag.NotFounf = "لا توجد بيانات مطابقة للبحث";
                return View("Index", Patients);
            }
        }

        [Authorize(Roles = "Administrator,المسئولون,المشرفون")]
        // GET: PatientSurgeries/Details/5
        public ActionResult Details(int? id)
        {
            PatientSurgery tc = db.PatientSurgeries.Where(t => t.ID == id).FirstOrDefault();
            ViewBag.SurgeryID = db.Surgeries.Where(m => m.ID == tc.SurgeryID).FirstOrDefault();
            ViewBag.PatientID = db.Patients.Where(m => m.ID == tc.PatientID).FirstOrDefault();
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PatientSurgery patientSurgery = db.PatientSurgeries.Find(id);
            if (patientSurgery == null)
            {
                return HttpNotFound();
            }
            return View(patientSurgery);
        }

        // GET: PatientSurgeries/Create
        [Authorize(Roles = "Administrator,المسئولون")]

        public ActionResult Create()
        {
            ViewBag.PatientID = new SelectList(db.Patients, "ID", "Name");
            ViewBag.SurgeryID = new SelectList(db.Surgeries, "ID", "Name");
            ViewBag.BalanceID = db.Balances.Where(b => b.ID == 3).FirstOrDefault().TotalValue;
            return View();
        }
        //public void DecreaseBalanceBySurgeryCost(double Surgerycost)
        //{
        //    Balance balance = db.Balances.Where(b => b.ID == 3).FirstOrDefault();
        //    double CurrentBalance = balance.TotalValue - Surgerycost;
        //    balance.TotalValue = CurrentBalance;
        //    db.SaveChanges();

        //}
        // POST: PatientSurgeries/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator,المسئولون")]

        public ActionResult Create([Bind(Include = "ID,PatientID,SurgeryID,Place,DoctorName,Cost,DateCreated")] PatientSurgery patientSurgery)
        {
            if (ModelState.IsValid)
            {
                double currentBalance = db.Balances.Where(b => b.ID == 3).FirstOrDefault().TotalValue;
                if (patientSurgery.Cost <= currentBalance)
                {
                    double NewBalance = currentBalance - patientSurgery.Cost;
                    currentBalance = NewBalance;
                    db.Balances.Where(b => b.ID == 3).FirstOrDefault().TotalValue = currentBalance;
                    db.PatientSurgeries.Add(patientSurgery);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                else
                {
                    ViewBag.Cost = "التكلفة المطلوبة : " + patientSurgery.Cost + " ج";
                    ViewBag.Balance = "الرصيد الحالي غير كافئ";
                    ViewBag.BalanceID = db.Balances.Where(b => b.ID == 3).FirstOrDefault().TotalValue;

                }

            }

            ViewBag.PatientID = new SelectList(db.Patients, "ID", "Name", patientSurgery.PatientID);
            ViewBag.SurgeryID = new SelectList(db.Surgeries, "ID", "Name", patientSurgery.SurgeryID);
            return View(patientSurgery);
        }

        // GET: PatientSurgeries/Edit/5
        [Authorize(Roles = "Administrator,المسئولون")]

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PatientSurgery patientSurgery = db.PatientSurgeries.Find(id);
            if (patientSurgery == null)
            {
                return HttpNotFound();
            }
            ViewBag.BalanceID = db.Balances.Where(b => b.ID == 3).FirstOrDefault().TotalValue;
            ViewBag.PatientID = new SelectList(db.Patients, "ID", "Name", patientSurgery.PatientID);
            ViewBag.SurgeryID = new SelectList(db.Surgeries, "ID", "Name", patientSurgery.SurgeryID);
            return View(patientSurgery);
        }

        // POST: PatientSurgeries/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator,المسئولون")]

        public ActionResult Edit([Bind(Include = "ID,PatientID,SurgeryID,Place,DoctorName,Cost,DateCreated")] PatientSurgery patientSurgery)
        {
            if (ModelState.IsValid)
            {
                //get old cost then remove old
                PatientSurgery tc = db.PatientSurgeries.Where(t => t.ID == patientSurgery.ID).FirstOrDefault();
                double oldcost = tc.Cost;
                db.PatientSurgeries.Remove(tc);
                double currentBalance = db.Balances.Where(b => b.ID == 3).FirstOrDefault().TotalValue;
                //add the old cost to balance
                double NewBalance = currentBalance + oldcost;
                currentBalance = NewBalance;
                db.Balances.Where(b => b.ID == 3).FirstOrDefault().TotalValue = currentBalance;

                if (patientSurgery.Cost <= currentBalance)
                {
                    // Add new cost and new object
                    double NB = db.Balances.Where(b => b.ID == 3).FirstOrDefault().TotalValue;
                    double NewBalance2 = NB - patientSurgery.Cost;
                    NB = NewBalance2;
                    db.Balances.Where(b => b.ID == 3).FirstOrDefault().TotalValue = NB;
                    db.PatientSurgeries.Add(patientSurgery);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                else
                {
                    ViewBag.Cost = "التكلفة المطلوبة : " + patientSurgery.Cost + " ج";
                    ViewBag.Balance = "الرصيد الحالي غير كافئ";
                    ViewBag.BalanceID = db.Balances.Where(b => b.ID == 3).FirstOrDefault().TotalValue;

                }
                //db.Entry(patientSurgery).State = EntityState.Modified;
                
                
            }
            ViewBag.PatientID = new SelectList(db.Patients, "ID", "Name", patientSurgery.PatientID);
            ViewBag.SurgeryID = new SelectList(db.Surgeries, "ID", "Name", patientSurgery.SurgeryID);
            return View(patientSurgery);
        }

        // GET: PatientSurgeries/Delete/5
        [Authorize(Roles = "Administrator,المسئولون")]

        public ActionResult Delete(int? id)
        {
            PatientSurgery tc = db.PatientSurgeries.Where(t => t.ID == id).FirstOrDefault();
            ViewBag.SurgeryID = db.Surgeries.Where(m => m.ID == tc.SurgeryID).FirstOrDefault();
            ViewBag.PatientID = db.Patients.Where(m => m.ID == tc.PatientID).FirstOrDefault();

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PatientSurgery patientSurgery = db.PatientSurgeries.Find(id);
            if (patientSurgery == null)
            {
                return HttpNotFound();
            }
            return View(patientSurgery);
        }

        // POST: PatientSurgeries/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator,المسئولون")]

        public ActionResult DeleteConfirmed(int id)
        {
            PatientSurgery patientSurgery = db.PatientSurgeries.Find(id);
            double currentBalance = db.Balances.Where(b => b.ID == 3).FirstOrDefault().TotalValue;
            double NewBalance = currentBalance + patientSurgery.Cost;
            currentBalance = NewBalance;
            db.Balances.Where(b => b.ID == 3).FirstOrDefault().TotalValue = currentBalance;
            db.PatientSurgeries.Remove(patientSurgery);
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
