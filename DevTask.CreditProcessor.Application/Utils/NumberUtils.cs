namespace DevTask.CreditProcessor.Application.Utils;

public static class NumberUtils
{
    public static decimal PercentOf(this decimal part, decimal total) => (total == 0) ? 0 : (part / total * 100).ToPrecision2();

    public static decimal ToPrecision2(this decimal value) => Math.Round(value, 2);
}
