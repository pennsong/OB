﻿using FrameLog.Contexts;
using OB.Models;
using OB.Models.DAL;
using OB.Models.FrameLog;
using OB.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
using WebMatrix.WebData;

namespace OB.Lib
{
    public class Common<T>
    {
        public static IQueryable<T> Page(Controller c, RouteValueDictionary rv, IQueryable<T> q, int size = 10)
        {
            var tmpPage = rv.Where(a => a.Key == "page").Select(a => a.Value).SingleOrDefault();
            int page = int.Parse(tmpPage.ToString());
            var tmpTotalPage = (int)Math.Ceiling(((decimal)(q.Count()) / size));
            page = page > tmpTotalPage ? tmpTotalPage : page;
            page = page == 0 ? 1 : page;
            rv.Add("totalPage", tmpTotalPage);
            rv["page"] = page;

            c.ViewBag.RV = rv;
            return q.Skip(((tmpTotalPage > 0 ? page : 1) - 1) * size).Take(size);
        }

        public static IEnumerable<T> Page(Controller c, RouteValueDictionary rv, IEnumerable<T> q, int size = 2)
        {
            var tmpPage = rv.Where(a => a.Key == "page").Select(a => a.Value).SingleOrDefault();
            int page = int.Parse(tmpPage.ToString());
            var tmpTotalPage = (int)Math.Ceiling(((decimal)(q.Count()) / size));
            page = page > tmpTotalPage ? tmpTotalPage : page;
            rv.Add("totalPage", tmpTotalPage);
            rv["page"] = page;

            c.ViewBag.RV = rv;
            return q.Skip(((tmpTotalPage > 0 ? page : 1) - 1) * size).Take(size);
        }
    }

    public class Common
    {
        public static string UploadImg(Controller controller, HttpPostedFileBase filebase, string path = "")
        {
            DateTime importNow = DateTime.Now;
            TimeSpan _TimeSpan = (DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0));
            long importTime = (long)_TimeSpan.TotalMilliseconds;

            HttpPostedFileBase file = controller.Request.Files["files"];
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
                int Maxsize = 5* 1024 * 1024;//定义上传文件的最大空间大小为5M
                string FileType = ".xls,.xlsx,.png,.jpg,.jpeg,.pdf";//定义上传文件的类型字符串

