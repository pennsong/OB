using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OB.Models.ViewModel
{
    public class EditSingleEmployeeDoc
    {
        public int EmployeeDocId { get; set; }
        public int DocumentId { get; set; }
        public string DocumentName { get; set; }
        public string ImgPath { get; set; }
    }
}