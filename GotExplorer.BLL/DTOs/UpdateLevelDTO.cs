namespace GotExplorer.BLL.DTOs
{
    public class UpdateLevelDTO
    {
        public string? Name { get; set; }

        public float? X { get; set; }

        public float? Y { get; set; }

        public IEnumerable<int>? Models { get; set; }
    }
}
