using OB.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OB.Models.ViewModel
{
    public class EditEmployeeEducation
    {
        public EditEmployeeEducation()
        {
            EditEducations = new List<EditEducation> { };
        }

        [HiddenInput(DisplayValue = false)]
        public int EmployeeId { get; set; }

        public virtual ICollection<EditEducation> EditEducations { get; set; }
    }
}