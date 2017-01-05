using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using QLKS.Models;

namespace QLKS.Controllers
{
    public class NationalityController : Controller
    {
        private Entities db = new Entities();

        //
        // GET: /Nationality/

        public ActionResult Index()
        {
            return View(db.Nationalities.ToList());
        }

        //
        // GET: /Nationality/Details/5

        public ActionResult Details(int id = 0)
        {
            Nationality nationality = db.Nationalities.Find(id);
            if (nationality == null)
            {
                return HttpNotFound();
            }
            return View(nationality);
        }

        //
        // GET: /Nationality/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Nationality/Create

        [HttpPost]
        public ActionResult Create(Nationality nationality)
        {
            if (ModelState.IsValid)
            {
                db.Nationalities.Add(nationality);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(nationality);
        }

        //
        // GET: /Nationality/Edit/5

        public ActionResult Edit(int id = 0)
        {
            Nationality nationality = db.Nationalities.Find(id);
            if (nationality == null)
            {
                return HttpNotFound();
            }
            return View(nationality);
        }

        //
        // POST: /Nationality/Edit/5

        [HttpPost]
        public ActionResult Edit(Nationality nationality)
        {
            if (ModelState.IsValid)
            {
                db.Entry(nationality).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(nationality);
        }

        //
        // GET: /Nationality/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Nationality nationality = db.Nationalities.Find(id);
            if (nationality == null)
            {
                return HttpNotFound();
            }
            return View(nationality);
        }

        //
        // POST: /Nationality/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            Nationality nationality = db.Nationalities.Find(id);
            db.Nationalities.Remove(nationality);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}