namespace EpayPaymentServiceDotNet.Common.Enumerations
{
    /// <summary>
    /// <see cref="PaymentResponseStatus"/> extensions.
    /// </summary>
    public static class PaymentResponseStatusExtensions
    {
        public static string ToStatusString(this PaymentResponseStatus status)
        {
            return ((int)status).ToString("00");
        }
    }
}
