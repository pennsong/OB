using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Globalization;
using System.Web.Security;

namespace OB.Models.ViewModel
{
    public class CreateCandidate
    {
        [Required]
        [Display(Name = "用户名")]
        public string UserName { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "{0} 必须至少包含 {2} 个字符。", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "密码")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "确认密码")]
        [Compare("Password", ErrorMessage = "密码和确认密码不匹配。")]
        public string ConfirmPassword { get; set; }

        [Required]
        [Display(Name = "邮箱")]
        public string Mail { get; set; }

        [Required]
        [Display(Name = "中文名")]
        public string ChineseName { get; set; }

        [Display(Name = "所属客户")]
        public int ClientId { get; set; }

        [Display(Name = "社保城市")]
        public int? PensionCityId { get; set; }
    }
}