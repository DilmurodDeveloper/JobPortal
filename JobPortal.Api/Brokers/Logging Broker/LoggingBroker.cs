namespace JobPortal.Api.Brokers.Logging_Broker
{
    public class LoggingBroker : ILoggingBroker
    {
        private readonly ILogger<LoggingBroker> _logger;

        public LoggingBroker(ILogger<LoggingBroker> logger)
        {
            _logger = logger;
        }

        public void LogInformation(string message) =>
            _logger.LogInformation(message);

        public void LogWarning(string message) =>
            _logger.LogWarning(message);

        public void LogError(Exception exception) =>
            _logger.LogError(exception, exception.Message);

        public void LogCritical(Exception exception) =>
            _logger.LogCritical(exception, exception.Message);
    }
}
