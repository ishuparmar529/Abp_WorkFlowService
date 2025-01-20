using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace EShopOnAbp.WorkflowService.Application.Contracts.Workflow.Dto
{
  public interface IWorkflowAppService :IApplicationService
  {
    Task<WorkflowDto> CreateAsync(workflowCreateDto input);
    Task<WorkflowDto> GetAsync(Guid id);
    Task<PagedResultDto<WorkflowDto>> GetListPagedAsync(PagedAndSortedResultRequestDto input);
    Task<bool> DeleteAsync(Guid id);
    //Task<DashboardDto> GetDashboardAsync(DashboardInput input);
  }
}
