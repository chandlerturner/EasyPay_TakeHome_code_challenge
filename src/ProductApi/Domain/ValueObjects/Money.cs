namespace ProductApi.Domain.ValueObjects;

public record Money(decimal Amount, string Currency)
{
    public static Money operator +(Money a, Money b)
    {
        if (a.Currency != b.Currency)
        {
            throw new InvalidOperationException("Cannot add different currencies");
        }

        return a with { Amount = a.Amount + b.Amount };
    }

    public static Money operator -(Money a, Money b)
    {
        if (a.Currency != b.Currency)
        {
            throw new InvalidOperationException("Cannot subtract different currencies");
        }

        return a with { Amount = a.Amount - b.Amount };
    }

    public override string ToString()
    {
        return $"{Amount:N2} {Currency}";
    }
}