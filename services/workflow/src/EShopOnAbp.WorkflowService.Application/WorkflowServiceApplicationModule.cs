using EShopOnAbp.WorkflowService.Domain;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application;
using Volo.Abp.AutoMapper;
using Volo.Abp.Modularity;

namespace EShopOnAbp.WorkflowService.Application
{
  [DependsOn(typeof(AbpDddApplicationModule), typeof(WorkflowServiceDomainModule))]
  public class WorkflowServiceApplicationModule:AbpModule
    {
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
      context.Services.AddAutoMapperObjectMapper<WorkflowServiceApplicationModule>();
      Configure<AbpAutoMapperOptions>(options =>
      {
        options.AddMaps<WorkflowServiceApplicationModule>(validate: true);
      });
    }
  }
}
