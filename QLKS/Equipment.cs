//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace QLKS
{
    using System;
    using System.Collections.Generic;
    
    public partial class Equipment
    {
        public string AmenityID { get; set; }
        public string RoomTypeID { get; set; }
        public Nullable<int> Amount { get; set; }
    
        public virtual Amenity Amenity { get; set; }
        public virtual RoomType RoomType { get; set; }
    }
}
