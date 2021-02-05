
using System.Collections.Generic;
using CSharpNewAPI.BaseClasses;

namespace CSharpNewAPI.DTOS
{
    public class MemberResponseDto : BaseUser
    {
        public string Username { get; set; }
        public int Age { get; set; }
        
        // the main photo url
        public string MainPhoto { get; set; }
        
        public IEnumerable<PhotoDto> Photos { get; set; }
    }
}
