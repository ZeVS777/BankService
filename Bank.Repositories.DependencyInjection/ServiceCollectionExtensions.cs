using System;
using System.Data;
using System.Data.SqlClient;
using Dapper;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Bank.Repositories.Abstractions;

namespace Bank.Repositories.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddSmsRepositories(this IServiceCollection services, Action<InviteMessagesRepositoryOptions> options)
        {
            services
                .Configure(options)
                .AddScoped<IDbConnection, SqlConnection>(provider => new SqlConnection(provider.GetRequiredService<IOptions<InviteMessagesRepositoryOptions>>().Value.ConnectionString))
                .AddScoped<IInviteMessagesRepository, InviteMessagesRepository>();

			InitialDbMigration(services);

            return services;
        }

        private static void InitialDbMigration(IServiceCollection services)
        {
            using var sp = services.BuildServiceProvider();
			using var connection = sp.GetRequiredService<IDbConnection>();

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
