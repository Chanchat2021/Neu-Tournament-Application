﻿using NeuTournament.Domain.Entities.Enum;

namespace NeuTournament.Application.DTO
{
    public class EventsUpcomingDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Location { get; set; }
        public string Rules { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public TimeSpan Duration { get; set; }
        public EventType EventType { get; set; }
        public byte[]? Banner { get; set; }
        public string CountDown { get; set; }
        public DateTime? DeletedDate { get; set; }
        public string FeeType { get; set; }
    }
}
