using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SKD.Operations;
using StrawberryShake;

Console.WriteLine("start");
// Setup IConfiguration
var configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
    .AddJsonFile("appsettings.Development.json", optional: true, reloadOnChange: true)
    .AddEnvironmentVariables()
    .Build();

// get configuration setting "AppTitle"
var skdServerGraphqlUrl = configuration["SkdGraphqlServerUrl"] ?? "";

Console.WriteLine($"skdServerGraphqlUrl: {skdServerGraphqlUrl}");

var services = new ServiceCollection();
services.AddSkdGqlClient(ExecutionStrategy.CacheAndNetwork)
    .ConfigureHttpClient(c => c.BaseAddress = new Uri(skdServerGraphqlUrl));


var serviceProvider = services.BuildServiceProvider();
var client = serviceProvider.GetRequiredService<ISkdGqlClient>();

var result = await client.GetComponents.ExecuteAsync();

if (result.Data?.Components?.Nodes is not null)
{
    foreach (var component in result.Data.Components.Nodes)
    {
        Console.WriteLine(component.Name);
    }
}


