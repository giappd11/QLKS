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
    public class RoomController : Controller
    {
        private Entities db = new Entities();

        //
        // GET: /Room/

        public ActionResult Index()
        {
            ViewRoom view = new ViewRoom()
            {
                ListRoomType = db.RoomTypes.ToList(),
                List = db.Rooms.Include(r => r.RoomType).ToList(),
                Create = new Room()
            };
            ViewBag.RoomTypeID = new SelectList(db.RoomTypes, "RoomTypeID", "RoomTypeName");
            var floor = db.CONFIGCOMPs;
            int floorNo = 10;
            if (floor.FirstOrDefault() != null)
            {
                floorNo = Convert.ToInt32(floor.FirstOrDefault().FloorNo);
            }
            if (floorNo != 0)
            {

                IList<SelectListItem> listValue = new List<SelectListItem>();
                for (int i = 1; i < floorNo; i++)
                {
                    SelectListItem select = new SelectListItem()
                    {
                        Value = Convert.ToString(i),
                        Text = "Tầng " + Convert.ToString(i)
                    };
                    listValue.Add(select);
                }
                ViewBag.Floor = listValue.ToList();
            }
            return View(view); 
        }
        [HttpPost]
        public ActionResult Index(Room room)
        {
            room.RoomStatus = 1;
            if (room.RoomNoID == null)
            {
                var data = db.Database.SqlQuery<System.Int64>("exec GetNextRoomSeq");
                room.RoomNoID = GlobalFunction.addDateToID(data.FirstOrDefault().ToString());
            }
            if (ModelState.IsValid)
            {
                db.Rooms.Add(room);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            else
            {
                ViewRoom view = new ViewRoom()
                {
                    ListRoomType = db.RoomTypes.ToList(),
                    List = db.Rooms.Include(r => r.RoomType).ToList(),
                    Create = room
                };
                ViewBag.RoomTypeID = new SelectList(db.RoomTypes, "RoomTypeID", "RoomTypeName");
                var floor = db.CONFIGCOMPs;
                int floorNo = 10;
                if (floor.FirstOrDefault() != null)
                {
                    floorNo = Convert.ToInt32(floor.FirstOrDefault().FloorNo);
                }
                if (floorNo != 0)
                {

                    IList<SelectListItem> listValue = new List<SelectListItem>();
                    for (int i = 1; i < floorNo; i++)
                    {
                        SelectListItem select = new SelectListItem()
                        {
                            Value = Convert.ToString(i),
                            Text = "Tầng " + Convert.ToString(i)
                        };
                        listValue.Add(select);
                    }
                    ViewBag.Floor = listValue.ToList();
                }
                return View(view); 
            }
        }

        //
        // GET: /Room/Details/5

        public ActionResult Details(string id = null)
        {
            Room room = db.Rooms.Find(id);
            if (room == null)
            {
                return HttpNotFound();
            }
            return View(room);
        }

        //
        // GET: /Room/Create

        public ActionResult Create()
        { 
            return PartialView("Create", new QLKS.Models.Room());
           // return View();
        }

        //
        // POST: /Room/Create

        [HttpPost]
        public ActionResult Create(Room room)
        {
            room.RoomStatus = 1;
            if (room.RoomNoID == null)
            {
                var data = db.Database.SqlQuery<System.Int64>("exec GetNextRoomSeq");
                room.RoomNoID = GlobalFunction.addDateToID(data.FirstOrDefault().ToString());  
            }
            if (ModelState.IsValid)
            {
                db.Rooms.Add(room);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index",room);
            //return View("Index", room);
             
        }

        //
        // GET: /Room/Edit/5

        public ActionResult Edit(string id = null)
        {
            Room room = db.Rooms.Find(id);
            if (room == null)
            {
                return HttpNotFound();
            }
            ViewBag.RoomTypeID = new SelectList(db.RoomTypes, "RoomTypeID", "RoomTypeName", room.RoomTypeID);
            return View(room);
        }

        //
        // POST: /Room/Edit/5

        [HttpPost]
        public ActionResult Edit(Room room)
        {
            if (ModelState.IsValid)
            {
                db.Entry(room).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.RoomTypeID = new SelectList(db.RoomTypes, "RoomTypeID", "RoomTypeName", room.RoomTypeID);
            return View("Index",room);
        }

        //
        // GET: /Room/Delete/5

        public ActionResult Delete(string id = null)
        {
            Room room = db.Rooms.Find(id);
            if (room == null)
            {
                return HttpNotFound();
            }
            return View(room);
        }

        //
        // POST: /Room/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(string id)
        {
            Room room = db.Rooms.Find(id);
            room.RoomStatus = 0;
            //db.Rooms.Remove(room);
            db.Entry(room).State = EntityState.Modified;
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