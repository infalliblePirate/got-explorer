using System.ComponentModel.DataAnnotations.Schema;

namespace GotExplorer.DAL.Entities
{
    public class GameLevel
    {
        public int GameId { get; set; }

        [ForeignKey("GameId")]
        public Game Game { get; set; }

        public int LevelId { get; set; }

        [ForeignKey("LevelId")]
        public Level Level { get; set; }

        public int? Score { get; set; }
    }
}
