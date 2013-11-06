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

namespace OB.Controllers
{
    public class EmployeeController : Controller
    {
        private OBContext db = new OBContext();

        //
        // GET: /Employee/

        public ActionResult Index()
        {
            return View(db.Employee.ToList());
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
                db.SaveChanges();
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

            var docList = db.ClientPensionCityDocument.Where(a => a.ClientId == employee.ClientId && ((employee.PensionCityId == null && a.PensionCityId == null) || employee.PensionCityId == a.PensionCityId)).SingleOrDefault();
            if (docList == null)
            {
                throw new Exception("对应资料列表未配置, 请联系客服人员!");
            }
            ViewBag.DocList = docList;

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
                db.SaveChanges();
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
            return HttpNotFound();
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
            ViewBag.Path1 = "员工资料编辑>";
            var clientList = db.User.Where(a => a.Id == WebSecurity.CurrentUserId).Single().HRClients.Select(a => a.Id).ToList();
            Employee employee = db.Employee.Where(a => a.Id == id && clientList.Contains(a.ClientId)).SingleOrDefault();
            if (employee == null)
            {
                return HttpNotFound();
            }

            // 处理EditEmployeeBack中必填但在employee中为空的值



            Mapper.CreateMap<Employee, EditEmployeeBack>().ForMember(x => x.EmployeeId, o => o.MapFrom(s => s.Id));
            var editEmployeeBack = Mapper.Map<Employee, EditEmployeeBack>(employee);
            editEmployeeBack.Employee = employee;

            ViewBag.PensionTypeList = db.PensionType.OrderBy(a => a.Id).ToList();
            ViewBag.AccumulationTypeList = db.AccumulationType.OrderBy(a => a.Id).ToList();
            ViewBag.TaxTypeList = db.TaxType.OrderBy(a => a.Id).ToList();

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

            ViewBag.SexList = db.Sex.OrderBy(a => a.Id).ToList();
            ViewBag.MarriageList = db.Marriage.OrderBy(a => a.Id).ToList();
            ViewBag.CertificateList = db.Certificate.OrderBy(a => a.Id).ToList();
            ViewBag.HukouTypeList = db.HukouType.OrderBy(a => a.Id).ToList();
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
                employee.SexId = editEmployeeFront.SexId;
                employee.MarriageId = editEmployeeFront.MarriageId;
                employee.Nationality = editEmployeeFront.Nationality;
                employee.Nation = editEmployeeFront.Nation;
                employee.CertificateId = editEmployeeFront.CertificateId;
                employee.CertificateNumber = editEmployeeFront.CertificateNumber;
                employee.Birthday = editEmployeeFront.Birthday;
                employee.JuzhuAddress = editEmployeeFront.JuzhuAddress;
                employee.JuzhudiZipCode = editEmployeeFront.JuzhudiZipCode;
                employee.Mobile1 = editEmployeeFront.Mobile1;
                employee.Mobile2 = editEmployeeFront.Mobile2;
                employee.EmergencyContract = editEmployeeFront.EmergencyContract;
                employee.EmergencyContractPhone = editEmployeeFront.EmergencyContractPhone;
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

                employee.HukouTypeId = editEmployeeFront.HukouTypeId;
                employee.PensionStatusId = editEmployeeFront.PensionStatusId;
                employee.YibaokaAvailable = editEmployeeFront.YibaokaAvailable;
                employee.AccumulationStatusId = editEmployeeFront.AccumulationStatusId;
                employee.AccumulationNumber = editEmployeeFront.AccumulationNumber;
                employee.DanganAddress = editEmployeeFront.DanganAddress;
                employee.DanganOrganization = editEmployeeFront.DanganOrganization;
                employee.DanganNumber = editEmployeeFront.DanganNumber;
                employee.PensionInfo1 = editEmployeeFront.PensionInfo1;
                employee.PensionInfo2 = editEmployeeFront.PensionInfo2;
                employee.PensionInfo3 = editEmployeeFront.PensionInfo3;
                employee.PensionInfo4 = editEmployeeFront.PensionInfo4;
                employee.PensionInfo5 = editEmployeeFront.PensionInfo5;

                db.SaveChanges();
                return RedirectToAction("EditEmployeeEducation");
            }
            editEmployeeFront.Employee = employee;

