namespace Acerola.Domain.Accounts;

public sealed class TransactionCollection
{
    private readonly List<ITransaction> _transactions = [];

    public IReadOnlyCollection<ITransaction> GetTransactions()
    {
        return new ReadOnlyCollection<ITransaction>(_transactions);
    }

    public ITransaction GetLastTransaction()
    {
        return _transactions[^1];
    }

    public void Add(ITransaction transaction)
    {
        _transactions.Add(transaction);
    }

    public void Add(IEnumerable<ITransaction> transactions)
    {
        foreach (var transaction in transactions)
        {
            Add(transaction);
        }
    }

    public Amount GetCurrentBalance()
    {
        Amount totalAmount = 0;

        foreach (ITransaction item in _transactions)
        {
            switch (item)
            {
                case Debit:
                    totalAmount -= item.Amount;
                    break;
                case Credit:
                    totalAmount += item.Amount;
                    break;
            }
        }

        return totalAmount;
    }
}