using GotExplorer.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GotExplorer.BLL.DTOs
{
    public class LeaderboardRecordDTO
    {
        public int UserId { get; set; }
        public string Username { get; set; }
        public int? Score { get; set; } = null;
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
    }
}