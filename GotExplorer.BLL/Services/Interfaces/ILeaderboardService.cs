using GotExplorer.BLL.DTOs;
using GotExplorer.BLL.Services.Results;
using FluentValidation.Results;

namespace GotExplorer.BLL.Services.Interfaces
{
    public interface ILeaderboardService
    {
        Task<ValidationWithEntityModel<List<LeaderboardRecordDTO>>> GetLeaderboardAsync(LeaderboardRequestDTO requestDTO);
        Task<ValidationWithEntityModel<LeaderboardUserDTO>> GetUserLeaderboardAsync(LeaderboardUserRequestDTO requestDTO);
    }
}