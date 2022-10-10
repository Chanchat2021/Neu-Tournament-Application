using AutoMapper;
using Microsoft.EntityFrameworkCore;
using NeuTournament.Application.DTO;
using NeuTournament.Application.Exceptions;
using NeuTournament.Application.Services.Interface;
using NeuTournament.Domain.Entities;
using NeuTournament.Infrastructure.Repositories.Interface;

namespace NeuTournament.Application.Services
{
    public class RegistrationService : IRegistrationService
    {
        private readonly IGenericRepository<Event> _eventRepository;
        private readonly IGenericRepository<Registration> _registrationRepository;
        private readonly IMapper _mapper;
        public RegistrationService(IGenericRepository<Event> eventRepository, IGenericRepository<Registration> registrationRepository, IMapper mapper)
        {
            _eventRepository = eventRepository;
            _registrationRepository = registrationRepository;
            _mapper = mapper;
        }
        public async Task<string> CreateRegistration(CreateRegistrationDTO createRegistration)
        {
            var response = await _eventRepository.GetById(createRegistration.EventId);
            if (response != null)
            {
                var registration = _mapper.Map<Registration>(createRegistration);
                await _registrationRepository.Create(registration);
                return ($"Registration {registration.Id} Created Successfully");
            }
            throw new RecordNotFoundException($"EventId: {createRegistration.EventId} does not exist");
        }
        public async Task<PaginationResponse<RegistrationDTO>> GetAllRegistrations(int Pagesize, int CurrentPage)
        {
            var response = new List<RegistrationDTO>();
            var result = await _registrationRepository.GetAll();
            var query = _registrationRepository.GetQuery();
            var totalCount = query.Count();
            if (result.Count() != 0)
            {
                var data = _mapper.Map<List<RegistrationDTO>>(result);
                var paginatedResponse = new PaginationResponse<RegistrationDTO>(data, totalCount);
                return paginatedResponse;
            }
            return new PaginationResponse<RegistrationDTO>(Enumerable.Empty<RegistrationDTO>(), totalCount);
        }
        public async Task<bool> CheckUserRegistration(int EventId, string EmailId)
        {
            var query = _registrationRepository.GetQuery();
            var response = await query.FirstOrDefaultAsync(x => x.EventId == EventId && x.EmailId == EmailId);
            if (response?.Id > 0)
            {
                return true;
            }
            return false;
        }
        public async Task<RegistrationDTO> GetRegistrationtbyId(int id)
        {
            var result = await _registrationRepository.GetById(id);
            if (result == null)
            {
                throw new KeyNotFoundException($"Registration Id: {id} does not exist");
            }
            var response = _mapper.Map<RegistrationDTO>(result);
            return response;
        }
        public async Task<string> UpdateRegistration(RegistrationDTO _registration)
        {
            var response = await _registrationRepository.GetById(_registration.Id);
            if (response != null)
            {
                var events = _eventRepository.GetById(_registration.EventId);
                if (events != null)
                {
                    response.EmailId = _registration.EmailId;
                    response.EventId = _registration.EventId;
                    await _registrationRepository.Update(response);
                    return "Team Updated Successfully";
                }
                throw new KeyNotFoundException($"Event Id: {_registration.EventId} does not exist");
            }
            throw new KeyNotFoundException($"Registration Id: {_registration.Id} does not exist");
        }
        public async Task<string> DeleteRegistration(int id)
        {
            var response = await _registrationRepository.Delete(id);
            if (response)
                return ($"Registration {id} deleted Successfully");
            else
                throw new KeyNotFoundException($"Registration Id: {id} does not exist");
        }
    }
}

