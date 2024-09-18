namespace Acerola.WebApi.Model;

public sealed class CustomerDetailsModel(
    Guid customerId,
    string personnummer,
    string name,
    List<AccountDetailsModel> accounts)
{
    public Guid CustomerId { get; } = customerId;
    public string Personnummer { get; } = personnummer;
    public string Name { get; } = name;
    public List<AccountDetailsModel> Accounts { get; } = accounts;
}