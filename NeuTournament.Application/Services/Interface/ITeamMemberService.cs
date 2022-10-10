using NeuTournament.Application.DTO;

namespace NeuTournament.Application.Services.Interface
{
    public interface ITeamMemberService
    {
        Task<PaginationResponse<TeamMemberDTO>> GetAllTeamMembers(int Pagesize, int CurrentPage);
        Task<TeamMemberDTO> GetTeamMemberById(int id);
        public Task<string> AddTeamMember(CreateTeamMemberDTO createTeamMember);
        public Task<string> UpdateTeamMember(TeamMemberDTO teamMember);
        public Task<string> DeleteTeamMember(int id);
    }
}
