namespace Acerola.Application.Commands.Deposit;

public sealed class DepositUseCase(
    IAccountReadOnlyRepository accountReadOnlyRepository,
    IAccountWriteOnlyRepository accountWriteOnlyRepository)
    : IDepositUseCase
{
    public async Task<DepositResult> Execute(Guid accountId, Amount amount)
    {
        Account account =
            await accountReadOnlyRepository.Get(accountId)
            ?? throw new AccountNotFoundException($"The account {accountId} does not exists or is already closed.");

        account.Deposit(amount);
        Credit credit = (Credit)account.GetLastTransaction();

        await accountWriteOnlyRepository.Update(
            account,
            credit);

        DepositResult result = new DepositResult(
            credit,
            account.GetCurrentBalance());
        return result;
    }
}