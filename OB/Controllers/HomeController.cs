using OB.Models.DAL;
using OB.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using WebMatrix.WebData;


using System.Data.Entity;


namespace OB.Controllers
{
    public class HomeController : Controller
    {
        [Authorize]
        public RedirectToRouteResult Index()
        {
            if (Roles.GetRolesForUser(WebSecurity.CurrentUserName).Contains("Admin"))
            {
                return RedirectToAction("AdminIndex");
            }
            if (Roles.GetRolesForUser(WebSecurity.CurrentUserName).Contains("HRAdmin"))
            {
                return RedirectToAction("HRAdminIndex");
            }
            if (Roles.GetRolesForUser(WebSecurity.CurrentUserName).Contains("HR"))
            {
                return RedirectToAction("HRIndex");
            }
            if (Roles.GetRolesForUser(WebSecurity.CurrentUserName).Contains("Candidate"))
            {
                return RedirectToAction("CandidateIndex");
            }
            return RedirectToAction("AnonymousIndex");
        }

        public ActionResult AnonymousIndex()
        {
            ViewBag.Message = "欢迎使用E-Onboarding.";

            return View();
        }

        [Authorize(Roles = "Admin")]
        public ActionResult AdminIndex()
        {
            ViewBag.Path1 = "首页";
            ViewBag.Message = "Admin:" + WebSecurity.CurrentUserName + ",欢迎使用E-Onboarding.";

            return View();
        }

        [Authorize(Roles = "HRAdmin")]
        public ActionResult HRAdminIndex()
        {
            ViewBag.Path1 = "首页";
            ViewBag.Message = "HRAdmin:" + WebSecurity.CurrentUserName + ",欢迎使用E-Onboarding.";

            return View();
        }

        [Authorize(Roles = "HR")]
        public ActionResult HRIndex()
        {
            ViewBag.Path1 = "首页";
            ViewBag.Message = "HR:" + WebSecurity.CurrentUserName + ",欢迎使用E-Onboarding.";

            return View();
        }

