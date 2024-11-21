using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace GotExplorer.BLL.DTOs
{
    public class UpdateUserPasswordDTO
    {
        [JsonIgnore]
        [IgnoreDataMember]
        public string Id { get; set; }

        [Required]
        public string CurrentPassword { get; set; }

        [Required]
        public string NewPassword { get; set; }
    }
}
