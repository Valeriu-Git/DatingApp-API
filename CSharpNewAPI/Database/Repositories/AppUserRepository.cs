using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using CSharpNewAPI.DTOS;
using CSharpNewAPI.Interfaces;
using CSharpNewAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace CSharpNewAPI.Database.Repositories
{
    public class AppUserRepository:IAppUsersRepository
    {
        private readonly DatabaseContext _context;
        private IConfigurationProvider _config;


        public AppUserRepository(DatabaseContext context,IMapper mapper)
        {
            _config = mapper.ConfigurationProvider;
            _context = context;
        }

        public async Task<AppUser> AddAppUser(AppUser user)
        {
            var x = await _context.Users.AddAsync(user);
            AppUser u = x.Entity;
            await SaveChangesAsync();
            return u;
        }

        public async Task<AppUser> GetAppUserByIdAsync(int id)
        {
            return await _context.Users.Include(u=>u.Photos).FirstOrDefaultAsync(u=>u.Id==id);
        }

        public async Task<MemberResponseDto> GetMemberByIdAsync(int id)
        {
            return await _context.Users.Include(u => u.Photos).ProjectTo<MemberResponseDto>(_config)
                .FirstOrDefaultAsync(user => user.Id == id);
        }

        public async Task<AppUser> GetAppUserByNameAsync(string username)
        {
            return await _context.Users.Include(u=>u.Photos).FirstOrDefaultAsync((user)=>user.UserName.Equals(username));
        }

        public async Task<MemberResponseDto> GetMemberByNameAsync(string username)
        {
            return await _context.Users.Include(u => u.Photos).AsSingleQuery().ProjectTo<MemberResponseDto>(_config)
                .FirstOrDefaultAsync(user => user.Username.Equals(username));
        }

        public async Task<IEnumerable<AppUser>> GetAppUsersAsync()
        {
            return await _context.Users.Include(u=>u.Photos).ToListAsync();
        }

        public async Task<IEnumerable<MemberResponseDto>> GetMembersAsync()
        {
            return await _context.Users.Include(u => u.Photos).ProjectTo<MemberResponseDto>(_config).ToListAsync();
        }

        public async Task<bool> DoesAppUserExistAsync(IAppUser obj)
        {
            AppUser appUser =await _context.Users.FirstOrDefaultAsync((user) => user.UserName.Equals(obj.UserName));
            return appUser != null;
        }

        public async void UpdateAppUserAsync(AppUser obj)
        {
            AppUser user =await GetAppUserByNameAsync(obj.UserName);
            _context.Entry(user).CurrentValues.SetValues(obj);
            await SaveChangesAsync();
        }

        public async Task<bool> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync() != -1;
        }
    }
}