﻿using Acerola.Application.Queries;
using Acerola.WebApi.Model;
using Microsoft.AspNetCore.Mvc;

namespace Acerola.WebApi.UseCases.GetAccountDetails;

[Route("api/[controller]")]
public sealed class AccountsController(IAccountsQueries accountsQueries)
    : Controller
{
    /// <summary>
    /// Get an account balance
    /// </summary>
    [HttpGet("{accountId}", Name = "GetAccount")]
    public async Task<IActionResult> Get(Guid accountId)
    {
        var account = await accountsQueries.GetAccount(accountId);

        List<TransactionModel> transactions = [];

        foreach (var item in account.Transactions)
        {
            var transaction = new TransactionModel(
                item.Amount,
                item.Description,
                item.TransactionDate);

            transactions.Add(transaction);
        }

        return new ObjectResult(new AccountDetailsModel(
            account.AccountId,
            account.CurrentBalance,
            transactions));
    }
}