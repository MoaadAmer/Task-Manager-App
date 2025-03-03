namespace Controller_based_APIs.Services
{
    public class LocalMailService : IMailService
    {
        private readonly ILogger<LocalMailService> logger;
        private readonly IConfiguration configuration;
        private readonly string? fromAddress;
        private readonly string? toAddress;

        public LocalMailService(ILogger<LocalMailService> logger, IConfiguration configuration)
        {
            this.logger = logger;
            this.configuration = configuration;
            fromAddress = configuration["MailSettings:FromAddress"];
            toAddress = configuration["MailSettings:ToAddress"];
        }
        public void Send(string subject, string message)
        {
            logger.LogInformation(
                $"""
                Sending email from {fromAddress} to {toAddress}
                Subject: {subject}
                Message: {message}
                """);

        }
    }
}
