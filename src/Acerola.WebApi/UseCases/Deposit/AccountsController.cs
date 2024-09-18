using Acerola.Application.Commands.Deposit;
using Microsoft.AspNetCore.Mvc;

namespace Acerola.WebApi.UseCases.Deposit;

[Route("api/[controller]")]
public sealed class AccountsController(IDepositUseCase depositService)
    : Controller
{
    /// <summary>
    /// Deposit from an account
    /// </summary>
    [HttpPatch("Deposit")]
    public async Task<IActionResult> Deposit([FromBody]DepositRequest request)
    {
        DepositResult depositResult = await depositService.Execute(
            request.AccountId,
            request.Amount);

        if (depositResult == null)
        {
            return new NoContentResult();
        }

        Model model = new Model(
            depositResult.Transaction.Amount,
            depositResult.Transaction.Description,
            depositResult.Transaction.TransactionDate,
            depositResult.UpdatedBalance
        );

        return new ObjectResult(model);
    }
}