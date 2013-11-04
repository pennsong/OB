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
using System.Web.Security;
using OB.Lib;
using OB.Models.ViewModel;
using WebMatrix.WebData;

namespace OB.Controllers
{
    public class ClientController : Controller
    {
        private OBContext db = new OBContext();
        public ClientController()
        {
            ViewBag.Path1 = "参数设置";
        }

        //
        // GET: /Client/
        [Authorize(Roles = "Admin")]
        public ActionResult Index()
        {

            var client = db.Client.Include(c => c.HRAdmin);
            return View(client.ToList());
        }

        [Authorize(Roles = "HRAdmin")]
        public ActionResult HRAdminClientIndex()
        {

            var client = db.Client.Where(a => a.HRAdminId == WebSecurity.CurrentUserId);
            return View(client.ToList());
        }

        //
        // GET: /Client/Details/5
        [Authorize(Roles = "Admin")]
        public ActionResult Details(int id = 0)
        {
            Client client = db.Client.Find(id);
            if (client == null)
            {
                return HttpNotFound();
            }
            return View(client);
        }

        //
        // GET: /Client/Create
        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            ViewBag.HRAdminId = new SelectList(Common.UserList("HRAdmin", db), "Id", "Name");
            return View();
        }

        //
        // POST: /Client/Create
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Client client)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    db.Client.Add(client);
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
            ViewBag.HRAdminId = new SelectList(Common.UserList("HRAdmin", db), "Id", "Name");
            return View(client);
        }

        //
        // GET: /Client/Edit/5
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int id = 0)
        {
            Client client = db.Client.Find(id);
            if (client == null)
            {
                return HttpNotFound();
            }

            ViewBag.HRAdminList = Common.UserList("HRAdmin", db);

            return View(client);
        }

        //
        // POST: /Client/Edit/5
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Client client)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    db.Entry(client).State = EntityState.Modified;
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
            ViewBag.HRAdminId = new SelectList(Common.UserList("HRAdmin", db), "Id", "Name");
            return View(client);
        }

        [Authorize(Roles = "HRAdmin")]
        public ActionResult EditClientHR(int id = 0)
        {
            Client client = db.Client.Where(a => a.Id == id && a.HRAdminId == WebSecurity.CurrentUserId).SingleOrDefault();
            if (client == null)
            {
                return HttpNotFound();
            }
            var editClientHr = new EditClientHR
            {
                ClientId = client.Id,
                ClientName = client.Name,
                HRIds = client.HRs.Select(a => a.Id).ToList()
            };
            return View(editClientHr);
        }

        [Authorize(Roles = "HRAdmin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditClientHR(EditClientHR editClientHR)
        {
            Client client = db.Client.Include(a => a.HRs).Where(a => a.Id == editClientHR.ClientId && a.HRAdminId == WebSecurity.CurrentUserId).SingleOrDefault();
            if (client == null)
            {
                return HttpNotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var t = db.User.Where(a => editClientHR.HRIds.Any(b => b == a.Id)).ToList();
                    client.HRs = t;
                    db.SaveChanges();
                    return RedirectToAction("HRAdminClientIndex");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                }
            }
            return View(editClientHR);
        }
        //
        // GET: /Client/Delete/5
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int id = 0)
        {
            Client client = db.Client.Find(id);
            if (client == null)
            {
                return HttpNotFound();
            }
            return View(client);
        }

        //
        // POST: /Client/Delete/5
        [Authorize(Roles = "Admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Client client = db.Client.Find(id);
            db.Client.Remove(client);
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