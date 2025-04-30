using Newtonsoft.Json;
using Telegram.Bot.Types;

namespace AB.TelegramBot.Web.MainApp.Services
{
    public class UpdateParseService
    {
        public async Task<Update?> Parse(HttpRequest request)
        {
            using var streamReader = new StreamReader(request.Body);
            var updateJsonString = await streamReader.ReadToEndAsync();
            if (updateJsonString is not null)
            {
                return JsonConvert.DeserializeObject<Update>(updateJsonString);
            }
            return null;
        }
    }
}
