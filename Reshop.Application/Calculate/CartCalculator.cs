using System;
using System.Globalization;

namespace Reshop.Application.Calculate
{
    public static class CartCalculator
    {
        public static decimal CalculateDiscount(decimal price, byte discountPercent,int count = 1)
        {
            decimal fullSum = count * price;

            if (discountPercent == 0)
            {
                return fullSum;
            }
            else
            {
                return (decimal)((fullSum * discountPercent) / 100);
            }
        }

        public static byte CalculatePercentOfTwoPrice(decimal originalPrice, decimal newPrice)
        {
            int percent = Math.Abs((int)(((newPrice - originalPrice) / originalPrice) * 100));

            return Convert.ToByte(percent);
        }

    }
}
