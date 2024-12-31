using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EShopOnAbp.WorkflowService.EntityFrameworkCore.Entity
{
  public class WorkflowEngine
  {
    public int ID { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
  }
}
