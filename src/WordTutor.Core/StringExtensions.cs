namespace WordTutor.Core
{
    /// <summary>
    /// Extension methods for working with Strings
    /// </summary>
    public static class StringExtensions
    {
        public static string RemoveSuffix(this string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return string.Empty;
            }

            // Look for a transition from lowercase -> Uppercase
            var length = value.Length;
            while (--length > 0)
            {
                if (char.IsLower(value[length - 1]) && char.IsUpper(value[length]))
                {
                    break;
                }
            }

            return value.Substring(0, length);
        }

        public static string ReplaceSuffix(this string value, string suffix)
        {
            return value.RemoveSuffix() + suffix;
        }
    }
}
