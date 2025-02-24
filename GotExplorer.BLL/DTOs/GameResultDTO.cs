using GotExplorer.DAL.Entities;

namespace GotExplorer.BLL.DTOs
{
    public class GameResultDTO
    {
        public int GameId { get; set; }
        public int UserId { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public int Score { get; set; }
    }
}
