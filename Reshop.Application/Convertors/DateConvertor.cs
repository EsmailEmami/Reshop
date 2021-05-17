using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reshop.Application.Convertors
{
    public static class DateConvertor
    {
        public static string ToShamsi(this DateTime value)
        {
            PersianCalendar pc = new PersianCalendar();

            return pc.GetHour(value).ToString("00") + ":" + pc.GetMinute(value).ToString("00") + " - " +
                   pc.GetYear(value).ToString("00") + "/" + pc.GetMonth(value).ToString("00") + "/" +
                   pc.GetDayOfMonth(value).ToString("00");
        }

        public static DateTime ToMiladi(DateTime dateTime)
        {
            return new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, new PersianCalendar());
        }

        public static DateTime ConvertPersianDateTimeToEnglishDateTime(this string dateTime)
        {
            string englishDateTime = PersianToEnglish(dateTime);

            string PersianToEnglish(string persianStr)
            {
                Dictionary<string, string> LettersDictionary = new Dictionary<string, string>
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
                return LettersDictionary.Aggregate(persianStr, (current, item) =>
                    current.Replace(item.Key, item.Value));
            }



            string[] dateTimeSplit = englishDateTime.Split(" ");
            string[] time = dateTimeSplit[0].Split(":");
            string[] date = dateTimeSplit[1].Split("/");

            return new DateTime(int.Parse(date[0]),
                int.Parse(date[1]),
                int.Parse(date[2]),
                int.Parse(time[0]),
                int.Parse(time[1]),
                0,
                new PersianCalendar());
        }
    }
}
