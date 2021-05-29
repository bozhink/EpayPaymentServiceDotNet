namespace EpayPaymentServiceDotNet.Web.PaymentsServiceApi.Controllers
{
    using System;
    using EpayPaymentServiceDotNet.Common.Constants;
    using EpayPaymentServiceDotNet.Common.Enumerations;
    using EpayPaymentServiceDotNet.Services.Web.Models;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;

    [Route(RouteConstants.PaymentsServiceApiConfirmRoute)]
    [ApiController]
    public class ConfirmController : ControllerBase
    {
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PayConfirmResponseModel))]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult Get(string idn, string merchantId, string tid, string date, int total, string type, string checksum, string? invoices = null)
        {
            var method = this.ValidateRequestConsistency(idn, merchantId, tid, date, type, checksum);

            if (this.ModelState.ErrorCount > 0)
            {
                return this.BadRequest(this.ModelState);
            }

            if (method == PaymentRequestMethod.Deposit && !(total > 0))
            {
                return this.Ok(new PayConfirmResponseModel
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
        /// <param name="tid">TID to settle payment transaction.</param>
        /// <param name="date">Date of the payment.</param>
        /// <param name="type">Type of the confirm action. Valid values are PARTIAL, BILLING, and DEPOSIT.</param>
        /// <param name="checksum">Checksum for request verification.</param>
        /// <returns>Processed type.</returns>
        private PaymentRequestMethod ValidateRequestConsistency(string idn, string merchantId, string tid, string date, string type, string checksum)
        {
            if (string.IsNullOrEmpty(idn))
            {
                this.ModelState.AddModelError("IDN", "Missing required parameter.");
            }

            if (string.IsNullOrEmpty(merchantId))
            {
                this.ModelState.AddModelError("MERCHANTID", "Missing required parameter.");
            }

            if (string.IsNullOrEmpty(tid))
            {
                this.ModelState.AddModelError("TID", "Missing required parameter.");
            }

            if (string.IsNullOrEmpty(date))
            {
                this.ModelState.AddModelError("DATE", "Missing required parameter.");
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
                    case PaymentRequestMethod.Billing:
                    case PaymentRequestMethod.Deposit:
                    case PaymentRequestMethod.Partial:
                        break;

                    default:
                        // Invalid type for the pay/init method.
                        this.ModelState.AddModelError("TYPE", "Invalid type.");
                        break;
                }
            }

            if (string.IsNullOrEmpty(checksum))
            {
                this.ModelState.AddModelError("CHECKSUM", "Missing required parameter.");
            }

            return paymentRequestMethod;
        }
    }
}
