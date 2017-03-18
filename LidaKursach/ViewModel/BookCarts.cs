using LidaKursach.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LidaKursach.ViewModel
{
    public class BookCarts
    {
        public Carts Cart { get; set; }
        public ICollection<Book> Books { get; set; }
    }
}