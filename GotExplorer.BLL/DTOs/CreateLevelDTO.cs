using GotExplorer.DAL.Entities;

namespace GotExplorer.BLL.DTOs
{
    public class CreateLevelDTO
    {
        public string Name { get; set; }

        public float X { get; set; }

        public float Y { get; set; }

        public IEnumerable<int>? Models { get; set; }
    }
}
