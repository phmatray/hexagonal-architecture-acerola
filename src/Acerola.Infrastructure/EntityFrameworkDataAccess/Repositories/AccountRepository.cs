using Microsoft.Data.SqlClient;
using Acerola.Application.Repositories;
using Acerola.Domain.Accounts;
using Microsoft.EntityFrameworkCore;

namespace Acerola.Infrastructure.EntityFrameworkDataAccess;

public class AccountRepository(Context context)
    : IAccountReadOnlyRepository, IAccountWriteOnlyRepository
{
    private readonly Context _context = context ?? throw new ArgumentNullException(nameof(context));

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
            Id = credit.Id,
            TransactionDate = credit.TransactionDate
        };

        await _context.Accounts.AddAsync(accountEntity);
        await _context.Credits.AddAsync(creditEntity);
        await _context.SaveChangesAsync();
    }

    public async Task Delete(Account account)
    {
        const string deleteSQL =
            """
            DELETE FROM Credit WHERE AccountId = @Id;
            DELETE FROM Debit WHERE AccountId = @Id;
            DELETE FROM Account WHERE Id = @Id;
            """;

        var id = new SqlParameter("@Id", account.Id);

        await _context.Database.ExecuteSqlRawAsync(deleteSQL, id);
    }

    public async Task<Account?> Get(Guid id)
    {
        Entities.Account? account = await _context
            .Accounts
            .FindAsync(id);

        List<Entities.Credit> credits = await _context
            .Credits
            .Where(e => e.AccountId == id)
            .ToListAsync();

        List<Entities.Debit> debits = await _context
            .Debits
            .Where(e => e.AccountId == id)
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

        return result;
    }

    public async Task Update(Account account, Credit credit)
    {
        Entities.Credit creditEntity = new()
        {
            AccountId = credit.AccountId,
            Amount = credit.Amount,
            Id = credit.Id,
            TransactionDate = credit.TransactionDate
        };

        await _context.Credits.AddAsync(creditEntity);
        await _context.SaveChangesAsync();
    }

    public async Task Update(Account account, Debit debit)
    {
        Entities.Debit debitEntity = new()
        {
            AccountId = debit.AccountId,
            Amount = debit.Amount,
            Id = debit.Id,
            TransactionDate = debit.TransactionDate
        };

        await _context.Debits.AddAsync(debitEntity);
        await _context.SaveChangesAsync();
    }
}