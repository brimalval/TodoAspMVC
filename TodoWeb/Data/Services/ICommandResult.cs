namespace TodoWeb.Data.Services
{
    public interface ICommandResult
    {
        public void AddError(string key, string message);
        public bool IsValid { get; }
        public Dictionary<string, string> Errors { get; }
    }
}
