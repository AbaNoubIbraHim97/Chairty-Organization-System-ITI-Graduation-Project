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
    public class MonthlyCachingsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        //public void GetBalance()
        //{
        //    Balance balance = db.Balances.Where(b => b.ID == 1).FirstOrDefault();
        //}
        // GET: MonthlyCachings


        public ActionResult Index()
        {
            var monthlyCachings = db.MonthlyCachings.Include(m => m.Patient);
            return View(monthlyCachings.ToList());
        }
        public ActionResult BulkData()
        {
            List<MonthlyCaching> mc = new List<MonthlyCaching> { new MonthlyCaching { PatientID = 1, PatientName = "" } };
            return View(mc);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult BulkData(List<MonthlyCaching> ci)
        {
            if (ModelState.IsValid)
            {
                using (ApplicationDbContext dc = new ApplicationDbContext())
                {
                    foreach (var i in ci)
                    {
                        i.PatientID = 1;
                        dc.MonthlyCachings.Add(i);
                    }
                    dc.SaveChanges();
                    ViewBag.Message = "Data successfully saved!";
                    ModelState.Clear();
                    ci = new List<MonthlyCaching> { new MonthlyCaching {  PatientID = 1, PatientName = "" } };
                }
            }
            return View(ci);
        }
        // GET: MonthlyCachings/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MonthlyCaching monthlyCaching = db.MonthlyCachings.Find(id);
            if (monthlyCaching == null)
            {
                return HttpNotFound();
            }
            return View(monthlyCaching);
        }

        public double PMTotalCost(int Pid)
        {
            List<PatientMedicineTest> Costs =
            db.PatientMedicineTests.Where(S => S.PatientID == Pid).ToList();
            double TotalCost = 0;
            foreach (var item in Costs)
            {
                TotalCost += item.CostPerMedicineType;
            }
            return TotalCost;
        }
        // GET: MonthlyCachings/Create
        public ActionResult Create()
        {
            ViewBag.PatientL = db.Patients.Where(p => p.isalife == true).ToList();
           // List<Patient> PatientList = db.Patients.Where(p => p.isalife == true).ToList();
            return View();
            //List<double> c=new List<double>();
            //List<Patient> PatientList = db.Patients.Where(p=>p.isalife==true).ToList();
            //foreach (var item in PatientList)
            //{
            //  c.Add(PMTotalCost(item.ID));
            //}
            ////ViewBag.PatientID = new SelectList(db.Patients, "ID", "Name");
            //ViewBag.VBC = c;

        }
        //public void PatientMedicineDetails(int ID)
        //{
        //    MonthlyCaching MC = db.MonthlyCachings.Where(m => m.PatientID == ID).FirstOrDefault();
        //    List<PatientMedicineTest> Degrees =
        //        db.PatientMedicineTests.Where(S => S.PatientID == ID).ToList();
        //    double totalCost = 0;
        //    foreach (var item in Degrees)
        //    {
        //        totalCost += item.CostPerMedicineType;
        //    }
        //    MC.Cost = totalCost;
        //    db.SaveChanges();
        //}
        // POST: MonthlyCachings/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        //public ActionResult Create([Bind(Include = "PatientID,DateCreated,Cost,status")] MonthlyCaching monthlyCaching)
        public ActionResult Create(List<MonthlyCaching> ci)
        {

            //List<MonthlyCaching> MCN =
            //db.MonthlyCachings.Where(S => S.PatientID == monthlyCaching.PatientID).ToList();
            if (ModelState.IsValid)
            {
                foreach (var i in ci)
                {
                    db.MonthlyCachings.Add(i);
                    db.SaveChanges();
                }
              //  ModelState.Clear();
            //    ci = new List<MonthlyCaching> { new MonthlyCaching { PatientID = 0,  = 0, NumberOfUnits = 0 } };
                
            }
            return RedirectToAction("Index");
            //  ViewBag.PatientID = new SelectList(db.Patients, "ID", "Name", monthlyCaching.PatientID);
            //  return View(monthlyCaching);
            //  return View();
        }

        // GET: MonthlyCachings/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MonthlyCaching monthlyCaching = db.MonthlyCachings.Find(id);
            if (monthlyCaching == null)
            {
                return HttpNotFound();
            }
            //ViewBag.PatientList = db.Patients.ToList();
            ViewBag.PatientID = new SelectList(db.Patients, "ID", "Name", monthlyCaching.PatientID);
            return View(monthlyCaching);
        }

        // POST: MonthlyCachings/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "PatientID,DateCreated,Cost,status")] MonthlyCaching monthlyCaching)
        {
            if (ModelState.IsValid)
            {
                db.Entry(monthlyCaching).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.PatientID = new SelectList(db.Patients, "ID", "Name", monthlyCaching.PatientID);
            return View(monthlyCaching);
        }

        // GET: MonthlyCachings/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MonthlyCaching monthlyCaching = db.MonthlyCachings.Find(id);
            if (monthlyCaching == null)
            {
                return HttpNotFound();
            }
            return View(monthlyCaching);
        }

        // POST: MonthlyCachings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            MonthlyCaching monthlyCaching = db.MonthlyCachings.Find(id);
            db.MonthlyCachings.Remove(monthlyCaching);
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
