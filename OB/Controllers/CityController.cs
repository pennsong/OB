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
    public class CityController : Controller
    {
        private OBContext db = new OBContext();

        //
        // GET: /City/
        [Authorize(Roles = "Admin")]
        public ActionResult Index(int page = 1, string keyword = "", bool includeSoftDeleted = false)
        {
            ViewBag.Path1 = "参数设置";
            ViewBag.RV = new RouteValueDictionary { { "tickTime", DateTime.Now.ToLongTimeString() }, { "returnRoot", "Index" }, { "actionAjax", "GetCity" }, { "page", page }, { "keyword", keyword }, { "includeSoftDeleted", includeSoftDeleted } };
            return View();
        }

        [Authorize(Roles = "Admin")]
        public PartialViewResult GetCity(string returnRoot, string actionAjax = "", int page = 1, string keyword = "", bool includeSoftDeleted = false)
        {
            keyword = keyword.ToUpper();
            var results = Common.GetCityQuery(db, includeSoftDeleted, keyword);
            results = results.OrderBy(a => a.Name);
            var rv = new RouteValueDictionary { { "tickTime", DateTime.Now.ToLongTimeString() }, { "returnRoot", returnRoot }, { "actionAjax", actionAjax }, { "page", page }, { "keyword", keyword }, { "includeSoftDeleted", includeSoftDeleted } };
            return PartialView(Common<City>.Page(this, rv, results));
        }

        //
        // GET: /City/Details/5
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Details(int id = 0, string returnUrl = "Index")
        {
            ViewBag.Path1 = "参数设置";
            //检查记录在权限范围内
            var result = Common.GetCityQuery(db).Where(a => a.Id == id).SingleOrDefault();
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
        // GET: /City/Create
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
        // POST: /City/Create
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateSave(City model, string returnUrl = "Index")
        {
            ViewBag.Path1 = "参数设置";
            //检查记录在权限范围内
            //end
            if (ModelState.IsValid)
            {
                try
                {
                    db.City.Add(model);
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
        // GET: /City/Edit/5
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id = 0, string returnUrl = "Index")
        {
            ViewBag.Path1 = "参数设置";
            //检查记录在权限范围内
            var result = Common.GetCityQuery(db).Where(a => a.Id == id).SingleOrDefault();
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
        // POST: /City/Edit/5
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditSave(City model, string returnUrl = "Index")
        {
            ViewBag.Path1 = "参数设置";
            //检查记录在权限范围内
            var result = Common.GetCityQuery(db).Where(a => a.Id == model.Id).SingleOrDefault();
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

        //
        // GET: /City/Delete/5
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id = 0, string returnUrl = "Index")
        {
            ViewBag.Path1 = "参数设置";
            //检查记录在权限范围内
            var result = Common.GetCityQuery(db).Where(a => a.Id == id).SingleOrDefault();
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
        // POST: /City/Delete/5
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteSave(int id, string returnUrl = "Index")
        {
            ViewBag.Path1 = "参数设置";
            //检查记录在权限范围内
            var result = Common.GetCityQuery(db).Where(a => a.Id == id).SingleOrDefault();
            if (result == null)
            {
                Common.RMError(this);
                return Redirect(Url.Content(returnUrl));
            }
            //end
            var removeName = result.ToString();
            try
            {
                db.City.Remove(result);
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
            var result = Common.GetCityQuery(db, true).Where(a => a.IsDeleted == true).Where(a => a.Id == id).SingleOrDefault();
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
        public ActionResult RestoreSave(City record, string returnUrl = "Index")
        {
            ViewBag.Path1 = "参数设置";
            //检查记录在权限范围内
            var result = Common.GetCityQuery(db, true).Where(a => a.IsDeleted == true).Where(a => a.Id == record.Id).SingleOrDefault();
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

        [ChildActionOnly]
        public PartialViewResult Abstract(int id)
        {
            var result = db.City.Find(id);
            return PartialView(result);
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}