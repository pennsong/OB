using OB.Models.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace OB.Models
{
    public class BudgetCenter : SoftDelete
    {
        public int Id { get; set; }
        [DisplayName("客户")]
        public int ClientId { get; set; }
        [Required]
        [DisplayName("名称")]
        [MaxLength(50)]
        public string Name { get; set; }

        public virtual Client Client { get; set; }
        public virtual ICollection<Employee> Employees { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }
}