using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GotExplorer.BLL.DTOs
{
    public class LeaderboardRequestDTO
    {
        public int? Limit { get; set; }
        public LeaderboardSortBy SortBy { get; set; } = LeaderboardSortBy.Score;
        public OrderBy OrderBy { get; set; } = OrderBy.Desc;
    }
}