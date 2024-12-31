using EShopOnAbp.WorkflowService.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EShopOnAbp.WorkflowService.EntityFrameworkCore.EntityMigrations
{
  public class WorkflowServiceDbContextFactory: IDesignTimeDbContextFactory<WorkflowServiceDbContext>
  {
    public WorkflowServiceDbContext CreateDbContext(string[] args)
    {
      WorkflowServiceEfCoreEntityExtensionMappings.Configure();

      var configuration = BuildConfiguration();

      var builder = new DbContextOptionsBuilder<WorkflowServiceDbContext>()
          .UseNpgsql(
              configuration.GetConnectionString(WorkflowServiceDbProperties.ConnectionStringName),
              b =>
              {
                b.MigrationsHistoryTable("__workflowService_Migrations");
              });

      // https://www.npgsql.org/efcore/release-notes/6.0.html#opting-out-of-the-new-timestamp-mapping-logic
      AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

      return new WorkflowServiceDbContext(builder.Options);
    }

    private static IConfigurationRoot BuildConfiguration()
    {
      var builder = new ConfigurationBuilder()
          .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "../EShopOnAbp.WorkflowService.HttpApi.Host/"))
          .AddJsonFile("appsettings.json", optional: false);

      return builder.Build();
    }
  }
}
