using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Entities.Auditing;

namespace EShopOnAbp.WorkflowService.EntityFrameworkCore.Entity
{
  public class WorkflowEngine: FullAuditedEntity<int>
  {
    public string Name { get; set; }
    public string Description { get; set; }
    public string WorkflowType { get; set; }
  }
}
