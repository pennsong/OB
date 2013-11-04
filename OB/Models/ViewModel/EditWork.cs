using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OB.Models
{
    public class EditWork
    {
        [HiddenInput(DisplayValue = false)]
        public int WorkId { get; set; }

        [Required]
        [MaxLength(100)]
        [DisplayName("公司")]
        public string Company { get; set; }
        [Required]
        [MaxLength(100)]
        [DisplayName("职位")]
        public string Position { get; set; }

        private DateTime _Begin = DateTime.Now;
        [DisplayName("开始时间")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime Begin
        {
            get { return _Begin; }
            set { _Begin = value; }
        }

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
        [DataType(DataType.MultilineText)]
        public string Note { get; set; }

        public bool Delete { get; set; }
    }
}