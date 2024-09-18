using Acerola.Application.Repositories;
using Acerola.Domain.Accounts;
using System.Data;
using Dapper;
using Microsoft.Data.SqlClient;

namespace Acerola.Infrastructure.DapperDataAccess.Repositories;

public class AccountRepository(string connectionString)
    : IAccountReadOnlyRepository, IAccountWriteOnlyRepository
{
    public async Task Add(Account account, Credit credit)
    {
        using IDbConnection db = new SqlConnection(connectionString);
        
        const string insertAccountSQL = 
            "INSERT INTO Account (Id, CustomerId) VALUES (@Id, @CustomerId)";

        DynamicParameters accountParameters = new();
        accountParameters.Add("@id", account.Id);
        accountParameters.Add("@customerId", account.CustomerId);

        await db.ExecuteAsync(insertAccountSQL, accountParameters);

        const string insertCreditSQL =
            """
            INSERT INTO [Credit] (Id, Amount, TransactionDate, AccountId) 
            VALUES (@Id, @Amount, @TransactionDate, @AccountId)
            """;

        DynamicParameters transactionParameters = new();
        transactionParameters.Add("@id", credit.Id);
        transactionParameters.Add("@amount", (double)credit.Amount);
        transactionParameters.Add("@transactionDate", credit.TransactionDate);
        transactionParameters.Add("@accountId", credit.AccountId);

        await db.ExecuteAsync(insertCreditSQL, transactionParameters);
    }

    public async Task Delete(Account account)
    {
        using IDbConnection db = new SqlConnection(connectionString);
        
        const string deleteSQL =
            """
            DELETE FROM [Credit] WHERE AccountId = @Id;
            DELETE FROM [Debit] WHERE AccountId = @Id;
            DELETE FROM Account WHERE Id = @Id;
            """;
        
        await db.ExecuteAsync(deleteSQL, account);
    }

    public async Task<Account?> Get(Guid id)
    {
        using IDbConnection db = new SqlConnection(connectionString);
        
        const string accountSQL =
            "SELECT * FROM Account WHERE Id = @Id";
            
        Entities.Account? account = await db
            .QueryFirstOrDefaultAsync<Entities.Account>(accountSQL, new { id });

        if (account == null)
            return null;

        const string credits = 
            "SELECT * FROM [Credit] WHERE AccountId = @Id";

        List<ITransaction> transactionsList = [];

        using (var reader = await db.ExecuteReaderAsync(credits, new { id }))
        {
            var parser = reader.GetRowParser<Credit>();

            while (reader.Read())
            {
                ITransaction transaction = parser(reader);
                transactionsList.Add(transaction);
            }
        }

        const string debits =
            "SELECT * FROM [Debit] WHERE AccountId = @Id";

        using (var reader = await db.ExecuteReaderAsync(debits, new { id }))
        {
            var parser = reader.GetRowParser<Debit>();

            while (reader.Read())
            {
                ITransaction transaction = parser(reader);
                transactionsList.Add(transaction);
            }
        }

        TransactionCollection transactionCollection = new();

        foreach (var item in transactionsList.OrderBy(e => e.TransactionDate))
        {
            transactionCollection.Add(item);
        }

        Account result = Account.Load(account.Id, account.CustomerId, transactionCollection);
        return result;
    }

    public async Task Update(Account account, Credit credit)
    {
        using IDbConnection db = new SqlConnection(connectionString);
        
        const string insertCreditSQL =
            """
            INSERT INTO [Credit] (Id, Amount, TransactionDate, AccountId)
            VALUES (@Id, @Amount, @TransactionDate, @AccountId)
            """;

        DynamicParameters transactionParameters = new();
        transactionParameters.Add("@id", credit.Id);
        transactionParameters.Add("@amount", (double)credit.Amount);
        transactionParameters.Add("@transactionDate", credit.TransactionDate);
        transactionParameters.Add("@accountId", credit.AccountId);

        await db.ExecuteAsync(insertCreditSQL, transactionParameters);
    }

    public async Task Update(Account account, Debit debit)
    {
        using IDbConnection db = new SqlConnection(connectionString);
        
        const string insertDebitSQL =
            """
            INSERT INTO [Debit] (Id, Amount, TransactionDate, AccountId) 
            VALUES (@Id, @Amount, @TransactionDate, @AccountId)
            """;

        DynamicParameters transactionParameters = new();
        transactionParameters.Add("@id", debit.Id);
        transactionParameters.Add("@amount", (double)debit.Amount);
        transactionParameters.Add("@transactionDate", debit.TransactionDate);
        transactionParameters.Add("@accountId", debit.AccountId);

        await db.ExecuteAsync(insertDebitSQL, transactionParameters);
    }
}