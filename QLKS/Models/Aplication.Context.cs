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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class Entities : DbContext
    {
        public Entities()
            : base("name=Entities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public DbSet<Amenity> Amenities { get; set; }
        public DbSet<CheckIn> CheckIns { get; set; }
        public DbSet<checkin_Custommer> checkin_Custommer { get; set; }
        public DbSet<CONFIGCOMP> CONFIGCOMPs { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Invoice> Invoices { get; set; }
        public DbSet<ListRoom> ListRooms { get; set; }
        public DbSet<Nationality> Nationalities { get; set; }
        public DbSet<RevervationRoomType> RevervationRoomTypes { get; set; }
        public DbSet<Revervation> Revervations { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<RoomType> RoomTypes { get; set; }
        public DbSet<RoomType_Amenity> RoomType_Amenity { get; set; }
        public DbSet<RoomType_ServiceFix> RoomType_ServiceFix { get; set; }
        public DbSet<ServiceFix> ServiceFixes { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<ReservationsRoom> ReservationsRooms { get; set; }
    }
}