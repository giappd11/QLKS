using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using QLKS.Models;
using System.Web.Security;

namespace QLKS.Controllers
{
    public class HomeController : Controller
    {
        private Entities db = new Entities();
        

        // Màn hình trang chủ
        public ActionResult Index()
        {
            ViewBag.rommsActive = db.Rooms.Where(u => u.RoomStatus == 1).Count();
            ViewBag.rommsDeactive = db.Rooms.Where(u => u.RoomStatus == 2).Count();
            ViewBag.rommsWait = db.Rooms.Where(u => u.RoomStatus == 3).Count();
            ViewBag.rommsExpery = db.Rooms.Where(u => u.RoomStatus == 4).Count();
            ViewBag.rommsRepair = db.Rooms.Where(u => u.RoomStatus == 5).Count();
            var ListRooms = db.ListRooms.Where(u => db.CheckIns.Where(k => k.CheckInStatus == false).Select(k => k.CheckInID).Contains(u.CheckInID));

            // Danh sách phòng đã dc nhận có include thông tin khách hàng
            var result = from r in db.Rooms
                         join lr in db.ListRooms
                         on r.RoomNoID equals lr.RoomID
                         join ci in db.CheckIns
                         on lr.CheckInID equals ci.CheckInID
                         join ciCus in db.checkin_Custommer
                         on ci.CheckInID equals ciCus.CheckInID
                         join cus in db.Customers
                         on ciCus.CustomerID equals cus.CustomerID
                         where (ci.CheckInStatus == false && ciCus.roomMaster == true)
                         select new RoomHasCheckin { room = r, checkIn = ci, customer = cus }; 

            var rooms = db.Rooms.Where(u => u.RoomStatus != 0).OrderBy(u => u.Floor).OrderBy(u => u.Position);
            ListRoomHome roomHome = new ListRoomHome()
            {
                listCheckIn = result,
                listRoom = rooms
            };
            return View(roomHome);
        }

        // Trang Login 
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }


        // Function Login
        [HttpPost]
        public ActionResult Login(FormCollection collection)
        {
            string userName = collection["tendangnhap"].ToString();
            string passWord = collection["matkhau"].ToString();
            bool save = collection["save"] != null ? true : false;

            User user = db.Users.Where(u => u.login_id == userName && u.Pass == passWord).FirstOrDefault();
            if (user != null)
            {
                FormsAuthentication.SetAuthCookie(userName, save);

                return RedirectToAction("Index");
            }
            else
            {
                return View();
            }
        }

        // Function Logout
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login", "Home");
        }


    }
}
