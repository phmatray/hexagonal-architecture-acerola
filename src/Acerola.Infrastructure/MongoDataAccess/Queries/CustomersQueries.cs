using Acerola.Application.Queries;
using Acerola.Application.Results;
using Acerola.Infrastructure.MongoDataAccess.Entities;
using MongoDB.Driver;

namespace Acerola.Infrastructure.MongoDataAccess.Queries;

public class CustomersQueries(Context context, IAccountsQueries accountsQueries)
    : ICustomersQueries
{
    public async Task<CustomerResult?> GetCustomer(Guid customerId)
    {
        Customer customer = await context
            .Customers
            .Find(e => e.Id == customerId)
            .SingleOrDefaultAsync();

        if (customer == null)
            throw new CustomerNotFoundException($"The customer {customerId} does not exists or is not processed yet.");

        List<Guid> accountIds = await context
            .Accounts
            .Find(e => e.CustomerId == customerId)
            .Project(p => p.Id)
            .ToListAsync();

        List<AccountResult> accountsResult = [];

        foreach (Guid accountId in accountIds)
        {
            AccountResult? accountResult = await accountsQueries.GetAccount(accountId);
            accountsResult.Add(accountResult);
        }

        CustomerResult customerResult = new(
            customer.Id, customer.Name, customer.SSN,
            accountsResult);

        return customerResult;
    }
}