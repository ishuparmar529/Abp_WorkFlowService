using EShopOnAbp.WorkflowService.Domain;
using EShopOnAbp.WorkflowService.Domain.Shared;
using Localization.Resources.AbpUi;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.AspNetCore.Mvc.AntiForgery;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;

namespace EShopOnAbp.WorkflowService.EntityFrameworkCore
{
  [DependsOn(
    typeof(AbpEntityFrameworkCoreModule),
    typeof(WorkflowServiceDomainModule))]
  public class WorkflowServiceEntityFrameworkCoreModule: AbpModule  
  {
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
      WorkflowServiceEfCoreEntityExtensionMappings.Configure();
    }

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
      context.Services.AddAbpDbContext<WorkflowServiceDbContext>(options =>
      {
        /* Remove "includeAllEntities: true" to create
         * default repositories only for aggregate roots */
        options.AddDefaultRepositories(includeAllEntities: true);
      });

      // https://www.npgsql.org/efcore/release-notes/6.0.html#opting-out-of-the-new-timestamp-mapping-logic
      AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

      Configure<AbpDbContextOptions>(options =>
      {
        /* The main point to change your DBMS.
         * See also OrderingServiceMigrationsDbContextFactory for EF Core tooling. */
        options.UseNpgsql(b =>
        {
          b.MigrationsHistoryTable("__WorkflowService_Migrations");
        });
      });
      //context.Services.AddElsaApiEndpoints();

      //Configure<AbpAntiForgeryOptions>(options =>
      //{
      //  options.AutoValidateFilter = type => type.Assembly != typeof(ElsaApiOptions).Assembly;
      //});

      //Configure<AbpLocalizationOptions>(options =>
      //{
      //  options.Resources
      //      .Get<WorkflowServiceResource>()
      //      .AddBaseTypes(typeof(AbpUiResource));
      //});
    }
  }
  
}
