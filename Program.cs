using Repro;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.Sqlite;

using var connection = new SqliteConnection("Data Source=:memory:");
connection.Open();
{
    // Setup database structure
    using var contextSetup = new MyDbContext(connection);
    contextSetup.Database.EnsureCreated();
}

var one = new One { Id = 1 };
var two1 = new Two { Id = 1 };
var two2 = new Two { Id = 2 };
var two3 = new Two { Id = 3 };
var two4 = new Two { Id = 4 };
one.OneTwos = new()
{
    new() { One = one, Two = two1, Order = 0 },
    new() { One = one, Two = two2, Order = 1 },
    new() { One = one, Two = two3, Order = 2 },
};

{
    // Add existing entities
    using var context = new MyDbContext(connection);

    context.Ones.Add(one);
    context.Add(two4);
    context.SaveChanges();
}

var newOrder = new List<int>
{
    two2.Id,
    two1.Id,
    two4.Id,
};

{
    // Reorder the referenced `Two`s according to the new order

    using var context = new MyDbContext(connection);
    var o = context.Ones.Include(_ => _.OneTwos).SingleOrDefault(_ => _.Id == one.Id)!;
    o.OneTwos.Clear();
    var order = 0;
    foreach (var id in newOrder)
    {
        o.OneTwos.Add(new() { One = one, TwoId = id, Order = order });
        order++;
    }

    context.SaveChanges();
}

{
    // Show the result
    using var c = new MyDbContext(connection);
    var query = c.OneTwos
        .OrderBy(x => x.Order)
        .Select(x => new { x.Order, x.Two.Id });
    foreach (var entry in query)
        Console.WriteLine($"Order: {entry.Order}; Two: {entry.Id}");
}