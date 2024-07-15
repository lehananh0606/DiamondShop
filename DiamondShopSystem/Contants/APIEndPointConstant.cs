namespace DiamondShopSystem.Constants
{
    public static class APIEndPointConstant
    {
        private const string RootEndPoint = "/api";
        private const string ApiVersion = "/v1";
        private const string ApiEndpoint = RootEndPoint + ApiVersion;


        public static class Authentication
        {
            public const string AuthenticationEndpoint = ApiEndpoint + "/authentications";
            public const string Login = AuthenticationEndpoint + "/login";
            public const string ReGenerationTokens = AuthenticationEndpoint + "/regeneration-tokens";
            public const string PasswordResetation = AuthenticationEndpoint + "/password-resetation";
        }

        public static class Verification
        {
            public const string VerificationEndpoint = ApiEndpoint + "/verifications";
            public const string EmailVerificationEndpoint = VerificationEndpoint + "/email-verification";
            public const string OTPVerificationEndpoint = VerificationEndpoint + "/otp-verification";
        }

       
        public static class Account
        {
            public const string AccountEndpoint = ApiEndpoint + "/accounts" + "/{id}";
        }

        public static class Customer
        {
            public const string CustomersEndpoint = ApiEndpoint + "/user";
            public const string CustomerEndpoint = CustomersEndpoint + "/{id}";
            public const string CustomerProfileEndpoint = CustomersEndpoint + "/profile";
            public const string UpdatingCustomerStatusEndpoint = CustomersEndpoint + "/updating-status";
            public const string CustomerReportEndpoint = CustomersEndpoint + "/report";
        }

    }
}
