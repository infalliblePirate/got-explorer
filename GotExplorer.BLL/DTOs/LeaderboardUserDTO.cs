namespace GotExplorer.BLL.DTOs
{
    public class LeaderboardUserDTO
    {
        public int UserId { get; set; }
        public string Username { get; set; }
        public int Score { get; set; }
        public int Rank { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
    }
}
