using System.ComponentModel.DataAnnotations;

namespace GotExplorer.BLL.DTOs
{
    public class ImageDTO
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Path { get; set; }
    }
}
