using CSharpNewAPI.Database;
using CSharpNewAPI.DTOS;
using CSharpNewAPI.Interfaces;
using CSharpNewAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using CSharpNewAPI.BaseClasses;

namespace CSharpNewAPI.Controllers
{
    public class AccountController : BaseApiController
    {
        public ITokenService TokenService { get; }
        private readonly IAppUsersRepository _usersRepository;
        private readonly IMapper _mapper;

        public AccountController(IAppUsersRepository repository, ITokenService tokenService,IMapper mapper)
        {
            _mapper = mapper;
            _usersRepository = repository;
            TokenService = tokenService;
        }

        [HttpPost("register")]
        public async Task<ActionResult<AppUser>> RegisterUser([FromBody] RegisterUserDto addUser)
        {
            if (await _usersRepository.DoesAppUserExistAsync(addUser))
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
            await _usersRepository.AddAppUser(user);
            return StatusCode(201);
        }

        [HttpPost("login")]
        public async Task<ActionResult<MemberResponseDto>> LoginUser([FromBody] LoginUserRequestDto user)
        {
            AppUser userFromDB;

            if ((userFromDB = await _usersRepository.GetAppUserByNameAsync(user.UserName)) != null)
            {
               
                if (CheckCredentials(user, userFromDB))
                {
                    string token = TokenService.GenerateToken(userFromDB);
                    MemberResponseDto memberDto = _mapper.Map<MemberResponseDto>(userFromDB);
                    LoginUserResponseDto loginUserResponseDto = _mapper.Map<LoginUserResponseDto>(memberDto);
                    loginUserResponseDto.Token = token;
                    return Ok(loginUserResponseDto);
                }
                return BadRequest("WRONG PASSWORD");
            }
            return BadRequest("No user with this username exist");
        }

        private bool CheckCredentials(LoginUserRequestDto userDTO, AppUser userFromDB)
        {

            using HMACSHA512 hmac = new HMACSHA512(userFromDB.PasswordSalt);
            byte[] hashedPassword = hmac.ComputeHash(Encoding.ASCII.GetBytes(userDTO.Password));
            if (hashedPassword.SequenceEqual(userFromDB.PasswordHash))
            {
                return true;
            }
            return false;
        }
    }
}
