using AutoMapper;
using Microsoft.EntityFrameworkCore;
using NeuTournament.Application.DTO;
using NeuTournament.Application.Exceptions;
using NeuTournament.Domain.Entities;
using NeuTournament.Infrastructure.Repositories.Interface;

namespace NeuTournament.Application.Services
{
    public class EventService : IEventService
    { 
        private readonly IGenericRepository<Event> _genericRepositoryEvent;
        private readonly IGenericRepository<Team> _genericRepositoryTeam;
        private readonly IMapper _mapper;
        public EventService(IGenericRepository<Event> genericRepositoryEvent, IGenericRepository<Team> genericRepositoryTeam, IMapper mapper)
        {
            _genericRepositoryEvent = genericRepositoryEvent;
            _genericRepositoryTeam = genericRepositoryTeam;
            _mapper = mapper;
        }
        public async Task<IEnumerable<EventsUpcomingDTO>> GetAllUpcomingEvents()
        {
            var query = _genericRepositoryEvent.GetQuery();
            var result = await query.Where(e => e.StartDate > DateTime.UtcNow).ToListAsync();
            var eventList = _mapper.Map<List<EventsUpcomingDTO>>(result);
            eventList.ForEach(item =>
            {
                item.Duration = item.EndDate.Subtract(item.StartDate);
                item.CountDown = "Event starts in "
                + item.Duration.Days.ToString() + " days, "
                + item.Duration.Hours.ToString() + " hours, "
                + item.Duration.Minutes.ToString() + " minutes "
                + item.Duration.Seconds.ToString() + " seconds.";
            });
            return eventList;
        }
        public async Task<IEnumerable<EventsDTO>> GetAllHistoricalEvents()
        {
            var query = _genericRepositoryEvent.GetQuery();
            var result = await query.Where(e => e.EndDate < DateTime.UtcNow).ToListAsync();
            var eventData = _mapper.Map<List<EventsDTO>>(result);
            eventData.ForEach(item =>
            {
                item.Duration = item.EndDate.Subtract(item.StartDate);
            });
            return eventData;
        }
        public async Task<EventDTO> GetEventById(int id)
        {
            var result = await _genericRepositoryEvent.GetById(id);
            if (result == null)
            {
                throw new KeyNotFoundException($"Event Id: {id} does not exist");
            }
            var query = _genericRepositoryTeam.GetQuery();
            var data = query.Where(x => x.EventId == id)
                 .Include(tm => tm.TeamMembers)
                 .ToList();
            var response = _mapper.Map<EventDTO>(result);
            response.Teams = data.Select(e =>
            {
                var teamMembers = e.TeamMembers.Select(e => new TeamMemberDTO() { EmailId = e.EmailId, TeamId = e.TeamId, Id = e.Id });
                var team = new TeamDTO(e.Id, e.Name, e.Event.Id, e.CreatedBy, e.CreatedDate, teamMembers);
                return team;
            }).ToList();
            return (response);
        }
        public async Task<IEnumerable<EventsDTO>> GetEvents()
        {
            var result = await _genericRepositoryEvent.GetAll();
            var data = _mapper.Map<List<EventsDTO>>(result);
            return data;
        }

        public async Task<IEnumerable<EventsDTO>> GetDisabledEvents()
        {
            var query = _genericRepositoryEvent.GetQuery();
            var result = await query.Where(e => e.DeletedDate != null).ToListAsync();
            if (result == null)
            {
                throw new RecordNotFoundException($"There is no Disabled Event");
            }
            var eventList = _mapper.Map<List<EventsDTO>>(result);
            return eventList;
        }
        public async Task<string> CreateEvent(CreateEventDTO createEvent)
        {
            var eventData = _mapper.Map<Event>(createEvent);
            eventData.CreatedDate = DateTime.UtcNow;
            await _genericRepositoryEvent.Create(eventData);
            return ($"Event {eventData.Id} Created Successfully");
        }
        public async Task<string> UpdateEvent(EventUpdateDTO updateEvent)
        {
            var response = await _genericRepositoryEvent.GetById(updateEvent.Id);
            if (response != null)
            {
                if (updateEvent.CreatedBy == response.CreatedBy)
                {
                    if (response.DeletedDate == null)
                    {
                        response.Id = updateEvent.Id;
                        response.Name = updateEvent.Name;
                        response.Description = updateEvent.Description;
                        response.StartDate = updateEvent.StartDate;
                        response.EndDate = updateEvent.EndDate;
                        response.EventType = updateEvent.EventType;
                        response.Location = updateEvent.Location;
                        response.Banner = updateEvent.Banner;
                        response.Rules = updateEvent.Rules;
                        response.CreatedBy = updateEvent.CreatedBy;
                        response.FeeType = updateEvent.FeeType;
                        response.LastUpdateDate = DateTime.UtcNow;
                        await _genericRepositoryEvent.Update(response);
                        return "Event Updated Successfully";
                    }
                    else
                    {
                        throw new MethodNotAllowed($"Cannot Update Disabled Event");
                    }
                }
                else
                {
                    throw new UnauthorizedAccessException($"Unauthorized User");
                }
            }
            else
            {
                throw new KeyNotFoundException($"Event Id: {updateEvent.Id} does not exist");
            }
        }
        public async Task<string> DeleteEvent(int id)
        {
            var response = await _genericRepositoryEvent.Delete(id);
            if (response)
                return ($"Event Id: {id} successfully deleted");
            else
                throw new KeyNotFoundException($"Event Id: {id} does not exist");
        }
        public async Task<string> DisableEvent(int id)
        {
            var response = await _genericRepositoryEvent.GetById(id);
            if (response != null)
            {
                response.DeletedDate = DateTime.UtcNow;
                await _genericRepositoryEvent.Update(response);
                return "Event Disabled Successfully";
            }
            else
            {
                throw new KeyNotFoundException($"Event Id: {id} does not exist");
            }
        }
        public async Task<byte[]> GetBannerById(int id)
        {
            var result = await _genericRepositoryEvent.GetById(id);
            if (result == null)
            {
                throw new KeyNotFoundException($"Event Id: {id} does not exist");
            }
            var query = _genericRepositoryTeam.GetQuery();
            var data = query.Where(x => x.EventId == id);
            
            return result.Banner;
        }
    }
}