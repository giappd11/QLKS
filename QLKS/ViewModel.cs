using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

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
    }
    public class ViewCreateChecin
    { 
        public CheckIn Create { get; set; }
        public Customer Customer { get; set; } 
    } 


}