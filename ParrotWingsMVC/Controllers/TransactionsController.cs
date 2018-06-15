using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using ParrotWingsMVC;
using ParrotWingsMVC.Models;
using Vereyon.Web;

namespace ParrotWingsMVC.Controllers
{
    public class TransactionsController : Controller
    {
        private Entities db = new Entities();

        // GET: Transactions
        [Authorize]
        public ActionResult Index()
        {
            return View(db.Transactions.ToList());
        }

        // GET: Transactions/Details/5
        [Authorize]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Transactions transactions = db.Transactions.Find(id);
            if (transactions == null)
            {
                return HttpNotFound();
            }
            return View(transactions);
        }

        // GET: Transactions/Create
        [Authorize]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Transactions/Create
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(TransactionViewModel mod, [Bind(Include = "CorrespondentName,TransactionAmount")] Transactions transactions)
        {
            if (!ModelState.IsValid)
            {
                return View("Create", transactions);
            }
            using (var context = new ApplicationDbContext())
            {
                transactions.DateTime = DateTime.Now; //initializing datetime

                var store = new UserStore<ApplicationUser>(context);
                var manager = new UserManager<ApplicationUser>(store);

                var currentUser = manager.FindById(User.Identity.GetUserId()); //getting current user (sender) by id           
                var queryresult = from u in db.AspNetUsers  //getting correspondent user with selected name
                                  where u.Name == transactions.CorrespondentName
                                  select u;

                var correspondentUser = manager.FindById(queryresult.First().Id.ToString()); //finally getting correspondent user by id
                int resultBalance = currentUser.Balance - transactions.TransactionAmount; 
                transactions.UserId = currentUser.Id;

                if (resultBalance >= 0) //else show message
                {
                    if (ModelState.IsValid && (correspondentUser != null))
                    {
                        transactions.ResultBalance = resultBalance; //initializing result balance
                        db.Transactions.Add(transactions);
                        currentUser.Balance = resultBalance;
                        correspondentUser.Balance += transactions.TransactionAmount; //transfer money

                        var result1 = manager.Update(currentUser);
                        context.Entry(currentUser).State = EntityState.Modified;
                        var result2 = manager.Update(correspondentUser);
                        context.Entry(correspondentUser).State = EntityState.Modified;
                        if (!result1.Succeeded || !result2.Succeeded)
                        {
                            Response.Write("<script>alert('Sorry, transaction can't be completed. Try again later.')</script>");
                            return View("Create", transactions);
                        }
                        else
                        {
                            db.SaveChanges(); //saving transaction
                            var ctx = store.Context;
                            ctx.SaveChanges(); //updating users' balances
                            Response.Write("<script>alert('Transaction completed succesfully!')</script>");
                        }
                        return RedirectToAction("Index");
                    }
                }
                else Response.Write("<script>alert('You do not have enough money for this transaction.')</script>");
                
                return View(transactions);
            }
        }

        public IEnumerable<SelectListItem> GetCorrespondentNames() //get all users who available to get money
        {
            List<SelectListItem> ls = new List<SelectListItem>();
            ApplicationDbContext context = new ApplicationDbContext();
            List<ApplicationUser> users = context.Users.ToList();

            foreach (ApplicationUser au in users)
                if (au.Id != User.Identity.GetUserId()) //if it's not a current user
                {
                    ls.Add(new SelectListItem() { Text = au.Name, Value = au.Name });
                }
            return ls;
        }

        /*     [HttpGet]
             public JsonResult CheckTransactionSum(int TransactionAmount)
             {
                 bool result = false;//!(sum < InformationProvider.GetCurrentUserBalance());
                 //return result;
                 return Json(result, JsonRequestBehavior.AllowGet);
             }*/

        // GET: Transactions/Edit/5
        [Authorize]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Transactions transactions = db.Transactions.Find(id);
            if (transactions == null)
            {
                return HttpNotFound();
            }
            return View(transactions);
        }

        // POST: Transactions/Edit/5
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "TransactionId,DateTime,CorrespondentName,TransactionAmount,ResultBalance,UserId")] Transactions transactions)
        {
            if (ModelState.IsValid)
            {
                db.Entry(transactions).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(transactions);
        }

        // GET: Transactions/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Transactions transactions = db.Transactions.Find(id);
            if (transactions == null)
            {
                return HttpNotFound();
            }
            return View(transactions);
        }

        // POST: Transactions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Transactions transactions = db.Transactions.Find(id);
            db.Transactions.Remove(transactions);
            db.SaveChanges();
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
