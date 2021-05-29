namespace EpayPaymentServiceDotNet.Contracts.Services.Web
{
    using System.Threading.Tasks;
    using EpayPaymentServiceDotNet.Services.Web.Models;

    /// <summary>
    /// Init web service.
    /// </summary>
    public interface IInitWebService
    {
        /// <summary>
        /// Check for charges.
        /// </summary>
        /// <param name="idn">IDN of the customer.</param>
        /// <param name="merchantId">ID of the merchant.</param>
        /// <returns>Information for the found charge.</returns>
        Task<PayInitResponseModel> CheckForChargesAsync(string idn, string merchantId);

        /// <summary>
        /// Open transaction for payment.
        /// </summary>
        /// <param name="idn">IDN of the customer.</param>
        /// <param name="merchantId">ID of the merchant.</param>
        /// <param name="tid">TID of the transaction to be opened.</param>
        /// <returns>Information for the found charge.</returns>
        Task<PayInitResponseModel> OpenPaymentTransactionAsync(string idn, string merchantId, string tid);

        /// <summary>
        /// Open transaction for deposit.
        /// </summary>
        /// <param name="idn">IDN of the customer.</param>
        /// <param name="merchantId">ID of the merchant.</param>
        /// <param name="tid">TID of the transaction to be opened.</param>
        /// <param name="total">Amount of the deposit.</param>
        /// <returns>Information for the created deposit.</returns>
        Task<PayInitResponseModel> OpenDepositTransactionAsync(string idn, string merchantId, string tid, int total);
    }
}
