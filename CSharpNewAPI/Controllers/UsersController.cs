using CSharpNewAPI.Database;
using CSharpNewAPI.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CSharpNewAPI.Controllers
{
    public class UsersController : BaseApiController
    {
        public DatabaseContext Context { get; set; }
        public UsersController(DatabaseContext context)
        {
            this.Context = context;
        }

        [HttpGet("getAll")]
        public async Task<ActionResult<List<AppUser>>> GetUsers()
        {
            Task<List<AppUser>> usersTask = new Task<List<AppUser>>(() => this.Context.Users.ToList());
            usersTask.Start();
            List<AppUser> result = await usersTask;
            return result;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<AppUser>> GetUser(int id)
        {
            Task<AppUser> getUserTask = new Task<AppUser>(() => this.Context.Users.Find(id));
            getUserTask.Start();
            AppUser user = await getUserTask;
            return user;
        }
    }
}
