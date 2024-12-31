using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain;
using Volo.Abp.Modularity;

namespace EShopOnAbp.WorkflowService.Domain
{
  [DependsOn(typeof(AbpDddDomainModule))]
  public class WorkflowServiceDomainModule:AbpModule
    {
        
    }
}
