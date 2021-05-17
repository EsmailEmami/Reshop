namespace Reshop.Application.Calculate
{
    public static class CartCalculator
    {
        public static decimal CalculateOrderDetail(int count, decimal price)
        {
            return count * price;
        }
    }
}
