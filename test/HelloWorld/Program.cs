
using Aptabase.Core;
using Microsoft.Extensions.DependencyInjection;

// Microsoft dependency injection semantics
var services = new ServiceCollection();

services.AddLogging(); 

services.UseAptabase("<YOUR_APP_KEY>", new AptabaseOptions
{
#if DEBUG
            IsDebugMode = true,
#else
    IsDebugMode = false,
#endif
    EnableCrashReporting = false,
    EnablePersistence = true,
});

// Build the service provider
var serviceProvider = services.BuildServiceProvider();

// Get an instance of the Aptabase service
var aptabaseClient = serviceProvider.GetRequiredService<IAptabaseClient>(); 

// Track a sample event
await aptabaseClient.TrackEvent("HelloWorldEvent", new Dictionary<string, object> 
{
    { "Message", "Hello from Aptabase!" }
});

Console.WriteLine("Hello, World!");
Console.ReadKey();