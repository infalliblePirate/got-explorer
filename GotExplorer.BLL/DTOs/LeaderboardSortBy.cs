using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace GotExplorer.BLL.DTOs
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum LeaderboardSortBy
    {
        [EnumMember(Value = "score")]
        Score,
        [EnumMember(Value = "time")]
        Time,
    }
}
