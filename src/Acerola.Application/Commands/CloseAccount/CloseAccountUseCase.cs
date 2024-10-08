﻿namespace Acerola.Application.Commands.CloseAccount;

public sealed class CloseAccountUseCase(
    IAccountReadOnlyRepository accountReadOnlyRepository,
    IAccountWriteOnlyRepository accountWriteOnlyRepository)
    : ICloseAccountUseCase
{
    public async Task<Guid> Execute(Guid accountId)
    {
        Account account =
            await accountReadOnlyRepository.Get(accountId)
            ?? throw new AccountNotFoundException(accountId);
			
        account.Close();

        await accountWriteOnlyRepository.Delete(account);

        return account.Id;
    }
}