namespace Acerola.Application.Queries;

public interface IAccountsQueries
{
    Task<AccountResult> GetAccount(Guid accountId);
}