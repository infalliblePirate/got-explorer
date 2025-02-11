using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GotExplorer.BLL.DTOs
{
    public class UpdateGameLevelDTO
    {
        public int GameId { get; set; }

        public int LevelId { get; set; }

        public int? Score { get; set; }
    }
}