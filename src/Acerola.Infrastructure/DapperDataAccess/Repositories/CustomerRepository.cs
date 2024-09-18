using Acerola.Application.Repositories;
using Acerola.Domain.Customers;
using System.Data;
using Dapper;
using Microsoft.Data.SqlClient;

namespace Acerola.Infrastructure.DapperDataAccess.Repositories;

public class CustomerReadOnlyRepository(string connectionString)
    : ICustomerReadOnlyRepository, ICustomerWriteOnlyRepository
{
    public async Task Add(Customer customer)
    {
        using IDbConnection db = new SqlConnection(connectionString);
        
        const string insertCustomerSQL =
            "INSERT INTO Customer (Id, Name, SSN) VALUES (@Id, @Name, @SSN)";
        
        DynamicParameters customerParameters = new();
        customerParameters.Add("@id", customer.Id);
        customerParameters.Add("@name", (string)customer.Name, DbType.AnsiString);
        customerParameters.Add("@SSN", (string)customer.SSN, DbType.AnsiString);

        await db.ExecuteAsync(insertCustomerSQL, customerParameters);
    }

    public async Task<Customer?> Get(Guid id)
    {
        using IDbConnection db = new SqlConnection(connectionString);
        
        const string customerSQL = 
            "SELECT * FROM Customer WHERE Id = @Id";

        Entities.Customer? customer = await db
            .QueryFirstOrDefaultAsync<Entities.Customer>(customerSQL, new { id });
					
        if (customer == null)
            return null;

        const string accountSQL = 
            "SELECT * FROM Account WHERE CustomerId = @Id";

        IEnumerable<Guid> accounts = await db
            .QueryAsync<Guid>(accountSQL, new { id });

        AccountCollection accountCollection = new();

        foreach (Guid accountId in accounts)
        {
            accountCollection.Add(accountId);
        }

        Customer result = Customer.Load(
            customer.Id,
            customer.Name,
            customer.SSN,
            accountCollection);

        return result;
    }

    public async Task Update(Customer customer)
    {
        using IDbConnection db = new SqlConnection(connectionString);
        
        const string updateCustomerSQL = 
            "UPDATE Customer SET Name = @Name, SSN = @SSN WHERE Id = @Id";
            
        await db.ExecuteAsync(updateCustomerSQL, customer);
    }
}