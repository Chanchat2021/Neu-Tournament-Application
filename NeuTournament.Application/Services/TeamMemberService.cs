using NeuTournament.Application.Services.Interface;
using NeuTournament.Application.DTO;
using NeuTournament.Domain.Entities;
using NeuTournament.Infrastructure.Repositories.Interface;
using NeuTournament.Application.Exceptions;
using AutoMapper;

namespace NeuTournament.Application.Services
{
    public class TeamMemberService : ITeamMemberService
    {
        private readonly IGenericRepository<TeamMember> _teamMemberRepository;
        private readonly IGenericRepository<Team> _teamRepository;
        private readonly IMapper _mapper;
        public TeamMemberService(IGenericRepository<TeamMember> teamMemberRepository, IGenericRepository<Team> teamRepository, IMapper mapper)
        {
            _teamMemberRepository = teamMemberRepository;
            _teamRepository = teamRepository;
            _mapper = mapper;
        }
        public async Task<PaginationResponse<TeamMemberDTO>> GetAllTeamMembers(int Pagesize, int CurrentPage)
        {
            var response = new List<TeamMemberDTO>();
            var result = await _teamMemberRepository.GetAll();
            var query = _teamRepository.GetQuery();
            var totalCount = query.Count();
            if (result.Count() != 0)
            {
                var data = _mapper.Map<List<TeamMemberDTO>>(result);
                var paginatedResponse = new PaginationResponse<TeamMemberDTO>(data, totalCount);
                return paginatedResponse;
            }
            return new PaginationResponse<TeamMemberDTO>(Enumerable.Empty<TeamMemberDTO>(), totalCount);
        }
        public async Task<TeamMemberDTO> GetTeamMemberById(int id)
        {

            var result = await _teamMemberRepository.GetById(id);
            if (result == null)
            {
                throw new KeyNotFoundException($"Team member with Id: {id} does not exist");
            }
            var response = _mapper.Map<TeamMemberDTO>(result);
            return response;
        }
        public async Task<string> AddTeamMember(CreateTeamMemberDTO createTeamMember)
        {
            var response = await _teamRepository.GetById(createTeamMember.TeamId);
            if (response != null)
            {
                var result = _mapper.Map<TeamMember>(createTeamMember);
                await _teamMemberRepository.Create(result);
                return "Team Member added successfully";
            }
            else
            {
                throw new RecordNotFoundException($"Team Id: {createTeamMember.TeamId} does not exist");
            }
        }
        public async Task<string> UpdateTeamMember(TeamMemberDTO teamMember)
        {
            var teams = await _teamRepository.GetById(teamMember.TeamId);
            if (teams != null)
            {
                var response = await _teamMemberRepository.GetById(teamMember.Id);
                if (response != null)
                {
                    response.Id = teamMember.Id;
                    response.TeamId = teamMember.TeamId;
                    response.EmailId = teamMember.EmailId;
                    await _teamMemberRepository.Update(response);
                    return "Team Member Updated Successfully";
                }
                else
                {
                    throw new RecordNotFoundException($"Team Member Id: {teamMember.Id} does not exist");
                }
            }
            else
            {
                throw new KeyNotFoundException($"Team Id: {teamMember.TeamId} does not exist");
            }
        }
        public async Task<string> DeleteTeamMember(int id)
        {
            var response = await _teamMemberRepository.Delete(id);
            if (response)
                return ($"Team member {id} deleted Successfully");
            else
                throw new KeyNotFoundException($"Team Member Id: {id} does not exist");
        }
    }
}

