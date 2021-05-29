namespace EpayPaymentServiceDotNet.Services.Web
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using EpayPaymentServiceDotNet.Common.Enumerations;
    using EpayPaymentServiceDotNet.Contracts.Services.Web;
    using EpayPaymentServiceDotNet.Services.Web.Models;

    /// <summary>
    /// Init web service.
    /// </summary>
    public class InitWebService : IInitWebService
    {
        public Task<PayInitResponseModel> CheckForChargesAsync(string idn, string merchantId)
        {
            if (string.IsNullOrEmpty(idn))
            {
                throw new ArgumentNullException(nameof(idn));
            }

            if (string.IsNullOrEmpty(merchantId))
            {
                throw new ArgumentNullException(nameof(merchantId));
            }

            return this.CheckForChargesInternalAsync(idn: idn, merchantId: merchantId);
        }

        public Task<PayInitResponseModel> OpenPaymentTransactionAsync(string idn, string merchantId, string tid)
        {
            if (string.IsNullOrEmpty(idn))
            {
                throw new ArgumentNullException(nameof(idn));
            }

            if (string.IsNullOrEmpty(merchantId))
            {
                throw new ArgumentNullException(nameof(merchantId));
            }

            if (string.IsNullOrEmpty(tid))
            {
                throw new ArgumentNullException(nameof(tid));
            }

            return this.OpenPaymentTransactionInternalAsync(idn: idn, merchantId: merchantId, tid: tid);
        }

        public Task<PayInitResponseModel> OpenDepositTransactionAsync(string idn, string merchantId, string tid, int total)
        {
            if (string.IsNullOrEmpty(idn))
            {
                throw new ArgumentNullException(nameof(idn));
            }

            if (string.IsNullOrEmpty(merchantId))
            {
                throw new ArgumentNullException(nameof(merchantId));
            }

            if (string.IsNullOrEmpty(tid))
            {
                throw new ArgumentNullException(nameof(tid));
            }

            if (total <= 0)
            {
                throw new ArgumentException("The total amount of the deposit must be positive", nameof(total));
            }

            return this.OpenDepositTransactionInternalAsync(idn: idn, merchantId: merchantId, tid: tid, total: total);
        }

        private async Task<PayInitResponseModel> CheckForChargesInternalAsync(string idn, string merchantId)
        {
            // TODO: throw new NotImplementedException();
            return GetDummyPayInitResponseModel(idn);
        }

        private async Task<PayInitResponseModel> OpenPaymentTransactionInternalAsync(string idn, string merchantId, string tid)
        {
            // TODO: throw new NotImplementedException();
            return GetDummyPayInitResponseModel(idn);
        }

        private async Task<PayInitResponseModel> OpenDepositTransactionInternalAsync(string idn, string merchantId, string tid, int total)
        {
            // TODO: throw new NotImplementedException();
            return GetDummyPayInitResponseModel(idn);
        }

        private static PayInitResponseModel GetDummyPayInitResponseModel(string idn)
        {
            return new PayInitResponseModel
            {
                Idn = idn,
                Amount = 1000,
                ShortDescription = "Customer Name // Customer Address",
                LongDescription = "Some details for the charge\nand payments\nfor the customer",
                ValidTo = DateTime.UtcNow.AddMonths(1).ToString("yyyyMMdd"),
                Status = PaymentResponseStatus.Ok.ToStatusString(),
                Invoices = new List<PayInitInvoiceResponseModel>
                {
                    new PayInitInvoiceResponseModel
                    {
                        Idn = $"{idn}.INV0001",
                        Amount = 300,
                        ValidTo=DateTime.UtcNow.AddMonths(1).ToString("yyyyMMdd"),
                        ShortDescription = "Short description INV0001",
                        LongDescription = "Long description INV0001",
                    },
                    new PayInitInvoiceResponseModel
                    {
                        Idn = $"{idn}.INV0002",
                        Amount = 200,
                        ValidTo=DateTime.UtcNow.AddMonths(1).ToString("yyyyMMdd"),
                        ShortDescription = "Short description INV0002",
                        LongDescription = "Long description INV0002",
                    },
                    new PayInitInvoiceResponseModel
                    {
                        Idn = $"{idn}.INV0003",
                        Amount = 500,
                        ValidTo=DateTime.UtcNow.AddMonths(1).ToString("yyyyMMdd"),
                        ShortDescription = "Short description INV0003",
                        LongDescription = "Long description INV0003",
                    },
                },
            };
        }
    }
}
