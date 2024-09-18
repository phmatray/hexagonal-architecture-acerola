namespace Acerola.Domain.Customers;

public sealed class AccountCollection
{
    private readonly List<Guid> _accountIds = [];

    public IReadOnlyCollection<Guid> GetAccountIds()
    {
        IReadOnlyCollection<Guid> accountIds = new ReadOnlyCollection<Guid>(_accountIds);
        return accountIds;
    }

    public void Add(Guid accountId)
    {
        _accountIds.Add(accountId);
    }
}