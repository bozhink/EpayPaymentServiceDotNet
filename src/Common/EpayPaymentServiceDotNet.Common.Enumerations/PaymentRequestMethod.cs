namespace EpayPaymentServiceDotNet.Common.Enumerations
{
    /// <summary>
    /// Payment method in requests if the payments service.
    /// </summary>
    public enum PaymentRequestMethod : int
    {
        /// <summary>
        /// Undefined.
        /// Default value.
        /// </summary>
        Undefined = 0,

        /// <summary>
        /// Only check for charges. Do not open payment transaction.
        /// </summary>
        Check = 1,

        /// <summary>
        /// Check for charges and open payment transaction or settle opened transaction for payment.
        /// </summary>
        Billing = 2,

        /// <summary>
        /// Settle opened transaction with partial payment.
        /// </summary>
        Partial = 3,

        /// <summary>
        /// Open transaction for deposit or settle opened transaction for deposit.
        /// </summary>
        Deposit = 4,
    }
}
