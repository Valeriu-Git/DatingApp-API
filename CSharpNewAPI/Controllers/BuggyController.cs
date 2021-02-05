using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using CSharpNewAPI.Models;
using CSharpNewAPI.Database;
using CSharpNewAPI.Utils;
using Json.Net;

namespace CSharpNewAPI.Controllers
{
    public class BuggyController : BaseApiController
    {
        public DatabaseContext DatabaseContext { get; }
        public BuggyController(DatabaseContext context)
        {
            this.DatabaseContext = context;
        }

        [Authorize]
        [HttpGet("auth")]
        public ActionResult<string> GetAuthError()
        {
            return "authentication error";
        }


        [HttpGet("not-found")]
        public async Task<ActionResult<AppUser>> GetNotFoundError()
        {
           await CustomJsonParser.GetDataFromJson(DatabaseContext);
           AppUser user = DatabaseContext.Find<AppUser>(-1);
            if (user == null)
            {
                return NotFound();
            }
            Console.WriteLine("test");
            return user;
        }

        [HttpGet("null-exception")]
        public ActionResult<string> GetNullReferenceException()
        {
            AppUser user = DatabaseContext.Find<AppUser>(-1);
            return user.ToString();
        }

        [HttpGet("bad-request")]
        public ActionResult<string> GetBadRequest()
        {
            return BadRequest();
        }
    }
}
