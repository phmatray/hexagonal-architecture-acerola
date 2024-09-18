namespace Acerola.Domain;

public class DomainException(string businessMessage)
    : Exception(businessMessage);