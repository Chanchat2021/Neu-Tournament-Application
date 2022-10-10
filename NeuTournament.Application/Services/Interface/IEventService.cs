using NeuTournament.Application.DTO;

namespace NeuTournament.Application.Services
{
    public interface IEventService
    {
        Task<IEnumerable<EventsUpcomingDTO>> GetAllUpcomingEvents();
        Task<IEnumerable<EventsDTO>> GetDisabledEvents();
        Task<IEnumerable<EventsDTO>> GetAllHistoricalEvents();
        Task<EventDTO> GetEventById(int id);
        public Task<string> CreateEvent(CreateEventDTO createEvent);
        Task<IEnumerable<EventsDTO>> GetEvents();
        public Task<byte[]> GetBannerById (int id);
        public Task<string> UpdateEvent(EventUpdateDTO updateEvent);
        public Task<string> DeleteEvent(int id);
        public Task<string> DisableEvent(int id);
    }
}