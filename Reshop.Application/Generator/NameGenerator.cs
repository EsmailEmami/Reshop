using System;

namespace Reshop.Application.Generator
{
    public class NameGenerator
    {
        public static string GenerateUniqCodeWithDash()
        {
            return Guid.NewGuid().ToString();
        }

        public static string GenerateUniqUpperCaseCodeWithoutDash()
        {
            return Guid.NewGuid().ToString("N").ToUpper();
        }

        public static string GenerateShortKey(int length = 5)
        {
            return Guid.NewGuid().ToString("N").Substring(0, length);
        }

        public static string GenerateNumber(int minValue = 100000000, int maxValue = 999999999)
        {
            return new Random().Next(minValue, maxValue).ToString();
        }
    }
}