using System.Data;
using System.Data.SqlClient;
using Microsoft.Extensions.DependencyInjection;
using SovComBankTest.Repositories.Abstractions;

namespace SovComBankTest.Repositories.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddSmsRepositories(this IServiceCollection services, string connectionString) => services
            .AddScoped<IDbConnection, SqlConnection>(_ => new SqlConnection(connectionString))
            .AddScoped<IInviteMessagesRepository, InviteMessagesRepository>();
    }
}
