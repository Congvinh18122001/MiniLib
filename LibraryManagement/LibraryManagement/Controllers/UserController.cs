using LibraryManagement.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LibraryManagement.Objects;
using System.Net;
using System.Data.Entity;

namespace LibraryManagement.Controllers
{
    public class UserController : Controller
    {
        private CompanyContext db = new CompanyContext();
        // GET: User
        public ActionResult Catalogue()
        {
                        return View(db.Books.Where(p=>p.HasBeenBorrowed==false).ToList());
        }
        [Authorize]
        public ActionResult Borrow(int? id)
        {
            string UserId = string.Format(User.Identity.GetUserId());
            var Customer = db.Customers.SingleOrDefault(p => p.UserId.Equals(UserId));
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Book book = db.Books.Find(id);
            if (book == null)
            {
                return HttpNotFound();
            }
            LibraryManagement.Objects.Borrow borrow = new Borrow();
            if (Customer != null)
            {
                
                borrow.CustomerId = Customer.Id;
                borrow.BookId = book.Id;
                borrow.Customer = Customer;
                borrow.Book = book;
                return View(borrow);
            }
            return RedirectToAction("Catalogue");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Borrow([Bind(Include = "Id,CustomerId,BookId")] Borrow borrow)
        {
            if (ModelState.IsValid)
            {
                db.Borrows.Add(borrow);
                db.SaveChanges();
                return RedirectToAction("Catalogue");
            }

            ViewBag.BookId = new SelectList(db.Books, "Id", "Author", borrow.BookId);
            ViewBag.CustomerId = new SelectList(db.Customers, "Id", "Name", borrow.CustomerId);
            return View(borrow);
        }
        [Authorize]
        public ActionResult Profile(string UserId)
        {
            if (string.IsNullOrEmpty(UserId))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Customer customer = db.Customers.SingleOrDefault(p=>p.UserId.Equals(UserId));
            if (customer == null)
            {
                return RedirectToAction("CreateProfile");
            }
            return View(customer);
        }
        [Authorize]
        public ActionResult CreateProfile()
        {
            return View();
        }

        // POST: Customers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateProfile([Bind(Include = "Id,Name,Phone,Address,UserId")] Customer customer)
        {
            if (ModelState.IsValid)
            {
                db.Customers.Add(customer);
                db.SaveChanges();
                return RedirectToAction("Catalogue");
            }

            return View(customer);
        }
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Customer customer = db.Customers.Find(id);
            if (customer == null)
            {
                return HttpNotFound();
            }
            return View(customer);
        }

        // POST: Customers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Phone,Address,UserId")] Customer customer)
        {
            if (ModelState.IsValid)
            {
                db.Entry(customer).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Catalogue");
            }
            return View(customer);
        }



    }
}