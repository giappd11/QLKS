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
    public class AmenityController : Controller
    {
        private Entities db = new Entities();

        public string AddAmenity(string ids)
        {
            string result = "";
            if (ids != "")
            {
                string[] listid = ids.Split(',');
                if (listid.Length > 1)
                {
                    for (int i = 0; i < listid.Length - 1; i++)
                    {
                        var amenity = db.Amenities.Find(listid[i]);
                        result += "<tr>"
                           + "<td>"+amenity.AmenityName+"</td>"
                           + "<td>"+amenity.AmenityUnit+"</td>"
                           + "<td><input type='number' class = 'form-control form-control-inline input-medium' name ='number' value='1' /></td>"
                           + "<td>"+ amenity.Price
                           + "<input type = 'hidden' name = 'price' value ='" + amenity.Price + "'/>"
                           +"</td>"
                           + "<td><span class ='total_price'>" + amenity.Price + "</span></td>"
                           + "<td><span class = 'remove' data = "+ amenity.AmenityID +">remove</span></td>"
                           + "</tr>";
                    }
                }
            }
            return result;

        }



        //
        // GET: /Amenity/

        public ActionResult Index()
        {
            return View(db.Amenities.ToList());
        }

        //
        // GET: /Amenity/Details/5

        public ActionResult Details(string id = null)
        {
            Amenity amenity = db.Amenities.Find(id);
            if (amenity == null)
            {
                return HttpNotFound();
            }
            return View(amenity);
        }

        //
        // GET: /Amenity/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Amenity/Create

        [HttpPost]
        public ActionResult Create(Amenity amenity)
        {
            if (ModelState.IsValid)
            {
                var data = db.Database.SqlQuery<System.Int64>("exec GetNextAmenitySeq");
                amenity.AmenityID = GlobalFunction.addDateToID(data.FirstOrDefault().ToString());
                db.Amenities.Add(amenity);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(amenity);
        }

        //
        // GET: /Amenity/Edit/5

        public ActionResult Edit(string id = null)
        {
            Amenity amenity = db.Amenities.Find(id);
            if (amenity == null)
            {
                return HttpNotFound();
            }
            return View(amenity);
        }

        //
        // POST: /Amenity/Edit/5

        [HttpPost]
        public ActionResult Edit(Amenity amenity)
        {
            if (ModelState.IsValid)
            {
                db.Entry(amenity).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(amenity);
        }

        //
        // GET: /Amenity/Delete/5

        public ActionResult Delete(string id = null)
        {
            Amenity amenity = db.Amenities.Find(id);
            if (amenity == null)
            {
                return HttpNotFound();
            }
            return View(amenity);
        }

        //
        // POST: /Amenity/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(string id)
        {
            Amenity amenity = db.Amenities.Find(id);
            db.Amenities.Remove(amenity);
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