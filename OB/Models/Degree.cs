using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace OB.Models
{
    public class Degree
    {
        public int Id { get; set; }
        [Required]
        [DisplayName("名称")]
        [MaxLength(50)]
        public string Name { get; set; }
    }
}