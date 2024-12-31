using EShopOnAbp.Shared.Hosting.Microservices.DbMigrations.EfCore;
using EShopOnAbp.WorkflowService.Domain;
using EShopOnAbp.WorkflowService.EntityFrameworkCore;
using Volo.Abp.DistributedLocking;
using Volo.Abp.EventBus.Distributed;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Uow;

namespace EShopOnAbp.WorkflowService.HttpApi.Host.DbMigration
{
  public class WorkflowServiceDatabaseMigrationChecker:PendingEfCoreMigrationsChecker<WorkflowServiceDbContext>
  {
    public WorkflowServiceDatabaseMigrationChecker(IUnitOfWorkManager unitOfWorkManager,
        IServiceProvider serviceProvider,
        ICurrentTenant currentTenant,
        IDistributedEventBus distributedEventBus,
        IAbpDistributedLock abpDistributedLock) : base(
            unitOfWorkManager,
            serviceProvider,
            currentTenant,
            distributedEventBus,
            abpDistributedLock,
            WorkflowServiceDbProperties.ConnectionStringName)
    {
          
    }
  }
}
