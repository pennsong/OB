using FrameLog;
using OB.Models.Base;
using OB.Models.DAL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace OB.Models
{
    public class EmployeeDoc : SoftDelete, IHasLoggingReference
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        [DisplayName("资料名称")]
        public int DocumentId { get; set; }

        [DisplayName("上传图片")]
        public string ImgPath { get; set; }

        public virtual Employee Employee { get; set; }
        public virtual Document Document { get; set; }

        //FrameLog related
        public object Reference
        {
            get { return Id; }
        }

        public override string ToString()
        {
            if (Employee == null)
            {
                using (var db = new OBContext())
                {
                    Employee = db.Employee.Find(EmployeeId);
                    Document = db.Document.Find(DocumentId);
                }
            }
            return Employee + "_" + Document;
        }
    }
}