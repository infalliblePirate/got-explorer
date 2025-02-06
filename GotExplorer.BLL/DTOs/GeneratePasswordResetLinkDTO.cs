using System.ComponentModel.DataAnnotations;

namespace GotExplorer.BLL.DTOs
{
    public class GeneratePasswordResetLinkDTO
    {
        [Required]
        [EmailAddress]
        public string Email { get; set;}
    }
}
