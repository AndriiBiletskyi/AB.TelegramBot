using Telegram.Bot;
using Telegram.Bot.Types.Enums;

namespace AB.TelegramBot.Web.MainApp.Services
{
    public class SetupWebhook : IHostedService
    {
        private readonly ILogger<SetupWebhook>? _logger;
        private readonly IServiceProvider _serviceProvider;
        private readonly IConfiguration _configuration;

        public SetupWebhook(IServiceProvider serviceProvider,
                            IConfiguration configuration,
                            ILoggerFactory? logger = null)
        {
            _logger = logger?.CreateLogger<SetupWebhook>();
            _serviceProvider = serviceProvider;
            _configuration = configuration;
            _logger?.LogInformation("Create an instance of SetupWebhook.");
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            using var scope = _serviceProvider.CreateScope();
            var botClient = scope.ServiceProvider.GetRequiredService<ITelegramBotClient>();

            var webhookAddress = @$"{_configuration["Url"]}/bot/ABBOT";

            _logger?.LogInformation($"Setting webhook: [{webhookAddress}]");

            await botClient.SetWebhookAsync(
                url: webhookAddress,
                allowedUpdates: Array.Empty<UpdateType>(),
                cancellationToken: cancellationToken);
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            using var scope = _serviceProvider.CreateScope();
            var botClient = scope.ServiceProvider.GetRequiredService<ITelegramBotClient>();

            _logger?.LogInformation("Removing webhook");
            await botClient.DeleteWebhookAsync(cancellationToken: cancellationToken);
        }
    }
}
