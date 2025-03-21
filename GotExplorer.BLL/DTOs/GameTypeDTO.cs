using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace GotExplorer.BLL.DTOs
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum GameTypeDto
    {
        [EnumMember(Value = "standard")]
        Standard,
        [EnumMember(Value = "daily")]
        Daily,
    }
}
