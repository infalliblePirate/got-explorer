using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace GotExplorer.BLL.DTOs
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum OrderBy
    {
        [EnumMember(Value = "asc")]
        Asc,
        [EnumMember(Value = "desc")]
        Desc,
    }
}
