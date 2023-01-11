using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Application.ViewModel
{
    public class SignUpViewModel
    {
        [Required, MinLength(3)]
        public string? Name { get; set; }
        [Required, MinLength(3)]
        public string? LastName { get; set; }
        [Required, DataType(DataType.EmailAddress)]
        public string EMail { get; set; }
        [Required, MinLength(10)]
        public string? UserName { get; set; }
        public string? Age { get; set; }
        public string? Adress { get; set; }
        public IFormFile? FileImg { get; set; }
        [Required, DataType(DataType.Password), MinLength(10)]
        public string? Password { get; set; }
        [Required, Compare("Password")]
        public string? RePassword { get; set; }
    }
}
