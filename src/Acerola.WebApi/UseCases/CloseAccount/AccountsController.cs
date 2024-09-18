using Acerola.Application.Commands.CloseAccount;
using Microsoft.AspNetCore.Mvc;

namespace Acerola.WebApi.UseCases.CloseAccount;

[Route("api/[controller]")]
public sealed class AccountsController(ICloseAccountUseCase closeService)
    : Controller
{
    /// <summary>
    /// Close an account
    /// </summary>
    [HttpDelete("{accountId}")]
    public async Task<IActionResult> Close(Guid accountId)
    {
        Guid closeResult = await closeService.Execute(accountId);

        if (closeResult == Guid.Empty)
        {
            return new NoContentResult();
        }

        return Ok();
    }
}