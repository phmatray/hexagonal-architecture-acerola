using Acerola.Application.Commands.Register;
using Acerola.WebApi.Model;
using Microsoft.AspNetCore.Mvc;

namespace Acerola.WebApi.UseCases.Register;

[Route("api/[controller]")]
public sealed class CustomersController(IRegisterUseCase registerService)
    : Controller
{
    /// <summary>
    /// Register a new Customer
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> Post([FromBody]RegisterRequest request)
    {
        RegisterResult result = await registerService.Execute(
            request.Personnummer, request.Name, request.InitialAmount);

        List<TransactionModel> transactions = [];

        foreach (var item in result.Account.Transactions)
        {
            var transaction = new TransactionModel(
                item.Amount,
                item.Description,
                item.TransactionDate);

            transactions.Add(transaction);
        }

        AccountDetailsModel account = new AccountDetailsModel(
            result.Account.AccountId,
            result.Account.CurrentBalance,
            transactions);

        List<AccountDetailsModel> accounts =
        [
            account
        ];

        Model model = new Model(
            result.Customer.CustomerId,
            result.Customer.Personnummer,
            result.Customer.Name,
            accounts
        );

        return CreatedAtRoute("GetCustomer", new { customerId = model.CustomerId }, model);
    }
}