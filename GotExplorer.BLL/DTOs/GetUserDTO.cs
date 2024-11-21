using System.ComponentModel.DataAnnotations;

namespace GotExplorer.BLL.DTOs
{
    public class GetUserDTO
    {
        public int Id { get; set; }
        public string UserName { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        public int ImageId { get; set; }
    }
}
