using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace OB.Models
{
    public class Supplier
    {
        public int Id { get; set; }
        [Required]
        [DisplayName("名称")]
        [MaxLength(50)]
        public string Name { get; set; }

        public bool IsPension { get; set; }

        public bool IsAccumulation { get; set; }
    }
}