using System.Linq;
using AutoMapper;
using CSharpNewAPI.DTOS;
using CSharpNewAPI.Models;

namespace CSharpNewAPI.Utils
{
    public class AutoMapperProfile:Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<MemberResponseDto, LoginUserResponseDto>();
            CreateMap<Photo, PhotoDto>();
            CreateMap<AppUser, MemberResponseDto>().ForMember(member => member.Age,
                age => age.MapFrom((appUser) => appUser.CalculateAge())).ForMember(member => member.MainPhoto, url =>
                url.MapFrom((appUser) => appUser.Photos.FirstOrDefault(photo => photo.IsMain).Url));
        }
    }
}