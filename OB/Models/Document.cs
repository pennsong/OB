using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OB.Models
{
    public class Document
    {
        [HiddenInput(DisplayValue = false)]
        public int Id { get; set; }

        [HiddenInput(DisplayValue = true)]
        [DisplayName("客户")]
        public int ClientId { get; set; }

        [Required]
        [DisplayName("名称")]
        [MaxLength(50)]
        public string Name { get; set; }

        [Required]
        [DisplayName("权重")]
        public int Weight { get; set; }

        public virtual Client Client { get; set; }
    }
}