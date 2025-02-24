using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace GotExplorer.BLL.DTOs
{
    public class CalculateScoreDTO
    {
        [JsonIgnore]
        [IgnoreDataMember]
        public string? UserId { get; set; }
        [JsonIgnore]
        [IgnoreDataMember]
        public int GameId { get; set; }
        public int LevelId { get; set; }
        public float X { get; set; }
        public float Y { get; set; }
    }
}