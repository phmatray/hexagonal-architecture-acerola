using Acerola.Infrastructure.MongoDataAccess.Entities;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;

namespace Acerola.Infrastructure.MongoDataAccess;

public class Context
{
    private readonly MongoClient _mongoClient;
    private readonly IMongoDatabase _database;

    public Context(string connectionString, string databaseName)
    {
        _mongoClient = new MongoClient(connectionString);
        _database = _mongoClient.GetDatabase(databaseName);
        Map();
    }

    public IMongoCollection<Customer> Customers
        => _database.GetCollection<Customer>("Customers");

    public IMongoCollection<Account> Accounts
        => _database.GetCollection<Account>("Accounts");

    public IMongoCollection<Credit> Credits
        => _database.GetCollection<Credit>("Credits");

    public IMongoCollection<Debit> Debits
        => _database.GetCollection<Debit>("Debits");

    private static void Map()
    {
        BsonClassMap.RegisterClassMap<Account>(cm => { cm.AutoMap(); });
        BsonClassMap.RegisterClassMap<Credit>(cm => { cm.AutoMap(); });
        BsonClassMap.RegisterClassMap<Debit>(cm => { cm.AutoMap(); });
        BsonClassMap.RegisterClassMap<Customer>(cm => { cm.AutoMap(); });
    }
}