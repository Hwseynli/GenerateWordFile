using System;
using System.Text;

namespace GenerateWordFile.Helpers
{
    public static class StringHelper
    {
        public static string Capitalize(this string text)
        {
            if (string.IsNullOrWhiteSpace(text)) return text;

            text = text.Trim();
            string[] arr = text.Split(" ");
            StringBuilder newstr = new StringBuilder();
            for (int i = 0; i < arr.Length; i++)
            {
                if (arr[i].Length > 0)
                {
                    string firstChar = arr[i][0].ToString().ToUpper();
                    string rest = arr[i].Substring(1).ToLower();
                    newstr.Append($"{firstChar}{rest} ");
                }
            }
            return newstr.ToString().Trim();
        }
    }
}

