namespace EpayPaymentServiceDotNet.Services.Web.Models
{
    using System.Text.Json.Serialization;

    /// <summary>
    /// Pay/Init invoice response model.
    /// </summary>
    public class PayInitInvoiceResponseModel
    {
        /// <summary>
        /// Gets or sets the IDN of the invoice. It is the 'IDN of the customer' + '.' + 'the invoice number'.
        /// </summary>
        [JsonPropertyName("IDN")]
        public string Idn { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the amount.
        /// </summary>
        [JsonPropertyName("AMOUNT")]
        public int? Amount { get; set; }

        /// <summary>
        /// Gets or sets the validity date of the charge.
        /// </summary>
        [JsonPropertyName("VALIDTO")]
        public string ValidTo { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the short description.
        /// </summary>
        [JsonPropertyName("SHORTDESC")]
        public string ShortDescription { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the long description.
        /// </summary>
        [JsonPropertyName("LONGDESC")]
        public string LongDescription { get; set; } = string.Empty;
    }
}
