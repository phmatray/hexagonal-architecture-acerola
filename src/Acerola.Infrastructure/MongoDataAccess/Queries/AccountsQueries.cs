using Acerola.Application.Queries;
using Acerola.Application.Results;
using Acerola.Infrastructure.MongoDataAccess.Entities;
using MongoDB.Driver;

namespace Acerola.Infrastructure.MongoDataAccess.Queries;

public class AccountsQueries(Context context)
    : IAccountsQueries
{
    public async Task<AccountResult?> GetAccount(Guid accountId)
    {
        Account data = await context
            .Accounts
            .Find(e => e.Id == accountId)
            .SingleOrDefaultAsync();

        if (data == null)
        {
            throw new AccountNotFoundException(accountId);
        }

        List<Credit> credits = await context
            .Credits
            .Find(e => e.AccountId == accountId)
            .ToListAsync();

        List<Debit> debits = await context
            .Debits
            .Find(e => e.AccountId == accountId)
            .ToListAsync();

        double credit = credits.Sum(c => c.Amount);
        double debit = debits.Sum(d => d.Amount);

        List<TransactionResult> transactionResults = [];

        foreach (Credit transaction in credits)
        {
            TransactionResult transactionResult = new(
                transaction.Description, transaction.Amount, transaction.TransactionDate);
            transactionResults.Add(transactionResult);
        }

        foreach (Debit transaction in debits)
        {
            TransactionResult transactionResult = new(
                transaction.Description, transaction.Amount, transaction.TransactionDate);
            transactionResults.Add(transactionResult);
        }

        List<TransactionResult> orderedTransactions = transactionResults
            .OrderBy(e => e.TransactionDate)
            .ToList();

        AccountResult accountResult = new(
            data.Id,
            credit - debit,
            orderedTransactions);

        return accountResult;
    }
}