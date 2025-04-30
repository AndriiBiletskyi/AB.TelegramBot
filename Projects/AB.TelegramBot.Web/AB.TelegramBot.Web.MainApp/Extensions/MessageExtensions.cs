using Telegram.Bot.Types;

namespace AB.TelegramBot.Web.MainApp.Extensions
{
    public static class MessageExtensions
    {
        public static bool IsCorrect(this Message message, out long chatId, out string input)
        {
            input = message?.Text ?? string.Empty;

            chatId = message?.Chat?.Id ?? 0;

            if (chatId == 0 || string.IsNullOrEmpty(input))
            {
                return false;
            }
            return true;
        }
    }
}
