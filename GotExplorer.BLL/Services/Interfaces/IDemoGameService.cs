using GotExplorer.BLL.DTOs;
using GotExplorer.BLL.Services.Results;
using GotExplorer.DAL.Entities;

namespace GotExplorer.BLL.Services.Interfaces
{
    public interface IDemoGameService
    {
        Task<ValidationWithEntityModel<NewDemoGameDTO>> StartGameAsync();
    }
}
