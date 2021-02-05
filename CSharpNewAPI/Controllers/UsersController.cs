using CSharpNewAPI.Database;
using CSharpNewAPI.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CSharpNewAPI.DTOS;
using CSharpNewAPI.Interfaces;

namespace CSharpNewAPI.Controllers
{
    public class UsersController : BaseApiController
    {
        private readonly IAppUsersRepository _repository;
        private readonly IMapper _mapper;

        public UsersController(IAppUsersRepository repository,IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        [HttpGet("getAll")]
        public async Task<ActionResult<IEnumerable<MemberResponseDto>>> GetUsers()
        {
            // var users = await _repository.GetAppUsersAsync();
            // var membersToReturn = _mapper.Map<IEnumerable<MemberDto>>(users);
            var membersToReturn = await _repository.GetMembersAsync();
            return Ok(membersToReturn);
        }

        [HttpGet("{username}")]
        public async Task<ActionResult<MemberResponseDto>> GetUser(string username)
        {
            // var user =await _repository.GetAppUserByNameAsync(username);
            // var member = _mapper.Map<MemberDto>(user);
            var member = await _repository.GetMemberByNameAsync(username);
            return Ok(member);
        }
    }
}
