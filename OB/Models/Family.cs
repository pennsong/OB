using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace OB.Models
{
    public class Family
    {
        public int Id { get; set; }

        public int EmployeeId { get; set; }
        [Required]
        [MaxLength(100)]
        [DisplayName("姓名")]
        public string Name { get; set; }

        [Required]
        [MaxLength(100)]
        [DisplayName("关系")]
        public string Relation { get; set; }

        [DisplayName("性别")]
        public Sex Sex { get; set; }

        [MaxLength(100)]
        [DisplayName("公司")]
        public string Company { get; set; }

        [MaxLength(100)]
        [DisplayName("职位")]
        public string Position { get; set; }

        [MaxLength(100)]
        [DisplayName("联系电话")]
        public string Phone { get; set; }

        public virtual Employee Employee { get; set; }
    }
}