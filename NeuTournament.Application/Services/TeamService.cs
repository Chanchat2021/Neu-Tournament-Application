using NeuTournament.Application.Services.Interface;
using NeuTournament.Infrastructure.Repositories.Interface;
using NeuTournament.Domain.Entities;
using NeuTournament.Application.DTO;
using NeuTournament.Application.Exceptions;
using Microsoft.EntityFrameworkCore;
using AutoMapper;

namespace NeuTournament.Application.Services
{
    public class TeamService : ITeamService
    {
        private readonly IGenericRepository<Event> _eventRepository;
        private readonly IGenericRepository<Team> _teamRepository;
        private readonly IMapper _mapper;
        public TeamService(IGenericRepository<Event> eventRepository, IGenericRepository<Team> teamRepository, IMapper mapper)
        {
            _eventRepository = eventRepository;
            _teamRepository = teamRepository;
            _mapper = mapper;
        }
        public async Task<PaginationResponse<TeamDTO>> GetAllTeams(int Pagesize, int CurrentPage)
        {
            var response = new List<TeamDTO>();
            var result = await _teamRepository.GetAll();
            var query = _teamRepository.GetQuery();
            var totalCount = query.Count();
            if (result.Count() != 0)
            {
                var data = _mapper.Map<List<TeamDTO>>(result);
                var paginatedResponse = new PaginationResponse<TeamDTO>(data, totalCount);
                return paginatedResponse;
            }
            return new PaginationResponse<TeamDTO>(Enumerable.Empty<TeamDTO>(), totalCount);
        }

        public IEnumerable<TeamDTO> GetTeamsByEventId(int eventId)
        {
            var query = _teamRepository.GetQuery();
            var result = query.Where(x => x.EventId == eventId).Include(tm => tm.TeamMembers)
                 .ToList();
            if (result == null)
            {
                throw new KeyNotFoundException($"Event Id: {eventId} does not exist");
            }
            var teams = _mapper.Map<List<TeamDTO>>(result);
            teams[0].TeamMembers = result[0].TeamMembers.Select(e => new TeamMemberDTO() { EmailId = e.EmailId, Id = e.Id, TeamId = e.Id });
            return teams;
        }
        public async Task<string> CreateTeam(CreateTeamDTO createTeam)
        {
            var events = await _eventRepository.GetById(createTeam.EventId);
            if (events != null)
            {
                var team = _mapper.Map<Team>(createTeam);
                team.CreatedDate = DateTime.Now;
                await _teamRepository.Create(team);
                return ($"Team {team.Id} Created Successfully");
            }
            else
            {
                throw new RecordNotFoundException($"Event Id : {createTeam.EventId} does not exist");
            }
        }
        public async Task<string> UpdateTeam(TeamUpdateDTO team)
        {
            var response = await _teamRepository.GetById(team.Id);
            if (response != null)
            {
                var events = await _eventRepository.GetById(team.EventId);
                if (events != null)
                {
                    response.Id = team.Id;
                    response.Name = team.Name;
                    response.EventId = team.EventId;
                    response.CreatedBy = team.CreatedBy;
                    await _teamRepository.Update(response);
                    return "Team Updated Successfully";
                }
                else
                {
                    throw new RecordNotFoundException($"Event Id: {team.EventId} does not exist");
                }
            }
            else
            {
                throw new KeyNotFoundException($"Team Id: {team.Id} does not exist");
            }
        }
        public async Task<string> DeleteTeam(int id)
        {
            var response = await _teamRepository.Delete(id);
            if (response)
                return ($"Team Id: {id} successfully deleted");
            else
                throw new KeyNotFoundException($"Team Id: {id} does not exist");
        }

    }
}

