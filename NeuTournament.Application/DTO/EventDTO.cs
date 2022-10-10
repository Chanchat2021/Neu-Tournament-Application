namespace NeuTournament.Application.DTO
{
    public class EventDTO
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Location { get; set; }
        public string Rules { get; set; }
        public byte[]? Banner { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? DeletedDate { get; set; }
        public List<TeamDTO> Teams { get; set; }
    }
}
