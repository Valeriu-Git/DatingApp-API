using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using CSharpNewAPI.Interfaces;

namespace CSharpNewAPI.DTOS
{
    public class RegisterUserDto:IAppUser
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
