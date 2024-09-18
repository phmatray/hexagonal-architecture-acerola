namespace Acerola.Domain.Customers;

public sealed class AccountCollection
{
    private readonly List<Guid> _accountIds = [];

    public IReadOnlyCollection<Guid> GetAccountIds()
    {
        return new ReadOnlyCollection<Guid>(_accountIds);
    }

    public void Add(Guid accountId)
    {
        _accountIds.Add(accountId);
    }
}