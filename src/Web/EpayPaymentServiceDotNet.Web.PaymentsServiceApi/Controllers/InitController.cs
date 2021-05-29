namespace EpayPaymentServiceDotNet.Web.PaymentsServiceApi.Controllers
{
    using EpayPaymentServiceDotNet.Common.Constants;
    using Microsoft.AspNetCore.Mvc;

    [Route(RouteConstants.PaymentsServiceApiInitRoute)]
    [ApiController]
    public class InitController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            return this.Ok();
        }
    }
}
