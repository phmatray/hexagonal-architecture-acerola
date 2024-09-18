namespace Acerola.WebApi.UseCases.Deposit;

public sealed class DepositRequest
{
    public Guid AccountId { get; set; }
    public Double Amount { get; set; }
}