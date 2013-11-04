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
    public class LevelController : Controller
    {
        private OBContext db = new OBContext();

        //
        // GET: /Level/

        public ActionResult Index()
        {
            var level = db.Level.Include(l => l.Client);
            return View(level.ToList());
        }

        //
        // GET: /Level/Details/5

        public ActionResult Details(int id = 0)
        {
            Level level = db.Level.Find(id);
            if (level == null)
            {
                return HttpNotFound();
            }
            return View(level);
        }

        //
        // GET: /Level/Create

        public ActionResult Create()
        {
            ViewBag.ClientId = new SelectList(db.Client, "Id", "Name");
            return View();
        }

        //
        // POST: /Level/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Level level)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    db.Level.Add(level);
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

            ViewBag.ClientId = new SelectList(db.Client, "Id", "Name", level.ClientId);
            return View(level);
        }

        //
        // GET: /Level/Edit/5

        public ActionResult Edit(int id = 0)
        {
            Level level = db.Level.Find(id);
            if (level == null)
            {
                return HttpNotFound();
            }
            ViewBag.ClientId = new SelectList(db.Client, "Id", "Name", level.ClientId);
            return View(level);
        }

        //
        // POST: /Level/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Level level)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    db.Entry(level).State = EntityState.Modified;
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
            ViewBag.ClientId = new SelectList(db.Client, "Id", "Name", level.ClientId);
            return View(level);
        }

        //
        // GET: /Level/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Level level = db.Level.Find(id);
            if (level == null)
            {
                return HttpNotFound();
            }
            return View(level);
        }

        //
        // POST: /Level/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Level level = db.Level.Find(id);
            db.Level.Remove(level);
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