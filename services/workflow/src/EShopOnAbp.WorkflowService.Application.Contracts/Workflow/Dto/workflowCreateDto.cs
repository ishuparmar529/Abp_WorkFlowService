using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EShopOnAbp.WorkflowService.Application.Contracts.Workflow.Dto
{
  public class workflowCreateDto
  {
    public string WorkflowName { get; set; }
    public string WorkflowType { get; set; }
  }
}
