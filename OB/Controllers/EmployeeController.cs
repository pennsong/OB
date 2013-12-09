using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OB.Models;
using OB.Models.DAL;
using AutoMapper;
using WebMatrix.WebData;
using OB.Models.ViewModel;
using System.IO;
using System.Reflection;
using System.Web.Helpers;

namespace OB.Controllers
{
    public class EmployeeController : Controller
    {
        private OBContext db = new OBContext();

        //
        // GET: /Employee/

        public ActionResult Index(int page = 1)
        {
            return View(db.Employee.OrderBy(a => a.Id).ToList());
        }

        //
        // GET: /Employee/Details/5

        public ActionResult Details(int id = 0)
        {
            Employee employee = db.Employee.Find(id);
            if (employee == null)
            {
                return HttpNotFound();
            }
            return View(employee);
        }

        //
        // GET: /Employee/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Employee/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Employee employee)
        {
            if (ModelState.IsValid)
            {
                db.Employee.Add(employee);
                db.PPSave();
                return RedirectToAction("Index");
            }

            return View(employee);
        }

        //
        // GET: /Employee/Edit/5

        public ActionResult Edit(int id = 0)
        {
            Employee employee = db.Employee.Find(id);
            if (employee == null)
            {
                return HttpNotFound();
            }
            return View(employee);
        }

        [Authorize(Roles = "Candidate")]
        public ActionResult FrontDetail()
        {
            ViewBag.Path1 = "信息详情";
            Employee employee = db.Employee.Where(a => a.UserId == WebSecurity.CurrentUserId).SingleOrDefault();
            if (employee == null)
            {
                return HttpNotFound();
            }

            var docList = db.ClientPensionCityDocument.Include(a => a.Documents).Where(a => a.ClientId == employee.ClientId && ((employee.PensionCityId == null && a.PensionCityId == null) || employee.PensionCityId == a.PensionCityId)).SingleOrDefault();
            if (docList == null)
            {
                throw new Exception("对应资料列表未配置, 请联系客服人员!");
            }
            ViewBag.DocList = docList;

            return View(employee);
        }

        [Authorize(Roles = "HR")]
        public ActionResult BackDetail(int id)
        {
            ViewBag.Path1 = "员工管理";
            var clientList = db.User.Where(a => a.Id == WebSecurity.CurrentUserId).Single().HRClients.Select(a => a.Id).ToList();
            Employee employee = db.Employee.Where(a => a.Id == id && clientList.Contains(a.ClientId)).SingleOrDefault();
            if (employee == null)
            {
                return HttpNotFound();
            }

            return View(employee);
        }

        //
        // POST: /Employee/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Employee employee)
        {
            if (ModelState.IsValid)
            {
                db.Entry(employee).State = EntityState.Modified;
                db.PPSave();
                return RedirectToAction("Index");
            }
            return View(employee);
        }

        [Authorize(Roles = "HR")]
        public ActionResult SendOffer(int id)//employeeId
        {
            ViewBag.Path1 = "员工管理";
            var clientList = db.User.Where(a => a.Id == WebSecurity.CurrentUserId).Single().HRClients.Select(a => a.Id).ToList();
            Employee employee = db.Employee.Where(a => a.Id == id && clientList.Contains(a.ClientId)).SingleOrDefault();
            if (employee == null)
            {
                return HttpNotFound();
            }
            Offer offer = new Offer { EmployeeId = id, Content = "test" };
            return View(offer);
        }

        [Authorize(Roles = "HR")]
        [HttpPost]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult SendOffer(Offer offer)
        {
            ViewBag.Path1 = "员工管理";
            var clientList = db.User.Where(a => a.Id == WebSecurity.CurrentUserId).Single().HRClients.Select(a => a.Id).ToList();
            Employee employee = db.Employee.Where(a => a.Id == offer.EmployeeId && clientList.Contains(a.ClientId)).SingleOrDefault();
            if (employee == null)
            {
                return HttpNotFound();
            }
            if (ModelState.IsValid)
            {
                employee.Timestamp = offer.Timestamp;
                employee.EmployeeStatus = EmployeeStatus.新增已通知;
                employee.OfferContent = offer.Content;

                db.PPSave();

                try
                {
                    WebMail.SmtpServer = "smtp.163.com";
                    WebMail.SmtpPort = 25;
                    WebMail.EnableSsl = false;
                    WebMail.UserName = "ssss123456ssss@163.com";
                    WebMail.Password = "tcltcl";
                    WebMail.From = "ssss123456ssss@163.com";
                    WebMail.Send("ssss123456ssssb@163.com", "RSVP Notification", offer.Content);
                }
                catch (Exception e)
                {
                    System.Diagnostics.Debug.WriteLine(e.ToString());
                }

                return RedirectToAction("HREmployeeIndex");
            }

            return View(offer);
        }

