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
using OB.Lib;
using WebMatrix.WebData;
using System.Web.Routing;

namespace OB.Controllers
{
    public class WeightController : Controller
    {
        private OBContext db = new OBContext();

        //
        // GET: /Weight/
        [Authorize(Roles = "HRAdmin")]
        public ActionResult Index(int page = 1, string keyword = "", bool includeSoftDeleted = false)
        {
            ViewBag.Path1 = "参数设置";
            ViewBag.RV = new RouteValueDictionary { { "tickTime", DateTime.Now.ToLongTimeString() }, { "returnRoot", "Index" }, { "actionAjax", "GetWeight" }, { "page", page }, { "keyword", keyword }, { "includeSoftDeleted", includeSoftDeleted } };
            return View();
        }

        [Authorize(Roles = "HRAdmin")]
        public PartialViewResult GetWeight(string returnRoot, string actionAjax = "", int page = 1, string keyword = "", bool includeSoftDeleted = false)
        {
            keyword = keyword.ToUpper();
            var results = Common.GetHRAdminWeightQuery(db, WebSecurity.CurrentUserId, includeSoftDeleted, keyword);
            results = results.OrderBy(a => a.WeightClient.Name);
            var rv = new RouteValueDictionary { { "tickTime", DateTime.Now.ToLongTimeString() }, { "returnRoot", returnRoot }, { "actionAjax", actionAjax }, { "page", page }, { "keyword", keyword }, { "includeSoftDeleted", includeSoftDeleted } };
            return PartialView(Common<Weight>.Page(this, rv, results));
        }

