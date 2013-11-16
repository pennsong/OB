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

        //
        // GET: /Client/
        [Authorize(Roles = "Admin")]
        public ActionResult Index(int page = 1, string keyword = "", bool includeSoftDeleted = false)
        {
            ViewBag.Path1 = "参数设置";
            ViewBag.Action = "GetClient";
            ViewBag.RV = new { page = page, keyword = keyword, includeSoftDeleted = includeSoftDeleted };
            return View();
        }

        [Authorize(Roles = "Admin")]
        public PartialViewResult GetClient(int page = 1, string keyword = "", bool includeSoftDeleted = false)
        {
            keyword = keyword.ToUpper();
            var results = Common.GetClientQuery(db, includeSoftDeleted, keyword);
            results = results.OrderBy(a => a.Name);
            var rv = new { keyword = keyword, includeSoftDeleted = includeSoftDeleted };
            return PartialView(Common<Client>.Page(this, "GetClient", rv, results, page));
        }

        [Authorize(Roles = "HRAdmin")]
        public ActionResult HRAdminClientIndex(int page = 1, string keyword = "")
        {
            ViewBag.Path1 = "参数设置";
            ViewBag.Action = "GetHRAdminClient";
            ViewBag.RV = new { page = page, keyword = keyword };
            return View();
        }

        [Authorize(Roles = "HRAdmin")]
        public PartialViewResult GetHRAdminClient(int page = 1, string keyword = "")
        {
            keyword = keyword.ToUpper();
            var results = Common.GetHRAdminClientQuery(db, WebSecurity.CurrentUserId, keyword);
            results = results.OrderBy(a => a.Name);
            var rv = new { keyword = keyword };
            return PartialView(Common<Client>.Page(this, "GetHRAdminClient", rv, results, page));
        }

        //
        // GET: /Client/Details/5
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Details(int id = 0, string returnUrl = "Index")
        {
            ViewBag.Path1 = "参数设置";
            //检查记录在权限范围内
            var result = Common.GetClientQuery(db).Where(a => a.Id == id).SingleOrDefault();
            if (result == null)
            {
                Common.Rd(this, MsgType.ERROR, "没有找到对应记录!");
                return Redirect(Url.Content(returnUrl));
            }
            //end

            ViewBag.ReturnUrl = returnUrl;

            return View(result);
        }

        //
        // GET: /Client/Create
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(string returnUrl = "Index")
        {
            ViewBag.Path1 = "参数设置";
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        //
        // POST: /Client/Create
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateSave(Client record, string returnUrl = "Index")
        {
            ViewBag.Path1 = "参数设置";
            //检查记录在权限范围内
            //end
            if (ModelState.IsValid)
            {
                try
                {
                    db.Client.Add(record);
                    db.PPSave();
                    Common.Rd(this, MsgType.OK, "记录:" + record.ToString() + "新建成功!");
                    return Redirect(Url.Content(returnUrl));
                }
                catch (DbUpdateException e)
                {
                    if (e.InnerException.InnerException.Message.Contains("Cannot insert duplicate key row"))
                    {
                        ModelState.AddModelError(string.Empty, "相同名称的记录已存在,保存失败!");
                    }
                }
            }
            ViewBag.ReturnUrl = returnUrl;
            return View("Create", record);
        }

        //
        // GET: /Client/Edit/5
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id = 0, string returnUrl = "Index")
        {
            ViewBag.Path1 = "参数设置";
            //检查记录在权限范围内
            var result = Common.GetClientQuery(db).Where(a => a.Id == id).SingleOrDefault();
            if (result == null)
            {
                Common.Rd(this, MsgType.ERROR);
                return Redirect(Url.Content(returnUrl));
            }
            //end

            ViewBag.ReturnUrl = returnUrl;

            return View(result);
        }

        //
        // POST: /Client/Edit/5
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditSave(Client record, string returnUrl = "Index")
        {
            ViewBag.Path1 = "参数设置";
            //检查记录在权限范围内
            var result = Common.GetClientQuery(db).Where(a => a.Id == record.Id).SingleOrDefault();
            if (result == null)
            {
                Common.Rd(this, MsgType.ERROR);
                return Redirect(Url.Content(returnUrl));
            }
            //end

            if (ModelState.IsValid)
            {
                try
                {
                    result.Name = record.Name;
                    result.HRAdminId = record.HRAdminId;
                    db.PPSave();
                    Common.Rd(this, MsgType.OK, "记录:" + record.ToString() + "保存成功!");
                    return Redirect(Url.Content(returnUrl));
                }
                catch (DbUpdateException e)
                {
                    if (e.InnerException.InnerException.Message.Contains("Cannot insert duplicate key row"))
                    {
                        ModelState.AddModelError(string.Empty, "相同名称的记录已存在,保存失败!");
                    }
                }
            }
            ViewBag.ReturnUrl = returnUrl;

            return View("Edit", record);
        }

        [Authorize(Roles = "HRAdmin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditClientHR(int id = 0, string returnUrl = "HRAdminClientIndex")
        {
            ViewBag.Path1 = "参数设置";
            //检查记录在权限范围内
            var results = Common.GetHRAdminClientQuery(db, WebSecurity.CurrentUserId);
            var result = results.Where(a => a.Id == id).SingleOrDefault();
            if (result == null)
            {
                Common.Rd(this, MsgType.ERROR);
                return Redirect(Url.Content(returnUrl));
            }
            //end

            var record = new EditClientHR
            {
                ClientId = result.Id,
                ClientName = result.Name,
                HRIds = result.HRs.Select(a => a.Id).ToList()
            };
            ViewBag.ReturnUrl = returnUrl;
            return View(record);
        }

        [Authorize(Roles = "HRAdmin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditClientHRSave(EditClientHR record, string returnUrl = "HRAdminClientIndex")
        {
            ViewBag.Path1 = "参数设置";
            //检查记录在权限范围内
            var results = Common.GetHRAdminClientQuery(db, WebSecurity.CurrentUserId);
            var result = results.Include(a => a.HRs).Where(a => a.Id == record.ClientId).SingleOrDefault();
            if (result == null)
            {
                Common.Rd(this, MsgType.ERROR);
                return Redirect(Url.Content(returnUrl));
            }
            //end

            if (ModelState.IsValid)
            {
                try
                {
                    if (record.HRIds == null)
                    {
                        record.HRIds = new List<int> { };
                    }
                    var hrs = Common.GetUserQuery("HR", db).Where(a => record.HRIds.Any(b => b == a.Id)).ToList();
                    result.HRs = hrs;
                    db.PPSave();
                    Common.Rd(this, MsgType.OK, "记录:" + record.ToString() + "保存成功!");
                    return Redirect(Url.Content(returnUrl));
                }
                catch (Exception e)
                {
                    ModelState.AddModelError(string.Empty, e.Message);
                }
            }
            ViewBag.ReturnUrl = returnUrl;
            return View("EditClientHR", record);
        }
        //
        // GET: /Client/Delete/5
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id = 0, string returnUrl = "Index")
        {
            ViewBag.Path1 = "参数设置";
            //检查记录在权限范围内
            var result = Common.GetClientQuery(db).Where(a => a.Id == id).SingleOrDefault();
            if (result == null)
            {
                Common.Rd(this, MsgType.ERROR);
                return Redirect(Url.Content(returnUrl));
            }
            //end

            ViewBag.ReturnUrl = returnUrl;

            return View(result);
        }

        //
        // POST: /Client/Delete/5
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteSave(int id, string returnUrl = "Index")
        {
            ViewBag.Path1 = "参数设置";
            //检查记录在权限范围内
            var result = Common.GetClientQuery(db).Where(a => a.Id == id).SingleOrDefault();
            if (result == null)
            {
                Common.Rd(this, MsgType.ERROR);
                return Redirect(Url.Content(returnUrl));
            }
            //end

            try
            {
                db.Client.Remove(result);
                db.PPSave();
                Common.Rd(this, MsgType.OK, "客户信息:" + result.ToString() + "删除成功!");
                return Redirect(Url.Content(returnUrl));
            }
            catch (Exception e)
            {
                if (e.InnerException.InnerException.Message.Contains("The DELETE statement conflicted with the REFERENCE constraint"))
                {
                    Common.Rd(this, MsgType.ERROR, "记录" + result.ToString() + "被其他记录引用, 不能删除!");
                }
                else
                {
                    Common.Rd(this, MsgType.ERROR, "记录" + result.ToString() + "删除失败!");
                }
            }
            return Redirect(Url.Content(returnUrl));
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Restore(int id = 0, string returnUrl = "Index")
        {
            ViewBag.Path1 = "参数设置";
            //检查记录在权限范围内
            var result = Common.GetClientQuery(db, true).Where(a => a.IsDeleted == true).Where(a => a.Id == id).SingleOrDefault();
            if (result == null)
            {
                Common.Rd(this, MsgType.ERROR);
                return Redirect(Url.Content(returnUrl));
            }
            //end

            ViewBag.ReturnUrl = returnUrl;

            return View(result);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RestoreSave(Client record, string returnUrl = "Index")
        {
            ViewBag.Path1 = "参数设置";
            //检查记录在权限范围内
            var result = Common.GetClientQuery(db, true).Where(a => a.IsDeleted == true).Where(a => a.Id == record.Id).SingleOrDefault();
            if (result == null)
            {
                Common.Rd(this, MsgType.ERROR);
                return Redirect(Url.Content(returnUrl));
            }
            //end

            try
            {
                result.IsDeleted = false;
                db.PPSave();
                Common.Rd(this, MsgType.OK, "客户信息:" + result.ToString() + "恢复成功!");
                return Redirect(Url.Content(returnUrl));
            }
            catch (Exception e)
            {
                Common.Rd(this, MsgType.ERROR, "记录" + result.ToString() + "恢复失败!");
            }
            return Redirect(Url.Content(returnUrl));
        }

        [Authorize(Roles = "Admin")]
        public PartialViewResult History(int id)
        {
            //检查记录在权限范围内
            var result = Common.GetClientQuery(db, true).Where(a => a.Id == id).SingleOrDefault();
            if (result == null)
            {
                return PartialView();
            }
            //end
            return PartialView(db.HistoryExplorer.ChangesTo(result));
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}