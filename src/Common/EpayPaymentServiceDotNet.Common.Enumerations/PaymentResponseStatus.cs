namespace EpayPaymentServiceDotNet.Common.Enumerations
{
    /// <summary>
    /// Payment response status.
    /// </summary>
    public enum PaymentResponseStatus : int
    {
        /// <summary>
        /// OK. Request completed successfully (pay_init and pay_confirm).
        /// </summary>
        Ok = 0,

        /// <summary>
        /// Invalid amount for deposit (pay_init).
        /// </summary>
        InvalidDepositAmount = 13,

        /// <summary>
        /// Invalid IDN (pay_init).
        /// </summary>
        InvalidIdn = 14,

        /// <summary>
        /// No charge is found (pay_init).
        /// </summary>
        NoCharge = 62,

        /// <summary>
        /// Temporary unavailable (pay_init).
        /// </summary>
        TemporaryUnavailable = 80,

        /// <summary>
        /// Invalid checksum (pay_init and pay_confirm).
        /// </summary>
        InvalidChecksum = 93,

        /// <summary>
        /// Repetition of payment notification (pay_confirm).
        /// </summary>
        Repetition = 94,

        /// <summary>
        /// General error (pay_init and pay_confirm).
        /// </summary>
        Error = 96,
    }
}
