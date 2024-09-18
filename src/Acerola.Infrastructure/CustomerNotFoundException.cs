namespace Acerola.Infrastructure;

public class CustomerNotFoundException(Guid customerId)
    : InfrastructureException($"The customer {customerId} does not exists or is not processed yet.");