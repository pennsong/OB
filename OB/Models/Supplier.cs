using OB.Models.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace OB.Models
{
    public class Supplier : SoftDelete
    {
        public int Id { get; set; }
        [Required]
        [DisplayName("名称")]
        [MaxLength(50)]
        public string Name { get; set; }

        [DisplayName("社保")]
        public bool IsPension { get; set; }

        [DisplayName("公积金")]
        public bool IsAccumulation { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }
}