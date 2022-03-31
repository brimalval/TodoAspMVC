namespace TodoWeb.Config
{
    public static class Config
    {
        public static readonly int DebounceTime = 3000;
        public static readonly string DateInputFormat = "yyyy-MM-ddTHH:mm";
        public static readonly Dictionary<string, string> Colors = new()
        {
            {   "red", "bg-red-700" },
            {  "blue", "bg-blue-700"},
            { "green", "bg-green-700"},
            {"yellow", "bg-yellow-700"},
            {"orange", "bg-orange-700"},
            {"purple", "bg-purple-700"},
            { "black", "bg-black"},
            {  "none", ""},
        };
    }
}
