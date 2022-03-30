using System.Text.RegularExpressions;

namespace TodoWeb.Extensions
{
    public static class StringValidation
    {
        public static bool HasSpecialChars(this string str)
        {
            return Regex.Match(str, @"[^a-zA-Z0-9\-_\s()]").Success;
        }
        public static bool IsBelowMaxLength(this string str, int maxLength)
        {
            return str.Length <= maxLength;
        }
    }
}
