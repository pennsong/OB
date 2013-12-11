using FrameLog;
using FrameLog.Contexts;
using OB.Models.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Globalization;
using System.Web.Security;
using System.Linq;
using System.ComponentModel;

namespace OB.Models
{
    public class UsersContext : DbContext
    {
        public UsersContext()
            : base("OB")
        {
        }

        public DbSet<User> User { get; set; }
    }

    public class User : SoftDelete, IHasLoggingReference
    {
        public User()
        {
            HRAdminClients = new List<Client> { };
            HRClients = new List<Client> { };
        }

        [DisplayName("ID")]
        public int Id { get; set; }
        [Required]
        [DisplayName("用户名")]
        public string Name { get; set; }
        [Required]
        [DisplayName("电子邮件")]
        public string Mail { get; set; }

        [DisplayName("客户列表")]
        public virtual ICollection<Client> HRAdminClients { get; set; }
        [DisplayName("客户列表")]
        public virtual ICollection<Client> HRClients { get; set; }

        public List<Client> GetHRAdminClients()
        {
            return HRAdminClients.Where(a => a.IsDeleted == false).ToList();
        }

        public List<Client> GetHRClients()
        {
            return HRClients.Where(a => a.IsDeleted == false).ToList();
        }


        //FrameLog related
        public object Reference
        {
            get { return Id; }
        }

        public override string ToString()
        {
            return Name;
        }
    }

    public class RegisterExternalLoginModel
    {
        [Required]
        [Display(Name = "用户名")]
        public string UserName { get; set; }

        public string ExternalLoginData { get; set; }
    }

    public class LocalPasswordModel
    {
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "当前密码")]
        public string OldPassword { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "{0} 必须至少包含 {2} 个字符。", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "新密码")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "确认新密码")]
        [Compare("NewPassword", ErrorMessage = "新密码和确认密码不匹配。")]
        public string ConfirmPassword { get; set; }

        [Required]
        [Display(Name = "邮箱")]
        public string Mail { get; set; }
    }

    public class LoginModel
    {
        [Required]
        [Display(Name = "用户名")]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "密码")]
        public string Password { get; set; }

        [Display(Name = "记住我?")]
        public bool RememberMe { get; set; }
    }

    public class RegisterModel
    {
        [Required]
        [Display(Name = "用户名")]
        public string UserName { get; set; }

        //[Required]
        //[StringLength(100, ErrorMessage = "{0} 必须至少包含 {2} 个字符。", MinimumLength = 6)]
        //[DataType(DataType.Password)]
        //[Display(Name = "密码")]
        //public string Password { get; set; }

        //[DataType(DataType.Password)]
        //[Display(Name = "确认密码")]
        //[Compare("Password", ErrorMessage = "密码和确认密码不匹配。")]
        //public string ConfirmPassword { get; set; }

        [Required]
        [Display(Name = "邮箱")]
        public string Mail { get; set; }
    }

    public class ExternalLogin
    {
        public string Provider { get; set; }
        public string ProviderDisplayName { get; set; }
        public string ProviderUserId { get; set; }
    }
}
