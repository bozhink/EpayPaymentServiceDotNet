namespace EpayPaymentServiceDotNet.Web.PaymentsServiceApi.Controllers
{
    using System;
    using EpayPaymentServiceDotNet.Common.Constants;
    using EpayPaymentServiceDotNet.Common.Enumerations;
    using EpayPaymentServiceDotNet.Services.Web.Models;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;

    [Route(RouteConstants.PaymentsServiceApiInitRoute)]
    [ApiController]
    public class InitController : ControllerBase
    {
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PayInitResponseModel))]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult Get(string idn, string merchantId, string checksum, string type, string? tid = null, int? total = null)
        {
            var method = this.ValidateRequestConsistency(idn, merchantId, checksum, type, tid);

            if (this.ModelState.ErrorCount > 0)
            {
                return this.BadRequest(this.ModelState);
            }

            if (method == PaymentRequestMethod.Deposit && !(total > 0))
            {
                return this.Ok(new PayInitResponseModel
                {
                    Status = PaymentResponseStatus.InvalidDepositAmount.ToStatusString(),
                });
            }





            return this.Ok();
        }

        /// <summary>
        /// Validate request consistency.
        /// </summary>
        /// <param name="idn">IDN of the customer.</param>
        /// <param name="merchantId">ID of the merchant.</param>
        /// <param name="checksum">Checksum for request verification.</param>
        /// <param name="type">Type of the init action. Valid values are CHECK, BILLING, and DEPOSIT.</param>
        /// <param name="tid">TID to open payment transaction. Required for BILLING and DEPOSIT, ignored for CHECK.</param>
        /// <returns>Processed type.</returns>
        private PaymentRequestMethod ValidateRequestConsistency(string idn, string merchantId, string checksum, string type, string? tid)
        {
            if (string.IsNullOrEmpty(idn))
            {
                this.ModelState.AddModelError("IDN", "Missing required parameter.");
            }

            if (string.IsNullOrEmpty(merchantId))
            {
                this.ModelState.AddModelError("MERCHANTID", "Missing required parameter.");
            }

            if (string.IsNullOrEmpty(checksum))
            {
                this.ModelState.AddModelError("CHECKSUM", "Missing required parameter.");
            }

            if (string.IsNullOrEmpty(type))
            {
                this.ModelState.AddModelError("TYPE", "Missing required parameter.");
            }

            if (!Enum.TryParse<PaymentRequestMethod>(type, true, out PaymentRequestMethod paymentRequestMethod))
            {
                this.ModelState.AddModelError("TYPE", "Unknown type.");
            }
            else
            {
                switch (paymentRequestMethod)
                {
                    case PaymentRequestMethod.Check:
                        // No further validation is needed here.
                        break;

                    case PaymentRequestMethod.Billing:
                    case PaymentRequestMethod.Deposit:
                        // This is operation for opening of payment transaction and the TID is required.
                        if (string.IsNullOrEmpty(tid))
                        {
                            this.ModelState.AddModelError("TID", "Missing required parameter.");
                        }

                        break;

                    default:
                        // Invalid type for the pay/init method.
                        this.ModelState.AddModelError("TYPE", "Invalid type.");
                        break;
                }
            }

            return paymentRequestMethod;
        }
    }
}
