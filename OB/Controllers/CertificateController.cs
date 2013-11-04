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
    public class CertificateController : Controller
    {
        private OBContext db = new OBContext();

        //
        // GET: /Certificate/

        public ActionResult Index()
        {
            return View(db.Certificate.ToList());
        }

        //
        // GET: /Certificate/Details/5

        public ActionResult Details(int id = 0)
        {
            Certificate certificate = db.Certificate.Find(id);
            if (certificate == null)
            {
                return HttpNotFound();
            }
            return View(certificate);
        }

        //
        // GET: /Certificate/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Certificate/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Certificate certificate)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    db.Certificate.Add(certificate);
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

            return View(certificate);
        }

        //
        // GET: /Certificate/Edit/5

        public ActionResult Edit(int id = 0)
        {
            Certificate certificate = db.Certificate.Find(id);
            if (certificate == null)
            {
                return HttpNotFound();
            }
            return View(certificate);
        }

        //
        // POST: /Certificate/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Certificate certificate)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    db.Entry(certificate).State = EntityState.Modified;
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
            return View(certificate);
        }

        //
        // GET: /Certificate/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Certificate certificate = db.Certificate.Find(id);
            if (certificate == null)
            {
                return HttpNotFound();
            }
            return View(certificate);
        }

        //
        // POST: /Certificate/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Certificate certificate = db.Certificate.Find(id);
            db.Certificate.Remove(certificate);
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