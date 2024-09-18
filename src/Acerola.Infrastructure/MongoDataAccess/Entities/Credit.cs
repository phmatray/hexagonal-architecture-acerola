﻿namespace Acerola.Infrastructure.MongoDataAccess.Entities;

public class Credit
{
    public Guid Id { get; set; }
    public Guid AccountId { get; set; }
    public double Amount { get; set; }
    public string Description { get; set; }
    public DateTime TransactionDate { get; set; }
}