using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace CSharpNewAPI.Models
{
    [Table("Photos")]
    public class Photo
    {
        public int Id { get; set; }
        
        public string Url { get; set; }
        
        public bool IsMain { get; set; }
        
        public string PublicId { get; set; }
        
        [JsonIgnore]
        public AppUser AppUser { get; set; }
        
        
        public int AppUserId { get; set; }
    }
}