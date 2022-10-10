using NeuTournament.Domain.Entities.Enum;

namespace NeuTournament.Application.DTO
{
    public class EventUpdateDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public EventType EventType { get; set; }
        public Location Location { get; set; }
        public byte[]? Banner { get; set; }
        public DateTime? LastUpdateDate { get; set; }
        public string Rules { get; set; }
        public string CreatedBy{ get; set; }
        public FeeType FeeType { get; set; }
    }
}
