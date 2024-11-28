using GotExplorer.DAL.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GotExplorer.DAL.Entities
{
    public class Level : IEntity<int>
    {
        public int Id { get; set; }

        [MaxLength(200)]
        public string Name { get; set; }

        public float X { get; set; }

        public float Y { get; set; }

        public List<Model3D> Models { get; set; }

        [Timestamp]
        public uint Version { get; set; }
    }
}
