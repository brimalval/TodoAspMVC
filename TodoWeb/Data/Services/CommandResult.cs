namespace TodoWeb.Data.Services
{
    public class CommandResult
    {
        private readonly Dictionary<string, string> _errors;
        public CommandResult()
        {
            _errors = new();
        }
        public void AddError(string key, string message)
        {
            _errors[key] = message;
        }
        public bool IsValid { get => _errors.Count == 0; }
        public Dictionary<string, string> Errors { get => _errors; }
    }
}
