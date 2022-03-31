namespace TodoWeb.Config
{
    public static class Config
    {
        public static readonly int DebounceTime = 3000;
        public static readonly string DateInputFormat = "yyyy-MM-ddTHH:mm";
        public static readonly Dictionary<string, string> Colors = new()
        {
            {   "Red", "bg-red-700" },
            {  "Blue", "bg-blue-700"},
            { "Green", "bg-green-700"},
            {"Yellow", "bg-yellow-700"},
            {"Orange", "bg-orange-700"},
            {"Purple", "bg-purple-700"},
            { "Black", "bg-black"},
            {  "None", ""},
        };
    }
}
