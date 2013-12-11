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
using System.Web.Routing;

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
            ViewBag.RV = new RouteValueDictionary { { "tickTime", DateTime.Now.ToLongTimeString() }, { "returnRoot", "Index" }, { "actionAjax", "GetClient" }, { "page", page }, { "keyword", keyword }, { "includeSoftDeleted", includeSoftDeleted } };
            return View();
        }

        [Authorize(Roles = "Admin")]
        public PartialViewResult GetClient(string returnRoot, string actionAjax = "", int page = 1, string keyword = "", bool includeSoftDeleted = false)
        {
            keyword = keyword.ToUpper();
            var results = Common.GetClientQuery(db, includeSoftDeleted, keyword);
            results = results.OrderBy(a => a.Name);
            var rv = new RouteValueDictionary { { "tickTime", DateTime.Now.ToLongTimeString() }, { "returnRoot", returnRoot }, { "actionAjax", actionAjax }, { "page", page }, { "keyword", keyword }, { "includeSoftDeleted", includeSoftDeleted } };
            return PartialView(Common<Client>.Page(this, rv, results));
        }

        [Authorize(Roles = "HRAdmin")]
        public ActionResult HRAdminClientIndex(int page = 1, string keyword = "")
        {
            ViewBag.Path1 = "参数设置";
            ViewBag.RV = new RouteValueDictionary { { "tickTime", DateTime.Now.ToLongTimeString() }, { "returnRoot", "HRAdminClientIndex" }, { "actionAjax", "GetHRAdminClient" }, { "page", page }, { "keyword", keyword } };
            return View();
        }

        [Authorize(Roles = "HRAdmin")]
        public PartialViewResult GetHRAdminClient(string returnRoot, string actionAjax = "", int page = 1, string keyword = "")
        {
            keyword = keyword.ToUpper();
            var results = Common.GetHRAdminClientQuery(db, WebSecurity.CurrentUserId, keyword);
            results = results.OrderBy(a => a.Name);
            var rv = new RouteValueDictionary { { "tickTime", DateTime.Now.ToLongTimeString() }, { "returnRoot", returnRoot }, { "actionAjax", actionAjax }, { "page", page }, { "keyword", keyword } };
            return PartialView(Common<Client>.Page(this, rv, results));
        }

        //
        // GET: /Client/Details/5
        [Authorize(Roles = "Admin, HRAdmin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Details(int id = 0, string returnUrl = "Index")
        {
            ViewBag.Path1 = "参数设置";
            //检查记录在权限范围内
            var result = Common.GetClientQuery(db).Where(a => a.Id == id).SingleOrDefault();
            if (result == null)
            {
                Common.RMError(this);
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
        public ActionResult CreateSave(Client model, string returnUrl = "Index")
        {
            ViewBag.Path1 = "参数设置";
            //检查记录在权限范围内
            //end
            if (ModelState.IsValid)
            {
                try
                {
                    db.Client.Add(model);
                    db.PPSave();
                    Common.RMOk(this, "记录:" + model + "新建成功!");
                    return Redirect(Url.Content(returnUrl));
                }
                catch (Exception e)
                {
                    if (e.InnerException.Message.Contains("Cannot insert duplicate key row"))
                    {
                        ModelState.AddModelError(string.Empty, "相同名称的记录已存在,保存失败!");
                    }
                }
            }
            ViewBag.ReturnUrl = returnUrl;
            return View("Create", model);
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
                Common.RMError(this);
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
        public ActionResult EditSave(Client model, string returnUrl = "Index")
        {
            ViewBag.Path1 = "参数设置";
            //检查记录在权限范围内
            var result = Common.GetClientQuery(db).Where(a => a.Id == model.Id).SingleOrDefault();
            if (result == null)
            {
                Common.RMError(this);
                return Redirect(Url.Content(returnUrl));
            }
            //end

            if (ModelState.IsValid)
            {
                try
                {
                    result.Name = model.Name;
                    result.HRAdminId = model.HRAdminId;
                    db.PPSave();
                    Common.RMOk(this, "记录:" + model + "保存成功!");
                    return Redirect(Url.Content(returnUrl));
                }
                catch (Exception e)
                {
                    if (e.InnerException.Message.Contains("Cannot insert duplicate key row"))
                    {
                        ModelState.AddModelError(string.Empty, "相同名称的记录已存在,保存失败!");
                    }
                }
            }
            ViewBag.ReturnUrl = returnUrl;

            return View("Edit", model);
        }

        [Authorize(Roles = "HRAdmin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult HRAdminEditClient(int id = 0, string returnUrl = "HRAdminClientIndex")
        {
            ViewBag.Path1 = "参数设置";
            //检查记录在权限范围内
            var results = Common.GetHRAdminClientQuery(db, WebSecurity.CurrentUserId);
            var result = results.Where(a => a.Id == id).SingleOrDefault();
            if (result == null)
            {
                Common.RMError(this);
                return Redirect(Url.Content(returnUrl));
            }
            //end

            var record = new HRAdminEditClient
            {
                ClientId = result.Id,
                ClientName = result.Name,
                EducationNote = result.EducationNote,
                FamilyNote = result.FamilyNote,
                PersonInfoNote = result.PersonInfoNote,
                WorkNote = result.WorkNote,

                HRIds = result.HRs.Select(a => a.Id).ToList(),
                PensionCities = result.PensionCities.Select(a => a.Id).ToList(),
                TaxCities = result.TaxCities.Select(a => a.Id).ToList(),
            };
            ViewBag.ReturnUrl = returnUrl;
            return View(record);
        }

        [Authorize(Roles = "HRAdmin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult HRAdminEditClientSave(HRAdminEditClient model, string returnUrl = "HRAdminClientIndex")
        {
            ViewBag.Path1 = "参数设置";
            //检查记录在权限范围内
            var results = Common.GetHRAdminClientQuery(db, WebSecurity.CurrentUserId);
            var result = results.Include(a => a.HRs).Include(a => a.PensionCities).Include(a => a.TaxCities).Where(a => a.Id == model.ClientId).SingleOrDefault();
            if (result == null)
            {
                Common.RMError(this);
                return Redirect(Url.Content(returnUrl));
            }
            //end

            if (ModelState.IsValid)
            {
                try
                {
                    var hrs = Common.GetHRQuery(db).Where(a => model.HRIds.Any(b => b == a.Id)).ToList();
                    var taxCities = Common.GetCityQuery(db).Where(a => model.TaxCities.Any(b => b == a.Id)).ToList();
                    var pensionCities = Common.GetCityQuery(db).Where(a => model.PensionCities.Any(b => b == a.Id)).ToList();
                    result.HRs = hrs;
                    result.PensionCities = pensionCities;
                    result.TaxCities = taxCities;
                    result.PersonInfoNote = model.PersonInfoNote;
                    result.EducationNote = model.EducationNote;
                    result.WorkNote = model.WorkNote;
                    result.FamilyNote = model.FamilyNote;
                    db.PPSave();
                    Common.RMOk(this, "记录:" + result + "保存成功!");
                    return Redirect(Url.Content(returnUrl));
                }
                catch (Exception e)
                {
                    ModelState.AddModelError(string.Empty, e.Message);
                }
            }
            ViewBag.ReturnUrl = returnUrl;
            return View("HRAdminEditClient", model);
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
                Common.RMError(this);
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
                Common.RMError(this);
                return Redirect(Url.Content(returnUrl));
            }
            //end
            var removeName = result.ToString();
            try
            {
                db.Client.Remove(result);
                db.PPSave();
                Common.RMOk(this, "记录:" + removeName + "删除成功!");
                return Redirect(Url.Content(returnUrl));
            }
            catch (Exception e)
            {
                if (e.InnerException.InnerException.Message.Contains("The DELETE statement conflicted with the REFERENCE constraint"))
                {
                    Common.RMError(this, "记录" + removeName + "被其他记录引用, 不能删除!");
                }
                else
                {
                    Common.RMError(this, "记录" + removeName + "删除失败!");
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
                Common.RMError(this);
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
                Common.RMError(this);
                return Redirect(Url.Content(returnUrl));
            }
            //end

            try
            {
                result.IsDeleted = false;
                db.PPSave();
                Common.RMOk(this, "记录:" + result + "恢复成功!");
                return Redirect(Url.Content(returnUrl));
            }
            catch (Exception e)
            {
                Common.RMOk(this, "记录" + result + "恢复失败!" + e.ToString());
            }
            return Redirect(Url.Content(returnUrl));
        }

        [Authorize(Roles = "Admin")]
        public PartialViewResult HistoryHR(int id)
        {
            //检查记录在权限范围内
            var result = Common.GetClientQuery(db, true).Where(a => a.Id == id).SingleOrDefault();
            if (result == null)
            {
                return PartialView();
            }
            //end
            var changes = db.HistoryExplorer.ChangesTo(result, a => a.HRs).ToList();
            return PartialView(changes);
        }

        [Authorize(Roles = "Admin")]
        public PartialViewResult HistoryTaxCity(int id)
        {
            //检查记录在权限范围内
            var result = Common.GetClientQuery(db, true).Where(a => a.Id == id).SingleOrDefault();
            if (result == null)
            {
                return PartialView();
            }
            //end
            var changes = db.HistoryExplorer.ChangesTo(result, a => a.TaxCities).ToList();
            return PartialView(changes);
        }

        [ChildActionOnly]
        public PartialViewResult Abstract(int id)
        {
            var result = db.Client.Find(id);
            return PartialView(result);
        }

        [ChildActionOnly]
        public PartialViewResult AbstractAll(int id)
        {
            var result = db.Client.Find(id);
            return PartialView(result);
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}