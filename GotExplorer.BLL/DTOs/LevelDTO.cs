using GotExplorer.DAL.Entities;
using System.ComponentModel.DataAnnotations;

namespace GotExplorer.BLL.DTOs
{
    public class LevelDTO
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public float X { get; set; }

        public float Y { get; set; }

        public IEnumerable<Model3dDTO> Models { get; set; }
    }
}
