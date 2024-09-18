using Acerola.Domain.Accounts;
using Acerola.Domain.Customers;
using System.Collections.ObjectModel;

namespace Acerola.Infrastructure.InMemoryDataAccess;

public class Context
{
    public Collection<Customer> Customers { get; set; } = [];
    public Collection<Account> Accounts { get; set; } = [];
}