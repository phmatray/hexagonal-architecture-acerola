namespace Acerola.UseCases.Tests;

public class CustomerTests
{
    public readonly IAccountReadOnlyRepository AccountReadOnlyRepository
        = Substitute.For<IAccountReadOnlyRepository>();
    
    public readonly IAccountWriteOnlyRepository AccountWriteOnlyRepository
        = Substitute.For<IAccountWriteOnlyRepository>();
    
    public readonly ICustomerReadOnlyRepository CustomerReadOnlyRepository
        = Substitute.For<ICustomerReadOnlyRepository>();
    
    public readonly ICustomerWriteOnlyRepository CustomerWriteOnlyRepository
        = Substitute.For<ICustomerWriteOnlyRepository>();

    [Theory]
    [InlineData("08724050601", "Ivan Paulovich", 300)]
    [InlineData("08724050601", "Ivan Paulovich Pinheiro Gomes", 100)]
    [InlineData("08724050601", "Ivan Paulovich", 500)]
    [InlineData("08724050601", "Ivan Paulovich", 10000)]
    public async Task Register_Valid_User_Account(string personnummer, string name, double amount)
    {
        var registerUseCase = new RegisterUseCase(
            CustomerWriteOnlyRepository,
            AccountWriteOnlyRepository
        );

        RegisterResult result = await registerUseCase
            .Execute(personnummer, name, amount);

        Assert.Equal(personnummer, result.Customer.Personnummer);
        Assert.Equal(name, result.Customer.Name);
        Assert.True(result.Customer.CustomerId != Guid.Empty);
        Assert.True(result.Account.AccountId != Guid.Empty);
    }
}