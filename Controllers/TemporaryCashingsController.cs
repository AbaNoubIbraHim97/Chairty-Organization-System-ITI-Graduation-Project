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
    public class TemporaryCashingsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
    
        [Authorize(Roles = "Administrator,المسئولون,المشرفون")]

        // GET: TemporaryCashings
        public ActionResult Index()
        {
            ViewBag.MedicineID = new SelectList(db.Medicines, "ID", "Name");
            ViewBag.PatientID = new SelectList(db.Patients, "ID", "Name");

            ViewBag.BalanceID = db.Balances.Where(b => b.ID == 3).FirstOrDefault().TotalValue;
            var temporaryCashings = db.TemporaryCashings.Include(t => t.Medicines).Include(t => t.Patient);
            return View(temporaryCashings.ToList());
        }
        [Authorize(Roles = "Administrator,المسئولون,المشرفون")]

        public ActionResult IndexMain()
        {
            ViewBag.BalanceID = db.Balances.Where(b => b.ID == 3).FirstOrDefault().TotalValue;
            ViewBag.PatientID = new SelectList(db.Patients, "ID", "Name");
            ViewBag.MedicineID = new SelectList(db.Medicines, "ID", "Name");
            var temporaryCashings = db.TemporaryCashings.Include(p => p.Medicines).Include(p => p.Patient);
            return View(temporaryCashings.ToList());
        }
        [Authorize(Roles = "Administrator,المسئولون,المشرفون")]

        public ActionResult Search(int PatientID)
        {
            ViewBag.BalanceID = db.Balances.Where(b => b.ID == 3).FirstOrDefault().TotalValue;
            ViewBag.PatientID = new SelectList(db.Patients, "ID", "Name");
            ViewBag.MedicineID = new SelectList(db.Medicines, "ID", "Name");
            List<TemporaryCashing> Patients = db.TemporaryCashings.Where(c => c.PatientID.Equals(PatientID)).ToList();
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

        // GET: TemporaryCashings/Details/5
        public ActionResult Details(int? id)
        {
            TemporaryCashing tc = db.TemporaryCashings.Where(t => t.ID == id).FirstOrDefault();
            ViewBag.MedicineID = db.Medicines.Where(m => m.ID == tc.MedicineID).FirstOrDefault();
            ViewBag.PatientID = db.Patients.Where(m => m.ID == tc.PatientID).FirstOrDefault();
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TemporaryCashing temporaryCashing = db.TemporaryCashings.Find(id);
            if (temporaryCashing == null)
            {
                return HttpNotFound();
            }
            return View(temporaryCashing);
        }

        [Authorize(Roles = "Administrator,المسئولون")]

        public ActionResult BulkData()
        {
            ViewBag.MedicineID = new SelectList(db.Medicines, "ID", "Name");
            ViewBag.PatientID = new SelectList(db.Patients, "ID", "Name");
            ViewBag.BalanceID = db.Balances.Where(b => b.ID == 3).FirstOrDefault().TotalValue;
            List<TemporaryCashing> pd = new List<TemporaryCashing> { new TemporaryCashing { PatientID = 0, MedicineID = 0, NumberofUnites = 0 } };
            return View(pd);
        }
        [HttpPost]
        [Authorize(Roles = "Administrator,المسئولون")]

        [ValidateAntiForgeryToken]
        public ActionResult BulkData(List<TemporaryCashing> ci)
        {

            if (ModelState.IsValid)
            {

                
                using (ApplicationDbContext db = new ApplicationDbContext())
                {
                    double theBigTotal = 0;
                    double currentBalance = db.Balances.Where(b => b.ID == 3).FirstOrDefault().TotalValue;
                    foreach (var i in ci)
                    {

                        if (i.PatientID == 0 || i.MedicineID == 0 || i.NumberofUnites == 0)
                        { }
                        else
                        {
                            Medicines Medicine = db.Medicines.Where(m => m.ID == i.MedicineID).FirstOrDefault();
                            double CostPerUnits = Medicine.PricePerUnit * i.NumberofUnites;
                            i.Cost = CostPerUnits;
                           
                                theBigTotal += i.Cost;

                        }

                    }
                    if(theBigTotal<=currentBalance)
                    {
                        foreach (var i in ci)
                        {

                            if (i.PatientID == 0 || i.MedicineID == 0 || i.NumberofUnites == 0)
                            { }
                            else
                            {
                                Medicines Medicine = db.Medicines.Where(m => m.ID == i.MedicineID).FirstOrDefault();
                                double CostPerUnits = Medicine.PricePerUnit * i.NumberofUnites;
                                i.Cost = CostPerUnits;
                                
                                //if (i.Cost <= currentBalance)
                                //{

                                    double NewBalance = currentBalance - i.Cost;
                                    currentBalance = NewBalance;
                                    db.Balances.Where(b => b.ID == 3).FirstOrDefault().TotalValue = currentBalance;
                                    db.TemporaryCashings.Add(i);
                                    db.SaveChanges();

                            }

                        }
                        ViewBag.Correct = "تمت الإضافة بنجاح";
                    }
                    else
                    {
                        ViewBag.Cost = "التكلفة المطلوبة : " + theBigTotal + " ج";
                        ViewBag.Balance = "الرصيد الحالي غير كافئ";
                        ViewBag.BalanceID = db.Balances.Where(b => b.ID == 3).FirstOrDefault().TotalValue;
                        
                    }
                   ci = new List<TemporaryCashing> { new TemporaryCashing { PatientID = 0, MedicineID = 0, NumberofUnites = 0 } };
                }
                
            }
           
            ViewBag.MedicineID = new SelectList(db.Medicines, "ID", "Name");
            ViewBag.PatientID = new SelectList(db.Patients, "ID", "Name");
            ViewBag.BalanceID = db.Balances.Where(b => b.ID == 3).FirstOrDefault().TotalValue;
            
            return View(ci);
        }


       // [Authorize(Roles = "Administrator,المسئولون")]

        // GET: TemporaryCashings/Create
        //public ActionResult Create()
        //{

        //    ViewBag.MedicineID = new SelectList(db.Medicines, "ID", "Name");
        //    ViewBag.PatientID = new SelectList(db.Patients, "ID", "Name");
        //    ViewBag.BalanceID = db.Balances.Where(b => b.ID == 3).FirstOrDefault().TotalValue;
        //    return View();
        //}
        //// POST: TemporaryCashings/Create
        //// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        //// more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //[Authorize(Roles = "Administrator,المسئولون")]

        //public ActionResult Create([Bind(Include = "ID,NumberofUnites,DateCreated,Cost,MedicineID,PatientID")] TemporaryCashing temporaryCashing)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        Medicines Medicine = db.Medicines.Where(m => m.ID == temporaryCashing.MedicineID).FirstOrDefault();
        //         double CostPerUnits = Medicine.PricePerUnit * temporaryCashing.NumberofUnites;
        //        temporaryCashing.Cost = CostPerUnits;
        //        double currentBalance = db.Balances.Where(b => b.ID == 3).FirstOrDefault().TotalValue;
        //        if(temporaryCashing.Cost <= currentBalance)
        //        {
                    
        //            double NewBalance = currentBalance - temporaryCashing.Cost;
        //            currentBalance = NewBalance;
        //            db.Balances.Where(b => b.ID == 3).FirstOrDefault().TotalValue = currentBalance;
        //            db.TemporaryCashings.Add(temporaryCashing);
        //            db.SaveChanges();
                    
        //            return RedirectToAction("Index");
        //        }
        //       else
        //        {
        //            ViewBag.Cost ="التكلفة المطلوبة : " +temporaryCashing.Cost + " ج";
        //            ViewBag.Balance ="الرصيد الحالي غير كافئ";
        //            ViewBag.BalanceID= db.Balances.Where(b => b.ID == 3).FirstOrDefault().TotalValue;

        //        }

        //        // db.SaveChanges();
        //        //  CalcCostByUnitsNumber(temporaryCashing.PatientID, temporaryCashing.MedicineID, temporaryCashing.NumberofUnites);

        //    }
        //    ViewBag.MedicineID = new SelectList(db.Medicines, "ID", "Name", temporaryCashing.MedicineID);
        //    ViewBag.PatientID = new SelectList(db.Patients, "ID", "Name", temporaryCashing.PatientID);
        //    return View(temporaryCashing);
        //}

        // GET: TemporaryCashings/Edit/5
        [Authorize(Roles = "Administrator,المسئولون")]

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TemporaryCashing temporaryCashing = db.TemporaryCashings.Find(id);
            if (temporaryCashing == null)
            {
                return HttpNotFound();
            }
            ViewBag.BalanceID = db.Balances.Where(b => b.ID == 3).FirstOrDefault().TotalValue;
            ViewBag.MedicineID = new SelectList(db.Medicines, "ID", "Name", temporaryCashing.MedicineID);
            ViewBag.PatientID = new SelectList(db.Patients, "ID", "Name", temporaryCashing.PatientID);
            return View(temporaryCashing);
        }

        // POST: TemporaryCashings/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize(Roles = "Administrator,المسئولون")]

        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,NumberofUnites,DateCreated,Cost,MedicineID,PatientID")] TemporaryCashing temporaryCashing)
        {
            if (ModelState.IsValid)
            {
                TemporaryCashing tc = db.TemporaryCashings.Where(t => t.ID == temporaryCashing.ID).FirstOrDefault();
                double oldcost = tc.Cost;
                db.TemporaryCashings.Remove(tc);
                //add the old cost to balance
                double currentBalance = db.Balances.Where(b => b.ID == 3).FirstOrDefault().TotalValue;
                double NewBalance = currentBalance + oldcost;
                currentBalance = NewBalance;
                db.Balances.Where(b => b.ID == 3).FirstOrDefault().TotalValue = currentBalance;
               // Calculate  new cost
                Medicines Medicine = db.Medicines.Where(m => m.ID == temporaryCashing.MedicineID).FirstOrDefault();
                double CostPerUnits = Medicine.PricePerUnit * temporaryCashing.NumberofUnites;
                temporaryCashing.Cost = CostPerUnits;
                double currentBalance2 = db.Balances.Where(b => b.ID == 3).FirstOrDefault().TotalValue;
                if (temporaryCashing.Cost <= currentBalance2)
                {
                    double NewBalance2 = currentBalance2 - temporaryCashing.Cost;
                    currentBalance2 = NewBalance2;
                    db.Balances.Where(b => b.ID == 3).FirstOrDefault().TotalValue = currentBalance2;
                    db.TemporaryCashings.Add(temporaryCashing);
                    db.SaveChanges();

                    return RedirectToAction("Index");
                }
                else
                {
                    ViewBag.Cost = "التكلفة المطلوبة : " + temporaryCashing.Cost + " ج";
                    ViewBag.Balance = "الرصيد الحالي غير كافئ";
                    ViewBag.BalanceID = db.Balances.Where(b => b.ID == 3).FirstOrDefault().TotalValue;

                }

                // db.SaveChanges();
                //  CalcCostByUnitsNumber(temporaryCashing.PatientID, temporaryCashing.MedicineID, temporaryCashing.NumberofUnites);


                //db.Entry(temporaryCashing).State = EntityState.Modified;
                //db.SaveChanges();

                /////
                //PatientMedicineTest tc = db.PatientMedicineTests.Where(t => t.ID == patientMedicineTest.ID).FirstOrDefault();
                //db.PatientMedicineTests.Remove(tc);
                //db.PatientMedicineTests.Add(patientMedicineTest);
                //db.SaveChanges();
                //CalcCostByUnitsNumber(patientMedicineTest.PatientID, patientMedicineTest.MedicineID, patientMedicineTest.NumberOfUnits);
                //return RedirectToAction("Index");
                //
            }
            ViewBag.MedicineID = new SelectList(db.Medicines, "ID", "Name", temporaryCashing.MedicineID);
            ViewBag.PatientID = new SelectList(db.Patients, "ID", "Name", temporaryCashing.PatientID);
            return View(temporaryCashing);
        }

        // GET: TemporaryCashings/Delete/5
        [Authorize(Roles = "Administrator,المسئولون")]

        public ActionResult Delete(int? id)
        {
            TemporaryCashing tc = db.TemporaryCashings.Where(t => t.ID == id).FirstOrDefault();
            ViewBag.MedicineID = db.Medicines.Where(m => m.ID == tc.MedicineID).FirstOrDefault();
            ViewBag.PatientID = db.Patients.Where(m => m.ID == tc.PatientID).FirstOrDefault();
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TemporaryCashing temporaryCashing = db.TemporaryCashings.Find(id);
            if (temporaryCashing == null)
            {
                return HttpNotFound();
            }
            return View(temporaryCashing);
        }

        // POST: TemporaryCashings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator,المسئولون")]

        public ActionResult DeleteConfirmed(int id)
        {
            TemporaryCashing temporaryCashing = db.TemporaryCashings.Find(id);
             double currentBalance = db.Balances.Where(b => b.ID == 3).FirstOrDefault().TotalValue;
            double NewBalance = currentBalance + temporaryCashing.Cost;
            currentBalance = NewBalance;
            db.Balances.Where(b => b.ID == 3).FirstOrDefault().TotalValue = currentBalance;
            db.TemporaryCashings.Remove(temporaryCashing);
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
