namespace Acerola.Infrastructure;

public class InfrastructureException(string businessMessage)
    : Exception(businessMessage);