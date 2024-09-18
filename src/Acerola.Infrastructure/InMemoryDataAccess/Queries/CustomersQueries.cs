using Acerola.Application.Queries;
using Acerola.Application.Results;
using Acerola.Domain.Customers;

namespace Acerola.Infrastructure.InMemoryDataAccess.Queries;

public class CustomersQueries(Context context, IAccountsQueries accountsQueries)
    : ICustomersQueries
{
    public async Task<CustomerResult?> GetCustomer(Guid customerId)
    {
        Customer? customer = context
            .Customers
            .SingleOrDefault(e => e.Id == customerId);

        if (customer == null)
            throw new CustomerNotFoundException($"The customer {customerId} does not exists or is not processed yet.");

        List<AccountResult> accountsResult = [];

        foreach (Guid accountId in customer.Accounts)
        {
            AccountResult? accountResult = await accountsQueries.GetAccount(accountId);
            accountsResult.Add(accountResult);
        }

        CustomerResult customerResult = new(
            customer.Id, customer.Name, customer.SSN,
            accountsResult);

        return await Task.FromResult(customerResult);
    }
}