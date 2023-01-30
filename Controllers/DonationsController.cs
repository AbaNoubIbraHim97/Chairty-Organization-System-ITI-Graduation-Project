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
    
    public class DonationsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        [Authorize(Roles = "Administrator,المسئولون,المشرفون")]

        // GET: Donations
        public ActionResult Index()
        {
            
            return View(db.Donations.ToList());
        }
        [Authorize(Roles = "Administrator,المسئولون,المشرفون")]
        // GET: Donations/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Donations donations = db.Donations.Find(id);
            if (donations == null)
            {
                return HttpNotFound();
            }
            return View(donations);
        }

        // GET: Donations/Create
        [Authorize(Roles = "Administrator,المسئولون")]

        public ActionResult Create()
        {
            
            return View();
        }
        

        public void IncreaseBalance(double DonationedValue)
        {
            //double CurrentBalance=db.Balances.Where(id=>id==1)
            Balance balance = db.Balances.Where(b => b.ID == 3).FirstOrDefault();
            double CurrentBalance = balance.TotalValue + DonationedValue;
            balance.TotalValue = CurrentBalance;
            db.SaveChanges();

        }
        // POST: Donations/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize(Roles = "Administrator,المسئولون")]

        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,DonarName,RecieverName,Value,Date")] Donations donations)
        {
            if (ModelState.IsValid)
            {
                
                db.Donations.Add(donations);
                db.SaveChanges();
                IncreaseBalance(donations.Value);
                return RedirectToAction("Index");
            }

            return View(donations);
        }
        [Authorize(Roles = "Administrator,المسئولون")]

        // GET: Donations/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Donations donations = db.Donations.Find(id);
            if (donations == null)
            {
                return HttpNotFound();
            }
            return View(donations);
        }

        // POST: Donations/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator,المسئولون")]

        public ActionResult Edit([Bind(Include = "ID,DonarName,RecieverName,Value,Date")] Donations donations)
        {
            if (ModelState.IsValid)
            {
                db.Entry(donations).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(donations);
        }
        [Authorize(Roles = "Administrator,المسئولون")]

        // GET: Donations/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Donations donations = db.Donations.Find(id);
            if (donations == null)
            {
                return HttpNotFound();
            }
            return View(donations);
        }
        [Authorize(Roles = "Administrator,المسئولون")]

        // POST: Donations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Donations donations = db.Donations.Find(id);
            db.Donations.Remove(donations);
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
