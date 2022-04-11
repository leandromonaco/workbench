using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace TeamAlignment.Core.Common.Extensions
{
    public static class StringExtensions
    {
        public static List<string> ParseList(this string text)
        {
            text = text.Replace(" , ", ",").Replace(" ,", ",").Replace(", ", ",");
            return text.Split(',').ToList();
        }

        public static string Sanitize(this string text)
        {
            var value = HttpUtility.JavaScriptStringEncode(text);
            value = HttpUtility.HtmlEncode(value);
            return value;
        }

        public static string ParseWorkItemNumber(this string text)
        {
            Regex regex = new Regex("E-[0-9]*|S-[0-9]*|D-[0-9]*");
            var value = regex.Match(text.ToUpper()).Value;
            if (string.IsNullOrEmpty(value))
            {
                value = string.Empty;
            }
            return value;
        }

        public static MatchCollection DetectTags(this string text)
        {
            Regex regex = new Regex(@"\[.*?\]");
            var matches = regex.Matches(text);
            return matches;
        }

        public static string RemoveTags(this string text)
        {
            //text = text.Replace("\\", string.Empty).Replace("-", string.Empty);
            var tags = text.DetectTags();
            foreach (var tag in tags)
            {
                text = text.Replace(tag.ToString(), string.Empty);
            }
            return text.Trim();
        }

        public static double ParseCarryOverPoints(this string text)
        {
            double doubleParsed = 0;
            if (!string.IsNullOrEmpty(text))
            {
                Regex regex = new Regex(@"(\[[0-9]\.[1-9].*point.*\])|(\[[1-9].*point.*\])");
                var textWithoutSpaces = text.Replace(" ", "").ToLower();
                var regexMatch = regex.Match(textWithoutSpaces);

                var cleaneddoubleValue = Regex.Match(regexMatch.Value, @"\d+").Value;
                var isdoubleParsed = double.TryParse(cleaneddoubleValue, out doubleParsed);

                if (!isdoubleParsed)
                {
                    return 0;
                }
            }

            return doubleParsed; //replace estimate with remaining points
        }
    }
}
