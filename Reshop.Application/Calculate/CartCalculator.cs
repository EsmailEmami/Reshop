using System;
using System.Globalization;

namespace Reshop.Application.Calculate
{
    public static class CartCalculator
    {
        public static decimal CalculateOrderDetailSum(int count, decimal price, byte discountPercent)
        {
            decimal fullSum = count * price;
            return (decimal)((fullSum * discountPercent) / 100);
        }

        public static decimal CalculateDiscountPrice(decimal price, byte discountPercent)
        {
            return (decimal)((price * discountPercent) / 100);
        }


        public static byte CalculatePercentOfTwoPrice(decimal originalPrice, decimal newPrice)
        {
            int percent = Convert.ToInt32(((newPrice - originalPrice) / originalPrice) * 100);

            if (percent < 0)
            {
                return Convert.ToByte((percent * -1));
            }

            return Convert.ToByte(percent);
        }
    }
}
