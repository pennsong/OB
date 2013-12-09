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
    public class Education : SoftDelete, IHasLoggingReference
    {
        public int Id { get; set; }

        public int EmployeeId { get; set; }
        [Required]
        [MaxLength(100)]
        [DisplayName("学校")]
        public string School { get; set; }
        [Required]
        [MaxLength(100)]
        [DisplayName("专业")]
        public string Major { get; set; }

        [DisplayName("学历")]
        public Degree Degree { get; set; }

        [DisplayName("开始时间")]
        public DateTime Begin { get; set; }

        [DisplayName("结束时间")]
        public DateTime? End { get; set; }

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
            return Employee + "_" + School + "_" + Major;
        }
    }
}