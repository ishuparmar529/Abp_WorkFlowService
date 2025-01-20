using EShopOnAbp.WorkflowService.Application.Contracts.Workflow.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace EShopOnAbp.WorkflowService.Application.WorkflowService
{
  public class WorkflowAppService : ApplicationService, IWorkflowAppService
  {
    public Task<WorkflowDto> CreateAsync(workflowCreateDto input)
    {
      throw new NotImplementedException();
    }

    public Task<bool> DeleteAsync(Guid id)
    {
      throw new NotImplementedException();
    }

    public Task<WorkflowDto> GetAsync(Guid id)
    {
      throw new NotImplementedException();
    }

    public Task<PagedResultDto<WorkflowDto>> GetListPagedAsync(PagedAndSortedResultRequestDto input)
    {
      throw new NotImplementedException();
    }
  }
}