        [Authorize(Roles = "HR")]
        public ActionResult OnPosition(int id)//employeeId
        {
            ViewBag.Path1 = "员工管理";
            var clientList = db.User.Where(a => a.Id == WebSecurity.CurrentUserId).Single().HRClients.Select(a => a.Id).ToList();
            Employee employee = db.Employee.Where(a => a.Id == id && clientList.Contains(a.ClientId)).SingleOrDefault();
            if (employee == null)
            {
                return HttpNotFound();
            }
            OnPosition onPosition = new OnPosition { EmployeeId = employee.Id, Timestamp = employee.Timestamp };

            return View(onPosition);
        }

        [Authorize(Roles = "HR")]
        [HttpPost]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult OnPosition(OnPosition onPosition)
        {
            ViewBag.Path1 = "员工管理";
            var clientList = db.User.Where(a => a.Id == WebSecurity.CurrentUserId).Single().HRClients.Select(a => a.Id).ToList();
            Employee employee = db.Employee.Where(a => a.Id == onPosition.EmployeeId && clientList.Contains(a.ClientId)).SingleOrDefault();
            if (employee == null)
            {
                return HttpNotFound();
            }
            if (ModelState.IsValid)
            {
                employee.Timestamp = onPosition.Timestamp;
                employee.EmployeeStatus = EmployeeStatus.在职;

                db.PPSave();
                return RedirectToAction("HREmployeeIndex");
            }

            return View(onPosition);
        }

        [Authorize(Roles = "HR")]
        public ActionResult HREmployeeIndex()
        {
            ViewBag.Path1 = "员工管理";
            var clientList = db.User.Where(a => a.Id == WebSecurity.CurrentUserId).Single().HRClients.Select(a => a.Id).ToList();
            var employees = db.Employee.Where(a => clientList.Contains(a.ClientId));
            return View(employees.ToList());
        }

        [Authorize(Roles = "HR")]
        public ActionResult EditEmployeeBack(int id)
        {
            ViewBag.Path1 = "员工管理";
            var clientList = db.User.Where(a => a.Id == WebSecurity.CurrentUserId).Single().HRClients.Select(a => a.Id).ToList();
            Employee employee = db.Employee.Where(a => a.Id == id && clientList.Contains(a.ClientId)).SingleOrDefault();
            if (employee == null)
            {
                return HttpNotFound();
            }

            Mapper.CreateMap<Employee, EditEmployeeBack>().ForMember(x => x.EmployeeId, o => o.MapFrom(s => s.Id));
            var editEmployeeBack = Mapper.Map<Employee, EditEmployeeBack>(employee);
            editEmployeeBack.Employee = employee;
            editEmployeeBack.BudgetCenterIds = employee.BudgetCenters.Select(a => a.Id).ToList();
            editEmployeeBack.AssuranceIds = employee.Assurances.Select(a => a.Id).ToList();

            ViewBag.PensionTypeList = db.PensionType.OrderBy(a => a.Id).ToList();
            ViewBag.AccumulationTypeList = db.AccumulationType.OrderBy(a => a.Id).ToList();

            return View(editEmployeeBack);
        }

