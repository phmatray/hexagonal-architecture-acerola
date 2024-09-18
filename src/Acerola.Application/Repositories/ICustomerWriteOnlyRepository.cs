namespace Acerola.Application.Repositories;

public interface ICustomerWriteOnlyRepository
{
    Task Add(Customer customer);
    Task Update(Customer customer);
}