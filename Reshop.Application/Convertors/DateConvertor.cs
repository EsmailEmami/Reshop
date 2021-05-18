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
            string englishDateTime = Fixer.PersianNumberToLatinNumber(dateTime);

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

        public static DateTime ConvertPersianDateToEnglishDate(this string date)
        {
            string englishDate = Fixer.PersianNumberToLatinNumber(date);

            string[] dateSplit = englishDate.Split("/");

            return new DateTime(int.Parse(dateSplit[0]),
                int.Parse(dateSplit[1]),
                int.Parse(dateSplit[2]),
                0,
                0,
                0,
                new PersianCalendar());
        }
    }
}
