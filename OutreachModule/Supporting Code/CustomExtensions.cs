using System;
namespace CustomExtensions
{
    //Extension methods must be defined in a static class
    public static class StringExtension
    {
        public static bool Contains(this string source, string toCheck, StringComparison comp)
        {
            return source.IndexOf(toCheck, comp) >= 0;
        }

        public static bool ContainsIgnoreCase(this string source, string toCheck)
        {
            return source.Contains(toCheck, StringComparison.InvariantCultureIgnoreCase);
        }

        public static bool EqualsIgnoreCase(this string source, string toCheck)
        {
            return source.Equals(toCheck, StringComparison.InvariantCultureIgnoreCase);
        }
    }
}