﻿//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace QLKS.Models
{
    using System;
    using System.Web.Security;
    using System.Collections.Generic;
    using System.Web.Mvc;
    using System.ComponentModel.DataAnnotations;

    public partial class CheckIn
    {
        public CheckIn()
        {
            this.checkin_Custommer = new HashSet<checkin_Custommer>();
            this.Invoices = new HashSet<Invoice>();
            this.ListRooms = new HashSet<ListRoom>();
        }

        public int CheckInID { get; set; }

        public Nullable<int> GroupID { get; set; }

        [Required(ErrorMessage = "Không được để trống")]
        [Display(Name = "Ngày nhận phòng")]
        [DataType(DataType.Date)]
        [DateMustBeEqualOrGreaterThanCurrentDateValidation]

        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]

        public Nullable<System.DateTime> DateIn { get; set; }

        public Nullable<int> Discount { get; set; }

        public Nullable<decimal> Tax { get; set; }

        [Required(ErrorMessage = "Không được để trống")]
        [DataType(DataType.Date)]
        [Display(Name = "Ngày trả phòng")]
        [StartEndDateValidator]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]

        public Nullable<System.DateTime> DateOut { get; set; }

        public Nullable<int> OutofTime { get; set; }

        [Display(Name = "Ghi chú")]
        public string Comment { get; set; }

        public Nullable<decimal> RoomCharge { get; set; }

        public Nullable<bool> CheckInStatus { get; set; }


        public virtual ICollection<checkin_Custommer> checkin_Custommer { get; set; }
        public virtual ICollection<Invoice> Invoices { get; set; }
        public virtual ICollection<ListRoom> ListRooms { get; set; }
    }

    public class StartEndDateValidator : ValidationAttribute
    {
        protected override ValidationResult
                IsValid(object value, ValidationContext validationContext)
        {
            var model = (Models.CheckIn)validationContext.ObjectInstance;
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
}
