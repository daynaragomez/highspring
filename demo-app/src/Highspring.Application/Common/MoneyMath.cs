namespace Highspring.Application.Common;

public static class MoneyMath
{
    public static decimal RoundCurrency(decimal amount)
    {
        return Math.Round(amount, 2, MidpointRounding.AwayFromZero);
    }
}
