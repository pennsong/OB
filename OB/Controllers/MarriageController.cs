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
    public class MarriageController : Controller
    {
        private OBContext db = new OBContext();

        //
        // GET: /Marriage/

        public ActionResult Index()
        {
            return View(db.Marriage.ToList());
        }

        //
        // GET: /Marriage/Details/5

        public ActionResult Details(int id = 0)
        {
            Marriage marriage = db.Marriage.Find(id);
            if (marriage == null)
            {
                return HttpNotFound();
            }
            return View(marriage);
        }

        //
        // GET: /Marriage/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Marriage/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Marriage marriage)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    db.Marriage.Add(marriage);
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

            return View(marriage);
        }

        //
        // GET: /Marriage/Edit/5

        public ActionResult Edit(int id = 0)
        {
            Marriage marriage = db.Marriage.Find(id);
            if (marriage == null)
            {
                return HttpNotFound();
            }
            return View(marriage);
        }

        //
        // POST: /Marriage/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Marriage marriage)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    db.Entry(marriage).State = EntityState.Modified;
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
            return View(marriage);
        }

        //
        // GET: /Marriage/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Marriage marriage = db.Marriage.Find(id);
            if (marriage == null)
            {
                return HttpNotFound();
            }
            return View(marriage);
        }

        //
        // POST: /Marriage/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Marriage marriage = db.Marriage.Find(id);
            db.Marriage.Remove(marriage);
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