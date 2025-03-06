using System;
using System.Text.RegularExpressions;

namespace PartitioningEfCore.EfCoreMigrationExtensions
{
    public static class StringExtensions
    {
        public static string Pluralize(this string word)
        {
            if (string.IsNullOrEmpty(word))
                return word;

            // Simple pluralization rules
            if (Regex.IsMatch(word, "(s|sh|ch|x|z)$", RegexOptions.IgnoreCase))
                return Regex.Replace(word, "(s|sh|ch|x|z)$", "$1es", RegexOptions.IgnoreCase);

            if (Regex.IsMatch(word, "[aeiou]y$", RegexOptions.IgnoreCase))
                return Regex.Replace(word, "y$", "ies", RegexOptions.IgnoreCase);

            return word + "s";
        }

        public static string Depluralize(this string word)
        {
            if (string.IsNullOrEmpty(word))
                return word;

            // Simple depluralization rules
            if (Regex.IsMatch(word, "(s|sh|ch|x|z)es$", RegexOptions.IgnoreCase))
                return Regex.Replace(word, "(s|sh|ch|x|z)es$", "$1", RegexOptions.IgnoreCase);

            if (Regex.IsMatch(word, "ies$", RegexOptions.IgnoreCase))
                return Regex.Replace(word, "ies$", "y", RegexOptions.IgnoreCase);

            if (word.EndsWith("s", StringComparison.OrdinalIgnoreCase))
                return word.Substring(0, word.Length - 1);

            return word;
        }
    }
}