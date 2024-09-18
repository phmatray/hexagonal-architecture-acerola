namespace Acerola.WebApi.Model;

public sealed class TransactionModel(
    double amount,
    string description,
    DateTime transactionDate)
{
    public double Amount { get; } = amount;
    public string Description { get; } = description;
    public DateTime TransactionDate { get; } = transactionDate;
}