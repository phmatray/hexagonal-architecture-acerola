using Acerola.Application.Queries;
using Acerola.Application.Results;
using Acerola.Domain.Accounts;

namespace Acerola.Infrastructure.InMemoryDataAccess.Queries;

public class AccountsQueries(Context context)
    : IAccountsQueries
{
    public async Task<AccountResult?> GetAccount(Guid accountId)
    {
        Account? data = context
            .Accounts
            .SingleOrDefault(e => e.Id == accountId);

        if (data == null)
        {
            throw new AccountNotFoundException(accountId);
        }

        List<ITransaction> credits = data
            .GetTransactions()
            .Where(e => e is Credit)
            .ToList();

        List<ITransaction> debits = data
            .GetTransactions()
            .Where(e => e is Debit)
            .ToList();

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

        return await Task.FromResult(accountResult);
    }
}