﻿namespace Acerola.Infrastructure.EntityFrameworkDataAccess.Entities;

public class Credit
{
    public Guid Id { get; set; }
    public Guid AccountId { get; set; }
    public double Amount { get; set; }
    public DateTime TransactionDate { get; set; }
}