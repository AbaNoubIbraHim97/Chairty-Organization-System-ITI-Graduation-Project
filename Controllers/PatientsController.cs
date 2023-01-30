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
   
    public class PatientsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        [Authorize(Roles = "Administrator,المسئولون,المشرفون")]


        // GET: Patients
        public ActionResult Index()
        {
            return View(db.Patients.ToList());
        }
        [Authorize(Roles = "Administrator,المسئولون,المشرفون")]

        // GET: Patients/Details/5
        public ActionResult Details(int? id)
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
            return View(patient);
        }
        [Authorize(Roles = "Administrator,المسئولون,المشرفون")]
        public ActionResult Search(string PatientName,string Shepherd, bool isalife)
        {
            ViewBag.PatientName = PatientName;
            ViewBag.Shepherd = Shepherd;
            List<Patient> Patients = db.Patients.Where(c => c.Name.Contains(PatientName) && c.Shepherd.Contains(Shepherd) && c.isalife== isalife).ToList();
            
            if(Patients.Count>0)
            {
                return View("Index", Patients);
            }
            else
            {
                ViewBag.NotFounf = "لا توجد بيانات مطابقة للبحث";
                return View("Index", Patients);
            }
        }
        [Authorize(Roles = "Administrator,المسئولون")]

        public ActionResult BulkData()
        {
           // List<Patient> pd = new List<Patient> { new Patient { ID = 0, PMTotalcost = 0 } };
            return View(db.Patients.Where(p=>p.isalife==true).ToList());
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator,المسئولون")]

        public ActionResult BulkData(List<Patient> MC)
        {

            if (ModelState.IsValid)
            {
                foreach (var i in MC)
                {
                    db.Patients.Add(i);
                    db.SaveChanges();
                }
            }
           return RedirectToAction("Index");
        }
        [Authorize(Roles = "Administrator,المسئولون")]
        // GET: Patients/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Patients/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Administrator,المسئولون")]

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Name,SSNNew,BirthDate,Shepherd,isalife,DateCreated")] Patient patient)
        {
            if (ModelState.IsValid)
            {
         
                Patient p = db.Patients.Where(c => c.Name.Equals(patient.Name)).FirstOrDefault();
                if (p == null)
                {
                    db.Patients.Add(patient);
                    db.SaveChanges();
                    return RedirectToAction("BulkDataa", "PatientDiseases", new { @id = patient.ID });
                }
                else
                {
                    ViewBag.Repeat = "هذا الاسم موجود بالفعل !!";
                }
            }  
            return View(patient); 
        }
        [Authorize(Roles = "Administrator,المسئولون")]

        // GET: Patients/Edit/5
        public ActionResult Edit(int? id)
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
            return View(patient);
        }

        // POST: Patients/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Administrator,المسئولون")]

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Name,SSNNew,BirthDate,Shepherd,isalife,DateCreated")] Patient patient)
        {
            if (ModelState.IsValid)
            {
                db.Entry(patient).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(patient);
        }
        [Authorize(Roles = "Administrator,المسئولون")]

        // GET: Patients/Delete/5
        public ActionResult Delete(int? id)
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
            return View(patient);
        }
        [Authorize(Roles = "Administrator,المسئولون")]

        // POST: Patients/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Patient patient = db.Patients.Find(id);
            db.Patients.Remove(patient);
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
