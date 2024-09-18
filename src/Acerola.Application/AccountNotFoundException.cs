namespace Acerola.Application;

public sealed class AccountNotFoundException(Guid accountId)
    : ApplicationException($"The account {accountId} does not exists or is already closed.");