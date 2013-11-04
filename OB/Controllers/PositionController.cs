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
    public class PositionController : Controller
    {
        private OBContext db = new OBContext();

        //
        // GET: /Position/

        public ActionResult Index()
        {
            var position = db.Position.Include(p => p.Client);
            return View(position.ToList());
        }

        //
        // GET: /Position/Details/5

        public ActionResult Details(int id = 0)
        {
            Position position = db.Position.Find(id);
            if (position == null)
            {
                return HttpNotFound();
            }
            return View(position);
        }

        //
        // GET: /Position/Create

        public ActionResult Create()
        {
            ViewBag.ClientId = new SelectList(db.Client, "Id", "Name");
            return View();
        }

        //
        // POST: /Position/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Position position)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    db.Position.Add(position);
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

            ViewBag.ClientId = new SelectList(db.Client, "Id", "Name", position.ClientId);
            return View(position);
        }

        //
        // GET: /Position/Edit/5

        public ActionResult Edit(int id = 0)
        {
            Position position = db.Position.Find(id);
            if (position == null)
            {
                return HttpNotFound();
            }
            ViewBag.ClientId = new SelectList(db.Client, "Id", "Name", position.ClientId);
            return View(position);
        }

        //
        // POST: /Position/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Position position)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    db.Entry(position).State = EntityState.Modified;
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
            ViewBag.ClientId = new SelectList(db.Client, "Id", "Name", position.ClientId);
            return View(position);
        }

        //
        // GET: /Position/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Position position = db.Position.Find(id);
            if (position == null)
            {
                return HttpNotFound();
            }
            return View(position);
        }

        //
        // POST: /Position/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Position position = db.Position.Find(id);
            db.Position.Remove(position);
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