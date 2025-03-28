namespace Bank.Credits.Domain.Common.Helpers
{
    public static class MathHelper
    {
        public static decimal Multiplies(decimal first, decimal second)
        {
            return Math.Round(first * second, 2);
        }

        public static decimal Pow(decimal baseNum, int exponent)
        {
            if (exponent == 0) return 1;
            if (exponent < 0) return 1 / Pow(baseNum, -exponent);

            decimal result = 1;
            for (int i = 0; i < exponent; i++)
            {
                result *= baseNum;
            }
            return result;
        }
    }
}
