using System.Data;
using Acerola.Application.Queries;
using Acerola.Application.Results;
using Acerola.Domain.Accounts;
using Dapper;
using Microsoft.Data.SqlClient;

namespace Acerola.Infrastructure.DapperDataAccess.Queries;

public class AccountsQueries(string connectionString)
    : IAccountsQueries
{
    public async Task<AccountResult?> GetAccount(Guid accountId)
    {
        using IDbConnection db = new SqlConnection(connectionString);
        
        const string accountSQL = 
            "SELECT * FROM Account WHERE Id = @accountId";
        
        Entities.Account? account = await db
            .QueryFirstOrDefaultAsync<Entities.Account>(accountSQL, new { accountId });

        if (account == null)
        {
            return null;
        }

        const string credits = 
            "SELECT * FROM [Credit] WHERE AccountId = @accountId";

        List<ITransaction> transactionsList = [];

        using (var reader = await db.ExecuteReaderAsync(credits, new { accountId }))
        {
            var parser = reader.GetRowParser<Credit>();

            while (reader.Read())
            {
                ITransaction transaction = parser(reader);
                transactionsList.Add(transaction);
            }
        }

        const string debits = 
            "SELECT * FROM [Debit] WHERE AccountId = @accountId";

        using (var reader = await db.ExecuteReaderAsync(debits, new { accountId }))
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
        AccountResult accountResult = new(result);
        return accountResult;
    }
}