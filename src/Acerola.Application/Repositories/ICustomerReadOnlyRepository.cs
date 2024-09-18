namespace Acerola.Application.Repositories;

public interface ICustomerReadOnlyRepository
{
    Task<Customer?> Get(Guid id);
}