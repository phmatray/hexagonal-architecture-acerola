namespace Acerola.Application;

internal sealed class CustomerNotFoundException(string message)
    : ApplicationException(message);