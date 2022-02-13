using UnityEngine;
using System.Text.RegularExpressions;

namespace Utilities
{
    public class RegexUtility
    {
        private static readonly string ENUMPATTERN = @"//\#\#\#START\#\#\#(.*?)//\&\&\&END\&\&\&(\n|\r|\r\n|\t).*?}";
        public static string ReplaceEnums(string enumText, string fileText)
        {
            string newText = Regex.Replace(fileText, ENUMPATTERN,
                $"//###START###{enumText}//&&&END&&&\r\n\t}}",
                RegexOptions.Multiline | RegexOptions.Singleline | RegexOptions.IgnoreCase);
            return newText;
        }

        public static string CleanText(string text)
        {
            text = Regex.IsMatch(text, "^([0-9].*?)") ? $"_{text}" : text;
            return Regex.Replace(text, "[^a-zA-Z0-9-]", "_");
        }
    }
}
