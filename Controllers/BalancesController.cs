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
   
    public class BalancesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

       [Authorize(Roles = "Administrator,المسئولون,المشرفون")]

        // GET: Balances
        public ActionResult Index()
        {
            return View(db.Balances.ToList());
        }
        // GET: Balances/Details/5
        //public ActionResult Details(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Balance balance = db.Balances.Find(id);
        //    if (balance == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(balance);
        //}
        [Authorize(Roles = "Administrator")]

        // GET: Balances/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Balances/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Administrator")]

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,TotalValue")] Balance balance)
        {
            if (ModelState.IsValid)
            {
                db.Balances.Add(balance);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(balance);
        }

        //// GET: Balances/Edit/5
        //public ActionResult Edit(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Balance balance = db.Balances.Find(id);
        //    if (balance == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(balance);
        //}

        //// POST: Balances/Edit/5
        //// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        //// more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Edit([Bind(Include = "ID,TotalValue")] Balance balance)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.Entry(balance).State = EntityState.Modified;
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }
        //    return View(balance);
        //}

        //// GET: Balances/Delete/5
        //public ActionResult Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Balance balance = db.Balances.Find(id);
        //    if (balance == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(balance);
        //}

        // POST: Balances/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public ActionResult DeleteConfirmed(int id)
        //{
        //    Balance balance = db.Balances.Find(id);
        //    db.Balances.Remove(balance);
        //    db.SaveChanges();
        //    return RedirectToAction("Index");
        //}

        //protected override void Dispose(bool disposing)
        //{
        //    if (disposing)
        //    {
        //        db.Dispose();
        //    }
        //    base.Dispose(disposing);
        //}
    }
}
