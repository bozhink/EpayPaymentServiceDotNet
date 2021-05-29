namespace EpayPaymentServiceDotNet.Common.Enumerations
{
    /// <summary>
    /// Payment method in requests if the payments service.
    /// </summary>
    public enum PaymentRequestMethod
    {
        /// <summary>
        /// Only check for charges. Do not open payment transaction.
        /// </summary>
        Check,

        /// <summary>
        /// Check for charges and open payment transaction or settle opened transaction for payment.
        /// </summary>
        Billing,

        /// <summary>
        /// Settle opened transaction with partial payment.
        /// </summary>
        Partial,

        /// <summary>
        /// Open transaction for deposit or settle opened transaction for deposit.
        /// </summary>
        Deposit,
    }
}
