namespace EpayPaymentServiceDotNet.Web.PaymentsServiceApi.Controllers
{
    using EpayPaymentServiceDotNet.Common.Constants;
    using Microsoft.AspNetCore.Mvc;

    [Route(RouteConstants.PaymentsServiceApiConfirmRoute)]
    [ApiController]
    public class ConfirmController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            return this.Ok();
        }
    }
}
