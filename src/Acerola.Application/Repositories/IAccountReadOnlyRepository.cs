namespace Acerola.Application.Repositories;

public interface IAccountReadOnlyRepository
{
    Task<Account?> Get(Guid id);        
}