using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using SovComBankTest.Entities;
using SovComBankTest.Repositories.Abstractions;
using SovComBankTest.Services.Abstractions;
using SovComBankTest.Services.Models;
using SovComBankTest.Utils;

namespace SovComBankTest.Services
{
    public sealed class SmsService: ISmsService
    {
        private static readonly AsyncDuplicateLock Awaiter = new();
        private readonly IInviteMessagesRepository _repository;

        //Конечно же в продакшене должен быть Redis или иной кэш на основе ключ-значение
        private readonly IMemoryCache _memoryCache;

        public SmsService(IInviteMessagesRepository repository, IMemoryCache memoryCache) =>
            (_repository, _memoryCache) = (repository, memoryCache);

        public async Task<SendResult> SendAsync(InviteMessageModel inviteMessage)
        {
            if (inviteMessage.ApiId != 4) return new SendResult(SendStatus.Forbidden, 0);

            if (_memoryCache.TryGetValue(inviteMessage.ApiId, out _))
                return new SendResult(SendStatus.TooMany, 0);
            
            //приняв во внимание единственность ответственности сервиса,
            //решил не использовать транзакции в пользу блокировки всех запросов от конкретного ApiId
            using (await Awaiter.LockAsync(inviteMessage.ApiId))
            {
                //Повторно проверяем. К этому моменту, могло быть отправлено последнее на сегодня разрешённое
                if (_memoryCache.TryGetValue(inviteMessage.ApiId, out _))
                    return new SendResult(SendStatus.TooMany, 0);

                var messagesCount = await _repository.GetMessagesCountAsync(inviteMessage.ApiId, DateTime.Today);

                if (messagesCount >= ISmsService.Threshold)
                {
                    _memoryCache.Set(inviteMessage.ApiId, true, DateTime.Today.AddDays(1).AddTicks(-1));
                    return new SendResult(SendStatus.TooMany, 0);
                }

                var remains = ISmsService.Threshold - messagesCount;

                //Если номеров указано больше, чем количество возможных для отправки сообщений,
                //то не будем отправлять ничего, пусть клиент сам решает, на какие телефоны отправить в приоритете.
                if (inviteMessage.Phones.Length > remains)
                    return new SendResult(SendStatus.TooMany, remains);
                
                await Task.Delay(1000); //TODO: отправка СМС

                // Если будет требование отсылать по одному и записи делать после каждой оправки, то можно и так.
                // Я же принимаю тот факт, что у нас 100% гарантия отправки смс по всем номерам

                var messageId =
                    await _repository.AddMessageAsync(new InviteMessageEntity(inviteMessage.ApiId,
                        inviteMessage.Message));

                var log = new InviteMessagesLogEntity(DateTimeOffset.Now, string.Empty, messageId);
                
                await _repository.AddMessageLogEntryAsync(inviteMessage.Phones.Select(phone => log with { Phone = phone }).ToArray());

                remains -= inviteMessage.Phones.Length;
                if (remains == 0) _memoryCache.Set(inviteMessage.ApiId, true, DateTime.Today.AddDays(1).AddTicks(-1));

                return new SendResult(SendStatus.Ok, remains);
            }
        }
    }
}
