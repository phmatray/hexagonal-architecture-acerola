namespace Acerola.Domain.ValueObjects;

public sealed class NameShouldNotBeEmptyException()
    : DomainException("The 'Name' field is required");