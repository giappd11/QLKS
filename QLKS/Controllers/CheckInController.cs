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
    public class CheckInController : Controller
    {
        private Entities db = new Entities();


        // Danh sách checkIn chưa dc check out 
        public ActionResult ListCheckin()
        {
            // lấy toàn bộ danh sách checkin trong table checkIn trừ những hàng có checkinStatus = false ( đã trả hết phòng)
            var List = db.CheckIns.Where(u => u.CheckInStatus == false).ToList();
            return View(List);
        }

        // Danh sách checkIn đã dc check out 
        public ActionResult ListCheckout()
        {
            // lấy toàn bộ danh sách checkin trong table checkIn trừ những hàng có checkinStatus = false ( đã trả hết phòng)
            var List = db.CheckIns.Where(u => u.CheckInStatus == true).ToList();
            return View(List);
        }

        // vào trang đặt phòng
        public ActionResult CreateCheckin(int? resvationsID)
        {
            // Các ViewBag để hiện selectbox trên UI cho người dùng chọn
            ViewBag.NationalityID = new SelectList(db.Nationalities, "NationalityID", "NationalityName");
            ViewBag.CustomerID = new SelectList(db.Customers, "CustomerID", "IDCard");
            ViewBag.RoomNoID = new SelectList(db.Rooms.Where(u => u.RoomStatus == 1), "RoomNoID", "Position");
            ViewBag.RoomType = new SelectList(db.RoomTypes, "RoomTypeID", "RoomTypeName");

            ViewBag.ReservationsID = resvationsID;
            CheckIn checkin = new CheckIn();
            checkin.DateIn = DateTime.Now;
            List<Customer> listCus = new List<Customer>();
            string[] ListRooms = null;
            if (resvationsID != null && resvationsID != 0)
            {
                Revervation reser = db.Revervations.Find(resvationsID); 
                
                listCus.Add(reser.Customer);
                
                string list = "";
                foreach (ReservationsRoom rseRoom in  reser.ReservationsRooms ){
                    list += rseRoom.RoomID + ","; 
                }
                ListRooms = list.Split(',');
                checkin.DateOut = reser.DateOut;
            }
            // Khởi tạo ob View
            ViewChecin view = new ViewChecin()
            {
                Create = new ViewCreateChecin() { Create = checkin, CustomerList = listCus, ListRooms = ListRooms },
                List = db.CheckIns.Where(u => u.CheckInStatus == false).ToList(),
                ListRoom = db.Rooms.Where(a => a.RoomStatus == 1).Include(c => c.RoomType).ToList()
            };
            return View(view);
        }


        // Vào màn hình chọn phòng để checkout
        public ActionResult Checkout()
        {
            //viewBag danh sách phòng đã dc nhận
            ViewBag.RoomNoID = new SelectList(db.Rooms.Where(u => u.RoomStatus == 2), "RoomNoID", "Position");
            return View();
        }



        // vào trang Checkout
        public ActionResult ChooseRoom()
        {
            // Vì người dùng chưa chọn phòng, đánh link tĩnh để vào, thì chuyển sang trang chọn phòng để checkout
            return Redirect("Checkout");
        }


        // người dùng checkout 
        // truyền vào id của phòng
        [HttpPost]
        public ActionResult ChooseRoom(string RoomsId)
        {
            // Nếu RoomsId trống thì trở về trang chọn phòng để checkout
            if (RoomsId == "" || RoomsId == null)
            {
                return Redirect("Checkout");
            }
            //Nếu RoomsId khác rỗng thì vào trang trả phòng
            else
            {
                // Lấy ID của trong bảng checkin khi biết id của phòng (RoomsId)
                int maxcheckinID = db.CheckIns.Where(z => z.ListRooms.Select(c => c.RoomID).Contains(RoomsId) && z.CheckInStatus == false).OrderByDescending(z => z.CheckInID).Select(z => z.CheckInID).FirstOrDefault();
                // Khởi tao ob Checkin khi 
                //CheckIn checkin = db.CheckIns.Where(u => u.ListRooms.Select(a => a.RoomID).Contains(RoomsId) && u.CheckInStatus == false && u.CheckInID == maxcheckinID).FirstOrDefault();
                CheckIn checkin = db.CheckIns.Find(maxcheckinID);
                // Khởi tạo ob Invoice
                Invoice invoice = new Invoice();

                // tìm giá tiền của phòng đã dc thuê. ở bảng RoomType có giá phòng
                var priceRoom = db.RoomTypes.Where(a => a.RoomTypeID == db.Rooms.Where(u => u.RoomNoID == RoomsId).Select(u => u.RoomTypeID).FirstOrDefault()).Select(b => b.Price).FirstOrDefault();
                
                // Tính số ngày thuê phòng = ngày hiện tại trừ ngày nhận phòng
                int date = 1;                 
                date = Convert.ToInt32( DateTime.Now.Subtract(Convert.ToDateTime(checkin.DateIn)).TotalDays);
                // Nếu số ngày thuê phòng tính dc = 0 thì tính bằng 1
                if (date == 0) date = 1;

                // Tổng tiền bằng số ngày nhân với đơn giá phòng
                invoice.Total = (int)date * priceRoom;
                //
                invoice.CheckInID = checkin.CheckInID;
                // Ngày tạo invoice = ngày hiện tại 
                invoice.InvoiceDate = DateTime.Now;

                // Khởi tạo OB view 
                ViewCheckOut view = new ViewCheckOut()
                {
                    Create = checkin,
                    Invoice = invoice,
                    RoomID = RoomsId
                };

                ViewBag.Amenity = db.Amenities.ToList();
                //  System.Web.HttpContext.Current.Session["checkout"] = view;
                return View(view);
            }

        }



        //Khi người dùng vào trang checkout theo id của phòng và ấn "trả phòng"
        [HttpPost]
        public ActionResult CreateCheckOut(ViewCheckOut checkout)
        {
            // check modle có bị thiếu thông tin gì không. nếu thiếu thì sẽ ở lại trang và hiện thông báo còn không thì tiếp tục 
           // if (ModelState.IsValid)
           // {
                // Khởi tạo ob checkin 
               // CheckIn checkin = db.CheckIns.Where(u => u.CheckInID == checkout.Create.CheckInID).FirstOrDefault();
                CheckIn checkin = db.CheckIns.Find(checkout.Create.CheckInID);
                // Đặt lại checinID cho OB Invoice
                checkout.Invoice.CheckInID = checkin.CheckInID;

                // tạm insert ob Invoice vào database ( chưa có db.SaveChange() thì chưa dc gọi là lưu)
                db.Invoices.Add(checkout.Invoice);

                // Khởi tạo OB phòng để trả trạng thái phòng về trống
                Room room = db.Rooms.Find(checkout.RoomID);
                //Room room = db.Rooms.Where(u => u.RoomNoID == checkout.RoomID).FirstOrDefault();

                // Set trạng thái phòng = 1 ( trống)
                room.RoomStatus = 1;

                // Tạm update table Room ( chưa có db.SaveChange() thì chưa dc gọi là lưu)
                db.Rooms.Attach(room);
                db.Entry(room).State = EntityState.Modified;

                // Nếu 1 checkin thuê nhiều phòng. người dùng phải trả hết phòng thì mới chuyển trang thái checkin sang đã hoàn thành (true)
                // Đếm số lượng phòng chưa checkin 
                int count = db.ListRooms.Where(u => u.CheckInID == checkin.CheckInID).Select(a => a.Room).Where(a => a.RoomStatus == 2).Count();
                // Nếu số lượng phòng chưa checkin <= 1 (tính cả phòng chuẩn bị checkin )
                if (count <= 1)
                {
                    // Nếu thõa mãn điều kiện thì chuyển trạng thái của checkin sang true ( đã checkout hoàn toàn)
                    checkin.CheckInStatus = true;
                    // Tạm update table checkin
                    db.CheckIns.Attach(checkin);
                    db.Entry(checkin).State = EntityState.Modified;
                }
                db.Configuration.ValidateOnSaveEnabled = false;
                //Lưu tất cả các thay đổi
                db.SaveChanges(); 
          //  }
            ViewBag.Message = "Trả phòng thành công";
            // Trở về trang chọn phòng để Home
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

            var rooms = db.Rooms.OrderBy(u => u.Floor).OrderBy(u => u.Position);
            ListRoomHome roomHome = new ListRoomHome()
            {
                listCheckIn = result,
                listRoom = rooms
            };
            return View("../Home/Index",   roomHome);
        }



       
        // Tính toán lại tiền trả phòng + dịch vụ thêm
        public double Caculator(ViewCheckOut checkout, double Total)
        {
            double amount = Total;
            return amount;
        }



        // Xem thông tin checkIn  dựa vào id checkin
        // GET: /CheckIn/Details/5
        public ActionResult Details(int id = 0)
        {
            // Khởi tạo ob checkin dựa vào id checkin 
            CheckIn checkin = db.CheckIns.Find(id);

            // nếu checkin null: lỗi
            if (checkin == null)
            {
                return HttpNotFound();
            }

            // Khơi tạo OB View
            var detail = new QLKS.Models.DetailsChecin()
            {
                Create = checkin, 
                CustomerList = db.Customers.Where(u => db.checkin_Custommer.Where(ci => ci.CheckInID == checkin.CheckInID).Select(cus => cus.CustomerID).Contains(u.CustomerID)).ToList(),
                ListRoom = db.Rooms.Where(u => db.ListRooms.Where(a => a.CheckInID == checkin.CheckInID).Select(b => b.RoomID).Contains(u.RoomNoID)).ToList()
            };

            // các ViewBag hiện thông tin select box
            ViewBag.NationalityID = new SelectList(db.Nationalities, "NationalityID", "NationalityName");
            ViewBag.CustomerID = new SelectList(db.Customers, "CustomerID", "IDCard");
            ViewBag.RoomNoID = new SelectList(db.Rooms.Where(u => u.RoomStatus == 2), "RoomNoID", "Position");
            ViewBag.RoomType = new SelectList(db.RoomTypes, "RoomTypeID", "RoomTypeName");

            // trả về view (View/Checkin/Detail.cshtml)
            return View(detail);
        }

        
        //
        // GET: /CheckIn/Create 
        // Trang tạo checkin dc include vào trang CreateChecin.cshtml
        public ActionResult Create()
        {
            CheckIn checkin = new CheckIn();
            checkin.DateIn = DateTime.Now;
            return PartialView("Create", new QLKS.Models.ViewCreateChecin() { Create = checkin });
            //return View();
        }


         // Loại bỏ khách hàng đã thêm vào khi nhận phòng
        [HttpPost]
        public ActionResult RemoveCustomer(ViewCreateChecin checkin, string[] ListRooms, string selectedCus, int remove)
        {

            if (ListRooms != null)
            {
                for (int i = 0; i < ListRooms.Count(); i++)
                {
                    ListRoom room = new ListRoom();
                    room.RoomID = ListRooms[i];
                    room.CheckInID = checkin.Create.CheckInID;
                    checkin.Create.ListRooms.Add(room);
                }
            }
            if (checkin.CustomerList != null)
            {
                int i = 0;
                List<Customer> ListCustomer = new List<Customer>();
                foreach (var cus in checkin.CustomerList)
                {
                    if (i != remove)
                    {
                        ListCustomer.Add(cus);
                    }
                    i++;
                }
                checkin.CustomerList = ListCustomer;
            }
            ViewBag.RoomNoID = new SelectList(db.Rooms.Where(u => u.RoomStatus == 1), "RoomNoID", "Position", ListRooms);
            ViewBag.NationalityID = new SelectList(db.Nationalities, "NationalityID", "NationalityName");
            ViewBag.RoomType = new SelectList(db.RoomTypes, "RoomTypeID", "RoomTypeName");

            ViewChecin view = new ViewChecin()
            {
                Create = checkin,
                List = db.CheckIns.Where(u => u.CheckInStatus == false).Include(c => c.ListRooms).ToList(),
                ListRoom = db.Rooms.Where(a => a.RoomStatus == 1).Include(c => c.RoomType).ToList(),
                removeCus = "Y"
            };

            return View("CreateCheckin", view);
        }


        // Vào trang nhận phòng từ trang chủ
        [HttpPost]
        public ActionResult Index(string[] ListRooms)
        {
            ViewBag.NationalityID = new SelectList(db.Nationalities, "NationalityID", "NationalityName");
            ViewBag.CustomerID = new SelectList(db.Customers, "CustomerID", "IDCard");
            ViewBag.RoomNoID = new SelectList(db.Rooms.Where(u => u.RoomStatus == 1), "RoomNoID", "Position");
            ViewBag.RoomType = new SelectList(db.RoomTypes, "RoomTypeID", "RoomTypeName");
            ViewCreateChecin create = new ViewCreateChecin();
            ListRoom listRoom = new ListRoom() { RoomID = ListRooms[0], CheckInID = 0 };
            create.Create = new CheckIn();
            if (create.Create.DateIn == null)
            {
                create.Create.DateIn = DateTime.Now;
            }
            create.Create.ListRooms = new List<ListRoom>();
            create.Create.ListRooms.Add(listRoom);
            ViewChecin view = new ViewChecin()
            {
                Create = create,
                List = db.CheckIns.Where(u => u.CheckInStatus == false).ToList(),
                ListRoom = db.Rooms.Where(a => a.RoomStatus == 1).Include(c => c.RoomType).ToList()
            };
            //string[] ListRooms = { id };
            return View("CreateCheckin", view);
        }


        //Nhận phòng 
        // param: checkin: OB ViewCreateChecIn 
        //        ListRooms: Danh sách id phòng muốn nhận
        //        selectedCus:  Số thứ tự khách hàng là chủ phòng trong danh sách khách hàng
        [HttpPost]
        public ActionResult CreateCheckin(ViewCreateChecin checkin, string[] ListRooms, string selectedCus, int? ReservationsID)
        {
            if (checkin.removeCus != "Y")
            {
                if (ModelState.IsValid)
                {
                    // Lấy ID dựa vào seq trong database
                    var checkinID = db.Database.SqlQuery<System.Int64>("exec GetNextCheckinSeq").FirstOrDefault();
                    // Tạo ID cho OB checkin
                    checkin.Create.CheckInID = int.Parse(checkinID.ToString());
                    //Tạo status mặc định bằng false cho "nhận phòng" mới
                    checkin.Create.CheckInStatus = false;

                    // Tạm lưu OB checkin vào database
                    db.CheckIns.Add(checkin.Create);

                    // Danh sách phòng nhận
                    if (ListRooms != null)
                    {
                        // Thêm tất cả các phòng dc nhận vào bàng "ListRooms"
                        for (int i = 0; i < ListRooms.Count(); i++)
                        {
                            ListRoom room = new ListRoom();
                            room.RoomID = ListRooms[i];
                            room.CheckInID = checkin.Create.CheckInID;
                            checkin.Create.ListRooms.Add(room);
                            db.ListRooms.Add(room);
                        }
                    }

                    // Chuyển trạng thái tất cả các phòng dc nhận sang 2 ( đã có người nhận phòng)
                    foreach (var item in checkin.Create.ListRooms)
                    {
                        var room = db.Rooms.Where(u => u.RoomNoID == item.RoomID).FirstOrDefault();
                        room.RoomStatus = 2;
                        db.Rooms.Attach(room);
                        db.Entry(room).State = EntityState.Modified;
                    }

                    // Danh sách khách hàng
                    if (checkin.CustomerList != null)
                    {
                        int i = 0;
                        foreach (var cus in checkin.CustomerList)
                        {
                            // Nếu id khách hàng = 0 thì insert vào database, còn khác 0 thì chỉnh sửa thông tin khác hàng
                            if (cus.CustomerID == 0)
                            {
                                // Lấy ID mới cho khác hàng
                                var CustomerID = db.Database.SqlQuery<System.Int64>("exec GetNextCustomersSeq").FirstOrDefault();
                                cus.CustomerID = int.Parse(CustomerID.ToString());
                                // Tạm thêm khác hàng vào database
                                db.Customers.Add(cus);
                            }
                            else
                            {
                                db.Customers.Attach(cus);
                                db.Entry(cus).State = EntityState.Modified;
                            }

                            // Khởi tạo liên kết giữa khách hàng và checkin 
                            checkin_Custommer checkin_cus = new checkin_Custommer();

                            // set giá trị checkin ID vào liên kết
                            checkin_cus.CheckInID = int.Parse(checkinID.ToString());
                            // set giá trị Custommer id vào liên kết
                            checkin_cus.CustomerID = int.Parse(cus.CustomerID.ToString());

                            // nếu selectCus( chủ phòng) là đúng thì set liên kết có roomMaster = true còn không phải thì set là false
                            if (selectedCus != null && selectedCus != "")
                            {
                                if (int.Parse(selectedCus) == i)
                                {
                                    checkin_cus.roomMaster = true;
                                }
                                else
                                {
                                    checkin_cus.roomMaster = false;
                                }
                            }

                            // thêm liên kết checkin_cus vào danh sách  liên kết khách hàng - Checkin ( checkin_Custommer )
                            checkin.Create.checkin_Custommer.Add(checkin_cus);

                            // Khởi tạo id cho liên kết khách hàng - Checkin 
                            var id_customer_checkin = db.Database.SqlQuery<System.Int64>("exec GetNextcheckInCustomerSeq").FirstOrDefault();
                            checkin_cus.id = int.Parse(id_customer_checkin.ToString());
                            // Tạm lưu liên kết vào database
                            db.checkin_Custommer.Add(checkin_cus);
                            i++;
                        }
                    }
                    if (ReservationsID != null && ReservationsID != 0  )
                    {
                        Revervation rev = db.Revervations.Find(ReservationsID);
                        rev.Status = 2;
                        db.Revervations.Attach(rev);
                        db.Entry(rev).State = EntityState.Modified;
                    }
                    // Lưu lại tất cả thay đổi
                    db.SaveChanges();

                    // Lấy danh sách đã checkin và chuyển sang trang danh sách checkin + thông báo nhân phòng thành công
                    var List = db.CheckIns.Where(u => u.CheckInStatus == false).ToList();
                    ViewBag.Message = "Nhận phòng thành công";
                    return View("ListCheckin", List.ToList());
                    //return RedirectToAction("Index");
                }
            }

            // Nếu bị lỗi dữ liệu thì  trở lại trang và thông báo lỗi hoặc thiếu thông tin ở các field
            ViewBag.RoomNoID = new SelectList(db.Rooms.Where(u => u.RoomStatus == 1), "RoomNoID", "Position", ListRooms);
            ViewBag.NationalityID = new SelectList(db.Nationalities, "NationalityID", "NationalityName");
            ViewBag.RoomType = new SelectList(db.RoomTypes, "RoomTypeID", "RoomTypeName");
            ViewBag.ReservationsID = ReservationsID;
            //
            ViewChecin view = new ViewChecin()
            {
                Create = checkin,
                List = db.CheckIns.Where(u => u.CheckInStatus == false).Include(c => c.ListRooms).ToList(),
                ListRoom = db.Rooms.Where(a => a.RoomStatus == 1).Include(c => c.RoomType).OrderBy(a => a.Position).ToList()
            };

            // trở lại trang nhận phòng nếu lỗi dữ liệu
            return View(view);
        }


        // checkout dựa vào checkIn id
        public ActionResult TraPhong(int id = 0)
        {
            // Tim
            CheckIn checkin = db.CheckIns.Where(i => i.CheckInID == id).FirstOrDefault();
            Invoice invoice = new Invoice();
            // var priceRoom = db.RoomTypes.Where(a => a.RoomTypeID == db.Rooms.Where(u => u.RoomNoID == ListRoomss).Select(u => u.RoomTypeID).FirstOrDefault()).Select(b => b.Price).FirstOrDefault();

            //Tính thời gian thuê phòng
            int date = 1;
            // số ngày thuê phòng = thời gian hiện tại trừ thời gian bắt đầu nhận phòng 
            date = Convert.ToInt32( DateTime.Now.Subtract(Convert.ToDateTime(checkin.DateIn)).TotalDays );
            // Nếu số ngày mượn phòng < 1 ngày thì tính bằng 1 ngày
            if (date <= 1) date = 1;
            invoice.Total = 0;
            for (int i = 0; i < checkin.ListRooms.Count(); i++)
            {
                if (checkin.ListRooms.ToList()[i].Room.RoomStatus == 2)
                {
                    string roomTypeID = checkin.ListRooms.ToList()[i].Room.RoomTypeID;
                    var priceRoom =  db.RoomTypes.Where(a => a.RoomTypeID == roomTypeID).Select(b => b.Price).FirstOrDefault() ;
                    invoice.Total += (int)date * priceRoom;
                }
            }
            
            invoice.CheckInID = checkin.CheckInID;
            invoice.InvoiceDate = DateTime.Now;

            ViewCheckOut view = new ViewCheckOut()
            {
                Create = checkin,
                Invoice = invoice
            };
            ViewBag.Amenity = db.Amenities.ToList();
            return View(view); 
        }

        [HttpPost]
        public ActionResult TraPhong(ViewCheckOut checkout)
        {
            foreach (var key in ModelState.Keys.ToList().Where(key => ModelState.ContainsKey(key)))
            {
                //ModelState.Remove(key); //This was my solution before
                ModelState[key].Errors.Clear(); //This is my new solution. Thanks bbak
            }
            // check modle có bị thiếu thông tin gì không. nếu thiếu thì sẽ ở lại trang và hiện thông báo còn không thì tiếp tục 
           // if (ModelState.IsValid)
           // {
                // Khởi tạo ob checkin 
                // CheckIn checkin = db.CheckIns.Where(u => u.CheckInID == checkout.Create.CheckInID).FirstOrDefault();
                CheckIn checkin = db.CheckIns.Find(checkout.Create.CheckInID);
                // Đặt lại checinID cho OB Invoice
                checkout.Invoice.CheckInID = checkin.CheckInID;

                // tạm insert ob Invoice vào database ( chưa có db.SaveChange() thì chưa dc gọi là lưu)
                db.Invoices.Add(checkout.Invoice);

                for (int i = 0; i < checkin.ListRooms.Count(); i++)
                {
                    if (checkin.ListRooms.ToList()[i].Room.RoomStatus == 2)
                    {
                        // Khởi tạo OB phòng để trả trạng thái phòng về trống
                        Room room = db.Rooms.Find(checkin.ListRooms.ToList()[i].Room.RoomNoID);
                        //Room room = db.Rooms.Where(u => u.RoomNoID == checkout.RoomID).FirstOrDefault();

                        // Set trạng thái phòng = 1 ( trống)
                        room.RoomStatus = 1;

                        // Tạm update table Room ( chưa có db.SaveChange() thì chưa dc gọi là lưu)
                        db.Rooms.Attach(room);
                        db.Entry(room).State = EntityState.Modified;
                    }
                } 
                 
                // Nếu thõa mãn điều kiện thì chuyển trạng thái của checkin sang true ( đã checkout hoàn toàn)
                checkin.CheckInStatus = true;
                // Tạm update table checkin
                db.CheckIns.Attach(checkin);
                db.Entry(checkin).State = EntityState.Modified;
                db.Configuration.ValidateOnSaveEnabled = false;
                //Lưu tất cả các thay đổi
                db.SaveChanges();
            //}
            ViewBag.Message = "Trả phòng thành công";
            // Trở về trang danh sách nhận phòng
            return View("ListCheckin", db.CheckIns.Where(u => u.CheckInStatus == false).ToList()); 
        }


        // Các hàm dưới được khởi tạo mặc định, ko dùng tới
        //
        // GET: /CheckIn/Edit/5

        public ActionResult Edit(int id = 0)
        {
            CheckIn checkin = db.CheckIns.Find(id);
            if (checkin == null)
            {
                return HttpNotFound();
            }
            ViewBag.ListRooms = new SelectList(db.ListRooms, "RoomNoID", "RoomTypeID", checkin.CheckInID);
            return View(checkin);
        }

        //
        // POST: /CheckIn/Edit/5

        [HttpPost]
        public ActionResult Edit(CheckIn checkin)
        {
            if (ModelState.IsValid)
            {
                db.Entry(checkin).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.RoomNoID = new SelectList(db.Rooms, "RoomNoID", "RoomTypeID", checkin.CheckInID);
            return View(checkin);
        }

        //
        // GET: /CheckIn/Delete/5

        public ActionResult Delete(int id = 0)
        {
            CheckIn checkin = db.CheckIns.Find(id);
            if (checkin == null)
            {
                return HttpNotFound();
            }
            return View(checkin);
        }

        //
        // POST: /CheckIn/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            CheckIn checkin = db.CheckIns.Find(id);
            db.CheckIns.Remove(checkin);
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