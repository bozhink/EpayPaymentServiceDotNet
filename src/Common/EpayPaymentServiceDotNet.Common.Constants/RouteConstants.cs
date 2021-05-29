namespace EpayPaymentServiceDotNet.Common.Constants
{
    public static class RouteConstants
    {
        public const string PaymentsServiceApiRoute = "api/" + VersionConstants.PaymentsServiceApiVersion + "/epay/";
        public const string PaymentsServiceApiInitRoute = PaymentsServiceApiRoute + "/init";
        public const string PaymentsServiceApiConfirmRoute = PaymentsServiceApiRoute + "/confirm";
    }
}
