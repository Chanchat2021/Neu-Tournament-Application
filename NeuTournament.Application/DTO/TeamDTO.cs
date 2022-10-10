namespace NeuTournament.Application.DTO
{
    public class TeamDTO
    {
        public TeamDTO(int id, string name, int eventId, string createdBy, DateTime createdDate, IEnumerable<TeamMemberDTO> teamMembers)
        {
            Id = id;
            Name = name;
            EventId = eventId;
            CreatedBy = createdBy;
            CreatedDate = createdDate;
            TeamMembers = teamMembers;
        }
        public TeamDTO() { }
        public int Id { get; set; }
        public string Name { get; set; }
        public int EventId { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public IEnumerable<TeamMemberDTO> TeamMembers { get; set; }
    }
}
