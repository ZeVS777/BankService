using AsyncKeyedLock;
using Bank.Entities;
using Bank.Repositories.Abstractions;
using Bank.Utils;
using Microsoft.Extensions.Caching.Memory;

namespace Bank.Services;

/// <inheritdoc />
public sealed class SmsService : ISmsService
{
    private static readonly AsyncKeyedLocker<int> Awaiter = new(o =>
    {
        o.PoolSize = 20;
        o.PoolInitialFill = 1;
    });

    //Конечно же в продакшене должен быть Redis или иной кэш на основе ключ-значение
    private readonly IMemoryCache _memoryCache;
    private readonly IInviteMessagesRepository _repository;

    /// <summary>
    /// Создать экземпляр класса <see cref="SmsService"/>
    /// </summary>
    /// <param name="repository">Репозиторий работы с сообщениями</param>
    /// <param name="memoryCache">Кэш в памяти</param>
    public SmsService(IInviteMessagesRepository repository, IMemoryCache memoryCache) =>
        (_repository, _memoryCache) = (repository, memoryCache);

    /// <inheritdoc />
    public async Task<SendResult> SendAsync(InviteMessageModel inviteMessage)
    {
        if (inviteMessage.ApiId != 4)
            return new SendResult(SendStatus.Forbidden, 0, null);

        if (_memoryCache.TryGetValue(inviteMessage.ApiId, out _))
            return new SendResult(SendStatus.TooMany, 0, null);

        //приняв во внимание единственность ответственности сервиса,
        //решил не использовать транзакции в пользу блокировки всех запросов от конкретного ApiId
        using (await Awaiter.LockAsync(inviteMessage.ApiId).ConfigureAwait(false))
        {
            //Повторно проверяем. К этому моменту, могло быть отправлено последнее на сегодня разрешённое
            if (_memoryCache.TryGetValue(inviteMessage.ApiId, out _))
                return new SendResult(SendStatus.TooMany, 0, null);

            var messagesCount = await _repository.GetMessagesCountAsync(inviteMessage.ApiId, DateTime.Today);

            if (messagesCount >= ISmsService.Threshold)
            {
                _memoryCache.Set(inviteMessage.ApiId, true, DateTime.Today.AddDays(1).AddTicks(-1));
                return new SendResult(SendStatus.TooMany, 0, null);
            }

            var remains = ISmsService.Threshold - messagesCount;

            //Если номеров указано больше, чем количество возможных для отправки сообщений,
            //то не будем отправлять ничего, пусть клиент сам решает, на какие телефоны отправить в приоритете.
            if (inviteMessage.Phones.Length > remains)
                return new SendResult(SendStatus.TooMany, remains, null);

            var message = Transliteration.CyrillicToLatin(inviteMessage.Message);
            //TODO: отправка СМС

            // Если будет требование отсылать по одному и записи делать после каждой оправки, то можно и так.
            // Я же принимаю тот факт, что у нас 100% гарантия отправки смс по всем номерам

            var messageId =
                await _repository.AddMessageAsync(new InviteMessageEntity(inviteMessage.ApiId,
                    message));

            var log = new InviteMessagesLogEntity(DateTimeOffset.Now, string.Empty, messageId);

            await _repository.AddMessageLogEntryAsync(inviteMessage.Phones.Select(phone => log with { Phone = phone })
                .ToArray());

            remains -= inviteMessage.Phones.Length;
            if (remains == 0)
                _memoryCache.Set(inviteMessage.ApiId, true, DateTime.Today.AddDays(1).AddTicks(-1));

            return new SendResult(SendStatus.Ok, remains, message);
        }
    }
}
