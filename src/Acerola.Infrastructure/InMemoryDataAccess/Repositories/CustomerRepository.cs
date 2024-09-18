using Acerola.Application.Repositories;
using Acerola.Domain.Customers;

namespace Acerola.Infrastructure.InMemoryDataAccess.Repositories;

public class CustomerRepository(Context context)
    : ICustomerReadOnlyRepository, ICustomerWriteOnlyRepository
{
    public async Task Add(Customer customer)
    {
        context.Customers.Add(customer);
        await Task.CompletedTask;
    }

    public async Task<Customer?> Get(Guid id)
    {
        Customer? customer = context.Customers
            .SingleOrDefault(e => e.Id == id);

        return await Task.FromResult<Customer>(customer);
    }

    public async Task Update(Customer customer)
    {
        Customer? customerOld = context.Customers
            .SingleOrDefault(e => e.Id == customer.Id);

        customerOld = customer;
        await Task.CompletedTask;
    }
}