        [Authorize(Roles = "HR")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditEmployeeBack(EditEmployeeBack editEmployeeBack)
        {
            var clientList = db.User.Where(a => a.Id == WebSecurity.CurrentUserId).Single().HRClients.Select(a => a.Id).ToList();
            Employee employee = db.Employee.Include(a => a.BudgetCenters).Include(a => a.Assurances).Where(a => a.Id == editEmployeeBack.EmployeeId && clientList.Contains(a.ClientId)).SingleOrDefault();
            if (employee == null)
            {
                return HttpNotFound();
            }
            if (ModelState.IsValid)
            {
                employee.Timestamp = editEmployeeBack.Timestamp;
                employee.ChineseName = editEmployeeBack.ChineseName;
                employee.Mobile1 = editEmployeeBack.Mobile1;
                employee.PrivateMail = editEmployeeBack.PrivateMail;
                employee.EmployeeNote = editEmployeeBack.EmployeeNote;
                employee.BasicInfo6 = editEmployeeBack.BasicInfo6;
                employee.BasicInfo7 = editEmployeeBack.BasicInfo7;
                employee.BasicInfo8 = editEmployeeBack.BasicInfo8;
                employee.BasicInfo9 = editEmployeeBack.BasicInfo9;
                employee.BasicInfo10 = editEmployeeBack.BasicInfo10;
                employee.PensionCityId = editEmployeeBack.PensionCityId;
                employee.AccumulationCityId = editEmployeeBack.AccumulationCityId;
                employee.PensionTypeId = editEmployeeBack.PensionTypeId;
                employee.AccumulationTypeId = editEmployeeBack.AccumulationTypeId;
                employee.WorkClient = editEmployeeBack.WorkClient;
                employee.CompanyMail = editEmployeeBack.CompanyMail;
                employee.EnterDate = editEmployeeBack.EnterDate;
                employee.ProbationDueDate = editEmployeeBack.ProbationDueDate;
                employee.EnterClientDate = editEmployeeBack.EnterClientDate;
                employee.CompanyYearAdjust = editEmployeeBack.CompanyYearAdjust;
                employee.SocialYearAdjust = editEmployeeBack.SocialYearAdjust;
                employee.VacationDays = editEmployeeBack.VacationDays;
                employee.WorkCityId = editEmployeeBack.WorkCityId;
                employee.DepartmentId = editEmployeeBack.DepartmentId;
                employee.LevelId = editEmployeeBack.LevelId;
                employee.PositionId = editEmployeeBack.PositionId;
                employee.ContractNumber = editEmployeeBack.ContractNumber;
                employee.ContractBeginDate = editEmployeeBack.ContractBeginDate;
                employee.ContractEndDate = editEmployeeBack.ContractEndDate;
                employee.ContractTypeId = editEmployeeBack.ContractTypeId;
                employee.ProbationSalary = editEmployeeBack.ProbationSalary;
                employee.Salary = editEmployeeBack.Salary;
                employee.PensionStartMonth = editEmployeeBack.PensionStartMonth;
                employee.AccumulationStartMonth = editEmployeeBack.AccumulationStartMonth;
                employee.PayByCompany = editEmployeeBack.PayByCompany;
                employee.Yljs = editEmployeeBack.Yljs;
                employee.Syjs = editEmployeeBack.Syjs;
                employee.Yiliaojs = editEmployeeBack.Yiliaojs;
                employee.Gsjs = editEmployeeBack.Gsjs;
                employee.Shengyujs = editEmployeeBack.Shengyujs;
                employee.Qtjs = editEmployeeBack.Qtjs;
                employee.Bcjs = editEmployeeBack.Bcjs;
                employee.Gjjjs = editEmployeeBack.Gjjjs;
                employee.Bcgjjjs = editEmployeeBack.Bcgjjjs;
                employee.TaxType = editEmployeeBack.TaxType;
                employee.ZhangtaoId = editEmployeeBack.ZhangtaoId;
                employee.TaxCityId = editEmployeeBack.TaxCityId;
                employee.HireInfo1 = editEmployeeBack.HireInfo1;
                employee.HireInfo2 = editEmployeeBack.HireInfo2;
                employee.HireInfo3 = editEmployeeBack.HireInfo3;
                employee.HireInfo4 = editEmployeeBack.HireInfo4;
                employee.HireInfo5 = editEmployeeBack.HireInfo5;
                employee.HireInfo6 = editEmployeeBack.HireInfo6;
                employee.HireInfo7 = editEmployeeBack.HireInfo7;
                employee.HireInfo8 = editEmployeeBack.HireInfo8;
                employee.HireInfo9 = editEmployeeBack.HireInfo9;
                employee.HireInfo10 = editEmployeeBack.HireInfo10;
                employee.HireInfo11 = editEmployeeBack.HireInfo11;
                employee.HireInfo12 = editEmployeeBack.HireInfo12;
                employee.HireInfo13 = editEmployeeBack.HireInfo13;
                employee.HireInfo14 = editEmployeeBack.HireInfo14;
                employee.HireInfo15 = editEmployeeBack.HireInfo15;
                employee.HireInfo16 = editEmployeeBack.HireInfo16;
                employee.HireInfo17 = editEmployeeBack.HireInfo17;
                employee.HireInfo18 = editEmployeeBack.HireInfo18;
                employee.HireInfo19 = editEmployeeBack.HireInfo19;
                employee.HireInfo20 = editEmployeeBack.HireInfo20;

                //save budgetcenters, assurances

                var budgetcenters = db.BudgetCenter.Where(a => editEmployeeBack.BudgetCenterIds.Any(b => b == a.Id)).ToList();
                employee.BudgetCenters = budgetcenters;

                var assurances = db.Assurance.Where(a => editEmployeeBack.AssuranceIds.Any(b => b == a.Id)).ToList();
                employee.Assurances = assurances;

                db.PPSave();
                return RedirectToAction("HREmployeeIndex");
            }
            editEmployeeBack.Employee = employee;
            editEmployeeBack.BudgetCenterIds = employee.BudgetCenters.Select(a => a.Id).ToList();
            editEmployeeBack.AssuranceIds = employee.Assurances.Select(a => a.Id).ToList();

            ViewBag.PensionTypeList = db.PensionType.OrderBy(a => a.Id).ToList();
            ViewBag.AccumulationTypeList = db.AccumulationType.OrderBy(a => a.Id).ToList();

            return View(editEmployeeBack);
        }

        [Authorize(Roles = "Candidate")]
        public ActionResult EditEmployeeFront()
        {
            ViewBag.Path1 = "个人资料>";
            Employee employee = db.Employee.Where(a => a.UserId == WebSecurity.CurrentUserId).SingleOrDefault();
            if (employee == null)
            {
                return HttpNotFound();
            }
            Mapper.CreateMap<Employee, EditEmployeeFront>().ForMember(x => x.EmployeeId, o => o.MapFrom(s => s.Id));
            var editEmployeeFront = Mapper.Map<Employee, EditEmployeeFront>(employee);
            editEmployeeFront.Employee = employee;

            ViewBag.CertificateList = db.Certificate.OrderBy(a => a.Id).ToList();
            ViewBag.CityList = db.City.OrderBy(a => a.Id).ToList();
            ViewBag.PensionTypeList = db.PensionType.OrderBy(a => a.Id).ToList();
            ViewBag.ClientList = db.Client.OrderBy(a => a.Id).ToList();

            return View(editEmployeeFront);
        }

        [Authorize(Roles = "Candidate")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditEmployeeFront(EditEmployeeFront editEmployeeFront)
        {
            ViewBag.Path1 = "个人资料>";
            Employee employee = db.Employee.Where(a => a.UserId == WebSecurity.CurrentUserId && a.Id == editEmployeeFront.EmployeeId).SingleOrDefault();
            if (employee == null)
            {
                return HttpNotFound();
            }
            if (ModelState.IsValid)
            {
                employee.Timestamp = editEmployeeFront.Timestamp;
                employee.EnglishName = editEmployeeFront.EnglishName;
                employee.Sex = editEmployeeFront.Sex;
                employee.Marriage = editEmployeeFront.Marriage;
                employee.Nationality = editEmployeeFront.Nationality;
                employee.Nation = editEmployeeFront.Nation;
                employee.CertificateId = editEmployeeFront.CertificateId;
                employee.CertificateNumber = editEmployeeFront.CertificateNumber;
                employee.Birthday = editEmployeeFront.Birthday;
                employee.JuzhuAddress = editEmployeeFront.JuzhuAddress;
                employee.JuzhudiZipCode = editEmployeeFront.JuzhudiZipCode;
                employee.Mobile1 = editEmployeeFront.Mobile1;
                employee.Mobile2 = editEmployeeFront.Mobile2;
                employee.EmergencyContact = editEmployeeFront.EmergencyContact;
                employee.EmergencyContactPhone = editEmployeeFront.EmergencyContactPhone;
                employee.PrivateMail = editEmployeeFront.PrivateMail;
                employee.HujiAddress = editEmployeeFront.HujiAddress;
                employee.HujiZipCode = editEmployeeFront.HujiZipCode;
                employee.JuzhuzhengNumber = editEmployeeFront.JuzhuzhengNumber;
                employee.JuzhuzhengDueDate = editEmployeeFront.JuzhuzhengDueDate;
                employee.SocialGonglingStartDate = editEmployeeFront.SocialGonglingStartDate;
                employee.Bank = editEmployeeFront.Bank;
                employee.BankAccount = editEmployeeFront.BankAccount;
                employee.BankAccountName = editEmployeeFront.BankAccountName;
                employee.BasicInfo1 = editEmployeeFront.BasicInfo1;
                employee.BasicInfo2 = editEmployeeFront.BasicInfo2;
                employee.BasicInfo3 = editEmployeeFront.BasicInfo3;
                employee.BasicInfo4 = editEmployeeFront.BasicInfo4;
                employee.BasicInfo5 = editEmployeeFront.BasicInfo5;
                employee.BasicInfo6 = editEmployeeFront.BasicInfo6;
                employee.BasicInfo7 = editEmployeeFront.BasicInfo7;
                employee.BasicInfo8 = editEmployeeFront.BasicInfo8;
                employee.BasicInfo9 = editEmployeeFront.BasicInfo9;
                employee.BasicInfo10 = editEmployeeFront.BasicInfo10;

                employee.HukouType = editEmployeeFront.HukouType.Value;
                employee.PensionStatus = editEmployeeFront.PensionStatus;
                employee.YibaokaAvailable = editEmployeeFront.YibaokaAvailable;
                employee.AccumulationStatus = editEmployeeFront.AccumulationStatus;
                employee.AccumulationNumber = editEmployeeFront.AccumulationNumber;
                employee.DanganAddress = editEmployeeFront.DanganAddress;
                employee.DanganOrganization = editEmployeeFront.DanganOrganization;
                employee.DanganNumber = editEmployeeFront.DanganNumber;
                employee.PensionInfo1 = editEmployeeFront.PensionInfo1;
                employee.PensionInfo2 = editEmployeeFront.PensionInfo2;
                employee.PensionInfo3 = editEmployeeFront.PensionInfo3;
                employee.PensionInfo4 = editEmployeeFront.PensionInfo4;
                employee.PensionInfo5 = editEmployeeFront.PensionInfo5;

                db.PPSave();
                return RedirectToAction("EditEmployeeEducation");
            }
            editEmployeeFront.Employee = employee;

            ViewBag.CertificateList = db.Certificate.OrderBy(a => a.Id).ToList();
            ViewBag.CityList = db.City.OrderBy(a => a.Id).ToList();
            ViewBag.PensionTypeList = db.PensionType.OrderBy(a => a.Id).ToList();
            ViewBag.ClientList = db.Client.OrderBy(a => a.Id).ToList();

            return View(editEmployeeFront);
        }

        [Authorize(Roles = "Candidate")]
        public ActionResult EditEmployeeEducation()
        {
            ViewBag.Path1 = "教育信息>";
            Employee employee = db.Employee.Where(a => a.UserId == WebSecurity.CurrentUserId).SingleOrDefault();
            if (employee == null)
            {
                return HttpNotFound();
            }
            var editEmployeeEducation = new EditEmployeeEducation { EmployeeId = employee.Id };

            Mapper.CreateMap<Education, EditEducation>().ForMember(x => x.EducationId, o => o.MapFrom(s => s.Id));
            var list = Mapper.Map<ICollection<Education>, ICollection<EditEducation>>(employee.GetEducations());

            editEmployeeEducation.EditEducations = list;

            return View(editEmployeeEducation);
        }

        [Authorize(Roles = "Candidate")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditEmployeeEducation(EditEmployeeEducation editEmployeeEducation)
        {
            ViewBag.Path1 = "教育信息>";
            var employee = db.Employee.Include(a => a.Educations).Where(a => a.UserId == WebSecurity.CurrentUserId && a.Id == editEmployeeEducation.EmployeeId).SingleOrDefault();
            if (employee == null)
            {
                return HttpNotFound();
            }
            if (ModelState.IsValid)
            {
                var old = new HashSet<int>(employee.GetEducations().Select(a => a.Id));
                var cur = new HashSet<int>(editEmployeeEducation.EditEducations.Where(a => a.Delete == false).Select(a => a.EducationId));
                // 取得不在最新列表中的记录删除
                var del = (from a in old
                           where !(cur.Contains(a))
                           select a).ToList();
                foreach (var i in del)
                {
                    var e = db.Education.Find(i);
                    db.Education.Remove(e);
                }
                // end

                // 取得在最新列表中的记录更新
                var upd = (from a in old
                           where cur.Contains(a)
                           select a).ToList();
                foreach (var i in upd)
                {
                    var e1 = db.Education.Find(i);
                    var e2 = editEmployeeEducation.EditEducations.Where(a => a.EducationId == i).Single();
                    e1.School = e2.School;
                    e1.Major = e2.Major;
                    e1.Degree = e2.Degree.Value;
                    e1.Begin = e2.Begin.Value;
                    e1.End = e2.End;
                }
                // end

                // 取得在最新列表中的记录添加
                var add = editEmployeeEducation.EditEducations.Where(a => a.Delete == false && a.EducationId == 0);
                foreach (var i in add)
                {
                    var e = new Education { EmployeeId = employee.Id, School = i.School, Major = i.Major, Degree = i.Degree.Value, Begin = i.Begin.Value, End = i.End };
                    employee.Educations.Add(e);
                }
                // end
                db.PPSave();
                return RedirectToAction("EditEmployeeWork");
            }

            return View(editEmployeeEducation);
        }

        [Authorize(Roles = "Candidate")]
        public ActionResult EditEmployeeWork()
        {
            ViewBag.Path1 = "工作经历>";
            var employee = db.Employee.Where(a => a.UserId == WebSecurity.CurrentUserId).SingleOrDefault();
            if (employee == null)
            {
                return HttpNotFound();
            }
            var editEmployeeWork = new EditEmployeeWork { EmployeeId = employee.Id };

            Mapper.CreateMap<Work, EditWork>().ForMember(x => x.WorkId, o => o.MapFrom(s => s.Id));
            var list = Mapper.Map<ICollection<Work>, ICollection<EditWork>>(employee.GetWorks());

            editEmployeeWork.EditWorks = list;

            return View(editEmployeeWork);
        }

        [Authorize(Roles = "Candidate")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditEmployeeWork(EditEmployeeWork editEmployeeWork)
        {
            ViewBag.Path1 = "工作经历>";
            var employee = db.Employee.Include(a => a.Works).Where(a => a.UserId == WebSecurity.CurrentUserId && a.Id == editEmployeeWork.EmployeeId).SingleOrDefault();
            if (employee == null)
            {
                return HttpNotFound();
            }
            if (ModelState.IsValid)
            {
                var old = new HashSet<int>(employee.GetWorks().Select(a => a.Id));
                var cur = new HashSet<int>(editEmployeeWork.EditWorks.Where(a => a.Delete == false).Select(a => a.WorkId));
                // 取得不在最新列表中的记录删除
                var del = (from a in old
                           where !(cur.Contains(a))
                           select a).ToList();
                foreach (var i in del)
                {
                    var e = db.Work.Find(i);
                    db.Work.Remove(e);
                }
                // end

                // 取得在最新列表中的记录更新
                var upd = (from a in old
                           where cur.Contains(a)
                           select a).ToList();
                foreach (var i in upd)
                {
                    var e1 = db.Work.Find(i);
                    var e2 = editEmployeeWork.EditWorks.Where(a => a.WorkId == i).Single();
                    e1.Company = e2.Company;
                    e1.Position = e2.Position;
                    e1.Begin = e2.Begin.Value;
                    e1.End = e2.End;
                    e1.Contact = e2.Contact;
                    e1.Phone = e2.Phone;
                    e1.Note = e2.Note;
                }
                // end

                // 取得在最新列表中的记录添加
                var add = editEmployeeWork.EditWorks.Where(a => a.Delete == false && a.WorkId == 0);
                foreach (var i in add)
                {
                    var e = new Work { Company = i.Company, Position = i.Position, Begin = i.Begin.Value, End = i.End, Contact = i.Contact, Phone = i.Phone, Note = i.Note };
                    employee.Works.Add(e);
                }
                // end
                db.PPSave();
                return RedirectToAction("EditEmployeeFamily");
            }

            return View(editEmployeeWork);
        }

        [Authorize(Roles = "Candidate")]
        public ActionResult EditEmployeeFamily()
        {
            ViewBag.Path1 = "家庭信息>";
            var employee = db.Employee.Where(a => a.UserId == WebSecurity.CurrentUserId).SingleOrDefault();
            if (employee == null)
            {
                return HttpNotFound();
            }
            var editEmployeeFamily = new EditEmployeeFamily { EmployeeId = employee.Id };

            Mapper.CreateMap<Family, EditFamily>().ForMember(x => x.FamilyId, o => o.MapFrom(s => s.Id));
            var list = Mapper.Map<ICollection<Family>, ICollection<EditFamily>>(employee.GetFamilies());

            editEmployeeFamily.EditFamilies = list;

            return View(editEmployeeFamily);
        }

        [Authorize(Roles = "Candidate")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditEmployeeFamily(EditEmployeeFamily editEmployeeFamily)
        {
            ViewBag.Path1 = "家庭信息>";
            var employee = db.Employee.Include(a => a.Families).Where(a => a.UserId == WebSecurity.CurrentUserId && a.Id == editEmployeeFamily.EmployeeId).SingleOrDefault();
            if (employee == null)
            {
                return HttpNotFound();
            }
            if (ModelState.IsValid)
            {
                var old = new HashSet<int>(employee.GetFamilies().Select(a => a.Id));
                var cur = new HashSet<int>(editEmployeeFamily.EditFamilies.Where(a => a.Delete == false).Select(a => a.FamilyId));
                // 取得不在最新列表中的记录删除
                var del = (from a in old
                           where !(cur.Contains(a))
                           select a).ToList();
                foreach (var i in del)
                {
                    var e = db.Family.Find(i);
                    db.Family.Remove(e);
                }
                // end

                // 取得在最新列表中的记录更新
                var upd = (from a in old
                           where cur.Contains(a)
                           select a).ToList();
                foreach (var i in upd)
                {
                    var e1 = db.Family.Find(i);
                    var e2 = editEmployeeFamily.EditFamilies.Where(a => a.FamilyId == i).Single();
                    e1.Name = e2.Name;
                    e1.Relation = e2.Relation;
                    e1.Sex = e2.Sex.Value;
                    e1.Company = e2.Company;
                    e1.Position = e2.Position;
                    e1.Phone = e2.Phone;
                }
                // end

                // 取得在最新列表中的记录添加
                var add = editEmployeeFamily.EditFamilies.Where(a => a.Delete == false && a.FamilyId == 0);
                foreach (var i in add)
                {
                    var e = new Family { Name = i.Name, Relation = i.Relation, Sex = i.Sex.Value, Company = i.Company, Position = i.Position, Phone = i.Phone };
                    employee.Families.Add(e);
                }
                // end
                db.PPSave();
                return RedirectToAction("EditEmployeeDoc");
            }

            return View(editEmployeeFamily);
        }

        [Authorize(Roles = "Candidate")]
        public ActionResult EditEmployeeDoc()
        {
            ViewBag.Path1 = "上传资料>";
            var employee = db.Employee.Include(a => a.EmployeeDocs).Where(a => a.UserId == WebSecurity.CurrentUserId).SingleOrDefault();
            if (employee == null)
            {
                return HttpNotFound();
            }
            var editEmployeeDoc = new EditEmployeeDoc { EmployeeId = employee.Id };

            // 取得资料列表模版
            var clientPensionCityDocument = db.ClientPensionCityDocument.Include(a => a.Documents).Where(a => a.ClientId == employee.ClientId && ((a.PensionCityId == null && employee.PensionCityId == null) || (a.PensionCityId == employee.PensionCityId))).SingleOrDefault();
            if (clientPensionCityDocument == null || clientPensionCityDocument.Documents == null)
            {
                throw new Exception("对应资料列表未配置, 请联系客服人员!");
            }

            foreach (var item in clientPensionCityDocument.Documents)
            {
                //var employeeDoc = db.EmployeeDoc.Where(a => a.EmployeeId == employee.Id && a.DocumentId == item.Id).SingleOrDefault();
                var employeeDoc = employee.EmployeeDocs.Where(a => a.IsDeleted == false).Where(a => a.DocumentId == item.Id).SingleOrDefault();
                var imgPath = "";
                int employeeDocId = 0;
                if (employeeDoc != null && !String.IsNullOrWhiteSpace(employeeDoc.ImgPath))
                {
                    imgPath = employeeDoc.ImgPath;
                    employeeDocId = employeeDoc.Id;
                }
                var editSingleEmployeeDoc = new EditSingleEmployeeDoc { EmployeeDocId = employeeDocId, DocumentId = item.Id, DocumentName = item.Name, ImgPath = (String.IsNullOrWhiteSpace(imgPath)) ? null : imgPath };
                editEmployeeDoc.EditSingleEmployeeDocs.Add(editSingleEmployeeDoc);
            }

            return View(editEmployeeDoc);
        }

        [Authorize(Roles = "Candidate")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditEmployeeDoc(EditEmployeeDoc editEmployeeDoc)
        {
            ViewBag.Path1 = "上传资料>";
            var employee = db.Employee.Include(a => a.EmployeeDocs).Where(a => a.UserId == WebSecurity.CurrentUserId && a.Id == editEmployeeDoc.EmployeeId).SingleOrDefault();
            if (employee == null)
            {
                return HttpNotFound();
            }
            if (ModelState.IsValid)
            {
                var old = new HashSet<int>(employee.GetEmployeeDocs().Select(a => a.Id));
                var cur = new HashSet<int>(editEmployeeDoc.EditSingleEmployeeDocs.Where(a => !String.IsNullOrWhiteSpace(a.ImgPath)).Select(a => a.EmployeeDocId));
                // 取得不在最新列表中的记录删除
                var del = (from a in old
                           where !(cur.Contains(a))
                           select a).ToList();
                foreach (var i in del)
                {
                    var e = db.EmployeeDoc.Find(i);
                    db.EmployeeDoc.Remove(e);
                }
                // end

                // 取得在最新列表中的记录更新
                var upd = (from a in old
                           where cur.Contains(a)
                           select a).ToList();
                foreach (var i in upd)
                {
                    var e1 = db.EmployeeDoc.Find(i);
                    var e2 = editEmployeeDoc.EditSingleEmployeeDocs.Where(a => a.EmployeeDocId == i).Single();
                    e1.ImgPath = e2.ImgPath;
                }
                // end

                // 取得在最新列表中的记录添加
                var add = editEmployeeDoc.EditSingleEmployeeDocs.Where(a => !String.IsNullOrWhiteSpace(a.ImgPath) && a.EmployeeDocId == 0);
                foreach (var i in add)
                {
                    var e = new EmployeeDoc { EmployeeId = employee.Id, DocumentId = i.DocumentId, ImgPath = i.ImgPath };
                    employee.EmployeeDocs.Add(e);
                }
                // end
                db.PPSave();
                return RedirectToAction("FrontDetail");
            }

            return View(editEmployeeDoc);
        }

        [Authorize(Roles = "Candidate")]
        [HttpPost]
        public string UploadImg(HttpPostedFileBase filebase)
        {
            DateTime importNow = DateTime.Now;
            TimeSpan _TimeSpan = (DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0));
            long importTime = (long)_TimeSpan.TotalMilliseconds;

            HttpPostedFileBase file = Request.Files["files"];
            string FileName;
            string savePath;
            if (file == null || file.ContentLength <= 0)
            {
                return "文件不能为空";
            }
            else
            {
                string filename = Path.GetFileName(file.FileName);
                int filesize = file.ContentLength;//获取上传文件的大小单位为字节byte
                string fileEx = System.IO.Path.GetExtension(filename);//获取上传文件的扩展名
                string NoFileName = System.IO.Path.GetFileNameWithoutExtension(filename);//获取无扩展名的文件名
                int Maxsize = 10000 * 1024;//定义上传文件的最大空间大小为4M
                string FileType = ".xls,.xlsx";//定义上传文件的类型字符串

                FileName = NoFileName + importNow.ToString("yyyyMMddhhmmss") + "_" + importTime + fileEx;
                //if (!FileType.Contains(fileEx))
                //{
                //    return "文件类型不对，只能导入xls和xlsx格式的文件";
                //}
                if (filesize >= Maxsize)
                {
                    return "上传文件超过2M，不能上传";
                }
                string uploadPath = AppDomain.CurrentDomain.BaseDirectory + "Content/UploadedFolder/";
                savePath = Path.Combine(uploadPath, FileName);
                file.SaveAs(savePath);
                return "OK" + FileName;
            }
        }

        //
        // GET: /Employee/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Employee employee = db.Employee.Find(id);
            if (employee == null)
            {
                return HttpNotFound();
            }
            return View(employee);
        }

        //
        // POST: /Employee/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Employee employee = db.Employee.Find(id);
            db.Employee.Remove(employee);
            db.PPSave();
            return RedirectToAction("Index");
        }

        [ChildActionOnly]
        public PartialViewResult EmployeeAbstract(int id = 0)
        {
            var employee = db.Employee.Find(id);
            return PartialView(employee);
        }

        [ChildActionOnly]
        public PartialViewResult EmployeeDetail(int id = 0)//employeeId
        {
            var employee = db.Employee.Find(id);
            return PartialView(employee);
        }

        [ChildActionOnly]
        public PartialViewResult EmployeeDoc(int id = 0)//employeeId
        {
            var employee = db.Employee.Find(id);
            var docList = db.ClientPensionCityDocument.Include(a => a.Documents).Where(a => a.ClientId == employee.ClientId && ((employee.PensionCityId == null && a.PensionCityId == null) || employee.PensionCityId == a.PensionCityId)).SingleOrDefault();
            if (docList == null)
            {
                throw new Exception("对应资料列表未配置, 请联系客服人员!");
            }
            ViewBag.DocList = docList;

            return PartialView(employee);
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}