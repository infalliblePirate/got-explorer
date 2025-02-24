using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace GotExplorer.BLL.DTOs
{
    public class SubmitScoreDTO
    {
        [JsonIgnore]
        [IgnoreDataMember]
        public string? UserId { get; set; }
        public int Score { get; set; }
    }
}