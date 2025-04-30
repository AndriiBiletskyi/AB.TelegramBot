using AB.TelegramBot.Bank.Abstraction.Interfaces;
using AB.TelegramBot.Banks.NBU;
using AB.TelegramBot.Banks.PrivatBank;
using AB.TelegramBot.CacheServices;
using AB.TelegramBot.DateTimes;
using AB.TelegramBot.Services;
using AB.TelegramBot.Web.Clients;
using AB.TelegramBot.Web.MainApp.Services;
using Serilog;
using Serilog.Formatting;
using Serilog.Templates;
using System.Text;
using Telegram.Bot;

public class Program
{
    private static Microsoft.Extensions.Logging.ILogger? _logger;

    private static readonly string _logFilePath = $"{Environment.GetFolderPath(Environment.SpecialFolder.Desktop)}/log-.txt";

    private static void Main(string[] args)
    {
        Console.OutputEncoding = Encoding.UTF8;

        var builder = WebApplication.CreateBuilder(args);

        ITextFormatter textFormatter = new ExpressionTemplate("[{@t:HH:mm:ss} {@l:u3} {SourceContext}]\n" +
                                                              "{@m}\n" +
                                                              "{@x}");

        Log.Logger = new LoggerConfiguration()
           .MinimumLevel.Information()
           .WriteTo.Console(textFormatter)
           .WriteTo.File(path: _logFilePath, formatter: textFormatter, rollingInterval: RollingInterval.Day)
           .CreateLogger();
        using var loggerFactory = LoggerFactory.Create(builder => builder.AddSerilog(Log.Logger, dispose: true));
        _logger = loggerFactory.CreateLogger<Program>();

        builder.Services.AddSingleton(loggerFactory);
        builder.Services.AddTransient<IDateTimeProvider, DateTimeProvider>();
        builder.Services.AddTransient<IBanksServices, BanksServices>();
        builder.Services.AddTransient<IWebClient, ABRestClient>();
        builder.Services.AddTransient<IBank, PBBank>();
        builder.Services.AddTransient<IBank, NBUBank>();
        builder.Services.AddSingleton<ICacheServices, InMemoryCacheServices>();
        //var redisConnectionString = builder.Configuration["Redis"] ?? string.Empty;
        //builder.Services.AddSingleton<ICacheServices>(new RedisCacheServices(redisConnectionString));

        var botToken = builder.Configuration["Token"] ?? string.Empty;

        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        builder.Services.AddHttpClient("TelegramWebhook")
            .AddTypedClient<ITelegramBotClient>(httpClient => new TelegramBotClient(botToken, httpClient));

        builder.Services.AddScoped<HandleUpdateService>();
        builder.Services.AddScoped<ExchangeServices>();
        builder.Services.AddScoped<UpdateParseService>();

        builder.Services.AddHostedService<SetupWebhook>();

        var app = builder.Build();

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();
        app.UseHttpLogging();

        app.MapGet("/", () => "TelegramBot is running...");
        app.MapPost($"/bot/ABBOT", HandleTelegramUpdate)
        .WithName("TelegramWebhook");

        _logger?.LogInformation("Start the application.");
        app.Run();
    }

    private static async Task<IResult> HandleTelegramUpdate(HandleUpdateService handleUpdateService, HttpRequest request, UpdateParseService parseService)
    {
        try
        {
            _logger?.LogInformation("Received a message from TelegramBot and start processing it.");

            var update = await parseService.Parse(request);
            if (update is not null)
            {
                await handleUpdateService.EchoAsync(update);
                _logger?.LogInformation("Message has been processed successfully.");
                return Results.Ok();
            }
        }
        catch (Exception ex)
        {
            _logger?.LogError($"During work with the message, an unhandled error occurred.\n{ex.Message}.");
        }

        _logger?.LogError($"HttpRequest has wrong data.");
        return Results.Ok();
    }
}