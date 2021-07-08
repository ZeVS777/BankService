using Microsoft.Extensions.DependencyInjection;
using SovComBankTest.Repositories.DependencyInjection;
using SovComBankTest.Services.Abstractions;

namespace SovComBankTest.Services.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddSmsService(this IServiceCollection services, string connectionString) => services
            .AddSmsRepositories(connectionString)
            .AddMemoryCache()
            .AddScoped<ISmsService, SmsService>();
    }
}
