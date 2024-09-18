namespace Acerola.Infrastructure;

public class AccountNotFoundException(Guid accountId)
    : InfrastructureException($"The account {accountId} does not exists or is not processed yet.");