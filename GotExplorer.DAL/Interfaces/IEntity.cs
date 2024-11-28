namespace GotExplorer.DAL.Interfaces
{
    public interface IEntity<TKey>
    {
        TKey Id { get; set; }
    }
}
