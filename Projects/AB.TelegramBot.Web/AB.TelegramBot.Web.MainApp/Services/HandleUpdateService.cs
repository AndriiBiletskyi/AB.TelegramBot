using AB.TelegramBot.Bank.Models;
using AB.TelegramBot.Resources.Strings;
using AB.TelegramBot.Web.MainApp.Extensions;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace AB.TelegramBot.Web.MainApp.Services
{
    public class HandleUpdateService
    {
        private readonly ITelegramBotClient _botClient;
        private readonly ExchangeServices _exchangeServices;
        private readonly ILogger<HandleUpdateService>? _logger;

        public HandleUpdateService(ITelegramBotClient botClient,
                                   ExchangeServices exchangeServices,
                                   ILoggerFactory? logger = null)
        {
            _botClient = botClient;
            _exchangeServices = exchangeServices;
            _logger = logger?.CreateLogger<HandleUpdateService>();
            _logger?.LogInformation("Create an instance of HandleUpdateService.");
        }

        public async Task EchoAsync(Update update)
        {
            var handler = update.Type switch
            {
                UpdateType.Message => BotOnMessageReceived(update.Message!),
                UpdateType.EditedMessage => BotOnMessageReceived(update.EditedMessage!),
                _ => UnknownUpdateHandlerAsync(update)
            };

            try
            {
                await handler;
            }
            catch (Exception exception)
            {
                await HandleErrorAsync(exception);
            }
        }

        private async Task BotOnMessageReceived(Message message)
        {
            _logger?.LogInformation($"Receive message type: [{message.Type}]");
            if (message.Type != MessageType.Text)
            {
                _logger?.LogWarning($"Received wrong type of message - [{message.Type}].");
                await _botClient.SendTextMessageAsync(message.Chat.Id, StringResources.IDonotWortkWithThis);
                return;
            }

            if (!message.IsCorrect(out long chatId, out string input))
            {
                _logger?.LogWarning($"Received wrong data in message.");
                return;
            }

            if (input.ContainsExchangeQuery(out ExchangeQuery exchangeQuery, out string error))
            {
                exchangeQuery.CurrencyTo = new Currency() { CurrencyCode = "UAH" };

                var rates = await _exchangeServices.GetRates(exchangeQuery);
                await _botClient.SendTextMessageAsync(chatId, string.Join(Environment.NewLine, rates));
                return;
            }

            var answer = string.IsNullOrEmpty(error) ? StringResources.SomethingWentWrongTryLater : error;
            await _botClient.SendTextMessageAsync(chatId, answer);
        }

        private async Task UnknownUpdateHandlerAsync(Update update)
        {
            _logger?.LogInformation($"HandleUpdateService.UnknownUpdateHandlerAsync: Unknown update type: [{update.Type}]");
            await _botClient.SendTextMessageAsync(update.Message.Chat.Id, StringResources.IDonotWortkWithThis);
        }

        public Task HandleErrorAsync(Exception exception)
        {
            var ErrorMessage = exception switch
            {
                ApiRequestException apiRequestException => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
                _ => exception.ToString()
            };

            _logger?.LogInformation($"HandleUpdateService.HandleErrorAsync: HandleError: {ErrorMessage}");
            return Task.CompletedTask;
        }
    }
}
