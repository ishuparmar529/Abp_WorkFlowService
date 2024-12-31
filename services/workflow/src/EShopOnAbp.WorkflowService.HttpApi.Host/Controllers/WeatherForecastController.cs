using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc;

namespace EShopOnAbp.OrderingService.HttpApi.Host.Controllers
{
  public class WeatherForecastController : AbpController
  {
    public ActionResult Index()
    {
      return Redirect("~/swagger");
    }
  }
}
