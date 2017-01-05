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
    public class RoomTypeController : Controller
    {
        private Entities db = new Entities();

        
        public ActionResult Search(string id)
        {
            var rooms = db.Rooms.Where(a => a.RoomStatus == 1).Include(c => c.RoomType).OrderBy(a => a.Position) ;
            if (id != "")
            {
                 rooms = db.Rooms.Where(u => u.RoomTypeID == id && u.RoomStatus == 1).OrderBy( u => u.Position);
            }
            return View(rooms.ToList());

        }


        public ActionResult Index()
        {
            ViewRoomType view = new ViewRoomType()
            {
                List = db.RoomTypes.ToList(),
                Create = new QLKS.Models.RoomType()
            };


            return View(view);
        }


        [HttpPost]
        public ActionResult Index(RoomType roomtype)
        {
            if (roomtype.RoomTypeID == null)
            {
                var data = db.Database.SqlQuery<System.Int64>("exec GetNextRoomTypeSeq");
                roomtype.RoomTypeID = GlobalFunction.addDateToID(data.FirstOrDefault().ToString());
            } 
            if (ModelState.IsValid)
            {
                db.RoomTypes.Add(roomtype);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewRoomType view = new ViewRoomType()
            {
                List = db.RoomTypes.ToList(),
                Create = roomtype
            };
            return View(view); 
        }
        //
        // GET: /RoomType/Details/5

        public ActionResult Details(string id = null)
        {
            RoomType roomtype = db.RoomTypes.Find(id);
            if (roomtype == null)
            {
                return HttpNotFound();
            }
            return View(roomtype);
        }

        //
        // GET: /RoomType/Create
        [HttpGet]
        public ActionResult Create()
        { 
            return PartialView("Create", new QLKS.Models.RoomType());
        }

        //
        // POST: /RoomType/Create

        [HttpPost]
        public ActionResult Create(RoomType roomtype)
        {
            var data = db.Database.SqlQuery<System.Int64>("exec GetNextRoomTypeSeq");
            roomtype.RoomTypeID = GlobalFunction.addDateToID(data.FirstOrDefault().ToString()); 
            if (ModelState.IsValid)
            {
                db.RoomTypes.Add(roomtype);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(roomtype);
        }

        //
        // GET: /RoomType/Edit/5

        public ActionResult Edit(string id = null)
        {
            RoomType roomtype = db.RoomTypes.Find(id);
            if (roomtype == null)
            {
                return HttpNotFound();
            }
            return View(roomtype);
        }

        //
        // POST: /RoomType/Edit/5

        [HttpPost]
        public ActionResult Edit(RoomType roomtype)
        {
            if (ModelState.IsValid)
            {
                db.Entry(roomtype).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(roomtype);
        }

        //
        // GET: /RoomType/Delete/5

        public ActionResult Delete(string id = null)
        {
            RoomType roomtype = db.RoomTypes.Find(id);
            if (roomtype == null)
            {
                return HttpNotFound();
            }
            return View(roomtype);
        }

        //
        // POST: /RoomType/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(string id)
        {
            RoomType roomtype = db.RoomTypes.Find(id);
            db.RoomTypes.Remove(roomtype);
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