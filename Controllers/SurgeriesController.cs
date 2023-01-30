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
    public class SurgeriesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        [Authorize(Roles = "Administrator,المسئولون,المشرفون")]

        // GET: Surgeries
        public ActionResult Index()
        {
            return View(db.Surgeries.ToList());
        }
        [Authorize(Roles = "Administrator,المسئولون,المشرفون")]

        // GET: Surgeries/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Surgery surgery = db.Surgeries.Find(id);
            if (surgery == null)
            {
                return HttpNotFound();
            }
            return View(surgery);
        }

        // GET: Surgeries/Create
        [Authorize(Roles = "Administrator,المسئولون")]

        public ActionResult Create()
        {
            return View();
        }

        // POST: Surgeries/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator,المسئولون")]

        public ActionResult Create([Bind(Include = "ID,Name")] Surgery surgery)
        {
            if (ModelState.IsValid)
            {
                Surgery name2 = db.Surgeries.Where(c => c.Name.Equals(surgery.Name)).FirstOrDefault();
                if (name2 == null)
                {
                    db.Surgeries.Add(surgery);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                else
                {
                    ViewBag.Repeat = "هذا الاسم موجود بالفعل !!";
                }
            }

            return View(surgery);
        }

        // GET: Surgeries/Edit/5
        [Authorize(Roles = "Administrator,المسئولون")]

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Surgery surgery = db.Surgeries.Find(id);
            if (surgery == null)
            {
                return HttpNotFound();
            }
            return View(surgery);
        }

        // POST: Surgeries/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator,المسئولون")]

        public ActionResult Edit([Bind(Include = "ID,Name")] Surgery surgery)
        {
            if (ModelState.IsValid)
            {
                db.Entry(surgery).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(surgery);
        }

        // GET: Surgeries/Delete/5
        [Authorize(Roles = "Administrator,المسئولون")]

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Surgery surgery = db.Surgeries.Find(id);
            if (surgery == null)
            {
                return HttpNotFound();
            }
            return View(surgery);
        }

        // POST: Surgeries/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator,المسئولون")]

        public ActionResult DeleteConfirmed(int id)
        {
            Surgery surgery = db.Surgeries.Find(id);
            db.Surgeries.Remove(surgery);
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
