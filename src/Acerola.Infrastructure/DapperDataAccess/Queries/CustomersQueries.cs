using System.Data;
using Acerola.Application.Queries;
using Acerola.Application.Results;
using Dapper;
using Microsoft.Data.SqlClient;

namespace Acerola.Infrastructure.DapperDataAccess.Queries;

public class CustomersQueries(string connectionString, IAccountsQueries accountsQueries)
    : ICustomersQueries
{
    public async Task<CustomerResult?> GetCustomer(Guid customerId)
    {
        using IDbConnection db = new SqlConnection(connectionString);
        
        const string customerSQL = 
            "SELECT * FROM Customer WHERE Id = @customerId";
            
        Entities.Customer? customer = await db
            .QueryFirstOrDefaultAsync<Entities.Customer>(customerSQL, new { customerId });

        if (customer == null)
            return null;

        const string accountSQL = 
            "SELECT id FROM Account WHERE CustomerId = @customerId";
            
        IEnumerable<Guid> accounts = await db
            .QueryAsync<Guid>(accountSQL, new { customerId });

        List<AccountResult> accountCollection = [];

        foreach (Guid accountId in accounts)
        {
            accountCollection.Add(await accountsQueries.GetAccount(accountId));
        }

        CustomerResult customerResult = new(customer.Id,
            customer.Name,
            customer.SSN,
            accountCollection);

        return customerResult;
    }
}