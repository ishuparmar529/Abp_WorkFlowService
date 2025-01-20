using Volo.Abp.Modularity;
using EShopOnAbp.WorkflowService.EntityFrameworkCore;
using EShopOnAbp.WorkflowService.Application;
using EShopOnAbp.Shared.Hosting.Microservices;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Cors;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc;
using EShopOnAbp.WorkflowService.HttpApi.Host.DbMigration;
using EShopOnAbp.Shared.Hosting.AspNetCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using EShopOnAbp.WorkflowService.Application.Contracts.Workflow.Dto;
using EShopOnAbp.WorkflowService.Application.WorkflowService;
using Elsa.Extensions;
using Elsa.EntityFrameworkCore.Modules.Management;
using Elsa.EntityFrameworkCore.Modules.Runtime;
using Microsoft.OpenApi.Models;
using NetBox.Extensions;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace EShopOnAbp.WorkflowService.HttpApi.Host
{
  [DependsOn(
      typeof(WorkflowServiceApplicationModule), // Removed self-reference
      typeof(WorkflowServiceEntityFrameworkCoreModule),
      typeof(EShopOnAbpSharedHostingMicroservicesModule)
  )]
  public class WorkflowServiceHttpApiHostModule : AbpModule
  {
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
      JwtBearerConfigurationHelper.Configure(context, "WorkflowService");
      var configuration = context.Services.GetConfiguration();

      var hostingEnvironment = context.Services.GetHostingEnvironment();

      // Configure JWT Authentication
      //context.Services.AddDbContext<WorkflowServiceDbContext>(options =>
      //{
      //  // Use Npgsql for PostgreSQL
      //  options.UseNpgsql(configuration.GetConnectionString("WorkflowService"));
      //});
      // Swagger configuration
      //SwaggerConfigurationHelper.ConfigureWithOidc(
      //    context: context,
      //    authority: configuration["AuthServer:Authority"]!,
      //    scopes: new[] { "WorkflowService" },
      //    discoveryEndpoint: configuration["AuthServer:MetadataAddress"],
      //    apiTitle: "Workflow Service API"
      //);

      // Configure CORS

      context.Services.AddScoped<IWorkflowAppService, WorkflowAppService>();
      context.Services.AddElsa(elsa =>
      {
        // Configure Management layer to use EF Core.
        elsa.UseWorkflowManagement(management => management.UseEntityFrameworkCore());

        // Configure Runtime layer to use EF Core.
        elsa.UseWorkflowRuntime(runtime => runtime.UseEntityFrameworkCore());

        // Default Identity features for authentication/authorization.
        //elsa.UseIdentity(identity =>
        //{
        //  identity.TokenOptions = options => options.SigningKey = "smsmsmsmsmsmskakakaka1k"; // This key needs to be at least 256 bits long.
        //  identity.UseAdminUserProvider();
        //});

        // Configure ASP.NET authentication/authorization.
        //elsa.UseDefaultAuthentication(auth => auth.UseAdminApiKey());

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
        elsa.AddActivitiesFrom<Program>();

        // Register custom workflows from the application, if any.
        elsa.AddWorkflowsFrom<Program>();
      });
      context.Services.AddSwaggerDocument();
      context.Services.AddSwaggerGen(c =>
      {
        //c.IgnoreObsoleteProperties();
        //c.CustomSchemaIds(type => type.FullName);
        //c.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());
        //c.MapType(typeof(IFormFile), () => new OpenApiSchema() { Type = "file", Format = "binary" });
        //c.OperationFilter<FileUploadOperationFilter>();
        c.CustomSchemaIds(type => type.ToString());

        //c.SwaggerDoc("v1", new OpenApiInfo { Title = "Elsa API", Version = "v1" });
      });
      //context.Services.AddElsa(elsa =>
      //{
      //  // Configure Management layer to use EF Core.
      //  elsa.UseWorkflowManagement(management => management.UseEntityFrameworkCore());

      //  // Configure Runtime layer to use EF Core.
      //  elsa.UseWorkflowRuntime(runtime => runtime.UseEntityFrameworkCore());

      //  // Default Identity features for authentication/authorization.
      //  elsa.UseIdentity(identity =>
      //  {
      //    identity.TokenOptions = options => options.SigningKey = "sufficiently-large-secret-signing-key"; // This key needs to be at least 256 bits long.
      //    identity.UseAdminUserProvider();
      //  });

      //  // Configure ASP.NET authentication/authorization.
      //  elsa.UseDefaultAuthentication(auth => auth.UseAdminApiKey());

      //  // Expose Elsa API endpoints.
      //  elsa.UseWorkflowsApi();

      //  // Setup a SignalR hub for real-time updates from the server.
      //  elsa.UseRealTimeWorkflows();

      //  // Enable C# workflow expressions
      //  elsa.UseCSharp();

      //  // Enable HTTP activities.
      //  elsa.UseHttp();

      //  // Use timer activities.
      //  elsa.UseScheduling();

      //  // Register custom activities from the application, if any.
      //  elsa.AddActivitiesFrom<WorkflowServiceApplicationModule>();

      //  // Register custom workflows from the application, if any.
      //  elsa.AddWorkflowsFrom<WorkflowServiceApplicationModule>();
      //});
      context.Services.AddCors(cors => cors
     .AddDefaultPolicy(policy => policy
         .AllowAnyOrigin() // For demo purposes only. Use a specific origin instead.
         .AllowAnyHeader()
         .AllowAnyMethod()
         .WithExposedHeaders("x-elsa-workflow-instance-id"))); // Required for Elsa Studio in order to support running workflows from the designer. Alternatively, you can use the `*` wildcard to expose all headers.

      // Add Health Checks.
      context.Services.AddHealthChecks();// Required for Elsa Studio in order to support running workflows from the designer. Alternatively, you can use the `*` wildcard to expose all headers.
      //context.Services.AddApiVersioning(config =>
      //{
      //  // Specify the default API Version as 1.0
      //  config.DefaultApiVersion = new ApiVersion(1, 0);
      //  // Advertise the API versions supported for the particular endpoint (through 'api-supported-versions' response header which lists all available API versions for that endpoint)
      //  config.ReportApiVersions = true;
      //});

      //  context.Services.AddVersionedApiExplorer(setup =>
      //  {
      //    setup.GroupNameFormat = "'v'VV";
      //    setup.SubstituteApiVersionInUrl = true;
      //  });
      // context.Services
      //.AddElsaCore(elsa => elsa
      //    .UseEntityFrameworkPersistence(options => options.UseNpgsql(configuration.GetConnectionString("WorkflowService")))
      //    .AddHttpActivities()
      //    .AddConsoleActivities());
      // Configure conventional controllers
      Configure<AbpAspNetCoreMvcOptions>(options =>
      {
        options.ConventionalControllers.Create(typeof(WorkflowServiceApplicationModule).Assembly, opts =>
        {
          opts.RootPath = "workflow";
          opts.RemoteServiceName = "Workflow";
        });
      });
    }

    public override void OnApplicationInitialization(ApplicationInitializationContext context)
    {
      var app = context.GetApplicationBuilder();
      var env = context.GetEnvironment();

      if (env.IsDevelopment())
      {
        app.UseDeveloperExceptionPage();
      }

      // Middleware setup
      //app.UseCorrelationId();
      app.UseCors();
      //app.UseAbpRequestLocalization();
      //app.MapAbpStaticAssets();
      app.UseRouting();
      app.UseAuthentication();
      app.UseAuthorization();
      app.UseSwagger();
      app.UseAbpSwaggerUI(

        c => { c.SwaggerEndpoint("/swagger/v1/swagger.json", "Workflow Service API");
         
        });
      //app.UseAbpSwaggerWithCustomScriptUI(options =>
      //{
      //  var configuration = context.ServiceProvider.GetRequiredService<IConfiguration>();

      //  options.SwaggerEndpoint("/swagger/v1/swagger.json", "Workflow Service API");
      //  //options.OAuthClientId(configuration["AuthServer:SwaggerClientId"]);
      //  //options.ValidatorUrl(null);
      //});
      app.UseAbpSerilogEnrichers();
      //app.UseAuditing();
      //app.UseUnitOfWork();
      //app.UseConfiguredEndpoints();
      app.UseWorkflowsApi(); // Use Elsa API endpoints.
      app.UseWorkflows(); // Use Elsa middleware to handle HTTP requests mapped to HTTP Endpoint activities.
      app.UseWorkflowsSignalRHubs(); // Optional SignalR integration. Elsa Studio uses SignalR to receive real-time updates from the server. 
      //app.Run();
    }

    public override async Task OnPostApplicationInitializationAsync(ApplicationInitializationContext context)
    {
      // Apply database migrations
      await context.ServiceProvider
          .GetRequiredService<WorkflowServiceDatabaseMigrationChecker>()
          .CheckAndApplyDatabaseMigrationsAsync();
    }
  }
}
public class FileUploadOperationFilter : IOperationFilter
{
  public void Apply(OpenApiOperation operation, OperationFilterContext context)
  {
    // Ensure the context and operation are valid
    if (operation == null || context == null) return;

    // Loop through parameters and check for IFormFile type
    var fileParameters = context.ApiDescription.ParameterDescriptions
        .Where(p => p.ParameterDescriptor?.ParameterType == typeof(IFormFile))
        .ToList();

    foreach (var parameter in fileParameters)
    {
      var fileParam = operation.Parameters
          .FirstOrDefault(p => p.Name == parameter.Name);

      // If the parameter is found, adjust its schema to indicate binary file upload
      if (fileParam != null)
      {
        fileParam.Schema.Type = "string";
        fileParam.Schema.Format = "binary";
      }
    }
  }
}
