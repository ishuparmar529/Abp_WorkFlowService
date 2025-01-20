using EShopOnAbp.WorkflowService.Domain.Shared;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc;


namespace EShopOnAbp.WorkflowService;

public class WorkflowServiceController : AbpControllerBase
{
    //protected WorkflowServiceController()
    //{
    //    LocalizationResource = typeof(WorkflowServiceResource);
    //}
  //private readonly IWorkflowRunner _workflowRunner;

  //public WorkflowServiceController(IWorkflowRunner workflowRunner)
  //{
  //  _workflowRunner = workflowRunner;
  //}


  [HttpPost("start-order")]
  public async Task<IActionResult> StartOrderWorkflow()
  {
    return Ok();
  //  var result = await _workflowRunner.RunWorkflowAsync();
  //  return Ok(result.WorkflowInstanceId);
  }
}
