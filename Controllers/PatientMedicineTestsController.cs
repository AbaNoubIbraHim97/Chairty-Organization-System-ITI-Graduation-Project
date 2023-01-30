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
    public class PatientMedicineTestsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        [Authorize(Roles = "Administrator,المسئولون,المشرفون")]

        // GET: PatientMedicineTests
        public ActionResult Index()
        {
            ViewBag.PatientID = new SelectList(db.Patients, "ID", "Name");
            ViewBag.MedicineID = new SelectList(db.Medicines, "ID", "Name");
            var patientMedicineTests = db.PatientMedicineTests.Include(p => p.Medicines).Include(p => p.Patient);
            return View(patientMedicineTests.ToList());
        }
        [Authorize(Roles = "Administrator,المسئولون,المشرفون")]

        public ActionResult IndexMain()
        {
            ViewBag.PatientID = new SelectList(db.Patients, "ID", "Name");
            ViewBag.MedicineID = new SelectList(db.Medicines, "ID", "Name");
            var patientMedicines = db.PatientMedicineTests.Include(p => p.Medicines).Include(p => p.Patient);
            return View(patientMedicines.ToList());
        }
        [Authorize(Roles = "Administrator,المسئولون,المشرفون")]

        public ActionResult Search(int PatientID)
        {
            ViewBag.PatientID = new SelectList(db.Patients, "ID", "Name");
            ViewBag.MedicineID = new SelectList(db.Medicines, "ID", "Name");
            List<PatientMedicineTest> Patients = db.PatientMedicineTests.Where(c => c.PatientID.Equals(PatientID)).ToList();
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

        // GET: PatientMedicineTests/Details/5
        public ActionResult Details(int? id)
        {
            PatientMedicineTest tc = db.PatientMedicineTests.Where(t => t.ID == id).FirstOrDefault();
            ViewBag.MedicineID = db.Medicines.Where(m => m.ID == tc.MedicineID).FirstOrDefault();
            ViewBag.PatientID = db.Patients.Where(m => m.ID == tc.PatientID).FirstOrDefault();

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            
            PatientMedicineTest patientMedicineTest = db.PatientMedicineTests.SingleOrDefault(m => m.ID == id);
            if (patientMedicineTest == null)
            {
                return HttpNotFound();
            }
            return View(patientMedicineTest);
        }
        //public ActionResult Createe()
        //{

        //    ViewBag.MedicineID = new SelectList(db.Medicines, "ID", "Name");
        //    ViewBag.PatientID = new SelectList(db.Patients, "ID", "Name");
        //    return View();
        //}
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Createe([Bind(Include = "PatientID,MedicineID,NumberOfUnits,CostPerMedicineType")] PatientMedicineTest patientMedicineTest)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.PatientMedicineTests.Add(patientMedicineTest);
        //        db.SaveChanges();
        //        CalcCostByUnitsNumber(patientMedicineTest.PatientID, patientMedicineTest.MedicineID, patientMedicineTest.NumberOfUnits);

        //    }
        //    ViewBag.MedicineID = new SelectList(db.Medicines, "ID", "Name", patientMedicineTest.MedicineID);
        //    ViewBag.PatientID = new SelectList(db.Patients, "ID", "Name", patientMedicineTest.PatientID);
        //    return View(patientMedicineTest);
        //}
        // GET: PatientMedicineTests/Create
        //public ActionResult Create(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Patient patient = db.Patients.Find(id);
        //    if (patient == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    ViewBag.MedicineID = new SelectList(db.Medicines, "ID", "Name");
        //    ViewBag.PatientID = new SelectList(db.Patients, "ID", "Name", patient.ID);
        //    return View();
        //}

        // POST: PatientMedicineTests/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Create([Bind(Include = "PatientID,MedicineID,NumberOfUnits,CostPerMedicineType")] PatientMedicineTest patientMedicineTest)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.PatientMedicineTests.Add(patientMedicineTest);
        //        db.SaveChanges();
        //        CalcCostByUnitsNumber(patientMedicineTest.PatientID, patientMedicineTest.MedicineID, patientMedicineTest.NumberOfUnits);
        //        //return RedirectToAction("create");
        //    }
        //    ViewBag.MedicineID = new SelectList(db.Medicines, "ID", "Name", patientMedicineTest.MedicineID);
        //    ViewBag.PatientID = new SelectList(db.Patients, "ID", "Name", patientMedicineTest.PatientID);
        //    return View(patientMedicineTest);
        //}

        
        public void CalcCostByUnitsNumber(int PatientID, int MedicineID, int UnitsNumber)
        {
            Medicines Medicine = db.Medicines.Where(m => m.ID == MedicineID).FirstOrDefault();
            PatientMedicineTest PMT = db.PatientMedicineTests.Where(t => t.PatientID == PatientID && t.MedicineID == MedicineID && t.NumberOfUnits == UnitsNumber).FirstOrDefault();
            //TemporaryCashing temporaryCashing = db.TemporaryCashings.Where(t => t.PatientID == PatientID && t.MedicineID == MedicineID && t.NumberofUnites == UnitsNumber).FirstOrDefault();
            double CostPerUnits = Medicine.PricePerUnit * UnitsNumber;
            PMT.CostPerMedicineType = CostPerUnits;
            db.SaveChanges();
        }

        [Authorize(Roles = "Administrator,المسئولون")]

        public ActionResult BulkData()
        {
            ViewBag.MedicineID = new SelectList(db.Medicines, "ID", "Name");
            ViewBag.PatientID = new SelectList(db.Patients, "ID", "Name");
            List<PatientMedicineTest> pd = new List<PatientMedicineTest> { new PatientMedicineTest { PatientID = 0, MedicineID = 0, NumberOfUnits = 0 } };
            return View(pd);
        }
        [HttpPost]
        [Authorize(Roles = "Administrator,المسئولون")]

        [ValidateAntiForgeryToken]
        public ActionResult BulkData(List<PatientMedicineTest> ci)
        {

            if (ModelState.IsValid)
            {
                using (ApplicationDbContext dc = new ApplicationDbContext())
                {
                    int Correctcounter = 0;
                    int Wrongcounter = 0;
                    int Counter = 0;
                    foreach (var i in ci)
                    {
                        Counter++;
                        if (i.PatientID == 0 || i.MedicineID == 0 || i.NumberOfUnits == 0)
                        {
                            Correctcounter = 0;
                            Wrongcounter = 0;
                            Counter = 0;
                        }
                        else
                        {
                            PatientMedicineTest p = db.PatientMedicineTests.Where(c => c.PatientID == i.PatientID && c.MedicineID == i.MedicineID).FirstOrDefault();
                            if (p == null)
                            {
                                dc.PatientMedicineTests.Add(i);
                                Correctcounter++;
                                dc.SaveChanges();
                                CalcCostByUnitsNumber(i.PatientID, i.MedicineID, i.NumberOfUnits);
                                PMTotalCost(i.PatientID);
                            }
                            else
                            {
                                Wrongcounter++;
                            }

                               
                            
                        }

                    }
                    if (Counter == 1)
                    {
                        if (Wrongcounter == 1)
                        {
                            ViewBag.Repeat = "مضاف بالفعل !!";
                        }
                        if (Correctcounter == 1)
                        {
                            ViewBag.Repeat = "تمت الإضافة بنجاح !!";
                        }
                    }
                    if (Counter >= 2)
                    {
                        if (Correctcounter == Counter)
                        {
                            ViewBag.Repeat = "تمت الإضافة بنجاح !!";
                        }
                        else if (Wrongcounter == Counter)
                        {
                            ViewBag.Repeat = "مضاف بالفعل !!";
                        }
                        else
                        {
                            ViewBag.Repeat = "تمت إضافة العناصر الجديدة فقط !!";
                        }
                    }
                    // dc.SaveChanges();
                    //ViewBag.Message = "تمت الإضافة بنجاح";
                    ModelState.Clear();
                    ci = new List<PatientMedicineTest> { new PatientMedicineTest { PatientID = 0, MedicineID = 0, NumberOfUnits = 0 } };
                }
            }
            ViewBag.MedicineID = new SelectList(db.Medicines, "ID", "Name");
            ViewBag.PatientID = new SelectList(db.Patients, "ID", "Name");
            return View(ci);
        }
        [Authorize(Roles = "Administrator,المسئولون")]

        public ActionResult BulkDataa(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Patient patient = db.Patients.Find(id);
            if (patient == null)
            {
                return HttpNotFound();
            }
            ViewBag.MedicineID = new SelectList(db.Medicines, "ID", "Name");
            ViewBag.PatientID = new SelectList(db.Patients, "ID", "Name", patient.ID);
            List<PatientMedicineTest> pd = new List<PatientMedicineTest> { new PatientMedicineTest { PatientID = 0, MedicineID = 0, NumberOfUnits = 0 } };
            return View(pd);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator,المسئولون")]

        public ActionResult BulkDataa(List<PatientMedicineTest> ci)
        {

            if (ModelState.IsValid)
            {
                using (ApplicationDbContext dc = new ApplicationDbContext())
                {
                    int Correctcounter = 0;
                    int Wrongcounter = 0;
                    int Counter = 0;
                    foreach (var i in ci)
                    {
                        Counter++;
                        if (i.PatientID == 0 || i.MedicineID == 0 || i.NumberOfUnits == 0)
                        {
                            Correctcounter = 0;
                            Wrongcounter = 0;
                            Counter = 0;
                        }
                        else
                        {
                            PatientMedicineTest p = db.PatientMedicineTests.Where(c => c.PatientID == i.PatientID && c.MedicineID == i.MedicineID).FirstOrDefault();
                            if (p == null)
                            {
                                dc.PatientMedicineTests.Add(i);
                                Correctcounter++;
                                dc.SaveChanges();
                                CalcCostByUnitsNumber(i.PatientID, i.MedicineID, i.NumberOfUnits);
                            }
                            else
                            {
                                Wrongcounter++;
                            }
                        }

                    }

                    if (Counter == 1)
                    {
                        if (Wrongcounter == 1)
                        {
                            ViewBag.Repeat = "مضاف بالفعل !!";
                        }
                        if (Correctcounter == 1)
                        {
                            ViewBag.Repeat = "تمت الإضافة بنجاح !!";
                        }
                    }
                    if (Counter >= 2)
                    {
                        if (Correctcounter == Counter)
                        {
                            ViewBag.Repeat = "تمت الإضافة بنجاح !!";
                        }
                        else if (Wrongcounter == Counter)
                        {
                            ViewBag.Repeat = "مضاف بالفعل !!";
                        }
                        else
                        {
                            ViewBag.Repeat = "تمت إضافة العناصر الجديدة فقط !!";
                        }
                    }
                   // dc.SaveChanges();
                   // ViewBag.Message = "تمت الإضافة بنجاح";
                    ModelState.Clear();      
                    ci = new List<PatientMedicineTest> { new PatientMedicineTest { PatientID = 0, MedicineID = 0 ,NumberOfUnits=0} };
                }
            }
            ViewBag.MedicineID = new SelectList(db.Medicines, "ID", "Name");
            ViewBag.PatientID = new SelectList(db.Patients, "ID", "Name");
            return View(ci);
        }

        [Authorize(Roles = "Administrator,المسئولون")]

        // GET: PatientMedicineTests/Edit/5
        public ActionResult Edit(int? id)
        {
            PatientMedicineTest tc = db.PatientMedicineTests.Where(t => t.ID == id).FirstOrDefault();
            ViewBag.MedicineID = db.Medicines.Where(m => m.ID == tc.MedicineID).FirstOrDefault();
            ViewBag.PatientID = db.Patients.Where(m => m.ID == tc.PatientID).FirstOrDefault();
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PatientMedicineTest patientMedicineTest = db.PatientMedicineTests.SingleOrDefault(m => m.ID == id);
            if (patientMedicineTest == null)
            {
                return HttpNotFound();
            }
            ViewBag.MedicineID = new SelectList(db.Medicines, "ID", "Name", patientMedicineTest.MedicineID);
            ViewBag.PatientID = new SelectList(db.Patients, "ID", "Name", patientMedicineTest.PatientID);
            return View(patientMedicineTest);
        }
        [Authorize(Roles = "Administrator,المسئولون")]

        // POST: PatientMedicineTests/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,PatientID,MedicineID,NumberOfUnits,CostPerMedicineType")] PatientMedicineTest patientMedicineTest)
        {
            if (ModelState.IsValid)
            {
                PatientMedicineTest tc = db.PatientMedicineTests.Where(t => t.ID == patientMedicineTest.ID).FirstOrDefault();
                db.PatientMedicineTests.Remove(tc);
                db.PatientMedicineTests.Add(patientMedicineTest);
                db.SaveChanges();
                
                CalcCostByUnitsNumber(patientMedicineTest.PatientID, patientMedicineTest.MedicineID, patientMedicineTest.NumberOfUnits);
                PMTotalCost(patientMedicineTest.PatientID);
                return RedirectToAction("Index");
            }
            ViewBag.MedicineID = new SelectList(db.Medicines, "ID", "Name", patientMedicineTest.MedicineID);
            ViewBag.PatientID = new SelectList(db.Patients, "ID", "Name", patientMedicineTest.PatientID);
            return View(patientMedicineTest);
        }
        public void PMTotalCost(int Pid)
        {
            List<PatientMedicineTest> Costs =
            db.PatientMedicineTests.Where(S => S.PatientID == Pid).ToList();
            double TotalCost = 0;
            foreach (var item in Costs)
            {
                TotalCost += item.CostPerMedicineType;
            }
            Patient p = db.Patients.Where(pp => pp.ID == Pid).FirstOrDefault();
            p.PMTotalcost = TotalCost;
            db.SaveChanges();
        }

        [Authorize(Roles = "Administrator,المسئولون")]

        // GET: PatientMedicineTests/Delete/5
        public ActionResult Delete(int? id)
        {
            PatientMedicineTest tc = db.PatientMedicineTests.Where(t => t.ID == id).FirstOrDefault();
            ViewBag.MedicineID = db.Medicines.Where(m => m.ID == tc.MedicineID).FirstOrDefault();
            ViewBag.PatientID = db.Patients.Where(m => m.ID == tc.PatientID).FirstOrDefault();
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PatientMedicineTest patientMedicineTest = db.PatientMedicineTests.SingleOrDefault(m => m.ID == id);
            if (patientMedicineTest == null)
            {
                return HttpNotFound();
            }
            return View(patientMedicineTest);
        }
        [Authorize(Roles = "Administrator,المسئولون")]

        // POST: PatientMedicineTests/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {

            PatientMedicineTest patientMedicineTest = db.PatientMedicineTests.SingleOrDefault(m => m.ID == id);
            double old = patientMedicineTest.CostPerMedicineType;
            Patient pp = db.Patients.Where(p => p.ID == patientMedicineTest.PatientID).FirstOrDefault();
            double CurrentTotal = pp.PMTotalcost;
            double NewTotal = CurrentTotal - old;
            pp.PMTotalcost = NewTotal;
            //PMTotalCostDelete(id, old);
            db.PatientMedicineTests.Remove(patientMedicineTest);

            db.SaveChanges();
           // PMTotalCost(id);
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
