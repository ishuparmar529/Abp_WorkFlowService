using Elsa.Activities.Console;
using Elsa.Activities.Http;
using Elsa.Builders;
using Elsa.Services;
using System.Net;

namespace EShopOnAbp.WorkflowService.Application
{
  public class WorkflowProcessing : IWorkflow
  {
    public void Build(IWorkflowBuilder builder)
    {
      builder.StartWith<WriteLine>(step => step.WithText("Starting Order Processing Workflow..."))

             .Then<WriteHttpResponse>(step =>
             {
               step.WithContent("Workflow Completed!");
               step.WithStatusCode(HttpStatusCode.OK);
             });
    }
  }
}
