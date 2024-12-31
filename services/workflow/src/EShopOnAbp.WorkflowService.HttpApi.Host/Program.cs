using System;
using System.Threading.Tasks;
using EShopOnAbp.Shared.Hosting.AspNetCore;
using EShopOnAbp.WorkflowService.HttpApi.Host;
using Microsoft.AspNetCore.Builder;
using Serilog;

namespace EShopOnAbp.WorkflowService
{
  public class Program
  {
    public static async Task<int> Main(string[] args)
    {
      var assemblyName = typeof(Program).Assembly.GetName().Name;

      // Configure Serilog for application logging
      SerilogConfigurationHelper.Configure(assemblyName);

      try
      {
        Log.Information($"Starting {assemblyName}.");

        // Build the application
        var app = await ApplicationBuilderHelper
            .BuildApplicationAsync<WorkflowServiceHttpApiHostModule>(args);

        // Initialize application services
        await app.InitializeApplicationAsync();

        // Run the application
        await app.RunAsync();

        return 0; // Successful execution
      }
      catch (Exception ex)
      {
        Log.Fatal(ex, $"{assemblyName} terminated unexpectedly!");

        // Provide detailed diagnostics
        Console.WriteLine("Application failed to start due to an error:");
        Console.WriteLine(ex.Message);
        if (ex.InnerException != null)
        {
          Console.WriteLine("Inner Exception:");
          Console.WriteLine(ex.InnerException.Message);
        }
        Console.WriteLine(ex.StackTrace);

        return 1; // Exit with failure code
      }
      finally
      {
        Log.CloseAndFlush(); // Ensure logs are properly flushed before exit
      }
    }
  }
}
