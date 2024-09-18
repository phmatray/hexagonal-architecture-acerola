using System.Globalization;

namespace Acerola.Domain.ValueObjects;

public sealed record Amount(double Value)
{
    public override string ToString()
        => Value.ToString(CultureInfo.InvariantCulture);

    public static implicit operator double(Amount amount)
        => amount.Value;

    public static Amount operator -(Amount amount)
        => new(Math.Abs(amount.Value) * -1);

    public static implicit operator Amount(double value)
        => new(value);

    public static Amount operator +(Amount amount1, Amount amount2)
        => new(amount1.Value + amount2.Value);

    public static Amount operator -(Amount amount1, Amount amount2)
        => new(amount1.Value - amount2.Value);

    public static bool operator <(Amount amount1, Amount amount2)
        => amount1.Value < amount2.Value;

    public static bool operator >(Amount amount1, Amount amount2)
        => amount1.Value > amount2.Value;

    public static bool operator <=(Amount amount1, Amount amount2)
        => amount1.Value <= amount2.Value;

    public static bool operator >=(Amount amount1, Amount amount2)
        => amount1.Value >= amount2.Value;
}