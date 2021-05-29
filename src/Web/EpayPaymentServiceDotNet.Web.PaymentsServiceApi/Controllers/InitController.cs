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

    [Route(RouteConstants.PaymentsServiceApiInitRoute)]
    [ApiController]
    public class InitController : ControllerBase
    {
        private readonly IInitWebService service;
        private readonly IValidationWebService validationService;
        private readonly ILogger logger;

        public InitController(IInitWebService service, IValidationWebService validationService, ILogger<InitController> logger)
        {
            this.service = service ?? throw new ArgumentNullException(nameof(service));
            this.validationService = validationService ?? throw new ArgumentNullException(nameof(validationService));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PayInitResponseModel))]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetAsync(string idn, string merchantId, string checksum, string type, string? tid = null, int? total = null)
        {
            try
            {
                var method = this.ValidateRequestConsistency(idn, merchantId, checksum, type, tid);

                if (this.ModelState.ErrorCount > 0)
                {
                    this.logger.LogError((EventId)(int)HttpStatusCode.BadRequest, this.ModelState.ToString());
                    return this.BadRequest(this.ModelState);
                }

                if (method == PaymentRequestMethod.Deposit && !(total > 0))
                {
                    this.logger.LogWarning((EventId)(int)PaymentResponseStatus.InvalidDepositAmount, PaymentResponseStatus.InvalidDepositAmount.ToString());
                    return this.Ok(new PayInitResponseModel
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

                PayInitResponseModel responseModel;
                switch (method)
                {
                    case PaymentRequestMethod.Check:
                        responseModel = await this.service.CheckForChargesAsync(idn: idn, merchantId: merchantId).ConfigureAwait(false);
                        break;

                    case PaymentRequestMethod.Billing:
                        responseModel = await this.service.OpenPaymentTransactionAsync(idn: idn, merchantId: merchantId, tid: tid).ConfigureAwait(false);
                        break;

                    case PaymentRequestMethod.Deposit:
                        responseModel = await this.service.OpenDepositTransactionAsync(idn: idn, merchantId: merchantId, tid: tid, total: total ?? 0).ConfigureAwait(false);
                        break;

                    default:
                        responseModel = new PayInitResponseModel
                        {
                            Status = PaymentResponseStatus.Error.ToStatusString(),
                        };
                        break;
                }

                return this.Ok(responseModel);
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
