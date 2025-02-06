using System.ComponentModel.DataAnnotations;

namespace GotExplorer.BLL.DTOs
{
    public class ResetPasswordDTO
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        public string Token { get; set; }
    }
}
