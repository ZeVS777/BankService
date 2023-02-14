using System;
using Microsoft.Extensions.DependencyInjection;
using Bank.Repositories.Abstractions;
using Bank.Repositories.DependencyInjection;
using Bank.Services.Abstractions;

namespace Bank.Services.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddSmsService(this IServiceCollection services, Action<InviteMessagesRepositoryOptions> options) => services
            .AddSmsRepositories(options)
            .AddMemoryCache()
            .AddScoped<ISmsService, SmsService>();
    }
}
