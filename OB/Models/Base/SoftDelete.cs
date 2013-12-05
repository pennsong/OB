using System;
using System.Collections.Generic;
using System.ComponentModel;
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

        [DisplayName("已删除")]
        public bool IsDeleted { get; set; }
    }
}