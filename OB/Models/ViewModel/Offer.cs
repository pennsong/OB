using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace OB.Models.ViewModel
{
    public class Offer
    {
        public int EmployeeId { get; set; }

        [Timestamp]
        public Byte[] Timestamp { get; set; }

        [Required]
        [MaxLength(1000)]
        [DisplayName("Offer")]
        [DataType(DataType.MultilineText)]
        public string Content { get; set; }
    }
}