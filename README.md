# Monobank.API
.NET Client for Monobank API

Using example:
```csharp
var xToken = "OHkXlpgO8bHp1IjYKIKCRAEFbYZwbnKz2Qu3ispKhWrc";

var client = new MonoClient(xToken);

var currencys = await client.GetCurrency();
var userInfo = await client.GetClientInfo();

var uahAccount ="8p74Sco2TTs1DmduFKnsrd";
var usdAccount ="1HszeB7PMfisUBEYm9EMqx";
var from = 1559347200;//Unix time
var statements = client.GetStatements(uahAccount, 1559347200);

client.OnError += (s,e)=> { Console.WriteLine("ERROR: " + e.Description); };
client.OnStatement += (s,e)=> { Console.WriteLine($"Account {e.Account}: {e.Statement.Description}: {e.Statement.Amount/100.0}"); };

client.StartReceiving(uahAcc, from);
client.StartReceiving(usdAccount, from);
```
