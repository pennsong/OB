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
    public class CustomFieldController : Controller
    {
        private OBContext db = new OBContext();

        //
        // GET: /CustomField/

        public ActionResult Index()
        {
            var customfield = db.CustomField.Include(c => c.Client);
            return View(customfield.ToList());
        }

        //
        // GET: /CustomField/Details/5

        public ActionResult Details(int id = 0)
        {
            CustomField customfield = db.CustomField.Find(id);
            if (customfield == null)
            {
                return HttpNotFound();
            }
            return View(customfield);
        }

        //
        // GET: /CustomField/Create

        public ActionResult Create()
        {
            ViewBag.ClientId = new SelectList(db.Client, "Id", "Name");
            return View();
        }

        //
        // POST: /CustomField/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CustomField customfield)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    db.CustomField.Add(customfield);
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

            ViewBag.ClientId = new SelectList(db.Client, "Id", "Name", customfield.ClientId);
            return View(customfield);
        }

        //
        // GET: /CustomField/Edit/5

        public ActionResult Edit(int id = 0)
        {
            CustomField customfield = db.CustomField.Find(id);
            if (customfield == null)
            {
                return HttpNotFound();
            }
            ViewBag.ClientId = new SelectList(db.Client, "Id", "Name", customfield.ClientId);
            return View(customfield);
        }

        //
        // POST: /CustomField/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(CustomField customfield)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    db.Entry(customfield).State = EntityState.Modified;
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
            ViewBag.ClientId = new SelectList(db.Client, "Id", "Name", customfield.ClientId);
            return View(customfield);
        }

        //
        // GET: /CustomField/Delete/5

        public ActionResult Delete(int id = 0)
        {
            CustomField customfield = db.CustomField.Find(id);
            if (customfield == null)
            {
                return HttpNotFound();
            }
            return View(customfield);
        }

        //
        // POST: /CustomField/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            CustomField customfield = db.CustomField.Find(id);
            db.CustomField.Remove(customfield);
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