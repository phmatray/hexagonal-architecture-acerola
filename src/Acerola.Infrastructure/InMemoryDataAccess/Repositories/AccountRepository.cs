using Acerola.Application.Repositories;
using Acerola.Domain.Accounts;

namespace Acerola.Infrastructure.InMemoryDataAccess.Repositories;

public class AccountRepository(Context context)
    : IAccountReadOnlyRepository, IAccountWriteOnlyRepository
{
    public async Task Add(Account account, Credit credit)
    {
        context.Accounts.Add(account);
        await Task.CompletedTask;
    }

    public async Task Delete(Account account)
    {
        Account? accountOld = context.Accounts
            .SingleOrDefault(e => e.Id == account.Id);

        context.Accounts.Remove(accountOld);

        await Task.CompletedTask;
    }

    public async Task<Account?> Get(Guid id)
    {
        Account? account = context.Accounts
            .SingleOrDefault(e => e.Id == id);

        return await Task.FromResult<Account>(account);
    }

    public async Task Update(Account account, Credit credit)
    {
        Account? accountOld = context.Accounts
            .SingleOrDefault(e => e.Id == account.Id);

        accountOld = account;
        await Task.CompletedTask;
    }

    public async Task Update(Account account, Debit debit)
    {
        Account? accountOld = context.Accounts
            .SingleOrDefault(e => e.Id == account.Id);

        accountOld = account;
        await Task.CompletedTask;
    }
}