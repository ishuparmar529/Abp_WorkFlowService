using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EShopOnAbp.WorkflowService.Domain
{
  public static class WorkflowServiceDbProperties
  {
    public static string DbTablePrefix { get; set; } = "";

    public static string DbSchema { get; set; }

    public const string ConnectionStringName = "WorkflowService";
  }
}
