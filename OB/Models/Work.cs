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
    public class Work : SoftDelete, IHasLoggingReference
    {
        public int Id { get; set; }

        public int EmployeeId { get; set; }
        [Required]
        [MaxLength(100)]
        [DisplayName("公司")]
        public string Company { get; set; }
        [Required]
        [MaxLength(100)]
        [DisplayName("职位")]
        public string Position { get; set; }

        [DisplayName("开始时间")]
        public DateTime Begin { get; set; }

        [DisplayName("结束时间")]
        public DateTime? End { get; set; }

        [Required]
        [MaxLength(100)]
        [DisplayName("联系人")]
        public string Contact { get; set; }
        [Required]
        [MaxLength(100)]
        [DisplayName("联系人电话")]
        public string Phone { get; set; }

        [MaxLength(1000)]
        [DisplayName("备注")]
        public string Note { get; set; }

        public virtual Employee Employee { get; set; }

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
                }
            }
            return Employee + "_" + Company;
        }
    }
}