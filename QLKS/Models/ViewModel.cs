using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using QLKS.Models;
namespace QLKS.Models
{
   
    public class ViewRoomType
    {
        public IEnumerable<RoomType> List { get; set; }
        public RoomType Create { get; set; }
    }
    public class ViewRoom
    {
        public IEnumerable<Room> List { get; set; }
        public Room Create { get; set; }
        public IEnumerable<RoomType> ListRoomType { get; set; }
    }
    public class ViewChecin
    {
        public IEnumerable<CheckIn> List { get; set; }
        public ViewCreateChecin Create { get; set; } 
        public IEnumerable<Customer> ListCustomer { get; set; }
        public IEnumerable<Room> ListRoom { get; set; }
        public string removeCus { get; set; }
       
    }
    public class ViewCreateChecin
    { 
        public CheckIn Create { get; set; }
        public Customer Customer { get; set; }
        public IEnumerable<Customer> CustomerList { get; set; }
        [Required(ErrorMessage="Chủ phòng/đoàn không được để trống")]
        public string selectedCus { get; set; }
        public string removeCus { get; set; }
        [Required(ErrorMessage = "Không được để trống")]
        [ListRoomChecinValidator]
        public string[] ListRooms { get; set; }
        public string ReservationsID { get; set; }
    }
    public class DetailsChecin
    {
        public CheckIn Create { get; set; }
        public Customer Customer { get; set; }
        public IEnumerable<Room> ListRoom { get; set; }
        public IEnumerable<Customer> CustomerList { get; set; }
        public string selectedCus { get; set; }
        public string removeCus { get; set; }
    }
    public class ViewCheckOut
    {
        public CheckIn Create { get; set; }
        public Invoice Invoice { get; set; }
        public string RoomID { get; set; }  
    }
    public class RoomHasCheckin 
    {
        public CheckIn checkIn { get; set; }
        public Room room { get; set; }
        public Customer customer { get; set; }
    }
    public class ListRoomHome 
    {
        public IEnumerable<RoomHasCheckin> listCheckIn { get; set; }
        public IEnumerable<Room> listRoom { get; set; }
    
    }
    public class CreateRevervation
    {
        public Revervation revervation { get; set; }
        public Customer customer { get; set; }
        [Required(ErrorMessage = "Không được để trống")]
        public string[] ReservationsRoom { get; set; }

    }
    public class ChooseDate
    {
        [Display(Name = "Ngày nhận phòng")]
        [DateMustBeEqualOrGreaterThanCurrentDateValidation]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        [Required(ErrorMessage = "Không được để trống")]
        public DateTime DateIn { get; set; }

        [Display(Name = "Ngày trả phòng")]
        [StartEndDateChoseValidator]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        [Required(ErrorMessage = "Không được để trống")]
        public DateTime DateOut { get; set; }


    }
    
    public class CustomerView { 
        public int CustomerID { get; set; } 
        public string IDCard { get; set; } 
        public string Name { get; set; } 
        public string Phone { get; set; } 
        public string Mail { get; set; } 
        public string Address { get; set; } 
        public Nullable<bool> Sex { get; set; } 
        public Nullable<int> NationalityID { get; set; } 
        public string Company { get; set; } 
    
    }

    public class StartEndDateChoseValidator : ValidationAttribute
    {
        protected override ValidationResult
                IsValid(object value, ValidationContext validationContext)
        {
            var model = (Models.ChooseDate)validationContext.ObjectInstance;
            DateTime StartDate = Convert.ToDateTime(model.DateIn);
            DateTime EndDate = Convert.ToDateTime(value);

            if (StartDate > EndDate)
            {
                return new ValidationResult
                    ("Ngày phải lớn hơn hoặc bằng ngày nhận phòng");
            }
            else
            {
                return ValidationResult.Success;
            }
        }
    }
    public class ListRoomChecinValidator : ValidationAttribute
    {
        protected override ValidationResult
                IsValid(object value, ValidationContext validationContext)
        {
            Entities db = new Entities();
            var model = (Models.ViewCreateChecin)validationContext.ObjectInstance;
            DateTime StartDate = Convert.ToDateTime(model.Create.DateIn);
            DateTime EndDate = Convert.ToDateTime(model.Create.DateOut);
            string[] listRoom = (string[]) value ;
            string messsage = "";
            bool check = true;
            for (int i = 0; i < listRoom.Length; i++)
            {
                string id = listRoom[i];
                var result = from r in db.Rooms
                              join resverRoom in db.ReservationsRooms
                              on r.RoomNoID equals resverRoom.RoomID
                              join resver in db.Revervations
                              on resverRoom.ReservercationID equals resver.ReservationID
                             where r.RoomNoID == id &&  resver.Status != 2 && ((resver.DateOut >= StartDate) && (resver.DateOut <= EndDate) || (resver.DateIn <= EndDate && resver.DateIn >= StartDate))
                              select r ;
                if (result.Count() > 0)
                {
                    check = false;
                    if (messsage == "")
                    {
                        messsage += result.FirstOrDefault().Position ;
                    }
                    else
                    {
                        messsage += ", " + result.FirstOrDefault().Position;
                    }
                }
            }
            if (check == false && model.ReservationsID == null)
            { 
                return new ValidationResult
                    ("Phòng " + messsage + " Đã được đặt trong thời gian " + StartDate.ToShortDateString() + " đến " + EndDate.ToShortDateString());
            }
            
            else
            {
                return ValidationResult.Success;
            }
        }
    }
}