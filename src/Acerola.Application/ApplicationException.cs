namespace Acerola.Application;

public class ApplicationException(string businessMessage)
    : Exception(businessMessage);