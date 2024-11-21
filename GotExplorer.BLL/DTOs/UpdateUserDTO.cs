using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace GotExplorer.BLL.DTOs
{
    public class UpdateUserDTO
    {
        [JsonIgnore]
        [IgnoreDataMember]
        public string Id { get; set; }

        public string? Username { get; set; }

        [EmailAddress]
        public string? Email { get; set; }

        public int? ImageId { get; set; }
    }
}
