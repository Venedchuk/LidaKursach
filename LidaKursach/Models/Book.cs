using AspNetIdentityApp.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace LidaKursach.Models
{
    public class Book
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public virtual Genres Genre { get; set; }
        public int Count { get; set; }
    }
    public class Genres
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
    }
    public class Authors
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Biography { get; set; }
    }
    public class Carts
    {
        public int Id { get; set; }
        public DateTime Start_reading { get; set; }
        public DateTime? FinishReading { get; set; }
        public string Status { get; set; }
        public virtual Book Book_Id { get; set; }
        public virtual RegisterModel User_Id { get; set; }
    }



    public class MovieDBContext : DbContext
    {
        public DbSet<Book> Books { get; set; }
        public DbSet<Genres> Genres { get; set; }
    }
}