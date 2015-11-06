using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Catalogue.Controllers
{
    public class LoginAccount
    {
        [Required, StringLength(128), Display(Name = "Username")]
        public string UserName { get; set; }

        [Required, StringLength(128), Display(Name = "Password"), DataType(DataType.Password)]
        public string Password { get; set; }
    }
}