using Acerola.Domain.Accounts;
using Acerola.Application.Repositories;
using MongoDB.Driver;

namespace Acerola.Infrastructure.MongoDataAccess.Repositories;

public class AccountRepository(Context context)
    : IAccountReadOnlyRepository, IAccountWriteOnlyRepository
{
    public async Task Add(Account account, Credit credit)
    {
        Entities.Account accountEntity = new()
        {
            CustomerId = account.CustomerId,
            Id = account.Id
        };

        Entities.Credit creditEntity = new()
        {
            AccountId = credit.AccountId,
            Amount = credit.Amount,
            Description = credit.Description,
            Id = credit.Id,
            TransactionDate = credit.TransactionDate
        };

        await context.Accounts.InsertOneAsync(accountEntity);
        await context.Credits.InsertOneAsync(creditEntity);
    }

    public async Task Delete(Account account)
    {
        await context.Credits.DeleteOneAsync(e => e.AccountId == account.Id);
        await context.Debits.DeleteOneAsync(e => e.AccountId == account.Id);
        await context.Accounts.DeleteOneAsync(e => e.Id == account.Id);
    }

    public async Task<Account?> Get(Guid id)
    {
        Entities.Account account = await context
            .Accounts
            .Find(e => e.Id == id)
            .SingleOrDefaultAsync();

        List<Entities.Credit> credits = await context
            .Credits
            .Find(e => e.AccountId == id)
            .ToListAsync();

        List<Entities.Debit> debits = await context
            .Debits
            .Find(e => e.AccountId == id)
            .ToListAsync();

        List<ITransaction> transactions = new();

        foreach (Entities.Credit transactionData in credits)
        {
            Credit transaction = Credit.Load(
                transactionData.Id,
                transactionData.AccountId,
                transactionData.Amount,
                transactionData.TransactionDate);

            transactions.Add(transaction);
        }

        foreach (Entities.Debit transactionData in debits)
        {
            Debit transaction = Debit.Load(
                transactionData.Id,
                transactionData.AccountId,
                transactionData.Amount,
                transactionData.TransactionDate);

            transactions.Add(transaction);
        }

        var orderedTransactions = transactions.OrderBy(o => o.TransactionDate).ToList();

        TransactionCollection transactionCollection = new();
        transactionCollection.Add(orderedTransactions);

        Account result = Account.Load(
            account.Id,
            account.CustomerId,
            transactionCollection);

        return result;
    }

    public async Task Update(Account account, Credit credit)
    {
        Entities.Credit creditEntity = new()
        {
            AccountId = credit.AccountId,
            Amount = credit.Amount,
            Description = credit.Description,
            Id = credit.Id,
            TransactionDate = credit.TransactionDate
        };

        await context.Credits.InsertOneAsync(creditEntity);
    }

    public async Task Update(Account account, Debit debit)
    {
        Entities.Debit debitEntity = new()
        {
            AccountId = debit.AccountId,
            Amount = debit.Amount,
            Description = debit.Description,
            Id = debit.Id,
            TransactionDate = debit.TransactionDate
        };

        await context.Debits.InsertOneAsync(debitEntity);
    }
}