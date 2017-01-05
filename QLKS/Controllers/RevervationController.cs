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
    public class RevervationController : Controller
    {
        private Entities db = new Entities();

        //
        // GET: /Revervation/

        public ActionResult Index()
        {
            var revervations = db.Revervations.Where(u => u.Status != 2).Include(r => r.Customer).Include(r => r.RevervationRoomType);
            return View(revervations.ToList());
        }

        //
        // GET: /Revervation/Details/5

        public ActionResult Details(int id = 0)
        {
            Revervation revervation = db.Revervations.Find(id);
            if (revervation == null)
            {
                return HttpNotFound();
            }
            return View(revervation);
        }

        //
        // GET: /Revervation/Create

        public ActionResult Create(ChooseDate datechoose)
        { 
            List<string> result = (from r in db.Rooms
                                join resverRoom in db.ReservationsRooms
                                on r.RoomNoID equals resverRoom.RoomID
                                join resver in db.Revervations
                                on resverRoom.ReservercationID equals resver.ReservationID
                                   where resver.Status != 2 && ((resver.DateOut >= datechoose.DateIn) && (resver.DateOut <= datechoose.DateOut) || (resver.DateIn <= datechoose.DateOut && resver.DateIn >= datechoose.DateIn ))
                                select r.RoomNoID).ToList<string>();
                             ;
            var result1 = (from r in db.Rooms where !result.Contains( r.RoomNoID) && r.RoomStatus == 1 select r).Distinct();
            ViewBag.RoomNoID = new SelectList(result1, "RoomNoID", "Position");
            ViewBag.customer_NationalityID = new SelectList(db.Nationalities, "NationalityID", "NationalityName");
            CreateRevervation revervation = new CreateRevervation();
            Revervation rev = new Revervation();
            Customer cus = new Customer();
            cus.CustomerID = 0;
            rev.DateIn = datechoose.DateIn;
            rev.DateOut = datechoose.DateOut;
            rev.BookingDate = DateTime.Now;
            revervation.revervation = rev;
            revervation.customer = cus;
            return View(revervation);
        }

        //
        // POST: /Revervation/Create

        [HttpPost]
        public ActionResult Create(CreateRevervation reve, string[] ReservationsRoom)
        {
            reve.revervation.CustomerID = reve.customer.CustomerID;
            //reve.revervation.Customer = reve.customer; 
            if (ModelState.IsValid)
            {
                if (reve.customer.CustomerID == 0)
                {
                    var CustomerID = db.Database.SqlQuery<System.Int64>("exec GetNextCustomersSeq").FirstOrDefault();
                    reve.customer.CustomerID = int.Parse(CustomerID.ToString());
                    db.Customers.Add(reve.customer);
                }
                var resevationsID = db.Database.SqlQuery<System.Int64>("exec GetNextReservationsSeq").FirstOrDefault();

                reve.revervation.ReservationID = int.Parse(resevationsID.ToString());

                if (ReservationsRoom != null)
                { 
                    for (int i = 0; i < ReservationsRoom.Count(); i++)
                    {
                        ReservationsRoom room = new ReservationsRoom();
                        room.RoomID = ReservationsRoom[i];
                        room.ReservercationID = reve.revervation.ReservationID; 
                        reve.revervation.ReservationsRooms.Add(room);
                        db.ReservationsRooms.Add(room);
                    }
                }
                reve.revervation.Status = 1;
                reve.revervation.CustomerID = reve.customer.CustomerID;
                db.Revervations.Add(reve.revervation);
                
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            List<string> result = (from r in db.Rooms
                                   join resverRoom in db.ReservationsRooms
                                   on r.RoomNoID equals resverRoom.RoomID
                                   join resver in db.Revervations
                                   on resverRoom.ReservercationID equals resver.ReservationID
                                   where resver.Status != 2 && ((resver.DateOut >= reve.revervation.DateIn) && (resver.DateOut <= reve.revervation.DateOut) || (resver.DateIn <= reve.revervation.DateOut && resver.DateIn >= reve.revervation.DateIn))
                                   select r.RoomNoID).ToList<string>();
            ;
            var result1 = (from r in db.Rooms where !result.Contains(r.RoomNoID) select r).Distinct();
            ViewBag.RoomNoID = new SelectList(result1, "RoomNoID", "Position");
            ViewBag.customer_NationalityID = new SelectList(db.Nationalities, "NationalityID", "NationalityName");
            return View(reve);
        }


        public ActionResult ChooseDateReservation(){
            return View();
        }
        [HttpPost]
        public ActionResult ChooseDateReservation(ChooseDate datechoose)
        {
            if (ModelState.IsValid)
            {
                return RedirectToAction("Create", datechoose);
            }
            return View();
            
        }
        public ActionResult Deactive(int? id)
        {
            if (id != null && id != 0)
            {
                Revervation rev = db.Revervations.Find(id);
                rev.Status = 2;
                db.Revervations.Attach(rev);
                db.Entry(rev).State = EntityState.Modified;
                db.SaveChanges();
            }
            ViewBag.Message = "Hủy đặt phòng thành công";
            var revervations = db.Revervations.Where(u => u.Status != 2).Include(r => r.Customer).Include(r => r.RevervationRoomType);
            return View("Index",revervations.ToList()); 
        }
        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}