namespace NeuTournament.Application.Exceptions
{
    public class MethodNotAllowed : Exception
    {
        public MethodNotAllowed() : base() { }
        public MethodNotAllowed(string message) : base(message) { }
    }
}
