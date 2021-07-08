using System;
using Microsoft.Extensions.DependencyInjection;
using SovComBankTest.Repositories.Abstractions;
using SovComBankTest.Repositories.DependencyInjection;
using SovComBankTest.Services.Abstractions;

namespace SovComBankTest.Services.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddSmsService(this IServiceCollection services, Action<InviteMessagesRepositoryOptions> options) => services
            .AddSmsRepositories(options)
            .AddMemoryCache()
            .AddScoped<ISmsService, SmsService>();
    }
}
