namespace EpayPaymentServiceDotNet.Contracts.Services.Web
{
    using System.Threading.Tasks;

    /// <summary>
    /// Validation web service.
    /// </summary>
    public interface IValidationWebService
    {
        /// <summary>
        /// Validates the checksum.
        /// </summary>
        /// <param name="merchantId">ID of the merchant.</param>
        /// <param name="checksum">String value of the checksum.</param>
        /// <param name="request">The request content, e.g. the query string of GET request.</param>
        /// <returns>True if the checksum is valid.</returns>
        Task<bool> ValidateChecksumAsync(string merchantId, string checksum, string request);

        /// <summary>
        /// Validates the IDN of the customer.
        /// </summary>
        /// <param name="idn">IDN to be validated.</param>
        /// <returns>True if the IDN is valid.</returns>
        Task<bool> ValidateIdnAsync(string idn);
    }
}
