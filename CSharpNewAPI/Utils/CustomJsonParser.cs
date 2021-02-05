using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using CSharpNewAPI.Database;
using CSharpNewAPI.Models;
using Json.Net;
using Newtonsoft.Json;

namespace CSharpNewAPI.Utils
{
    public static class CustomJsonParser
    {
        public  static async Task GetDataFromJson(DatabaseContext context)
        {
            ICollection<AppUser> users = new List<AppUser>();
            using (StreamReader reader=new StreamReader("Database/SeedData/data.json"))
            {
                string json = reader.ReadToEnd();
                users = JsonConvert.DeserializeObject<List<AppUser>>(json);
                foreach (var user in users)
                {
                    HMACSHA512 hmac = new HMACSHA512();
                    user.PasswordHash = hmac.ComputeHash(ASCIIEncoding.UTF8.GetBytes(user.UserName));
                    user.PasswordSalt = hmac.Key;
                    await context.AddAsync(user);
                }
                await context.SaveChangesAsync();
            }
        }
    }
}