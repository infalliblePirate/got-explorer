using System.ComponentModel.DataAnnotations;

namespace GotExplorer.BLL.DTOs
{
    public class UserDTO
    {
        public int Id { get; set; }

        public string Username { get; set; }

        [EmailAddress]
        public string Email { get; set; }
      
        public string Token { get; set; }

        public Guid ImageId { get; set; }
    }
}
