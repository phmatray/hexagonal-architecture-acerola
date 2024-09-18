using Acerola.WebApi.Model;

namespace Acerola.WebApi.UseCases.Register;

internal sealed class Model(
    Guid customerId,
    string perssonnummer,
    string name,
    List<AccountDetailsModel> accounts)
{
    public Guid CustomerId { get; } = customerId;
    public string Personnummer { get; } = perssonnummer;
    public string Name { get; } = name;
    public List<AccountDetailsModel> Accounts { get; set; } = accounts;
}