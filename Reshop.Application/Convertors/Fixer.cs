using System;
using System.Collections.Generic;
using System.Linq;

namespace Reshop.Application.Convertors;

public static class Fixer
{
    public static string FixedText(this string text)
    {
        try
        {
            return text.Trim().ToLower();
        }
        catch
        {
            return null;
        }
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

    public static List<int> ToListInt(this List<string> values)
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
    public static List<string> ToListString(this List<int> values)
    {
        List<string> list = new List<string>();

        try
        {
            if (values != null && values.Any())
            {
                foreach (var value in values)
                {
                    list.Add(value.ToString());
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

    public static string TruncateLongString(this string str, int maxLength)
    {
        return str?[0..Math.Min(str.Length, maxLength)];
    }

    public static string WorldSelector(this string str, int skip)
    {
        var b = str.Split(" ").Skip(skip);

        var result = string.Join(" ", b);

        return result;
    }

    public static string WorldSelector(this string str, int skip, int take)
    {
        if (take <= 0)
            return null;


        var b = str.Split(" ").Skip(skip).Take(take);

        var result = string.Join(" ", b);

        return result;
    }

    public static string BoolToText(this bool value, string beforeText)
    {
        string boolText = value ? "بله" : "خیر";

        return $"{beforeText}: {boolText}";
    }

    public static string BoolToText(this bool value) =>
         value ? "بله" : "خیر";

    public static string BoolToText(this bool value, string trueText, string falseText) =>
        value ? trueText : falseText;


    public static string ListToString(this IEnumerable<int> list, string delimiter) =>
        string.Join(delimiter, list);

    public static string ListToString(this IEnumerable<string> list, string delimiter)
    {
        try
        {
            return string.Join(delimiter, list);
        }
        catch
        {
            return null;
        }
    }

    public static bool EnumContainValue<T>(string value) where T : Enum
    {
        var values = Enum.GetValues(typeof(T)).Cast<T>();

        return values.Any(c => string.Equals(c.ToString(), value, StringComparison.CurrentCultureIgnoreCase));
    }

    public static string ProductTypesValueToPersian(this string text)
    {
        Dictionary<string, string> lettersDictionary = new Dictionary<string, string>
        {
            ["mobile"] = "موبایل",
            ["aux"] = "ای یو ایکس",
        };
        return lettersDictionary.Aggregate(text, (current, item) =>
            current.Replace(item.Key, item.Value));
    }

    public static List<string> ToList(this string value, string delimiter) =>
        value == null ? new List<string>() : value.Split(delimiter).ToList();


}