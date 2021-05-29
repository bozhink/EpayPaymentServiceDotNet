namespace EpayPaymentServiceDotNet.Web.PaymentsServiceApi.Controllers
{
    using System;
    using System.Net;
    using System.Threading.Tasks;
    using EpayPaymentServiceDotNet.Common.Constants;
    using EpayPaymentServiceDotNet.Common.Enumerations;
    using EpayPaymentServiceDotNet.Contracts.Services.Web;
    using EpayPaymentServiceDotNet.Services.Web.Models;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;

    [Route(RouteConstants.PaymentsServiceApiConfirmRoute)]
    [ApiController]
    public class ConfirmController : ControllerBase
    {
        private readonly IValidationWebService validationService;
        private readonly ILogger logger;

        public ConfirmController(IValidationWebService validationService, ILogger<ConfirmController> logger)
        {
            this.validationService = validationService ?? throw new ArgumentNullException(nameof(validationService));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PayConfirmResponseModel))]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetAsync(string idn, string merchantId, string tid, string date, int total, string type, string checksum, string? invoices = null)
        {
            try
            {
                var method = this.ValidateRequestConsistency(idn, merchantId, tid, date, type, checksum);

                if (this.ModelState.ErrorCount > 0)
                {
                    this.logger.LogError((EventId)(int)HttpStatusCode.BadRequest, this.ModelState.ToString());
                    return this.BadRequest(this.ModelState);
                }

                if (method == PaymentRequestMethod.Deposit && !(total > 0))
                {
                    this.logger.LogWarning((EventId)(int)PaymentResponseStatus.InvalidDepositAmount, PaymentResponseStatus.InvalidDepositAmount.ToString());
                    return this.Ok(new PayConfirmResponseModel
                    {
                        Status = PaymentResponseStatus.InvalidDepositAmount.ToStatusString(),
                    });
                }

                bool isValidChecksum = await this.validationService.ValidateChecksumAsync(merchantId: merchantId, checksum: checksum, request: this.Request.QueryString.ToString()).ConfigureAwait(false);

                if (!isValidChecksum)
                {
                    this.logger.LogWarning((EventId)(int)PaymentResponseStatus.InvalidChecksum, PaymentResponseStatus.InvalidChecksum.ToString());
                    return this.Ok(new PayInitResponseModel
                    {
                        Status = PaymentResponseStatus.InvalidChecksum.ToStatusString(),
                    });
                }

                bool isValidIdn = await this.validationService.ValidateIdnAsync(idn: idn).ConfigureAwait(false);

                if (!isValidIdn)
                {
                    this.logger.LogWarning((EventId)(int)PaymentResponseStatus.InvalidIdn, PaymentResponseStatus.InvalidIdn.ToString());
                    return this.Ok(new PayInitResponseModel
                    {
                        Status = PaymentResponseStatus.InvalidIdn.ToStatusString(),
                    });
                }

                return this.Ok();
            }
            catch (Exception ex)
            {
                this.logger.LogError((EventId)(int)PaymentResponseStatus.Error, ex, PaymentResponseStatus.Error.ToString());
                return this.Ok(new PayInitResponseModel
                {
                    Status = PaymentResponseStatus.Error.ToStatusString(),
                });
            }
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
