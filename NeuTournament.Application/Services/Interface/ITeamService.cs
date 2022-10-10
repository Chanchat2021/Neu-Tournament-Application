using NeuTournament.Application.DTO;
using NeuTournament.Domain.Entities;

namespace NeuTournament.Application.Services.Interface
{
    public interface ITeamService
    {
        public Task<PaginationResponse<TeamDTO>> GetAllTeams(int Pagesize, int CurrentPage);
        public IEnumerable<TeamDTO> GetTeamsByEventId(int eventId);
        public Task<string> CreateTeam(CreateTeamDTO createTeam);
        public Task<string> UpdateTeam(TeamUpdateDTO team);
        public Task<string> DeleteTeam(int id);
    }
}