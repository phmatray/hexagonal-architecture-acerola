﻿namespace Acerola.Domain.Accounts;

public sealed class Account : IAggregateRoot
{
    public Guid Id { get; private set; }
    public Guid CustomerId { get; private set; }
    public IReadOnlyCollection<ITransaction> GetTransactions()
    {
        IReadOnlyCollection<ITransaction> readOnly = _transactions.GetTransactions();
        return readOnly;
    }

    private TransactionCollection _transactions;

    public Account(Guid customerId)
    {
        Id = Guid.NewGuid();
        _transactions = new TransactionCollection();
        CustomerId = customerId;
    }

    public void Deposit(Amount amount)
    {
        Credit credit = new Credit(Id, amount);
        _transactions.Add(credit);
    }

    public void Withdraw(Amount amount)
    {
        if (_transactions.GetCurrentBalance() < amount)
        {
            throw new InsufficientFundsException(Id, amount);
        }

        Debit debit = new Debit(Id, amount);
        _transactions.Add(debit);
    }

    public void Close()
    {
        if (_transactions.GetCurrentBalance() > 0)
        {
            throw new AccountCannotBeClosedException(Id);
        }
    }

    public Amount GetCurrentBalance()
    {
        Amount totalAmount = _transactions.GetCurrentBalance();
        return totalAmount;
    }

    public ITransaction GetLastTransaction()
    {
        ITransaction transaction = _transactions.GetLastTransaction();
        return transaction;
    }

    private Account() { }

    public static Account Load(Guid id, Guid customerId, TransactionCollection transactions)
    {
        Account account = new()
        {
            Id = id,
            CustomerId = customerId,
            _transactions = transactions
        };
        return account;
    }
}