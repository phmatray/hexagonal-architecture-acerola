namespace Acerola.WebApi.UseCases.Deposit;

internal sealed class Model(
    double amount,
    string description,
    DateTime transactionDate,
    double updatedBalance)
{
    public double Amount { get; } = amount;
    public string Description { get; } = description;
    public DateTime TransactionDate { get; } = transactionDate;
    public double UpdateBalance { get; } = updatedBalance;
}