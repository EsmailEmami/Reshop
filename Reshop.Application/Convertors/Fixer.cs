using System;
using System.Collections.Generic;
using System.Linq;

namespace Reshop.Application.Convertors
{
    public static class Fixer
    {
        public static string FixedText(this string text)
        {
            return text.Trim().ToLower();
        }

        public static string NumSplitter(this decimal value)
        {
            return value.ToString("#,0");
        }

        public static string NumSplitter(this int value)
        {
            return value.ToString("#,0");
        }

        public static decimal ToDecimal(this string value)
        {
            try
            {
                return !string.IsNullOrEmpty(value) ? decimal.Parse(value) : 0;
            }
            catch
            {
                return 0;
            }
        }

        public static string FixedToString(object text)
        {
            return text.ToString()?.ToLower();
        }

        public static List<int> ArrayToListInt(this string[] values)
        {
            List<int> list = new List<int>();

            try
            {
                if (values != null && values.Any())
                {
                    foreach (var value in values)
                    {
                        list.Add(Convert.ToInt32(value));
                    }
                }

                return list;
            }
            catch
            {
                return null;
            }
        }

        public static List<int> SplitToListInt(string value, string splitBy = ",")
        {
            List<int> list = new List<int>();
            try
            {
                if (string.IsNullOrEmpty(value))
                    return list;


                string[] values = value.Split(splitBy);


                if (values.Any())
                {
                    foreach (var item in values)
                    {
                        list.Add(Convert.ToInt32(item));
                    }
                }

                return list;
            }
            catch
            {
                return null;
            }
        }

        public static string PersianNumberToLatinNumber(string text)
        {
            Dictionary<string, string> lettersDictionary = new Dictionary<string, string>
            {
                ["۰"] = "0",
                ["۱"] = "1",
                ["۲"] = "2",
                ["۳"] = "3",
                ["۴"] = "4",
                ["۵"] = "5",
                ["۶"] = "6",
                ["۷"] = "7",
                ["۸"] = "8",
                ["۹"] = "9"
            };
            return lettersDictionary.Aggregate(text, (current, item) =>
                current.Replace(item.Key, item.Value));
        }

        public static string TextAfter(this string value, string search)
        {
            return value.Substring(value.IndexOf(search) + search.Length);
        }
    }
}