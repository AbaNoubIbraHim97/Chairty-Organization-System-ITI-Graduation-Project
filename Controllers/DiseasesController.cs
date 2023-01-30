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
   // [Authorize(Roles = "Administrator,المسئولون,المشرفون")]

    
    public class DiseasesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
       [Authorize(Roles = "Administrator,المسئولون,المشرفون")]
        // GET: Diseases
        public ActionResult Index()
        {
            return View(db.Diseases.ToList());
        }
        [Authorize(Roles = "Administrator,المسئولون,المشرفون")]
        // GET: Diseases/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Diseases diseases = db.Diseases.Find(id);
            if (diseases == null)
            {
                return HttpNotFound();
            }
            return View(diseases);
        }
        [Authorize(Roles = "Administrator,المسئولون")]
        // GET: Diseases/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Diseases/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Administrator,المسئولون")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Name")] Diseases diseases)
        {
            if (ModelState.IsValid)
            {
               
                Diseases name2 = db.Diseases.Where(c => c.Name.Equals(diseases.Name)).FirstOrDefault();
                if (name2 == null)
                {
                    db.Diseases.Add(diseases);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                else
                {
                    ViewBag.Repeat = "هذا الاسم موجود بالفعل !!";
                }
            }

            return View(diseases);
        }

        [Authorize(Roles = "Administrator,المسئولون")]

        // GET: Diseases/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Diseases diseases = db.Diseases.Find(id);
            if (diseases == null)
            {
                return HttpNotFound();
            }
            return View(diseases);
        }

        // POST: Diseases/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Administrator,المسئولون")]

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Name")] Diseases diseases)
        {
            if (ModelState.IsValid)
            {
                db.Entry(diseases).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(diseases);
        }
        [Authorize(Roles = "Administrator,المسئولون")]

        // GET: Diseases/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Diseases diseases = db.Diseases.Find(id);
            if (diseases == null)
            {
                return HttpNotFound();
            }
            return View(diseases);
        }
        [Authorize(Roles = "Administrator,المسئولون")]

        // POST: Diseases/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Diseases diseases = db.Diseases.Find(id);
            db.Diseases.Remove(diseases);
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
