using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using CSharpNewAPI.BaseClasses;
using CSharpNewAPI.Extensions;
using CSharpNewAPI.Interfaces;

namespace CSharpNewAPI.Models
{
    [Table("AppUsers")]
    public class AppUser:BaseUser,IAppUser
    {
        public string UserName { get; set; }

        public DateTime DateOfBirth { get; set; }
        public byte[] PasswordHash { get; set; }

        public byte[] PasswordSalt { get; set; }

        public ICollection<Photo> Photos { get; set; }
        
        public int CalculateAge()
        {
            return DateOfBirth.CalculateAge();
        }
    }
}
