namespace Acerola.Domain.ValueObjects;

internal sealed class InvalidSSNException()
    : DomainException("Invalid SSN format. Use YYMMDDNNNN.");