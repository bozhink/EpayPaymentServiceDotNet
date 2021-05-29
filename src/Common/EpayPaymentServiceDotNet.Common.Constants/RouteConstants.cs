namespace EpayPaymentServiceDotNet.Common.Constants
{
    /// <summary>
    /// Route constants.
    /// </summary>
    public static class RouteConstants
    {
        /// <summary>
        /// Payments service API route.
        /// </summary>
        public const string PaymentsServiceApiRoute = "api/" + VersionConstants.PaymentsServiceApiVersion + "/epay";

        /// <summary>
        /// Payments service API init route.
        /// </summary>
        public const string PaymentsServiceApiInitRoute = PaymentsServiceApiRoute + "/init";

        /// <summary>
        /// Payments service API confirm route.
        /// </summary>
        public const string PaymentsServiceApiConfirmRoute = PaymentsServiceApiRoute + "/confirm";
    }
}
