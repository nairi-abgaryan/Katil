namespace Katil.Common.Utilities
{
    public static class Constants
    {
        public const int AccessCodeLenght = 7;
        public const int FileUploadType = 1;
        public const int ScheduledHearingDuration = 8;
    }

    public static class SettingKeys
    {
        public const string SessionTimeout = "SessionTimeout";

        public const string FileStorageRoot = "FileStorageRoot";
        public const string CommonFileStorageRoot = "CommonFileStorageRoot";
        public const string CommonFileRepositoryBaseUrl = "CommonFileRepositoryBaseUrl";
        public const string FileRepositoryBaseUrl = "FileRepositoryBaseUrl";

        public const string MerchantId = "MerchantId";
        public const string HashKey = "HashKey";
        public const string ReturnUrl = "ReturnUrl";
        public const string PaymentURI = "PaymentURI";
        public const string PaymentReportURI = "PaymentReportURI";
        public const string PaymentConfirmationNumberOfRetries = "PaymentConfirmationNumberOfRetries";
        public const string PaymentConfirmationDelayBetweenRetries = "PaymentConfirmationDelayBetweenRetries";

        public const string SmtpClientTimeout = "SmtpClientTimeout";
        public const string SmtpClientPort = "SmtpClientPort";
        public const string SmtpClientHost = "SmtpClientHost";
        public const string SmtpClientFromEmail = "SmtpClientFromEmail";
        public const string SmtpClientUsername = "SmtpClientUsername";
        public const string SmtpClientPassword = "SmtpClientPassword";
        public const string SmtpClientEnableSsl = "SmtpClientEnableSsl";
        public const string SmtpClientNumberOfRetries = "SmtpClientNumberOfRetries";
        public const string SmtpClientDelayBetweenRetries = "SmtpClientDelayBetweenRetries";

        public const string PdfGenerationServiceUri = "PdfGenerationServiceUri";
        public const string PdfPageHeaderHtmlKey = "PdfPageHeaderHtmlKey";
        public const string PdfPageFooterHtmlKey = "PdfPageFooterHtmlKey";
    }
}
