using GotExplorer.DAL.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GotExplorer.DAL.Entities
{
    public class Model3D : IEntity<int>
    {
        public int Id { get; set; }

        [MaxLength(100)]
        public string Name { get; set; }

        [MaxLength(255)]
        public string Path { get; set; }

        [Timestamp]
        public uint Version { get; set; }
    }
}
