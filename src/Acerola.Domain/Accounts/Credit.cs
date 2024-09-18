namespace Acerola.Domain.Accounts;

public sealed class Credit : IEntity, ITransaction
{
    public Guid Id { get; private set; }
    public Guid AccountId { get; private set; }
    public Amount Amount { get; private set; }
    public string Description => "Credit";
    public DateTime TransactionDate { get; private set; }

    private Credit() { }

    public static Credit Load(Guid id, Guid accountId, Amount amount, DateTime transactionDate)
    {
        Credit credit = new()
        {
            Id = id,
            AccountId = accountId,
            Amount = amount,
            TransactionDate = transactionDate
        };
        return credit;
    }

    public Credit(Guid accountId, Amount amount)
    {
        Id = Guid.NewGuid();
        AccountId = accountId;
        Amount = amount;
        TransactionDate = DateTime.UtcNow;
    }
}