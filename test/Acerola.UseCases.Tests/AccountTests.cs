using Acerola.Domain.Customers;
using Acerola.Application.Commands.Deposit;
using Acerola.Domain.Accounts;

namespace Acerola.UseCases.Tests;

public class AccountTests
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
    [InlineData("08724050601", "Ivan Paulovich", 100)]
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
        Assert.True(Guid.Empty != result.Customer.CustomerId);
        Assert.True(Guid.Empty != result.Account.AccountId);
    }

    [Theory]
    [InlineData("c725315a-1de6-4bf7-aecf-3af8f0083681", 100)]
    public async Task Deposit_Valid_Amount(string accountId, double amount)
    {
        var account = new Account(Guid.NewGuid());
        var customer = new Customer("08724050601", "Ivan Paulovich");

        AccountReadOnlyRepository
            .Get(Guid.Parse(accountId))
            .Returns(account);

        var depositUseCase = new DepositUseCase(
            AccountReadOnlyRepository,
            AccountWriteOnlyRepository
        );

        DepositResult result = await depositUseCase
            .Execute(Guid.Parse(accountId), amount);

        Assert.Equal(amount, result.Transaction.Amount);
    }

    [Theory]
    [InlineData(100)]
    public void Account_With_Credits_Should_Not_Allow_Close(double amount)
    {
        var account = new Account(Guid.NewGuid());
        account.Deposit(amount);

        Assert.Throws<AccountCannotBeClosedException>(
            () => account.Close());
    }
}