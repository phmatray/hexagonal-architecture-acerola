﻿using Acerola.Application.Repositories;
using Acerola.Domain.Customers;
using Microsoft.EntityFrameworkCore;

namespace Acerola.Infrastructure.EntityFrameworkDataAccess.Repositories;

public class CustomerRepository(Context context)
    : ICustomerReadOnlyRepository, ICustomerWriteOnlyRepository
{
    private readonly Context _context = context ?? throw new ArgumentNullException(nameof(context));

    public async Task Add(Customer customer)
    {
        Entities.Customer customerEntity = new()
        {
            Id = customer.Id,
            Name = customer.Name,
            SSN = customer.SSN
        };

        await _context.Customers.AddAsync(customerEntity);
        await _context.SaveChangesAsync();
    }

    public async Task<Customer?> Get(Guid id)
    {
        Entities.Customer? customer = await _context.Customers
            .FindAsync(id);

        List<Guid> accounts = await _context.Accounts
            .Where(e => e.CustomerId == id)
            .Select(p => p.Id)
            .ToListAsync();

        AccountCollection accountCollection = new();
        foreach (var accountId in accounts)
            accountCollection.Add(accountId);

        return Customer.Load(customer.Id, customer.Name, customer.SSN, accountCollection);
    }

    public async Task Update(Customer customer)
    {
        Entities.Customer customerEntity = new()
        {
            Id = customer.Id,
            Name = customer.Name,
            SSN = customer.SSN
        };

        _context.Customers.Update(customerEntity);
        await _context.SaveChangesAsync();
    }
}