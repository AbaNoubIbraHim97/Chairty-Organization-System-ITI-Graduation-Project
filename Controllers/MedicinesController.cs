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
    
    public class MedicinesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        [Authorize(Roles = "Administrator,المسئولون,المشرفون")]

        // GET: Medicines
        public ActionResult Index()
        {
            var medicines = db.Medicines.Include(m => m.MedicinesCategory);
            return View(medicines.ToList());
        }
        // GET: Medicines/Details/5
        [Authorize(Roles = "Administrator,المسئولون,المشرفون")]

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Medicines medicines = db.Medicines.Find(id);
            if (medicines == null)
            {
                return HttpNotFound();
            }
            return View(medicines);
        }
        [Authorize(Roles = "Administrator,المسئولون")]

        // GET: Medicines/Create
        public ActionResult Create()
        {
            ViewBag.MedicineCategoryID = new SelectList(db.MedicinesCategories, "ID", "Name");
            return View();
        }

        // POST: Medicines/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Administrator,المسئولون")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Name,PricePerUnit,MedicineCategoryID")] Medicines medicines)
        {
            if (ModelState.IsValid)
            {
                Medicines name2 = db.Medicines.Where(c => c.Name.Equals(medicines.Name)).FirstOrDefault();
                if (name2 == null)
                {
                db.Medicines.Add(medicines);
                db.SaveChanges();
                return RedirectToAction("Index");
                }
                else
                {
                    ViewBag.Repeat = "هذا الاسم موجود بالفعل !!";
                }
            }

            ViewBag.MedicineCategoryID = new SelectList(db.MedicinesCategories, "ID", "Name", medicines.MedicineCategoryID);
            return View(medicines);
        }
        [Authorize(Roles = "Administrator,المسئولون")]

        // GET: Medicines/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Medicines medicines = db.Medicines.Find(id);
            if (medicines == null)
            {
                return HttpNotFound();
            }
            ViewBag.MedicineCategoryID = new SelectList(db.MedicinesCategories, "ID", "Name", medicines.MedicineCategoryID);
            return View(medicines);
        }
        [Authorize(Roles = "Administrator,المسئولون")]

        // POST: Medicines/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Name,PricePerUnit,MedicineCategoryID")] Medicines medicines)
        {
            if (ModelState.IsValid)
            {
                db.Entry(medicines).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.MedicineCategoryID = new SelectList(db.MedicinesCategories, "ID", "Name", medicines.MedicineCategoryID);
            return View(medicines);
        }
        [Authorize(Roles = "Administrator,المسئولون")]

        // GET: Medicines/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Medicines medicines = db.Medicines.Find(id);
            if (medicines == null)
            {
                return HttpNotFound();
            }
            return View(medicines);
        }
        [Authorize(Roles = "Administrator,المسئولون")]
        // POST: Medicines/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Medicines medicines = db.Medicines.Find(id);
            db.Medicines.Remove(medicines);
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
