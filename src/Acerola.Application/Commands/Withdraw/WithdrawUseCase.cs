namespace Acerola.Application.Commands.Withdraw;

public sealed class WithdrawUseCase(
    IAccountReadOnlyRepository accountReadOnlyRepository,
    IAccountWriteOnlyRepository accountWriteOnlyRepository)
    : IWithdrawUseCase
{
    public async Task<WithdrawResult> Execute(Guid accountId, Amount amount)
    {
        Account account =
            await accountReadOnlyRepository.Get(accountId)
            ?? throw new AccountNotFoundException(accountId);

        account.Withdraw(amount);
        Debit debit = (Debit)account.GetLastTransaction();

        await accountWriteOnlyRepository.Update(account, debit);

        WithdrawResult result = new WithdrawResult(
            debit,
            account.GetCurrentBalance()
        );

        return result;
    }
}