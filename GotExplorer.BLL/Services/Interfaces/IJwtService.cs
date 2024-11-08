using GotExplorer.DAL.Models;

namespace GotExplorer.BLL.Services.Interfaces
{
    public interface IJwtService
    {
        string GenerateToken(User user);
    }
}
