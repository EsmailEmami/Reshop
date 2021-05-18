using System.Collections.Generic;
using System.Linq;

namespace Reshop.Application.Convertors
{
    public static class Fixer
    {
        public static string FixedText(string email)
        {
            return email.Trim().ToLower();
        }

        public static string ToToman(this decimal value)
        {
            return value.ToString("#,0 تومان");
        }

        public static string FixedToString(object text)
        {
            return text.ToString()?.ToLower();
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
    }
}