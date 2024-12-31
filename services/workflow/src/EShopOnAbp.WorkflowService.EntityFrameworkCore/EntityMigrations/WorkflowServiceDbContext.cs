using Elsa.Persistence.EntityFramework.Core.Extensions; // For Elsa persistence setup
using EShopOnAbp.WorkflowService.Domain;
using EShopOnAbp.WorkflowService.EntityFrameworkCore.Entity;
using EShopOnAbp.WorkflowService.EntityFrameworkCore.EntityMigrations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using System.Runtime.InteropServices.Marshalling;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace EShopOnAbp.WorkflowService.EntityFrameworkCore
{
  [ConnectionStringName(WorkflowServiceDbProperties.ConnectionStringName)]
  public class WorkflowServiceDbContext : AbpDbContext<WorkflowServiceDbContext>, IWorkflowServiceDbContext
  {
    public WorkflowServiceDbContext(DbContextOptions<WorkflowServiceDbContext> options) : base(options)
    {
    }

    public virtual DbSet<WorkflowEngine> WorkflowEngines { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
      base.OnModelCreating(modelBuilder);

      // Configure Elsa persistence
      //modelBuilder.(options =>
      //{
      //  options.UseEntityFrameworkCorePersistence();
      //});

      // Additional model configurations (if needed)
      // Example: modelBuilder.Entity<YourEntity>().ToTable("YourTable");
    }
  }
}
