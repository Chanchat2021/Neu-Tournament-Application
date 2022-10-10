namespace NeuTournament.Application.Exceptions
{ 
    public class RecordNotFoundException : Exception
    {
            public RecordNotFoundException() : base() { }
            public RecordNotFoundException(string message) : base(message) { }
    }
}
