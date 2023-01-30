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
    
    public class MedicinesCategoriesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        [Authorize(Roles = "Administrator,المسئولون,المشرفون")]

        // GET: MedicinesCategories
        public ActionResult Index()
        {
            return View(db.MedicinesCategories.ToList());
        }
        [Authorize(Roles = "Administrator,المسئولون,المشرفون")]
        // GET: MedicinesCategories/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MedicinesCategory medicinesCategory = db.MedicinesCategories.Find(id);
            if (medicinesCategory == null)
            {
                return HttpNotFound();
            }
            return View(medicinesCategory);
        }
        [Authorize(Roles = "Administrator,المسئولون")]

        // GET: MedicinesCategories/Create
        public ActionResult Create()
        {
            return View();
        }
        [Authorize(Roles = "Administrator,المسئولون")]

        // POST: MedicinesCategories/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Name")] MedicinesCategory medicinesCategory)
        {
            if (ModelState.IsValid)
            {

                MedicinesCategory name2 = db.MedicinesCategories.Where(c => c.Name.Equals(medicinesCategory.Name)).FirstOrDefault();
                if (name2 == null)
                {
                    db.MedicinesCategories.Add(medicinesCategory);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                else
                {
                    ViewBag.Repeat = "هذا الاسم موجود بالفعل !!";
                }
            }

            return View(medicinesCategory);
        }
        [Authorize(Roles = "Administrator,المسئولون")]

        // GET: MedicinesCategories/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MedicinesCategory medicinesCategory = db.MedicinesCategories.Find(id);
            if (medicinesCategory == null)
            {
                return HttpNotFound();
            }
            return View(medicinesCategory);
        }
        [Authorize(Roles = "Administrator,المسئولون")]

        // POST: MedicinesCategories/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Name")] MedicinesCategory medicinesCategory)
        {
            if (ModelState.IsValid)
            {
                db.Entry(medicinesCategory).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(medicinesCategory);
        }
        [Authorize(Roles = "Administrator,المسئولون")]

        // GET: MedicinesCategories/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MedicinesCategory medicinesCategory = db.MedicinesCategories.Find(id);
            if (medicinesCategory == null)
            {
                return HttpNotFound();
            }
            return View(medicinesCategory);
        }
        [Authorize(Roles = "Administrator,المسئولون")]

        // POST: MedicinesCategories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            MedicinesCategory medicinesCategory = db.MedicinesCategories.Find(id);
            db.MedicinesCategories.Remove(medicinesCategory);
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
