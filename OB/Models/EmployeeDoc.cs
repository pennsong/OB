using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace OB.Models
{
    public class EmployeeDoc
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        [DisplayName("资料名称")]
        public int DocumentId { get; set; }

        [DisplayName("上传图片")]
        public string ImgPath { get; set; }

        public virtual Employee Employee { get; set; }
        public virtual Document Document { get; set; }

    }
}