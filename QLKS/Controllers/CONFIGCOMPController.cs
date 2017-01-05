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
    public class CONFIGCOMPController : Controller
    {
        private Entities db = new Entities();

        //
        // GET: /CONFIGCOMP/

        public ActionResult Index()
        {
            return View(db.CONFIGCOMPs.ToList());
        }

        //
        // GET: /CONFIGCOMP/Details/5

        public ActionResult Details(string id = null)
        {
            CONFIGCOMP configcomp = db.CONFIGCOMPs.Find(id);
            if (configcomp == null)
            {
                return HttpNotFound();
            }
            return View(configcomp);
        }

        //
        // GET: /CONFIGCOMP/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /CONFIGCOMP/Create

        [HttpPost]
        public ActionResult Create(CONFIGCOMP configcomp)
        {
            if (ModelState.IsValid)
            {
                db.CONFIGCOMPs.Add(configcomp);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(configcomp);
        }

        //
        // GET: /CONFIGCOMP/Edit/5

        public ActionResult Edit(string id = null)
        {
            CONFIGCOMP configcomp = db.CONFIGCOMPs.Find(id);
            if (configcomp == null)
            {
                return HttpNotFound();
            }
            return View(configcomp);
        }

        //
        // POST: /CONFIGCOMP/Edit/5

        [HttpPost]
        public ActionResult Edit(CONFIGCOMP configcomp)
        {
            if (ModelState.IsValid)
            {
                db.Entry(configcomp).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(configcomp);
        }

        //
        // GET: /CONFIGCOMP/Delete/5

        public ActionResult Delete(string id = null)
        {
            CONFIGCOMP configcomp = db.CONFIGCOMPs.Find(id);
            if (configcomp == null)
            {
                return HttpNotFound();
            }
            return View(configcomp);
        }

        //
        // POST: /CONFIGCOMP/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(string id)
        {
            CONFIGCOMP configcomp = db.CONFIGCOMPs.Find(id);
            db.CONFIGCOMPs.Remove(configcomp);
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