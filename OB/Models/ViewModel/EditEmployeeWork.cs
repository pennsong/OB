using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OB.Models.ViewModel
{
    public class EditEmployeeWork
    {
        public EditEmployeeWork()
        {
            EditWorks = new List<EditWork> { };
        }

        [HiddenInput(DisplayValue = false)]
        public int EmployeeId { get; set; }

        public virtual ICollection<EditWork> EditWorks { get; set; }
    }
}