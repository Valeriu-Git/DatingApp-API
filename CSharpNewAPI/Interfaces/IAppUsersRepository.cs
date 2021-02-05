using System.Collections.Generic;
using System.Threading.Tasks;
using CSharpNewAPI.DTOS;
using CSharpNewAPI.Models;

namespace CSharpNewAPI.Interfaces
{
    public interface IAppUsersRepository
    {
        public Task<AppUser> AddAppUser(AppUser user);
        public Task<AppUser> GetAppUserByIdAsync(int id);

        public Task<MemberResponseDto> GetMemberByIdAsync(int id);
        
        public Task<AppUser> GetAppUserByNameAsync(string username);

        public Task<MemberResponseDto> GetMemberByNameAsync(string username);
        
        public Task<IEnumerable<AppUser>> GetAppUsersAsync();

        public Task<IEnumerable<MemberResponseDto>> GetMembersAsync();

        public Task<bool> DoesAppUserExistAsync(IAppUser obj);

        public void UpdateAppUserAsync(AppUser obj);

        Task<bool> SaveChangesAsync();
    }
}