namespace Acerola.Domain.Accounts;

public sealed class AccountCannotBeClosedException(Guid id)
    : DomainException($"The account {id} can not be closed because it has funds.");