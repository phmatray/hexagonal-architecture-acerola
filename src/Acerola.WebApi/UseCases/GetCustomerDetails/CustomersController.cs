using Acerola.Application.Queries;
using Acerola.Application.Results;
using Acerola.WebApi.Model;
using Microsoft.AspNetCore.Mvc;

namespace Acerola.WebApi.UseCases.GetCustomerDetails;

[Route("api/[controller]")]
public sealed class CustomersController(ICustomersQueries customersQueries)
    : Controller
{
    /// <summary>
    /// Get a Customer details 
    /// </summary>
    [HttpGet("{customerId}", Name = "GetCustomer")]
    public async Task<IActionResult> GetCustomer(Guid customerId)
    {
        CustomerResult? customer = await customersQueries.GetCustomer(customerId);

        if (customer == null)
        {
            return new NoContentResult();
        }

        List<AccountDetailsModel> accounts = [];

        foreach (var account in customer.Accounts)
        {
            List<TransactionModel> transactions = [];

            foreach (var item in account.Transactions)
            {
                var transaction = new TransactionModel(
                    item.Amount,
                    item.Description,
                    item.TransactionDate);

                transactions.Add(transaction);
            }

            accounts.Add(new AccountDetailsModel(
                account.AccountId,
                account.CurrentBalance,
                transactions));
        }

        CustomerDetailsModel model = new CustomerDetailsModel(
            customer.CustomerId,
            customer.Personnummer,
            customer.Name,
            accounts
        );

        return new ObjectResult(model);
    }
}