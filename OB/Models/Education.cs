using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace OB.Models
{
    public class Education
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
    }
}