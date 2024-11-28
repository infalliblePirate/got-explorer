using GotExplorer.DAL.Entities;

namespace GotExplorer.BLL.Services.Interfaces
{
    public interface IJwtService
    {
        string GenerateToken(User user);
    }
}
