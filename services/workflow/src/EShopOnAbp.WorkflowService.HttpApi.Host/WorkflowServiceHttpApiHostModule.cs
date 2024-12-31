using Volo.Abp.Modularity;
using EShopOnAbp.WorkflowService.EntityFrameworkCore;
using EShopOnAbp.WorkflowService.Application;
using EShopOnAbp.Shared.Hosting.Microservices;
using Microsoft.AspNetCore.Cors;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc;
using EShopOnAbp.WorkflowService.HttpApi.Host.DbMigration;
using EShopOnAbp.Shared.Hosting.AspNetCore;
using Elsa.Extensions;
using Elsa.EntityFrameworkCore.PostgreSql;
using Elsa.EntityFrameworkCore.Modules.Management;
using Elsa.EntityFrameworkCore.Modules.Runtime;
using Microsoft.AspNetCore.Builder;
using Antlr4.Runtime.Misc;
using Elsa.EntityFrameworkCore.Extensions;

namespace EShopOnAbp.WorkflowService.HttpApi.Host
{
  [DependsOn(
      typeof(WorkflowServiceApplicationModule),
      typeof(WorkflowServiceEntityFrameworkCoreModule),
      typeof(EShopOnAbpSharedHostingMicroservicesModule)
  )]
  public class WorkflowServiceHttpApiHostModule : AbpModule
  {
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
      var configuration = context.Services.GetConfiguration();

      //// Configure JWT Authentication with a unique scheme

      ////JwtBearerConfigurationHelper.Configure(context, "WorkflowService");

      //// Configure Swagger with OIDC
      //SwaggerConfigurationHelper.ConfigureWithOidc(
      //    context: context,
      //    authority: configuration["AuthServer:Authority"]!,
      //    scopes: new[] { "WorkflowService" },
      //    discoveryEndpoint: configuration["AuthServer:MetadataAddress"],
      //    apiTitle: "Workflow Service API"
      //);
      context.Services.AddSwaggerGen(c =>
      {
        // Resolves conflicting actions by picking the first API description
        c.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());

        // Customizes schema IDs to avoid conflicts by using the full type name
        c.CustomSchemaIds(type => type.ToString());
      });
      //var builder = WebApplication.CreateBuilder(args);
      context.Services.AddElsa(elsa =>
      {
        // Configure Management layer to use EF Core.
        elsa.UseWorkflowManagement(management => management.UseEntityFrameworkCore(e =>e.UsePostgreSql(configuration.GetConnectionString("WorkflowService"))));
        

        // Configure Runtime layer to use EF Core.
        elsa.UseWorkflowRuntime(runtime => runtime.UseEntityFrameworkCore());

        // Default Identity features for authentication/authorization.
        elsa.UseIdentity(identity =>
        {
          identity.TokenOptions = options => options.SigningKey = "sufficiently-large-secret-signing-key"; // This key needs to be at least 256 bits long.
          identity.UseAdminUserProvider();
        });

        // Configure ASP.NET authentication/authorization.
        elsa.UseDefaultAuthentication(auth => auth.UseAdminApiKey());

        // Expose Elsa API endpoints.
        elsa.UseWorkflowsApi();

        // Setup a SignalR hub for real-time updates from the server.
        elsa.UseRealTimeWorkflows();

        // Enable C# workflow expressions
        elsa.UseCSharp();

        // Enable HTTP activities.
        elsa.UseHttp();

        // Use timer activities.
        elsa.UseScheduling();

        // Register custom activities from the application, if any.
        elsa.AddActivitiesFrom<WorkflowServiceHttpApiHostModule>();

        // Register custom workflows from the application, if any.
        elsa.AddWorkflowsFrom<WorkflowServiceHttpApiHostModule>();
      });
     context.Services.AddCors(cors => cors
    .AddDefaultPolicy(policy => policy
        .AllowAnyOrigin() // For demo purposes only. Use a specific origin instead.
        .AllowAnyHeader()
        .AllowAnyMethod()
        .WithExposedHeaders("x-elsa-workflow-instance-id")));
      context.Services.AddHealthChecks();
      //// Configure CORS
      //context.Services.AddCors(options =>
      //{
      //  options.AddDefaultPolicy(builder =>
      //  {
      //    builder
      //        .WithOrigins(
      //            configuration["App:CorsOrigins"]!
      //                .Split(",", StringSplitOptions.RemoveEmptyEntries)
      //                .Select(o => o.Trim().RemovePostFix("/"))
      //                .ToArray()
      //        )
      //        .WithAbpExposedHeaders()
      //        .SetIsOriginAllowedToAllowWildcardSubdomains()
      //        .AllowAnyHeader()
      //        .AllowAnyMethod()
      //        .AllowCredentials()
      //        .WithExposedHeaders("x-elsa-workflow-instance-id"); // Required for Elsa Studio
      //  });
      //});

