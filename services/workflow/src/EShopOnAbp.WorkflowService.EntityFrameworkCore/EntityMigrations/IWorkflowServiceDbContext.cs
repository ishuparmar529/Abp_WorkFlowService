using EShopOnAbp.WorkflowService.EntityFrameworkCore.Entity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace EShopOnAbp.WorkflowService.EntityFrameworkCore.EntityMigrations
{
  [ConnectionStringName(Domain.WorkflowServiceDbProperties.ConnectionStringName)]
  public interface IWorkflowServiceDbContext: IEfCoreDbContext
  {
    DbSet<WorkflowEngine> WorkflowEngines { get; }
  }

}
