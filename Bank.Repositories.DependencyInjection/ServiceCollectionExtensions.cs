using System.Data.SqlClient;

namespace Bank.Repositories.DependencyInjection;

/// <summary>
/// Расширение методов коллекции сервисов
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Добавить в список сервисов репозиторории работы с сообщениями
    /// </summary>
    /// <param name="services">Список зарегистрированных сервисов</param>
    /// <param name="options">Настройки</param>
    /// <returns>Список зарегистрированных сервисов</returns>
    public static IServiceCollection AddSmsRepositories(this IServiceCollection services,
        Action<InviteMessagesRepositoryOptions> options)
    {
        services
            .Configure(options)
            .AddScoped<IDbConnection, SqlConnection>(provider =>
                new SqlConnection(provider.GetRequiredService<IOptions<InviteMessagesRepositoryOptions>>().Value
                    .ConnectionString))
            .AddScoped<IInviteMessagesRepository, InviteMessagesRepository>();

        InitialDbMigration(services);

        return services;
    }

    private static void InitialDbMigration(IServiceCollection services)
    {
        using var sp = services.BuildServiceProvider();
        using var connection = sp.GetRequiredService<IDbConnection>();

        connection.Execute("""
        IF NOT EXISTS(SELECT * FROM sys.tables WHERE name = 'InviteMessage')
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
        END
        """);
    }
}
