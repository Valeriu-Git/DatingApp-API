using CSharpNewAPI.Database;
using CSharpNewAPI.DTOS;
using CSharpNewAPI.Interfaces;
using CSharpNewAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace CSharpNewAPI.Controllers
{
    public class AccountController : BaseApiController
    {
        public DatabaseContext Context { get; }
        public ITokenService TokenService { get; }
        public ITokenService TokenService2 { get; }

        public AccountController(DatabaseContext context, ITokenService tokenService, ITokenService tokenService2)
        {
            this.Context = context;
            this.TokenService = tokenService;
            this.TokenService2 = tokenService2;
        }

        [HttpPost("register")]
        public async Task<ActionResult<AppUser>> RegisterUser([FromBody] RegisterUserDTO addUser)
        {
            if (await DoesUserExist(addUser))
            {
                return BadRequest("Username is already taken");
            }
            using var hmac = new HMACSHA512();
            AppUser user = new AppUser
            {
                UserName = addUser.UserName,
                PasswordHash = hmac.ComputeHash(Encoding.ASCII.GetBytes(addUser.Password)),
                PasswordSalt = hmac.Key
            };
            await this.Context.AddAsync<AppUser>(user);
            await this.Context.SaveChangesAsync();
            return StatusCode(201);
        }

        [HttpPost("login")]
        public async Task<ActionResult<SuccessLoginUser>> LoginUser([FromBody] AttemptLoginUserDTO user)
        {
            AppUser userFromDB;

            if ((userFromDB = await GetUserByUserName(user)) != null)
            {
                string token = TokenService.GenerateToken(userFromDB);
                if (CheckCredentials(user, userFromDB))
                {
                    return new SuccessLoginUser()
                    {
                        UserName = userFromDB.UserName,
                        Token = token
                    };
                }
                return BadRequest("WRONG PASSWORD");
            }
            return BadRequest("No user with this username exist");
        }

        private async Task<AppUser> GetUserByUserName(AttemptLoginUserDTO credentials)
        {
            AppUser user = await Context.Users.FirstOrDefaultAsync(u => u.UserName.Equals(credentials.UserName));
            return user;
        }

        private bool CheckCredentials(AttemptLoginUserDTO userDTO, AppUser userFromDB)
        {

            using HMACSHA512 hmac = new HMACSHA512(userFromDB.PasswordSalt);
            byte[] hashedPassword = hmac.ComputeHash(Encoding.ASCII.GetBytes(userDTO.Password));
            if (hashedPassword.SequenceEqual(userFromDB.PasswordHash))
            {
                return true;
            }
            return false;
        }

        private async Task<bool> DoesUserExist(RegisterUserDTO addUser)
        {
            return await Context.Users.AnyAsync(user => user.UserName.ToLower().Equals(addUser.UserName.ToLower()));
        }
    }
}
