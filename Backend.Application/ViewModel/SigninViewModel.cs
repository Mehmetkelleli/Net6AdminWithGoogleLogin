using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Application.ViewModel
{
    public class SigninViewModel
    {
        [Required,MinLength(3)]
        public string EMail { get; set; }
        [Required,MinLength(5),DataType(DataType.Password)]
        public string Password { get; set; }
        public bool RememberMe { get; set; }
    }
}
