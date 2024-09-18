using Acerola.Application.Queries;
using Acerola.Application.Results;
using Acerola.Infrastructure.EntityFrameworkDataAccess.Entities;
using Microsoft.EntityFrameworkCore;

namespace Acerola.Infrastructure.EntityFrameworkDataAccess.Queries;

public class CustomersQueries(Context context, IAccountsQueries accountsQueries)
    : ICustomersQueries
{
    public async Task<CustomerResult?> GetCustomer(Guid customerId)
    {
        Customer? customer = await context
            .Customers
            .FindAsync(customerId);

        List<Account> accounts = await context
            .Accounts
            .Where(e => e.CustomerId == customerId)
            .ToListAsync();

        if (customer == null)
        {
            throw new CustomerNotFoundException($"The customer {customerId} does not exists or is not processed yet.");
        }

        List<AccountResult> accountsResult = [];

        foreach (Account account in accounts)
        {
            AccountResult? accountResult = await accountsQueries.GetAccount(account.Id);
            accountsResult.Add(accountResult);
        }

        CustomerResult customerResult = new(
            customer.Id, customer.Name, customer.SSN,
            accountsResult);

        return await Task.FromResult(customerResult);
    }
}