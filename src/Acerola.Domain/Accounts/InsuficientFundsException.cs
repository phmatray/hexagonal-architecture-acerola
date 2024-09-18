namespace Acerola.Domain.Accounts;

public sealed class InsufficientFundsException(Guid id, Amount amount)
    : DomainException($"The account {id} does not have enough funds to withdraw {amount}.");