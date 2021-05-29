namespace EpayPaymentServiceDotNet.Services.Web
{
    using System;
    using System.Security.Cryptography;
    using System.Text;
    using System.Threading.Tasks;
    using EpayPaymentServiceDotNet.Contracts.Services.Web;

    /// <summary>
    /// Validation web service.
    /// </summary>
    public class ValidationWebService : IValidationWebService
    {
        /// <inheritdoc/>
        public Task<bool> ValidateChecksumAsync(string merchantId, string checksum, string request)
        {
            if (string.IsNullOrEmpty(merchantId) || string.IsNullOrEmpty(checksum) || string.IsNullOrEmpty(request))
            {
                return Task.FromResult<bool>(false);
            }

            return this.ValidateChecksumInternalAsync(merchantId: merchantId, checksum: checksum, request: request);
        }

        /// <inheritdoc/>
        public Task<bool> ValidateIdnAsync(string idn)
        {
            if (string.IsNullOrEmpty(idn))
            {
                return Task.FromResult<bool>(false);
            }

            // TODO: here goes some internal vendor logic for validation of the IDN.
            return Task.FromResult<bool>(true);
        }

        private async Task<bool> ValidateChecksumInternalAsync(string merchantId, string checksum, string request)
        {
            string[] requestParameters = request.TrimStart(new char[] { '?' }).Split(new char[] { '&' });

            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < requestParameters.Length; i++)
            {
                string parameter = requestParameters[i].Replace("=", string.Empty);
                if (parameter.IndexOf("CHECKSUM", StringComparison.InvariantCultureIgnoreCase) < 0)
                {
                    // Skip the checksum parameter from the request.
                    sb.Append(parameter);
                    sb.Append('\n');
                }
            }

            string payload = sb.ToString();

            // TODO: get the secret by the merchantId; this should be async method.
            string secret = await Task.FromResult<string>(string.Empty).ConfigureAwait(false);

            string evaluatedChecksum = this.GetHmacSha1(payload: payload, secret: secret);

            // TODO: enable the validation.
            ////return string.Compare(checksum, evaluatedChecksum, ignoreCase: true) == 0;
            return true;
        }

        private string GetHmacSha1(string payload, string secret)
        {
            if (string.IsNullOrWhiteSpace(payload))
            {
                throw new ArgumentNullException(nameof(payload));
            }

            if (string.IsNullOrWhiteSpace(secret))
            {
                throw new ArgumentNullException(nameof(secret));
            }

            using (KeyedHashAlgorithm algorithm = new HMACSHA1(Encoding.UTF8.GetBytes(secret)))
            {
                var bytes = Encoding.UTF8.GetBytes(payload);
                var hash = algorithm.ComputeHash(bytes);
                return BitConverter.ToString(hash).Replace("-", string.Empty);
            }
        }
    }
}
