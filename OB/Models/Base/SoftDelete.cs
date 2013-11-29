using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OB.Models.Base
{
    public class SoftDelete
    {
        public SoftDelete()
        {
            IsDeleted = false;
        }
        public bool IsDeleted { get; set; }
    }
}