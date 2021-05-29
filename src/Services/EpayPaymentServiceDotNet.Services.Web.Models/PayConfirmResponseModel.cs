namespace EpayPaymentServiceDotNet.Services.Web.Models
{
    using System.Text.Json.Serialization;

    /// <summary>
    /// Pay/Confirm response model.
    /// </summary>
    public class PayConfirmResponseModel
    {
        /// <summary>
        /// Gets or sets the status.
        /// </summary>
        [JsonPropertyName("STATUS")]
        public string Status { get; set; } = string.Empty;
    }
}
