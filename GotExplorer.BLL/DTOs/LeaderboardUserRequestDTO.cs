using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace GotExplorer.BLL.DTOs
{
    public class LeaderboardUserRequestDTO 
    {
        [JsonIgnore]
        [IgnoreDataMember]
        [BindNever]
        public int? UserId { get; set; }
        public LeaderboardSortBy SortBy { get; set; } = LeaderboardSortBy.Score;
        public OrderBy OrderBy { get; set; } = OrderBy.Desc;
    }
}
