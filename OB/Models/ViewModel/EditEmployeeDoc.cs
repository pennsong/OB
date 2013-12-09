using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OB.Models.ViewModel
{
    public class EditEmployeeDoc
    {
        public EditEmployeeDoc()
        {
            EditSingleEmployeeDocs = new List<EditSingleEmployeeDoc> { };
        }

        [HiddenInput(DisplayValue = false)]
        public int EmployeeId { get; set; }

        public virtual ICollection<EditSingleEmployeeDoc> EditSingleEmployeeDocs { get; set; }
    }
}