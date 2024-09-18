using Acerola.Application.Queries;
using Acerola.Application.Results;
using Acerola.Domain.Accounts;
using Microsoft.EntityFrameworkCore;

namespace Acerola.Infrastructure.EntityFrameworkDataAccess.Queries;

public class AccountsQueries(Context context) : IAccountsQueries
{
    public async Task<AccountResult?> GetAccount(Guid accountId)
    {
        Entities.Account? account = await context
            .Accounts
            .FindAsync(accountId);

        List<Entities.Credit> credits = await context
            .Credits
            .Where(e => e.AccountId == accountId)
            .ToListAsync();

        List<Entities.Debit> debits = await context
            .Debits
            .Where(e => e.AccountId == accountId)
            .ToListAsync();

        List<ITransaction> transactions = [];

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

        var orderedTransactions = transactions
            .OrderBy(o => o.TransactionDate)
            .ToList();

        TransactionCollection transactionCollection = new();
        transactionCollection.Add(orderedTransactions);

        Account result = Account.Load(
            account.Id,
            account.CustomerId,
            transactionCollection);

        AccountResult re = new(result);
        return re;
    }
}