        [Authorize(Roles = "Candidate")]
        public ActionResult CandidateIndex()
        {
            ViewBag.Path1 = "首页";
            ViewBag.Message = "Candidate:" + WebSecurity.CurrentUserName + ",欢迎使用E-Onboarding.";

            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your app description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        [Authorize(Roles = "Admin")]
        public ActionResult AdminDataSetting()
        {
            ViewBag.Path1 = "参数设置";

            return View();
        }

        [Authorize(Roles = "HRAdmin")]
        public ActionResult HRAdminDataSetting()
        {
            ViewBag.Path1 = "参数设置";

            return View();
        }
        public PartialViewResult AdminMenus(string path1 = "", string path2 = "", string path3 = "")
        {
            ViewBag.Path1 = path1;
            ViewBag.Path2 = path2;
            ViewBag.Path3 = path3;
            var menus = new List<Menu> { 
                new Menu{Name="首页", Controller ="Home", Action="Index"},
                new Menu{Name = "用户", Controller="Account", Action="HRAdminList"},
                new Menu{Name = "参数设置", Controller="Home", Action="AdminDataSetting"},
            };

            return PartialView("Menus", menus);
        }

        public PartialViewResult HRAdminMenus(string path1 = "", string path2 = "", string path3 = "")
        {
            ViewBag.Path1 = path1;
            ViewBag.Path2 = path2;
            ViewBag.Path3 = path3;
            var menus = new List<Menu> { 
                new Menu{Name="首页", Controller ="Home", Action="Index"},
                new Menu{Name = "用户", Controller="Account", Action="HRList"},
                new Menu{Name = "参数设置", Controller="Home", Action="HRAdminDataSetting"},
            };

            return PartialView("Menus", menus);
        }

        public PartialViewResult HRMenus(string path1 = "", string path2 = "", string path3 = "")
        {
            ViewBag.Path1 = path1;
            ViewBag.Path2 = path2;
            ViewBag.Path3 = path3;
            var menus = new List<Menu> { 
                new Menu{Name="首页", Controller ="Home", Action="Index"},
                new Menu{Name = "用户", Controller="Account", Action="CandidateList"},
            };

            return PartialView("Menus", menus);
        }

        public PartialViewResult CandidateMenus(string path1 = "", string path2 = "", string path3 = "")
        {
            ViewBag.Path1 = path1;
            ViewBag.Path2 = path2;
            ViewBag.Path3 = path3;
            var menus = new List<Menu> {                 
                new Menu{Name="个人资料>", Controller ="Employee", Action="EditEmployeeFront"},
                new Menu{Name="教育信息>", Controller ="Employee", Action="EditEmployeeEducation"},
                new Menu{Name="工作经历>", Controller ="Employee", Action="EditEmployeeWork"},
                new Menu{Name="家庭信息>", Controller ="Employee", Action="EditEmployeeFamily"},
                new Menu{Name="上传资料", Controller ="Employee", Action="EditEmployeeDoc"},
            };

            return PartialView("Menus", menus);
        }

        public JsonResult ClientCustomField(int employeeId)
        {
            using (OBContext db = new OBContext())
            {
                var clientId = db.Employee.Where(a => a.Id == employeeId).Select(a => a.ClientId).SingleOrDefault();
                Object ob = (from a in db.CustomField
                             where a.ClientId == clientId
                             select new
                             {
                                 BasicInfo1 = a.BasicInfo1.Trim(),
                                 BasicInfo2 = a.BasicInfo2.Trim(),
                                 BasicInfo3 = a.BasicInfo3.Trim(),
                                 BasicInfo4 = a.BasicInfo4.Trim(),
                                 BasicInfo5 = a.BasicInfo5.Trim(),
                                 BasicInfo6 = a.BasicInfo6.Trim(),
                                 BasicInfo7 = a.BasicInfo7.Trim(),
                                 BasicInfo8 = a.BasicInfo8.Trim(),
                                 BasicInfo9 = a.BasicInfo9.Trim(),
                                 BasicInfo10 = a.BasicInfo10.Trim(),
                                 ////
                                 PensionInfo1 = a.PensionInfo1.Trim(),
                                 PensionInfo2 = a.PensionInfo2.Trim(),
                                 PensionInfo3 = a.PensionInfo3.Trim(),
                                 PensionInfo4 = a.PensionInfo4.Trim(),
                                 PensionInfo5 = a.PensionInfo5.Trim(),
                                 ////
                                 HireInfo1 = a.HireInfo1.Trim(),
                                 HireInfo2 = a.HireInfo2.Trim(),
                                 HireInfo3 = a.HireInfo3.Trim(),
                                 HireInfo4 = a.HireInfo4.Trim(),
                                 HireInfo5 = a.HireInfo5.Trim(),
                                 HireInfo6 = a.HireInfo6.Trim(),
                                 HireInfo7 = a.HireInfo7.Trim(),
                                 HireInfo8 = a.HireInfo8.Trim(),
                                 HireInfo9 = a.HireInfo9.Trim(),
                                 HireInfo10 = a.HireInfo10.Trim(),
                                 HireInfo11 = a.HireInfo11.Trim(),
                                 HireInfo12 = a.HireInfo12.Trim(),
                                 HireInfo13 = a.HireInfo13.Trim(),
                                 HireInfo14 = a.HireInfo14.Trim(),
                                 HireInfo15 = a.HireInfo15.Trim(),
                                 HireInfo16 = a.HireInfo16.Trim(),
                                 HireInfo17 = a.HireInfo17.Trim(),
                                 HireInfo18 = a.HireInfo18.Trim(),
                                 HireInfo19 = a.HireInfo19.Trim(),
                                 HireInfo20 = a.HireInfo20.Trim()
                             }
                                                       ).AsNoTracking().FirstOrDefault();
                return this.Json(ob, JsonRequestBehavior.AllowGet);
            }
        }
    }
}
