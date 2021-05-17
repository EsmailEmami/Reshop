using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Reshop.Application.Validations.Attributes
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class NationalCodeAttribute : ValidationAttribute
    {
        public NationalCodeAttribute(string errorMessage)
            : base()
        {
            this.ErrorMessage = errorMessage;
        }

        public override bool IsValid(object value)
        {
            if (value == null) return false;
            if (IsValidNationalCode(value.ToString()) == false) return false;

            return true;
        }

        public bool IsValidNationalCode(string nationalCode)
        {
            if (string.IsNullOrEmpty(nationalCode)) return false;

            // check national code is 10 character
            if (!new Regex(@"\d{10}").IsMatch(nationalCode)) return false;

            var array = nationalCode.ToCharArray();

            var allDigitEqual = new[] { "0000000000", "1111111111", "2222222222", "3333333333", "4444444444", "5555555555", "6666666666", "7777777777", "8888888888", "9999999999" };

            if (allDigitEqual.Contains(nationalCode)) return false;

            var j = 10;
            var sum = 0;
            for (var i = 0; i < array.Length - 1; i++)
            {
                sum += Int32.Parse(array[i].ToString(CultureInfo.InvariantCulture)) * j;
                j--;
            }
            var div = sum / 11;
            var r = div * 11;
            var diff = Math.Abs(sum - r);
            if (diff <= 2)
            {
                return diff == Int32.Parse(array[9].ToString(CultureInfo.InvariantCulture));
            }
            var temp = Math.Abs(diff - 11);
            return temp == Int32.Parse(array[9].ToString(CultureInfo.InvariantCulture));
        }
    }
}
