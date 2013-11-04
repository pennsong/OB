using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OB.Models;
using OB.Models.DAL;
using System.Data.Entity.Infrastructure;

namespace OB.Controllers
{
    public class BudgetCenterController : Controller
    {
        private OBContext db = new OBContext();

        //
        // GET: /BudgetCenter/

        public ActionResult Index()
        {
            var budgetcenter = db.BudgetCenter.Include(b => b.Client);
            return View(budgetcenter.ToList());
        }

        //
        // GET: /BudgetCenter/Details/5

        public ActionResult Details(int id = 0)
        {
            BudgetCenter budgetcenter = db.BudgetCenter.Find(id);
            if (budgetcenter == null)
            {
                return HttpNotFound();
            }
            return View(budgetcenter);
        }

        //
        // GET: /BudgetCenter/Create

        public ActionResult Create()
        {
            ViewBag.ClientId = new SelectList(db.Client, "Id", "Name");
            return View();
        }

        //
        // POST: /BudgetCenter/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(BudgetCenter budgetcenter)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    db.BudgetCenter.Add(budgetcenter);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                catch (DbUpdateException ex)
                {
                    if (ex.InnerException.InnerException.Message.Contains("Cannot insert duplicate key row"))
                    {
                        ModelState.AddModelError(string.Empty, "相同名称的记录已存在,保存失败!");
                    }
                }
            }

            ViewBag.ClientId = new SelectList(db.Client, "Id", "Name", budgetcenter.ClientId);
            return View(budgetcenter);
        }

        //
        // GET: /BudgetCenter/Edit/5

        public ActionResult Edit(int id = 0)
        {
            BudgetCenter budgetcenter = db.BudgetCenter.Find(id);
            if (budgetcenter == null)
            {
                return HttpNotFound();
            }
            ViewBag.ClientId = new SelectList(db.Client, "Id", "Name", budgetcenter.ClientId);
            return View(budgetcenter);
        }

        //
        // POST: /BudgetCenter/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(BudgetCenter budgetcenter)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    db.Entry(budgetcenter).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                catch (DbUpdateException ex)
                {
                    if (ex.InnerException.InnerException.Message.Contains("Cannot insert duplicate key row"))
                    {
                        ModelState.AddModelError(string.Empty, "相同名称的记录已存在,保存失败!");
                    }
                }
            }
            ViewBag.ClientId = new SelectList(db.Client, "Id", "Name", budgetcenter.ClientId);
            return View(budgetcenter);
        }

        //
        // GET: /BudgetCenter/Delete/5

        public ActionResult Delete(int id = 0)
        {
            BudgetCenter budgetcenter = db.BudgetCenter.Find(id);
            if (budgetcenter == null)
            {
                return HttpNotFound();
            }
            return View(budgetcenter);
        }

        //
        // POST: /BudgetCenter/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            BudgetCenter budgetcenter = db.BudgetCenter.Find(id);
            db.BudgetCenter.Remove(budgetcenter);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}