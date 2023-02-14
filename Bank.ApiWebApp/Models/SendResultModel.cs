using System.ComponentModel.DataAnnotations;

namespace Bank.ApiWebApp.Models
{
    /// <summary>
    ///     Результат отправки
    /// </summary>
    internal sealed class SendResultModel
    {
        public SendResultModel(int remains, string sentMessage)
        {
            Remains = remains;
            SentMessage = sentMessage;
        }

        /// <summary>
        ///     Количество оставшихся сообщений
        /// </summary>
        public int Remains { get; init; }
        
        /// <summary>
        ///     Сообщение, которое было отправлено
        /// </summary>
        [Required]
        public string SentMessage { get; init; }
    }
}