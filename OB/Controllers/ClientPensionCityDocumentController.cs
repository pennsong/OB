using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OB.Models;
using OB.Models.DAL;
using WebMatrix.WebData;
using System.Data.Entity.Infrastructure;
using OB.Models.ViewModel;

namespace OB.Controllers
{
    public class ClientPensionCityDocumentController : Controller
    {
        private OBContext db = new OBContext();

        //
        // GET: /ClientPensionCityDocument/
        [Authorize(Roles = "HRAdmin")]
        public ActionResult Index()
        {
            var clients = db.User.Where(a => a.Id == WebSecurity.CurrentUserId).Single().HRAdminClients.Select(b => b.Id);
            var clientpensioncitydocument = db.ClientPensionCityDocument.Include(c => c.Client).Include(c => c.PensionCity).Where(c => clients.Contains(c.ClientId));
            return View(clientpensioncitydocument.ToList());
        }

        //
        // GET: /ClientPensionCityDocument/Details/5

        public ActionResult Details(int id = 0)
        {
            ClientPensionCityDocument clientpensioncitydocument = db.ClientPensionCityDocument.Find(id);
            if (clientpensioncitydocument == null)
            {
                return HttpNotFound();
            }
            return View(clientpensioncitydocument);
        }

        //
        // GET: /ClientPensionCityDocument/Create
        [Authorize(Roles = "HRAdmin")]
        public ActionResult Create()
        {
            var clients = db.User.Where(a => a.Id == WebSecurity.CurrentUserId).Single().HRAdminClients;

            ViewBag.ClientId = new SelectList(clients, "Id", "Name");
            ViewBag.PensionCityId = new SelectList(db.City, "Id", "Name");
            return View();
        }

        //
        // POST: /ClientPensionCityDocument/Create
        [Authorize(Roles = "HRAdmin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ClientPensionCityDocument clientpensioncitydocument)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    db.ClientPensionCityDocument.Add(clientpensioncitydocument);
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

            var clients = db.User.Where(a => a.Id == WebSecurity.CurrentUserId).Single().HRAdminClients;
            ViewBag.ClientId = new SelectList(clients, "Id", "Name");
            ViewBag.PensionCityId = new SelectList(db.City, "Id", "Name");
            //ViewBag.ClientId = new SelectList(db.Client, "Id", "Name", clientpensioncitydocument.ClientId);
            //ViewBag.PensionCityId = new SelectList(db.City, "Id", "Name", clientpensioncitydocument.PensionCityId);
            return View(clientpensioncitydocument);
        }

        //
        // GET: /ClientPensionCityDocument/Edit/5
        [Authorize(Roles = "HRAdmin")]
        public ActionResult Edit(int id = 0)
        {
            var clients = db.User.Where(a => a.Id == WebSecurity.CurrentUserId).Single().HRAdminClients.Select(b => b.Id);

            ClientPensionCityDocument clientpensioncitydocument = db.ClientPensionCityDocument.Include(a => a.Documents).Where(a => a.Id == id && clients.Contains(a.ClientId)).SingleOrDefault();
            if (clientpensioncitydocument == null)
            {
                return HttpNotFound();
            }
            var editClientPensionCityDocument = new EditClientPensionCityDocument
            {
                ClientPensionCityDocumentId = clientpensioncitydocument.Id,
                ClientName = clientpensioncitydocument.Client.Name,
                PensionCityName = clientpensioncitydocument.PensionCityId == null ? "无" : clientpensioncitydocument.PensionCity.Name,
                DocumentIds = clientpensioncitydocument.Documents.Select(a => a.Id).ToList()
            };
            return View(editClientPensionCityDocument);
        }

        //
        // POST: /ClientPensionCityDocument/Edit/5
        [Authorize(Roles = "HRAdmin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(EditClientPensionCityDocument editClientPensionCityDocument)
        {
            var clients = db.User.Where(a => a.Id == WebSecurity.CurrentUserId).Single().HRAdminClients.Select(b => b.Id);

            ClientPensionCityDocument clientpensioncitydocument = db.ClientPensionCityDocument.Include(a => a.Documents).Where(a => a.Id == editClientPensionCityDocument.ClientPensionCityDocumentId && clients.Contains(a.ClientId)).SingleOrDefault();
            if (clientpensioncitydocument == null)
            {
                return HttpNotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var t = db.Document.Where(a => editClientPensionCityDocument.DocumentIds.Any(b => b == a.Id)).ToList();
                    clientpensioncitydocument.Documents = t;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                }
            }
            return View(editClientPensionCityDocument);
        }

        //
        // GET: /ClientPensionCityDocument/Delete/5

        public ActionResult Delete(int id = 0)
        {
            ClientPensionCityDocument clientpensioncitydocument = db.ClientPensionCityDocument.Find(id);
            if (clientpensioncitydocument == null)
            {
                return HttpNotFound();
            }
            return View(clientpensioncitydocument);
        }

        //
        // POST: /ClientPensionCityDocument/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ClientPensionCityDocument clientpensioncitydocument = db.ClientPensionCityDocument.Find(id);
            db.ClientPensionCityDocument.Remove(clientpensioncitydocument);
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