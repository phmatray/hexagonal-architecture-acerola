namespace Acerola.Application.Commands.Register;

public sealed class RegisterUseCase(
    ICustomerWriteOnlyRepository customerWriteOnlyRepository,
    IAccountWriteOnlyRepository accountWriteOnlyRepository)
    : IRegisterUseCase
{
    public async Task<RegisterResult> Execute(string pin, string name, double initialAmount)
    {
        Customer customer = new Customer(pin, name);

        Account account = new Account(customer.Id);
        account.Deposit(initialAmount);
        Credit credit = (Credit)account.GetLastTransaction();

        customer.Register(account.Id);

        await customerWriteOnlyRepository.Add(customer);
        await accountWriteOnlyRepository.Add(account, credit);

        RegisterResult result = new RegisterResult(customer, account);
        return result;
    }
}