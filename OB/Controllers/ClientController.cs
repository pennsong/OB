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
        public ActionResult Index(int page = 1, string keyword = "")
        {
            ViewBag.Path1 = "参数设置";
            ViewBag.Action = "GetClient";
            ViewBag.RV = new { page = page, keyword = keyword };
            return View();
        }

        [Authorize(Roles = "Admin")]
        public PartialViewResult GetClient(int page = 1, string keyword = "")
        {
            keyword = keyword.ToUpper();
            var results = Common.GetClient(db, keyword);
            results = results.OrderBy(a => a.Name);
            var rv = new { keyword = keyword };
            return PartialView(Common<Client>.Page(this, "GetClient", rv, results, page));
        }

        [Authorize(Roles = "HRAdmin")]
        public ActionResult HRAdminClientIndex()
        {
            ViewBag.Path1 = "参数设置";
            ViewBag.Action = "GetHRAdminClient";
            return View();
        }

        [Authorize(Roles = "HRAdmin")]
        public PartialViewResult GetHRAdminClient(int page = 1, string keyword = "")
        {
            keyword = keyword.ToUpper();
            var results = Common.GetHRAdminClient(WebSecurity.CurrentUserId, db, keyword);
            results = results.OrderBy(a => a.Name);
            var rv = new { keyword = keyword };
            return PartialView(Common<Client>.Page(this, "GetHRAdminClient", rv, results, page));
        }

        //
        // GET: /Client/Details/5
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult Details(int id = 0, string returnUrl = "Index")
        {
            Client client = db.Client.Find(id);
            if (client == null)
            {
                return HttpNotFound();
            }

            ViewBag.ReturnUrl = returnUrl;

            return View(client);
        }

        //
        // GET: /Client/Create
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(string returnUrl = "Index")
        {
            ViewBag.HRAdminId = new SelectList(Common.UserQuery("HRAdmin", db), "Id", "Name");
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        //
        // POST: /Client/Create
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateSave(Client client, string returnUrl = "Index")
        {
            if (ModelState.IsValid)
            {
                try
                {
                    db.Client.Add(client);
                    db.SaveChanges();
                    Msg msg = new Msg { MsgType = MsgType.OK, Content = "客户信息:" + client.ToString() + "新建成功!" };
                    TempData["msg"] = msg;
                    return Redirect(Url.Content(returnUrl));
                }
                catch (DbUpdateException ex)
                {
                    if (ex.InnerException.InnerException.Message.Contains("Cannot insert duplicate key row"))
                    {
                        ModelState.AddModelError(string.Empty, "相同名称的记录已存在,保存失败!");
                    }
                }
            }
            ViewBag.HRAdminId = new SelectList(Common.UserQuery("HRAdmin", db), "Id", "Name");
            ViewBag.ReturnUrl = returnUrl;
            return View("Create", client);
        }

        //
        // GET: /Client/Edit/5
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id = 0, string returnUrl = "Index")
        {
            Client client = db.Client.Find(id);
            if (client == null)
            {
                return HttpNotFound();
            }

            ViewBag.ReturnUrl = returnUrl;
            ViewBag.HRAdminList = Common.UserQuery("HRAdmin", db);

            return View(client);
        }

        //
        // POST: /Client/Edit/5
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditSave(Client client, string returnUrl = "Index")
        {
            if (ModelState.IsValid)
            {
                try
                {
                    db.Entry(client).State = EntityState.Modified;
                    db.SaveChanges();
                    Msg msg = new Msg { MsgType = MsgType.OK, Content = "客户信息:" + client.ToString() + "保存成功!" };
                    TempData["msg"] = msg;
                    return Redirect(Url.Content(returnUrl));
                }
                catch (DbUpdateException ex)
                {
                    if (ex.InnerException.InnerException.Message.Contains("Cannot insert duplicate key row"))
                    {
                        ModelState.AddModelError(string.Empty, "相同名称的记录已存在,保存失败!");
                    }
                }
            }
            ViewBag.HRAdminId = new SelectList(Common.UserQuery("HRAdmin", db), "Id", "Name");
            ViewBag.ReturnUrl = returnUrl;

            return View("Edit", client);
        }

        [Authorize(Roles = "HRAdmin")]
        public ActionResult EditClientHR(int id = 0)
        {
            var results = Common.GetHRAdminClient(WebSecurity.CurrentUserId, db);
            var result = results.Where(a => a.Id == id).SingleOrDefault();
            if (result == null)
            {
                return HttpNotFound();
            }
            var editClientHr = new EditClientHR
            {
                ClientId = result.Id,
                ClientName = result.Name,
                HRIds = result.HRs.Select(a => a.Id).ToList()
            };
            return View(editClientHr);
        }

        [Authorize(Roles = "HRAdmin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditClientHR(EditClientHR editClientHR)
        {
            var results = Common.GetHRAdminClient(WebSecurity.CurrentUserId, db);
            var result = results.Include(a => a.HRs).Where(a => a.Id == editClientHR.ClientId).SingleOrDefault();
            if (result == null)
            {
                return HttpNotFound();
            }
            if (ModelState.IsValid)
            {
                try
                {
                    if (editClientHR.HRIds == null)
                    {
                        editClientHR.HRIds = new List<int> { };
                    }
                    var hrs = Common.UserQuery("HR", db).Where(a => editClientHR.HRIds.Any(b => b == a.Id)).ToList();
                    result.HRs = hrs;
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
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id = 0, string returnUrl = "Index")
        {
            Client client = db.Client.Find(id);
            if (client == null)
            {
                return HttpNotFound();
            }

            ViewBag.ReturnUrl = returnUrl;
            return View(client);
        }

        //
        // POST: /Client/Delete/5
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id, string returnUrl = "Index")
        {
            Client client = db.Client.Find(id);
            db.Client.Remove(client);
            db.SaveChanges();
            Msg msg = new Msg { MsgType = MsgType.OK, Content = "客户信息:" + client.ToString() + "删除成功!" };
            TempData["msg"] = msg;
            return Redirect(Url.Content(returnUrl));
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}