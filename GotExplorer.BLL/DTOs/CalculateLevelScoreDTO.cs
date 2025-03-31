using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace GotExplorer.BLL.DTOs
{
    public class CalculateLevelScoreDTO
    {
        public int LevelId { get; set; }
        public float X { get; set; }
        public float Y { get; set; }
    }
}
