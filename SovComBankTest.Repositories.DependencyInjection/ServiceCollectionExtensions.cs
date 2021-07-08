using System.Data;
using System.Data.SqlClient;
using Dapper;
using Microsoft.Extensions.DependencyInjection;
using SovComBankTest.Repositories.Abstractions;

namespace SovComBankTest.Repositories.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddSmsRepositories(this IServiceCollection services, string connectionString)
        {
            InitialDbMigration(connectionString);

            return services
                .AddScoped<IDbConnection, SqlConnection>(_ => new SqlConnection(connectionString))
                .AddScoped<IInviteMessagesRepository, InviteMessagesRepository>();
        }

        private static void InitialDbMigration(string connectionString)
        {
            using var connection = new SqlConnection(connectionString);

            connection.Execute(@"IF NOT EXISTS(SELECT * FROM sys.tables WHERE name = 'InviteMessage')
BEGIN
	CREATE TABLE [dbo].[InviteMessage] (
		[Id] [int] IDENTITY(1,1) NOT NULL,
		[ApiId] [int] NOT NULL,
		[Message] [nvarchar](160) NOT NULL,
		CONSTRAINT [PK_InviteMessage] PRIMARY KEY CLUSTERED ([Id] ASC)
	)
	CREATE NONCLUSTERED INDEX [IX_InviteMessage_ApiId] ON [dbo].[InviteMessage]	([ApiId] ASC)

	CREATE TABLE [dbo].[InviteMessagesLog] (
		[Id] [int] IDENTITY(1,1) NOT NULL,
		[SendDateTime] [datetime] NOT NULL,
		[Phone] [char](11) NOT NULL,
		[InviteMessageId] [int] NOT NULL,
		CONSTRAINT [PK_InviteMessagesLog] PRIMARY KEY CLUSTERED ([Id] ASC)
	)
	CREATE NONCLUSTERED INDEX [IX_InviteMessagesLog_SendDateTime] ON [dbo].[InviteMessagesLog] ([SendDateTime] DESC)

	ALTER TABLE [dbo].[InviteMessagesLog]  WITH CHECK ADD  
	CONSTRAINT [FK_InviteMessagesLog_InviteMessage] 
	FOREIGN KEY([InviteMessageId]) REFERENCES [dbo].[InviteMessage] ([Id])

	ALTER TABLE [dbo].[InviteMessagesLog] CHECK CONSTRAINT [FK_InviteMessagesLog_InviteMessage]
END");
        }
    }
}
