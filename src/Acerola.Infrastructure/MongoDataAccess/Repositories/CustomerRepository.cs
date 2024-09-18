using Acerola.Application.Repositories;
using Acerola.Domain.Customers;
using MongoDB.Driver;

namespace Acerola.Infrastructure.MongoDataAccess.Repositories;

public class CustomerRepository(Context context)
    : ICustomerReadOnlyRepository, ICustomerWriteOnlyRepository
{
    public async Task<Customer?> Get(Guid customerId)
    {
        Entities.Customer customer = await context.Customers
            .Find(e => e.Id == customerId)
            .SingleOrDefaultAsync();

        List<Guid> accounts = await context.Accounts
            .Find(e => e.CustomerId == customerId)
            .Project(p => p.Id)
            .ToListAsync();

        AccountCollection accountCollection = new();
        foreach (var accountId in accounts)
            accountCollection.Add(accountId);

        return Customer.Load(customer.Id, customer.Name, customer.SSN, accountCollection);
    }

    public async Task Add(Customer customer)
    {
        Entities.Customer customerEntity = new()
        {
            Id = customer.Id,
            Name = customer.Name,
            SSN = customer.SSN
        };

        await context.Customers
            .InsertOneAsync(customerEntity);
    }

    public async Task Update(Customer customer)
    {
        Entities.Customer customerEntity = new()
        {
            Id = customer.Id,
            Name = customer.Name,
            SSN = customer.SSN
        };

        await context.Customers
            .ReplaceOneAsync(e => e.Id == customer.Id, customerEntity);
    }
}