            ViewBag.SexList = db.Sex.OrderBy(a => a.Id).ToList();
            ViewBag.MarriageList = db.Marriage.OrderBy(a => a.Id).ToList();
            ViewBag.CertificateList = db.Certificate.OrderBy(a => a.Id).ToList();
            ViewBag.HukouTypeList = db.HukouType.OrderBy(a => a.Id).ToList();
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
            var list = Mapper.Map<ICollection<Education>, ICollection<EditEducation>>(employee.Educations);

            editEmployeeEducation.EditEducations = list;

            ViewBag.DegreeList = db.Degree.OrderBy(a => a.Id).ToList();
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
                if (editEmployeeEducation.EditEducations == null)
                {
                    editEmployeeEducation.EditEducations = new List<EditEducation> { };
                }
                var old = new HashSet<int>(db.Education.Where(a => a.EmployeeId == editEmployeeEducation.EmployeeId).Select(a => a.Id));
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
                    e1.DegreeId = e2.DegreeId;
                    e1.Begin = e2.Begin;
                    e1.End = e2.End;
                }
                // end

                // 取得在最新列表中的记录添加
                var add = editEmployeeEducation.EditEducations.Where(a => a.Delete == false && a.EducationId == 0);
                foreach (var i in add)
                {
                    var e = new Education { School = i.School, Major = i.Major, DegreeId = i.DegreeId, Begin = i.Begin, End = i.End };
                    employee.Educations.Add(e);
                }
                // end
                db.SaveChanges();
                return RedirectToAction("EditEmployeeWork");
            }

            ViewBag.DegreeList = db.Degree.OrderBy(a => a.Id).ToList();
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
            var list = Mapper.Map<ICollection<Work>, ICollection<EditWork>>(employee.Works);

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
                if (editEmployeeWork.EditWorks == null)
                {
                    editEmployeeWork.EditWorks = new List<EditWork> { };
                }
                var old = new HashSet<int>(db.Work.Where(a => a.EmployeeId == editEmployeeWork.EmployeeId).Select(a => a.Id));
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
                    e1.Begin = e2.Begin;
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
                    var e = new Work { Company = i.Company, Position = i.Position, Begin = i.Begin, End = i.End, Contact = i.Contact, Phone = i.Phone, Note = i.Note };
                    employee.Works.Add(e);
                }
                // end
                db.SaveChanges();
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
            var list = Mapper.Map<ICollection<Family>, ICollection<EditFamily>>(employee.Families);

            editEmployeeFamily.EditFamilies = list;

            ViewBag.SexList = db.Sex.OrderBy(a => a.Id).ToList();
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
                if (editEmployeeFamily.EditFamilies == null)
                {
                    editEmployeeFamily.EditFamilies = new List<EditFamily> { };
                }
                var old = new HashSet<int>(db.Family.Where(a => a.EmployeeId == editEmployeeFamily.EmployeeId).Select(a => a.Id));
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
                    e1.SexId = e2.SexId;
                    e1.Company = e2.Company;
                    e1.Position = e2.Position;
                    e1.Phone = e2.Phone;
                }
                // end

                // 取得在最新列表中的记录添加
                var add = editEmployeeFamily.EditFamilies.Where(a => a.Delete == false && a.FamilyId == 0);
                foreach (var i in add)
                {
                    var e = new Family { Name = i.Name, Relation = i.Relation, SexId = i.SexId, Company = i.Company, Position = i.Position, Phone = i.Phone };
                    employee.Families.Add(e);
                }
                // end
                db.SaveChanges();
                return RedirectToAction("EditEmployeeDoc");
            }

            ViewBag.SexList = db.Sex.OrderBy(a => a.Id).ToList();
            return View(editEmployeeFamily);
        }

        [Authorize(Roles = "Candidate")]
        public ActionResult EditEmployeeDoc()
        {
            ViewBag.Path1 = "上传资料>";
            var employee = db.Employee.Where(a => a.UserId == WebSecurity.CurrentUserId).SingleOrDefault();
            if (employee == null)
            {
                return HttpNotFound();
            }
            var editEmployeeDoc = new EditEmployeeDoc { EmployeeId = employee.Id, EditSingleEmployeeDocs = new List<EditSingleEmployeeDoc> { } };

            // 取得资料列表模版
            var clientPensionCityDocument = db.ClientPensionCityDocument.Include(a => a.Documents).Where(a => a.ClientId == employee.ClientId && ((a.PensionCityId == null && employee.PensionCityId == null) || (a.PensionCityId == employee.PensionCityId))).SingleOrDefault();
            if (clientPensionCityDocument == null || clientPensionCityDocument.Documents == null)
            {
                throw new Exception("对应资料列表未配置, 请联系客服人员!");
            }

            foreach (var item in clientPensionCityDocument.Documents)
            {
                var employeeDoc = db.EmployeeDoc.Where(a => a.EmployeeId == employee.Id && a.DocumentId == item.Id).SingleOrDefault();
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
            var employee = db.Employee.Include(a => a.Families).Where(a => a.UserId == WebSecurity.CurrentUserId && a.Id == editEmployeeDoc.EmployeeId).SingleOrDefault();
            if (employee == null)
            {
                return HttpNotFound();
            }
            if (ModelState.IsValid)
            {
                if (editEmployeeDoc.EditSingleEmployeeDocs == null)
                {
                    editEmployeeDoc.EditSingleEmployeeDocs = new List<EditSingleEmployeeDoc> { };
                }
                var old = new HashSet<int>(db.EmployeeDoc.Where(a => a.EmployeeId == editEmployeeDoc.EmployeeId).Select(a => a.Id));
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
                db.SaveChanges();
                return RedirectToAction("FrontDetail");
            }

            ViewBag.SexList = db.Sex.OrderBy(a => a.Id).ToList();
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
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public PartialViewResult EmployeeAbstract(int id = 0)
        {
            var employee = db.Employee.Find(id);
            return PartialView(employee);
        }

        public string Percent(int id = 0)
        {
            // 常规项
            var employee = db.Employee.Find(id);
            if (employee == null)
            {
                return "0%";
            }

            // 取得普通权重
            var weight = db.Weight.Where(a => a.WeightClientId == employee.ClientId).SingleOrDefault();
            if (weight == null)
            {
                weight = db.Weight.Where(a => a.WeightClientId == null).SingleOrDefault();
            }
            PropertyInfo[] fs = typeof(Weight).GetProperties();
            decimal count = 0;
            decimal total = 0;
            string[] exclude = { "Id", "WeightClientId", "WeightClient" };
            string[] collection = { "Educations", "Works", "Families" };
            foreach (PropertyInfo item in fs)
            {
                if (exclude.Contains(item.Name))
                {
                    continue;
                }
                else
                {
                    var t = typeof(Employee).GetProperty(item.Name);
                    var v = t.GetValue(employee);
                    if ((collection.Contains(item.Name) && Convert.ToInt32(v.GetType().GetProperty("Count").GetValue(v)) > 0) || !(collection.Contains(item.Name) && v != null))
                    {
                        count += Convert.ToInt32(typeof(Weight).GetProperty(item.Name).GetValue(weight));
                    }
                }
                total += Convert.ToInt32(typeof(Weight).GetProperty(item.Name).GetValue(weight));
            }

            // 取得上传文件权重
            var clientPensionCityDocument = db.ClientPensionCityDocument.Where(a => a.ClientId == employee.ClientId && ((a.PensionCityId == null && employee.PensionCityId == null) || (a.PensionCityId == employee.PensionCityId))).SingleOrDefault();
            if (clientPensionCityDocument == null)
            {
                return "0%";
            }
            foreach (var item in clientPensionCityDocument.Documents)
            {
                var doc = db.EmployeeDoc.Where(a => a.EmployeeId == employee.Id && a.DocumentId == item.Id).SingleOrDefault();
                if (doc != null && !String.IsNullOrWhiteSpace(doc.ImgPath))
                {
                    count += item.Weight;
                }
                total += item.Weight;
            }

            if (total == 0)
            {
                return "0%";
            }
            else
            {
                return String.Format("{0:P2}.", count / total);
            }
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}