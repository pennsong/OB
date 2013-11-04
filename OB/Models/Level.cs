using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace OB.Models
{
    public class Level
    {
        public int Id { get; set; }
        public int ClientId { get; set; }
        [Required]
        [DisplayName("名称")]
        [MaxLength(50)]
        public string Name { get; set; }

        public virtual Client Client { get; set; }
    }
}