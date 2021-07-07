using Microsoft.Extensions.DependencyInjection;
using SovComBankTest.Services.Abstractions;

namespace SovComBankTest.Services.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddSmsService(this IServiceCollection services) => services
            .AddScoped<ISmsService, SmsService>();
    }
}
