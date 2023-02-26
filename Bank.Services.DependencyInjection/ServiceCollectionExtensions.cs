using Bank.Repositories.Abstractions;

namespace Bank.Services.DependencyInjection;

/// <summary>
/// Расширение методов коллекции сервисов
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Добавить сервис отправки сообщений
    /// </summary>
    /// <param name="services">Список зарегистрированных сервисов</param>
    /// <param name="options">Настройки</param>
    /// <returns>Список зарегистрированных сервисов</returns>
    public static IServiceCollection AddSmsService(
        this IServiceCollection services, Action<InviteMessagesRepositoryOptions> options) => services
        .AddSmsRepositories(options)
        .AddMemoryCache()
        .AddScoped<ISmsService, SmsService>();
}
