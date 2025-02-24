namespace GotExplorer.BLL.DTOs
{
    public class NewGameDTO
    {
        public int GameId { get; set; }
        public IEnumerable<int> LevelIds { get; set; }
    }
}
