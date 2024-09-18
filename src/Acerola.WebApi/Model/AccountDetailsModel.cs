namespace Acerola.WebApi.Model;

public sealed class AccountDetailsModel(
    Guid accountId,
    double currentBalance,
    List<TransactionModel> transactions)
{
    public Guid AccountId { get; } = accountId;
    public double CurrentBalance { get; } = currentBalance;
    public List<TransactionModel> Transactions { get; } = transactions;
}