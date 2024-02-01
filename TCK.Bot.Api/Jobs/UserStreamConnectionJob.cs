using TCK.Bot.Services;

namespace TCK.Bot.Api.Jobs
{
    public class UserStreamConnectionJob : CronJobServiceBase
    {
        private readonly ILogger<UserStreamConnectionJob> _logger;
        private readonly IUserStreamer _userStream;

        public UserStreamConnectionJob(IScheduleConfig<UserStreamConnectionJob> config, ILogger<UserStreamConnectionJob> logger, IUserStreamer userStream)
            : base(config.CronExpression, config.TimeZoneInfo)
        {
            _logger = logger;
            _userStream = userStream;
        }

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            return base.StartAsync(cancellationToken);
        }

        public override Task DoWork(CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Establishing UserStream Connections");

            _userStream.EstablishConnection();

            return Task.CompletedTask;
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            return base.StopAsync(cancellationToken);
        }
    }
}