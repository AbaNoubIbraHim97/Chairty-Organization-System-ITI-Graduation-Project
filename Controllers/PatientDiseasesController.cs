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
    public class PatientDiseasesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        [Authorize(Roles = "Administrator,المسئولون")]

        public ActionResult BulkData()
        {
            ViewBag.DiseasesID = new SelectList(db.Diseases, "ID", "Name");
            ViewBag.PatientID = new SelectList(db.Patients, "ID", "Name");
            List<PatientDisease> pd = new List<PatientDisease> { new PatientDisease { PatientID = 0,  DiseasesID=0 } };
            return View(pd);
        }
        [Authorize(Roles = "Administrator,المسئولون")]

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult BulkData(List<PatientDisease> ci)
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
                        
                        if(i.PatientID==0 || i.DiseasesID==0)
                        {
                             Correctcounter = 0;
                             Wrongcounter = 0;
                            Counter = 0;
                        }
                        else
                        {
                            PatientDisease p = db.patientDiseases.Where(c => c.PatientID==i.PatientID && c.DiseasesID==i.DiseasesID).FirstOrDefault();
                            
                            if (p == null)
                            {
                                dc.patientDiseases.Add(i);
                                Correctcounter++;
                               // ViewBag.Message = "تمت إضافة العناصر الجديدة";
                            }
                            else
                            {
                                Wrongcounter++;
                             //   ViewBag.Repeat = "مضاف بالفعل !!";
                            }
                        }
                       
                    }

                    dc.SaveChanges();
                    if (Counter == 1)
                    {
                        if (Wrongcounter ==1)
                        {
                            ViewBag.Repeat = "مضاف بالفعل !!";
                        }
                       if(Correctcounter==1)
                        {
                            ViewBag.Repeat = "تمت الإضافة بنجاح !!";
                        }
                    }
                    if(Counter>=2)
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
                    
                    ModelState.Clear();
                    ci = new List<PatientDisease> { new PatientDisease { PatientID = 0, DiseasesID = 0 } };
                }
            }
            ViewBag.DiseasesID = new SelectList(db.Diseases, "ID", "Name");
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
            ViewBag.DiseasesID = new SelectList(db.Diseases, "ID", "Name");
            ViewBag.PatientID = new SelectList(db.Patients, "ID", "Name", patient.ID);
            List<PatientDisease> pd = new List<PatientDisease> { new PatientDisease { PatientID = 0, DiseasesID = 0 } };
            return View(pd);
        }
        [Authorize(Roles = "Administrator,المسئولون")]

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult BulkDataa(List<PatientDisease> ci)
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

                        if (i.PatientID == 0 || i.DiseasesID == 0)
                        {
                            Correctcounter = 0;
                            Wrongcounter = 0;
                            Counter = 0;
                        }
                        else
                        {
                            Wrongcounter++;
                            dc.patientDiseases.Add(i);
                            dc.SaveChanges();
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
                    //ViewBag.Message = "تمت الإضافة بنجاح";
                    ModelState.Clear();
                    ci = new List<PatientDisease> { new PatientDisease { PatientID = 0, DiseasesID = 0 } };
                    int max = db.Patients.OrderByDescending( p => p.ID).FirstOrDefault().ID;
                    //Patient p = db.Patients.Where(s => s.ID == i.PatientID).FirstOrDefault();
                    return RedirectToAction("BulkDataa", "PatientMedicineTests", new { @id = max });

                }
            }
            ViewBag.DiseasesID = new SelectList(db.Diseases, "ID", "Name");
            ViewBag.PatientID = new SelectList(db.Patients, "ID", "Name");
            return View(ci);
        }
        // GET: PatientDiseases

        [Authorize(Roles = "Administrator,المسئولون,المشرفون")]

        public ActionResult Index()
        {
            ViewBag.PatientID = new SelectList(db.Patients, "ID", "Name");
            ViewBag.DiseasesID = new SelectList(db.Diseases, "ID", "Name");
            var patientDiseases = db.patientDiseases.Include(p => p.Diseases).Include(p => p.Patient);
            return View(patientDiseases.ToList());
        }

        [Authorize(Roles = "Administrator,المسئولون,المشرفون")]

        public ActionResult IndexMain()
        {
            ViewBag.PatientID = new SelectList(db.Patients, "ID", "Name");
            ViewBag.DiseasesID = new SelectList(db.Diseases, "ID", "Name");
            var patientDiseases = db.patientDiseases.Include(p => p.Diseases).Include(p => p.Patient);
            return View(patientDiseases.ToList());
        }
        [Authorize(Roles = "Administrator,المسئولون,المشرفون")]

        public ActionResult Search(int PatientID)
        {
            ViewBag.PatientID = new SelectList(db.Patients, "ID", "Name");
            ViewBag.DiseasesID = new SelectList(db.Diseases, "ID", "Name");
            List<PatientDisease> Patients = db.patientDiseases.Where(c => c.PatientID.Equals(PatientID)).ToList();
           if(Patients == null)
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

        // GET: PatientDiseases/Details/5
        public ActionResult Details(int? id)
        {
            PatientDisease tc = db.patientDiseases.Where(t => t.ID == id).FirstOrDefault();
            ViewBag.DiseasesID = db.Diseases.Where(m => m.ID == tc.DiseasesID).FirstOrDefault();
            ViewBag.PatientID = db.Patients.Where(m => m.ID == tc.PatientID).FirstOrDefault();

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PatientDisease patientDisease = db.patientDiseases.SingleOrDefault(m => m.ID == id);
            if (patientDisease == null)
            {
                return HttpNotFound();
            }
            return View(patientDisease);
        }
        //public ActionResult Createe()
        //{
        //  // ViewData["Submarkets"] = new SelectList(db.Patients.AllOrdered(), "ID", "Name");
        //    ViewBag.DiseasesID = new SelectList(db.Diseases, "ID", "Name");
        //    ViewBag.PatientID = new SelectList(db.Patients, "ID", "Name");
        //    return View();
        //}
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Createe([Bind(Include = "PatientID,DiseasesID")] PatientDisease patientDisease)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        ViewBag.DiseasesID = new SelectList(db.Diseases, "ID", "Name", patientDisease.DiseasesID);
        //        ViewBag.PatientID = new SelectList(db.Patients, "ID", "Name", patientDisease.PatientID);

        //        db.patientDiseases.Add(patientDisease);
        //        db.SaveChanges();
        //        ViewBag.PD1 = db.patientDiseases.Where(s => s.PatientID== patientDisease.PatientID).FirstOrDefault().PatientID;
        //        ViewBag.PD2 = db.patientDiseases.Where(s => s.DiseasesID== patientDisease.DiseasesID).FirstOrDefault().DiseasesID;
        //        Patient p = db.Patients.Where(s => s.ID == patientDisease.PatientID).FirstOrDefault();
        //        return View();
        //    }
        //    return View();
        //}
        // GET: PatientDiseases/Create
        //public ActionResult Create()
        //{
        //    ViewBag.DiseasesID = new SelectList(db.Diseases, "ID", "Name");
        //    ViewBag.PatientID = new SelectList(db.Patients, "ID", "Name");
        //    return View();
        //}

        //POST: PatientDiseases/Create
        //To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Create([Bind(Include = "PatientID,DiseasesID")] PatientDisease patientDisease)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.patientDiseases.Add(patientDisease);
        //        db.SaveChanges();
        //        Patient p = db.Patients.Where(s => s.ID == patientDisease.PatientID).FirstOrDefault();
        //        return RedirectToAction("Create", "PatientMedicineTests", new { @id = p.ID });

        //    }

        //    ViewBag.DiseasesID = new SelectList(db.Diseases, "ID", "Name", patientDisease.DiseasesID);
        //    ViewBag.PatientID = new SelectList(db.Patients, "ID", "Name", patientDisease.PatientID);
        //    return View(patientDisease);
        //}
        [Authorize(Roles = "Administrator,المسئولون")]

        //// GET: PatientDiseases/Edit/5
        public ActionResult Edit(int? id)
        {
            PatientDisease tc = db.patientDiseases.Where(t => t.ID == id).FirstOrDefault();
            ViewBag.DiseasesID = db.Diseases.Where(m => m.ID == tc.DiseasesID).FirstOrDefault();
            ViewBag.PatientID = db.Patients.Where(m => m.ID == tc.PatientID).FirstOrDefault();
            if (id == null )
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PatientDisease patientDisease = db.patientDiseases.SingleOrDefault(m=>m.ID==id);
            if (patientDisease == null)
            {
                return HttpNotFound();
            }
            ViewBag.DiseasesID = new SelectList(db.Diseases, "ID", "Name", patientDisease.DiseasesID);
            ViewBag.PatientID = new SelectList(db.Patients, "ID", "Name", patientDisease.PatientID);
            return View(patientDisease);
        }
        // POST: PatientDiseases/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator,المسئولون")]

        public ActionResult Edit([Bind(Include = "ID,PatientID,DiseasesID")] PatientDisease patientDisease)
        {
            if (ModelState.IsValid)
            {
                //db.Entry(patientDisease).State = EntityState.Modified;
                PatientDisease tc = db.patientDiseases.Where(t => t.ID == patientDisease.ID).FirstOrDefault();
                db.patientDiseases.Remove(tc);
                db.patientDiseases.Add(patientDisease);
                db.SaveChanges();
                PatientDisease patientDisease2 = patientDisease;
                return RedirectToAction("Index");
            }
            ViewBag.DiseasesID = new SelectList(db.Diseases, "ID", "Name", patientDisease.DiseasesID);
            ViewBag.PatientID = new SelectList(db.Patients, "ID", "Name", patientDisease.PatientID);
            return View(patientDisease);
        }
        [Authorize(Roles = "Administrator,المسئولون")]

        // GET: PatientDiseases/Delete/5
        public ActionResult Delete(int? id)
        {
            PatientDisease tc = db.patientDiseases.Where(t => t.ID == id).FirstOrDefault();
            ViewBag.DiseasesID = db.Diseases.Where(m => m.ID == tc.DiseasesID).FirstOrDefault();
            ViewBag.PatientID = db.Patients.Where(m => m.ID == tc.PatientID).FirstOrDefault();

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PatientDisease patientDisease = db.patientDiseases.SingleOrDefault(m => m.ID == id);

            if (patientDisease == null)
            {
                return HttpNotFound();
            }
            return View(patientDisease);
        }

        // POST: PatientDiseases/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator,المسئولون")]

        public ActionResult DeleteConfirmed(int id)
        {
            PatientDisease patientDisease = db.patientDiseases.SingleOrDefault(m => m.ID == id);
            db.patientDiseases.Remove(patientDisease);
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
