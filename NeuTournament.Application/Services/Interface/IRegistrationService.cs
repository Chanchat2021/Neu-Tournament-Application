using NeuTournament.Application.DTO;

namespace NeuTournament.Application.Services.Interface
{
    public interface IRegistrationService
    {
        public Task<PaginationResponse<RegistrationDTO>> GetAllRegistrations(int Pagesize, int CurrentPage);
        public Task<bool> CheckUserRegistration(int EventId, string EmailId);
        public Task<RegistrationDTO> GetRegistrationtbyId(int id);
        public Task<string> CreateRegistration(CreateRegistrationDTO createRegistrationDTO);
        public Task<string> UpdateRegistration(RegistrationDTO _registration);
        public Task<string> DeleteRegistration(int id);
    }
}
