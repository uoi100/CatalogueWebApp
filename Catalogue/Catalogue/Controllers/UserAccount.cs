using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Catalogue.Controllers
{
    public class UserAccount
    {
        [Required, StringLength(128), Display(Name="Username")]
        public string UserName { get; set; }

        [Required, StringLength(128), Display(Name = "Password"), DataType(DataType.Password)]
        public string Password { get; set; }

        [Required, StringLength(128), Display(Name = "Confirm Password"), DataType(DataType.Password), Compare("Password")]
        public string ConfirmPassword { get; set; }

        [Required, StringLength(128), Display(Name = "Email"), DataType(DataType.EmailAddress)]
        public string Email { get; set; }
    }
}