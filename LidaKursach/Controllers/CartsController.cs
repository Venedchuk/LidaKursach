using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using LidaKursach.ViewModel;
using Microsoft.AspNet.Identity;

namespace LidaKursach.Models
{
    public class CartsController : Controller
    {
        private LibraryDBContext db = new LibraryDBContext();

        // GET: Carts
        public async Task<ActionResult> Index()
        {
            var id = Guid.Parse(System.Web.HttpContext.Current.User.Identity.GetUserId());
            return View(await db.Carts.Include(x=>x.Book_Id).Where(x=>x.Status=="Reading"&&x.User_Id== id).ToListAsync());
        }
        public async Task<ActionResult> History()
        {
            var id = Guid.Parse(System.Web.HttpContext.Current.User.Identity.GetUserId());
            return View(await db.Carts.Include(x => x.Book_Id).Where(x =>x.User_Id == id).ToListAsync());
        }

        // GET: Carts/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Carts carts = await db.Carts.FindAsync(id);
            if (carts == null)
            {
                return HttpNotFound();
            }
            return View(carts);
        }

        // GET: Carts/Create
        public ActionResult Create()
        {
            //BookCarts bk = new BookCarts() { Books= db.Books.ToList(),Cart=new Carts()};
            //return View(bk);
            //ViewBag.Books = db.Books.Where(x=>x.Count>0).ToList();
            //SelectListItem LItems = db.Books.Select(x => new SelectListItem
            //{
            //    Value = x.Id.ToString(),
            //    Text = x.Title
            //});

            //db.Books.Select(x =>);
            //foreach (var item in db.Books)
            //{

            //   var a = new SelectListItem
            //    {
            //        Value = item.Id.ToString(),
            //        Text = item.Title
            //    };


            //}
            //electList lst = new SelectList();

            ViewBag.AllBook = db.Books.Where(x => x.Count > 0);
            return View();
        }

        // POST: Carts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id")]Book bookCarts)
        {
            if (ModelState.IsValid && User.Identity.IsAuthenticated)
            {
                var cart = new Carts();
                cart.Start_reading = DateTime.Now;
                cart.User_Id = Guid.Parse(System.Web.HttpContext.Current.User.Identity.GetUserId());
                cart.Book_Id = db.Books.Single(x => x.Id == bookCarts.Id);
                cart.Status="Reading";
                db.Carts.Add(cart);
                var countbook = db.Books.Single(x => x.Id == cart.Book_Id.Id);
                countbook.Count--;
                db.Entry(countbook).State = EntityState.Modified;
                //db.Carts.Add(new Carts()
                //{
                //    Book_Id = db.Books.Single(x=>x.Id==bookCarts.Id),
                //    Status = "Reading",
                //    Start_reading = DateTime.Now,
                //    User_Id = Guid.Parse(System.Web.HttpContext.Current.User.Identity.GetUserId()),
                //}
                //);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(bookCarts);
        }

        // GET: Carts/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Carts carts = await db.Carts.FindAsync(id);
            if (carts == null)
            {
                return HttpNotFound();
            }
            return View(carts);
        }

        // POST: Carts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Start_reading,FinishReading,Status,User_Id")] Carts carts)
        {
            if (ModelState.IsValid)
            {
                db.Entry(carts).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(carts);
        }

        // GET: Carts/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Carts carts = await db.Carts.FindAsync(id);
            if (carts == null)
            {
                return HttpNotFound();
            }
            return View(carts);
        }

        // POST: Carts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Carts carts = await db.Carts.FindAsync(id);
            carts.Book_Id.Count++;
            carts.Status = "Finished";
            db.Entry(carts).State = EntityState.Modified;
            //db.Carts.Remove(carts);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