      //// Configure Elsa workflows with PostgreSQL persistence
      //context.Services.AddElsa(elsa =>
      //{
      //  // Configure Management layer to use EF Core
      //  elsa.UseWorkflowManagement(management => management.UseEntityFrameworkCore());

      //  // Configure Runtime layer to use EF Core
      //  elsa.UseWorkflowRuntime(runtime => runtime.UseEntityFrameworkCore());

      //  // Default Identity features for authentication/authorization
      //  elsa.UseIdentity(identity =>
      //  {
      //    identity.TokenOptions = options => options.SigningKey = "sufficiently-large-secret-signing-key"; // Minimum 256-bit key
      //    identity.UseAdminUserProvider();
      //  });

      //  // Configure Elsa authentication with a unique scheme to avoid conflict
      //  elsa.UseDefaultAuthentication(auth => auth.UseAdminApiKey());


      //  // Expose Elsa API endpoints
      //  elsa.UseWorkflowsApi();

      //  // Setup SignalR hub for real-time updates
      //  elsa.UseRealTimeWorkflows();

      //  // Enable C# workflow expressions
      //  elsa.UseCSharp();

      //  // Enable HTTP activities
      //  elsa.UseHttp();

      //  // Use timer activities
      //  elsa.UseScheduling();

      //  // Register custom activities and workflows
      //  elsa.AddActivitiesFrom<WorkflowServiceHttpApiHostModule>();
      //  elsa.AddWorkflowsFrom<WorkflowServiceHttpApiHostModule>();
      //});

      //// Configure conventional controllers
      ////Configure<AbpAspNetCoreMvcOptions>(options =>
      ////{
      ////  options.ConventionalControllers.Create(typeof(WorkflowServiceApplicationModule).Assembly, opts =>
      ////  {
      ////    opts.RootPath = "workflow";
      ////    opts.RemoteServiceName = "Workflow";
      ////  });
      ////});
    }

    public override void OnApplicationInitialization(ApplicationInitializationContext context)
    {
      var app = context.GetApplicationBuilder();
      var env = context.GetEnvironment();

      // Use developer exception page for development environment
      if (env.IsDevelopment())
      {
        app.UseDeveloperExceptionPage();
      }

      // Add Swagger and configure Swagger UI
      app.UseSwagger();
      app.UseAbpSwaggerWithCustomScriptUI(options =>
      {
        var configuration = context.ServiceProvider.GetRequiredService<IConfiguration>();
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "Workflow Service API");
        options.OAuthClientId(configuration["AuthServer:SwaggerClientId"]); // Use OAuth if required
      });

      // Configure routing (required for SignalR)
      app.UseRouting();

      // Serve static files (required for Swagger UI and other static resources)
      app.UseStaticFiles();

      // Apply CORS policies if required
      app.UseCors();

      // Authentication and Authorization
      app.UseAuthentication();
      app.UseAuthorization();

      // Use Elsa API Endpoints for Workflow API
      app.UseWorkflowsApi();

      // Use Elsa middleware for handling HTTP Endpoint activities
      app.UseWorkflows();

      // Use SignalR hubs for real-time updates (optional, used by Elsa Studio)
      app.UseWorkflowsSignalRHubs();

    }

    //public override async Task OnPostApplicationInitializationAsync(ApplicationInitializationContext context)
    //{
    //  // Apply database migrations
    //  await context.ServiceProvider
    //      .GetRequiredService<WorkflowServiceDatabaseMigrationChecker>()
    //      .CheckAndApplyDatabaseMigrationsAsync();
    //}
  }
}