                FileName = NoFileName + importNow.ToString("yyyyMMddhhmmss") + "_" + importTime + fileEx;
                if (!FileType.Contains(fileEx.ToLower()))
                {
                    return "文件类型不对，只能上传xls,xlsx,png,jpg,jpeg,pdf格式的文件";
                }
                if (filesize >= Maxsize)
                {
                    return "上传文件超过2M，不能上传";
                }
                Directory.CreateDirectory(AppDomain.CurrentDomain.BaseDirectory + "Content/UploadedFolder/" + path + "/" + importNow.ToString("yyyyMMdd") + "/");
                string uploadPath = AppDomain.CurrentDomain.BaseDirectory + "Content/UploadedFolder/" + path + "/" + importNow.ToString("yyyyMMdd") + "/";
                savePath = Path.Combine(uploadPath, FileName);
                file.SaveAs(savePath);
                return "OK" + importNow.ToString("yyyyMMdd") + "/" + FileName;
            }
        }

        //带消息提示的返回索引页面
        public static void RMError(Controller controller, string msg = "权限范围内没有找到对应记录")
        {
            Msg message = new Msg { MsgType = MsgType.ERROR, Content = msg };
            controller.TempData["msg"] = message;
        }

        public static void RMOk(Controller controller, string msg = "操作成功!")
        {
            Msg message = new Msg { MsgType = MsgType.OK, Content = msg };
            controller.TempData["msg"] = message;
        }

        public static void RMWarn(Controller controller, string msg)
        {
            Msg message = new Msg { MsgType = MsgType.WARN, Content = msg };
            controller.TempData["msg"] = message;
        }

        public static IQueryable<User> GetUserList(OBContext context, string filter = "")
        {
            filter = filter.ToUpper();
            var usernames = Roles.GetUsersInRole("Candidate");
            var curUserHRClients = context.User.Where(a => a.Id == WebSecurity.CurrentUserId).Single().HRClients.Select(a => a.Id);
            var users = from a in context.User
                        join b in context.Employee
                        on a.Id equals b.UserId
                        where usernames.Contains(a.Name) && curUserHRClients.Contains(b.ClientId)
                        select a;

            if (!String.IsNullOrWhiteSpace(filter))
            {
                users = users.Where(a => a.Name.ToUpper().Contains(filter) || a.Mail.ToUpper().Contains(filter));
            }
            return users;
        }

        public static IQueryable<User> HRCandidateList(OBContext context, string filter = "")
        {
            filter = filter.ToUpper();
            var usernames = Roles.GetUsersInRole("Candidate");
            var curUserHRClients = context.User.Where(a => a.Id == WebSecurity.CurrentUserId).Single().HRClients.Select(a => a.Id);
            var users = from a in context.User
                        join b in context.Employee
                        on a.Id equals b.UserId
                        where usernames.Contains(a.Name) && curUserHRClients.Contains(b.ClientId)
                        select a;

            if (!String.IsNullOrWhiteSpace(filter))
            {
                users = users.Where(a => a.Name.ToUpper().Contains(filter) || a.Mail.ToUpper().Contains(filter));
            }
            return users;
        }

        public static IList<Document> ClientDocumentList(string clientName, OBContext context = null)
        {
            if (context == null)
            {
                using (var context2 = new OBContext())
                {
                    return _ClientDocumentList(clientName, context2);
                }
            }
            else
            {
                return _ClientDocumentList(clientName, context);
            }
        }

        private static IList<Document> _ClientDocumentList(string clientName, OBContext context)
        {
            return context.Document.Where(x => x.Client.Name == clientName).ToList();
        }

        //public static void MailTo(string mailAddress, string subject, string content)
        //{
        //    WebMail.SmtpServer = "smtp.163.com";
        //    WebMail.SmtpPort = 25;
        //    WebMail.EnableSsl = false;
        //    WebMail.UserName = "ssss123456ssss@163.com";
        //    WebMail.Password = "tcltcl";
        //    WebMail.From = "ssss123456ssss@163.com";
        //    WebMail.Send(mailAddress, subject, content);
        //}

        public static void MailTo(string emailid, string subject, string body)
        {
            System.Net.Mail.SmtpClient client = new System.Net.Mail.SmtpClient();
            client.DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network;
            client.EnableSsl = false;
            client.Host = "smtp.163.com";
            client.Port = 25;


            System.Net.NetworkCredential credentials = new System.Net.NetworkCredential("ssss123456ssss@163.com", "tcltcl");
            client.UseDefaultCredentials = false;
            client.Credentials = credentials;

            System.Net.Mail.MailMessage msg = new System.Net.Mail.MailMessage();
            msg.From = new MailAddress("ssss123456ssss@163.com");
            msg.To.Add(new MailAddress(emailid));

            msg.Subject = subject;
            msg.IsBodyHtml = true;
            msg.Body = body;

            client.Send(msg);
        }
        /////////////////////
        //user
        // general user
        public static IQueryable<User> GetUserQuery(OBContext db, bool includeSoftDeleted = false, string filter = "")
        {
            filter = filter.ToUpper();

            var result = db.User.Where(a => a.Name.ToUpper().Contains(filter) || a.Mail.ToUpper().Contains(filter));

            if (!includeSoftDeleted)
            {
                result = result.Where(a => a.IsDeleted == false);
            }
            return result;
        }
        // end general user

        // hradmin
        public static List<User> GetHRAdminList(bool includeSoftDeleted = false, string filter = "")
        {
            using (var db = new OBContext())
            {
                return GetHRAdminQuery(db, includeSoftDeleted, filter).ToList();
            }
        }

        public static IQueryable<User> GetHRAdminQuery(OBContext db, bool includeSoftDeleted = false, string filter = "")
        {
            filter = filter.ToUpper();

            var usernames = Roles.GetUsersInRole("HRAdmin");

            var result = db.User.Where(x => usernames.Contains(x.Name)).Where(a => a.Name.ToUpper().Contains(filter) || a.Mail.ToUpper().Contains(filter));

            if (!includeSoftDeleted)
            {
                result = result.Where(a => a.IsDeleted == false);
            }
            return result;
        }
        // end hradmin

        // hr
        public static List<User> GetHRList(bool includeSoftDeleted = false, string filter = "")
        {
            using (var db = new OBContext())
            {
                return GetHRQuery(db, includeSoftDeleted, filter).ToList();
            }
        }

        public static IQueryable<User> GetHRQuery(OBContext db, bool includeSoftDeleted = false, string filter = "")
        {
            filter = filter.ToUpper();

            var usernames = Roles.GetUsersInRole("HR");

            var result = db.User.Where(x => usernames.Contains(x.Name)).Where(a => a.Name.ToUpper().Contains(filter) || a.Mail.ToUpper().Contains(filter));

            if (!includeSoftDeleted)
            {
                result = result.Where(a => a.IsDeleted == false);
            }
            return result;
        }
        // end hr

        // candidate
        public static List<User> GetCandidateList(string role, bool includeSoftDeleted = false, string filter = "")
        {
            using (var db = new OBContext())
            {
                return GetCandidateQuery(db, includeSoftDeleted, filter).ToList();
            }
        }

        public static IQueryable<User> GetCandidateQuery(OBContext db, bool includeSoftDeleted = false, string filter = "")
        {
            filter = filter.ToUpper();

            var usernames = Roles.GetUsersInRole("Candidate");

            var curUserHRClients = db.User.Where(a => a.Id == WebSecurity.CurrentUserId).Single().HRClients.Select(a => a.Id);

            var result = from a in db.User
                         join b in db.Employee
                         on a.Id equals b.UserId
                         where usernames.Contains(a.Name) && curUserHRClients.Contains(b.ClientId)
                         select a;

            result = result.Where(a => a.Name.ToUpper().Contains(filter) || a.Mail.ToUpper().Contains(filter));

            if (!includeSoftDeleted)
            {
                result = result.Where(a => a.IsDeleted == false);
            }
            return result;
        }
        // end candidate
        //end user

        //certificate
        public static IQueryable<Certificate> GetCertificateQuery(OBContext db, bool includeSoftDeleted = false, string keyword = "")
        {
            keyword = keyword.ToUpper();

            var result = db.Certificate.Where(a => a.Name.Contains(keyword));

            if (!includeSoftDeleted)
            {
                result = result.Where(a => a.IsDeleted == false);
            }

            return result;
        }
        //end certificate

        //client
        public static IQueryable<Client> GetClientQuery(OBContext db, bool includeSoftDeleted = false, string keyword = "")
        {
            keyword = keyword.ToUpper();

            var result = db.Client.Where(a => a.Name.Contains(keyword));

            if (!includeSoftDeleted)
            {
                result = result.Where(a => a.IsDeleted == false);
            }

            return result;
        }
        // hrClient
        public static List<Client> GetHRClientList(int userId, string filter = "")
        {
            using (var db = new OBContext())
            {
                return GetHRClientQuery(db, userId, filter).ToList();
            }
        }

        public static IQueryable<Client> GetHRClientQuery(OBContext db, int userId, string filter = "")
        {
            filter = filter.ToUpper();

            var userHRClients = db.User.Where(a => a.Id == userId).Single().HRClients.Select(a => a.Id);

            var result = db.Client.Where(a => a.Name.ToUpper().Contains(filter)).Where(a => userHRClients.Contains(a.Id));

            if (true)
            {
                result = result.Where(a => a.IsDeleted == false);
            }
            return result;
        }
        // end hrClient

        // hrAdminClient
        public static List<Client> GetHRAdminClientList(int userId, string filter = "")
        {
            using (var db = new OBContext())
            {
                return GetHRAdminClientQuery(db, userId, filter).ToList();
            }
        }

        public static IQueryable<Client> GetHRAdminClientQuery(OBContext db, int userId, string filter = "")
        {
            filter = filter.ToUpper();

            var userHRAdminClients = db.User.Where(a => a.Id == userId).Single().HRAdminClients.Select(a => a.Id);

            var result = db.Client.Where(a => a.Name.ToUpper().Contains(filter)).Where(a => userHRAdminClients.Contains(a.Id));

            if (true)
            {
                result = result.Where(a => a.IsDeleted == false);
            }
            return result;
        }
        // end hrAdminClient

        //end client


        //ClientCitySupplierHukou
        // hrAdminClient
        public static List<ClientCitySupplierHukou> GetHRAdminClientCitySupplierHukouList(int hrAdminUserId, bool includeSoftDeleted = false, string filter = "")
        {
            using (var db = new OBContext())
            {
                return GetHRAdminClientCitySupplierHukouQuery(db, hrAdminUserId, includeSoftDeleted, filter).ToList();
            }
        }

        public static IQueryable<ClientCitySupplierHukou> GetHRAdminClientCitySupplierHukouQuery(OBContext db, int hrAdminUserId, bool includeSoftDeleted = false, string filter = "")
        {
            filter = filter.ToUpper();

            var userHRAdminClients = db.User.Where(a => a.Id == hrAdminUserId).Single().HRAdminClients.Select(a => a.Id);

            var hukouNameList = Enum.GetNames(typeof(HukouType)).Where(a => a.ToUpper().Contains(filter));

            var hukouList = new List<HukouType> { };

            foreach (var item in hukouNameList)
            {
                hukouList.Add((HukouType)(Enum.Parse(typeof(HukouType), item, true)));
            }

            var result = db.ClientCitySupplierHukou.Where(a => a.Client.Name.ToUpper().Contains(filter) || a.City.Name.ToUpper().Contains(filter) || a.Supplier.Name.ToUpper().Contains(filter) || hukouList.Contains(a.HukouType)).Where(a => userHRAdminClients.Contains(a.Client.Id));

            if (!includeSoftDeleted)
            {
                result = result.Where(a => a.IsDeleted == false);
            }
            return result;
        }
        // end hrAdminClient
        //end ClientCitySupplierHukou

        //document
        // HRAdmin document
        public static List<Document> GetHRAdminDocumentList(int userId)
        {
            using (var db = new OBContext())
            {
                return GetHRAdminDocumentQuery(db, userId).ToList();
            }
        }

        public static IQueryable<Document> GetHRAdminDocumentQuery(OBContext db, int userId, bool includeSoftDeleted = false, string filter = "")
        {
            filter = filter.ToUpper();

            var user = db.User.Find(userId);
            if (user == null)
            {
                return null;
            }
            var clientIds = user.HRAdminClients.Select(a => a.Id);
            var result = db.Document.Where(a => clientIds.Contains(a.ClientId));

            result = result.Where(a => a.Client.Name.ToUpper().Contains(filter) || a.Name.ToUpper().Contains(filter));

            if (!includeSoftDeleted)
            {
                result = result.Where(a => a.IsDeleted == false);
            }
            return result;
        }
        // end HRAdmin document
        // client document
        public static List<Document> GetClientDocumentList(int clientId)
        {
            using (var db = new OBContext())
            {
                return GetClientDocumentQuery(db, clientId).ToList();
            }
        }

        public static IQueryable<Document> GetClientDocumentQuery(OBContext db, int clientId, bool includeSoftDeleted = false, string filter = "")
        {
            filter = filter.ToUpper();

            var result = db.Document.Where(a => a.ClientId == clientId);

            result = result.Where(a => a.Client.Name.ToUpper().Contains(filter) || a.Name.ToUpper().Contains(filter));

            if (!includeSoftDeleted)
            {
                result = result.Where(a => a.IsDeleted == false);
            }
            return result;
        }
        // end client document
        //end document

        //ClientPensionCityDocument
        public static IQueryable<ClientPensionCityDocument> GetHRAdminClientPensionCityDocumentQuery(OBContext db, int userId, bool includeSoftDeleted = false, string filter = "")
        {
            filter = filter.ToUpper();

            var user = db.User.Find(userId);
            if (user == null)
            {
                return null;
            }
            var clientIds = user.HRAdminClients.Select(a => a.Id);
            var result = db.ClientPensionCityDocument.Where(a => clientIds.Contains(a.ClientId));

            result = result.Where(a => a.Client.Name.ToUpper().Contains(filter) || a.PensionCity.Name.ToUpper().Contains(filter));

            if (!includeSoftDeleted)
            {
                result = result.Where(a => a.IsDeleted == false);
            }
            return result;
        }
        //end ClientPensionCityDocument

        //city
        // client pension city
        public static List<City> GetClientPensionCityList(int clientId)
        {
            using (var db = new OBContext())
            {
                return GetClientPensionCityQuery(db, clientId).ToList();
            }
        }

        public static IQueryable<City> GetClientPensionCityQuery(OBContext db, int clientId)
        {
            var client = db.Client.Where(a => a.Id == clientId).SingleOrDefault();
            if (client == null)
            {
                return null;
            }
            var clientCities = client.PensionCities.Select(a => a.Id);
            var result = GetCityQuery(db).Where(a => clientCities.Contains(a.Id));
            return result;
        }
        // end client pension city

        public static List<City> GetCityList(bool includeSoftDeleted = false, string filter = "")
        {
            using (var db = new OBContext())
            {
                return GetCityQuery(db, includeSoftDeleted, filter).ToList();
            }
        }

        public static IQueryable<City> GetCityQuery(OBContext db, bool includeSoftDeleted = false, string filter = "")
        {
            filter = filter.ToUpper();

            var result = db.City.Where(a => a.Name.ToUpper().Contains(filter));

            if (!includeSoftDeleted)
            {
                result = result.Where(a => a.IsDeleted == false);
            }
            return result;
        }
        // end city

        //supplier
        public static List<Supplier> GetSupplierList(bool includeSoftDeleted = false, string filter = "")
        {
            using (var db = new OBContext())
            {
                return GetSupplierQuery(db, includeSoftDeleted, filter).ToList();
            }
        }

        public static IQueryable<Supplier> GetSupplierQuery(OBContext db, bool includeSoftDeleted = false, string filter = "")
        {
            filter = filter.ToUpper();

            var result = db.Supplier.Where(a => a.Name.ToUpper().Contains(filter));

            if (!includeSoftDeleted)
            {
                result = result.Where(a => a.IsDeleted == false);
            }
            return result;
        }
        // end supplier

        //accumulation type
        public static List<AccumulationType> GetAccumulationTypeList(bool includeSoftDeleted = false, string filter = "")
        {
            using (var db = new OBContext())
            {
                return GetAccumulationTypeQuery(db, includeSoftDeleted, filter).ToList();
            }
        }

        public static IQueryable<AccumulationType> GetAccumulationTypeQuery(OBContext db, bool includeSoftDeleted = false, string filter = "")
        {
            filter = filter.ToUpper();

            var result = db.AccumulationType.Where(a => a.Name.ToUpper().Contains(filter));

            if (!includeSoftDeleted)
            {
                result = result.Where(a => a.IsDeleted == false);
            }
            return result;
        }
        //end accumulation type

        //pension type
        public static List<PensionType> GetPensionTypeList(bool includeSoftDeleted = false, string filter = "")
        {
            using (var db = new OBContext())
            {
                return GetPensionTypeQuery(db, includeSoftDeleted, filter).ToList();
            }
        }

        public static IQueryable<PensionType> GetPensionTypeQuery(OBContext db, bool includeSoftDeleted = false, string filter = "")
        {
            filter = filter.ToUpper();

            var result = db.PensionType.Where(a => a.Name.ToUpper().Contains(filter));

            if (!includeSoftDeleted)
            {
                result = result.Where(a => a.IsDeleted == false);
            }
            return result;
        }
        //end pension type

        //assurance
        // hradmin assurance
        public static IQueryable<Assurance> GetHRAdminAssuranceQuery(OBContext db, int hrAdminUserId, bool includeSoftDeleted = false, string filter = "")
        {
            filter = filter.ToUpper();

            var result = db.Assurance.Where(a => a.Name.ToUpper().Contains(filter) || a.Client.Name.ToUpper().Contains(filter));

            result = result.Where(a => db.User.Where(c => c.Id == hrAdminUserId).FirstOrDefault().HRAdminClients.Select(b => b.Id).Contains(a.ClientId));

            if (!includeSoftDeleted)
            {
                result = result.Where(a => a.IsDeleted == false);
            }
            return result;
        }
        // end hradmin assurance
        //end assurance


        //CustomField
        public static IQueryable<CustomField> GetHRAdminCustomFieldQuery(OBContext db, int userId, bool includeSoftDeleted = false, string filter = "")
        {
            filter = filter.ToUpper();

            var user = db.User.Find(userId);
            if (user == null)
            {
                return null;
            }
            var clientIds = user.HRAdminClients.Select(a => a.Id);
            var result = db.CustomField.Where(a => clientIds.Contains(a.ClientId));

            result = result.Where(a => a.Client.Name.ToUpper().Contains(filter));

            if (!includeSoftDeleted)
            {
                result = result.Where(a => a.IsDeleted == false);
            }
            return result;
        }
        //end CustomField

        //Department
        public static IQueryable<Department> GetHRAdminDepartmentQuery(OBContext db, int userId, bool includeSoftDeleted = false, string filter = "")
        {
            filter = filter.ToUpper();

            var user = db.User.Find(userId);
            if (user == null)
            {
                return null;
            }
            var clientIds = user.HRAdminClients.Select(a => a.Id);
            var result = db.Department.Where(a => clientIds.Contains(a.ClientId));

            result = result.Where(a => a.Client.Name.ToUpper().Contains(filter) || a.Name.ToUpper().Contains(filter));

            if (!includeSoftDeleted)
            {
                result = result.Where(a => a.IsDeleted == false);
            }
            return result;
        }
        //end Department

        //Level
        public static IQueryable<Level> GetHRAdminLevelQuery(OBContext db, int userId, bool includeSoftDeleted = false, string filter = "")
        {
            filter = filter.ToUpper();

            var user = db.User.Find(userId);
            if (user == null)
            {
                return null;
            }
            var clientIds = user.HRAdminClients.Select(a => a.Id);
            var result = db.Level.Where(a => clientIds.Contains(a.ClientId));

            result = result.Where(a => a.Client.Name.ToUpper().Contains(filter) || a.Name.ToUpper().Contains(filter));

            if (!includeSoftDeleted)
            {
                result = result.Where(a => a.IsDeleted == false);
            }
            return result;
        }
        //end Level

        //Weight
        public static IQueryable<Weight> GetHRAdminWeightQuery(OBContext db, int userId, bool includeSoftDeleted = false, string filter = "")
        {
            filter = filter.ToUpper();

            var user = db.User.Find(userId);
            if (user == null)
            {
                return null;
            }
            var clientIds = user.HRAdminClients.Select(a => a.Id);
            var result = db.Weight.Where(a => clientIds.Contains(a.WeightClientId.Value));

            result = result.Where(a => a.WeightClient.Name.ToUpper().Contains(filter));

            if (!includeSoftDeleted)
            {
                result = result.Where(a => a.IsDeleted == false);
            }
            return result;
        }
        //end Weight

        //Zhangtao
        public static IQueryable<Zhangtao> GetHRAdminZhangtaoQuery(OBContext db, int userId, bool includeSoftDeleted = false, string filter = "")
        {
            filter = filter.ToUpper();

            var user = db.User.Find(userId);
            if (user == null)
            {
                return null;
            }
            var clientIds = user.HRAdminClients.Select(a => a.Id);
            var result = db.Zhangtao.Where(a => clientIds.Contains(a.ClientId));

            result = result.Where(a => a.Client.Name.ToUpper().Contains(filter));

            if (!includeSoftDeleted)
            {
                result = result.Where(a => a.IsDeleted == false);
            }
            return result;
        }
        //end Zhangtao

        //Position
        public static IQueryable<Position> GetHRAdminPositionQuery(OBContext db, int userId, bool includeSoftDeleted = false, string filter = "")
        {
            filter = filter.ToUpper();

            var user = db.User.Find(userId);
            if (user == null)
            {
                return null;
            }
            var clientIds = user.HRAdminClients.Select(a => a.Id);
            var result = db.Position.Where(a => clientIds.Contains(a.ClientId));

            result = result.Where(a => a.Client.Name.ToUpper().Contains(filter) || a.Name.ToUpper().Contains(filter));

            if (!includeSoftDeleted)
            {
                result = result.Where(a => a.IsDeleted == false);
            }
            return result;
        }
        //end Position

        //contractType
        // hradmin contractType
        public static IQueryable<ContractType> GetHRAdminContractTypeQuery(OBContext db, int hrAdminUserId, bool includeSoftDeleted = false, string filter = "")
        {
            filter = filter.ToUpper();

            var result = db.ContractType.Where(a => a.Name.ToUpper().Contains(filter) || a.Client.Name.ToUpper().Contains(filter));

            result = result.Where(a => db.User.Where(c => c.Id == hrAdminUserId).FirstOrDefault().HRAdminClients.Select(b => b.Id).Contains(a.ClientId));

            if (!includeSoftDeleted)
            {
                result = result.Where(a => a.IsDeleted == false);
            }
            return result;
        }
        // end hradmin contractType
        //end contractType

        //budgetCenter
        // hradmin budgetCenter
        public static IQueryable<BudgetCenter> GetHRAdminBudgetCenterQuery(OBContext db, int hrAdminUserId, bool includeSoftDeleted = false, string filter = "")
        {
            filter = filter.ToUpper();

            var result = db.BudgetCenter.Where(a => a.Name.ToUpper().Contains(filter) || a.Client.Name.ToUpper().Contains(filter));

            result = result.Where(a => db.User.Where(c => c.Id == hrAdminUserId).FirstOrDefault().HRAdminClients.Select(b => b.Id).Contains(a.ClientId));

            if (!includeSoftDeleted)
            {
                result = result.Where(a => a.IsDeleted == false);
            }
            return result;
        }
        // end hradmin budgetCenter
        //end budgetCenter

        //end common get record
    }
}