        [Authorize(Roles = "HRAdmin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Details(int id = 0, string returnUrl = "Index")
        {
            ViewBag.Path1 = "参数设置";
            //检查记录在权限范围内
            var result = Common.GetHRAdminWeightQuery(db, WebSecurity.CurrentUserId).Where(a => a.Id == id).SingleOrDefault();
            if (result == null)
            {
                Common.RMError(this);
                return Redirect(Url.Content(returnUrl));
            }
            //end

            ViewBag.ReturnUrl = returnUrl;

            return View(result);
        }

        [Authorize(Roles = "HRAdmin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(string returnUrl = "Index")
        {
            ViewBag.Path1 = "参数设置";
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        //
        // POST: /Account/Register

        [Authorize(Roles = "HRAdmin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateSave(Weight model, string returnUrl = "Index")
        {
            ViewBag.Path1 = "参数设置";
            if (ModelState.IsValid)
            {
                try
                {
                    db.Weight.Add(model);
                    db.PPSave();
                    Common.RMOk(this, "记录:'" + model + "'新建成功!");
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

            // 如果我们进行到这一步时某个地方出错，则重新显示表单
            ViewBag.ReturnUrl = returnUrl;
            return View("Create", model);
        }

        [Authorize(Roles = "HRAdmin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id = 0, string returnUrl = "Index")
        {
            ViewBag.Path1 = "参数设置";
            //检查记录在权限范围内
            var result = Common.GetHRAdminWeightQuery(db, WebSecurity.CurrentUserId).Where(a => a.Id == id).SingleOrDefault();
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
        [Authorize(Roles = "HRAdmin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditSave(Weight model, string returnUrl = "Index")
        {
            ViewBag.Path1 = "参数设置";
            //检查记录在权限范围内
            var result = Common.GetHRAdminWeightQuery(db, WebSecurity.CurrentUserId).Where(a => a.Id == model.Id).SingleOrDefault();
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
                    result.Educations = model.Educations;
                    result.Works = model.Works;
                    result.Families = model.Families;
                    result.ChineseName = model.ChineseName;
                    result.EnglishName = model.EnglishName;
                    result.Sex = model.Sex;
                    result.Marriage = model.Marriage;
                    result.Nationality = model.Nationality;
                    result.Nation = model.Nation;
                    result.CertificateId = model.CertificateId;
                    result.CertificateNumber = model.CertificateNumber;
                    result.Birthday = model.Birthday;
                    result.JuzhuAddress = model.JuzhuAddress;
                    result.JuzhudiZipCode = model.JuzhudiZipCode;
                    result.Mobile1 = model.Mobile1;
                    result.Mobile2 = model.Mobile2;
                    result.EmergencyContact = model.EmergencyContact;
                    result.EmergencyContactPhone = model.EmergencyContactPhone;
                    result.PrivateMail = model.PrivateMail;
                    result.HujiAddress = model.HujiAddress;
                    result.HujiZipCode = model.HujiZipCode;
                    result.JuzhuzhengNumber = model.JuzhuzhengNumber;
                    result.JuzhuzhengDueDate = model.JuzhuzhengDueDate;
                    result.SocialGonglingStartDate = model.SocialGonglingStartDate;
                    result.Bank = model.Bank;
                    result.BankAccount = model.BankAccount;
                    result.BankAccountName = model.BankAccountName;
                    result.EmployeeNote = model.EmployeeNote;
                    result.BasicInfo1 = model.BasicInfo1;
                    result.BasicInfo2 = model.BasicInfo2;
                    result.BasicInfo3 = model.BasicInfo3;
                    result.BasicInfo4 = model.BasicInfo4;
                    result.BasicInfo5 = model.BasicInfo5;
                    result.BasicInfo6 = model.BasicInfo6;
                    result.BasicInfo7 = model.BasicInfo7;
                    result.BasicInfo8 = model.BasicInfo8;
                    result.BasicInfo9 = model.BasicInfo9;
                    result.BasicInfo10 = model.BasicInfo10;
                    result.HukouType = model.HukouType;
                    result.PensionCityId = model.PensionCityId;
                    result.AccumulationCityId = model.AccumulationCityId;
                    result.PensionTypeId = model.PensionTypeId;
                    result.AccumulationTypeId = model.AccumulationTypeId;
                    result.PensionStatus = model.PensionStatus;
                    result.YibaokaAvailable = model.YibaokaAvailable;
                    result.AccumulationStatus = model.AccumulationStatus;
                    result.AccumulationNumber = model.AccumulationNumber;
                    result.DanganAddress = model.DanganAddress;
                    result.DanganOrganization = model.DanganOrganization;
                    result.DanganNumber = model.DanganNumber;
                    result.PensionInfo1 = model.PensionInfo1;
                    result.PensionInfo2 = model.PensionInfo2;
                    result.PensionInfo3 = model.PensionInfo3;
                    result.PensionInfo4 = model.PensionInfo4;
                    result.PensionInfo5 = model.PensionInfo5;
                    result.ClientId = model.ClientId;
                    result.WorkClient = model.WorkClient;
                    result.CompanyMail = model.CompanyMail;
                    result.EnterDate = model.EnterDate;
                    result.ProbationDueDate = model.ProbationDueDate;
                    result.EnterClientDate = model.EnterClientDate;
                    result.CompanyYearAdjust = model.CompanyYearAdjust;
                    result.SocialYearAdjust = model.SocialYearAdjust;
                    result.VacationDays = model.VacationDays;
                    result.WorkCityId = model.WorkCityId;
                    result.DepartmentId = model.DepartmentId;
                    result.LevelId = model.LevelId;
                    result.PositionId = model.PositionId;
                    result.ContractNumber = model.ContractNumber;
                    result.ContractBeginDate = model.ContractBeginDate;
                    result.ContractEndDate = model.ContractEndDate;
                    result.ContractTypeId = model.ContractTypeId;
                    result.ProbationSalary = model.ProbationSalary;
                    result.Salary = model.Salary;
                    result.PensionStartMonth = model.PensionStartMonth;
                    result.AccumulationStartMonth = model.AccumulationStartMonth;
                    result.PayByCompany = model.PayByCompany;
                    result.Yljs = model.Yljs;
                    result.Syjs = model.Syjs;
                    result.Yiliaojs = model.Yiliaojs;
                    result.Gsjs = model.Gsjs;
                    result.Shengyujs = model.Shengyujs;
                    result.Qtjs = model.Qtjs;
                    result.Bcjs = model.Bcjs;
                    result.Gjjjs = model.Gjjjs;
                    result.Bcgjjjs = model.Bcgjjjs;
                    result.TaxType = model.TaxType;
                    result.ZhangtaoId = model.ZhangtaoId;
                    result.TaxCityId = model.TaxCityId;
                    result.HireInfo1 = model.HireInfo1;
                    result.HireInfo2 = model.HireInfo2;
                    result.HireInfo3 = model.HireInfo3;
                    result.HireInfo4 = model.HireInfo4;
                    result.HireInfo5 = model.HireInfo5;
                    result.HireInfo6 = model.HireInfo6;
                    result.HireInfo7 = model.HireInfo7;
                    result.HireInfo8 = model.HireInfo8;
                    result.HireInfo9 = model.HireInfo9;
                    result.HireInfo10 = model.HireInfo10;
                    result.HireInfo11 = model.HireInfo11;
                    result.HireInfo12 = model.HireInfo12;
                    result.HireInfo13 = model.HireInfo13;
                    result.HireInfo14 = model.HireInfo14;
                    result.HireInfo15 = model.HireInfo15;
                    result.HireInfo16 = model.HireInfo16;
                    result.HireInfo17 = model.HireInfo17;
                    result.HireInfo18 = model.HireInfo18;
                    result.HireInfo19 = model.HireInfo19;
                    result.HireInfo20 = model.HireInfo20;

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
        public ActionResult Delete(int id = 0, string returnUrl = "Index")
        {
            ViewBag.Path1 = "参数设置";
            //检查记录在权限范围内
            var result = Common.GetHRAdminWeightQuery(db, WebSecurity.CurrentUserId).Where(a => a.Id == id).SingleOrDefault();
            if (result == null)
            {
                Common.RMError(this);
                return Redirect(Url.Content(returnUrl));
            }
            //end

            ViewBag.ReturnUrl = returnUrl;

            return View(result);
        }

        [Authorize(Roles = "HRAdmin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteSave(int id, string returnUrl = "Index")
        {
            ViewBag.Path1 = "参数设置";
            //检查记录在权限范围内
            var result = Common.GetHRAdminWeightQuery(db, WebSecurity.CurrentUserId).Where(a => a.Id == id).SingleOrDefault();
            if (result == null)
            {
                Common.RMError(this);
                return Redirect(Url.Content(returnUrl));
            }
            //end
            var removeName = result.ToString();
            try
            {
                db.Weight.Remove(result);
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

        [Authorize(Roles = "HRAdmin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Restore(int id = 0, string returnUrl = "Index")
        {
            ViewBag.Path1 = "参数设置";
            //检查记录在权限范围内
            var result = Common.GetHRAdminWeightQuery(db, WebSecurity.CurrentUserId, true).Where(a => a.IsDeleted == true).Where(a => a.Id == id).SingleOrDefault();
            if (result == null)
            {
                Common.RMError(this);
                return Redirect(Url.Content(returnUrl));
            }
            //end

            ViewBag.ReturnUrl = returnUrl;

            return View(result);
        }

        [Authorize(Roles = "HRAdmin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RestoreSave(Weight record, string returnUrl = "Index")
        {
            ViewBag.Path1 = "参数设置";
            //检查记录在权限范围内
            var result = Common.GetHRAdminWeightQuery(db, WebSecurity.CurrentUserId, true).Where(a => a.IsDeleted == true).Where(a => a.Id == record.Id).SingleOrDefault();
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
            var result = db.Weight.Find(id);
            return PartialView(result);
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}