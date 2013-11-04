using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OB.Models.ViewModel
{
    public class EditEducation
    {
        [HiddenInput(DisplayValue = false)]
        public int EducationId { get; set; }

        [Required]
        [DisplayName("学校")]
        public string School { get; set; }
        [Required]
        [DisplayName("专业")]
        public string Major { get; set; }
        [Required]
        [DisplayName("学历")]
        public int DegreeId { get; set; }

        private DateTime _Begin = DateTime.Now;
        [DisplayName("开始时间")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime Begin
        {
            get { return _Begin; }
            set { _Begin = value; }
        }

        private DateTime? _End;
        [DisplayName("结束时间")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? End
        {
            get { return _End; }
            set { _End = value; }
        }

        public bool Delete { get; set; }
    }
}