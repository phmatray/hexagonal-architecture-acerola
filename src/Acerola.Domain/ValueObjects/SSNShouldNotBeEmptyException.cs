namespace Acerola.Domain.ValueObjects;

public sealed class SSNShouldNotBeEmptyException()
    : DomainException("The 'SSN' field is required");