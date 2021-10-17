using System;

namespace Reshop.Application.Calculate
{
    public static class CommentCalculator
    {
        public static double CommentScore(double percent)
        {
            return percent * 0.05;
        }

        public static int CalculatePercentOfTwoNumber(int ofNumber, int toNumber)
        {
            try
            {
                return Math.Abs((ofNumber / toNumber) * 100);
            }
            catch
            {
                return 0;
            }
        }
    }
}