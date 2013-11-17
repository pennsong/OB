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
using WebMatrix.WebData;

namespace OB.Controllers
{
    public class DocumentController : Controller
    {
        private OBContext db = new OBContext();

        //
        // GET: /Document/
        [Authorize(Roles = "HRAdmin")]
        public ActionResult Index()
        {
            var clients = db.User.Where(a => a.Id == WebSecurity.CurrentUserId).Single().HRAdminClients.Select(b => b.Id);
            var document = db.Document.Where(a => clients.Contains(a.ClientId));
            return View(document.ToList());
        }

        //
        // GET: /Document/Details/5

        public ActionResult Details(int id = 0)
        {
            Document document = db.Document.Find(id);
            if (document == null)
            {
                return HttpNotFound();
            }
            return View(document);
        }

        //
        // GET: /Document/Create
        [Authorize(Roles = "HRAdmin")]
        public ActionResult Create()
        {
            ViewBag.ClientId = new SelectList(db.User.Where(a => a.Id == WebSecurity.CurrentUserId).Single().HRAdminClients, "Id", "Name");
            return View();
        }

        //
        // POST: /Document/Create
        [Authorize(Roles = "HRAdmin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Document document)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    db.Document.Add(document);
                    //db.SaveChanges();
                    db.PPSave();
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

            ViewBag.ClientId = new SelectList(db.User.Where(a => a.Id == WebSecurity.CurrentUserId).Single().HRAdminClients, "Id", "Name");
            return View(document);
        }

        //
        // GET: /Document/Edit/5
        [Authorize(Roles = "HRAdmin")]
        public ActionResult Edit(int id = 0)
        {
            var clients = db.User.Where(a => a.Id == WebSecurity.CurrentUserId).Single().HRAdminClients.Select(b => b.Id);
            Document document = db.Document.Where(a => a.Id == id && clients.Contains(a.ClientId)).SingleOrDefault();
            if (document == null)
            {
                return HttpNotFound();
            }
            return View(document);
        }

        //
        // POST: /Document/Edit/5
        [Authorize(Roles = "HRAdmin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Document document)
        {
            var clients = db.User.Where(a => a.Id == WebSecurity.CurrentUserId).Single().HRAdminClients.Select(b => b.Id);
            Document document0 = db.Document.Where(a => a.Id == document.Id && clients.Contains(a.ClientId)).SingleOrDefault();
            if (document0 == null)
            {
                return HttpNotFound();
            }
            if (ModelState.IsValid)
            {
                try
                {
                    document0.Name = document.Name;
                    document0.Weight = document.Weight;
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
            return View(document);
        }

        //
        // GET: /Document/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Document document = db.Document.Find(id);
            if (document == null)
            {
                return HttpNotFound();
            }
            return View(document);
        }

        //
        // POST: /Document/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Document document = db.Document.Find(id);
            db.Document.Remove(document);
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