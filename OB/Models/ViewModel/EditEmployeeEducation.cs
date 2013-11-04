using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OB.Models
{
    public class EditEmployeeEducation
    {
        [HiddenInput(DisplayValue = false)]
        public int EmployeeId { get; set; }

        public virtual ICollection<EditEducation> EditEducations { get; set; }
    }
}