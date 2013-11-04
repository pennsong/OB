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
    public class SexController : Controller
    {
        private OBContext db = new OBContext();

        //
        // GET: /Sex/

        public ActionResult Index()
        {
            return View(db.Sex.ToList());
        }

        //
        // GET: /Sex/Details/5

        public ActionResult Details(int id = 0)
        {
            Sex sex = db.Sex.Find(id);
            if (sex == null)
            {
                return HttpNotFound();
            }
            return View(sex);
        }

        //
        // GET: /Sex/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Sex/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Sex sex)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    db.Sex.Add(sex);
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

            return View(sex);
        }

        //
        // GET: /Sex/Edit/5

        public ActionResult Edit(int id = 0)
        {
            Sex sex = db.Sex.Find(id);
            if (sex == null)
            {
                return HttpNotFound();
            }
            return View(sex);
        }

        //
        // POST: /Sex/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Sex sex)
        {
            if (ModelState.IsValid)
            {
                try{
                db.Entry(sex).State = EntityState.Modified;
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
            return View(sex);
        }

        //
        // GET: /Sex/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Sex sex = db.Sex.Find(id);
            if (sex == null)
            {
                return HttpNotFound();
            }
            return View(sex);
        }

        //
        // POST: /Sex/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Sex sex = db.Sex.Find(id);
            db.Sex.Remove(sex);
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